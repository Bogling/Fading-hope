using System;
using System.Linq;
using UnityEngine;

public class MiniGame4Manager : MonoBehaviour
{
    [SerializeField] private MG4AttackBoard attackBoard;
    [SerializeField] private MG4PlayerBoard playerBoard;
    private bool isPlayersTurn = true;
    private bool isSelecting = false;
    private SlotPosition currentStartPoint;
    private MiniGame4MarkableSlot[] selectedSlots;

    private static MiniGame4Manager instance;

    void Awake() {
        instance = this;
        selectedSlots = new MiniGame4MarkableSlot[0];
    }

    public static MiniGame4Manager GetInstance() { 
        return instance; 
    }
    public bool IsPlayersTurn() { return isPlayersTurn; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MakeTurn(bool isPlayer) {

    }

    public bool IsSelecting() {
        return isSelecting;
    }

    public bool CanSelect() {
        return playerBoard.IsInPlacingStage() && !isSelecting;
    }

    public void SelectForShipPlacement(SlotPosition slotPosition) {
        isSelecting = true;
        currentStartPoint = slotPosition;
        TraceLine(currentStartPoint);
    }

    public void TraceLine(SlotPosition destinationSlotPosition) {
        switch (GetDirection(destinationSlotPosition)) {
            case -1:
                foreach (MiniGame4MarkableSlot slot in selectedSlots) {
                    slot.UnSelect();
                }
                selectedSlots = new MiniGame4MarkableSlot[0];
                break;
            case 0:
                foreach (MiniGame4MarkableSlot slot in selectedSlots) {
                    slot.UnSelect();
                }
                selectedSlots = new MiniGame4MarkableSlot[0];

                for (int i = (int)MathF.Min(currentStartPoint.XIndex, destinationSlotPosition.XIndex); i <= (int)MathF.Max(currentStartPoint.XIndex, destinationSlotPosition.XIndex); i++) {
                    if (playerBoard.GetSlot(new SlotPosition(i, currentStartPoint.YIndex)).IsMarked() || playerBoard.GetSlot(new SlotPosition(i, currentStartPoint.YIndex)).IsLocked()) {
                        return;
                    }
                }

                for (int i = (int)MathF.Min(currentStartPoint.XIndex, destinationSlotPosition.XIndex); i <= (int)MathF.Max(currentStartPoint.XIndex, destinationSlotPosition.XIndex); i++) {
                    selectedSlots = selectedSlots.Append(playerBoard.GetSlot(new SlotPosition(i, currentStartPoint.YIndex))).ToArray();
                    playerBoard.GetSlot(new SlotPosition(i, currentStartPoint.YIndex)).Select();
                }
                break;
            case 1:
                foreach (MiniGame4MarkableSlot slot in selectedSlots) {
                    slot.UnSelect();
                }
                selectedSlots = new MiniGame4MarkableSlot[0];

                for (int i = (int)MathF.Min(currentStartPoint.YIndex, destinationSlotPosition.YIndex); i <= (int)MathF.Max(currentStartPoint.YIndex, destinationSlotPosition.YIndex); i++) {
                    if (playerBoard.GetSlot(new SlotPosition(currentStartPoint.XIndex, i)).IsMarked() || playerBoard.GetSlot(new SlotPosition(currentStartPoint.XIndex, i)).IsLocked()) {
                        return;
                    }
                }

                for (int i = (int)MathF.Min(currentStartPoint.YIndex, destinationSlotPosition.YIndex); i <= (int)MathF.Max(currentStartPoint.YIndex, destinationSlotPosition.YIndex); i++) {
                    selectedSlots = selectedSlots.Append(playerBoard.GetSlot(new SlotPosition(currentStartPoint.XIndex, i))).ToArray();
                    playerBoard.GetSlot(new SlotPosition(currentStartPoint.XIndex, i)).Select();
                }
                break;
        }
    }

    public void SubmitLine() {
        isSelecting = false;
        if (selectedSlots.Length > 0) {
            if (playerBoard.AddShip(selectedSlots)) {
                selectedSlots = new MiniGame4MarkableSlot[0];
            }
            else {
                foreach (MiniGame4MarkableSlot slot in selectedSlots) {
                    slot.UnSelect();
                }

                selectedSlots = new MiniGame4MarkableSlot[0];
            }
        }
    }

    private int GetDirection(SlotPosition destinationSlotPosition) {
        if (currentStartPoint.XIndex == destinationSlotPosition.XIndex) {
            return 1;
        }
        else if (currentStartPoint.YIndex == destinationSlotPosition.YIndex) {
            return 0;
        }
        else {
            return -1;
        }
    }
}
