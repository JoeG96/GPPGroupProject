using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraPos : MonoBehaviour
{
    public Transform[] views;
    public float transitionSpeed;
    Transform currentView;

    PlayerControls input;

    private Transform camPosNorth;
    private Transform camPosSouth;
    private Transform camPosEast;
    private Transform camPosWest;

    bool isCamPosS;
    bool isCamPosE;
    bool isCamPosW;

    private void Awake()
    {
        input = new PlayerControls();

        camPosNorth = views[0];
        camPosSouth = views[1];
        camPosEast = views[2];
        camPosWest = views[3];

        currentView = views[0];

        input.Player.CameraNorth.started += CameraViewSouth;
        input.Player.CameraNorth.canceled += CameraViewSouth;
        input.Player.CameraWest.started += CameraViewWest;
        input.Player.CameraWest.canceled += CameraViewWest;
        input.Player.CameraEast.started += CameraViewEast;
        input.Player.CameraEast.canceled += CameraViewEast;

    }

    void CameraViewSouth(InputAction.CallbackContext context)
    {
        isCamPosS = context.ReadValueAsButton();
    }

    void CameraViewWest(InputAction.CallbackContext context)
    {
        isCamPosW = context.ReadValueAsButton();
    }

    void CameraViewEast(InputAction.CallbackContext context)
    {
        isCamPosE = context.ReadValueAsButton();
    }

    private void Update()
    {
        currentView = views[0];

        if (isCamPosS)
        {
            currentView = camPosSouth;
        }

        if(isCamPosE)
        {
            currentView = camPosEast;
        }

        if(isCamPosW)
        {
            currentView = camPosWest;
        }
    }

    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, currentView.position, Time.deltaTime * transitionSpeed);

        Vector3 currentAngle = new Vector3(Mathf.LerpAngle(transform.rotation.eulerAngles.x, currentView.transform.rotation.eulerAngles.x, Time.deltaTime * transitionSpeed),
                                         Mathf.LerpAngle(transform.rotation.eulerAngles.y, currentView.transform.rotation.eulerAngles.y, Time.deltaTime * transitionSpeed),
                                         Mathf.LerpAngle(transform.rotation.eulerAngles.z, currentView.transform.rotation.eulerAngles.z, Time.deltaTime * transitionSpeed));

        transform.eulerAngles = currentAngle;
    }

    private void OnEnable()
    {
        input.Player.Enable();
    }

    private void OnDisable()
    {
        input.Player.Disable();
    }
}
