//-----------------------------------------------------------------------
// <copyright file="CameraPointer.cs" company="Google LLC">
// Copyright 2020 Google LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//-----------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
#if UNITY_2017_2_OR_NEWER
using UnityEngine.XR;
#else
using XRSettings = UnityEngine.VR.VRSettings;
#endif

/// <summary>
/// Sends messages to gazed GameObject.
/// </summary>
public class CameraPointer : MonoBehaviour
{
    public static CameraPointer instance = null;

    private const float _maxDistance = 10;
    public GameObject _gazedAtObject = null;


    [SerializeField]
    FloatUnityEvent m_onLoad;

     [SerializeField, Tooltip("In seconds")]
    float m_loadingTime;
    [SerializeField, Tooltip("Treasure Loading")]
    float m_TreasureloadingTime;
    float m_elapsedTime = 0;

    bool isGaze = false;
    bool isGazeTreasure = false;

    public Image FadePanel;

    public Image GazeImage1;
    public RawImage GazeImage2;

    private GameObject tempTeleport = null;

    void Start()
    {
        if (instance == null)
            instance = this;
    }

    public void GameOver()
    {
        m_elapsedTime = 0;
        m_onLoad.Invoke(m_elapsedTime / m_loadingTime);
        isGaze = false;
        isGazeTreasure = false;
        _gazedAtObject = null;
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    public void Update()
    {
        if(GameManager.GameStart)
        {
            GameRaycast();
        }

        if(isGaze)
        {
            m_elapsedTime += Time.deltaTime;
            m_onLoad.Invoke(m_elapsedTime / m_loadingTime);

            if (m_elapsedTime > m_loadingTime)
            {
                if(_gazedAtObject.tag.Contains("Teleport"))
                {
                    m_elapsedTime=0;
                    m_onLoad.Invoke(m_elapsedTime / m_loadingTime);
                    isGaze = false;
                    isGazeTreasure=false;
                    Teleport(_gazedAtObject.transform);     

                    tempTeleport?.SetActive(true); 
                    tempTeleport = _gazedAtObject;
                    tempTeleport.SetActive(false);
                }
            }
        }
        else if(isGazeTreasure)
        {
            m_elapsedTime += Time.deltaTime;
            m_onLoad.Invoke(m_elapsedTime / m_TreasureloadingTime);

            if (m_elapsedTime > m_TreasureloadingTime)
            {
                if(_gazedAtObject.tag.Contains("Treasure"))
                {
                    m_elapsedTime=0;
                    m_onLoad.Invoke(m_elapsedTime / m_TreasureloadingTime);
                    isGaze = false;
                    isGazeTreasure=false;

                    var chk = _gazedAtObject.GetComponent<Treasure>().Check();

                    if (chk) 
                    {
                        GameManager.instance.FoundTreasure();
                        AnswerPanel.instance.ShowPanel(true);
                        SoundManager.instance.PlayCorrectSound();
                    }
                    else
                    {
                        AnswerPanel.instance.ShowPanel(false);
                        SoundManager.instance.PlayInCorrectSound();
                    }
                }
            }
        }
    }

    private void GameRaycast()
    {
        RaycastHit hit;
        int mask = (1 << 10);

        if (Physics.Raycast(transform.position, transform.forward, out hit, _maxDistance, mask))
        {
            //Debug.Log("hit " + hit.transform.name);
            if (_gazedAtObject != hit.transform.gameObject)
            {
                
                _gazedAtObject = hit.transform.gameObject;
                if (hit.transform.tag.Contains("Teleport"))
                { 
                    //_gazedAtObject.SendMessage("OnPointerEnter");
                    ChangeGazeColor(false);
                    isGaze = true;
                    isGazeTreasure=false;
    
                    // GazeImage.DOFillAmount(1,1.0f).OnComplete(()=>
                    // {
                    //     Debug.Log("Gaze");
                    // });
                }
                else if(hit.transform.tag.Contains("Treasure"))
                {
                    ChangeGazeColor(true);
                    isGaze = false;
                    isGazeTreasure=true;
                }
            }

        }
        else
        {
            m_elapsedTime = 0;
             m_onLoad.Invoke(m_elapsedTime / m_loadingTime);
            isGaze = false;
             isGazeTreasure=false;
            //_gazedAtObject?.SendMessage("OnPointerExit");
            _gazedAtObject = null;
        }
    }

    public void Teleport(Transform location)
    {
        SoundManager.instance.PlayTeleporSound();
        FadePanel.DOFade(1,0.5f).OnComplete(()=>
        {
            this.transform.parent.position = new Vector3(location.position.x, location.position.y + 1f, location.position.z);
            FadePanel.DOFade(0, 0.5f);
        });
    }

    private void ChangeGazeColor(bool b) // true : 보물 오브젝, false : 나머지 
    {
        if(b)
        {
            GazeImage1.color = Color.yellow;
            GazeImage2.color = Color.yellow;
        }
        else
        {
            GazeImage1.color = Color.white;
            GazeImage2.color = Color.white;
        }
    }


    // private void GameRaycast()
    // {
    //     RaycastHit hit;
    //     int mask = (1 << 10);

    //     if (Physics.Raycast(transform.position, transform.forward, out hit, _maxDistance, mask))
    //     {
    //         Debug.Log("game ray " + hit.transform.name);

    //         if (_gazedAtObject != hit.transform.gameObject)
    //         {
    //             // New GameObject.
    //             _gazedAtObject?.SendMessage("OnPointerExit");
    //             _gazedAtObject = hit.transform.gameObject;
    //             _gazedAtObject.SendMessage("OnPointerEnter");
    //         }
    //     }
    //     else
    //     {
    //         // No GameObject detected in front of the camera.
    //         _gazedAtObject?.SendMessage("OnPointerExit");
    //         _gazedAtObject = null;
    //     }

    //     // // Checks for screen touches.
    //     // if (Google.XR.Cardboard.Api.IsTriggerPressed)
    //     // {
    //     //     _gazedAtObject?.SendMessage("OnPointerClick");
    //     // }
    // }
}
