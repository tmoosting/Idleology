using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

// settings for a specific type of bubble spawn

[CreateAssetMenu]
public class BubbleBlower : ScriptableObject
{
    public GameObject bubblePrefab;
    public bool colored;
    public Color bubbleColor;
    [Range(10,100)]
    public float bubbleSize = 25;
    public bool spawnsResource;
    public Resource.Type resource;
    public int resourceMinAmount;
    public int resourceMaxAmount; 
    public bool spawnsWorker;
    public bool spawnsDialog;
    [Range(0,100)][Tooltip("Relative rarity, between 0 and 100")]
    public int spawnFrequency;  
    [Range(0,60)]
    public float persistTime;
//    public Sprite sprite;

}
