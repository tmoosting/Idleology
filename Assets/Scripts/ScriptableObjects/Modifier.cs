using Interfaces;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Modifier", menuName = "ScriptableObjects/Modifier", order = 3)]
    public class Modifier : ScriptableObject, IOperator
    {
        public enum Type
        {
            InfluenceOne, 
            InfluenceTwo  
        }

   
    
        public Type _type;
        private int _level; 
        [HideInInspector] public IOperator.State _state { get; set; }
        public Resource.Type _resource { get; set; }
        public Resource.Type _costResource { get; set; }
        public int _levelCost { get; set; }
        public int _purchaseCost { get; set; }
        public bool _requiresGenerator { get; set; }
        public Generator.Type _requiredGenerator { get; set; }
        public bool _requiresModifier { get; set; }
        public Type _requiredModifier { get; set; }
        public int _requiredLevel { get; set; }

        
        
        public Type GetModifierType()
        {
            return _type;
        }
        
        public void AddLevel()
        {
            _level++;
        }
        public int GetLevel()
        {
            return _level;
        }
    }
}