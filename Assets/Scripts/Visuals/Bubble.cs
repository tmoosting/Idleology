using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bubble : MonoBehaviour
{

    private BubbleUI _bubbleUI;
    private SpawnAnchor _spawnAnchor;
    private int containedValue = 0;
    
    private void OnMouseDown()
    {
        ClickBubble();
    }

    private void ClickBubble()
    {
        _spawnAnchor.SetOccupied(false);
        _bubbleUI.PopBubble(this);
    }

    public void InitializeBubble(BubbleUI bubbleUI, SpawnAnchor spawnAnchor)
    {
        _bubbleUI = bubbleUI;
        _spawnAnchor = spawnAnchor;
        transform.position = spawnAnchor.transform.position;
        _spawnAnchor.SetOccupied(true);
        containedValue = Random.Range(bubbleUI.bubbleCreditMin, bubbleUI.bubbleCreditMax);
        StartCoroutine(BubbleFade(bubbleUI.bubblePersistTime));
    }

    private IEnumerator BubbleFade(float persistTime)
    {
        //todo alpha change or smth
        yield return new WaitForSeconds(persistTime);
        _spawnAnchor.SetOccupied(false);
        Destroy(gameObject);
    }
}
