using System.Collections;
using System.Collections.Generic;
using Interfaces;
using Managers;
using Michsky.UI.ModernUIPack;
using ScriptableObjects;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class Tab : MonoBehaviour
{
    public Purchaser baseGenerator;
    public Image highlight;
    public GameObject amountText;
    public GameObject incomeText;
    public bool _requiresGenerator;
    public Generator.Type _requiredGenerator;
    public bool _requiresModifier;
    public Modifier.Type _requiredModifier;
    public ulong _requiredLevel;
    private bool _locked = true;

    
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

        ResourceManager resourceManager = GameStateManager.Instance.GetComponent<ResourceManager>();
      

        if (gameObject.name == "TabInfluence")
        {
            amountText.GetComponent<TextMeshProUGUI>().text =  resourceManager.FormatNumber(  resourceManager.GetResource(Resource.Type.Influence)._amount); 
            incomeText.GetComponent<TextMeshProUGUI>().text = "+" + string.Format("{0:N0}", resourceManager.FormatNumber(resourceManager.CalculateIncome(Resource.Type.Influence)));
        }
        else if (gameObject.name == "TabForce")
        {
            amountText.GetComponent<TextMeshProUGUI>().text =  resourceManager.FormatNumber( resourceManager.GetResource(Resource.Type.Force)._amount); 
            incomeText.GetComponent<TextMeshProUGUI>().text = "+" + string.Format("{0:N0}", resourceManager.FormatNumber(resourceManager.CalculateIncome(Resource.Type.Force)));
        }
        else if (gameObject.name == "TabTech")
        {
            amountText.GetComponent<TextMeshProUGUI>().text =  resourceManager.FormatNumber( resourceManager.GetResource(Resource.Type.Tech)._amount); 
            incomeText.GetComponent<TextMeshProUGUI>().text = "+" + string.Format("{0:N0}", resourceManager.FormatNumber(resourceManager.CalculateIncome(Resource.Type.Tech)));
        }  

    }
    
    private void OnMouseDown()
    {
        GameStateManager.Instance.UIManager.GetComponent<ContentUI>().OpenTab(this);
    }

    public void HighlightTab()
    {
        highlight.enabled = true;

    }
    public void RemoveHighlight()
    {
        highlight.enabled = false; 
    }

    public void ValidateUnlock()
    { 
        if (_locked == true)
        {
            if (_requiresGenerator)
            {
                Generator generator = GeneratorManager.GetGenerator(_requiredGenerator);
                if (generator._state == IOperator.State.Hidden || generator._state == IOperator.State.Visible || generator._state == IOperator.State.Buyable)
                    return;
                if (_requiredLevel > generator.GetLevel())
                    return;
            }
            if (_requiresModifier)
            {
                Modifier modifier = ModifierManager.GetModifier(_requiredModifier);
                if (modifier._state == IOperator.State.Hidden || modifier._state == IOperator.State.Visible || modifier._state == IOperator.State.Buyable)
                    return;
                if (_requiredLevel > modifier.GetLevel())
                    return;
            }

            UnlockTab();
        }
    }
    
    
    public void HideTab()
    {
        gameObject.SetActive(false);
    }
    private void UnlockTab()
    {
        _locked = false;
        gameObject.SetActive(true);
        baseGenerator.Reveal();
    }

    private GameStateManager _gameStateManager;
    private GameStateManager GameStateManager 
    {
        get
        {
            if (_gameStateManager == null)
                _gameStateManager = FindObjectOfType<GameStateManager>();
            return _gameStateManager;
        }
    }
          
    private GeneratorManager _generatorManager;
    private GeneratorManager GeneratorManager  
    {
        get
        {
            if (_generatorManager == null)
                _generatorManager = FindObjectOfType<GeneratorManager>();
            return _generatorManager;
        }
    }
    private ModifierManager _modifierManager;
    private ModifierManager ModifierManager  
    {
        get
        {
            if (_modifierManager == null)
                _modifierManager = FindObjectOfType<ModifierManager>();
            return _modifierManager;
        }
    }
}
