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
            GetComponent<PurchaserManager>().ValidatePurchasers(); 
            UIManager.GetComponent<ContentUI>().ScanUIUnlockables(); 
            UIManager.GetComponent<NarrativeUI>().ScanForNarrativeEventTriggers(); 
        }

        public void UpdateUI()
        {
            UIManager.GetComponent<ContentUI>().UpdateContentUI();
            UIManager.GetComponent<BubbleUI>().BubbleTick();
           GetComponent<ResourceManager>().UpdateTexts();
           GetComponent<PurchaserManager>().UpdateAllTexts();
        }
        private GameStateManager _gameStateManager;
        private GameStateManager GameStateManagerr  
        {
            get
            {
                if (_gameStateManager == null)
                    _gameStateManager = FindObjectOfType<GameStateManager>();
                return _gameStateManager;
            }
        }
        private GameSettings _gameSettings;
        private GameSettings GameSettings  
        {
            get
            {
                if (_gameSettings == null)
                    _gameSettings = FindObjectOfType<GameSettings>();
                return _gameSettings;
            }
        }
        private GeneratorManager _generatorManager;
        private GeneratorManager GeneratorManager  
        {
            get
            {
                if (_generatorManager == null)
                    _generatorManager = FindObjectOfType<GeneratorManager>();
                return _generatorManager;
            }
        }
        private ModifierManager _modifierManager;
        private ModifierManager ModifierManager  
        {
            get
            {
                if (_modifierManager == null)
                    _modifierManager = FindObjectOfType<ModifierManager>();
                return _modifierManager;
            }
        }
    }

    
}
