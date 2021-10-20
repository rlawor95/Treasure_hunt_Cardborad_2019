using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AnswerPanel : MonoBehaviour
{
    public static AnswerPanel instance = null;

    public Image Panel;
    public Text IncorrectText;
    public Text CorrectText;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void ShowPanel(bool b)  // true : correct 
    {
        if(b) 
        {
            CorrectText.DOFade(1, 1.0f);
        }
        else
        {
            IncorrectText.DOFade(1, 1.0f);
        }

        Panel.DOFade(1, 1.0f).OnComplete(() =>
        {
            StartCoroutine(Off());
        });
    }

    IEnumerator Off()
    {
        yield return new WaitForSeconds(1.0f);
        Panel.DOFade(0, 1.0f);
        CorrectText.DOFade(0, 1.0f);
        IncorrectText.DOFade(0, 1.0f);
    }
}
