using UnityEngine;

public class MG4AttackBoard : MonoBehaviour
{
    [SerializeField] private bool isLocked = false;
    [SerializeField] private MiniGame4MarkableSlot slotPrefab;
    [SerializeField] private GameObject pivotPoint;
    [SerializeField] private float slotDistance;

    MiniGame4MarkableSlot[][] markableSlots;

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
    }
}
