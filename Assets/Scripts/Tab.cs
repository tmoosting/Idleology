using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tab : MonoBehaviour
{
    public Image highlight;
    public GameObject amountText;
    public GameObject incomeText;
    public bool _requiresGenerator;
    public Generator.Type _requiredGenerator;
    public bool _requiresModifier;
    public Modifier.Type _requiredModifier;
    public int _requiredWorkers;


    
    private void Start ()
    {
        highlight.enabled = false;
        amountText.GetComponent<TextMeshProUGUI>().text =  "0"; 

    }
    
    public void Unlock()
    {
        gameObject.SetActive(true);
    }
    
    public void UpdateTab()
    {
        /*
        incomeText.GetComponent<TextMeshProUGUI>().text = "+" + string.Format("{0:N0}", ResourceController.Instance.GetResourceIncome(resourceType)); 

        if (resourceType == ResourceController.ResourceType.Influence) 
            amountText.GetComponent<TextMeshProUGUI>().text =  ResourceController.Instance.resources[ResourceController.ResourceType.Influence].ToString(); 
        else if (resourceType == ResourceController.ResourceType.Force) 
            amountText.GetComponent<TextMeshProUGUI>().text = ResourceController.Instance.resources[ResourceController.ResourceType.Force].ToString(); 
            */

    }
    
    private void OnMouseDown()
    {
     //   UIController.Instance.ClickTab(this);
    }

    public void HighlightTab()
    {
        highlight.enabled = true;

    }
    public void RemoveHighlight()
    {
        highlight.enabled = false; 
    }
}
