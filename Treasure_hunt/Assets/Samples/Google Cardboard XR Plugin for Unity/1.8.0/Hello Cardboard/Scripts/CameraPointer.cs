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
    private const float _maxDistance = 10;
    private GameObject _gazedAtObject = null;
    EventSystem m_eventSystem;
    PointerEventData m_pointerEvent;

    public Image GazeImage;

    public Selectable m_currentSelectable;
    IPointerClickHandler m_clickHandler;


    void Start()
    {
        m_eventSystem = EventSystem.current;
        m_pointerEvent = new PointerEventData(m_eventSystem);
        m_pointerEvent.button = PointerEventData.InputButton.Left;
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
        else
        {
            UIRaycast();
        }

    }

    private void UIRaycast()
    {
        m_pointerEvent.position =
#if UNITY_EDITOR
            new Vector2(Screen.width / 2, Screen.height / 2);
#else
            new Vector2(XRSettings.eyeTextureWidth / 2, XRSettings.eyeTextureHeight / 2);
#endif

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        m_eventSystem.RaycastAll(m_pointerEvent, raycastResults);

        if (raycastResults.Count > 0)
        {
            var newSelectable = raycastResults[0].gameObject.GetComponentInParent<Selectable>();
          
            if(m_currentSelectable != newSelectable)
            {
                GazeOff();
                Debug.Log("gaze  " + newSelectable);
                GazeOn(newSelectable);
                m_currentSelectable = newSelectable;
            }
        }
        else
        {
            m_currentSelectable = null;
            GazeOff();
        }
    }

    private void GameRaycast()
    {
        RaycastHit hit;
        int mask = (1 << 8);

        if (Physics.Raycast(transform.position, transform.forward, out hit, _maxDistance, mask))
        {
            if (_gazedAtObject != hit.transform.gameObject)
            {
                // New GameObject.
                _gazedAtObject?.SendMessage("OnPointerExit");
                _gazedAtObject = hit.transform.gameObject;
                _gazedAtObject.SendMessage("OnPointerEnter");
            }
        }
        else
        {
            // No GameObject detected in front of the camera.
            _gazedAtObject?.SendMessage("OnPointerExit");
            _gazedAtObject = null;
        }

        // Checks for screen touches.
        if (Google.XR.Cardboard.Api.IsTriggerPressed)
        {
            _gazedAtObject?.SendMessage("OnPointerClick");
        }
    }

    void GazeOn(Selectable selectable)
    {
        m_clickHandler = selectable.GetComponent<IPointerClickHandler>();

        GazeImage.DOFillAmount(1,1.0f).OnComplete(() =>{
            Debug.Log("Gaze " + selectable.name);
            m_clickHandler.OnPointerClick(m_pointerEvent);
        });
    }

    void GazeOff()
    {
        GazeImage.fillAmount = 0;
    }
}
