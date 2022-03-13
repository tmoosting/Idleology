using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using TMPro;


public class NarrativePopup : MonoBehaviour
{ 
     public TextMeshProUGUI narrativePopupText;
     
     
     public void ClickOK()
     {
      gameObject.SetActive(false);      
     }

     public void LoadNarrativeEventIntoPopup(NarrativeEvent narrativeEvent)
     {
         Debug.Log ("Setting for  " + gameObject.name + " with id: " + narrativeEvent.ID);
         narrativePopupText.text = narrativeEvent.eventText;
         gameObject.SetActive(true);
     }
}
