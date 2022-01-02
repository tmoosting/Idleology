using Interfaces;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Modifier", menuName = "ScriptableObjects/Modifier", order = 3)]
    public class Modifier : ScriptableObject, IOperator
    {
        public enum Type
        {
            LongerHours, // more income, decrease happiness
            LessPay, // decrease worker hire cost, decrease happiness
            NoSafetyRegulations // more income, decrease worker hire cost, decrease happiness
        }

   
    
        public Type _type;
        public int _level { get; set; }
        public IOperator.State _state { get; set; }
        public Resource.Type _resource { get; set; }
        public Resource.Type _costResource { get; set; }
        public int _levelCost { get; set; }
        public int _purchaseCost { get; set; }
        public bool _requiresGenerator { get; set; }
        public Generator.Type _requiredGenerator { get; set; }
        public bool _requiresModifier { get; set; }
        public Type _requiredModifier { get; set; }
        public int _requiredLevel { get; set; }
        public bool isGenerator { get; set; }

        [HideInInspector] public float _creditPercentage = 0f;
        [HideInInspector] public int _happinessCost = 0;
        [HideInInspector] public float _levelPricePercentage = 0;


        public Type GetModifierType()
        {
            return _type;
        }
        
        public void AddLevel()
        {
            _level++;
        }
        public void SetLevel(int level)
        {
            _level = level;
        }
        public int GetLevel()
        {
            return _level;
        }
    }
}