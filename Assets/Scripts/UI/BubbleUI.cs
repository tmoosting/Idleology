using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// Generates bubbles to pop


public class BubbleUI : MonoBehaviour
{
 // world map with some spawn anchors
 
   public GameObject bubblePrefab;
   public GameObject bubbleZone;
   public Transform bubblesParent;
   public Transform spawnAnchorParent;

   public int bubbleCreditMin;
   public int bubbleCreditMax;
   public float gameTickPopChance; // 0 to 1
   public float bubblePersistTime;


   private List<SpawnAnchor> _spawnAnchors;
   private void Awake()
   {
      _spawnAnchors = new List<SpawnAnchor>();
      foreach (var spawnAnchor in spawnAnchorParent.GetComponentsInChildren<SpawnAnchor>())
         _spawnAnchors.Add(spawnAnchor);
      foreach (var spawnAnchor in _spawnAnchors)
         spawnAnchor.DisableSprite();
      
   }

   public void BubbleTick()
   {
      if (Random.Range(0f,1f) < gameTickPopChance)
         SpawnBubble();
      
      
   }

   private void SpawnBubble()
   {
      GameObject obj = Instantiate(bubblePrefab, bubblesParent.transform);
      SpawnAnchor anchor = GetRandomUnoccupiedAnchor();
      obj.GetComponent<Bubble>().InitializeBubble(this, anchor);
   }

   private SpawnAnchor GetRandomUnoccupiedAnchor()
   {
      int randomIndex = (int) Random.Range(0, _spawnAnchors.Count - 1);
      SpawnAnchor chosenAnchor = _spawnAnchors[randomIndex];
      for (int i = 0; i < _spawnAnchors.Count-2; i++)
      {
         if (chosenAnchor.occupied == false)
            return chosenAnchor;
         else
         {
            randomIndex = (int) Random.Range(0, _spawnAnchors.Count - 1);
            chosenAnchor = _spawnAnchors[randomIndex];
         }
      }
      return _spawnAnchors[randomIndex];
   }

   public void PopBubble(Bubble bubble)
   {
      SoundController.Instance.PlayBubblePop();
       Destroy(bubble.gameObject);
   }
}
