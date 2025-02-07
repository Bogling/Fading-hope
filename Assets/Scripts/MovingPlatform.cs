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
    [SerializeField] private float MaxVelocity;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject player;

    private Rigidbody rb;
    private bool isPlayerAttached = false;
    private bool isMovingPlayer = false;
    private bool isMoving = false;
    private bool isMovingBack = false;

    private void Start() {
        rb = GetComponent<Rigidbody>();
    }
    public IEnumerator Move() {
        if (isMoving) {
            yield break;
        }
        isMoving = true;
        //Waiting Initial Delay
        yield return new WaitForSeconds(Delay);
        int timesLeft = MoveTimesBeforeDeactivation;
        float counter;
        if (isLooping) {
            while(true) {
                //Start Delay
                yield return new WaitForSeconds(StartSpeed);
                //Transfer loop
                while(Vector3.Distance(rb.position, EndPosition.position) > Range) {
                    if (isPlayerAttached && !isMovingPlayer) {
                        //player.transform.parent = gameObject.transform;
                        //isMovingPlayer = true;
                    }
                    Debug.Log(Vector3.Distance(rb.position, EndPosition.position));
                    counter = TravelSpeed * Time.timeScale;
                    if (rb.linearVelocity.magnitude <= MaxVelocity) {
                        rb.AddForce((EndPosition.position - StartPosition.position) * counter, ForceMode.Acceleration);
                    }
                    yield return 0;
                }
                counter = 0;
                while(Vector3.Distance(rb.position, EndPosition.position) > 0) {
                    if (isPlayerAttached && !isMovingPlayer) {
                        //player.transform.parent = gameObject.transform;
                        //isMovingPlayer = true;
                    }
                    Debug.Log(Vector3.Distance(rb.position, EndPosition.position) + "Lerping");
                    counter += 1f * Time.deltaTime;
                    rb.position = Vector3.Lerp(rb.position, EndPosition.position, counter);
                    if (counter > 1) {
                        rb.position = EndPosition.position;
                        break;
                    }
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
                isMovingBack = !isMovingBack;
            }
        }
        else {
            while (timesLeft >= 0) {
                //Start Delay
                yield return new WaitForSeconds(StartSpeed);
                //Transfer loop
                while(Vector3.Distance(rb.position, EndPosition.position) > Range) {
                    counter = TravelSpeed * Time.timeScale;
                    if (rb.linearVelocity.magnitude <= MaxVelocity) {
                        rb.AddForce((EndPosition.position - StartPosition.position) * counter, ForceMode.Acceleration);
                    }
                    if (isPlayerAttached) {
                        //player.gameObject.GetComponent<Rigidbody>().linearVelocity = rb.linearVelocity;
                    }
                    yield return 0;
                }
                counter = 0;
                rb.AddForce(Vector3.zero, ForceMode.Acceleration);
                rb.linearVelocity = Vector3.zero;
                //player.gameObject.GetComponent<Rigidbody>().linearVelocity = rb.linearVelocity;
                while(Vector3.Distance(rb.position, EndPosition.position) > 0) {
                        Debug.Log(Vector3.Distance(rb.position, EndPosition.position) + "Lerping");
                        counter += 1f * Time.deltaTime;
                        rb.position = Vector3.Lerp(rb.position, EndPosition.position, counter);
                        //if (isPlayerAttached) {
                        //    player.gameObject.GetComponent<Rigidbody>().linearVelocity = rb.linearVelocity;
                        //}
                        if (counter > 1) {
                            rb.position = EndPosition.position;
                            break;
                        }
                        yield return 0;
                    }
                timesLeft--;
                rb.AddForce(Vector3.zero, ForceMode.Acceleration);
                rb.linearVelocity = Vector3.zero;
                //player.gameObject.GetComponent<Rigidbody>().linearVelocity = rb.linearVelocity;
                //End Delay
                yield return new WaitForSeconds(EndSpeed);
                //transform.position = EndPosition.position;
                var temp = StartPosition;
                StartPosition = EndPosition;
                EndPosition = temp;
                isMovingBack = !isMovingBack;
            }
            if (deactivateOnArrival) {
                Deactivate();
            }
            rb.AddForce(Vector3.zero, ForceMode.Acceleration);
            rb.linearVelocity = Vector3.zero;
            isMoving = false;
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
            if (isMoving || isMovingBack) {
                //player.transform.parent = gameObject.transform;
                //isMovingPlayer = true;
            }
            player.transform.parent = gameObject.transform;
            //StopCoroutine("UnAttachPlayer");
            isPlayerAttached = true;
        }
    }

    private void OnCollisionExit(Collision other) {
        if (isPlayerAttached) {
            //StartCoroutine("UnAttachPlayer");
            if (player.transform.parent == gameObject.transform) {
                player.transform.parent = null;
            }
            isPlayerAttached = false;
            //isMovingPlayer = false;
        }
    }

    private IEnumerator UnAttachPlayer() {
        yield return new WaitForSeconds(0.5f);
        player.transform.parent = null;
        isPlayerAttached = false;
    }

    public bool IsMovingBack() {
        return isMovingBack;
    }
}
