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
           
            }
            else 
            {
                //TODO: Load from save
            }
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
