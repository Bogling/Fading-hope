using UnityEngine;

public class FearAnimatorManager : MonoBehaviour
{
    [SerializeField] private FearAI fearAI;
    public void Disappear() {
        fearAI.Disappear();
    }

    public void Activate() {
        fearAI.Activate();
    }

    public void KillPlayer() {
        fearAI.KillPlayer();
    }

    public void AttachCamera() {
        fearAI.AttachCamera();
    }
}
