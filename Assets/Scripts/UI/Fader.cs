using UnityEngine;

public class Fader : MonoBehaviour
{
    private static Fader instance;
    [SerializeField] private bool fadeinAtStart;

    [SerializeField] private float timeToFade;
    private CanvasGroup canvasGroup;

    private bool fadingIn = false;
    private bool fadingOut = false;

    private void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
        instance = this;
    }

    private void Start() {
        if (fadeinAtStart) {
            canvasGroup.alpha = 1;
            fadingIn = true;
        }
        else {
            canvasGroup.alpha = 0;
        }
    }

    public static Fader GetInstance() {
        return instance;
    }

    private void Update() {
        if (fadingOut) {
            if (canvasGroup.alpha < 1) {
                canvasGroup.alpha += Time.deltaTime / timeToFade;
                if (canvasGroup.alpha >= 1) {
                    fadingOut = false;
                }
            }
        }
        if (fadingIn) {
            if (canvasGroup.alpha >= 0) {
                canvasGroup.alpha -= Time.deltaTime / timeToFade;
                if (canvasGroup.alpha <= 0) {
                    fadingIn = false;
                }
            }
        }
    }

    public void FadeIn(Color color, float duration) {
        if (duration > 0) {
            timeToFade = duration;
        }
        if (fadingOut) {
            fadingOut = false;
            canvasGroup.alpha = 1;
        }
        GetComponent<UnityEngine.UI.Image>().color = color;
        fadingIn = true;
    }

    public void FadeOut(Color color, float duration) {
        if (duration > 0) {
            timeToFade = duration;
        }
        if (fadingIn) {
            fadingIn = false;
            canvasGroup.alpha = 0;
        }
        GetComponent<UnityEngine.UI.Image>().color = color;
        fadingOut = true;
    }
}
