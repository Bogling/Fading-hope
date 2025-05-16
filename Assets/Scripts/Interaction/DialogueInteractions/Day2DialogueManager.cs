using System.Threading.Tasks;
using UnityEngine;

public class Day2DialogueManager : MonoBehaviour, Interactable, ITalkable
{
    [SerializeField] private TextAsset[] inkJSON;
    [SerializeField] private DialogueController dialogueController;
    [SerializeField] private Fader fader;
    [SerializeField] private Transform position;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] audioClips;
    private GameManager gameManager;
    private SpriteRenderer spriteRenderer;

    private bool acceptedMG = false;
    private bool d1end = false;
    private bool d2end = false;

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = FindFirstObjectByType<GameManager>();
        Lock();
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
        FindFirstObjectByType<PlayerCam>().LookAtPosition(transform, 2);
    }

    public void OperateChoice(int qID, int cID) {
        switch (qID) {
            case 0:
                switch (cID) {
                    case 0:
                        Debug.Log("Answer is yes");
                        gameManager.DoubtedAnswer(false);
                        SaveLoadManager.Save();
                        break;
                    case 1:
                        Debug.Log("Answer is no");
                        gameManager.DoubtedAnswer(true);
                        SaveLoadManager.Save();
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
        }
    }

    public void Lock() {
        isLocked = true;
    }

    public void Unlock() {
        isLocked = false;
    }

    public void Activate() {
        Unlock();
        spriteRenderer.sprite = MiraSpritesData.GetInstance().GetSprite("idefault");
        audioSource.clip = audioClips[1];
        audioSource.Play();
    }

    public async void UponExit() {
        if (!d1end) {
            Lock();
            await Task.Delay(1000);
            if (acceptedMG) {
                ChangeSprite("ihide");
                MiniGame1Manager.GetInstance().StartMiniGame();
            }
            else {
                fader.FadeOut(Color.black, 1f);
                await Task.Delay(1000);
                gameObject.SetActive(false);
                FindFirstObjectByType<DayEnding>().Unlock();
                fader.FadeIn(Color.black, 1f);
                var t = audioSource.time;
                audioSource.clip = audioClips[0];
                audioSource.time = t;
                audioSource.Play();
                gameManager.ChangeMaxMood(-2);
            }
            d1end = true;
        }
        else if (!d2end) {
            Lock();
            await Task.Delay(1000);
            if (acceptedMG) {
                MiniGame2Manager.GetInstance().StartMiniGame();
                var t = audioSource.time;
                audioSource.clip = audioClips[2];
                audioSource.time = t;
                audioSource.Play();
            }
            else {
                fader.FadeOut(Color.black, 1f);
                await Task.Delay(1000);
                gameObject.SetActive(false);
                FindFirstObjectByType<DayEnding>().Unlock();
                fader.FadeIn(Color.black, 1f);
                var t = audioSource.time;
                audioSource.clip = audioClips[0];
                audioSource.time = t;
                audioSource.Play();
                gameManager.ChangeMaxMood(-1);
            }
            d2end = true;
        }
        else {
            fader.FadeOut(Color.black, 1f);
            await Task.Delay(1000);
            FindFirstObjectByType<ClockManager>().SetTime(15, 20, 0);
            gameObject.SetActive(false);
            FindFirstObjectByType<DayEnding>().Unlock();
            fader.FadeIn(Color.black, 1f);
        }
    }

    public void ChangeSprite(int spriteIndex) {
        spriteRenderer.sprite = sprites[spriteIndex];
    }
}
