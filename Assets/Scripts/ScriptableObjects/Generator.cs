using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Generator", menuName = "ScriptableObjects/Generator", order = 2)]
    public class Generator : ScriptableObject, IOperator
    {
        
     

        public enum Type
        {
            LemonadeStand, 
            PizzaPlace, 
            CarWash,           
            Senate, 
            PoliceStation,
            CandyFactory
        }


        
      

        public Type _type; 
        [HideInInspector] public IOperator.State _state { get; set; }
        
        public int _level { get; set; }
        public int _production;
        public Resource.Type _resource { get; set; }
        public Resource.Type _costResource { get; set; }
        public int _levelCost { get; set; }
        public int _purchaseCost { get; set; }
        public bool _requiresGenerator { get; set; }
        public Type _requiredGenerator { get; set; }
        public bool _requiresModifier { get; set; }
        public Modifier.Type _requiredModifier { get; set; }
        public int _requiredLevel { get; set; }
        public bool isGenerator { get; set; }

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
