using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using ScriptableObjects;
using TMPro;
using UnityEngine;

public class Purchaser : MonoBehaviour
{
        [Header("Assigns")]
        public ScriptableObject source;
        public GameObject imageObject;
        public GameObject nameObject;
        public GameObject buyButtonObject;
        public TextMeshProUGUI buyButtonText;
        public GameObject workersObject;
        public GameObject workersImage;
        
        public enum Type
        {
                Generator,
                Modifier 
        }
        public enum State
        {
                Hidden,
                Visible,
                Purchased
        }
        
        private State _state;
        [SerializeField] Type _type;
        
        public void HidePurchaser()
        {
                gameObject.SetActive(false);
                _state = State.Hidden;
        }
        
        public void RevealPurchaser()
        {
                // make a string array of each enum type then convert back to enum
                string[] generatorTypes = System.Enum.GetNames(typeof(Type));
                foreach (string type in generatorTypes)        
                        if (gameObject.name == type)            
                                _type = (Type)System.Enum.Parse(typeof(Type), type);

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
                
                // set texts
                if (_type == Type.Generator)
                {
                        int value = 888;
                        Generator generator = (Generator) source;
                        if (_state == State.Visible) 
                                value = DataManager.Instance.GetGeneratorData(generator._type)._purchaseCost;
                        else  if (_state == State.Purchased) 
                                value = DataManager.Instance.GetGeneratorData(generator._type)._workerBaseCost * generator.GetWorkers();
 
                        buyButtonText.text = "$ " + value;
                }
                //ADD: Modifier
                
        }       


        public Type GetPurchaserType()
        {
                return _type;
        }
}
