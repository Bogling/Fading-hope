using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightManager : MonoBehaviour
{
    [SerializeField] private Projectile projectile;
    [SerializeField] private Animator animator;
    private GameManager gameManager;
    [SerializeField] private Transform flashPosition;

    private void Start() {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    public void Flash() {
        if (!gameManager.isLPFull()) {return;}
        Debug.Log("Flash");
        animator.SetTrigger("Flash");
        gameManager.RefreshLP();
        flashPosition.rotation = transform.rotation;
        var newProjectile = Instantiate(projectile, flashPosition.position, flashPosition.rotation);
        //newProjectile.transform.parent = null;
        newProjectile.Launch(flashPosition.right);
    }
    
}
