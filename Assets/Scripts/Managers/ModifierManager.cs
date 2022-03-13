using System;
using System.Collections.Generic;
using Interfaces;
using ScriptableObjects;
using UnityEngine;

namespace Managers
{
    public class ModifierManager : MonoBehaviour
    {

        public static ModifierManager Instance;
        
        public List<Modifier> modifierList = new List<Modifier>();


        private void Awake()
        {
            Instance = this;
        }

        public void InitializeModifiers(bool newGame)
        { 
            if (newGame == true)
            {
                foreach (Modifier modifier in modifierList)
                {
                    modifier.SetLevel(0);
                    modifier.isGenerator = false;
                }
            }
            else 
            {
                //TODO: Load from save
            }
        }
  

        public void ScanUnlockables()
        {
            throw new System.NotImplementedException();
        }

        public void HandleStaticModifierEffects(IOperator iMod)
        {
            Modifier modifier = (Modifier) iMod;
            HandleHappinessCost(modifier);
        }

        void HandleHappinessCost(Modifier modifier)
        {
            if (modifier._happinessCost != 0)
            {
                GetComponent<ResourceManager>().PayResource(Resource.Type.Happiness, modifier._happinessCost );
            }
        }

        public int GetDiscountLevellingPrice(float rawCost)
        {
            float  modifiedCost = rawCost;
            
            foreach (Modifier mod in modifierList)
                if (mod._levelPricePercentage != 0)
                {
                    float multiplier = (1 + (mod._levelPricePercentage * mod.GetLevel())); 
                    modifiedCost *= multiplier;
                } 

            return (int)modifiedCost;
        }
        
        public List<Modifier> GetActiveModifiers()
        {
            List<Modifier> returnList = new List<Modifier>();

            foreach (Modifier modifier in modifierList)
            {
                if (modifier._state == Interfaces.IOperator.State.Owned)
                    returnList.Add(modifier);
            }
            return returnList;
        }

        public Modifier GetModifier(Modifier.Type type)
        {
            foreach (Modifier mod in modifierList)
                if (mod.GetModifierType() == type)
                    return mod;
            Debug.LogWarning(("Did not find Modifier for type: " + type.ToString()+"..!"));
            return null;
        }

        public Modifier GetModifier(string type)
        {
            foreach (Modifier gen in modifierList)
                if (gen.GetModifierType().ToString()  == type)
                    return gen;
            //   Debug.LogWarning(("Did not find Modifier for type: " + type +"..!"));
            return null;
        }
    }
}
