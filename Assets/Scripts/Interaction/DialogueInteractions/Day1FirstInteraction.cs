using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Day1FirstInteraction : MonoBehaviour, Interactable, ITalkable
{
    [SerializeField] private TextAsset[] inkJSON;
    [SerializeField] private DialogueController dialogueController;
    [SerializeField] private Fader fader;
    [SerializeField] private Transform position;
    [SerializeField] private Sprite[] sprites;
    private SpriteRenderer spriteRenderer;
    private bool d1end = false;
    [SerializeField] private int fakeAppearLookNeedad;
    private int lookAwayCount = 0;
    private bool isLookingAway = false;

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        Debug.Log("Hoba1");
        Talk(inkJSON[currentInk]);
        currentInk++;
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
        FindFirstObjectByType<PlayerCam>().LookAtPosition(transform, 0.5f);
    }

    public void OperateChoice(int qID, int cID) {
        return;
    }

    public void ChangeSprite(string spriteID) {
        spriteRenderer.sprite = MiraSpritesData.GetInstance().GetSprite(spriteID);
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

    void LateUpdate()
    {
        if (!isLocked) {
            return;
        }
        
        if (!GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(FindFirstObjectByType<PlayerCam>().GetComponent<Camera>()), GetComponent<BoxCollider>().bounds)) {
            if (!isLookingAway) {
                if (lookAwayCount >= fakeAppearLookNeedad) {
                    Unlock();
                    spriteRenderer.sprite = sprites[0];
                }
                else {
                    isLookingAway = true;
                    lookAwayCount++;
                }
            }
        }
        else {
            isLookingAway = false;
        }
    }
}
