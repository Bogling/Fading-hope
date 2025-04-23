using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MG4AttackBoard : MonoBehaviour
{
    [SerializeField] private bool isLocked = false;
    [SerializeField] private MiniGame4MarkableSlot slotPrefab;
    [SerializeField] private GameObject pivotPoint;
    [SerializeField] private float slotDistance;

    MiniGame4MarkableSlot[][] markableSlots;

    private bool isInPlacingStage = true;

    private List<OneSlotShip> oneSlotShips;
    private List<TwoSlotShip> twoSlotShips;
    private List<ThreeSlotShip> threeSlotShips;
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

        oneSlotShips = new List<OneSlotShip>();
        twoSlotShips = new List<TwoSlotShip>();
        threeSlotShips = new List<ThreeSlotShip>();
    }

    public bool AreAnyShipLeft() {
        return oneSlotShips.Count > 0 || twoSlotShips.Count > 0 || threeSlotShips.Count > 0 || fourSlotShip != null;
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
                if (oneSlotShips.Count < 4) {
                    oneSlotShips.Add(new OneSlotShip(slots[0].GetSlotPosition()));
                    LockArea(slots);
                }
                else {
                    return false;
                }
                break;
            case 2:
                if (twoSlotShips.Count < 3) {
                    twoSlotShips.Add(new TwoSlotShip(slots[0].GetSlotPosition(), slots[1].GetSlotPosition()));
                    LockArea(slots);
                }
                else {
                    return false;
                }
                break;
            case 3: 
                if (threeSlotShips.Count < 2) {
                    threeSlotShips.Add(new ThreeSlotShip(slots[0].GetSlotPosition(), slots[1].GetSlotPosition(), slots[2].GetSlotPosition()));
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
        return oneSlotShips.Count == 4 && twoSlotShips.Count == 3 && threeSlotShips.Count == 2 && fourSlotShip != null;
    }

    public void LockArea(MiniGame4MarkableSlot[] slots) {

        foreach (var slot in slots) {
            slot.Mark(false, true);
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
                        markableSlots[slots[n].GetSlotPosition().XIndex + i][slots[n].GetSlotPosition().YIndex + j].Lock(false);
                    }
                }
            }
        }   
    }

    public void EndPlacingStage() {
        isInPlacingStage = false;
    }

    public void Reset() {
        isInPlacingStage = true;

        foreach (var slots in markableSlots) {
            foreach (var markableSlot in slots) {
                markableSlot.ResetMark();
                markableSlot.UnLock();
            }
        }

        oneSlotShips = new List<OneSlotShip>();
        twoSlotShips = new List<TwoSlotShip>();
        threeSlotShips = new List<ThreeSlotShip>();
        fourSlotShip = null;
    }

    public void GenerateShips() {
        while (fourSlotShip == null) {
            int xIndex = Random.Range(0, 10);
            int yIndex = Random.Range(0, 10);

            if (GetSlot(new SlotPosition(xIndex, yIndex)).IsLocked()) {
                continue;
            }

            switch (SelectDirection(new SlotPosition(xIndex, yIndex), 4)) {
                case -1:
                    continue;
                case 0:
                    fourSlotShip = new FourSlotShip(new SlotPosition(xIndex, yIndex), new SlotPosition(xIndex + 1, yIndex), new SlotPosition(xIndex + 2, yIndex), new SlotPosition(xIndex + 3, yIndex));
                    SelectShips(new SlotPosition[] { new SlotPosition(xIndex, yIndex), new SlotPosition(xIndex + 1, yIndex), new SlotPosition(xIndex + 2, yIndex), new SlotPosition(xIndex + 3, yIndex) });
                    break;
                case 1:
                    fourSlotShip = new FourSlotShip(new SlotPosition(xIndex, yIndex), new SlotPosition(xIndex, yIndex + 1), new SlotPosition(xIndex, yIndex + 2), new SlotPosition(xIndex, yIndex + 3));
                    SelectShips(new SlotPosition[] { new SlotPosition(xIndex, yIndex), new SlotPosition(xIndex, yIndex + 1), new SlotPosition(xIndex, yIndex + 2), new SlotPosition(xIndex, yIndex + 3) });
                    break;
                case 2:
                    fourSlotShip = new FourSlotShip(new SlotPosition(xIndex, yIndex), new SlotPosition(xIndex - 1, yIndex), new SlotPosition(xIndex - 2, yIndex), new SlotPosition(xIndex - 3, yIndex));
                    SelectShips(new SlotPosition[] { new SlotPosition(xIndex, yIndex), new SlotPosition(xIndex - 1, yIndex), new SlotPosition(xIndex - 2, yIndex), new SlotPosition(xIndex - 3, yIndex) });
                    break;
                case 3:
                    fourSlotShip = new FourSlotShip(new SlotPosition(xIndex, yIndex), new SlotPosition(xIndex, yIndex - 1), new SlotPosition(xIndex, yIndex - 2), new SlotPosition(xIndex, yIndex - 3));
                    SelectShips(new SlotPosition[] { new SlotPosition(xIndex, yIndex), new SlotPosition(xIndex, yIndex - 1), new SlotPosition(xIndex, yIndex - 2), new SlotPosition(xIndex, yIndex - 3) });
                    break;
            }
        }
    
        while (threeSlotShips.Count < 2) {
            int xIndex = Random.Range(0, 10);
            int yIndex = Random.Range(0, 10);

            if (GetSlot(new SlotPosition(xIndex, yIndex)).IsLocked()) {
                continue;
            }

            switch (SelectDirection(new SlotPosition(xIndex, yIndex), 3)) {
                case -1:
                    continue;
                case 0:
                    threeSlotShips.Add(new ThreeSlotShip(new SlotPosition(xIndex, yIndex), new SlotPosition(xIndex + 1, yIndex), new SlotPosition(xIndex + 2, yIndex)));
                    SelectShips(new SlotPosition[] { new SlotPosition(xIndex, yIndex), new SlotPosition(xIndex + 1, yIndex), new SlotPosition(xIndex + 2, yIndex) });
                    break;
                case 1:
                    threeSlotShips.Add(new ThreeSlotShip(new SlotPosition(xIndex, yIndex), new SlotPosition(xIndex, yIndex + 1), new SlotPosition(xIndex, yIndex + 2)));
                    SelectShips(new SlotPosition[] { new SlotPosition(xIndex, yIndex), new SlotPosition(xIndex, yIndex + 1), new SlotPosition(xIndex, yIndex + 2) });
                    break;
                case 2:
                    threeSlotShips.Add(new ThreeSlotShip(new SlotPosition(xIndex, yIndex), new SlotPosition(xIndex - 1, yIndex), new SlotPosition(xIndex - 2, yIndex)));
                    SelectShips(new SlotPosition[] { new SlotPosition(xIndex, yIndex), new SlotPosition(xIndex - 1, yIndex), new SlotPosition(xIndex - 2, yIndex) });
                    break;
                case 3:
                    threeSlotShips.Add(new ThreeSlotShip(new SlotPosition(xIndex, yIndex), new SlotPosition(xIndex, yIndex - 1), new SlotPosition(xIndex, yIndex - 2)));
                    SelectShips(new SlotPosition[] { new SlotPosition(xIndex, yIndex), new SlotPosition(xIndex, yIndex - 1), new SlotPosition(xIndex, yIndex - 2) });
                    break;
            }
        }

        while (twoSlotShips.Count < 3) {
            int xIndex = Random.Range(0, 10);
            int yIndex = Random.Range(0, 10);

            if (GetSlot(new SlotPosition(xIndex, yIndex)).IsLocked()) {
                continue;
            }

            switch (SelectDirection(new SlotPosition(xIndex, yIndex), 2)) {
                case -1:
                    continue;
                case 0:
                    twoSlotShips.Add(new TwoSlotShip(new SlotPosition(xIndex, yIndex), new SlotPosition(xIndex + 1, yIndex)));
                    SelectShips(new SlotPosition[] { new SlotPosition(xIndex, yIndex), new SlotPosition(xIndex + 1, yIndex) });
                    break;
                case 1:
                    twoSlotShips.Add(new TwoSlotShip(new SlotPosition(xIndex, yIndex), new SlotPosition(xIndex, yIndex + 1)));
                    SelectShips(new SlotPosition[] { new SlotPosition(xIndex, yIndex), new SlotPosition(xIndex, yIndex + 1) });
                    break;
                case 2:
                    twoSlotShips.Add(new TwoSlotShip(new SlotPosition(xIndex, yIndex), new SlotPosition(xIndex - 1, yIndex)));
                    SelectShips(new SlotPosition[] { new SlotPosition(xIndex, yIndex), new SlotPosition(xIndex - 1, yIndex) });
                    break;
                case 3:
                    twoSlotShips.Add(new TwoSlotShip(new SlotPosition(xIndex, yIndex), new SlotPosition(xIndex, yIndex - 1)));
                    SelectShips(new SlotPosition[] { new SlotPosition(xIndex, yIndex), new SlotPosition(xIndex, yIndex - 1) });
                    break;
            }
        }

        while (oneSlotShips.Count < 4) {
            int xIndex = Random.Range(0, 10);
            int yIndex = Random.Range(0, 10);

            if (GetSlot(new SlotPosition(xIndex, yIndex)).IsLocked()) {
                continue;
            }

            oneSlotShips.Add(new OneSlotShip(new SlotPosition(xIndex, yIndex)));
            SelectShips(new SlotPosition[] { new SlotPosition(xIndex, yIndex) });
        }

        EndPlacingStage();
    }

    private int SelectDirection(SlotPosition slotPosition, int count) {
        int direction = Random.Range(0, 4);
        int rotateCount = 0;
        while (rotateCount < 4) {
            switch (direction) {
                case 0:
                    if (slotPosition.XIndex + count - 1 >= 10) {
                        break;
                    }
                    for (int i = 0; i < count; i++) {
                        if (GetSlot(new SlotPosition(slotPosition.XIndex + i, slotPosition.YIndex)).IsLocked()) {
                            break;
                        }

                        if (i == count - 1) {
                            return 0;
                        }
                    }
                    break;
                case 1:
                    if (slotPosition.YIndex + count - 1 >= 10) {
                        break;
                    }
                    for (int i = 0; i < count; i++) {
                        if (GetSlot(new SlotPosition(slotPosition.XIndex, slotPosition.YIndex + i)).IsLocked()) {
                            break;
                        }

                        if (i == count - 1) {
                            return 1;
                        }
                    }
                    break;
                case 2:
                    if (slotPosition.XIndex - count + 1 < 0) {
                        break;
                    }
                    for (int i = 0; i < count; i++) {
                        if (GetSlot(new SlotPosition(slotPosition.XIndex - i, slotPosition.YIndex)).IsLocked()) {
                            break;
                        }
                        
                        if (i == count - 1) {
                            return 2;
                        }
                    }
                    break;
                case 3:
                    if (slotPosition.YIndex - count + 1 < 0) {
                        break;
                    }
                    for (int i = 0; i < count; i++) {
                        if (GetSlot(new SlotPosition(slotPosition.XIndex, slotPosition.YIndex - i)).IsLocked()) {
                            break;
                        }

                        if (i == count - 1) {
                            return 3;
                        }
                    }
                    break;
            }

            direction = (direction + 1) % 4;
            rotateCount++;
        }

        return -1;
    }

    private void SelectShips(SlotPosition[] slotPositions) {
        MiniGame4MarkableSlot[] slots = new MiniGame4MarkableSlot[slotPositions.Length];
        for (int i = 0; i < slotPositions.Length; i++) {
            slots[i] = GetSlot(slotPositions[i]);
        }

        LockArea(slots);
    }

    public bool ManageShipDamage(SlotPosition slotPosition) {
        foreach(OneSlotShip ship in oneSlotShips) {
            if (ship.ContainsPoint(slotPosition)) {
                if (!GetSlot(ship.GetPosition()).IsMarked()) {
                        return true;
                    }

                MarkArea(new SlotPosition[] { ship.GetPosition() });
                oneSlotShips.Remove(ship);
                return true;
            }
        }

        if (fourSlotShip != null && fourSlotShip.ContainsPoint(slotPosition)) {
            foreach (SlotPosition slotpos in fourSlotShip.GetArray()) {
                if (!GetSlot(slotpos).IsMarked()) {
                    return true;
                }
            }

            MarkArea(fourSlotShip.GetArray());
            fourSlotShip = null;
            return true;
        }

        foreach(ThreeSlotShip ship in threeSlotShips) {
            if (ship.ContainsPoint(slotPosition)) {
                foreach (SlotPosition slotpos in ship.GetArray()) {
                    if (!GetSlot(slotpos).IsMarked()) {
                        return true;
                    }
                }

                MarkArea(ship.GetArray());
                threeSlotShips.Remove(ship);
                return true;
            }
        }

        foreach(TwoSlotShip ship in twoSlotShips) {
            if (ship.ContainsPoint(slotPosition)) {
                foreach (SlotPosition slotpos in ship.GetArray()) {
                    if (!GetSlot(slotpos).IsMarked()) {
                        return true;
                    }
                }

                MarkArea(ship.GetArray());
                twoSlotShips.Remove(ship);
                return true;
            }
        }


        return false;
    }
    
    private void MarkArea(SlotPosition[] slotPositions) {
        List<MiniGame4MarkableSlot> slots = new List<MiniGame4MarkableSlot>();

        foreach (SlotPosition slotpos in slotPositions) {
            GetSlot(slotpos).MarkDead();
            slots.Add(GetSlot(slotpos));
        }

        for (int n = 0; n < slots.Count; n++) {
            for (int i = -1; i < 2; i++) {
                for (int j = -1; j < 2; j++) {
                    if (!((i == 0) && (j == 0)) 
                    && slots[n].GetSlotPosition().XIndex + i >= 0 
                    && slots[n].GetSlotPosition().XIndex + i < 10 
                    && slots[n].GetSlotPosition().YIndex + j >= 0 
                    && slots[n].GetSlotPosition().YIndex + j < 10
                    && !GetSlot(new SlotPosition(slots[n].GetSlotPosition().XIndex + i, slots[n].GetSlotPosition().YIndex + j)).IsMarked()
                    && !GetSlot(new SlotPosition(slots[n].GetSlotPosition().XIndex + i, slots[n].GetSlotPosition().YIndex + j)).ContainsPart()) {
                        markableSlots[slots[n].GetSlotPosition().XIndex + i][slots[n].GetSlotPosition().YIndex + j].Mark(true, false);
                    }
                }
            }
        } 
    }
}
