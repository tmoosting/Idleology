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
       
 
        [HideInInspector]
       public List<Purchaser> purchaserList = new List<Purchaser>();


      


        public void InitializePurchasers(bool newGame)
        {

            foreach (Transform tf in basePurchaserParent.GetComponentsInChildren<Transform>())
                if (tf.GetComponent<Purchaser>() != null)
                    purchaserList.Add(tf.GetComponent<Purchaser>());

            foreach (Transform tf in contentPurchaserParent.GetComponentsInChildren<Transform>())
                if (tf.GetComponent<Purchaser>() != null)
                    purchaserList.Add(tf.GetComponent<Purchaser>());

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
                // unlock lemonade stand for purchasing
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
                purchaser.ValidateUnlock();
        }


        public void UpdateTexts()
        {
            foreach (Purchaser purchaser in purchaserList)
                purchaser.UpdateTexts();
        }
    }
}
