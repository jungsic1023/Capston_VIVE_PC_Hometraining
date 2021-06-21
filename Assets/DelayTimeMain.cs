using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System;
using System.Text;
using UnityEngine.PlayerLoop;

using System.Linq;
public class DelayTimeMain : MonoBehaviour

{

    [SerializeField] private Text TextTime;              //선언된  UI Text

    private bool Timebool = true;    //코루틴 호출을 위한 논리형 변수

    public int DelayCount = 3;        //화면에 띄어 줄 정수형 변수



    // Update is called once per frame

    void Update()

    {

        //만약 정수형 변수가 0 이라면

        if (DelayCount == 0)

        {

            //코루틴 정지

            StopCoroutine("TimeDelay");

        }

        //만약 논리형 변수가 참 이라면

        if (Timebool)

        {

            //코루틴 호출

            StartCoroutine("TimeDelay");

        }



    }



    //코루틴 호출

    IEnumerator TimeDelay()

    {

        Timebool = false;

        //UI Text에 정수형 변수 정의

        TextTime.text = DelayCount.ToString();



        //1초 후

        yield return new WaitForSeconds(1.0f);

        Debug.Log(DelayCount);

        //정수형 변수 1빼기

        DelayCount--;

        //만약 정수형 변수가 0이면

        if (DelayCount == 0)

        {

            //텍스트 판넬을 사라지게 함

            //GameObject.Find("TimePanel").SetActive(false);

        }

        //다시 논리형 변수 참으로 코루틴 호출

        Timebool = true;

    }



}