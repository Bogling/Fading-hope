using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    private TMP_Text text;
    private GameManager gameManager;
    void Start()
    {
        text = GetComponent<TMP_Text>();
        gameManager = FindFirstObjectByType<GameManager>();
        text.text = gameManager.GetHP().ToString();
    }

    public void UpdateHP() {
        text.text = gameManager.GetHP().ToString();
    }
}
