using UnityEngine;

public class MiniGame1Manager : MonoBehaviour
{
    [SerializeField] private testInteraction talker;
    private static MiniGame1Manager instance;

    public static MiniGame1Manager GetInstance() {
        return instance;
    }

    private void Awake() {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }


    public void StartMiniGame() {
        //a
        talker.Lock();
        gameObject.SetActive(true);
    }

    public void SelectOption(bool option) {
        if (option) {
            Debug.Log("++++");
            EndMiniGame();
        }
        else {
            Debug.Log("----");
            EndMiniGame();
        }
    }

    private void EndMiniGame() {
        //b
        talker.Unlock();
        gameObject.SetActive(false);
    }
}
