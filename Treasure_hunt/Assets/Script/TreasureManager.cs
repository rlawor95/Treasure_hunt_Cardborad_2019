using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TreasureType
{
    BIG, BLUE, RED
}

public class TreasureManager : MonoBehaviour
{
    public static TreasureManager instance=null;

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

    public void TreasureInit(int count)
    {
        var treasures = TreasureObjectParent.GetComponentsInChildren<Treasure>();

        foreach (var item in treasures)
        {
            item.Init();
        }

        float seed = UnityEngine.Time.time * 100f;
        Random.InitState((int)seed);

        for (int i = 0; i < count; i++)
        {
            int rnd = Random.Range(0, treasures.Length);
            while (treasures[rnd].GetTreasure())
            {
                rnd = Random.Range(0, treasures.Length);
            }

            var Type = PickTreasureType();
            treasures[rnd].SetTreasure(Type);
        }
    }

    TreasureType PickTreasureType()
    {
        float seed = UnityEngine.Time.time * 23;
        Random.InitState((int)seed);

        int rnd = Random.Range(0, 3);

        while(diatype[rnd]==0)
        {
            rnd = Random.Range(0, 3);
        }

        diatype[rnd] -= 1;
        return (TreasureType)rnd;
    }

}
