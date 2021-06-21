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







