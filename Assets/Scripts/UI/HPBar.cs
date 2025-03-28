using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    private TMP_Text text;
    protected float hp;
    protected GameManager gameManager;
    void Start()
    {
        text = GetComponent<TMP_Text>();
        gameManager = FindFirstObjectByType<GameManager>();
        hp = gameManager.GetHP();
        if (text != null) {
            text.text = hp.ToString();
        }
    }

    public virtual void UpdateHP() {
        hp = gameManager.GetHP();
        if (text != null) {
            text.text = hp.ToString();
        }
    }
}
