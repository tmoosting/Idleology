using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// manages visual effects: ChangeVisuals, Bubbles, ...


public class VisualsManager : MonoBehaviour
{
    public static VisualsManager Instance;

    /*CreditIncome,
    InfluenceIncome,
    ForceIncome,
    BuyButton,
    Happiness*/
    
    [Header("Assigns")]
    public GameObject changeVisualPrefab;
    public Transform changeVisualCreditLocation;

    [Header("Settings")] 
    public float fadeDelay = 1.5f; // in seconds
    public float fadeSpeed = 1f;

    private void Awake()
    {
        Instance = this;
    }



    public void SpawnChangeVisual(ChangeVisual.Type type, ulong amount, bool positive = true)
    {
        GameObject spawnObj = Instantiate(changeVisualPrefab,changeVisualCreditLocation );
        ChangeVisual changeVisual = spawnObj.GetComponent<ChangeVisual>();

        changeVisual.changeText.text = amount.ToString();
        if (type == ChangeVisual.Type.CreditIncome)
            spawnObj.transform.position = changeVisualCreditLocation.position;

        if (positive == false)
        {
            changeVisual.imageGreen.enabled = false;
            changeVisual.imageRed.enabled = true; 
        }
    }
    
    
}
