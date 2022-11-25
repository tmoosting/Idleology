using TMPro;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "ModifierData", menuName = "ScriptableObjects/ModifierData", order = 1)]
    public class ModifierData : ScriptableObject
    {
        // Stores data for a google sheet generator row
        
        public Modifier.Type _type;
        public Resource.Type _costResource;
        public ulong _purchaseCost;
        public ulong _levelCost;
        public bool _requiresGenerator;
        public Generator.Type _requiredGenerator;
        public bool _requiresModifier;
        public Modifier.Type _requiredModifier;
        public ulong _requiredLevel;
        public float _creditPercentage = 0f;
        public ulong _happinessCost = 0;
        public float _levelPricePercentage = 0;
        public string _purchaseTooltipText;
        public string _firstWorkerTooltipText;
        public string _completedTooltipText;

        public Modifier.Type GetModifierType()
        {
            return _type;
        }

    }
}