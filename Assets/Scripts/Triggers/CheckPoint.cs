using UnityEngine;
using UnityEngine.Rendering;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private Transform point;
    [SerializeField] private bool withLamp;
    [SerializeField] private bool withFlashlight;
    [SerializeField] private bool hasHP;
    [SerializeField] private bool hasLP;
    [SerializeField] private RenderPipelineAsset renderPipelineAsset;
    public int id;
    private bool isTriggered = false;
    private GameManager gameManager;

    private void Awake() {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    public Transform getPoint() {
        return point;
    }

    private void OnTriggerEnter(Collider other) {
        if (!isTriggered) {
            if (renderPipelineAsset != null && renderPipelineAsset != GraphicsSettings.defaultRenderPipeline) {
                GraphicsSettings.defaultRenderPipeline = renderPipelineAsset;
            }
            if (gameManager.GetCheckPoint() <= id) {
                gameManager.SetCheckPoint(id, withLamp, withFlashlight, hasHP, hasLP);
                SaveLoadManager.Save();
            }
            isTriggered = true;
        }
    }
}
