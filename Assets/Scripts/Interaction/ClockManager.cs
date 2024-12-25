using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockManager : MonoBehaviour, Interactable, ITalkable
{
    [SerializeField] private GameObject arrowS;
    [SerializeField] private GameObject arrowM;
    [SerializeField] private GameObject arrowH;
    [Tooltip("0 = hours, 1 = minutes, 2 = seconds")]
    [SerializeField] private List<int> initialTime = new List<int> { 0, 0, 0 };
    [SerializeField] private List<int> endTime = new List<int> { 0, 0, 0 };
    private List<int> currentTime = new List<int> { 0, 0, 0 };
    [SerializeField] private float timeSpeed;
    [SerializeField] private TextAsset timeInkJSON;
    [SerializeField] private bool isInteractable;
    private DialogueController dialogueController;
    private int interval;

    

    private void Start()
    {
        dialogueController = DialogueController.GetInstance();
        currentTime = initialTime;

        SetRotation();
        interval = Day5Manager.GetInstance().CountTimeInterval();
    }

    private List<float> GetRotationFromTime(List<int> time) {
        List<float> rotations = new List<float> { 0, 0, 0 };
        
        rotations[2] = time[2] * 6f;

        rotations[1] = (time[1] * 6f) + (time[2] / 10f);

        rotations[0] = ((time[0] % 12) * 30f) + (time[1] / 2f) + (time[2] / 120f);

        return rotations;
    }

    private void SetRotation() {
        List<float> rotations = GetRotationFromTime(currentTime);

        arrowS.transform.localEulerAngles = new Vector3(rotations[2] - 90, 0, 90);
        arrowM.transform.localEulerAngles = new Vector3(rotations[1] - 90, 0, 90);
        arrowH.transform.localEulerAngles = new Vector3(rotations[0] - 90, 0, 90);
    }

    public void StartClock() {
        StartCoroutine(ClockTick());
    }

    private IEnumerator ClockTick() {
        while (currentTime[0] != endTime[0] || currentTime[1] != endTime[1] || currentTime[2] != endTime[2]) {
            currentTime[2]++;
            if (currentTime[2] == 60) {
                currentTime[2] = 0;
                currentTime[1]++;
                if (currentTime[1] == 60) {
                    currentTime[1] = 0;
                    currentTime[0]++;
                    if (currentTime[0] == 24) {
                        currentTime[0] = 0;
                    }
                }
            }

            SetRotation();
            if (CompareTime(interval)) {
                Day5Manager.GetInstance().ChangeStage();
                interval += Day5Manager.GetInstance().CountTimeInterval();
            }
            yield return new WaitForSeconds(timeSpeed);            
        }

        StopClock();
    }

    public int GetWholeTime() {
        int wholeTime = 0;

        wholeTime += Mathf.Abs(endTime[0] - initialTime[0]) * 3600;
        wholeTime += (60 + endTime[1] - initialTime[1]) * 60;
        wholeTime += 60 + endTime[2] - initialTime[2];

        return wholeTime;
    }

    public bool CompareTime(int intTime) {
        int hour = intTime / 3600;
        int minute = intTime % 3600 / 60;
        int second = intTime % 3600 % 60;

        return currentTime[0] == hour && currentTime[1] == minute && currentTime[2] == second;
    }

    public void StopClock() {

    }

    public bool IsCurrentlyInteractable()
    {
        return isInteractable;
    }

    public void Interact()
    {
        if (IsCurrentlyInteractable()) {
            StartClock(); // <--- Delete this
            Talk(timeInkJSON);
        }
    }

    public void OnHover()
    {
        return;
    }

    public void OnHoverStop()
    {
        return;
    }

    public void InteractionCanceled()
    {
        return;
    }

    public void Focus()
    {
        FindFirstObjectByType<PlayerCam>().LookAtPosition(transform, 0.5f);
    }

    public void Talk(TextAsset inkJSON)
    {
        string text = (currentTime[0] % 12 == 0 ? 12 : currentTime[0] % 12) + ":" + currentTime[1] + " " + (currentTime[0] >= 12 ? "PM" : "AM");
        dialogueController.EnterDialogue(inkJSON, this, "time", text);
    }

    public void OperateChoice(int qID, int cID)
    {
        return;
    }

    public void UponExit()
    {
        return;
    }
}
