using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace UI
{
    public class PurchaserUI : MonoBehaviour
    {
        [Header("Assigns")] 
        [SerializeField] GameObject basePurchaserParent;
        [SerializeField] GameObject contentPurchaserParent;
        [SerializeField] GeneratorManager generatorManager;
        [SerializeField] ModifierManager modifierManager;
       
 
        List<Purchaser> purchaserList = new List<Purchaser>();


        private void Awake()
        {
            // Collect Purchasers From Parents
            foreach (Transform tf in basePurchaserParent.GetComponentsInChildren<Transform>())
                if (tf.GetComponent<Purchaser>() != null)
                    purchaserList.Add(tf.GetComponent<Purchaser>() );
            
            foreach (Transform tf in contentPurchaserParent.GetComponentsInChildren<Transform>())
                if (tf.GetComponent<Purchaser>() != null)
                    purchaserList.Add(tf.GetComponent<Purchaser>() ); 
     
        
            // ADD: influence, force etc 
        
        
        }


        public void InitializePurchasers(bool newGame)
        {
            // disable all purchasers
            foreach (Purchaser pur in purchaserList)
                pur.HidePurchaser();

            // Set source object for purchasers 
            foreach (Purchaser pur in purchaserList)
            {
                if (pur.GetPurchaserType() == Purchaser.Type.Generator)
                    pur.source =  generatorManager.GetGenerator(pur.gameObject.name);
                else if (pur.GetPurchaserType() == Purchaser.Type.Modifier)
                    pur.source =  modifierManager.GetModifier(pur.gameObject.name);
            }
        
            if (newGame == true)
            {
                // unlock lemonade stand for purchasing
                purchaserList[0].RevealPurchaser();
            }
            else if (newGame == false)
            {
                //TODO: Load from save
            }
        }
     
    

    }
}
