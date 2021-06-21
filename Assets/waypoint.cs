using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using System;
using System.IO.Ports;
public class waypoint : MonoBehaviour
{
    public List<Transform> waypoints = new List<Transform>();
    private Transform targetWaypoint;
    private int targetWaypointIndex = 0;
    private float minDistance = 0.1f;
    private int lastWaypointIndex;
    SerialPort m_SerialPort = new SerialPort("COM3", 38400, Parity.None, 8, StopBits.One);
    public float movementSpeed = 3.0f;
    private float rotationSpeed = 3.0f;
    string m_Data = null;
    public GameObject[] wap;
    public int wapnumber;
    float x;
    int count = 0;
    float x1;
    float sttsp;
    public float sp = 0;
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
        x = 0;
        
    }

    // Update is called once per frame
    void Update()
    {

        try
        {
            if (m_SerialPort.IsOpen)
            {

                m_Data = m_SerialPort.ReadLine();
                m_SerialPort.ReadTimeout = 30;
                Debug.Log(m_Data);

                string text = m_Data;
                string[] split_text = new string[8];
                split_text = text.Split(' ');

                int a = int.Parse(split_text[7]);
                Debug.Log("센서값 :" + a);

                if (count == 1)
                {

                }
                float movementStep = movementSpeed * Time.deltaTime;
                float rotationStep = rotationSpeed * Time.deltaTime;
                if (count == 0)
                {
                    if (a == 0) //적외선센서 a는 0// 
                    {
                        x = x + 1;
                        
                        if (x >= 1)
                        {
                            Vector3 directionToTarget = targetWaypoint.position - transform.position;
                            Quaternion rotationToTarget = Quaternion.LookRotation(directionToTarget);

                            transform.rotation = Quaternion.Slerp(transform.rotation, rotationToTarget, rotationStep);


                            float distance = Vector3.Distance(transform.position, targetWaypoint.position);
                            CheckDistanceToWaypoint(distance);
                            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, movementStep);

                        }
                    }
                }
                //if(count)
                else
                {
                    
                    Vector3 directionToTarget = targetWaypoint.position - transform.position;
                    Quaternion rotationToTarget = Quaternion.LookRotation(directionToTarget);

                    transform.rotation = Quaternion.Slerp(transform.rotation, rotationToTarget, rotationStep);


                    float distance = Vector3.Distance(transform.position, targetWaypoint.position);
                    CheckDistanceToWaypoint(distance);
                    transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, movementStep);
                    x = x + 0.001f;

                    if (x < 1)
                    {
                        sp = 2;
                        x = 1;
                    }
                    else if (x < 2)
                    {
                        sp = 2;
                    }
                    else if (x <3)
                    {
                        sp = 5;
                    }
                    else if (x < 4)
                    {
                        sp = 5;
                    }

                    movementSpeed = sp;
                    //if (movementSpeed > 10f)
                    //{
                    //    sttsp = movementSpeed;
                    //}

                    //movementSpeed = x;
                    //sttsp = movementSpeed;
                    //x1 = x + 0.005f;
                    //if (movementSpeed > 10f)
                    //{
                    //    sttsp = 10f;
                    //}
                    //else if (movementSpeed > 7f)
                    //{
                    //    sttsp = 7f;
                    //}
                    //else if (movementSpeed > 5f)
                    //{
                    //    sttsp = 5f;
                    //}
                    //else if (movementSpeed > 3f)
                    //{
                    //    sttsp = 3f;
                    //}

                }
                
                   
                





              



            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }


    }

    void CheckDistanceToWaypoint(float currentDistance)
    {
        if(currentDistance <= minDistance)
        {
            targetWaypointIndex++;
            UpdateTargetWaypoint();
        }
    }

    void UpdateTargetWaypoint()
    {
        if(targetWaypointIndex > lastWaypointIndex)
        {
            targetWaypointIndex = 0;
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
}
