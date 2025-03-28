using UnityEngine;

public class KyleD2Trigger : MonoBehaviour, ITalkable
{
    [SerializeField] private GameObject player;
    [SerializeField] private Animator kyleAnimator;
    [SerializeField] private Transform focusPosition;
    [SerializeField] private float focusSpeed;
    private int currentInk;
    private int currentStage;
    void OnTriggerEnter(Collider other)
    {
        Focus();
    }

    public void Focus()
    {
        FindFirstObjectByType<DreamPlayerCam>().LookAtPosition(focusPosition, focusSpeed);
    }

    public void OperateChoice(int qID, int cID)
    {
        throw new System.NotImplementedException();
    }

    public void Talk(TextAsset inkJSON)
    {
        throw new System.NotImplementedException();
    }

    public void UponExit()
    {
        throw new System.NotImplementedException();
    }    

    public void ChangeSprite(string spriteID) {
        return;
    }
}
