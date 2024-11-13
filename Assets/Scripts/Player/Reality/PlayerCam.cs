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

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (DialogueController.GetInstance().dialogueIsPlaying) {
            return;
        }
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Math.Clamp(xRotation, -90f, 90f);
        yRotation = Math.Clamp(yRotation, 80f, 280f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public void LookAtPosition(Vector3 lookPosition, float speed) {
        StartCoroutine(LookAtTransformCorutine(lookPosition, speed));
    }

    public void LookAtPosition(Transform lookPosition, float speed) {
        LookAtPosition(lookPosition.position, speed);
    }

    public IEnumerator LookAtTransformCorutine(Vector3 lookPosition, float speed) {
        Vector3 direction = (lookPosition - transform.position).normalized;
        Quaternion targetRotation = transform.rotation * Quaternion.FromToRotation(transform.forward, direction);
        while (Quaternion.Angle(transform.rotation, targetRotation) > 1f){
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, speed);
            transform.localRotation = Quaternion.LookRotation(transform.forward);
            yield return new WaitForEndOfFrame();
        }
    }
}
