using System.Threading.Tasks;
using UnityEngine;

public class Day4DialogueManager : MonoBehaviour, Interactable, ITalkable
{
    [SerializeField] private TextAsset[] inkJSON;
    [SerializeField] private DialogueController dialogueController;
    [SerializeField] private Fader fader;
    [SerializeField] private Transform position;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private SpriteRenderer secondSpriteRenderer;
    [SerializeField] private int fakeAppearLookNeedad;
    [SerializeField] private Transform firstPos;
    [SerializeField] private Transform focusPos;
    private Transform focusPosCurrent;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] audioClips;
    private int lookAwayCount = 0;
    private GameManager gameManager;
    private SpriteRenderer spriteRenderer;

    private bool acceptedMG = false;
    private bool d1end = false;
    private bool d2end = false;
    private bool isLookingAway = false;
    private bool d0end = false;

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

    void LateUpdate()
    {
        if (!isLocked || d0end) {
            return;
        }
        
        if (!GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(FindFirstObjectByType<PlayerCam>().GetComponent<Camera>()), GetComponent<BoxCollider>().bounds)) {
            if (!isLookingAway) {   
                isLookingAway = true;
                lookAwayCount++;
            }
        }
        else {
            if (lookAwayCount >= fakeAppearLookNeedad && isLocked) {
                Unlock();
                spriteRenderer.sprite = MiraSpritesData.GetInstance().GetSprite("iside2");
                secondSpriteRenderer.sprite = MiraSpritesData.GetInstance().GetSprite("iksidedefault");
                focusPosCurrent = firstPos;
                Interact();
            }
            isLookingAway = false;
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
        return;
    }

    public void OnHoverStop() {
        return;
    }

    public void Talk(TextAsset inkJSON)
    {
        Focus();
        dialogueController.EnterDialogue(inkJSON, this);
    }

    public void Focus() {
        FindFirstObjectByType<PlayerCam>().LookAtPosition(focusPosCurrent, 2);
    }
    
    private void Questioned() {
        FindFirstObjectByType<GameManager>().QuestionedOnD4();
    }

    public void OperateChoice(int qID, int cID) {
        switch (qID) {
            case 0:
                break;
            case 1:
                switch (cID) {
                    case 2:
                        Questioned();
                        break;
                }
                break;
            case 2:
                switch (cID) {
                    case 0:
                        acceptedMG = true;
                        break;
                    case 1:
                        acceptedMG = false;
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
        if (!d0end) {
            focusPosCurrent = focusPos;
            fader.FadeOut(Color.black, 1f);
            await Task.Delay(2000);
            gameObject.transform.position = position.position;
            secondSpriteRenderer.sprite = null;
            await Task.Delay(1000);
            Interact();
            fader.FadeIn(Color.black, 1f);
            d0end = true;
            return;
        }
        else if (!d1end) {
            Lock();
            await Task.Delay(1000);
            if (acceptedMG) {
                ChangeSprite("idefault");
                MiniGame5Manager.GetInstance().StartMiniGame();
                var t = audioSource.time;
                audioSource.clip = audioClips[1];
                audioSource.time = t;
                audioSource.Play();
            }
            else {
                fader.FadeOut(Color.black, 1f);
                await Task.Delay(1000);
                gameObject.SetActive(false);
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
                ChangeSprite("idefault");
                MiniGame6Manager.GetInstance().StartMiniGame();
                var t = audioSource.time;
                audioSource.clip = audioClips[2];
                audioSource.time = t;
                audioSource.Play();
            }
            else {
                fader.FadeOut(Color.black, 1f);
                await Task.Delay(1000);
                gameObject.SetActive(false);
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
            FindFirstObjectByType<DayEnding>().Unlock();
            fader.FadeIn(Color.black, 1f);
        }
    }

    public void ChangeSprite(string spriteID) {
        if (spriteID == "iksidedefault" || spriteID == "iksidedisapp") {
            secondSpriteRenderer.sprite = MiraSpritesData.GetInstance().GetSprite(spriteID);
        }
        else {
            spriteRenderer.sprite = MiraSpritesData.GetInstance().GetSprite(spriteID);
        }
    }
}
