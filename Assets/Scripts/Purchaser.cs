using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using Managers;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Purchaser : MonoBehaviour
{
        [Header("Assigns")]
        [HideInInspector] public IOperator source;  
        public GameObject imageObject;
        public GameObject nameObject;
        public GameObject buyButtonObject;
        public TextMeshProUGUI buyButtonText;
        public GameObject workersObject;
        public GameObject workersImage;
        public Color disabledColor;
        
        
        Color defaultColor; 
        GeneratorManager generatorManager;
        ModifierManager modifierManager;
        ResourceManager resourceManager; 
      
          
        [HideInInspector] public int _currentCostAmount;
        [HideInInspector] public Resource.Type _currentCostResource; 

        private void Awake()
        {
                defaultColor = buyButtonText.color;

        }

        private void Start()
        {
                generatorManager = GameStateManager.Instance.GetComponent<GeneratorManager>();
                modifierManager = GameStateManager.Instance.GetComponent<ModifierManager>();
                resourceManager = GameStateManager.Instance.GetComponent<ResourceManager>();
        }

        public void Unlock()
        {  
                if (source._state == IOperator.State.Hidden)
                { 
                        RevealPurchaser();
                }
                else     if (source._state == IOperator.State.Visible)
                { 
                        EnableBuyButton();
                }
                else     if (source._state == IOperator.State.Owned)
                { 
                        EnableBuyButton();
                }
                // Update buy button text
                buyButtonText.text = "$ " + GetCurrentBuyButtonCost();
        }
     
    


        public void ValidateUnlock()
        {   
                if (source._state == IOperator.State.Hidden)
                { 
                        if (source._requiresGenerator)
                        {
                                Generator generator = generatorManager.GetGenerator(source._requiredGenerator);
                                if ( generator._state != IOperator.State.Owned)
                                        return;
                                if (source._requiredLevel > generator.GetLevel()) 
                                        return;
                                
                        }
                        if (source._requiresModifier)
                        {
                                Modifier modifier = modifierManager.GetModifier(source._requiredModifier);
                                if ( modifier._state != IOperator.State.Owned)
                                        return;
                                if (source._requiredLevel > modifier.GetLevel()) 
                                        return;
                                
                        } 
                        Unlock(); 
                }
                else
                { 
                        if (resourceManager.RequirementsMet(source._costResource, source._purchaseCost) == false)
                                return; 
                        Unlock(); 
                }
              
        }



        public void ClickBuyButton()
        { 
                Purchase();   
                GameStateManager.Instance.ScanUnlockables();
        }

        private void Purchase()
        {
                if (source._state == IOperator.State.Visible)
                {
                        source._state = IOperator.State.Owned;
                }
                else  if (source._state == IOperator.State.Owned)
                {
                        source.AddLevel();
                }
        }

        public void HidePurchaser()
        {
                gameObject.SetActive(false);
                source._state = IOperator.State.Hidden; 
                DisableBuyButton();
        }
        public void RevealPurchaser()
        {
                // update state
                source._state = IOperator.State.Visible;
                
                // activate objects
                gameObject.SetActive(true);
                nameObject.SetActive(true);
                buyButtonObject.SetActive(true);
                
                // deactivate objects
                imageObject.SetActive(false);
                workersObject.SetActive(false);
                workersImage.SetActive(false);

                //     buyable = false;
        }


        void EnableBuyButton()
        {
                buyButtonText.color = defaultColor; 
                buyButtonObject.GetComponent<Button>().interactable = true; 
        }
        void DisableBuyButton()
        {
                buyButtonText.color = disabledColor; 
                buyButtonObject.GetComponent<Button>().interactable = false; 
        }


        

        public int GetCurrentBuyButtonCost()
        {
                if (source._state == IOperator.State.Hidden)
                {
                        Debug.LogWarning(("Why are you asking for a Hidden operator's buy cost..?"));
                        return source._purchaseCost; 
                }
                if (source._state == IOperator.State.Visible)
                        return source._purchaseCost;
             
                return source.GetLevel() * source._levelCost;
        }
        
        
        
}
