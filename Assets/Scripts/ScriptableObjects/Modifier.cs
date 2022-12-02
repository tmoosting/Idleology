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
            NoSafetyRegulations, // more income, decrease worker hire cost, decrease happiness
            UnionBusters,
            RiotControl,
            Curfews,
            SmartPhones,
            AutonomousCars,
            LoveBots
        }

   
    
        public Type _type;
        public ulong _level { get; set; }
        public IOperator.State _state { get; set; }
        public Resource.Type _resource { get; set; }
        public Resource.Type _costResource { get; set; }
        public ulong _levelCost { get; set; }
        public ulong _purchaseCost { get; set; }
        public bool _requiresGenerator { get; set; }
        public Generator.Type _requiredGenerator { get; set; }
        public bool _requiresModifier { get; set; }
        public Type _requiredModifier { get; set; }
        public ulong _requiredLevel { get; set; }
        public bool isGenerator { get; set; }
        public string _purchaseTooltip { get; set; }
        public string _firstWorkerTooltip { get; set; }
        public string _completedTooltip { get; set; }

        [HideInInspector] public float _creditPercentage = 0f;
        [HideInInspector] public ulong _happinessCost = 0;
        [HideInInspector] public float _levelPricePercentage = 0;


        public Type GetModifierType()
        {
            return _type;
        }
        
        public void AddLevel()
        {
            _level++;
     
        }
        public void SetLevel(ulong level)
        {
            _level = level;
        }
        public ulong GetLevel()
        {
            return _level;
        }
    }
}