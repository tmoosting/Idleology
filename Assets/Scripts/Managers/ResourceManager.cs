using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Xml.Serialization;
using ScriptableObjects;
using TMPro;
using UI;
using UnityEngine;

namespace Managers
{
    public class ResourceManager : MonoBehaviour
    {
        public static ResourceManager Instance;


        [Header("Assigns")] 
        [SerializeField] TextMeshProUGUI creditText;
        [SerializeField] TextMeshProUGUI creditIncomeText;
        [SerializeField] TextMeshProUGUI happinessText;
        public bool formatInScientific;
        public ulong formatThreshold;

        public List<Resource> resourceList = new List<Resource>();

       private bool countToStartMoney = false;


       private ModifierManager _modifierManager;
       
        private void Awake()
        {
            Instance = this;
        }

        private float initialDelay = 0.3f; // in seconds
        private int delayRate = 1;
        private ulong increaseAmount = 100;
        private ulong finalIncreaseAmount = 5;
        private int rateCounter = 0;
        private ulong _previousIncomeCredit;
        private ulong _previousIncomeInfluence;
        private ulong _previousIncomeForce;
        private ulong _previousIncomeTech;
        
        private void FixedUpdate()
        {
            if (countToStartMoney == true)
            {
                rateCounter++;
                if (rateCounter >= delayRate)
                {
                    rateCounter = 0;
                    if (GetResource(Resource.Type.Credit)._amount+increaseAmount > GetResource(Resource.Type.Credit)._newGame)
                        GetResource(Resource.Type.Credit)._amount+=finalIncreaseAmount;
                    else
                        GetResource(Resource.Type.Credit)._amount+=increaseAmount;
                    UpdateTexts();
                    if (GetResource(Resource.Type.Credit)._amount >= GetResource(Resource.Type.Credit)._newGame)
                    {
                        countToStartMoney = false;
                        GetComponent<GameStateManager>().UIManager.GetComponent<NarrativeUI>().FinishMoneyPiling();                     
                    }
                }
            }
        }

        public void InitializeResources(bool newGame, bool skipIntro)
        { 
            if (newGame == true)
            {
                if (GetComponent<DevManager>().enableDevMode == true)
                {
                    GetResource(Resource.Type.Credit)._amount = GetResource(Resource.Type.Credit)._devGame;
                    GetResource(Resource.Type.Happiness)._amount = GetResource(Resource.Type.Happiness)._devGame;
                    GetResource(Resource.Type.Influence)._amount = GetResource(Resource.Type.Influence)._devGame;
                    GetResource(Resource.Type.Force)._amount = GetResource(Resource.Type.Force)._devGame;
                    GetResource(Resource.Type.Tech)._amount = GetResource(Resource.Type.Tech)._devGame;
                }
                else
                {
                    GetResource(Resource.Type.Credit)._amount = GetResource(Resource.Type.Credit)._newGame;
                    GetResource(Resource.Type.Happiness)._amount = GetResource(Resource.Type.Happiness)._newGame;
                    GetResource(Resource.Type.Influence)._amount = GetResource(Resource.Type.Influence)._newGame;
                    GetResource(Resource.Type.Force)._amount = GetResource(Resource.Type.Force)._newGame;
                    GetResource(Resource.Type.Tech)._amount = GetResource(Resource.Type.Tech)._newGame;
                }

                if (skipIntro == false)
                {
                    GetResource(Resource.Type.Credit)._amount = 0;
                    StartCoroutine(WaitAMomentThenStartPiling());
                }
            }
            else 
            {
                //TODO: Load from save
            }
        }

        private IEnumerator WaitAMomentThenStartPiling()
        {
            yield return new WaitForSeconds(initialDelay);
            countToStartMoney = true;
        }

        public void GenerateIncome()
        {
            if (_modifierManager == null)
                _modifierManager = GetComponent<ModifierManager>();
            //Alternative: Have income as its own data type, persistent, with purchases immediately changing it
            GenerateCreditIncome(CalculateIncome(Resource.Type.Credit));
            GenerateInfluenceIncome(CalculateIncome(Resource.Type.Influence));
            GenerateForceIncome(CalculateIncome(Resource.Type.Force));
            GenerateTechIncome(CalculateIncome(Resource.Type.Tech));
        }
 


        public ulong CalculateIncome(Resource.Type resourceType)
        {
            if (_modifierManager == null)
                _modifierManager = GetComponent<ModifierManager>();
            ulong rawIncome = 0;
            foreach (Generator generator in GetComponent<GeneratorManager>().generatorList)
                 if (generator._resource == resourceType)                
                     rawIncome += generator._production * (ulong)generator.GetLevel();

            float modifiedIncome = rawIncome;

            if (resourceType == Resource.Type.Credit)
                modifiedIncome = ModifyCreditIncome(rawIncome);

            modifiedIncome = InnovateIncome(resourceType, modifiedIncome);
            return (ulong)modifiedIncome;
        }

