using System.Collections.Generic;
using ScriptableObjects;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class ResourceManager : MonoBehaviour
    {
        [Header("Assigns")] 
        [SerializeField] TextMeshProUGUI creditText;
        [SerializeField] TextMeshProUGUI creditIncomeText;
        [SerializeField] TextMeshProUGUI happinessText;
        
        public List<Resource> resourceList = new List<Resource>(); 
        
        public void InitializeResources(bool newGame)
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
      
            }
            else 
            {
                //TODO: Load from save
            }
        }


        public void GenerateIncome()
        {
            GenerateCreditIncome(CalculateIncome(Resource.Type.Credit));
            GenerateInfluenceIncome(CalculateIncome(Resource.Type.Influence));
            GenerateForceIncome(CalculateIncome(Resource.Type.Force));
        }

        

        public int CalculateIncome(Resource.Type resourceType)
        {
            int income = 0;
            foreach (Generator generator in GetComponent<GeneratorManager>().generatorList)
                 if (generator._resource == resourceType)
                    income += generator._production * generator.GetLevel();
            return income;
        }
        private void GenerateCreditIncome(int amount)
        { 
            AddIncome(Resource.Type.Credit,  amount);
        }

        private void GenerateInfluenceIncome(int amount)
        { 
            AddIncome(Resource.Type.Influence,  amount);
        }
        
        private void GenerateForceIncome(int amount)
        { 
            AddIncome(Resource.Type.Force,  amount);
        }

        public void AddIncome(Resource.Type resourceType, int amount)
        {
            GetResource(resourceType)._amount += amount;
        }
        public void PayResource(Resource.Type resourceType, int amount)
        {
            GetResource(resourceType)._amount -= amount;
        }


        public bool RequirementsMet(Resource.Type resourcetype, int amount)
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
