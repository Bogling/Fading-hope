using UnityEngine;

public class LSM : HPBar
{
    Animator animator;
    private bool isBroken;
    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        hp = gameManager.GetHP();
        animator = gameObject.GetComponent<Animator>();
        gameObject.SetActive(true);
    }
    
    public override void UpdateHP() {
        hp = gameManager.GetHP();
        animator.SetFloat("HP", hp);

    }

    public void BreakLSM() {
        isBroken = true;
        animator.SetBool("IsBroken", isBroken);
    }

    public void RepairLSM() {
        isBroken = false;
        animator.SetBool("IsBroken", isBroken);
    }
    
    public bool IsBroken() {
        return isBroken;
    }
}
