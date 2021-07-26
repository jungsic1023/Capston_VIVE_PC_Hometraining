using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using System;
using System.IO.Ports;
using UnityEngine.SceneManagement;
using System.Text;
using UnityEngine;
using System;
using System.IO.Ports;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class run : MonoBehaviour
{
    public List<Transform> waypoints = new List<Transform>();
    private Transform targetWaypoint;
    private int targetWaypointIndex = 0;
    private float minDistance = 0.1f;
    private int lastWaypointIndex;
    SerialPort m_SerialPort = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);
    public float movementSpeed = 0f;
    private float rotationSpeed = 3.0f;
    string m_Data = null;
    public GameObject[] wap;
    public int wapnumber;
    public int x = 0;
    public int y = 0;
    bool IsPause;
    GameObject player;
    public GameObject canbas;
    public bool check = false;
    public GameObject Target1;
    public GameObject Target2;
    public GameObject Target3;
    public GameObject fake1;
    public GameObject fake2;
    public GameObject AI1;
    public GameObject AI2;
   
    // Start is called before the first frame update
    void Start()
    {

        Debug.Log("start1 " + save.count);
        
        m_SerialPort.Open();
        lastWaypointIndex = waypoints.Count - 1;
        targetWaypoint = waypoints[targetWaypointIndex];
        //for(int i = 0; i < 50; i++)
        //{
        //    waypoints[i] = wap[i];
        //}
       
    }
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    // Update is called once per frame
    void Update()
    {

        try
        {
            if (m_SerialPort.IsOpen)
            {

                Invoke("v3", 1);
                Invoke("v2", 2);
                Invoke("v1", 3);
                Invoke("v4", 3);
                m_Data = m_SerialPort.ReadLine();
                m_SerialPort.ReadTimeout = 30;
                Debug.Log(m_Data);

                string text = m_Data;
                string[] split_text = new string[8];
                split_text = text.Split(' ');

                int a = int.Parse(split_text[7]);
                Debug.Log("센서값 :" + a);

                //if (count == 0)
                //{

                //}

                float rotationStep = rotationSpeed * Time.deltaTime;
                Vector3 directionToTarget = targetWaypoint.position - transform.position;
                Quaternion rotationToTarget = Quaternion.LookRotation(directionToTarget);

                transform.rotation = Quaternion.Slerp(transform.rotation, rotationToTarget, rotationStep);


                float distance = Vector3.Distance(transform.position, targetWaypoint.position);
                CheckDistanceToWaypoint(distance);


                if (a == 0) //The IR Seonsor 'a' is 0// 
                {

                    x = x + 1;
                    y = 0;
                    if (x == 1)
                    {
                        movementSpeed = 1.0f;


                        x = 1;
                        y = 0;



                    }
                    else if (x == 2)
                    {
                        movementSpeed = 1;


                        y = 0;


                    }
                    else if (x == 3)
                    {
                        movementSpeed = 1;


                        y = 0;


                    }
                    else if (x == 4)
                    {
                        movementSpeed = 1;


                        y = 0;


                    }
                    else if (x == 5)
                    {
                        movementSpeed = 2f;


                        y = 0;

                    }
                    else if (x == 6)
                    {
                        movementSpeed = 2f;


                        y = 0;


                    }
                    else if (x == 7)
                    {
                        movementSpeed = 2f;


                        y = 0;
                    }
                    else if (x == 8)
                    {
                        movementSpeed = 2f;


                        y = 0;

                    }
                    else if (x == 9)
                    {
                        movementSpeed = 3f;

                        y = 0;

                    }
                    else if (x == 10)
                    {
                        movementSpeed = 3f;

                        y = 0;
                    }
                    else if (x == 11)
                    {
                        movementSpeed = 3f;

                        y = 0;

                    }
                    else if (x == 12)
                    {
                        movementSpeed = 4f;

                        y = 0;

                    }
                    else if (x == 13)
                    {
                        movementSpeed = 4f;

                        y = 0;

                    }
                    else if (x == 14)
                    {
                        movementSpeed = 4f;

                        y = 0;

                    }
                    else if (x == 15)
                    {
                        movementSpeed = 5;

                        y = 0;

                    }
                    else if (x == 16)
                    {
                        movementSpeed = 5;

                        y = 0;

                    }
                    else if (x > 16)
                    {
                        x = 16;

                        y = 0;

                    }
                }

                if (a == 1)
                {
                    y = y + 1;
                    if (x == 0)
                    {
                        y = 0;
                        movementSpeed = 0;

                    }
                    if (x == 1)
                    {
                        if (y == 80)
                        {
                            movementSpeed = 0;

                            x = 0;
                            y = 0;

                        }


                    }
                    else if (x == 2)
                    {
                        if (y == 80)
                        {
                            movementSpeed = 0;

                            x = 0;
                            y = 0;

                        }

                    }
                    else if (x == 3)
                    {
                        if (y == 80)
                        {
                            movementSpeed = 0;

                            x = 1;
                            y = 0;

                        }

                    }
                    else if (x == 4)
                    {
                        if (y == 80)
                        {
                            movementSpeed = 0;

                            x = 1;
                            y = 0;

                        }

                    }
                    else if (x == 5)
                    {
                        if (y == 80)
                        {
                            movementSpeed = 1f;

                            x = 1;
                            y = 0;

                        }

                    }
                    else if (x == 6)
                    {
                        if (y == 80)
                        {
                            movementSpeed = 1f;

                            x = 2;
                            y = 0;

                        }

                    }
                    else if (x == 7)
                    {
                        if (y == 80)
                        {
                            movementSpeed = 1f;

                            x = 2;
                            y = 0;

                        }

                    }
                    else if (x == 8)
                    {
                        if (y == 80)
                        {
                            movementSpeed = 1f;

                            x = 3;
                            y = 0;

                        }

                    }
                    else if (x == 9)
                    {
                        if (y == 80)
                        {
                            movementSpeed = 2f;

                            x = 3;
                            y = 0;

                        }

                    }
                    else if (x == 10)
                    {
                        if (y == 80)
                        {
                            movementSpeed = 2f;

                            x = 4;
                            y = 0;

                        }

                    }
                    else if (x == 11)
                    {
                        if (y == 80)
                        {
                            movementSpeed = 2f;

                            x = 4;
                            y = 0;

                        }

                    }
                    else if (x == 12)
                    {
                        if (y == 80)
                        {
                            movementSpeed = 3f;


                            x = 5;
                            y = 0;

                        }

                    }
                    else if (x == 13)
                    {
                        if (y == 80)
                        {
                            movementSpeed = 3f;


                            x = 5;
                            y = 0;

                        }

                    }
                    else if (x == 14)
                    {
                        if (y == 80)
                        {
                            movementSpeed = 3f;


                            x = 5;
                            y = 0;

                        }

                    }
                    else if (x == 15)
                    {
                        if (y == 80)
                        {
                            movementSpeed = 3f;


                            x = 5;
                            y = 0;

                        }

                    }
                    else if (x == 16)
                    {
                        if (y == 80)
                        {
                            movementSpeed = 3f;


                            x = 5;
                            y = 0;

                        }

                    }

                }


                int br = int.Parse(split_text[0]);
                if (br == 1) //Bycle Break//
                {
                    movementSpeed = 0;


                }


                Debug.Log("x = " + x);
                Debug.Log("y = " + y);
                float movementStep = movementSpeed * 3 * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, movementStep);
                //transform.Translate(Vector3.forward * fMove);

                int b = int.Parse(split_text[1]);








            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }


    }

    void CheckDistanceToWaypoint(float currentDistance)
    {
        if (currentDistance <= minDistance)
        {
            targetWaypointIndex++;
            UpdateTargetWaypoint();
        }
    }

    void UpdateTargetWaypoint()
    {
        int b = 0;
        if (targetWaypointIndex > lastWaypointIndex)
        {
            targetWaypointIndex = 0;
            b++;
            if(b==1)
            {
                canbas.SetActive(true);
                Debug.Log("END");
                Time.timeScale = 0;
                IsPause = true;
                Debug.Log("invoke1");
                Invoke ("sceneChange", 0);
                //sceneChange();

                return;


         
            }

        }
        targetWaypoint = waypoints[targetWaypointIndex];
    }

    //public int countwap()
    //{
    //    wap = GameObject.FindGameObjectsWithTag("aypoint");
    //    wapnumber = 0;
    //    for(int i=0; i < wap.Length; i++)
    //    {
    //        if (wap[i].GetComponent().isStanding)
    //        {
    //            wapnumber++;
    //        }
    //    }
    //    return wapnumber;

    //}
 
    void sceneChange()
    {
        Debug.Log("invoke2");
        
        SceneManager.LoadScene("cap2");
        //Destroy(player);

    }
    void v3()
    {
        Target3.SetActive(false);
       

    }
    void v2()
    {
        
        Target2.SetActive(false);
      

    }
    void v1()
    {
        
        Target1.SetActive(false);
       

    }
    void v4()
    {
       
        fake1.SetActive(false);
        fake2.SetActive(false);
        AI1.SetActive(true);
        AI2.SetActive(true);

    }
}
