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

    public Image DiaImage;

    public Sprite BigDiamondsprite;
    public Sprite BlueDiamondsprite;
    public Sprite Redmondsprite;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void ShowPanel(bool b, TreasureType type = TreasureType.NONE)  // true : correct 
    {
        if(b) 
        {
            CorrectText.DOFade(1, 1.0f);
            DiaImage.DOFade(1, 1.0f);
            switch (type)
            {
                case TreasureType.BIG:
                    DiaImage.sprite = BigDiamondsprite;
                    break;
                case TreasureType.BLUE:
                    DiaImage.sprite = BlueDiamondsprite;
                    break;
                case TreasureType.RED:
                    DiaImage.sprite = Redmondsprite;
                    break;
            }
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
        DiaImage.DOFade(0, 1.0f);
    }
}
