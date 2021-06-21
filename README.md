캡스톤 가상환경 홈트레이닝 시스템
===================================   
### 1. 아두이노 시리얼 연결
>#### 1-1 Arduino로 사용하는 센서
>#### 1-2 Arduino IDE에서 Uno보드에 센서들을 연결하여 Unity로 전송하는 코드
>#### 1-3 Unity에서 Arduino를 시리얼포트로 받는 방법
### 2. Vive Unity 연결
>#### 2-1 Unity 연결 및 시작
>#### 2-2 컨트롤러 레이저 및 
### 3. Unity 개발 
>#### 3-1 자유로운 맵 주행
>#### 3-2 경기장 맵 주행 

<hr/>   

## 1. 아두이노 시리얼 연결

### 1-1 Arduino로 사용하는 센서
1. 자이로 가속도 센서 -> 자전거의 방향제어(좌 우)
2. 홀 센서 -> 자전거의 브레이크 제어
3. 적외선 센서 -> 자전거의 속도제어
<hr/>  

### 1-2 Arduino IDE에서 Uno보드에 센서들을 연결하여 Unity로 전송하는 코드

```cs
#include "SoftwareSerial.h"
#include "Wire.h"
SoftwareSerial mySerial(7, 8); // RX, TX //블루투스 센서
const int MPU_addr=0x68;  // I2C address of the MPU-6050
int16_t AcX,AcY,AcZ,Tmp,GyX,GyY,GyZ; //가속도 자이로센서
const int hallPin = 4; //홀센서
int infrared  = 3; //적외선센서
// LED는 디지털 6번핀으로 설정합니다.
int led = 6;
String sensorReading;   
String a = "2";
void setup() {
  Wire.begin();
  Wire.beginTransmission(MPU_addr);
  Wire.write(0x6B);  // PWR_MGMT_1 register
  Wire.write(0);     // set to zero (wakes up the MPU-6050)
  Wire.endTransmission(true);
 pinMode(infrared, INPUT);
  // LED 핀을 OUTPUT으로 설정합니다.
  pinMode(led, OUTPUT);
  Serial.begin(9600);
  mySerial.begin(9600);
}
void loop(){
  sensorReading = digitalRead(hallPin); 
   int state = digitalRead(infrared);
  Wire.beginTransmission(MPU_addr);
  Wire.write(0x3B);  // starting with register 0x3B (ACCEL_XOUT_H)
  Wire.endTransmission(false);
  Wire.requestFrom(MPU_addr,14,true);  // request a total of 14 registers
  AcX=Wire.read()<<8|Wire.read();  // 0x3B (ACCEL_XOUT_H) & 0x3C (ACCEL_XOUT_L)    
  AcY=Wire.read()<<8|Wire.read();  // 0x3D (ACCEL_YOUT_H) & 0x3E (ACCEL_YOUT_L)
  AcZ=Wire.read()<<8|Wire.read();  // 0x3F (ACCEL_ZOUT_H) & 0x40 (ACCEL_ZOUT_L)
  //Tmp=Wire.read()<<8|Wire.read();  // 0x41 (TEMP_OUT_H) & 0x42 (TEMP_OUT_L)
  GyX=Wire.read()<<8|Wire.read();  // 0x43 (GYRO_XOUT_H) & 0x44 (GYRO_XOUT_L)
  GyY=Wire.read()<<8|Wire.read();  // 0x45 (GYRO_YOUT_H) & 0x46 (GYRO_YOUT_L)
  GyZ=Wire.read()<<8|Wire.read();  // 0x47 (GYRO_ZOUT_H) & 0x48 (GYRO_ZOUT_L)
if(state == 0){
    // LED를 켜지도록 합니다.
    digitalWrite(led, HIGH);
    // 경보 메세지를 시리얼 모니터에 출력합니다.
  }
  /// 측정된 센서값이 0 이외(감지되지 않음) 이면 아래 블록을 실행합니다.
  else{
    // LED를 꺼지도록 합니다.
    digitalWrite(led, LOW);
    // 안전 메세지를 시리얼 모니터에 출력합니다.
     }
  if (mySerial.available())
    Serial.write(mySerial.read());
  if (Serial.available())
    Serial.println(sensorReading+" "+AcY+" "+AcX+" "+AcZ+" "+GyX+" "+GyY+" "+GyZ+" "+state);
  mySerial.println(sensorReading+" "+AcY+" "+AcX+" "+AcZ+" "+GyX+" "+GyY+" "+GyZ+" "+state);
  Serial.println(sensorReading+" "+AcY+" "+AcX+" "+AcZ+" "+GyX+" "+GyY+" "+GyZ+" "+state);
}
```
<hr/>   

