using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public static bool GameStart = false;
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


    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        TimeText.text = Time.ToString();
        TreasureCntText.text = TreasureCnt.ToString();
    }

    void Update()
    {
        
    }

    public void ShowSettingPanel()
    {
        canvas.gameObject.SetActive(true);
    }

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
        GameOverCanvas.GetComponent<GameoverPanel>().SetUIInfo(GetBigDia, GetBlueDia, GetRedDia);
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
