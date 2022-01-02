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
        public int _purchaseCost;
        public int _levelCost;
        public bool _requiresGenerator;
        public Generator.Type _requiredGenerator;
        public bool _requiresModifier;
        public Modifier.Type _requiredModifier;
        public int _requiredLevel;
        public float _creditPercentage = 0f;
        public int _happinessCost = 0;
        public float _levelPricePercentage = 0;

        public Modifier.Type GetModifierType()
        {
            return _type;
        }

    }
}