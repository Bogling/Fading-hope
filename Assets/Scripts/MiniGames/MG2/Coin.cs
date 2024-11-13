using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private Animator animator;
    private MiniGame2Manager mg2Man;
    private bool currentOption;
    void Awake() {
        gameObject.SetActive(false);
        mg2Man = FindFirstObjectByType<MiniGame2Manager>();
        animator = GetComponent<Animator>();
    }

    public void Flip() {
        gameObject.SetActive(true);
        animator.SetTrigger("flip");
    }

    private void EndFlip() {
        currentOption = Random.Range(0, 2) == 1;
        mg2Man.SetOption(currentOption);
        gameObject.SetActive(false);
    }
}
