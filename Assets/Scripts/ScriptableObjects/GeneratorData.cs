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
        public Resource.Type _costResource;
        public ulong _purchaseCost;
        public ulong _levelCost;
        public float _costMultiplier;
        public ulong _production;
        public bool _requiresGenerator;
        public Generator.Type _requiredGenerator;
        public bool _requiresModifier;
        public Modifier.Type _requiredModifier;
        public ulong _requiredLevel;
        public string _purchasteTooltipText;
        public string _firstWorkerTooltipText;

        public Generator.Type GetGeneratorType()
        {
            return _type;
        }

    }
}
