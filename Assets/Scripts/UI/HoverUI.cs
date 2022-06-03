using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HoverUI : MonoBehaviour
{
    public GameObject toolTip;
    public TextMeshProUGUI toolTipText;
    public Canvas canvas;

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
        toolTip.SetActive(true);
        toolTipText.text = purchaser.source._tooltipText;
        Vector2 movePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
           canvas.transform as RectTransform,
            Input.mousePosition,canvas.worldCamera,
            out movePos);
        movePos += new Vector2(0, -45);
        toolTip.transform.position =canvas.transform.TransformPoint(movePos);
         
    }

    public void ExitHoverPurchaser(Purchaser purchaser)
    {
      toolTip.SetActive(false);
    }
}
