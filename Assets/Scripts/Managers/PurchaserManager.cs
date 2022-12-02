using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Managers
{
    public class PurchaserManager : MonoBehaviour
    {
    
        [Header("Assigns")] 
        [SerializeField] GameObject basePurchaserParent;
        [SerializeField] GameObject contentPurchaserParent; 
       
 
        [HideInInspector] public List<Purchaser> purchaserList = new List<Purchaser>();

        private NarrativeUI _narrativeUI;
      


        public void InitializePurchasers(bool newGame, bool skipIntro)
        {
            _narrativeUI = FindObjectOfType<NarrativeUI>();
            foreach (Purchaser pur in basePurchaserParent.GetComponentsInChildren<Purchaser>(true))
                if (pur != null)
                {
                    pur.Initialize();
                    purchaserList.Add(pur.GetComponent<Purchaser>()); 
                }

            foreach (Purchaser pur in contentPurchaserParent.GetComponentsInChildren<Purchaser>(true))
                if (pur != null) 
                {
                    pur.Initialize();
                    purchaserList.Add(pur.GetComponent<Purchaser>()); 
                }

            // Set source object for purchasers 
            foreach (Purchaser pur in purchaserList)
            {
                if (GetComponent<GeneratorManager>().GetGenerator(pur.gameObject.name) != null)
                {
                    pur.source =  GetComponent<GeneratorManager>().GetGenerator(pur.gameObject.name);
                //    pur.isGenerator = true;
                }
                else  if (GetComponent<ModifierManager>().GetModifier(pur.gameObject.name) != null)
                    pur.source =  GetComponent<ModifierManager>().GetModifier(pur.gameObject.name);
                else 
                    Debug.LogWarning("Did not find a source for Purchaser: " + pur.gameObject.name + "..!");
            }
            
            // hide all purchasers
            foreach (Purchaser pur in purchaserList)
                pur.HidePurchaser();
        
            if (newGame == true)
            {
                 if (skipIntro == true)
                        purchaserList[0].Unlock();
            }
            else if (newGame == false)
            {
                //TODO: Load from save
            }
        } 
        
        
        public void ScanUnlockables()
        {
            foreach (Purchaser purchaser in purchaserList)
            {
                if (GameStateManager.Instance.skipIntro == false)
                {
                    if (_narrativeUI.finishedIntroEvents == false)
                    {
                        // don't unlock while piling start money
                    }
                    else 
                        purchaser.ValidateLockState();
                }
                else
                    purchaser.ValidateLockState();
            }
                
                     
        }


        public void UpdateAllTexts()
        {
            foreach (Purchaser purchaser in purchaserList)
                purchaser.UpdateTexts();
        }
    }
}
