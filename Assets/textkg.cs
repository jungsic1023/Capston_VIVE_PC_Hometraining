using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System;
using System.Text;
using UnityEngine.PlayerLoop;

using System.Linq;

public class textkg : MonoBehaviour
{
   

    public Text countText;
     float kg = 0;
    public Text kcaltext, speed;
    public int spsp = 0;
    [SerializeField] float timeStart;
    [SerializeField] float timeStart2;
    [SerializeField] Text timeText;
    float m_fSpeed = 5.0f;
    bool timeActive = false;
   
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("start1 " + save.count);
        countText.text = "kg:" + save.count;

        timeText.text = timeStart.ToString("F2");
      

    }

    // Update is called once per frame
    void Update()
    {

        int rna = UnityEngine.Random.Range(1, 7);

        float fHorizontal = Input.GetAxis("Horizontal");
        float fVertical = Input.GetAxis("Vertical");

        transform.Translate(Vector3.right * Time.deltaTime * m_fSpeed * fHorizontal, Space.World);
        transform.Translate(Vector3.forward * Time.deltaTime * m_fSpeed * fVertical, Space.World);


        // Debug.Log(rna);
        if (rna==6)
        {
            timeActive = !timeActive;
            StartTime();//시간 재기시작
           
        }

        if (rna > 0 && rna < 5)
        {
           
            StartTime();//시간 안재기시작
          
        }
        spsp = 20;
        speed.text = "speed : " + spsp;

    }

    void StartTime()
    {
       
        if(timeActive)
        {
            timeStart += Time.deltaTime;
            timeStart2 = timeStart / 60;
            Debug.Log(timeStart);
           timeText.text = timeStart2.ToString("F2") + "분";

            double ran2 = 0.0939;

            double kcal = kg * ran2 * timeStart2;

            
            kcaltext.text = "kcal:" + kcal.ToString("N2");
        }
    }
}
