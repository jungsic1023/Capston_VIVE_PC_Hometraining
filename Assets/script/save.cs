using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class save : MonoBehaviour
{
    public GameObject stagenum;
    public Text kg;
    public static int count;
    public static int count1 = 50;
    // Start is called before the first frame update
    public void Save()
    {
        PlayerPrefs.SetInt("kg", int.Parse(kg.text));
        count = int.Parse(kg.text);
        SceneManager.LoadScene("cap");
        DontDestroyOnLoad(stagenum);
        //Debug.Log(count);
    }

    public void Load()
    {
        kg.text = PlayerPrefs.GetInt("kg").ToString();
       // Debug.Log(count);
    }
}
