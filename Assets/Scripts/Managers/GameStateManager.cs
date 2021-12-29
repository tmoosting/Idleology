using System;
using UnityEngine;

namespace Managers
{
    public class GameStateManager : MonoBehaviour
    {

        [Header("Assigns")] 
        public GameObject UIManager;
        
        [Header("Settings")]
        public bool newGame;

        private void Start()
        {
           StartGame(); 
        }

     

        private void StartGame()
        {
            GetComponent<DataManager>().LoadDataObjects(); // if (loadDatabase == true)
            GetComponent<GeneratorManager>().InitializeGenerators(newGame);
            UIManager.GetComponent<PurchaserUI>().InitializePurchasers(newGame);
            GetComponent<ModifierManager>().InitializeModifiers(newGame);
            UIManager.GetComponent<ResourceUI>().InitializeResources(newGame);
            UIManager.GetComponent<ContentUI>().InitializeContentSections(newGame);
        }
      
        
        
        
        
        
    }
}
