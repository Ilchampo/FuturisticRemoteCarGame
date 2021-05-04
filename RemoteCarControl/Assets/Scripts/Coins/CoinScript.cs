using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Class Definition
// Class that contains information about the coin
[System.Serializable]
public class CoinAttributes
{
    private bool isDestroyed = false;
    public float rotationSpeed = 1f;

    // Setter and Getter
    public void setIsDestroyed(bool isDestroyed) { this.isDestroyed = isDestroyed; }
    public bool getIsDestroyed() { return this.isDestroyed; }
}
#endregion

// Main class for CoinScript
public class CoinScript : MonoBehaviour
{
    public CoinAttributes attributes;
    // Update is called once per frame
    void Update()
    {
        rotateCoin();
    }

    #region Coin Animation
    // Method that animates the coin rotation
    private void rotateCoin()
    {
        transform.Rotate(0f, 0f, attributes.rotationSpeed);
    }
    // Method that animates the coin pick-up
    private void pickUpCoin()
    {
        for(int i = 0; i < 10; i++)
            transform.position = transform.position + new Vector3(0f, 10f, 0) * Time.deltaTime;
    }
    #endregion

    private IEnumerator waitForAnimation()
    {
        yield return new WaitForSeconds(2);
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Player")
        {
            other.GetComponent<CoinCollector>().coinsCounter++;
            attributes.setIsDestroyed(true);
            pickUpCoin();
            waitForAnimation();
            Destroy(gameObject);
        }
    }

}
