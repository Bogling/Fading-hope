using UnityEngine;

public class FlashlightManager : MonoBehaviour
{
    [SerializeField] private Projectile projectile;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject spotLight1;
    [SerializeField] private GameObject spotLight2;
    private GameManager gameManager;
    [SerializeField] private Transform flashPosition;
    [SerializeField] private AudioSource turnOnClip;
    [SerializeField] private AudioSource turnOffClip;
    [SerializeField] private AudioSource flashClip;
    private bool isOn = true;
    private bool isLocked = false;

    private void Start() {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    public void Flash() {
        if (!gameManager.isLPFull() || isLocked) {return;}
        animator.SetTrigger("Flash");
        gameManager.RefreshLP();
        flashPosition.rotation = transform.rotation;
        flashClip.Stop();
        flashClip.Play();
        var newProjectile = Instantiate(projectile, flashPosition.position, flashPosition.rotation);
        newProjectile.Launch(flashPosition.right);
    }

    public void ToggleFlashlight() {
        if (isOn) {
            spotLight1.SetActive(false);
            spotLight2.SetActive(false);
            turnOffClip.Stop();
            turnOffClip.Play();
            isOn = false;
        }
        else {
            spotLight1.SetActive(true);
            spotLight2.SetActive(true);
            turnOnClip.Stop();
            turnOnClip.Play();
            isOn = true;
        }
    }

    public void Lock() {
        isLocked = true;
    }

    public void Unlock() {
        isLocked = false;
    }    
}
