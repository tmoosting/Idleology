using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class ClickDetector : MonoBehaviour
{
    private void OnMouseUp()
    {
        if (gameObject.name == "Happiness")
            FindObjectOfType<ContentUI>().OpenWorldSection();
    }
}
