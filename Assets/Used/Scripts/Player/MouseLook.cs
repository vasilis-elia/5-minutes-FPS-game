using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody; // We link this object to the FirstPersonPlayer object, therefore camera also transforms
    float xRotation = 0f;

    void Start()
    {
        if (!PauseMenu.isPaused)
        {
            Cursor.lockState = CursorLockMode.Locked; // To lock cursor in game window 
            Cursor.visible = false;
        }
    }
 
    void Update()
    {
        if (GameManager.gameOver)
            return;

        if (!PauseMenu.isPaused)
        {           
            Cursor.lockState = CursorLockMode.Locked; // To lock cursor in game window 
            Cursor.visible = false;
        }
        else
        {            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        float mouseX = Input.GetAxis(StringRepo.mouseX) * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis(StringRepo.mouseY) * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // So the camera does not flip vertically    

        playerBody.Rotate(0f, mouseX, 0f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);      
    }
}
