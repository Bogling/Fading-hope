using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private PlayerData playerData;
    [SerializeField] private GameObject lamp;
    [SerializeField] private GameObject flashlight;
    [SerializeField] private float maxHP;
    [SerializeField] private float maxLP;
    [SerializeField] private float hp;
    [SerializeField] private float lp;

    [SerializeField] private float hpRegenWaitTime;
    [SerializeField] private float hpRegenDelay;
    [SerializeField] private float lpRegenWaitTime;
    [SerializeField] private float lpRegenDelay;
    

    private bool canRegenHP;
    private int isHPRegenBlocked;
    private bool canRegenLP;
    private HPBar hpBar;
    private LPBar lpBar;

    private void Awake() {
        playerData = new PlayerData();
        playerData = SaveLoadManager.GetDirectData();
        if (lamp != null) {
            lamp.SetActive(false);
        }
        hp = maxHP;
        lp = maxLP;
        hpBar = FindFirstObjectByType<HPBar>();
        lpBar = FindFirstObjectByType<LPBar>();
    }

    public void SetCheckPoint(int newCheckPoint, bool withLamp, bool withFlashlight, bool hasHP, bool hasLP) {
        playerData.SetCheckPoint(newCheckPoint);
        playerData.hasLamp = withLamp;
        playerData.hasFlashlight = withFlashlight;
        playerData.hasHP = hasHP;
        playerData.hasLP = hasLP;
        SaveLoadManager.Save();
    }

    public void SetCheckPoint(int newCheckPoint, string level, bool withLamp, bool withFlashlight, bool hasHP, bool hasLP) {
        playerData.SetCheckPoint(newCheckPoint);
        playerData.currentScene = level;
        playerData.hasLamp = withLamp;
        playerData.hasFlashlight = withFlashlight;
        playerData.hasHP = hasHP;
        playerData.hasLP = hasLP;
        SaveLoadManager.Save();
        if (hpBar != null) {
            if (!hasHP) {
                hpBar.gameObject.SetActive(false);
            }
            else {
                hpBar.gameObject.SetActive(true);
            }
        }

        if (lpBar != null) {
            if (!hasLP) {
                lpBar.gameObject.SetActive(false);
            }
            else {
                lpBar.gameObject.SetActive(true);
            }
        }
    }

    public IEnumerator Respawn(Color faderColor, float fadeDuration, bool fadeIn, bool fadeOut) {
        if (FindFirstObjectByType<PlayerInputController>() != null) {
            FindFirstObjectByType<PlayerInputController>().FullEnable();
        }
        else if (FindFirstObjectByType<DreamPlayerInputController>() != null) {
            FindFirstObjectByType<DreamPlayerInputController>().FullEnable();
        }
        if (fadeOut) {
            Fader.GetInstance().FadeOut(faderColor, fadeDuration);
            yield return new WaitForSeconds(fadeDuration);
        }
        var points = FindObjectsByType<CheckPoint>(FindObjectsSortMode.InstanceID);
        foreach (var point in points) {
            if (point.id == playerData.GetCheckPoint()) {
                gameObject.transform.position = point.getPoint().position;
                if (gameObject.GetComponent<Rigidbody>() != null) {
                    gameObject.GetComponent<Rigidbody>().position = point.getPoint().position;
                }
                if (playerData.hasLamp && lamp != null) {
                    lamp.SetActive(true);
                }
                else if (lamp != null) {
                    lamp.SetActive(false);
                }
                if (playerData.hasFlashlight && flashlight != null) {
                    flashlight.SetActive(true);
                }
                else if (flashlight != null) {
                    flashlight.SetActive(false);
                }
                if (playerData.hasHP) {
                    hp = maxHP;
                    hpBar.UpdateHP();
                }
                if (playerData.hasLP) {
                    lp = maxLP;
                    lpBar.UpdateLP();
                }
                break;
                
            }
        }
        if (fadeIn) {
            Fader.GetInstance().FadeIn(faderColor, fadeDuration);
            yield return new WaitForSeconds(fadeDuration);
        }
    }

    public IEnumerator Respawn(Color faderColor, float fadeInDuration, bool fadeIn, float fadeOutDuration, bool fadeOut) {
        if (fadeOut) {
            Fader.GetInstance().FadeOut(faderColor, fadeOutDuration);
            yield return new WaitForSeconds(fadeOutDuration);
        }
        var points = FindObjectsByType<CheckPoint>(FindObjectsSortMode.InstanceID);
        foreach (var point in points) {
            if (point.id == playerData.GetCheckPoint()) {
                gameObject.transform.position = point.getPoint().position;
                if (gameObject.GetComponent<Rigidbody>() != null) {
                    gameObject.GetComponent<Rigidbody>().position = point.getPoint().position;
                }
                if (playerData.hasLamp && lamp != null) {
                    lamp.SetActive(true);
                }
                else if (lamp != null) {
                    lamp.SetActive(false);
                }
                if (playerData.hasFlashlight && flashlight != null) {
                    flashlight.SetActive(true);
                }
                else if (flashlight != null) {
                    flashlight.SetActive(false);
                }
                if (playerData.hasHP) {
                    hp = maxHP;
                    hpBar.UpdateHP();
                }
                if (playerData.hasLP) {
                    lp = maxLP;
                    lpBar.UpdateLP();
                }
                break;
                
            }
        }
        if (fadeIn) {
            Fader.GetInstance().FadeIn(faderColor, fadeInDuration);
            yield return new WaitForSeconds(fadeInDuration);
        }
    }

    public IEnumerator CallGameOver(Color faderColor, float fadeDuration, bool fadeIn, bool fadeOut) {
        if (FindFirstObjectByType<Day5Manager>() != null) {
            FindFirstObjectByType<Day5Manager>().GameOver();
            yield break;
        }
        if (fadeOut) {
            Fader.GetInstance().FadeOut(faderColor, fadeDuration);
            yield return new WaitForSeconds(fadeDuration);
        }
        if (FindFirstObjectByType<PlayerInputController>() != null) {
            FindFirstObjectByType<PlayerInputController>().FullDisable();
        }
        else if (FindFirstObjectByType<DreamPlayerInputController>() != null) {
            FindFirstObjectByType<DreamPlayerInputController>().FullDisable();
        }
        SaveLoadManager.Load();
        if (fadeIn) {
            Fader.GetInstance().FadeIn(faderColor, fadeDuration);
            yield return new WaitForSeconds(fadeDuration);
        }
    }

    public string GetScene() {
        return playerData.GetScene();
    }

    public int GetCheckPoint() {
        return playerData.GetCheckPoint();
    }

    public void LoadData(PlayerData loadData) {
        playerData = loadData;
        StartCoroutine(Respawn(Color.black, 5f, false, false));
        if (FindFirstObjectByType<Dream4Manager>()) {
            FindFirstObjectByType<Dream4Manager>().StartDream4();
        }
    }

    public PlayerData GetData() {
        return playerData;
    }

    public void GiveLamp() {
        playerData.hasLamp = true;
        lamp.SetActive(true);
    }

    public void TakeLamp() {
        playerData.hasLamp = false;
    }

    public void GiveFlashlight() {
        flashlight.SetActive(true);
        if (lpBar != null) {
            lpBar.gameObject.SetActive(true);
        }
        else {
            lpBar.gameObject.SetActive(false);
        }
        playerData.hasFlashlight = true;
    }

    public void TakeFlashlight() {
        playerData.hasFlashlight = false;
    }

    public void SetHP(float newHP) {
        hp = newHP;
    }

    public void SetMaxHP(float newMaxHP) {
        maxHP = newMaxHP;
    }

    public float GetMaxHP() {
        return maxHP;
    }

    public void SetLP(float newLP) {
        lp = newLP;
    }

    public float GetHP() {
        return hp;
    }

    public float GetLP() {
        return lp;
    }

    public float GetMaxLP() {
        return maxLP;
    }

    public bool isLPFull() {
        return lp == maxLP;
    }

    public int DealDamage(float damage) {
        StopHPRegen();
        StopCoroutine("StartHPDelay");
        if (hp - damage > 0) {
            Debug.Log(hp);
            hp -= damage;
            Debug.Log(hp);
            hpBar.UpdateHP();
            if (isHPRegenBlocked == 0) {
                StartCoroutine("StartHPDelay");
            }
            return -1;
        }
        else if (hp != 1) {
            hp = 1;
            hpBar.UpdateHP();
            if (isHPRegenBlocked == 0) {
                StartCoroutine("StartHPDelay");
            }
            return 0;
        }
        else {
            hp = 0;
            hpBar.UpdateHP();
            StartCoroutine(CallGameOver(Color.black, 1, true, true));
            return 1;
        }
    }

    public void BlockHPRegen() {
        isHPRegenBlocked++;
        StopCoroutine("StartHPDelay");
    }

    public void UnblockHPRegen() {
        isHPRegenBlocked--;
        if (isHPRegenBlocked == 0 && hp < maxHP) {
            StartCoroutine("StartHPDelay");
        }

    }

    public IEnumerator StartHPDelay() {
        yield return new WaitForSeconds(hpRegenWaitTime);
        if (isHPRegenBlocked > 0) {yield break;}
        StartHPRegen();
    }

    public void RefreshLP() {
        lp = 0;
        lpBar.UpdateLP();
        StartCoroutine(RegenerateLP());
    }

    public IEnumerator RegenerateHP() {
        if (!canRegenHP || isHPRegenBlocked > 0) {yield break;}
        while (true) {
            yield return new WaitForSeconds(hpRegenDelay);
            if (!canRegenHP || isHPRegenBlocked > 0) {yield break;}
            if (hp < maxHP) {
                if (hp + maxHP * 5 / 100 <= maxHP) {
                    hp += maxHP * 5 / 100;
                    hpBar.UpdateHP();
                }
                else {
                    hp = maxHP;
                    hpBar.UpdateHP();
                    StopHPRegen();
                }
            }
        }
    }

    public IEnumerator RegenerateLP() {
        yield return new WaitForSeconds(lpRegenWaitTime);
        while (lp < maxLP) {
            yield return new WaitForSeconds(lpRegenDelay);
            if (lp < maxLP) {
                if (lp + maxLP * 5 / 100 <= maxLP) {
                    lp += maxLP * 5 / 100;
                    lpBar.UpdateLP();
                }
                else {
                    lp = maxLP;
                    lpBar.UpdateLP();
                }
            }
        }
    }

    public void StartHPRegen() {
        canRegenHP = true;
        StartCoroutine(RegenerateHP());
    }

    public void StopHPRegen() {
        canRegenHP = false;
    }

    public void ChangeMaxMood(int number) {
        playerData.SetMaxMood(playerData.GetMaxMood() + number);
    }

    public int GetMaxMood() {
        return playerData.GetMaxMood();
    }

    public void DoubtedAnswer(bool state) {
        playerData.doubtedBio = state;
    }

    public void QuestionedOnD4() {
        playerData.questionedD4 = true;
    }

    public bool IsCrystal1Activated() {
        return playerData.activatedCrystal1;
    }

    public bool IsCrystal2Activated() {
        return playerData.activatedCrystal2;
    }

    public bool IsCrystal3Activated() {
        return playerData.activatedCrystal3;
    }

    public void ActivatedCrystal1() {
        playerData.activatedCrystal1 = true;
    }

    public void ActivatedCrystal2() {
        playerData.activatedCrystal2 = true;
    }

    public void ActivatedCrystal3() {
        playerData.activatedCrystal3 = true;
    }

    public int GetBigDoorStage() {
        return playerData.bigDoorStage;
    }

    public void SetBigDoorStage(int stage) {
        playerData.bigDoorStage = stage;
    }

    public void SetKyleSp1Par() {
        playerData.spokeToKyle1 = true;
    }

    public bool GetKyleSp1Par() {
        return playerData.spokeToKyle1;
    }

    public void SetChoice1(bool choice) {
        playerData.choice1 = choice;
    }

    public bool GetChoice1() {
        return playerData.choice1;
    }

    public void SetChoice2(bool choice) {
        playerData.choice2 = choice;
    }

    public bool GetChoice2() {
        return playerData.choice2;
    }

    public void BeatDay5() {
        playerData.beatDay5 = true;
    }
}
