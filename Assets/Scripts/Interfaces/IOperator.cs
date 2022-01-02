using ScriptableObjects;
using UnityEngine;

namespace Interfaces
{
    public interface IOperator  
    {
         
        public enum State
        {
            Hidden,
            Visible,
            Owned
        }
        
        [HideInInspector] public State _state { get; set; }
        public Resource.Type _resource { get; set; }
        public Resource.Type _costResource { get; set; }
        public int _levelCost { get; set; }
        public int _purchaseCost{ get; set; }
        public bool _requiresGenerator{ get; set; }
        public Generator.Type _requiredGenerator{ get; set; }
        public bool _requiresModifier{ get; set; }
        public Modifier.Type _requiredModifier{ get; set; }
        public int _requiredLevel{ get; set; }


        public void AddLevel();
        public int GetLevel();
    }
}
