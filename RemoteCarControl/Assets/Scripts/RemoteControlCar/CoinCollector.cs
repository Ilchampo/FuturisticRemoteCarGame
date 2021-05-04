using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    public int coinsCounter;

    private void Awake() 
    {
        coinsCounter = 0;    
    }

    private void OnGUI() 
    {
        GUI.Label(new Rect(10, 10, 100, 20), "Score : " + coinsCounter);    
    }
}
