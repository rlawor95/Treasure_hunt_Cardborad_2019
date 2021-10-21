using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplanationCanvas : MonoBehaviour
{

   public Image[] explanationImg;

   public Button next;
   public Button prev;

    int page;

    void Start()
    {
        foreach(var item in explanationImg)
        {
            item.gameObject.SetActive(false);
        }
        explanationImg[0].gameObject.SetActive(true);
        page = 0;

        //prev.gameObject.SetActive(false);
    }

    public void NextPageBtnClick()
    {
        if (page < explanationImg.Length-1)
        {
            explanationImg[page].gameObject.SetActive(false);
            page++;
            explanationImg[page].gameObject.SetActive(true);
            BtnSet();
        }
    }

    public void PrevPageBtnClick()
    {
        if (page > 0)
        {
            explanationImg[page].gameObject.SetActive(false);
            page--;
            explanationImg[page].gameObject.SetActive(true);

            BtnSet();
        }
    }

    void BtnSet()
    {
        StartCoroutine(ExcludeBtnInit());
        next.gameObject.SetActive(true);
        prev.gameObject.SetActive(true);

        if(page == explanationImg.Length -1)
        {
            next.gameObject.SetActive(false);
        }
        if(page==0)
        {
            prev.gameObject.SetActive(false);
        }
    }

    IEnumerator ExcludeBtnInit()
    {
        yield return new WaitForSeconds(1.0f);
        EyeRaycaster.instance.m_excluded = null;
    }

    public void StartBtnClickEvent()
    {
        this.gameObject.SetActive(false);
        GameManager.instance.ShowSettingPanel();
    }
}
