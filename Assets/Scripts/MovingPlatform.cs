using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float StartSpeed;
    [SerializeField] private float TravelSpeed;
    [SerializeField] private float EndSpeed;
    [SerializeField] private float Delay;
    [SerializeField] private bool isLooping;
    [SerializeField] private bool deactivateOnArrival;
    [SerializeField] private int MoveTimesBeforeDeactivation;
    [SerializeField] private Transform StartPosition;
    [SerializeField] private Transform EndPosition;
    [SerializeField] private float Range;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject player;

    private Rigidbody rb;
    private bool isPlayerAttached = false;

    private void Start() {
        rb = GetComponent<Rigidbody>();
    }
    public IEnumerator Move() {
        //Waiting Initial Delay
        yield return new WaitForSeconds(Delay);
        int timesLeft = MoveTimesBeforeDeactivation;
        float counter;
        if (isLooping) {
            while(true) {
                //Start Delay
                yield return new WaitForSeconds(StartSpeed);
                //Transfer loop
                while(Vector3.Distance(transform.position, EndPosition.position) > Range) {
                    Debug.Log(Vector3.Distance(transform.position, EndPosition.position));
                    counter = TravelSpeed * Time.timeScale;
                    rb.AddForce((EndPosition.position - StartPosition.position) * counter, ForceMode.Acceleration);
                    yield return 0;
                }
                counter = 0;
                while(Vector3.Distance(transform.position, EndPosition.position) > 0) {
                    Debug.Log(Vector3.Distance(transform.position, EndPosition.position) + "Lerping");
                    counter += 1f * Time.timeScale;
                    transform.position = Vector3.Lerp(transform.position, EndPosition.position, counter);
                    yield return 0;
                }
                rb.AddForce(Vector3.zero, ForceMode.Acceleration);
                rb.linearVelocity = Vector3.zero;
                //End Delay
                yield return new WaitForSeconds(EndSpeed);
                //transform.position = EndPosition.position;
                var temp = StartPosition;
                StartPosition = EndPosition;
                EndPosition = temp;
            }
        }
        else {
            while (timesLeft >= 0) {
                //Start Delay
                yield return new WaitForSeconds(StartSpeed);
                //Transfer loop
                while(Vector3.Distance(transform.position, EndPosition.position) > Range) {
                    counter = TravelSpeed * Time.timeScale;
                    rb.AddForce((EndPosition.position - StartPosition.position) * counter, ForceMode.Acceleration);
                    yield return 0;
                }
                counter = 0;
                while(Vector3.Distance(transform.position, EndPosition.position) > 0) {
                        Debug.Log(Vector3.Distance(transform.position, EndPosition.position) + "Lerping");
                        counter += 0.1f * Time.timeScale;
                        transform.position = Vector3.Lerp(transform.position, EndPosition.position, counter);
                        yield return 0;
                    }
                timesLeft--;
                rb.AddForce(Vector3.zero, ForceMode.Acceleration);
                rb.linearVelocity = Vector3.zero;
                //End Delay
                yield return new WaitForSeconds(EndSpeed);
                //transform.position = EndPosition.position;
                var temp = StartPosition;
                StartPosition = EndPosition;
                EndPosition = temp;
            }
            if (deactivateOnArrival) {
                Deactivate();
            }
            rb.AddForce(Vector3.zero, ForceMode.Acceleration);
            rb.linearVelocity = Vector3.zero;
            //transform.position = EndPosition.position;
        }
    }

    public void Activate() {
        animator.SetTrigger("Activate");
        StartCoroutine(Move());
    }

    public virtual void Deactivate() {
        animator.SetTrigger("Deactivate");
        StopCoroutine(Move());
    }

    private void OnCollisionEnter(Collision other) {
        Debug.Log("collEnter");
        if (other.gameObject == player) {
            Debug.Log("collParent");
            player.transform.parent = gameObject.transform;
            isPlayerAttached = true;
        }
    }

    private void OnCollisionExit(Collision other) {
        if (isPlayerAttached) {
            player.transform.parent = null;
        }
    }
}
