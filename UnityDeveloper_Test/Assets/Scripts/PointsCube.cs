using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsCube : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Ct");
            this.gameObject.SetActive(false);
            GameManager.Instance.Score++;
        }
    }

   
}
