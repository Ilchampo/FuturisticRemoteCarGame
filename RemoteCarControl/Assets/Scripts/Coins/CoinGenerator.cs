using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CoinInformation
{
    public Component coinModel;
}

public class CoinGenerator : MonoBehaviour
{
    public List<CoinInformation> numberCoins;

    private void Update() {
        
    }
}
