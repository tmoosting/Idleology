using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAnchor : MonoBehaviour
{

    public bool occupied = false;



    public void SetOccupied(bool setOccupied)
    {
        occupied = setOccupied;
    }
    

    
    public void DisableSprite()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }
    
    
}
