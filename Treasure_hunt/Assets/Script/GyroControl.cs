using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GyroControl : MonoBehaviour 
{

    public static GyroControl instance = null;

    bool isGyro = false;

    private bool gyroEnabled =false;
    private Gyroscope gyro;

    private GameObject cameraContainer;
    private Quaternion rot;

	// Use this for initialization
    public Text DebugText;

	void Start ()
    {
        if(instance == null)
            instance = this;

        // cameraContainer = new GameObject("Camera Container");
        // cameraContainer.transform.position = new Vector3(0,1,-10);
        // transform.SetParent(cameraContainer.transform);

       
	}


    public void StartGyro()
    {
        gyroEnabled = EnableGyro();
        isGyro = true;
    }

    public void StopGyro()
    {
        isGyro = false;
         this.transform.parent.rotation = Quaternion.Euler(0, 0, 0);
    }

	// Update is called once per frame
	void Update () 
    {
        if(isGyro == false)
            return;

        if(gyroEnabled)
        {
            transform.localRotation = gyro.attitude * rot;
        }

    
	}

    private bool EnableGyro()
    {
        if(SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;

            this.transform.parent.rotation = Quaternion.Euler(90, -90, 90);
            rot = new Quaternion(0, 0, 1, 0);

            return true;
        }

        return false;
    }

}
