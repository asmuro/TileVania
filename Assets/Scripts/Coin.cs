using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    #region SerializedFields

    [SerializeField] int coinValue = 50;
    [SerializeField] AudioClip audioClip;

    #endregion 

    #region Collisions

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other != null && other.GetComponent<Player>() != null)
        {
            AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position);            
            GameSession.Instance.AddScore(this.coinValue);
            Destroy(this.gameObject);
        }
    }

    #endregion
    
}
