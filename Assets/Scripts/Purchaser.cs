using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Purchaser : MonoBehaviour, IUnlockable
{
        [Header("Assigns")]
        [HideInInspector] public ScriptableObject source;
        public GameObject imageObject;
        public GameObject nameObject;
        public GameObject buyButtonObject;
        public TextMeshProUGUI buyButtonText;
        public GameObject workersObject;
        public GameObject workersImage;
        public Color disabledColor;
        Color defaultColor; 

        
        public enum Type
        {
                Generator,
                Modifier 
        }
        public enum State
        {
                Hidden,
                Visible,
                Owned
        }
        
        private State _state;
        [SerializeField] Type _type;
  //      [HideInInspector] public bool buyable = false;

        private void Awake()
        {
                defaultColor = buyButtonText.color; 

        }

      
        
      

        private void SetPrice()
        {
                if (_type == Type.Generator)
                {
                        int value = 888;
                        Generator generator = (Generator) source;
                        if (_state == State.Visible) 
                                value = DataManager.Instance.GetGeneratorData(generator._type)._purchaseCost;
                        else  if (_state == State.Owned) 
                                value = DataManager.Instance.GetGeneratorData(generator._type)._workerBaseCost * generator.GetWorkers();
 
                        buyButtonText.text = "$ " + value;
                }
                //ADD: Modifier
        }

    
        public void Unlock()
        {
                // Called from Unlockable objects
                if (_state == State.Hidden)
                {
                        RevealPurchaser();
                }else     if (_state == State.Visible)
                {
                        
                }else     if (_state == State.Owned)
                {
                        
                }
                SetPrice();
        }
        public void HidePurchaser()
        {
                gameObject.SetActive(false);
                _state = State.Hidden;
                DisableBuyButton();
        }
        public void RevealPurchaser()
        {
                // update state
                _state = State.Visible;
                
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
                buyButtonText.color = default; 
                buyButtonObject.GetComponent<Button>().interactable = true; 
        }
        void DisableBuyButton()
        {
                buyButtonText.color = disabledColor; 
                buyButtonObject.GetComponent<Button>().interactable = false; 
        }





        public void ClickPurchase()
        {
                GetComponent<UnlockManager>().UpdateUnlockables();
        }
        
        
        
        
        
        
        
        public Type GetPurchaserType()
        {
                return _type;
        }
}
