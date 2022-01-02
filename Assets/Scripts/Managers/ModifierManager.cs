using System;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

namespace Managers
{
    public class ModifierManager : MonoBehaviour
    {
        
        public List<Modifier> modifierList = new List<Modifier>();


        

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

        public void ScanUnlockables()
        {
            throw new System.NotImplementedException();
        }
    }
}
