using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Generator", menuName = "ScriptableObjects/Generator", order = 2)]
    public class Generator : ScriptableObject
    {
        
     

        public enum Type
        {
            LemonadeStand, 
            PizzaPlace, 
            CarWash, 
            Senate, 
            PoliceStation
        }


        
        public Type _type;
        public Resource.Type _resource;  
        public int _purchaseCost;
        public int _workerBaseCost;
        public int _production;
        private int _workers;
        public bool _requiresGenerator;
        public Type _requiredGenerator;
        public bool _requiresModifier;
        public Modifier.Type _requiredModifier;
        public int _requiredWorkers;


        public int GetWorkers()
        {
            return _workers;
        }



   
    }
}
