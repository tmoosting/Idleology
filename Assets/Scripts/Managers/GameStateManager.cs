using System;
using UI;
using UnityEngine;

namespace Managers
{
    public class GameStateManager : MonoBehaviour
    {
        // Starts Game, Creates GameTicks, Tracks Prestige Level
        
        [Header("Assigns")] 
        public GameObject UIManager;
        
        [Header("Settings")]
        public bool newGame;

        private float tickTimer = 0; 
  
        
        private void Start()
        {
           StartGame(); 
        }

        private void FixedUpdate()
        {
            tickTimer += Time.deltaTime;
            if (tickTimer > 1.0f)
            {
                tickTimer = 0f;
                GameTick();
            }
        }
  
   

        private void StartGame()
        {
            GetComponent<DataManager>().LoadDataObjects(); // if (loadDatabase == true)
            GetComponent<GeneratorManager>().InitializeGenerators(newGame);
            UIManager.GetComponent<PurchaserUI>().InitializePurchasers(newGame);
            GetComponent<ModifierManager>().InitializeModifiers(newGame);
        //    GetComponent<ResourceManager>().InitializeResources(newGame);
            UIManager.GetComponent<ContentUI>().InitializeContentSections(newGame);
        }


        private void GameTick()
        {

            // generate income
            // update resource texts
            // check for unlocks
                    // new tabs
                    // buy buttons
                    // new purchasers
                        // for generators
                        // for modifiers
                   
            
        }
        
        
        
    }
}
