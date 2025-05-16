using System;
using System.Collections.Generic;
using UnityEngine;

public class Dream4Manager : MonoBehaviour
{

    [SerializeField] private List<CheckPointDoorStruct> doorsToLockList;
    [SerializeField] private List<CheckPointDoorStruct> doorsToOpenList;
    [SerializeField] private List<CheckPointLeverStruct> leverList;
    [SerializeField] private List<CheckPointMapStruct> mapPiecesList;
    [SerializeField] private List<CheckPointAmbientStruct> ambientList;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private BigGreenCrystal bigGreenCrystal;
    [SerializeField] private List<GreenCrystal> greenCrystals;
    [SerializeField] private Door crystal1Door;
    [SerializeField] private Door crystal2Door;
    [SerializeField] private Door crystal3Door;
    [SerializeField] private DoorBigManager bigDoor;
    [SerializeField] private LightReceiver bigDoorReceiver1;
    [SerializeField] private Door waterLabDoor1;
    [SerializeField] private LightReceiver bigDoorReceiver2;
    [SerializeField] private Door waterLabDoor2;

    public void StartDream4() {
        int checkPoint = FindFirstObjectByType<GameManager>().GetCheckPoint();

        // Close and lock all doors past checkpoint
        foreach (CheckPointDoorStruct doorStruct in doorsToLockList) {
            if (doorStruct.checkPointIndex <= checkPoint) {
                doorStruct.door.Close();
                doorStruct.door.Lock();
                if (doorStruct.door.GetType() == typeof(TempleDoor)) {
                    TempleDoor t = (TempleDoor)doorStruct.door;
                    t.MakeUnlightable();
                }
            }
        }

        foreach (CheckPointDoorStruct doorStruct in doorsToOpenList) {
            if (doorStruct.checkPointIndex <= checkPoint) {
                doorStruct.door.InstaOpen();
            }
        }

        // Pull and lock all levers past checkpoint
        foreach (CheckPointLeverStruct leverStruct in leverList) {
            if (leverStruct.checkPointIndex <= checkPoint) {
                if (leverStruct.pullWithoutinteract) {
                    leverStruct.lever.PullWithoutSignal();
                }
                else {
                    leverStruct.lever.Interact();
                }
                leverStruct.lever.Lock();
            }
        }

        // Hide all map pieces that cannot be seen
        foreach(CheckPointMapStruct mapPieces in mapPiecesList) {
            bool isPointValid = false;
            for (int i = mapPieces.checkPointIndexStart; i <= mapPieces.checkPointIndexEnd; i++) {
                if (checkPoint == i) {
                    isPointValid = true;
                    mapPieces.mapPiece.SetActive(true);
                    break;
                }
            }
            if (!isPointValid) {
                mapPieces.mapPiece.SetActive(false);
            }
        }

        foreach(CheckPointAmbientStruct ambientStruct in ambientList) {
            for (int i = ambientStruct.checkPointIndexStart; i <= ambientStruct.checkPointIndexEnd; i++) {
                if (checkPoint == i) {
                    audioSource.clip = ambientStruct.audioClip;
                    audioSource.Play();
                    break;
                }
            }
        }

        if (FindFirstObjectByType<GameManager>().IsCrystal1Activated()) {
            crystal1Door.Close();
            crystal1Door.Lock();
        }
        if (FindFirstObjectByType<GameManager>().IsCrystal2Activated()) {
            crystal2Door.Close();
            crystal2Door.Lock();
        }
        if (FindFirstObjectByType<GameManager>().IsCrystal3Activated()) {
            crystal3Door.Close();
            crystal3Door.Lock();
        }

        bigGreenCrystal.SetCrystalStates(new bool[] {FindFirstObjectByType<GameManager>().IsCrystal1Activated(), FindFirstObjectByType<GameManager>().IsCrystal2Activated(), FindFirstObjectByType<GameManager>().IsCrystal3Activated()});

        switch (FindFirstObjectByType<GameManager>().GetBigDoorStage()) {
            case 0:
                break;
            case 1:
                waterLabDoor1.Close();
                waterLabDoor1.Lock();
                bigDoor.Active();
                break;
            case 2:
                waterLabDoor2.Close();
                waterLabDoor2.Lock();
                bigDoor.Active();
                break;
            case 3:
                waterLabDoor1.Close();
                waterLabDoor1.Lock();
                waterLabDoor2.Close();
                waterLabDoor2.Lock();
                bigDoor.Active();
                bigDoor.Active();
                break;
            case 4:
                waterLabDoor1.Close();
                waterLabDoor1.Lock();
                waterLabDoor2.Close();
                waterLabDoor2.Lock();
                bigDoor.Active();
                bigDoor.Active();
                bigDoor.DealDamage(0);
                break;
        }
    }
    }

[Serializable]
public struct CheckPointDoorStruct {
    public Door door;
    public int checkPointIndex;
}

[Serializable]
public struct CheckPointLeverStruct {
    public Lever lever;
    public int checkPointIndex;
    public bool pullWithoutinteract;
}

[Serializable]
public struct CheckPointMapStruct {
    public GameObject mapPiece;
    public int checkPointIndexStart;
    public int checkPointIndexEnd;
}

[Serializable]
public struct CheckPointAmbientStruct {
    public AudioClip audioClip;
    public int checkPointIndexStart;
    public int checkPointIndexEnd;
}