using UnityEngine;
using System;
using System.IO.Ports;

public class ssss : MonoBehaviour
{

    public int q = 0;
    public float sp = 0;
    public float turnSpeed = 540f;
    public int x = 0;
    public int y = 0;
    string deviceName;

    string received_message;

    SerialPort m_SerialPort = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);
    string m_Data = null;

    void Start()
    {
        Debug.Log("start1 " + save.count);
        m_SerialPort.Open();

    }

    private void Update()
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

                if (a == 0) //적외선센서 a는 0// 
                {
                    x = x + 1;
                    y = 0;
                    if (x == 1)
                    {
                        sp = 3;
                        x = 1;
                        y = 0;

                    }
                    else if (x == 2)
                    {
                        sp = 3;
                        y = 0;

                    }
                    else if (x == 3)
                    {
                        sp = 3;
                        y = 0;

                    }
                    else if (x == 4)
                    {
                        sp = 3;
                        y = 0;

                    }
                    else if (x == 5)
                    {
                        sp = 6;
                        y = 0;

                    }
                    else if (x == 6)
                    {
                        sp = 6;
                        y = 0;

                    }
                    else if (x == 7)
                    {
                        sp = 6;
                        y = 0;

                    }
                    else if (x == 8)
                    {
                        sp = 6;
                        y = 0;

                    }
                    else if (x == 9)
                    {
                        sp = 9;
                        y = 0;

                    }
                    else if (x == 10)
                    {
                        sp = 9;
                        y = 0;

                    }
                    else if (x == 11)
                    {
                        sp = 9;
                        y = 0;

                    }
                    else if (x == 12)
                    {
                        sp = 9;
                        y = 0;

                    }
                    else if (x > 12)
                    {
                        x = 12;
                        sp = 9;
                        y = 0;
                    }
                }

                if (a == 1)
                {
                    y = y + 1;
                    if (x == 0)
                    {
                        y = 0;

                    }
                    if (x == 1)
                    {
                        if (y == 80)
                        {
                            sp = 0;
                            x = 0;
                            y = 0;
                        }


                    }
                    else if (x == 2)
                    {
                        if (y == 80)
                        {
                            sp = 0;
                            x = 0;
                            y = 0;
                        }

                    }
                    else if (x == 3)
                    {
                        if (y == 80)
                        {
                            sp = 0;
                            x = 1;
                            y = 0;
                        }

                    }
                    else if (x == 4)
                    {
                        if (y == 80)
                        {
                            sp = 0;
                            x = 1;
                            y = 0;
                        }

                    }
                    else if (x == 5)
                    {
                        if (y == 80)
                        {
                            sp = 3;
                            x = 1;
                            y = 0;
                        }

                    }
                    else if (x == 6)
                    {
                        if (y == 80)
                        {
                            sp = 3;
                            x = 2;
                            y = 0;
                        }

                    }
                    else if (x == 7)
                    {
                        if (y == 80)
                        {
                            sp = 3;
                            x = 2;
                            y = 0;
                        }

                    }
                    else if (x == 8)
                    {
                        if (y == 80)
                        {
                            sp = 3;
                            x = 3;
                            y = 0;
                        }

                    }
                    else if (x == 9)
                    {
                        if (y == 80)
                        {
                            sp = 6;
                            x = 3;
                            y = 0;
                        }

                    }
                    else if (x == 10)
                    {
                        if (y == 80)
                        {
                            sp = 6;
                            x = 4;
                            y = 0;
                        }

                    }
                    else if (x == 11)
                    {
                        if (y == 80)
                        {
                            sp = 6;

                            x = 4;
                            y = 0;
                        }

                    }
                    else if (x == 12)
                    {
                        if (y == 80)
                        {
                            sp = 6;
                            x = 5;
                            y = 0;
                        }

                    }

                }


                int br = int.Parse(split_text[0]);
                if (br == 1) //브레이크//
                {
                    sp = 0;
                }

                Debug.Log(q);
                Debug.Log("x = " + x);
                Debug.Log("y = " + y);

                float fMove = Time.deltaTime * sp;
                transform.Translate(Vector3.forward * fMove);
                //transform.Translate(Vector3.forward * fMove);

                int b = int.Parse(split_text[1]);

                if (b > 300 && b <= 2000)
                {
                    transform.Rotate(0, 0, 0);
                }
                else if (b > 2000 && b <= 3000)
                {
                    transform.Rotate(Vector3.up * Time.deltaTime * 20);
                }
                else if (b > 3000 && b <= 4000)
                {
                    transform.Rotate(Vector3.up * Time.deltaTime * 40);
                }
                else if (b > 4000)
                {
                    transform.Rotate(Vector3.up * Time.deltaTime * 60);
                } //오른쪽





                else if (b > 0 && b <= 300)
                {
                    transform.Rotate(Vector3.down * Time.deltaTime * 20);
                }
                else if (b > -2000 && b <= 0)
                {
                    transform.Rotate(Vector3.down * Time.deltaTime * 40);
                }
                else if (b <= -2000)
                {
                    transform.Rotate(Vector3.down * Time.deltaTime * 60);
                }//왼쪽

                Debug.Log("b = " + b);

                //transform.Rotate(0f, -10 * Time.deltaTime, 0f);

                //transform.Translate(Vector3.forward * fMove);
                // float h = Input.GetAxis("Horizontal");
                //float v = Input.GetAxis("Vertical");

                //transform.Translate (0f, 0f, h * moveSpeed * Time.deltaTime);

                //Move
                //transform.Translate(0f, 0f, v * moveSpeed * Time.deltaTime);

                //Turn
                //  transform.Rotate(0f, h * turnSpeed * Time.deltaTime, 0f);





            }
        }

        catch (Exception e)
        {
            Debug.Log(e);
        }
    }


    void restrcitX()
    {
        if (x < 0)
        {
            x = 0;
        }
    }



    void OnApplicationQuit()
    {
        m_SerialPort.Close();
    }




}