using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    [SerializeField] private bool IsThisTreasure;

    public void Init()
    {
        this.GetComponent<MeshCollider>().enabled = true;
        IsThisTreasure=false;
    }

    public bool GetTreasure()
    {
        return IsThisTreasure;
    }

    public void SetTreasure()
    {
        IsThisTreasure = true;
        this.name = "Treasure ! ";
    }

    public bool Check()
    {
        this.GetComponent<MeshCollider>().enabled = false;
        return IsThisTreasure;
    }
   
}
