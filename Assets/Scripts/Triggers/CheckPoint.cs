using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private Transform point;
    [SerializeField] private bool withLamp;
    [SerializeField] private bool withFlashlight;
    [SerializeField] private bool hasHP;
    [SerializeField] private bool hasLP;
    [SerializeField] private RenderPipelineAsset renderPipelineAsset;
    [SerializeField] private bool isAutoCheckpoint = false;
    public int id;
    private bool isTriggered = false;
    private GameManager gameManager;

    private void Awake() {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    public Transform getPoint() {
        return point;
    }

    private void Start() {
        if (!isTriggered && isAutoCheckpoint) {
            OnTriggerEnter(null);
        }
    }
    private void OnTriggerEnter(Collider other) {
        if (!isTriggered) {
            if (renderPipelineAsset != null && renderPipelineAsset != GraphicsSettings.defaultRenderPipeline) {
                GraphicsSettings.defaultRenderPipeline = renderPipelineAsset;
            }
            if (gameManager.GetCheckPoint() <= id || gameManager.GetScene() != SceneManager.GetActiveScene().name) {
                gameManager.SetCheckPoint(id, SceneManager.GetActiveScene().name, withLamp, withFlashlight, hasHP, hasLP);
                SaveLoadManager.Save();
            }
            isTriggered = true;
        }
    }
}