### 1-3 Unity에서 Arduino를 시리얼포트로 받는 방법

![unity ](https://user-images.githubusercontent.com/62869017/122768881-fdf1ff80-d2de-11eb-8ea9-f85d17b2a3a4.png)

unity의 player 설정에서 Api Compatibility Level을 .Net 4.x로 한다.

```cs
using System.IO.Ports;

public class serialports : MonoBehaviour
{
    SerialPort m_SerialPort = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);
    
    private void Update()
    {
        try
        {
            if (m_SerialPort.IsOpen)
            {
                m_Data = m_SerialPort.ReadLine();
                m_SerialPort.ReadTimeout = 30;
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
}
```
사용할 스크립트 상단에 IO포트를 적어준다.
반드시 Arduino IDE에서 포트를 확인 후 동일하게 맞춰줘야하며 통신 속도 역시 동일해야한다.
또한 Arduino에서 시리얼 모니터를 켤 시 유니티에서는 에러가 뜨기에 확인을 하고 싶다면 아두이노의 시리얼 모니터를 종료해야한다.

<hr/>   

## 2. Vive 연결

### 2-1 Unity 연결 및 시작
Vive를 연결하기 위해선 Unity의 Asset Store에서 Steam VR을 추가하여야한다.
추가 후 Unity의 Hierarchy에 Steam VR의 Camera Rig와 SteamVR 프리팹을 추가하여야 한다.

![Steam VR](https://user-images.githubusercontent.com/62869017/122770997-f92e4b00-d2e0-11eb-8717-088ed282aed9.png)

### 2-2 컨트롤러에 레이저를 부착하여 버튼을 클릭하는 방법

![image](https://user-images.githubusercontent.com/62869017/122771455-68a43a80-d2e1-11eb-9b2e-66612014709e.png)

컨트롤러 밑에 게임오브젝트를 두개 생성하여 pointer, sphere이라 한다.
또한 VRinput이라는 게임오브젝트 또한 생성한다.

```cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.EventSystems;
public class VRInput : BaseInputModule
{
    public Camera m_camera;
   
    public SteamVR_Input_Sources m_TargetSouce;
    
    public SteamVR_Action_Boolean m_click;
   
    private GameObject m_CurrentObject = null;
    private PointerEventData m_Data = null;

    protected override void Awake()
    {
        base.Awake();
        m_Data = new PointerEventData(eventSystem);
    }

    public override void Process()
    {
        m_Data.Reset();
        m_Data.position = new Vector2(m_camera.pixelWidth / 2, m_camera.pixelHeight / 2);

        eventSystem.RaycastAll(m_Data, m_RaycastResultCache);
        m_Data.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);
        m_CurrentObject = m_Data.pointerCurrentRaycast.gameObject;
        m_RaycastResultCache.Clear();

        HandlePointerExitAndEnter(m_Data, m_CurrentObject);

        if (m_click.GetStateDown(m_TargetSouce))
            ProcessPress(m_Data);

        if (m_click.GetStateUp(m_TargetSouce))
            ProcessRelease(m_Data);
    }

    public PointerEventData GetData()
    {
        return m_Data;
    }
    private void ProcessPress(PointerEventData data)
    {
        data.pointerPressRaycast = data.pointerCurrentRaycast;

        GameObject newPointerPress = ExecuteEvents.ExecuteHierarchy(m_CurrentObject, data, ExecuteEvents.pointerDownHandler);

        if (newPointerPress == null)
            newPointerPress = ExecuteEvents.GetEventHandler<IPointerClickHandler>(m_CurrentObject);

        data.pressPosition = data.position;
        data.pointerPress = newPointerPress;
        data.rawPointerPress = m_CurrentObject;
    }

    private void ProcessRelease(PointerEventData data)
    {
        ExecuteEvents.Execute(data.pointerPress, data, ExecuteEvents.pointerUpHandler);

        GameObject pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(m_CurrentObject);

        if(data.pointerPress == pointerUpHandler)
        {
            ExecuteEvents.Execute(data.pointerPress, data, ExecuteEvents.pointerClickHandler);
        }

        eventSystem.SetSelectedGameObject(null);

        data.pressPosition = Vector2.zero;
        data.pointerPress = null;
        data.rawPointerPress = null;
    }
}
```
VRinput 오브젝트 코드

![VRinput](https://user-images.githubusercontent.com/62869017/122771665-9b4e3300-d2e1-11eb-8982-aa633a95ce7a.png)


```cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;
public class pointer : MonoBehaviour
{
    public float mlength = 5.0f;
    public GameObject Dot;
    public VRInput m_input;

    private LineRenderer line = null;
    private void Awake()
    {
        line = GetComponent<LineRenderer>();
    }


    // Update is called once per frame
   private void Update()
    {
        UpdateLine();
    }
    private void UpdateLine()
    {


        float targetLength = mlength;

        RaycastHit hit = CreateRaycast(targetLength);

        Vector3 endPosittion = transform.position + (transform.forward * targetLength);

        if (hit.collider != null)
            endPosittion = hit.point;

        Dot.transform.position = endPosittion;
        line.SetPosition(0, transform.position);
        line.SetPosition(1, endPosittion);

    }
    private RaycastHit CreateRaycast(float lenght)
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        Physics.Raycast(ray, out hit, mlength);
        return hit;
    }
}
```

pointer 코드

![pointer 1](https://user-images.githubusercontent.com/62869017/122771882-cf295880-d2e1-11eb-8887-82993b0fdb4b.png)

![pointer 2](https://user-images.githubusercontent.com/62869017/122771940-dd777480-d2e1-11eb-9af0-60a61dde687c.png)


버튼이 있는 Canvas에서는 카메라를 포인터로 지정해야한다.

![Canvas](https://user-images.githubusercontent.com/62869017/122772323-3941fd80-d2e2-11eb-8dac-be73c074deb1.png)

<hr/>  

# 3. Unity 개발 

### 3-1 자유로운 맵 주행

![몸무게 입력 1](https://user-images.githubusercontent.com/62869017/122772715-9ccc2b00-d2e2-11eb-8e57-54bb9bbadaf1.png)

컨트롤러를 이용하여 클릭한다.

![몸무게 입력 2](https://user-images.githubusercontent.com/62869017/122772761-a8b7ed00-d2e2-11eb-8c2d-f6bef6ac290d.png)

몸무게를 클릭하여 입력하면 다음 씬으로 넘어가게 되며 저장된 몸무게는 시간, 주행 거리와 함께 칼로리 계산에 사용된다.

![주행](https://user-images.githubusercontent.com/62869017/122772658-8f16a580-d2e2-11eb-813f-d3775e4e057d.png)

핸들에 주행 거리, 운동 시간, 소모 칼로리가 보이게 된다.

![Player](https://user-images.githubusercontent.com/62869017/122773076-fb91a480-d2e2-11eb-9d54-8dd3c1900ff5.png)

Player에 Serial Port라는 스크립트를 넣어 각종 text들과 같이 작동되도록 한다.

```cs
using UnityEngine;
using System;
using System.IO.Ports;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text;
using UnityEngine.PlayerLoop;
using System.Linq;


public class serialports : MonoBehaviour
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
        Debug.Log("start1 " + save.count);
        countText.text = "kg:" + save.count;

        timeText.text = timeStart.ToString("F2");
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
                    timeActive = !timeActive;
                    StartTime();//시간 재기시작
                    x = x + 1;
                    y = 0;
                    if (x == 1)
                    {
                        sp = 5;
                        x = 1;
                        y = 0;

                    }
                    else if (x == 2)
                    {
                        sp = 5;
                        y = 0;

                    }
                    else if (x == 3)
                    {
                        sp = 5;
                        y = 0;

                    }
                    else if (x == 4)
                    {
                        sp = 5;
                        y = 0;

                    }
                    else if (x == 5)
                    {
                        sp = 9;
                        y = 0;

                    }
                    else if (x == 6)
                    {
                        sp = 9;
                        y = 0;

                    }
                    else if (x == 7)
                    {
                        sp = 9;
                        y = 0;

                    }
                    else if (x == 8)
                    {
                        sp = 9;
                        y = 0;

                    }
                    else if (x == 9)
                    {
                        sp = 12;
                        y = 0;

                    }
                    else if (x == 10)
                    {
                        sp = 12;
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
                            sp = 5;
                            x = 1;
                            y = 0;
                        }

                    }
                    else if (x == 6)
                    {
                        if (y == 80)
                        {
                            sp = 5;
                            x = 2;
                            y = 0;
                        }

                    }
                    else if (x == 7)
                    {
                        if (y == 80)
                        {
                            sp = 5;
                            x = 2;
                            y = 0;
                        }

                    }
                    else if (x == 8)
                    {
                        if (y == 80)
                        {
                            sp = 5;
                            x = 3;
                            y = 0;
                        }

                    }
                    else if (x == 9)
                    {
                        if (y == 80)
                        {
                            sp = 9;
                            x = 3;
                            y = 0;
                        }

                    }
                    else if (x == 10)
                    {
                        if (y == 80)
                        {
                            sp = 9;
                            x = 4;
                            y = 0;
                        }

                    }
                 


                }



                speed.text = "speed : " + sp;

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

               
                if (b <= 0 && b >= -1200)
                {
                    transform.Rotate(0, 0, 0);
                } //가운데



                else if (b > 0 && b <= 1000)
                {
                    transform.Rotate(Vector3.up * Time.deltaTime * 20);
                }
                else if (b > 1000 && b <= 2000)
                {
                    transform.Rotate(Vector3.up * Time.deltaTime * 40);
                }
                else if (b > 2000)
                {
                    transform.Rotate(Vector3.up * Time.deltaTime * 60);
                } //오른쪽



                else if (b < -1200 && b >= -2400)
                {
                    transform.Rotate(Vector3.down * Time.deltaTime * 20);
                }
                else if (b < -2400 && b >= -3600)
                {
                    transform.Rotate(Vector3.down * Time.deltaTime * 40);
                }
                else if (b <= -3600)
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

    void StartTime()
    {
        double kcal = 0;

        if (timeActive)
        {
            timeStart += Time.deltaTime;
            timeStart2 = timeStart / 60;
            Debug.Log(timeStart);
            timeText.text = timeStart2.ToString("F2") + "분";
            kcal = save.count * 0.065 * timeStart2;
            Debug.Log("kcal: " + kcal);
            kcaltext.text = "kcal:" + kcal.ToString("N2");

        }

    }


}
```

<hr/>

### 3-2 경기장 맵 주행 

![경기장](https://user-images.githubusercontent.com/62869017/122773591-722ea200-d2e3-11eb-9d05-8bdf72670425.png)

![waypoint 생성](https://user-images.githubusercontent.com/62869017/122773773-97bbab80-d2e3-11eb-906e-bbc5f6e26a50.png)

경기장은 경쟁을 하여 먼저 들어오는것이 목표이기 때문에 Waypoint를 생성하여 센서 값이 들어오면
자동으로 경기장을 주행하도록 하였다.

![Start](https://user-images.githubusercontent.com/62869017/122774467-2b8d7780-d2e4-11eb-9088-1374a6eca492.png)
Player가 준비되면 3 2 1 카운트 후 시작하게 된다.

![Panel](https://user-images.githubusercontent.com/62869017/122774644-5677cb80-d2e4-11eb-9cba-aa4245440cde.png)

지정된 바퀴 수를 돌게 되면 판넬이 나오며 경기장을 빠져나가게 된다.

![player](https://user-images.githubusercontent.com/62869017/122773945-be79e200-d2e3-11eb-9585-f52775a38e84.png)

```cs
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


                if (a == 0) //적외선센서 a는 0// 
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
                 
                }


                int br = int.Parse(split_text[0]);
                if (br == 1) //브레이크//
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
```

AI는 2명을 생성하였으며 AI또한 지정된 Waypoint를 주행한다.

```cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class run1 : MonoBehaviour
{
    public List<Transform> waypoints = new List<Transform>();
    private Transform targetWaypoint;
   
    private int targetWaypointIndex = 0;
    private float minDistance = 0.1f;
    private int lastWaypointIndex;

    private float movementSpeed = 10;
    private float rotationSpeed = 3.0f;
    public bool check = true;
    public GameObject[] wap;
    public int wapnumber;
    // Start is called before the first frame update
    void Start()
    {
     
        lastWaypointIndex = waypoints.Count - 1;
        targetWaypoint = waypoints[targetWaypointIndex];
      

    }

    // Update is called once per frame
    void Update()
    {
       
        transform.Rotate(Vector3.down * Time.deltaTime * -270);
        float movementStep = movementSpeed * Time.deltaTime;
        float rotationStep = rotationSpeed * Time.deltaTime;

        Vector3 directionToTarget = targetWaypoint.position - transform.position;
        Quaternion rotationToTarget = Quaternion.LookRotation(directionToTarget);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotationToTarget, rotationStep);


        float distance = Vector3.Distance(transform.position, targetWaypoint.position);
        CheckDistanceToWaypoint(distance);
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, movementStep);

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
        if (targetWaypointIndex > lastWaypointIndex)
        {
            targetWaypointIndex = 0;
        }
        targetWaypoint = waypoints[targetWaypointIndex];
    }

    IEnumerator WaitForIt()
    {
        yield return new WaitForSeconds(3.0f);
        check = true;
    }
}
```


