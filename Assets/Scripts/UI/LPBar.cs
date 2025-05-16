using UnityEngine;
using TMPro;

public class LPBar : MonoBehaviour
{
    private TMP_Text text;
    private GameManager gameManager;
    [SerializeField] private GameObject lpMask;
    void Start()
    {
        text = GetComponent<TMP_Text>();
        gameManager = FindFirstObjectByType<GameManager>();
        if (text != null) {
            text.text = gameManager.GetLP().ToString();
        }
        float t = 1 - gameManager.GetLP() / gameManager.GetMaxLP();
        lpMask.transform.localScale = new Vector3(t, t, t);
    }

    public void UpdateLP() {
        if (text != null) {
            text.text = gameManager.GetLP().ToString();
        }
        float t = 1 - gameManager.GetLP() / gameManager.GetMaxLP();
        lpMask.transform.localScale = new Vector3(t, t, t);
    }
}
