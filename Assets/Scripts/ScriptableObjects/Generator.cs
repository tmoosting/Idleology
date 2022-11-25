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
            DepartmentStore,
            ChocolateFactory,
            LumberMill,
            StripMine,
            OilConglomerate,
            LobbyingFirm, 
            PoliceStation,
            Laboratory
        }


        
      

        public Type _type; 
        [HideInInspector] public IOperator.State _state { get; set; }
        
        public ulong _level { get; set; }
        public ulong _production  { get; set; }
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
        public string _purchaseTooltip { get; set; }
        public string _firstWorkerTooltip { get; set; }
        public string _completedTooltip { get; set; }

        public void AddLevel()
        {
            _level++;
            if (_resource == Resource.Type.Credit)
                VisualsManager.Instance.SpawnChangeVisual(ChangeVisual.Type.CreditIncome, _production );
            else  if (_resource == Resource.Type.Influence)
                VisualsManager.Instance.SpawnChangeVisual(ChangeVisual.Type.InfluenceIncome, _production );
            else  if (_resource == Resource.Type.Force)
                VisualsManager.Instance.SpawnChangeVisual(ChangeVisual.Type.ForceIncome, _production );
            
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
