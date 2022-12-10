using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Bubble : MonoBehaviour
{

    public Image baseImage;
    public Image contentImage;
    public ParticleSystem popParticle;
    private BubbleUI _bubbleUI;
    private SpawnAnchor _spawnAnchor;
    private Resource.Type resourceType;
    private int containedValue = 0;
    private int containedHappiness = 0;
    private int containedWorkers = 0;



    private bool _clicked = false;
    private void OnMouseDown()
    {
        if (_clicked == false)
        {
            _clicked = true;
            ClickBubble();
        }
    }

   

    private void ClickBubble()
    {
        _spawnAnchor.SetOccupied(false);
        _bubbleUI.PopBubble(this);
    }

    public void InitializeBubble(BubbleUI bubbleUI, SpawnAnchor spawnAnchor, BubbleBlower blower)
    {
        _bubbleUI = bubbleUI;
        
        // placement
        _spawnAnchor = spawnAnchor;
        transform.position = spawnAnchor.transform.position;
        _spawnAnchor.SetOccupied(true);
        
        // bubble blower settings
        if (blower.colored)
        {
            contentImage.gameObject.SetActive(true);
            contentImage.GetComponent<Image>().color = blower.bubbleColor;
        }
        baseImage.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, blower.bubbleSize);
        contentImage.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, blower.bubbleSize);
        baseImage.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, blower.bubbleSize);
        contentImage.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, blower.bubbleSize);
        GetComponent<BoxCollider2D>().size = new Vector2(blower.bubbleSize, blower.bubbleSize); 
        
        if (blower.spawnsResource)
        {
            resourceType = blower.resource;
            containedValue = Random.Range(blower.resourceMinAmount, blower.resourceMaxAmount);
        } 
        
        if (blower.spawnsWorker)
            containedWorkers = 1;

        StartCoroutine(BubbleFade(blower.persistTime));
    }
    
    
    

    private IEnumerator BubbleFade(float persistTime)
    {
        //todo alpha change or smth
        yield return new WaitForSeconds(persistTime);
        if (_clicked == false)
            DestroyBubble();
    }

    public void DestroyBubble()
    {
        _spawnAnchor.SetOccupied(false);
        Destroy(gameObject);
    }
}
