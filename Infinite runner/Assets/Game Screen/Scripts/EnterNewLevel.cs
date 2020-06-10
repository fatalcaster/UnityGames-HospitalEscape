using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterNewLevel : MonoBehaviour
{
    #region New Level Generator


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            
            Spawner.GenerateNewLevel();
            CountdownTimer.resetTimer();
            Debug.Log("New Level");
            Destroy(gameObject);
        }
    }
    #endregion

}
