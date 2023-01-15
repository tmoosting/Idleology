using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UI;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

// Generates bubbles to pop


public class BubbleUI : MonoBehaviour
{
 // world map with some spawn anchors
 
   public GameObject bubblePrefab;
   public GameObject bubbleZone;
   public Transform bubblesParent;
   public Transform spawnAnchorParent;
   public ContentUI contentUI;
 
   public float gameTickPopChance; // 0 to 1
   public float bubblePopSizeMultiplier;
   public float bubblePopTime;

   private List<BubbleBlower> _bubbleBlowers;
   private List<Bubble> _bubbles;
   private List<SpawnAnchor> _spawnAnchors;
   private Dictionary<int, BubbleBlower> blowerChancing;
   private void Awake()
   {
      _bubbles = new List<Bubble>();
      _spawnAnchors = new List<SpawnAnchor>();
      foreach (var spawnAnchor in spawnAnchorParent.GetComponentsInChildren<SpawnAnchor>())
         _spawnAnchors.Add(spawnAnchor);
      foreach (var spawnAnchor in _spawnAnchors)
         spawnAnchor.DisableSprite();
      LoadBubbleBlowers();
      CreateBubbleLowerChanceDict();
   } 

   private void LoadBubbleBlowers()
   {
      _bubbleBlowers = new List<BubbleBlower>();
      string path = "BubbleBlowers/"; 
      Object[] bubbleBlowerObjects = Resources.LoadAll(path);
      BubbleBlower[] blowers = new BubbleBlower[bubbleBlowerObjects.Length];
      bubbleBlowerObjects.CopyTo(blowers, 0);
      _bubbleBlowers = blowers.ToList();  
   }
   private void CreateBubbleLowerChanceDict()
   {
      blowerChancing = new Dictionary<int, BubbleBlower>();
      foreach (var blower in _bubbleBlowers)
         if (blower.spawnFrequency != 0)
         {
            int dictSize = blowerChancing.Keys.Count;
            for (int i = dictSize; i < dictSize+blower.spawnFrequency; i++)
               blowerChancing.Add(i,blower);
         }
   }
   public void BubbleTick()
   { 
      if (contentUI.IsWorldZoneOpen())
         if (Random.Range(0f,1f) < gameTickPopChance)
          SpawnBubble(ChooseBubbleBlower()); 
   }

   private BubbleBlower ChooseBubbleBlower()
   {
      return blowerChancing[Random.Range(0, blowerChancing.Count - 1)];
   }

   private void SpawnBubble(BubbleBlower blower)
   { 
      GameObject obj = Instantiate(blower.bubblePrefab, bubblesParent.transform);
      SpawnAnchor anchor = GetRandomUnoccupiedAnchor();
      obj.GetComponent<Bubble>().InitializeBubble(this, anchor, blower);
      _bubbles.Add(  obj.GetComponent<Bubble>());
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

   public void ClearBubbleFromList(Bubble bubble)
   {
      if (_bubbles.Contains(bubble))
         _bubbles.Remove(bubble);
   }
      
   public void PopBubble(Bubble bubble)
   {
      ClearBubbleFromList(bubble);
       StartCoroutine(BubblePopEffect(bubble ));
   }
    
    
   public void Reopen()
   {
      foreach (var bubble in _bubbles)
         if (bubble != null)
            Destroy(bubble.gameObject);
      _bubbles = new List<Bubble>();
   }

   private IEnumerator BubblePopEffect(Bubble bubble)
   {
      float t = 0;
      while (t < bubblePopTime)
      {
         t += 0.01f;
         if (bubble.contentImage != null)
         {
            bubble.baseImage.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, bubble.contentImage.GetComponent<RectTransform>().sizeDelta.x * bubblePopSizeMultiplier);
            bubble.contentImage.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, bubble.contentImage.GetComponent<RectTransform>().sizeDelta.x * bubblePopSizeMultiplier);
            bubble.baseImage.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, bubble.contentImage.GetComponent<RectTransform>().sizeDelta.y * bubblePopSizeMultiplier);
            bubble.contentImage.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, bubble.contentImage.GetComponent<RectTransform>().sizeDelta.y * bubblePopSizeMultiplier);
         }
         yield return null;
      }
      yield return new WaitForSeconds(0.01f);
      if (bubble != null)
      {
         bubble.baseImage.gameObject.SetActive(false);
         bubble.contentImage.gameObject.SetActive(false);
         SoundController.Instance.PlayBubblePop(); 
         float newScale = (bubble.popParticle.transform.localScale.x *
                           bubble.contentImage.GetComponent<RectTransform>().sizeDelta.x) / 25;
         bubble.popParticle.transform.localScale = new Vector3(newScale, newScale, newScale);
         bubble.popParticle.GetComponent<Renderer>().material.color = bubble.contentImage.color;
         bubble.popParticle.Play();
         yield return new WaitForSeconds(2.5f);
         bubble.DestroyBubble();
      } 
   }

  
}
