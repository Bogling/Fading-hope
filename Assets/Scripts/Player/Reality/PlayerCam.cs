using System;
using UnityEngine;
using System.Collections;

public class PlayerCam : MonoBehaviour
{

    [SerializeField]
    private float sensX;
    [SerializeField]
    private float sensY;
    [SerializeField]
    private Transform   orientation;

    private float xRotation;
    private float yRotation;
    private IEnumerator c;
    private bool isFollowingStopped = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (isFollowingStopped) return;
        if (DialogueController.GetInstance().dialogueIsPlaying) {
            return;
        }
        if (c != null) {
            StopCoroutine(c);
            c = null;
        }
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public void LookAtPosition(Vector3 lookPosition, float speed) {
        if (c != null) {
            StopCoroutine(c);
        }
        c = LookAtTransformCorutine(lookPosition, speed);
        StartCoroutine(c);
    }

    public void LookAtPosition(Transform lookPosition, float speed) {
        LookAtPosition(lookPosition.position, speed);
    }

    public IEnumerator LookAtTransformCorutine(Vector3 lookPosition, float speed) {
        Quaternion targetRotation = Quaternion.LookRotation(lookPosition - transform.position);
        while (Quaternion.Angle(transform.rotation, targetRotation) > 1f){
            targetRotation = Quaternion.LookRotation(lookPosition - transform.position);
            Quaternion rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
            xRotation = rotation.eulerAngles.x;
            if (xRotation > 180) {
                xRotation -= 360;
            }
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            yRotation = rotation.eulerAngles.y;
            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
            yield return new WaitForEndOfFrame();
        }
    }

}
