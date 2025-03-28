using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day5Manager : MonoBehaviour
{
    [SerializeField] private KylePendant pendant;
    [SerializeField] private ClockManager clock;
    [SerializeField] private int stagesCount;
    [SerializeField] private float eventStartDelay;
    [SerializeField] private float eventCheckDelay;
    [SerializeField] private int maxActiveEventsCount;
    private int activeEventsCount = 0;
    [SerializeField] private int maxEventOverallWeight;
    private int currentEventOverallWeight = 0;
    [SerializeField] private List<EventList> day5Events;
    private int currentStage = 0;
    private static Day5Manager instance = null;


    private void Awake() {
        instance = this;
    }
    private void Start() {
        if (day5Events.Count != stagesCount) {
            Debug.LogError("Day5Manager: day5Events.Count != stagesCount");
        }

        StartDay5();
    }

    public static Day5Manager GetInstance() {
        return instance;
    }

    public void StartDay5() {
        clock.StartClock();
        StartCoroutine(ManageEvents());
        pendant.Unlock();
    }
    
    public IEnumerator ManageEvents() {
        int timeInterval = CountTimeInterval();
        yield return new WaitForSeconds(eventStartDelay);
        while (true) {
            if (currentStage >= stagesCount) {
                yield return new WaitForSeconds(eventStartDelay);
            }  
            else {
                int stage = Random.Range(0, currentStage + 1);
                int index = Random.Range(0, day5Events[stage].events.Count);

                if (day5Events[stage].events[index].EventWeight + currentEventOverallWeight > maxEventOverallWeight
                || day5Events[stage].events[index].IsActive()
                || day5Events[stage].events[index].IsResting()) {
                    yield return new WaitForSeconds(eventStartDelay);
                }
                else {
                    TriggerEvent(stage, index);
                }
                yield return new WaitForSeconds(eventCheckDelay);
            }
        }
    }

    public int CountTimeInterval() {
        return clock.GetWholeTime() / stagesCount;
    }

    public void ChangeStage() {
        currentStage++;
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
}


[System.Serializable]
public class EventList 
{
    public List<Day5Event> events;
}