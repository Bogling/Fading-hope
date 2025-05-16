using System.Threading.Tasks;
using UnityEngine;

public class Day3DialogueManager : MonoBehaviour, Interactable, ITalkable
{
    [SerializeField] private TextAsset[] inkJSON;
    [SerializeField] private DialogueController dialogueController;
    [SerializeField] private Fader fader;
    [SerializeField] private Transform position;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private Transform focusPos;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;
    private float tempT = 0f;
    private GameManager gameManager;
    private SpriteRenderer spriteRenderer;

    private bool acceptedMG = false;
    private bool d1end = false;
    private bool d2end = false;

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = FindFirstObjectByType<GameManager>();
        Interact();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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
        if (IsCurrentlyInteractable()) {
            Talk(inkJSON[currentInk]);
            currentInk++;
        }
    }

    public void InteractionCanceled() {
        return;
    }

    public void OnHover() {
        return;
    }

    public void OnHoverStop() {
        return;
    }

    public void Talk(TextAsset inkJSON)
    {
        dialogueController.EnterDialogue(inkJSON, this);
        Focus();
    }

    public void ChangeSprite(string spriteID) {
        spriteRenderer.sprite = MiraSpritesData.GetInstance().GetSprite(spriteID);
    }

    public void Focus() {
        FindFirstObjectByType<PlayerCam>().LookAtPosition(focusPos, 2);
    }

    public void OperateChoice(int qID, int cID) {
        switch (qID) {
            case 0:
                switch (cID) {
                    case 0:
                        acceptedMG = true;
                        break;
                    case 1:
                        acceptedMG = false;
                        break;
                }
                break;
            case 1:
                switch (cID) {
                    case 0:
                        tempT = audioSource.time;
                        audioSource.Stop();
                        break;
                    case 1:
                        audioSource.time = tempT;
                        audioSource.Play();
                        break;
                    case 2:
                        audioSource.clip = audioClip;
                        audioSource.Play();
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
            await Task.Delay(1000);
            if (acceptedMG) {
                MiniGame3Manager.GetInstance().StartMiniGame();
            }
            else {
                fader.FadeOut(Color.black, 1f);
                await Task.Delay(1000);
                gameObject.SetActive(false);
                FindFirstObjectByType<DayEnding>().ChangeNextLevel("Day4");
                gameManager.ChangeMaxMood(-2);
                FindFirstObjectByType<DayEnding>().Unlock();
                fader.FadeIn(Color.black, 1f);
            }
            d1end = true;
        }
        else if (!d2end) {
            Lock();
            await Task.Delay(1000);
            if (acceptedMG) {
                MiniGame4Manager.GetInstance().StartMiniGame();
            }
            else {
                fader.FadeOut(Color.black, 1f);
                await Task.Delay(1000);
                gameObject.SetActive(false);
                FindFirstObjectByType<DayEnding>().ChangeNextLevel("Day4");
                gameManager.ChangeMaxMood(-1);
                FindFirstObjectByType<DayEnding>().Unlock();
                fader.FadeIn(Color.black, 1f);
            }
            d2end = true;
        }
        else {
            fader.FadeOut(Color.black, 1f);
            await Task.Delay(1000);
            gameObject.SetActive(false);
            FindFirstObjectByType<ClockManager>().SetTime(18, 20, 0);
            FindFirstObjectByType<DayEnding>().Unlock();
            fader.FadeIn(Color.black, 1f);
        }
    }
}
