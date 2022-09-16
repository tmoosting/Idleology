using System;
using ScriptableObjects;
using UI;
using UnityEngine;

namespace Managers
{
    public class GameStateManager : MonoBehaviour
    {
        // Starts Game, Creates GameTicks, Tracks Prestige Level

        public static GameStateManager Instance;
        [Header("Assigns")] 
        public GameObject UIManager;
        
        [Header("Settings")]
        public bool newGame;
        public bool skipIntro;

        private float tickTimer = 0;
        [HideInInspector]
        public float timePassed = 0;


        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
           StartGame(); 
        }

        private void FixedUpdate()
        {
            tickTimer += Time.deltaTime;
            timePassed += Time.deltaTime;
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
            GetComponent<PurchaserManager>().InitializePurchasers(newGame, skipIntro);
            GetComponent<ModifierManager>().InitializeModifiers(newGame);
            GetComponent<ResourceManager>().InitializeResources(newGame, skipIntro);
            UIManager.GetComponent<ContentUI>().InitializeContentSections(newGame);
            UIManager.GetComponent<NarrativeUI>().InitializeNarratives(newGame, skipIntro);
            ScanUnlockables();
            GameTick();
        }


        private void GameTick()
        {
            GetComponent<ResourceManager>().GenerateIncome();
            ScanUnlockables();
            UpdateUI();
        }

        public void ScanUnlockables()
        {
            // Called on GameStart, GameTick, and Purchaser's ClickBuyButton
            GetComponent<PurchaserManager>().ScanUnlockables(); 
            UIManager.GetComponent<ContentUI>().ScanUIUnlockables(); 
            UIManager.GetComponent<NarrativeUI>().ScanForNarrativeEventTriggers(); 
        }

        public void UpdateUI()
        {
            UIManager.GetComponent<ContentUI>().UpdateContentUI();
           GetComponent<ResourceManager>().UpdateTexts();
           GetComponent<PurchaserManager>().UpdateAllTexts();
        }
        
        
    }
}
