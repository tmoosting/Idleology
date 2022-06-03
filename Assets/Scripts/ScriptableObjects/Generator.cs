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
            LobbyingFirm, 
            PoliceStation,
            DepartmentStore,
            CandyFactory,
            LumberMill,
            StripMine,
            OilConglomerate
        }


        
      

        public Type _type; 
        [HideInInspector] public IOperator.State _state { get; set; }
        
        public ulong _level { get; set; }
        public ulong _production;
        public Resource.Type _resource { get; set; }
        public Resource.Type _costResource { get; set; }
        public ulong _levelCost { get; set; }
        public ulong _purchaseCost { get; set; }
        public bool _requiresGenerator { get; set; }
        public Type _requiredGenerator { get; set; }
        public bool _requiresModifier { get; set; }
        public Modifier.Type _requiredModifier { get; set; }
        public ulong _requiredLevel { get; set; }
        public bool isGenerator { get; set; }
        public string _tooltipText { get; set; }

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
