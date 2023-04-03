using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity=100f;
    [SerializeField] private Transform playerBody;
    [SerializeField] private bool isInvert = false;

    float mouseX, mouseY, invertMultiplier, xRotation=0f;

    void Start()
    {
        invertMultiplier = isInvert ? 1 : -1;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity*Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation += (mouseY * invertMultiplier);
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
