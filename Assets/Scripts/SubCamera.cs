using UnityEngine;

public class SubCamera : MonoBehaviour
{

    private bool revertFogState = false;
    [SerializeField] private bool renderFog;
    private void OnPreRender() {
        revertFogState = RenderSettings.fog;
        RenderSettings.fog = renderFog;   
    }

    private void OnPostRender() {
        RenderSettings.fog = revertFogState;
    }
}
