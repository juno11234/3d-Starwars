using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class TimeLineHandle : MonoBehaviour
{
    [SerializeField] private InputManager playerInput;
    [SerializeField] private GameObject freeLookCam;
    [SerializeField] private GameObject portalManager;
    [SerializeField] private PlayerUI playerUI;

    [SerializeField] private float slowSpeed = 0.2f;

    public void InputEnable()
    {
        playerInput.enabled = true;
    }

    public void InputDisable()
    {
        playerInput.enabled = false;
    }

    public void CamEnable()
    {
        freeLookCam.SetActive(true);
    }

    public void CamDisable()
    {
        freeLookCam.SetActive(false);
    }

    public void PlayerUIEnable()
    {
        playerUI.gameObject.SetActive(true);
    }

    public void PlayerUIDisable()
    {
        playerUI.gameObject.SetActive(false);
    }

    public void PortalEnable()
    {
        portalManager.SetActive(true);
    }

    public void PortalDisable()
    {
        portalManager.SetActive(false);
    }

    public void TimeLineSlow(PlayableDirector director)
    {
        director.playableGraph.GetRootPlayable(0).SetSpeed(slowSpeed);
    }

    public void TimeLineSlowCancel(PlayableDirector director)
    {
        director.playableGraph.GetRootPlayable(0).SetSpeed(1f);
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(0);
    }
}