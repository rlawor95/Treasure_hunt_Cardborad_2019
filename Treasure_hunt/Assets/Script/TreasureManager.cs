using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TreasureType
{
    NONE, BIG, BLUE, RED
}

public class TreasureManager : MonoBehaviour
{
    public static TreasureManager instance = null;

    const int bigDiamond = 1;
    const int blueDiamond = 3;
    const int redDiamond = 6;

    public GameObject TreasureObjectParent;

    List<int> diatype = new List<int>();

    void Start()
    {
        if (instance == null)
            instance = this;

        diatype.Add(bigDiamond);
        diatype.Add(blueDiamond);
        diatype.Add(redDiamond);
    }

    public void GameReSet()
    {
        diatype[0] = 1;
        diatype[1] = 3;
        diatype[2] = 6;
    }

    public void TreasureInit(int count)
    {
        Debug.Log("TreasureInit 1");
        var treasures = TreasureObjectParent.GetComponentsInChildren<Treasure>();

        foreach (var item in treasures)
        {
            item.Init();
        }

        for (int i = 0; i < count; i++)
        {
            float seed = UnityEngine.Time.time * 100f;
            Random.InitState((int)seed);
            int rnd = Random.Range(0, treasures.Length);

            while (treasures[rnd].GetTreasure())
            {
                rnd = Random.Range(0, treasures.Length);
            }

            var Type = PickTreasureType();
            treasures[rnd].SetTreasure(Type);
        }
        Debug.Log("TreasureInit 2");
    }

    TreasureType PickTreasureType()
    {
        Debug.Log("PickTreasureType 1");
        float seed = UnityEngine.Time.time * 23;
        Random.InitState((int)seed);

        int rnd = Random.Range(0, 3);

        while (diatype[rnd] == 0)
        {
            seed = UnityEngine.Time.time * 23;
            Random.InitState((int)seed);
            rnd = Random.Range(0, 3);
        }

        diatype[rnd] -= 1;

        TreasureType result = TreasureType.NONE;

        if (rnd == 0)
            result = TreasureType.BIG;
        else if (rnd == 1)
            result = TreasureType.BLUE;
        else if (rnd == 2)
            result = TreasureType.RED;

        Debug.Log("PickTreasureType 2");
        return result;
    }

}
