using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NarrativeEvent", menuName = "ScriptableObjects/NarrativeEvent", order = 4)]
    public class NarrativeEvent : ScriptableObject
    {
        public string ID;
        public string eventName;
        public string eventText;
        public Dictionary<Generator.Type, ulong> generatorTriggermap;
        public Dictionary<Modifier.Type, ulong> modifierTriggermap;
        public Dictionary<Resource.Type, ulong> resourceTriggermap;


        [HideInInspector] public bool hasBeenTriggered = false;


        /*public NarrativeEvent(string givenID, string givenText)
        {
            ID = givenID;
            eventName = "ManualPop" + ID;
            eventText = givenText;
            generatorTriggermap = new Dictionary<Generator.Type, int>();
            modifierTriggermap = new Dictionary<Modifier.Type, int>();
            resourceTriggermap = new Dictionary<Resource.Type, int>();
        }*/
    }
}
