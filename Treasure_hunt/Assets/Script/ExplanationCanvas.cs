using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplanationCanvas : MonoBehaviour
{

   public Image[] explanationImg;

    int page;

    void Start()
    {
        foreach(var item in explanationImg)
        {
            item.gameObject.SetActive(false);
        }
        explanationImg[0].gameObject.SetActive(true);
        page = 0;
    }

    public void NextPageBtnClick()
    {
        if (page < explanationImg.Length)
        {
            explanationImg[page].gameObject.SetActive(false);
            page++;
            explanationImg[page].gameObject.SetActive(true);

        }
    }

    public void StartBtnClickEvent()
    {
        this.gameObject.SetActive(false);
        GameManager.instance.ShowSettingPanel();
    }
}
