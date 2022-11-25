using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using Managers;
using Michsky.UI.ModernUIPack;
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
        private HoverUI hoverUI;
        private bool unlocked = false;
           

        private void Awake()
        {

        }

        private void Start()
        { 
             
        }

        public void Initialize()
        {
                defaultColor = buyButtonText.color;
                generatorManager = GameStateManager.Instance.GetComponent<GeneratorManager>();
                modifierManager = GameStateManager.Instance.GetComponent<ModifierManager>();
                resourceManager = GameStateManager.Instance.GetComponent<ResourceManager>();
                hoverUI = GameStateManager.Instance.UIManager.GetComponent<HoverUI>();
        }
        

        private void Lock()
        {
                unlocked = false;
                DisableBuyButton();
        }
        private void PermaLock()
        {
            unlocked = false;
            DisableBuyButton();
        buyButtonText.gameObject.SetActive(false);
        }
    public void Unlock()
        {
                unlocked = true;
                if (source._state == IOperator.State.Hidden)
                { 
                        RevealPurchaser();
                        ValidateUnlock();
                }
                else     if (source._state == IOperator.State.Visible)
                { 
                        EnableBuyButton(); 
                }
                else     if (source._state == IOperator.State.Owned)
                { 
                        EnableBuyButton();
                }
                // Update texts
                UpdateTexts();
        }
        public void ValidateUnlock()
        {
               
                if (source._state == IOperator.State.Hidden)
                { 
                        if (source._requiresGenerator)
                        { 
                                Generator generator = generatorManager.GetGenerator(source._requiredGenerator);
                                if (generator._state != IOperator.State.Owned)
                                {
                                        Lock();
                                        return;
                                }
                                if (source._requiredLevel > generator.GetLevel()) 
                                {
                                        Lock();
                                        return;
                                }
                                
                        }
                        if (source._requiresModifier)
                        {
                                Modifier modifier = modifierManager.GetModifier(source._requiredModifier);
                                if ( modifier._state != IOperator.State.Owned)
                                {
                                        Lock();
                                        return;
                                }
                                if (source._requiredLevel > modifier.GetLevel()) 
                                {
                                        Lock();
                                        return;
                                }
                                
                        }
                        if (source.isGenerator == false && source.GetLevel() == 10)
                        {
                                PermaLock();
                                return;
                        }
                        else
                                Unlock();
                }
                else
                {
                        if (resourceManager.RequirementsMet(source._costResource, GetCurrentCost()) == false)
                        {
                                Lock();
                                return;
                        }
                        if (source.isGenerator == false && source.GetLevel() == 10)
                        {
                                PermaLock();
                                return;
                        }
                        else
                                Unlock();
                }
              
        }

        public void ClickBuyButton()
        { 
                Purchase();
                if (Input.GetKey(KeyCode.LeftControl) )
                {
                        for (int i = 0; i < 10; i++)
                        {
                                GameStateManager.Instance.ScanUnlockables();
                                if (unlocked == true)
                                        Purchase();
                        }       
                                          
                }
        }

        void Purchase()
        { 
                resourceManager.PayResource(source._costResource, GetCurrentCost());
        if (source._state == IOperator.State.Visible)
        {
            source._state = IOperator.State.Owned;
            workersObject.SetActive(true);
            if (source.isGenerator)
                    workersImage.SetActive(true);
            else
                    source.AddLevel();
            imageObject.SetActive(true);
        }
        else  if (source._state == IOperator.State.Owned)
                source.AddLevel();

        if (source.isGenerator == false)
                modifierManager.HandleStaticModifierEffects(source);
        GameStateManager.Instance.ScanUnlockables();
        GameStateManager.Instance.UpdateUI();
        GameStateManager.Instance.UIManager.GetComponent<HoverUI>().ExitHoverPurchaser(null);
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


        public void UpdateTexts()
        {
        if (source._state != IOperator.State.Hidden)
        {
            string currencyString = "";
            if (source._costResource == Resource.Type.Credit)
                currencyString += "$ ";
            if (source._costResource == Resource.Type.Influence)
                currencyString += "";
            if (source._costResource == Resource.Type.Force)
                currencyString += "";
            if (source._costResource == Resource.Type.Happiness)
                currencyString += "";

            buyButtonText.text = currencyString + resourceManager.FormatNumber( GetCurrentCost());
            workersObject.GetComponent<TextMeshProUGUI>().text = source.GetLevel().ToString();
        }
    }

        public ulong GetCurrentCost()
        {
                if (source._state == IOperator.State.Hidden)
                {
                        Debug.LogWarning(("Why are you asking for a Hidden operator's buy cost..?"));
                        return source._purchaseCost; 
                }
                if (source._state == IOperator.State.Visible)
                        return source._purchaseCost;

                float rawCost = (source.GetLevel() + 1) * source._levelCost;
                float modifiedCost = modifierManager.GetDiscountLevellingPrice(rawCost);
                return (ulong)modifiedCost; 
        }

        private void OnMouseOver()
        {
                if (source._state != IOperator.State.Hidden)
                {
                        if (hoverUI == null)
                                hoverUI = FindObjectOfType<HoverUI>();
                        if (hoverUI != null) 
                                hoverUI.HoverPurchaser(this);    
                }
                
        }
        private void OnMouseExit()
        {
                if (source._state != IOperator.State.Hidden)
                {
                        if (hoverUI == null)
                                hoverUI = FindObjectOfType<HoverUI>();
                        if (hoverUI != null) 
                                hoverUI.ExitHoverPurchaser(this);       
                } 
        }
}
