using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;

    public float rotationSpeed;
    public Transform combatLookAt;

    [Header("Camera References")]
    public GameObject combatCam;
    public GameObject thirdPersonCam;
    public GameObject topDownCam;

    public CameraStyle currStyle;

    public enum CameraStyle
    {
        Basic,
        Combat,
        Topdown
    }


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        GetSwitchInput();
        RotatePlayer();   
    }

    private void RotatePlayer()
    {
        // Rotate Orientation
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        // Rotate Player
        if (currStyle == CameraStyle.Basic || currStyle == CameraStyle.Topdown)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

            if (inputDir != Vector3.zero)
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }

        else if (currStyle == CameraStyle.Combat)
        {
            Vector3 combatViewDir = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
            orientation.forward = combatViewDir.normalized;

            playerObj.forward = combatViewDir.normalized;
        }
    }

    private void GetSwitchInput()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1)) SwitchCamStyle(CameraStyle.Basic);
        if (Input.GetKeyDown(KeyCode.Keypad2)) SwitchCamStyle(CameraStyle.Combat);
        if (Input.GetKeyDown(KeyCode.Keypad3)) SwitchCamStyle(CameraStyle.Topdown);
    }

    private void SwitchCamStyle(CameraStyle newStyle)
    {
        combatCam.SetActive(false);
        thirdPersonCam.SetActive(false);
        topDownCam.SetActive(false);

        if (newStyle == CameraStyle.Basic) thirdPersonCam.SetActive(true);
        if (newStyle == CameraStyle.Combat) combatCam.SetActive(true);
        if (newStyle == CameraStyle.Topdown) topDownCam.SetActive(true);

        currStyle = newStyle;
    }
}
