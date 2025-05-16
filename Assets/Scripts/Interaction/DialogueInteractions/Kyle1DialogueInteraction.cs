using System.Collections;
using UnityEngine;

public class Kyle1DialogueInteraction : MonoBehaviour, Interactable, ITalkable
{
    [SerializeField] private TextAsset[] inkJSON;
    [SerializeField] private DreamDialogueController dreamDialogueController;
    [SerializeField] private Transform player;
    [SerializeField] private Transform kyleHead;
    [SerializeField] private Transform kyleHood;
    [SerializeField] private Transform headForward;
    [SerializeField] private float lookSpeed;
    [SerializeField] private Transform focusPosition;
    [SerializeField] private float focusSpeed;
    private int currentInk = 0;
    private Quaternion lastRotation;
    private float headResetTimer;
    private bool isLooking = false;
    private bool returns = true;
    private bool dialogueEnded = false;
    private DreamPlayerCam playerCam;
    private Quaternion initHeadRotation;
    private Quaternion initHoodRotation;

    void Start()
    {
        playerCam = FindFirstObjectByType<DreamPlayerCam>();
        if (FindFirstObjectByType<GameManager>().GetKyleSp1Par()) {
            gameObject.SetActive(false);
        }
        initHeadRotation = kyleHead.rotation;
        initHoodRotation = kyleHood.rotation;
    }

    public void Interact()
    {
        isLooking = true;
        Talk(inkJSON[currentInk]);
        if (currentInk == 0) {
            currentInk++;
        }
    }

    private void LateUpdate() {
        if (isLooking) {
            if (returns) {
                returns = false;
                lastRotation = kyleHead.rotation;
                kyleHead.forward = kyleHead.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(kyleHead.position - player.position);
            targetRotation.x += (targetRotation.x >= 0) ? -initHeadRotation.x : initHeadRotation.x;
            targetRotation.z += (targetRotation.z >= 0) ? -initHeadRotation.z : initHeadRotation.z;
            lastRotation = Quaternion.Slerp(lastRotation, targetRotation, lookSpeed * Time.deltaTime);
            kyleHead.rotation = lastRotation;
            headResetTimer = 3f;
        }
        else if (!returns) {
            lastRotation = Quaternion.Slerp(lastRotation, headForward.rotation, lookSpeed * Time.deltaTime);
            kyleHead.rotation = lastRotation;
            headResetTimer -= Time.deltaTime;
            if (headResetTimer <= 0) {
                returns = true;
            }
        }
        if (dialogueEnded) {
            if (!GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(playerCam.GetComponent<Camera>()), GetComponent<BoxCollider>().bounds)) {
                FindFirstObjectByType<GameManager>().SetKyleSp1Par();
                gameObject.SetActive(false);
            }
        }
    }

    public void InteractionCanceled()
    {
        return;
    }

    public bool IsCurrentlyInteractable()
    {
        return true;
    }

    public void OnHover()
    {
        return;
    }

    public void OnHoverStop()
    {
        return;
    }

    public void Talk(TextAsset inkJSON)
    {
        Focus();
        dreamDialogueController.EnterDialogue(inkJSON, this);
    }

    public void Focus() {
        FindFirstObjectByType<DreamPlayerCam>().LookAtPosition(focusPosition, focusSpeed);
    }

    public void OperateChoice(int qID, int cID)
    {
        throw new System.NotImplementedException();
    }

    public void UponExit()
    {
        isLooking = false;
        dialogueEnded = true;
        FindFirstObjectByType<GameManager>().SetKyleSp1Par();
    }

    public void CheckIgnored() {
        if (!FindFirstObjectByType<GameManager>().GetKyleSp1Par()) {
            gameObject.SetActive(false);
        }
    }

    public void ChangeSprite(string spriteID) {
        return;
    }
}
