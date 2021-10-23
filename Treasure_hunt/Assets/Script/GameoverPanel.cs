using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameoverPanel : MonoBehaviour
{
    public static GameoverPanel instance = null;

    public Text BigDiaCntText;
    public Text BlueDiaCntText;
    public Text RedDiaCntText;

    public Text TimeText;

    void Start()
    {
        if (instance == null)
            instance = this;
    }

    public void SetUIInfo(int big, int blue, int red, string time)
    {
        BigDiaCntText.text = big.ToString();
        BlueDiaCntText.text = blue.ToString();
        RedDiaCntText.text = red.ToString();
        TimeText.text = time;
    }
}
