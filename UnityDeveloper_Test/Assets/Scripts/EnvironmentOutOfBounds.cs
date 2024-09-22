using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentOutOfBounds : MonoBehaviour
{
    

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.gameObject.SetActive(false);
            StartCoroutine(ResetLevel());
        }
    }

    IEnumerator ResetLevel()
    {
        yield return new WaitForSeconds(1.5f);
        GameManager.Instance.RestartGame();
    }
}
