using System.Data.Common;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MG4PlayerBoard : MonoBehaviour
{
    [SerializeField] private bool isLocked = false;
    [SerializeField] private MiniGame4MarkableSlot slotPrefab;
    [SerializeField] private GameObject pivotPoint;
    [SerializeField] private float slotDistance;
    MiniGame4MarkableSlot[][] markableSlots;

    private bool isInPlacingStage = true;

    private OneSlotShip[] oneSlotShips;
    private TwoSlotShip[] twoSlotShips;
    private ThreeSlotShip[] threeSlotShips;
    private FourSlotShip fourSlotShip;

    void Awake() {
        markableSlots = new MiniGame4MarkableSlot[10][];

        for (int i = 0; i < 10; i++) {
            markableSlots[i] = new MiniGame4MarkableSlot[10];
            for (int j = 0; j < 10; j++) {
                markableSlots[i][j] = Instantiate(slotPrefab);;
                markableSlots[i][j].SetIndex(i, j);
                markableSlots[i][j].transform.SetParent(transform, false);
                markableSlots[i][j].transform.localPosition = new Vector3(j * slotDistance + pivotPoint.transform.localPosition.x, i * slotDistance + pivotPoint.transform.localPosition.y, 0);
            }
        }

        oneSlotShips = new OneSlotShip[0];
        twoSlotShips = new TwoSlotShip[0];
        threeSlotShips = new ThreeSlotShip[0];
    }

    public bool IsInPlacingStage() {
        return isInPlacingStage;
    }

    public MiniGame4MarkableSlot GetSlot(SlotPosition slotPosition) {
        return markableSlots[slotPosition.XIndex][slotPosition.YIndex];
    }

    public bool AddShip(MiniGame4MarkableSlot[] slots) {
        if (slots.Length > 4) {
            return false;
        }

        switch (slots.Length) {
            case 1:
                if (oneSlotShips.Length < 4) {
                    oneSlotShips = oneSlotShips.Append(new OneSlotShip(slots[0].GetSlotPosition())).ToArray();
                    LockArea(slots);
                }
                else {
                    return false;
                }
                break;
            case 2:
                if (twoSlotShips.Length < 3) {
                    twoSlotShips = twoSlotShips.Append(new TwoSlotShip(slots[0].GetSlotPosition(), slots[1].GetSlotPosition())).ToArray();
                    LockArea(slots);
                }
                else {
                    return false;
                }
                break;
            case 3: 
                if (threeSlotShips.Length < 2) {
                    threeSlotShips = threeSlotShips.Append(new ThreeSlotShip(slots[0].GetSlotPosition(), slots[1].GetSlotPosition(), slots[2].GetSlotPosition())).ToArray();
                    LockArea(slots);
                }
                else {
                    return false;
                }
                break;
            case 4:
                if (fourSlotShip == null) {
                    fourSlotShip = new FourSlotShip(slots[0].GetSlotPosition(), slots[1].GetSlotPosition(), slots[2].GetSlotPosition(), slots[3].GetSlotPosition());
                    LockArea(slots);
                }
                else {
                    return false;
                }
                break;
        }
        return true;
    }

    public bool CheckIfAllShipsPlaced() {
        return oneSlotShips.Length == 4 && twoSlotShips.Length == 3 && threeSlotShips.Length == 2 && fourSlotShip != null;
    }

    public void LockArea(MiniGame4MarkableSlot[] slots) {

        foreach (var slot in slots) {
            slot.Mark(true, true);
        }


        for (int n = 0; n < slots.Length; n++) {
            for (int i = -1; i < 2; i++) {
                for (int j = -1; j < 2; j++) {
                    if (!((i == 0) && (j == 0)) 
                    && slots[n].GetSlotPosition().XIndex + i >= 0 
                    && slots[n].GetSlotPosition().XIndex + i < 10 
                    && slots[n].GetSlotPosition().YIndex + j >= 0 
                    && slots[n].GetSlotPosition().YIndex + j < 10
                    && !GetSlot(new SlotPosition(slots[n].GetSlotPosition().XIndex + i, slots[n].GetSlotPosition().YIndex + j)).IsMarked()
                    && !GetSlot(new SlotPosition(slots[n].GetSlotPosition().XIndex + i, slots[n].GetSlotPosition().YIndex + j)).ContainsPart()) {
                        markableSlots[slots[n].GetSlotPosition().XIndex + i][slots[n].GetSlotPosition().YIndex + j].Lock();
                    }
                }
            }
        }   
    }
}








public struct SlotPosition {

    public SlotPosition(int xIndex, int yIndex) {
        XIndex = xIndex;
        YIndex = yIndex;
    }

    public int XIndex {get; set;}
    public int YIndex {get; set;}
}

public class OneSlotShip
{
    public OneSlotShip(SlotPosition slotPosition) {
        this.slotPosition = slotPosition;
    }

    SlotPosition slotPosition;
}

public class TwoSlotShip
{
    public TwoSlotShip(SlotPosition slot1Position, SlotPosition slot2Position) {
        this.slot1Position = slot1Position;
        this.slot2Position = slot2Position;
    }

    SlotPosition slot1Position;
    SlotPosition slot2Position;
}

public class ThreeSlotShip 
{

    public ThreeSlotShip(SlotPosition slot1Position, SlotPosition slot2Position, SlotPosition slot3Position) {
        this.slot1Position = slot1Position;
        this.slot2Position = slot2Position;
        this.slot3Position = slot3Position;
    }

    SlotPosition slot1Position;
    SlotPosition slot2Position;
    SlotPosition slot3Position;
}

public class FourSlotShip
{
    public FourSlotShip(SlotPosition slot1Position, SlotPosition slot2Position, SlotPosition slot3Position, SlotPosition slot4Position) {
        this.slot1Position = slot1Position;
        this.slot2Position = slot2Position;
        this.slot3Position = slot3Position;
        this.slot4Position = slot4Position;
    }

    SlotPosition slot1Position;
    SlotPosition slot2Position;
    SlotPosition slot3Position;
    SlotPosition slot4Position;
}