        private float InnovateIncome(Resource.Type resourceType, float originalIncome)
        {
            List<Innovation> applicableInnovations = new List<Innovation>();
            foreach (var innovation in InnovationManager.GetAllInnovations())
                if (innovation.affectParameter == Innovation.Paramater.Income)
                    if (resourceType ==  GeneratorManager.GetGeneratedResourceForGenerator(innovation.affectGeneratorType))
                            applicableInnovations.Add(innovation);
            if (applicableInnovations.Count == 0)
                return originalIncome;
            else // has relevant Innovations
            {
                foreach (var innovation in applicableInnovations)
                {
                    float effectAmount = 0;
                    if (innovation.originParamater == Innovation.Paramater.Income)
                    {
                        Resource.Type checkedResource =  GeneratorManager.GetGeneratedResourceForGenerator(innovation.affectGeneratorType);
                        effectAmount = GetResource(checkedResource)._amount;
                        effectAmount *= innovation.originMultiplier;
                    }

                }
                
            }

            return originalIncome;
        }

        private ulong ModifyCreditIncome(ulong rawAmount)
        {
            float modifiedAmount = rawAmount;
            foreach (Modifier mod in _modifierManager.GetActiveModifiers())
            {
                if (mod._creditPercentage != 0)
                {
                    float multiplier = 1 + mod._creditPercentage; 
                    for (ulong i = 0; i < mod.GetLevel(); i++)
                        modifiedAmount *= multiplier;   
                } 
            } 
            return (ulong)modifiedAmount;
        }
        private void GenerateCreditIncome(ulong amount)
        {
            _previousIncomeCredit = amount;
            AddResource(Resource.Type.Credit, (ulong)amount);
        }

        private void GenerateInfluenceIncome(ulong amount)
        { 
            _previousIncomeInfluence = amount; 
            AddResource(Resource.Type.Influence,  amount);
        }
        
        private void GenerateForceIncome(ulong amount)
        { 
            _previousIncomeInfluence = amount;
            AddResource(Resource.Type.Force,  amount);
        }  
        private void GenerateTechIncome(ulong amount)
        { 
            _previousIncomeTech = amount;
            AddResource(Resource.Type.Tech,  amount);
        }

        private void AddResource(Resource.Type resourceType, ulong amount)
        {
            GetResource(resourceType)._amount += amount;
        }
        public void PayResource(Resource.Type resourceType, ulong amount)
        {
            GetResource(resourceType)._amount -= amount;
        }


        public bool RequirementsMet(Resource.Type resourcetype, ulong amount)
        {
            bool returnBool = false;
            foreach (Resource resource in resourceList)
                if (resource._type == resourcetype)
                    if (resource._amount >= amount)
                        returnBool = true;
            return returnBool;
        }

        public Resource GetResource(Resource.Type type)
        {
            foreach (Resource resource in resourceList)
                if (resource._type == type)
                    return resource;
            Debug.LogWarning("Did not find resource for type: " + type + "..!");
            return null;
        }
        public Resource GetResource(string type)
        {
            foreach (Resource resource in resourceList)
                if (resource._type.ToString() == type)
                    return resource;
            Debug.LogWarning("Did not find resource for type: " + type + "..!");
            return null;
        }

      
        public void UpdateTexts()
        {
            creditText.text = FormatNumber(  GetResource(Resource.Type.Credit)._amount);
            creditIncomeText.text = "+ "+ FormatNumber( CalculateIncome(Resource.Type.Credit));
            happinessText.text = FormatNumber(GetResource(Resource.Type.Happiness)._amount);
        }

        
        private int charA = Convert.ToInt32('a');
        private Dictionary<int, string> units = new Dictionary<int, string>
        {
            {0, ""},
            {1, "K"},
            {2, "M"},
            {3, "B"},
            {4, "T"},
            {5, "KT"},
            {6, "MT"},
            {7, "BT"}
        };
        public string FormatNumber(ulong amount)
        {
            if (amount < formatThreshold)
            {
                return amount.ToString();
            }
            else
            {
                if (formatInScientific)
                    return amount.ToString("0.00E+0");
                var n = (int) Math.Log(amount, 1000);
                var m = amount / Math.Pow(1000, n);
                var unit = "";

                if (n < units.Count)
                {
                    unit = units[n];
                }
                else
                {
                    var unitInt = n - units.Count;
                    var secondUnit = unitInt % 26;
                    var firstUnit = unitInt / 26;
                    unit = Convert.ToChar(firstUnit + charA).ToString() + Convert.ToChar(secondUnit + charA).ToString();
                } 
                return (Math.Floor(m * 100) / 100).ToString("0.##") + unit;
            }
        } 
        private GeneratorManager _generatorManager;
        private GeneratorManager GeneratorManager  
        {
            get
            {
                if (_generatorManager == null)
                    _generatorManager = FindObjectOfType<GeneratorManager>();
                return _generatorManager;
            }
        }
        private InnovationManager _innovationManager;
        private InnovationManager InnovationManager  
        {
            get
            {
                if (_innovationManager == null)
                    _innovationManager = FindObjectOfType<InnovationManager>();
                return _innovationManager;
            }
        }
     
    }
    
    
}
