using UnityEngine;
using System;
using System.IO.Ports;

public class sdfsadf : MonoBehaviour
{
    SerialPort m_SerialPort = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);
    string m_Data = null;

    void Start()
    {
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
            }
        }

        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    void OnApplicationQuit()
    {
        m_SerialPort.Close();
    }
}