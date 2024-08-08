using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LPBar : MonoBehaviour
{
    private TMP_Text text;
    private GameManager gameManager;
    void Start()
    {
        text = GetComponent<TMP_Text>();
        gameManager = FindFirstObjectByType<GameManager>();
        text.text = gameManager.GetLP().ToString();
    }

    public void UpdateLP() {
        text.text = gameManager.GetLP().ToString();
    }
}
