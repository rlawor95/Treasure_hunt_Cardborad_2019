using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static bool GameStart = false;
    public Text TimeText;
    public Text TreasureCntText;
    public Toggle InfToggle;
    

    public int Time = 3;
    public int TreasureCnt = 1;

    public Canvas canvas;
    

    void Start()
    {
        TimeText.text = Time.ToString();
         TreasureCntText.text = TreasureCnt.ToString();
    }

    public void StartClick()
    {
        canvas.gameObject.SetActive(false);
        GameStart = true;
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
