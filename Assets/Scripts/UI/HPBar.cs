using Image = UnityEngine.UI.Image;
using TMPro;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    private TMP_Text text;
    protected float hp;
    protected GameManager gameManager;
    [SerializeField] Image image;
    [SerializeField] Sprite[] sprites;
    void Start()
    {
        text = GetComponent<TMP_Text>();
        gameManager = FindFirstObjectByType<GameManager>();
        hp = gameManager.GetHP();
        if (text != null) {
            text.text = hp.ToString();
        }
        UpdateHP();
    }

    public virtual void UpdateHP() {
        hp = gameManager.GetHP();
        if (text != null) {
            text.text = hp.ToString();
        }
        if (image != null) {
            if (hp <= 100 && hp > 75) {
                image.sprite = sprites[0];
                image.color = Color.clear;
            }
            else if (hp <= 75 && hp > 50) {
                image.sprite = sprites[1];
                image.color = Color.white;
            }
            else if (hp <= 50 && hp > 25) {
                image.sprite = sprites[2];
                image.color = Color.white;
            }
            else if (hp <= 25 && hp > 0) {
                image.sprite = sprites[3];
                image.color = Color.white;
            }
        }
    }
}
