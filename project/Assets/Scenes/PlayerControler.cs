using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using KinematicCharacterController;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    public VirtualJoystick virtualJoystick;
    public DivCharacterController characterController;
    public CinemachineBrain cinemachineBrain;
    public TouchLook touchLook;

    public CinemachineFreeLook cinemachineFreeLook;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var input = new PlayerCharacterInputs
        {
            MoveAxisForward = virtualJoystick.Input.y,
            MoveAxisRight = virtualJoystick.Input.x,
            CameraRotation = cinemachineBrain.transform.rotation,
            JumpDown = isJump
        };
        isJump = false;
        characterController.SetInputs(ref input);
    }

    private bool isJump = false;

    public void ClickJump()
    {
        isJump = true;
    }

    private void LateUpdate()
    {
        cinemachineFreeLook.m_XAxis.Value += (1 - Mathf.Exp(-touchLook.LookInputDelta.x * Time.deltaTime)) * -100;
        cinemachineFreeLook.m_YAxis.Value += (1 - Mathf.Exp(-touchLook.LookInputDelta.y * Time.deltaTime)) * -1;
    }
}