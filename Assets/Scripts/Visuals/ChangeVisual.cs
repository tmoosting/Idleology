using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// small red/green indicator that pops up and fades away on purchase actions



public class ChangeVisual : MonoBehaviour
{
  public enum Type // Determines placement location
  {
    CreditIncome,
    InfluenceIncome,
    ForceIncome,
    BuyButton,
    Happiness
  }

  public Image imageRed;
  public Image imageGreen;
  public TextMeshProUGUI changeText;


  private void Start()
  {
    StartCoroutine(FadeOut());
  }

  private IEnumerator FadeOut()
  { 
    for (float i = VisualsManager.Instance.fadeDelay; i >= 0; i -= Time.deltaTime * VisualsManager.Instance.fadeSpeed)
    {
      // set color with i as alpha
      imageRed.color = new Color(imageRed.color.r,imageRed.color.g , imageRed.color.b, i);
      imageGreen.color = new Color(imageGreen.color.r,imageGreen.color.g , imageGreen.color.b, i);
      changeText.color = new Color(0, 0, 0, i);
      yield return null;
    } 
    Destroy(gameObject); 
  }
}
