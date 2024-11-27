using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Day4DialogueManager : MonoBehaviour, Interactable, ITalkable
{
    [SerializeField] private TextAsset[] inkJSON;
    [SerializeField] private DialogueController dialogueController;
    [SerializeField] private Fader fader;
    [SerializeField] private Transform position;
    [SerializeField] private Sprite[] sprites;
    private GameManager gameManager;
    private SpriteRenderer spriteRenderer;

    private bool acceptedMG = false;
    private bool d1end = false;
    private bool d2end = false;

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = FindFirstObjectByType<GameManager>();
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
        if (IsCurrentlyInteractable()) {
            Talk(inkJSON[currentInk]);
            currentInk++;
        }
    }

    public void InteractionCanceled() {
        return;
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

    public void Focus() {
        FindFirstObjectByType<PlayerCam>().LookAtPosition(transform, 2);
    }

    public void OperateChoice(int qID, int cID) {
        /*switch (qID) {
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
                        acceptedMG = true;
                        break;
                    case 1:
                        Debug.Log("Answer is no1");
                        acceptedMG = false;
                        break;
                }
                break;
            case 2:
                switch (cID) {
                    case 0:
                        Debug.Log("Answer is yes2");
                        break;
                    case 1:
                        Debug.Log("Answer is no2");
                        break;
                }
                break;
            case 3:
                switch (cID) {
                    case 0:
                        Debug.Log("Answer is yes2");
                        break;
                    case 1:
                        Debug.Log("Answer is no2");
                        break;
                }
                break;
        }*/
    }

    public void Lock() {
        isLocked = true;
    }

    public void Unlock() {
        isLocked = false;
    }

    public async void UponExit() {
        // TEMPORARY || DELETE LATER //
        acceptedMG = true;
        if (!d1end) {
            Lock();
            //fader.FadeOut(Color.black, 1f);
            await Task.Delay(1000);
            if (acceptedMG) {
            //transform.position = position.position;
                ChangeSprite(1);
                //fader.FadeIn(Color.black, 1f);
                MiniGame5Manager.GetInstance().StartMiniGame();
            }
            else {
                fader.FadeIn(Color.black, 1f);
                await Task.Delay(1000);
                gameObject.SetActive(false);
                FindFirstObjectByType<DayEnding>().Unlock();
                fader.FadeIn(Color.black, 1f);
            }
            d1end = true;
        }
        else if (!d2end) {
            Lock();
            //fader.FadeOut(Color.black, 1f);
            await Task.Delay(1000);
            if (acceptedMG) {
            //transform.position = position.position;
                ChangeSprite(1);
                //fader.FadeIn(Color.black, 1f);
                MiniGame4Manager.GetInstance().StartMiniGame();
            }
            else {
                fader.FadeIn(Color.black, 1f);
                await Task.Delay(1000);
                gameObject.SetActive(false);
                FindFirstObjectByType<DayEnding>().Unlock();
                fader.FadeIn(Color.black, 1f);
            }
            d2end = true;
        }
        else {
            fader.FadeOut(Color.black, 1f);
            await Task.Delay(1000);
            gameObject.SetActive(false);
            FindFirstObjectByType<DayEnding>().Unlock();
            fader.FadeIn(Color.black, 1f);
        }
    }

    public void ChangeSprite(int spriteIndex) {
        spriteRenderer.sprite = sprites[spriteIndex];
    }
}
