using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using TMPro;
using UnityEngine;

public class HoverUI : MonoBehaviour
{
    [Header("Assigns")]
    public GameObject toolTip;
    public TextMeshProUGUI toolTipText;
    public Canvas canvas;

    [Header("Settings")] 
    public bool showTooltips = false;

    Vector2 pos;
    private void Awake()
    {
        toolTip.SetActive(false);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform, Input.mousePosition,
            canvas.worldCamera,
            out pos);
    }

    public void HoverPurchaser(Purchaser purchaser)
    { 
        if (showTooltips)
        {
            if (purchaser.GetState() == IOperator.State.Buyable)
                ShowHoverUI(purchaser.source._purchaseTooltip);
            else if (purchaser.GetState() == IOperator.State.Owned )
                ShowHoverUI(purchaser.source._firstWorkerTooltip);
            else if (purchaser.GetState() == IOperator.State.Operated)
              if (GameSettings.alwaysShowWorkerTooltip == true)
                 ShowHoverUI(purchaser.source._firstWorkerTooltip); 
            if (purchaser.source.isGenerator == false)
            {
                if (purchaser.source.GetLevel() == 10)
                    ShowHoverUI(purchaser.source._completedTooltip);
 
            }

        }  
    }

    private void ShowHoverUI(string givenText)
    {
        toolTip.SetActive(true);
        toolTipText.text = givenText.ToLower();
        Vector2 movePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Input.mousePosition,canvas.worldCamera,
            out movePos);
        movePos += new Vector2(0, -125);
        toolTip.transform.position =canvas.transform.TransformPoint(movePos);  
    }

    public void ExitHoverPurchaser(Purchaser purchaser)
    {
      toolTip.SetActive(false);
    }
    
    private GameSettings _gameSettings;
    private GameSettings GameSettings  
    {
        get
        {
            if (_gameSettings == null)
                _gameSettings = FindObjectOfType<GameSettings>();
            return _gameSettings;
        }
    }
    
}
