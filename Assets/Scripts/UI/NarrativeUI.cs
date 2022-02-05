using System.Collections.Generic;
using Managers;
using ScriptableObjects;
using UnityEngine;

namespace UI
{
     //TODO:
     // Import NarrPops from sheet and store+maintain in NarrativeUI
     // Create a callback system ingame ticks and user actions, that includes checking for triggers on all available NarrPops
     // Create NarrPop placement system

    public struct NarrativePop
    {
        private string ID;
        private string popName;
        private string popText;
        private Dictionary<Generator, int> generatorTriggers;
        private Dictionary<ModifierManager, int> modifierTriggers;
        private Dictionary<Resource.Type, int> resourcetiggers;

        public NarrativePop(string givenID, string givenText)
        {
            ID = givenID;
            popName = "ManualPop" + ID;
            popText = givenText;
            generatorTriggers = new Dictionary<Generator, int>();
            modifierTriggers = new Dictionary<ModifierManager, int>();
            resourcetiggers = new Dictionary<Resource.Type, int>();
        }
    } 
    
    
    public class NarrativeUI : MonoBehaviour
    {
        
        // TODO: logic for start-popup, which is not loaded through IdleDB Sheet
        private NarrativePop startPop = new NarrativePop("NAR00", 
            "Most young entrepreneurs need a lil’ something " +
            "to get them on the right track. The track to profit that is. " +
            "Luckily for some of them, they start off with a good amount " +
            "of investment capital courtesy of their parental figures. " +
            "You know what they say: 'it takes money to make money'.");
        
        

    }
}
