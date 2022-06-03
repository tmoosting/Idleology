using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using TMPro;
using UI;


public class NarrativePopup : MonoBehaviour
{ 
     public TextMeshProUGUI narrativePopupText;
     private NarrativeEvent loadedEvent; // can use later, just add exception for the NAR00 and NAR01 which don't get loaded
     
     public void ClickOK()
     {
      gameObject.SetActive(false);
      FindObjectOfType<NarrativeUI>().ClickedPopup(this);
     }

     public void LoadNarrativeEventIntoPopup(NarrativeEvent narrativeEvent)
     {
         Debug.Log ("Setting for  " + gameObject.name + " with id: " + narrativeEvent.ID);
         loadedEvent = narrativeEvent;
         narrativePopupText.text = narrativeEvent.eventText;
         gameObject.SetActive(true);
     }
}
