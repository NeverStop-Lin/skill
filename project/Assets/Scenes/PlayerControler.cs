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

    public CinemachineFreeLook cinemachineFreeLookAuto;
    public CinemachineFreeLook cinemachineFreeLookTouch;

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


        if (touchLook.IsDragging)
        {
            cinemachineFreeLookAuto.gameObject.SetActive(false);
            cinemachineFreeLookAuto.transform.position = cinemachineFreeLookTouch.transform.position;
            cinemachineFreeLookTouch.m_YAxis.m_InputAxisValue =
                (1 - Mathf.Exp(-touchLook.LookInputDelta.y * Time.deltaTime)) * -30;
            cinemachineFreeLookTouch.m_XAxis.m_InputAxisValue =
                (1 - Mathf.Exp(-touchLook.LookInputDelta.x * Time.deltaTime)) * -30;
        }
        else
        {
            cinemachineFreeLookAuto.gameObject.SetActive(true);

            var a = Vector3.ProjectOnPlane(
                cinemachineFreeLookAuto.VirtualCameraGameObject.transform.position -
                characterController.transform.position,
                Vector3.up
            );
            var targetX = Vector3.SignedAngle(-Vector3.forward,a, Vector3.up);
            var targetY = cinemachineFreeLookAuto.m_YAxis.Value;
            Debug.Log("角度"+targetX);
            cinemachineFreeLookTouch.m_XAxis.m_InputAxisValue =
                cinemachineFreeLookTouch.m_XAxis.Value -
                Mathf.LerpAngle(cinemachineFreeLookTouch.m_XAxis.Value, targetX, Time.deltaTime);
            // cinemachineFreeLookTouch.m_YAxis.m_InputAxisValue =
            // Mathf.LerpAngle(cinemachineFreeLookTouch.m_YAxis.Value, targetY, Time.deltaTime);
        }
    }


    public void TTTT(ICinemachineCamera a, ICinemachineCamera b)
    {
        // if (a != null && b != null)
        // {
        //     Debug.Log("new==>" + a.VirtualCameraGameObject.gameObject.name + "@@@" + "old==>" +
        //               b.VirtualCameraGameObject.gameObject.name);
        //     b.VirtualCameraGameObject.transform.position = a.VirtualCameraGameObject.transform.position;
        // }
    }

    private bool isJump = false;

    public void ClickJump()
    {
        isJump = true;
    }

    private void LateUpdate()
    {
    }
}