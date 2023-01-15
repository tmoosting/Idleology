using System.Collections;
using System.Collections.Generic;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


// UI container for Innovation, based on a simplified Purchaser
public class InnovationBody : MonoBehaviour
{
    [Header("Assigns")] 
    public GameObject imageObject;
    public GameObject nameObject;
    public GameObject buyButtonObject;
    public TextMeshProUGUI buyButtonText; 
    public Color disabledColor;


    private Innovation _innovation; 
    private  Color defaultColor;

    public void LoadInnovation(Innovation innov)
    {
        innov._state = innov._initialState;
        imageObject.SetActive(false);
        defaultColor = buyButtonText.color;
        buyButtonText.text = innov.buyCost.ToString();
        _innovation = innov;
        nameObject.GetComponent<TextMeshProUGUI>().text = innov.name; 
    } 
    public void ClickBuyButton()
    {
        Purchase(); 
    }
    void Purchase()
    { 
        imageObject.SetActive(true); 
        buyButtonObject.SetActive(false); 
        ResourceManager.PayResource(_innovation.primaryResource, _innovation.buyCost);
        _innovation._state = Innovation.State.Owned;
     
        InnovationManager.HandleInnovationPurchaseEffects(_innovation);
        GameStateManager.Instance.ScanUnlockables();
        GameStateManager.Instance.UpdateUI();
     //   GameStateManager.Instance.UIManager.GetComponent<HoverUI>().ExitHoverPurchaser(null);
    }
    
    public void ValidateInnovationBody()
    {
        if (ResourceManager.RequirementsMet(_innovation.primaryResource, _innovation.buyCost) == false)
            SetBuyButtonNonClickable();
        else
            SetBuyButtonClickable();
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

    
    private void OnMouseOver()
    {
        if (_innovation._state != Innovation.State.Hidden) 
            if (HoverUI != null) 
                HoverUI.HoverInnovationBody(this);    
      
                
    }
    private void OnMouseExit()
    {
        if (_innovation._state != Innovation.State.Hidden) 
            if (HoverUI != null) 
                HoverUI.ExitHoverInnovationBody(this);    
    }


    public Innovation GetInnovation()
    {
        return _innovation;
    }
    
    private ResourceManager _resourceManager;
    private ResourceManager ResourceManager  
    {
        get
        {
            if (_resourceManager == null)
                _resourceManager = FindObjectOfType<ResourceManager>();
            return _resourceManager;
        }
    }
    private InnovationManager _innovationManager;
    private InnovationManager InnovationManager  
    {
        get
        {
            if (_innovationManager == null)
                _innovationManager = FindObjectOfType<InnovationManager>();
            return _innovationManager;
        }
    }
    private HoverUI _hoverUI;
    private HoverUI HoverUI  
    {
        get
        {
            if (_hoverUI == null)
                _hoverUI = FindObjectOfType<HoverUI>();
            return _hoverUI;
        }
    }
}
