using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class UnlockManager : MonoBehaviour
    {
        // Create Unlockables List at game start and whenever an unlock is made
        // Check the List after every game tick and every purchase
        // so that not all objects have to be scanned every tick

        public List<IUnlockable> unlockablesList = new List<IUnlockable>();
        

        public void CreateUnlockablesList()
        {

            foreach (Purchaser purchaser in GetComponent<PurchaserManager>().purchaserList)
            {
                
            }
            // Generate a list of 'unlockables' by scanning through all purchasers
                 // state: hidden
                 // buybutton: disabled

        }

 
        
        
        
        
                // Triggers
        // game tick - check:
            // 
        
                //  Tasks / types
      // unlock tabs
      
      // 
      public void UpdateUnlockables()
      {
          // Called on every tick, and every purchase
          // Checks for open Unlockables
          
      }
    }
}
