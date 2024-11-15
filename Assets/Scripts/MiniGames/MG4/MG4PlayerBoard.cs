using System.Collections.Generic;
using UnityEngine;

public class MG4PlayerBoard : MonoBehaviour
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

    private MiniGame4MarkableSlot lastShotPosition;
    private MiniGame4MarkableSlot currentShotPosition;
    private bool[] checkedDirections = {false, false, false, false};

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
                        markableSlots[slots[n].GetSlotPosition().XIndex + i][slots[n].GetSlotPosition().YIndex + j].Lock(true);
                    }
                }
            }
        }   
    }

    public void EndPlacingStage() {
        isInPlacingStage = false;
        
        foreach (var slots in markableSlots) {
            foreach (var markableSlot in slots) {
                markableSlot.Seal();
            }
        }
    }

    public void Reset() {
        isInPlacingStage = true;

        foreach (var slots in markableSlots) {
            foreach (var markableSlot in slots) {
                markableSlot.ResetMark();
            }
        }

        oneSlotShips = new List<OneSlotShip>();
        twoSlotShips = new List<TwoSlotShip>();
        threeSlotShips = new List<ThreeSlotShip>();
        fourSlotShip = null;
    }

    public void Attack() {
        if (lastShotPosition == null) {
            while (true) {
                SlotPosition slot = new SlotPosition(Random.Range(0, 10), Random.Range(0, 10));
                if (GetSlot(slot).IsMarked()) {
                    continue;
                }

                MiniGame4Manager.GetInstance().MakeTurn(false, slot);
                break;
            }
        }
        else {
            int dir = 0;
            while (true) {
                dir = Random.Range(0, 4);
                if (checkedDirections[dir] == false) {
                    break;
                }
            }

            SlotPosition slot;
            if (currentShotPosition != null) {
                slot = currentShotPosition.GetSlotPosition();
            }
            else {
                slot = lastShotPosition.GetSlotPosition();
            }

            switch (dir) {
                case 0:
                    MiniGame4Manager.GetInstance().MakeTurn(false, new SlotPosition(slot.XIndex + 1, slot.YIndex));
                    break;        
                case 1:
                    MiniGame4Manager.GetInstance().MakeTurn(false, new SlotPosition(slot.XIndex, slot.YIndex + 1));
                    break;
                case 2:
                    MiniGame4Manager.GetInstance().MakeTurn(false, new SlotPosition(slot.XIndex - 1, slot.YIndex));
                    break;
                case 3:
                    MiniGame4Manager.GetInstance().MakeTurn(false, new SlotPosition(slot.XIndex, slot.YIndex - 1));
                    break;
            }
        }
    }

    public void ManageShipDamage(SlotPosition slotPosition) {
        foreach(OneSlotShip ship in oneSlotShips) {
            if (ship.ContainsPoint(slotPosition)) {
                if (!GetSlot(ship.GetPosition()).IsMarked()) {
                        return;
                    }

                MarkArea(new SlotPosition[] { ship.GetPosition() });
                checkedDirections = new bool[] {false, false, false, false};
                lastShotPosition = null;
                currentShotPosition = null;
                return;
            }
        }

        if (fourSlotShip.ContainsPoint(slotPosition)) {

            if (lastShotPosition != null) {
                SetManualDirection(GetDirection(lastShotPosition.GetSlotPosition(), slotPosition));
                currentShotPosition = GetSlot(slotPosition);
            }
            else {
                lastShotPosition = GetSlot(slotPosition);
                CheckDirections(slotPosition);
            }
            
            foreach (SlotPosition slotpos in fourSlotShip.GetArray()) {
                if (!GetSlot(slotpos).IsMarked()) {
                    return;
                }
            }

            MarkArea(fourSlotShip.GetArray());
            checkedDirections = new bool[] {false, false, false, false};
            lastShotPosition = null;
            currentShotPosition = null;
            return;
        }

        foreach(ThreeSlotShip ship in threeSlotShips) {
            if (ship.ContainsPoint(slotPosition)) {
                if (lastShotPosition != null) {
                    SetManualDirection(GetDirection(lastShotPosition.GetSlotPosition(), slotPosition));
                    currentShotPosition = GetSlot(slotPosition);
                }
                else {
                    lastShotPosition = GetSlot(slotPosition);
                    CheckDirections(slotPosition);
                }
            
                foreach (SlotPosition slotpos in ship.GetArray()) {
                    if (!GetSlot(slotpos).IsMarked()) {
                        return;
                    }
                }

                MarkArea(ship.GetArray());
                checkedDirections = new bool[] {false, false, false, false};
                lastShotPosition = null;
                currentShotPosition = null;
                return;
            }
        }

        foreach(TwoSlotShip ship in twoSlotShips) {
            if (ship.ContainsPoint(slotPosition)) {
                if (lastShotPosition != null) {
                    SetManualDirection(GetDirection(lastShotPosition.GetSlotPosition(), slotPosition));
                    lastShotPosition = GetSlot(slotPosition);
                }
                else {
                    lastShotPosition = GetSlot(slotPosition);
                    CheckDirections(slotPosition);
                }

                foreach (SlotPosition slotpos in ship.GetArray()) {
                    if (!GetSlot(slotpos).IsMarked()) {
                        return;
                    }
                }

                MarkArea(ship.GetArray());
                checkedDirections = new bool[] {false, false, false, false};
                lastShotPosition = null;
                currentShotPosition = null;
                return;
            }
        }

        if (currentShotPosition != null) {
            SetManualDirection(GetDirection(currentShotPosition.GetSlotPosition(), lastShotPosition.GetSlotPosition()));
            currentShotPosition = null;
        }
    }

    private void CheckDirections(SlotPosition slotPosition) {
        if (slotPosition.XIndex + 1 >= 10 || GetSlot(new SlotPosition(slotPosition.XIndex + 1, slotPosition.YIndex)).IsMarked()) {
            checkedDirections[0] = true;
        }
        if (slotPosition.YIndex + 1 >= 10 || GetSlot(new SlotPosition(slotPosition.XIndex, slotPosition.YIndex + 1)).IsMarked()) {
            checkedDirections[1] = true;
        }
        if (slotPosition.XIndex - 1 < 0 || GetSlot(new SlotPosition(slotPosition.XIndex - 1, slotPosition.YIndex)).IsMarked()) {
            checkedDirections[2] = true;
        }
        if (slotPosition.YIndex - 1 < 0 || GetSlot(new SlotPosition(slotPosition.XIndex, slotPosition.YIndex - 1)).IsMarked()) {
            checkedDirections[3] = true;
        }
    }   

    private int GetDirection(SlotPosition a, SlotPosition b) {
        if (a.XIndex + 1 == b.XIndex) {
            return 0;
        }
        else if (a.YIndex + 1 == b.YIndex) {
            return 1;
        }
        else if (a.XIndex - 1 == b.XIndex) {
            return 2;
        }
        else if (a.YIndex - 1 == b.YIndex) {
            return 3;
        }
        else {
            return -1;
        }
    }

    private void SetManualDirection(int dir) {
        switch (dir) {
            case 0:
                checkedDirections = new bool[] {false, true, true, true};
                break;
            case 1:
                checkedDirections = new bool[] {true, false, true, true};
                break;
            case 2:
                checkedDirections = new bool[] {true, true, false, true};
                break;
            case 3:
                checkedDirections = new bool[] {true, true, true, false};
                break;
        }
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








public struct SlotPosition {

    public SlotPosition(int xIndex, int yIndex) {
        XIndex = xIndex;
        YIndex = yIndex;
    }

    public int XIndex {get; set;}
    public int YIndex {get; set;}

    public static bool operator ==(SlotPosition a, SlotPosition b) {
        return a.XIndex == b.XIndex && a.YIndex == b.YIndex;
    }

    public static bool operator !=(SlotPosition a, SlotPosition b) {
        return a.XIndex != b.XIndex || a.YIndex != b.YIndex;
    }

    public override bool Equals(object o) {
        if (o.GetType() == GetType()) {
            SlotPosition other = (SlotPosition)o;
            return XIndex == other.XIndex && YIndex == other.YIndex;
        }
        return false;
    }

    public override int GetHashCode() {
        return XIndex + YIndex;
    }
}

public class OneSlotShip
{
    public OneSlotShip(SlotPosition slotPosition) {
        this.slotPosition = slotPosition;
    }

    SlotPosition slotPosition;

    public bool ContainsPoint(SlotPosition slotPosition) {
        if (this.slotPosition == slotPosition) {
            return true;
        }
        else {
            return false;
        }
    }

    public SlotPosition GetPosition() {
        return slotPosition;
    }
}

public class TwoSlotShip
{
    public TwoSlotShip(SlotPosition slot1Position, SlotPosition slot2Position) {
        this.slot1Position = slot1Position;
        this.slot2Position = slot2Position;
    }

    SlotPosition slot1Position;
    SlotPosition slot2Position;

    public bool ContainsPoint(SlotPosition slotPosition) {
        if (slot1Position == slotPosition || slot2Position == slotPosition) {
            return true;
        }
        else {
            return false;
        }
    }

    public SlotPosition[] GetArray() {
        return new SlotPosition[2] { slot1Position, slot2Position };
    }
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

    public bool ContainsPoint(SlotPosition slotPosition) {
        if (slot1Position == slotPosition || slot2Position == slotPosition || slot3Position == slotPosition) {
            return true;
        }
        else {
            return false;
        }
    }

    public SlotPosition[] GetArray() {
        return new SlotPosition[3] { slot1Position, slot2Position, slot3Position };
    }
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

    public bool ContainsPoint(SlotPosition slotPosition) {
        if (slot1Position == slotPosition || slot2Position == slotPosition || slot3Position == slotPosition || slot4Position == slotPosition) {
            return true;
        }
        else {
            return false;
        }
    }

    public SlotPosition[] GetArray() {
        return new SlotPosition[4] { slot1Position, slot2Position, slot3Position, slot4Position };
    }
}