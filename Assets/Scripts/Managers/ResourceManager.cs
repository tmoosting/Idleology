using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

namespace Managers
{
    public class ResourceManager : MonoBehaviour
    {
        public List<Resource> resourceList = new List<Resource>(); 
        
        public void InitializeResources(bool newGame)
        { 
            if (newGame == true)
            {
                GetResource(Resource.Type.Credit)._amount = GetResource(Resource.Type.Credit)._newGame;
                GetResource(Resource.Type.Happiness)._amount = GetResource(Resource.Type.Happiness)._newGame;
                GetResource(Resource.Type.Influence)._amount = GetResource(Resource.Type.Influence)._newGame;
                GetResource(Resource.Type.Force)._amount = GetResource(Resource.Type.Force)._newGame;
            }
            else 
            {
                //TODO: Load from save
            }
        }


        public void GenerateIncome()
        {
            int creditIncome = 0;
            foreach (Generator generator in GetComponent<GeneratorManager>().generatorList)
            {
                if (generator._resource == Resource.Type.Credit)
                {
                    creditIncome += generator._production * generator.GetLevel();
                }
            }
        }



        public void AddIncome(Resource.Type resourceType, int amount)
        {
            GetResource(resourceType)._amount += amount;
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
    }
}
