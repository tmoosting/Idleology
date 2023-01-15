using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;


 

[CreateAssetMenu(fileName = "Innovation", menuName = "ScriptableObjects/Innovation", order = 3)]
public class Innovation : ScriptableObject
{
    public enum State
    {
        Hidden, // nothing visible 
        Buyable, // name and buybutton visible
        Owned, // Purchased  
    }

    public enum Paramater
    {
        Income,
        PurchaseCost,
        WorkerCost,
        WorkerAmount
    }

    public enum Operator
    {
        Multiply,
        Divide,
        Add,
        Subtract
    }

    public State _initialState = State.Buyable;
    [HideInInspector] public State _state;

    
    [Header("Base Settings")]
    public Resource.Type primaryResource;
    public ulong buyCost;
    public string tooltip;

 
    [Header("Dependent - Origin")] 
    public Generator.Type originGeneratorType;
    public Paramater originParamater;
    public float originMultiplier;
    [Header("Dependent - Affect")] 
    public Operator operate;
    public Generator.Type affectGeneratorType;
    public Paramater affectParameter;
    public float affectMultiplier;


    [Header("Direct Effect")] 
    public Resource.Type resourceType;
    public ulong change;





}
