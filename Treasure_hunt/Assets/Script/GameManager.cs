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

    void InitTreasure()
    {
        var treasures = TreasureParent.GetComponentsInChildren<Treasure>();

        float seed = UnityEngine.Time.time * 100f;
        Random.InitState((int)seed);
        for (int i = 0; i < TreasureCnt; i++)
        {

            int rnd = Random.Range(0, treasures.Length);
            while (treasures[rnd].GetTreasure())
            {
                rnd = Random.Range(0, treasures.Length);
            }

            treasures[rnd].SetTreasure();

        }
    }

    public void FoundTreasure()
    {
         TreasureCnt--;
         CurTreasureUIText.text = TreasureCnt.ToString();
        if(TreasureCnt==0)
        {
            GameOver();
        }
        
    }

    private void GameOver()
    {
        Debug.Log("GameOVer ! ");
    }

    public void StartClick()
    {
        canvas.gameObject.SetActive(false);
        GameStart = true;

        TimeUI.gameObject.SetActive(true);
        TreasureUI.gameObject.SetActive(true);

        CurTimeUIText.text = TimeText.text;
        CurTreasureUIText.text = TreasureCntText.text;

        InitTreasure(); // 보물 초기화
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
        Debug.Log("TimeIncrease ");
        if (Time > 10)
        {
            TimeText.text = "INF";
        }
        else
        {
            Time++;
            TimeText.text = Time.ToString();
        }
    }

    public void TimeDecrease()
    {
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
