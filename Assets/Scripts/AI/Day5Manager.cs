using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Day5Manager : MonoBehaviour, ITalkable
{
    [SerializeField] private KylePendant pendant;
    [SerializeField] private ClockManager clock;
    [SerializeField] private int stagesCount;
    [SerializeField] private float eventStartDelay;
    [SerializeField] private float[] eventCheckDelay;
    [SerializeField] private int maxActiveEventsCount;
    private int activeEventsCount = 0;
    [SerializeField] private int[] maxEventOverallWeight;
    private int currentEventOverallWeight = 0;
    [SerializeField] private List<EventList> day5Events;
    private int currentStage = 0;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private TextAsset[] inkJSON;
    [SerializeField] private DialogueController dialogueController;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private AudioClip audioClip2;
    [SerializeField] private Fader fader;
    [SerializeField] private Transform focusPoint;
    [SerializeField] private ScreamerManager screamerManager;
    [SerializeField] private Portal portal;
    [SerializeField] private FakeFear fakeFear;

    private bool dayActive = false;
    private static Day5Manager instance = null;
    private bool d1end = false; 
    private bool d2end = false; 
    private bool choice = false;
    private List<Day5Event> uniqueEvents = new List<Day5Event>();


    private void Awake() {
        instance = this;
    }
    private void Start() {
        if (day5Events.Count != stagesCount) {
            Debug.LogError("Day5Manager: day5Events.Count != stagesCount");
        }
        pendant.gameObject.SetActive(false);
        Talk(inkJSON[0]);
        //StartDay5();
        foreach (EventList l in day5Events) {
            foreach (Day5Event e in l.events) {
                if (uniqueEvents.Contains(e)) {
                    continue;
                }
                else {
                    uniqueEvents.Add(e);
                }
            }
        }
    }

    public static Day5Manager GetInstance() {
        return instance;
    }

    public IEnumerator StartDay5(float startDelay) {
        FindFirstObjectByType<GameManager>().SetMaxHP(FindFirstObjectByType<GameManager>().GetMaxMood() * 10);
        yield return new WaitForSeconds(startDelay);
        audioSource.clip = audioClip;
        audioSource.Play();
        clock.StartClock();
        dayActive = true;
        StartCoroutine(ManageEvents());
        pendant.Unlock();
    }
    
    public IEnumerator ManageEvents() {
        int timeInterval = CountTimeInterval();
        yield return new WaitForSeconds(eventStartDelay);
        while (dayActive) {
            if (currentStage >= stagesCount) {
                yield return new WaitForSeconds(eventStartDelay);
            }  
            else {
                int index = Random.Range(0, day5Events[currentStage].events.Count);

                if (day5Events[currentStage].events[index].EventWeight + currentEventOverallWeight > maxEventOverallWeight[currentStage]
                || day5Events[currentStage].events[index].IsActive()
                || day5Events[currentStage].events[index].IsResting()) {
                    yield return new WaitForSeconds(eventStartDelay);
                }
                else {
                    TriggerEvent(currentStage, index);
                }
                yield return new WaitForSeconds(eventCheckDelay[currentStage]);
            }
        }
    }

    public int CountTimeInterval() {
        return clock.GetWholeTime() / stagesCount;
    }

    public void ChangeStage() {
        currentStage++;
        if (currentStage == stagesCount - 2) {
            foreach (Day5Event e in uniqueEvents) {
                e.Enrage();
            }
        }
        FindFirstObjectByType<RandomSoundManager>().Activate();
    }

    public void TriggerEvent(int stage, int index) {
        day5Events[stage].events[index].StartEvent();
        activeEventsCount++;
        currentEventOverallWeight += day5Events[stage].events[index].EventWeight;
    }

    public void OnEventEnded(int weight) {
        activeEventsCount--;
        currentEventOverallWeight -= weight;
    }

    public void Focus()
    {
        FindFirstObjectByType<PlayerCam>().LookAtPosition(focusPoint, 2);
    }

    public void Talk(TextAsset inkJSON)
    {
        Focus();
        dialogueController.EnterDialogue(inkJSON, this);
    }

    public void OperateChoice(int qID, int cID)
    {
        switch (qID) {
            case 0:
                pendant.gameObject.SetActive(true);
                break;
            case 1: 
                switch (cID) {
                    case 0:
                        choice = true;
                        FindFirstObjectByType<GameManager>().SetChoice2(true);
                        portal.ShowPortal();
                        break;
                    case 1:
                        choice = false;
                        FindFirstObjectByType<GameManager>().SetChoice2(false);
                        audioSource.Stop();
                        break;
                }
                break;
        }
    }

    public async void UponExit()
    {
        if (!d1end) {
            d1end = true;
            fader.FadeOut(Color.black, 1f);
            await Task.Delay(1000);
            spriteRenderer.sprite = null;
            StartCoroutine(StartDay5(10f));
            fader.FadeIn(Color.black, 1f);
        }
        else if (!d2end) {
            if (!choice) {
                fakeFear.ShowFear();
            }
        }
    }

    public void ChangeSprite(string spriteID)
    {
        spriteRenderer.sprite = MiraSpritesData.GetInstance().GetSprite(spriteID);
    }

    public void GameOver() {
        var tempmax = 0;
        Day5Event maxevent = null;
        foreach (Day5Event e in uniqueEvents) {
            if (e.IsActive() && e.EventWeight > tempmax) {
                maxevent = e;
            }
        }
        if (maxevent != null) {
            screamerManager.ShowScreamer(maxevent.ScreemerID);
            dayActive = false;
            gameObject.SetActive(false);
        }
    }

    public IEnumerator EndDay() {
        dayActive = false;
        FindFirstObjectByType<RandomSoundManager>().Deactivate();
        foreach (Day5Event e in uniqueEvents) {
            e.EndEvent();
        }

        audioSource.Stop();
        FindFirstObjectByType<Fader>().FadeOut(Color.white, 0.1f);
        if (pendant.IsUp()) {
            pendant.Interact();
        }
        yield return new WaitForSeconds(5f);
        Talk(inkJSON[1]);
        audioSource.clip = audioClip2;
        audioSource.Play();
        FindFirstObjectByType<Fader>().FadeIn(Color.white, 0.1f);
        FindFirstObjectByType<GameManager>().BeatDay5();
    }
}


[System.Serializable]
public class EventList 
{
    public List<Day5Event> events;
}