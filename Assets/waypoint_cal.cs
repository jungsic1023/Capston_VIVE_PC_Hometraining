using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Runtime;

public class waypoint_cal : MonoBehaviour
{
    public int WPNum = 10;
    public GameObject obj;
    public float XL = 0;
    public float ZL = 0;
    // 오브젝트에 대한 초기값 
    public GameObject[] wap;
    string objectname;
    public int count = 0;
    public float[,]  XY = new float[2,40];
    public float roundX;
    public float roundY;
    public float roundA;

    void Start()
    {
        XY[0, 0] = 10f;
        XY[1, 0] = 15f;
        objectname = "waypoint";
        roundA = 3;

        for (int x = 0; x < 15; x++)
        {
            roundX = x / 10f;
            roundY = roundA - roundX;
            XL = roundX * roundX;
            ZL = (roundA - XL);
            
            ZL = (float)System.Math.Sqrt(ZL);
            //XL = XL + roundX;
            //ZL = ZL - 0.1f;
            XY[0, x] = XY[0, x] + XL;
            XY[1, x] = XY[1, x] + ZL;
            count++;
            //Instantiate(obj, new Vector3(XL, 0, ZL), Quaternion.identity);
            objectname = "waypoint"+count;
            newObject(XY[0,x], 0, XY[1,x], objectname);
            Debug.Log("total: " + (XL+ZL));
        }
        //for (int x = 0; x < 10; x++)
        //{
        //    XL--;
        //    ZL--;
        //    count++;
        //    //Instantiate(obj, new Vector3(XL, 0, ZL), Quaternion.identity);
        //    objectname = "waypoint" + count;
        //    newObject(XL, 0, ZL, objectname);
        //}
        //for (int x = 0; x < 10; x++)
        //{
        //    XL--;
        //    ZL++;
        //    count++;
        //    //Instantiate(obj, new Vector3(XL, 0, ZL), Quaternion.identity);
        //    objectname = "waypoint" + count;
        //    newObject(XL, 0, ZL, objectname);
        //}
        //for (int x = 0; x < 10; x++)
        //{
        //    XL++;
        //    ZL++;
        //    count++;
        //    //Instantiate(obj, new Vector3(XL, 0, ZL), Quaternion.identity);
        //    objectname = "waypoint" + count;
        //    newObject(XL, 0, ZL, objectname);
        //}
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void newObject(float x, float y, float z, string objName)
    {
        GameObject Instance = Instantiate(obj, new Vector3(x, 0, z), Quaternion.identity) as GameObject;
        Instance.name = objName;
    }

    //public int countwap()
    //{
    //    wap = GameObject.FindGameObjectsWithTag("")
    //}
}
