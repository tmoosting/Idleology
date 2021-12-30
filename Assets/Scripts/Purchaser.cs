using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Purchaser : MonoBehaviour
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
        [HideInInspector] public bool buyable = false;
        
        public void HidePurchaser()
        {
                gameObject.SetActive(false);
                _state = State.Hidden;
        }
        public void UnlockPurchaser()
        {
                if (_state == State.Hidden)
                {
                        RevealPurchaser();
                }else     if (_state == State.Visible)
                {
                        
                }else     if (_state == State.Owned)
                {
                        
                }
                SetTexts();
        }
        
      

        private void SetTexts()
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

                buyable = false;
        }

        void DisableBuyButton()
        {
                buyButtonText.color = disabledColor; 
                buyButtonObject.GetComponent<Button>().interactable = false; 
        }

        public Type GetPurchaserType()
        {
                return _type;
        }
}
