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
        PurchaserManager purchaserManager;
        GeneratorManager generatorManager;
        ModifierManager modifierManager;
        ResourceManager resourceManager;
        private HoverUI hoverUI;
           
 
        public IOperator.State GetState()
        {
                return source._state;
        }
        public void Initialize()
        {
                defaultColor = buyButtonText.color;
                purchaserManager = GameStateManager.Instance.GetComponent<PurchaserManager>();
                generatorManager = GameStateManager.Instance.GetComponent<GeneratorManager>();
                modifierManager = GameStateManager.Instance.GetComponent<ModifierManager>();
                resourceManager = GameStateManager.Instance.GetComponent<ResourceManager>();
                hoverUI = GameStateManager.Instance.UIManager.GetComponent<HoverUI>();
        }

        
        
        
         
     
        
            
        /*
        public void HidePurchaser()
        { 
           
        }
        public void RevealPurchaser()
        { 
                // update state
                source._state = IOperator.GetState.Visible;
                
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
        */
        void SetImageGrey()
        { 
                imageObject.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
        }

        void SetImageBlack()
        { 
                imageObject.GetComponent<Image>().color = new Color(0, 0, 0, 1f);

        }
        public void Preview()
        {
                SetState(IOperator.State.Visible); 
        }
        public void Reveal() // only called for LemonadeStand at this point, for game start
        {
                Debug.Log("REVEAL " + gameObject.name);
                SetState(IOperator.State.Visible);
                SetState(IOperator.State.Buyable);
        }
      
        public void Hide()
        {
                SetState(IOperator.State.Hidden);
        }
        private void SetState(IOperator.State  newState)
        {
                if (gameObject.name == "LongerHours")
                        Debug.Log("longhour " + GetState() + "... " + newState);
                if (newState == IOperator.State.Hidden)
                {
                        gameObject.SetActive(false);  
                        nameObject.SetActive(false);  
                        workersObject.SetActive(false); 
                        workersImage.SetActive(false);

                }
                if (newState == IOperator.State.Visible)
                {
                        gameObject.SetActive(true);  
                        nameObject.SetActive(true);
                        imageObject.SetActive(false);
                        buyButtonObject.SetActive(false);
                }
                if (newState == IOperator.State.Buyable)
                {
                        buyButtonObject.SetActive(true);
                }
                if (newState == IOperator.State.Owned)
                {
                        workersObject.SetActive(true);
                        workersImage.SetActive(true); 
                        imageObject.SetActive(true);
                        SetImageGrey();
                }
                if (newState == IOperator.State.Operated)
                {
                        SetImageBlack();
                         if (source.isGenerator && source._resource == Resource.Type.Credit) // ugly way to limit to 'base' generators
                             purchaserManager.RevealNewestBaseGenerator(this);
                }
                if (newState == IOperator.State.Completed)
                {
                        buyButtonText.gameObject.SetActive(false);
                } 
                source._state = newState;
                UpdateTexts();
          
        }

        


        void SetBuyButtonClickable()
        {
                buyButtonText.color = defaultColor; 
                buyButtonObject.GetComponent<Button>().interactable = true; 
        }
        void SetBuyButtonNonClickable()
        {
                buyButtonText.color = disabledColor; 
                buyButtonObject.GetComponent<Button>().interactable = false; 
        } 
         
   

        public void ValidateLockState()
        { 
                if (gameObject.name == "LongerHours")
                        Debug.Log("VALIDATE longhour " + GetState() ); 
                if (AtMaxLevel()) 
                        SetState(IOperator.State.Completed);  
                else if (CheckNextStateRequirements())
                        ProceedToNextState();

                if (GetState() == IOperator.State.Hidden || 
                    GetState() == IOperator.State.Visible ||
                    GetState() == IOperator.State.Completed)
                        return;
           
                if (resourceManager.RequirementsMet(source._costResource, GetCurrentCost()) == false)
                        SetBuyButtonNonClickable();
                else
                        SetBuyButtonClickable();
        }

       
        /*
               Hidden, // nothing visible
               Visible, // name visible, buybutton is not
               Buyable, // name and buybutton visible
               Owned, // Purchased but with 0 workers
               Operated, // purchased with >= 1 worker 
               Completed // max level reached
               */
        private bool CheckNextStateRequirements()
        {  
              //  if (GetState() == IOperator.State.Hidden || GetState() == IOperator.State.Visible)  
                if (GetState() == IOperator.State.Hidden || GetState() == IOperator.State.Visible)
                { 
                        if (source._requiresGenerator)
                        {
                                Generator requiredGenerator = generatorManager.GetGenerator(source._requiredGeneratorType);
                                if (requiredGenerator._state == IOperator.State.Hidden 
                                    || requiredGenerator._state == IOperator.State.Visible
                                     ||    requiredGenerator._state == IOperator.State.Buyable)
                                        return false;
                                if (source._requiredLevel > requiredGenerator.GetLevel())
                                        return false; 
                        }
                        if (source._requiresModifier)
                        {
                                Modifier requiredModifier = modifierManager.GetModifier(source._requiredModifier);
                                if (requiredModifier._state == IOperator.State.Hidden 
                                    || requiredModifier._state == IOperator.State.Visible
                                    ||    requiredModifier._state == IOperator.State.Buyable)
                                        return false;
                                if (source._requiredLevel > requiredModifier.GetLevel())
                                        return false;
                        }
                } 
                return true;
        }

        private void ProceedToNextState()
        {
                Debug.Log("NEXT for " + gameObject.name);
                if (GetState() == IOperator.State.Hidden)
                        SetState(IOperator.State.Visible);
                 if (GetState() == IOperator.State.Visible)
                         SetState(IOperator.State.Buyable);
        }
        private bool AtMaxLevel()
        {
                if (source.isGenerator)
                        return false;
                else 
                    return source.GetLevel() == 10;
        }
        private bool PurchaseAllowed()
        {
                if (resourceManager.RequirementsMet(source._costResource, GetCurrentCost()) == false)
                        return false;
                return GetState() == IOperator.State.Buyable || GetState() == IOperator.State.Operated || GetState() == IOperator.State.Owned; 
        }
        public void ClickBuyButton()
        { 
                Purchase();
                if (Input.GetKey(KeyCode.LeftControl) )
                {
                        for (int i = 0; i < 10; i++)
                        {
                                GameStateManager.Instance.ScanUnlockables();
                                if (PurchaseAllowed())
                                        Purchase();
                        }
                }
        }

      

        void Purchase()
        { 
                resourceManager.PayResource(source._costResource, GetCurrentCost());
        if (GetState() == IOperator.State.Buyable)
        {
                SetState(IOperator.State.Owned); 
       
        }
        else if (GetState() == IOperator.State.Owned)
        {
                SetState(IOperator.State.Operated);
                source.AddLevel();
        }
        else if (GetState() == IOperator.State.Operated)
        { 
                source.AddLevel();
        }
        if (source.isGenerator == false)
                modifierManager.HandleStaticModifierEffects(source);
        GameStateManager.Instance.ScanUnlockables();
        GameStateManager.Instance.UpdateUI();
        GameStateManager.Instance.UIManager.GetComponent<HoverUI>().ExitHoverPurchaser(null);
        }

     

  


        public void UpdateTexts()
        {
                if (GetState() != IOperator.State.Hidden)
                {
                        string currencyString = "";
                        if (source._costResource == Resource.Type.Credit)
                                currencyString += "$ ";
                        if (source._costResource == Resource.Type.Influence)
                                currencyString += "INF ";
                        if (source._costResource == Resource.Type.Force)
                                currencyString += "FRC ";
                        if (source._costResource == Resource.Type.Tech) 
                                currencyString += "TEC ";
                        if (source._costResource == Resource.Type.Happiness)
                                currencyString += "HAP ";

                        buyButtonText.text = currencyString + resourceManager.FormatNumber( GetCurrentCost());
                        workersObject.GetComponent<TextMeshProUGUI>().text = source.GetLevel().ToString();
                }
        }

        private ulong GetCurrentCost()
        {
                if (GetState() == IOperator.State.Hidden)
                {
                        Debug.LogWarning(("Why am I asking for a Hidden operator's buy cost..?"));
                        return source._purchaseCost; 
                }
                if (GetState() == IOperator.State.Visible ||GetState() == IOperator.State.Visible )
                        return source._purchaseCost;

                float rawCost = (source.GetLevel() + 1) * source._levelCost;
                float modifiedCost = modifierManager.GetDiscountLevellingPrice(rawCost);
                return (ulong)modifiedCost; 
        }

    
        private void OnMouseOver()
        {
                if (GetState() != IOperator.State.Hidden)
                {
                        if (hoverUI == null)
                                hoverUI = FindObjectOfType<HoverUI>();
                        if (hoverUI != null) 
                                hoverUI.HoverPurchaser(this);    
                }
                
        }
        private void OnMouseExit()
        {
                if (GetState() != IOperator.State.Hidden)
                {
                        if (hoverUI == null)
                                hoverUI = FindObjectOfType<HoverUI>();
                        if (hoverUI != null) 
                                hoverUI.ExitHoverPurchaser(this);       
                } 
        }
}
