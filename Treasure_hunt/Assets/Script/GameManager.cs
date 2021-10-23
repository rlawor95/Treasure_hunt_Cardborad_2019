using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Management;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public static bool GameStart = false;
    public static bool NormalMode = false;
    public Text TimeText;
    public Text TreasureCntText;
    public Toggle InfToggle;
    

    public int Time = 3;
    public int TreasureCnt = 1;

    public Canvas canvas;



    // 게임 진행중 떠있는 UI /==============
    public GameObject TimeUI; 
    public GameObject TreasureUI; 
    public Text CurTimeUIText;
    public Text CurTreasureUIText;

    //-----

    public GameObject TreasureParent;


    public GameObject GameOverCanvas;

    public Transform Player;

    public Transform OriginPosition;

    
    public int GetBigDia;
    public int GetBlueDia;
    public int GetRedDia;

    public Text DebugTxt;

    public Camera cam;



    // public Camera VRCAM;
    // public Camera NORMALCAM;

    // public Canvas EyeRaycaster; // 이거 랜더링 카메라 바꿔줘야함.

    // float tempfov;

    void Awake()
    {
        if (instance == null)
            instance = this;
#if !UNITY_EDITOR
       StartCoroutine(StartXR());
#endif
    }

    void Start()
    {
        TimeText.text = Time.ToString();
        TreasureCntText.text = TreasureCnt.ToString();

        // foreach(var item in XRSettings.supportedDevices)
        //     Debug.Log(item);
        // Debug.Log(XRSettings.loadedDeviceName);
    }

    void Update()
    {
        //Vector3 angleAcceler = Input.acceleration;

        //var tracking = InputTracking.GetLocalRotation(XRNode.CenterEye);
        //DebugTxt.text = tracking.eulerAngles.x.ToString() + " " + tracking.eulerAngles.y.ToString() + " " + tracking.eulerAngles.z.ToString();
        
        


        if(Input.GetMouseButtonDown(0))
        {
            // if (NormalMode)
            // {
            //     VRCAM.enabled = true;
            //     NORMALCAM.enabled = false;
            //     EyeRaycaster.worldCamera = VRCAM;
            //     NormalMode = false;
            // }
            // else
            // {
            //     VRCAM.enabled = false;
            //     NORMALCAM.enabled = true;
            //     EyeRaycaster.worldCamera = NORMALCAM;
            //     NormalMode = true;
            // }

            if (XRSettings.loadedDeviceName == "CardboardDisplay")
            {
                //XRSettings.LoadDeviceByName("");
                //EnableVR();
                //DebugTxt.text = "go disable" + " 1";
                CameraPointer.instance.GazeInit();
                DisableVR();
                NormalMode = true;
               // SwitchTo2D();
            }
            else
            {
                //DisableVR();
                 //DebugTxt.text = "go Enable" + " 1";
                 CameraPointer.instance.GazeInit();
                EnableVR();
                NormalMode = false;
                //cam.fieldOfView = tempfov;
                //SwitchToVR();
            }

        }

        // if(XRSettings.enabled==false)
        // {
        //     DebugTxt.text = "XRSettings.enabled==false";

        //     // if (TryGetCenterEyePosition(out Vector3 _position))
        //     // {
        //     //     transform.localPosition = _position;
        //     // }
        //     if (TryGetCenterEyeFeature(out Quaternion _rotation))
        //     {
        //         DebugTxt.text = _rotation.eulerAngles.x.ToString() + "  " + _rotation.eulerAngles.y;
        //         cam.transform.parent.localRotation = _rotation;
        //     }
        // }
    }

    IEnumerator LoadDevice(string newDevice, bool enable)
    {
        
        XRSettings.LoadDeviceByName(newDevice);
        yield return null;
        XRSettings.enabled = enable;

       // DebugTxt.text = "LD : " + XRSettings.loadedDeviceName + " " + XRSettings.enabled;
    }

    void EnableVR()
    {
        //DebugTxt.text = "EnableVR 2";
        //StartCoroutine(LoadDevice("CardboardDisplay", true));
       
       StartCoroutine(StartXR());
       GyroControl.instance.StopGyro();
    }

    void DisableVR()
    {
        //DebugTxt.text = "DisableVR 2";
        // StartCoroutine(LoadDevice("", false));
       
       StopXR();
       GyroControl.instance.StartGyro();
    }

    public void ShowSettingPanel()
    {
        canvas.gameObject.SetActive(true);
    }



    public IEnumerator StartXR()
    {
        DebugTxt.text = "Initializing XR...";
        yield return XRGeneralSettings.Instance.Manager.InitializeLoader();

        if (XRGeneralSettings.Instance.Manager.activeLoader == null)
        {
            DebugTxt.text =  "Initializing XR Failed. Check Editor or Player log for details.";
        }
        else 
        {
             DebugTxt.text =  "Starting XR...";
            XRGeneralSettings.Instance.Manager.StartSubsystems();
          
            //CameraPointer.nomalMode = false;
        }
    }

    void StopXR()
    {
       DebugTxt.text = "Stopping XR...";

        XRGeneralSettings.Instance.Manager.StopSubsystems();
        XRGeneralSettings.Instance.Manager.DeinitializeLoader();
        DebugTxt.text =  "XR stopped completely.";

        // tempfov = cam.fieldOfView;
        // cam.fieldOfView = 60;
        // CameraPointer.nomalMode = true;
    }

    bool TryGetCenterEyeFeature(out Quaternion _rotation)
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(XRNode.CenterEye);
        if (device.isValid)
        {
            if (device.TryGetFeatureValue(CommonUsages.centerEyeRotation, out _rotation))
                return true;
        }
        // This is the fail case, where there was no center eye was available.
        _rotation = Quaternion.identity;
        return false;
    }
    bool TryGetCenterEyePosition(out Vector3 _position)
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(XRNode.CenterEye);
        if (device.isValid)
        {
            if (device.TryGetFeatureValue(CommonUsages.centerEyePosition, out _position))
                return true;
        }
        // This is the fail case, where there was no center eye was available.
        _position = transform.localPosition;
        return false;
    }
    ///

    IEnumerator SwitchToVR()
    {
        // Device names are lowercase, as returned by `XRSettings.supportedDevices`.
        string desiredDevice = "cardboard"; // Or "cardboard".

        // Some VR Devices do not support reloading when already active, see
        // https://docs.unity3d.com/ScriptReference/XR.XRSettings.LoadDeviceByName.html
        if (string.Compare(XRSettings.loadedDeviceName, desiredDevice, true) != 0)
        {
            XRSettings.LoadDeviceByName(desiredDevice);

            // Must wait one frame after calling `XRSettings.LoadDeviceByName()`.
            yield return null;
        }

        // Now it's ok to enable VR mode.
        XRSettings.enabled = true;
    }

    // Call via `StartCoroutine(SwitchTo2D())` from your code. Or, use
    // `yield SwitchTo2D()` if calling from inside another coroutine.
    IEnumerator SwitchTo2D()
    {
        // Empty string loads the "None" device.
        XRSettings.LoadDeviceByName("");

        // Must wait one frame after calling `XRSettings.LoadDeviceByName()`.
        yield return null;

        // Not needed, since loading the None (`""`) device takes care of this.
        // XRSettings.enabled = false;

        // Restore 2D camera settings.
        ResetCameras();
    }

    // Resets camera transform and settings on all enabled eye cameras.
    void ResetCameras()
    {
        // Camera looping logic copied from GvrEditorEmulator.cs
        for (int i = 0; i < Camera.allCameras.Length; i++)
        {
            Camera cam = Camera.allCameras[i];
            if (cam.enabled && cam.stereoTargetEye != StereoTargetEyeMask.None)
            {

                // Reset local position.
                // Only required if you change the camera's local position while in 2D mode.
                cam.transform.localPosition = Vector3.zero;

                // Reset local rotation.
                // Only required if you change the camera's local rotation while in 2D mode.
                cam.transform.localRotation = Quaternion.identity;

                // No longer needed, see issue github.com/googlevr/gvr-unity-sdk/issues/628.
                // cam.ResetAspect();

                // No need to reset `fieldOfView`, since it's reset automatically.
            }
        }
    }

    ///

    void InitTreasure()
    {


        // var treasures = TreasureParent.GetComponentsInChildren<Treasure>();

        // foreach(var item in treasures)
        // {
        //     item.Init();
        // }

        // float seed = UnityEngine.Time.time * 100f;
        // Random.InitState((int)seed);
        // for (int i = 0; i < TreasureCnt; i++)
        // {
        //     int rnd = Random.Range(0, treasures.Length);
        //     while (treasures[rnd].GetTreasure())
        //     {
        //         rnd = Random.Range(0, treasures.Length);
        //     }

        //     treasures[rnd].SetTreasure();
        // }
    }

    public void FoundTreasure(TreasureType type)
    {
        switch (type)
        {
            case TreasureType.BIG:
                GetBigDia++;
                break;
            case TreasureType.BLUE:
                GetBlueDia++;
                break;
            case TreasureType.RED:
                GetRedDia++;
                break;
        }

        TreasureCnt--;
        CurTreasureUIText.text = TreasureCnt.ToString();
        if(TreasureCnt==0)
        {
            GameStart = false;
            CameraPointer.instance.GameOver();
            StopAllCoroutines();
            GameOver();

        }
    }

    public void GameOver()
    {
        GameOverCanvas.GetComponent<GameoverPanel>().SetUIInfo(GetBigDia, GetBlueDia, GetRedDia, CurTimeUIText.text);
        GameOverCanvas.SetActive(true);
        GameOverCanvas.transform.position = CameraPointer.instance.transform.position + CameraPointer.instance.transform.forward * 2.5f;
        GameOverCanvas.transform.forward = CameraPointer.instance.transform.forward;

        GameOverCanvas.transform.localEulerAngles = new Vector3(0, GameOverCanvas.transform.localEulerAngles.y, GameOverCanvas.transform.localEulerAngles.z);
        GameOverCanvas.transform.position = new Vector3(GameOverCanvas.transform.position.x, 2, GameOverCanvas.transform.position.z);
    }

    public void ReStartBtnCliekEvent()
    {
        StartCoroutine(GameRestart());
    }

    private IEnumerator GameRestart()
    {
        GetBigDia = 0;
        GetBlueDia = 0;
        GetRedDia = 0;
        TreasureManager.instance.GameReSet();

        GameOverCanvas.SetActive(false);

        yield return new WaitForSeconds(1.0f);
        CameraPointer.instance.Teleport(OriginPosition);


        TimeUI.gameObject.SetActive(false);
        TreasureUI.gameObject.SetActive(false);

        canvas.gameObject.SetActive(true);
        Time = 3;
        TreasureCnt = 1;
        TreasureCntText.text = TreasureCnt.ToString();
        TimeText.text = Time.ToString();
        InfToggle.isOn = false;

        //GameOverCanvas.transform.position = Player.position + Player.forward * 10f;
        //GameOverCanvas.gameObject.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartClick()
    {
        Debug.Log("StartClick 1 ");
        canvas.gameObject.SetActive(false);
        GameStart = true;

        TimeUI.gameObject.SetActive(true);
        TreasureUI.gameObject.SetActive(true);

       
        CurTreasureUIText.text = TreasureCntText.text;

        TreasureManager.instance.TreasureInit(TreasureCnt);
        //InitTreasure(); // 보물 초기화

        if (TimeText.text != "INF")
        {
            var t = Time * 60;
            CurTimeUIText.text = t.ToString();
            StartCoroutine(TimeUICheck(t));
        }
        else
            CurTimeUIText.text = "INF";

            Debug.Log("StartClick 2 ");
    }

    IEnumerator TimeUICheck(int time)
    {
        Debug.Log("TimeUICheck");

        while (time > 0)
        {
            yield return new WaitForSeconds(1.0f);
            time--;
            CurTimeUIText.text = time.ToString();
        }

        GameOver();
    }

    public void TreasureIncrease()
    {
        Debug.Log("TreasureIncrease ");
        if (TreasureCnt < 10)
        {
            TreasureCnt++;
            TreasureCntText.text = TreasureCnt.ToString();
        }

    }
    public void TreasureDecrease()
    {
        Debug.Log("TreasureDecrease ");
        if (TreasureCnt > 1)
        {
            TreasureCnt--;
            TreasureCntText.text = TreasureCnt.ToString();
        }
    }

    public void TimeIncrease()
    {
        if(Time>10 || TimeText.text=="INF")
            return;

        if (Time > 10)
        {
           // TimeText.text = "INF";
        }
        else
        {
            Time++;
            TimeText.text = Time.ToString();
        }
    }

    public void TimeDecrease()
    {
        if(TimeText.text=="INF")
            return;

        if (Time > 3)
        {
            Time--;
            TimeText.text = Time.ToString();
        }
    }

    public void TimeInf()
    {
        if (InfToggle.isOn)
            TimeText.text = "INF";
        else
            TimeText.text = Time.ToString();
    }
}
