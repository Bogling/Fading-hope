using Unity.VisualScripting;
using UnityEngine;

public class CloseAnimationDecalTrigger : MonoBehaviour
{
    [SerializeField] private bool animateInRange = false;
    [SerializeField] private GameObject player;
    private AnimatedDecal animatedDecal;

    private void Start() {
        animatedDecal = GetComponent<AnimatedDecal>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject == player) {
            if (animateInRange) {
                animatedDecal.StartAnimation();
            }
            else {
                animatedDecal.StopAnimation();
            }
        }
    }  
    
    private void OnTriggerExit(Collider other) {
        if (other.gameObject == player) {
            if (animateInRange) {
                animatedDecal.StopAnimation();
            }
            else {
                animatedDecal.StartAnimation();
            }
        }
    }
}
