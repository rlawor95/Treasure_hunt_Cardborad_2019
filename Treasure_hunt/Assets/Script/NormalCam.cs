using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalCam : MonoBehaviour
{
    public static NormalCam instance = null;
    public GameObject MainCamera;

    void Start()
    {
        if (instance == null)
            instance = this;
    }

    
    void Update()
    {
        this.transform.localEulerAngles = MainCamera.transform.localEulerAngles;
    }
}
