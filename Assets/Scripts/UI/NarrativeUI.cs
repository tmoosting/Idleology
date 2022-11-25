using System;
using System.Collections.Generic;
using System.IO;
using Interfaces;
using Managers;
using ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace UI
{
     //TODO:
     // Create a callback system ingame ticks and user actions, that includes checking for triggers on all available NarrPops
     // Create NarrPop placement system 
    
    public class NarrativeUI : MonoBehaviour
    {

        [Header("Assigns")]
        public GameObject narrativePopupsParent;
        public GameObject narrativePopupPrefab;
        public GameObject narrativeZero;
        public GameObject narrativeZeroOKButton;
        public GameObject narrativeZeroOne; // second popup, behind the first
        [Header("Assigns")] 
        public bool showPopups;
        private List<NarrativePopup> narrativePopupList = new List<NarrativePopup>();
        private List<NarrativeEvent> narrativeEventList = new List<NarrativeEvent>();
        
        
        [HideInInspector] public bool finishedIntroEvents = false;

        
        public void InitializeNarratives(bool newGame, bool skipIntro)
        {
            foreach (NarrativePopup narrPopup in narrativePopupsParent.GetComponentsInChildren<NarrativePopup>())
                narrPopup.gameObject.SetActive(false);
            narrativeZeroOKButton.gameObject.SetActive(false);
            
            if (newGame == true && showPopups == true)
            {
                if (skipIntro == false)
                {
                    narrativeZero.SetActive(true);
                }
                else
                {
                    
                }
            }
            else
            {
                //todo: load from save
            }
        }
              
        void PopupEvent(NarrativeEvent narrativeEvent)
        {
            if (showPopups == false)
                return;
            // deactive previous popups
            foreach (var popup in narrativePopupList)
            {
                popup.gameObject.SetActive(false);
            }
            narrativeEvent.hasBeenTriggered = true;
            // create prefab narrativePopupPrefab
 

            if (PopupIsPremade(narrativeEvent))
            {
                GetExistingPopup(narrativeEvent).LoadNarrativeEventIntoPopup(narrativeEvent);
                narrativePopupList.Add(  GetExistingPopup(narrativeEvent) );

            
            }
            else
            {
                GameObject popObj = Instantiate(narrativePopupPrefab, narrativePopupsParent.transform);
                popObj.transform.position = new Vector3(0, 0, 0);
                popObj.GetComponent<NarrativePopup>().LoadNarrativeEventIntoPopup(narrativeEvent);
                narrativePopupList.Add( popObj.GetComponent<NarrativePopup>());
            }
                
        }
        public void FinishMoneyPiling()
        {
            narrativeZeroOKButton.gameObject.SetActive(true);  
        }
        public void ClickedPopup(NarrativePopup clickedPopup)
        { 
            if (clickedPopup.gameObject.name == "NAR00")
                narrativeZeroOne.SetActive(true);
            if (clickedPopup.gameObject.name == "NAR01")
            {
                finishedIntroEvents = true;
                FindObjectOfType<PurchaserManager>().purchaserList[0].Unlock();
            }
        }
        public void ScanForNarrativeEventTriggers()
        {
            NarrativeEvent triggeredEvent = null;
            foreach (NarrativeEvent narrativeEvent in narrativeEventList)
                if (narrativeEvent.hasBeenTriggered == false)
                    if (EventMeetsGeneratorRequirements(narrativeEvent))
                        if (EventMeetsModifierRequirements(narrativeEvent))
                            if (EventMeetsResourceRequirements(narrativeEvent)) 
                                triggeredEvent = narrativeEvent;  
            if (triggeredEvent != null)
                PopupEvent(triggeredEvent);
        }


        private bool EventMeetsGeneratorRequirements(NarrativeEvent narrativeEvent )
        { 
            bool returnValue = true;
            if (narrativeEvent.generatorTriggermap.Count == 0)
                return true;
            else
            { 
                foreach (Generator.Type type in narrativeEvent.generatorTriggermap.Keys)
                {
                    Generator generator = GeneratorManager.Instance.GetGenerator(type);
 

                    if (generator._state != IOperator.State.Owned)
                    {
                        returnValue = false; 
                    } 
                    else 
                    {  
                        if (narrativeEvent.generatorTriggermap[type] != 0)
                            if (generator._level < narrativeEvent.generatorTriggermap[type])
                                returnValue = false;
                    }
                }
            }

         //    Debug.Log("returning " + returnValue + " for id " + narrativeEvent.ID);
            return returnValue;
        } 
        private bool EventMeetsModifierRequirements(NarrativeEvent narrativeEvent )
        {
            bool returnValue = true;
            if (narrativeEvent.modifierTriggermap.Count == 0)
                return true;
            else
            {
                foreach (Modifier.Type type in narrativeEvent.modifierTriggermap.Keys)
                {
                    Modifier modifier = ModifierManager.Instance.GetModifier(type);
                    if (modifier._state != IOperator.State.Owned)
                    {
                        returnValue = false;
                    }
                    else 
                    {
                        if (narrativeEvent.modifierTriggermap[type] != 0)
                            if (modifier._level < narrativeEvent.modifierTriggermap[type])
                                returnValue = false;
                    } 

                }
            }
      //    Debug.Log("returning " + returnValue + " for id " + narrativeEvent.ID);

            return returnValue;
        } 
        private bool EventMeetsResourceRequirements(NarrativeEvent narrativeEvent )
        {
            bool returnValue = true;
            if (narrativeEvent.resourceTriggermap.Count == 0)
                return true;
            else
            {
                foreach (Resource.Type type in narrativeEvent.resourceTriggermap.Keys)
                {
                    Resource resource = ResourceManager.Instance.GetResource(type);
                    Debug.Log("type   " + type  + " amount " + resource._amount + " and in dict: " + narrativeEvent.resourceTriggermap[type] );

                    if (narrativeEvent.resourceTriggermap[type] != 0)
                    {
                        if (resource._type == Resource.Type.Happiness)
                        {
                            if (resource._amount < narrativeEvent.resourceTriggermap[type])
                                returnValue = true;  
                        }
                        else
                        if (resource._amount < narrativeEvent.resourceTriggermap[type])
                            returnValue = false;  
                    } 
                        
                }
            }

            return returnValue;
        } 
  


        private bool PopupIsPremade(NarrativeEvent narrativeEvent)
        {
            bool returnValue = false;
            foreach (NarrativePopup narrPopup in narrativePopupsParent.GetComponentsInChildren<NarrativePopup>(true))
                if (narrPopup.gameObject.name == narrativeEvent.ID)
                    returnValue =  true; 
            return returnValue;
        }

        private NarrativePopup GetExistingPopup(NarrativeEvent narrativeEvent)
        {
            NarrativePopup returnPopup = null;
            foreach (NarrativePopup narrPopup in narrativePopupsParent.GetComponentsInChildren<NarrativePopup>(true))
                if (narrPopup.gameObject.name == narrativeEvent.ID)
                    returnPopup =  narrPopup;

            return returnPopup;
        }
        
        
        
        
        
        #region DataManagerFunctions
        
        public void DeleteNarrativeObjects()
        {
            if (narrativeEventList.Count != 0)
            {
                foreach (NarrativeEvent narrEvent in narrativeEventList)
                {
                    string path = AssetDatabase.GetAssetPath(narrEvent);
                    AssetDatabase.DeleteAsset(path);
                    narrativeEventList.Remove(narrEvent);
                }
            }
        }
        public void LoadNarrativeObjects()
        { 
            narrativeEventList.Clear();
            string[] allPaths = Directory.GetFiles("Assets/ScriptableObjects/NarrativeEvents", "*.asset", 
                SearchOption.AllDirectories);
            foreach (string path in allPaths)
            {
                string cleanedPath = path.Replace("\\", "/");
                narrativeEventList.Add((NarrativeEvent)AssetDatabase.LoadAssetAtPath(cleanedPath,  typeof(NarrativeEvent)));
            }
        }
        public void ReceiveNewNarrativeEvent(NarrativeEvent narrEvent)
        {
            narrativeEventList.Add(narrEvent);
        }
        #endregion


    
    }
}
