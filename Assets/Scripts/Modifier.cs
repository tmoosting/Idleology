using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(fileName = "Modifier", menuName = "ScriptableObjects/Modifier", order = 3)]
public class Modifier : ScriptableObject
{
    public enum Type
    {
        InfluenceOne, 
        InfluenceTwo  
    }


        
    public Type _type;
    public Resource.Type _resource;  
    public int _purchaseCost;
    public bool _requiresGenerator;
    public Generator.Type _requiredGenerator;
    public bool _requiresModifier;
    public Modifier.Type _requiredModifier;
    public int _requiredWorkers;
    
    
    public Type GetModifierType()
    {
        return _type;
    }

}