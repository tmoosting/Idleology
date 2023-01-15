using ScriptableObjects;
using UnityEngine;

namespace Interfaces
{
    public interface IOperator  
    {
         
        public enum State
        {
            Hidden, // nothing visible
            Visible, // name visible, buybutton is not
            Buyable, // name and buybutton visible
            Owned, // Purchased but with 0 workers
            Operated, // purchased with >= 1 worker 
            Completed // max level reached
        }
        
        public State _state { get; set; }
        public Resource.Type _resource { get; set; }
        public Resource.Type _costResource { get; set; }
        public ulong _levelCost { get; set; }
        public float _costMultiplier { get; set; }
        public ulong _level { get; set; }
        public ulong _purchaseCost{ get; set; }
        public bool _requiresGenerator{ get; set; }
        public Generator.Type _requiredGeneratorType{ get; set; }
        public bool _requiresModifier{ get; set; }
        public Modifier.Type _requiredModifier{ get; set; }
        public ulong _requiredLevel{ get; set; }
        public bool isGenerator { get; set; }
        public string _purchaseTooltip { get; set; }
        public string _firstWorkerTooltip { get; set; }
        public string _completedTooltip { get; set; }

        public void AddLevel();
        public void SetLevel(ulong level);
        public ulong GetLevel();
    }
}
