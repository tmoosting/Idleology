using TMPro;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "GeneratorData", menuName = "ScriptableObjects/GeneratorData", order = 1)]
    public class GeneratorData : ScriptableObject
    {
        // Stores data for a google sheet generator row
        
        public Generator.Type _type;
        public Resource.Type _resource;
        public int _purchaseCost;
        public int _workerBaseCost;
        public int _production;
        public bool _requiresGenerator;
        public Generator.Type _requiredGenerator;
        public bool _requiresModifier;
        public Modifier.Type _requiredModifier;
        public int _requiredWorkers;

        public Generator.Type GetGeneratorType()
        {
            return _type;
        }

    }
}
