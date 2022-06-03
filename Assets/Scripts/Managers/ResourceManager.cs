using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
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
        
        public List<Resource> resourceList = new List<Resource>();

       private bool countToStartMoney = false;
        
        private void Awake()
        {
            Instance = this;
        }

        private float initialDelay = 0.3f; // in seconds
        private int delayRate = 1;
        private ulong increaseAmount = 100;
        private ulong finalIncreaseAmount = 5;
        private int rateCounter = 0;
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
                }
                else
                {
                    GetResource(Resource.Type.Credit)._amount = GetResource(Resource.Type.Credit)._newGame;
                    GetResource(Resource.Type.Happiness)._amount = GetResource(Resource.Type.Happiness)._newGame;
                    GetResource(Resource.Type.Influence)._amount = GetResource(Resource.Type.Influence)._newGame;
                    GetResource(Resource.Type.Force)._amount = GetResource(Resource.Type.Force)._newGame;
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
            //Alternative: Have income as its own data type, persistent, with purchases immediately changing it
            GenerateCreditIncome(CalculateIncome(Resource.Type.Credit));
            GenerateInfluenceIncome(CalculateIncome(Resource.Type.Influence));
            GenerateForceIncome(CalculateIncome(Resource.Type.Force));
        }

        

        public ulong CalculateIncome(Resource.Type resourceType)
        {
            ulong rawIncome = 0;
            foreach (Generator generator in GetComponent<GeneratorManager>().generatorList)
                 if (generator._resource == resourceType)                
                     rawIncome += generator._production * (ulong)generator.GetLevel();

            float modifiedIncome = rawIncome;

            if (resourceType == Resource.Type.Credit)
                modifiedIncome = ModifyCreditIncome(rawIncome);
            
         
            return (ulong)modifiedIncome;
        }

        private ulong ModifyCreditIncome(ulong rawAmount)
        {
            float modifiedAmount = rawAmount;
            foreach (Modifier modifier in GetComponent<ModifierManager>().GetActiveModifiers())
            {
                if (modifier._creditPercentage != 0)
                {
                    float multiplier = (1 + (modifier._creditPercentage * modifier.GetLevel())); 
                    modifiedAmount *= multiplier;
                }
            }
            
            

            return (ulong)modifiedAmount;
        }
        private void GenerateCreditIncome(ulong amount)
        { 
            AddIncome(Resource.Type.Credit, (ulong)amount);
        }

        private void GenerateInfluenceIncome(ulong amount)
        { 
            AddIncome(Resource.Type.Influence,  amount);
        }
        
        private void GenerateForceIncome(ulong amount)
        { 
            AddIncome(Resource.Type.Force,  amount);
        }

        public void AddIncome(Resource.Type resourceType, ulong amount)
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
            creditText.text = GetResource(Resource.Type.Credit)._amount.ToString();
            creditIncomeText.text = "+ "+ CalculateIncome(Resource.Type.Credit).ToString();
            happinessText.text = GetResource(Resource.Type.Happiness)._amount.ToString();
        }

     
    }
}
