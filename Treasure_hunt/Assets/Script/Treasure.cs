using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    public TreasureType Type;
    [SerializeField] private bool IsThisTreasure;

    public void Init()
    {
        this.GetComponent<MeshCollider>().enabled = true;
        this.name = "--";
        IsThisTreasure=false;
    }

 
    public bool GetTreasure()
    {
        return IsThisTreasure;
    }

    public void SetTreasure(TreasureType type)
    {
        IsThisTreasure = true;
        this.name = "Treasure ! ";
        this.Type = type;
    }

    public bool Check()
    {
        this.GetComponent<MeshCollider>().enabled = false;
        return IsThisTreasure;
    }
   
}
