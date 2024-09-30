using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Day2DialogueManager : MonoBehaviour, Interactable, ITalkable
{
    [SerializeField] private TextAsset[] inkJSON;
    [SerializeField] private DialogueController dialogueController;
    [SerializeField] private Fader fader;
    [SerializeField] private Transform position;
    [SerializeField] private Sprite[] sprites;
    private GameManager gameManager;
    private SpriteRenderer spriteRenderer;
    private MiniGame1Manager mg1;
    private MiniGame2Manager mg2;
    private bool d1end = false;

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = FindFirstObjectByType<GameManager>();
        mg1 = FindFirstObjectByType<MiniGame1Manager>();
        mg2 = FindFirstObjectByType<MiniGame2Manager>();
    }

    private bool isLocked = false;
    private int currentInk = 0;
    public bool IsCurrentlyInteractable() {
        if (isLocked) {
            return false;
        }
        else {
            return true;
        }
    }
 
    public void Interact() {
        Debug.Log("Hoba1");
        Talk(inkJSON[currentInk]);
        currentInk++;
    }

    public void OnHover() {
        //Debug.Log("Hovered");
    }

    public void OnHoverStop() {
        Debug.Log("Released");
    }

    public void Talk(TextAsset inkJSON)
    {
        dialogueController.EnterDialogue(inkJSON, this);
    }

    public void OperateChoice(int qID, int cID) {
        switch (qID) {
            case 0:
                switch (cID) {
                    case 0:
                        Debug.Log("Answer is yes");
                        break;
                    case 1:
                        Debug.Log("Answer is no");
                        gameManager.DoubtedAnswer();
                        break;
                }
                break;
            case 1:
                switch (cID) {
                    case 0:
                        Debug.Log("Answer is yes1");
                        MiniGame1Manager.GetInstance().StartMiniGame();
                        isLocked = true;
                        currentInk++;
                        break;
                    case 1:
                        Debug.Log("Answer is no1");
                        break;
                }
                break;
        }
    }

    public void Lock() {
        isLocked = true;
    }

    public void Unlock() {
        isLocked = false;
    }

    public async void UponExit() {
        if (!d1end) {
            Lock();
            fader.FadeOut(Color.black, 1f);
            await Task.Delay(1000);
            transform.position = position.position;
            spriteRenderer.sprite = sprites[1];
            fader.FadeIn(Color.black, 1f);
            Interact();
            d1end = true;
        }
        else {
            fader.FadeOut(Color.black, 1f);
            await Task.Delay(1000);
            gameObject.SetActive(false);
            FindFirstObjectByType<DayEnding>().Unlock();
            fader.FadeIn(Color.black, 1f);
        }
    }
}
