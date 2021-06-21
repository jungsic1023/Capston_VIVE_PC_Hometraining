using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonevent : MonoBehaviour
{
    GameObject player;
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

     void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player)
        {
            SceneManager.LoadScene("SampleScene");
            Destroy(player);
        }
    }
}
