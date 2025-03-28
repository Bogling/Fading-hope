using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        playerData.SetScene(SceneManager.GetActiveScene().name);
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

    public IEnumerator Respawn(Color faderColor, float fadeDuration, bool fadeIn, bool fadeOut) {
        if (fadeOut) {
            Fader.GetInstance().FadeOut(faderColor, fadeDuration);
            yield return new WaitForSeconds(fadeDuration);
        }
        var points = FindObjectsByType<CheckPoint>(FindObjectsSortMode.InstanceID);
        foreach (var point in points) {
            if (point.id == playerData.GetCheckPoint()) {
                gameObject.transform.position = point.getPoint().position;
                gameObject.GetComponent<Rigidbody>().position = point.getPoint().position;
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
                gameObject.GetComponent<Rigidbody>().position = point.getPoint().position;
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
        if (fadeOut) {
            Fader.GetInstance().FadeOut(faderColor, fadeDuration);
            yield return new WaitForSeconds(fadeDuration);
        }
        SceneManager.LoadScene("GameOverScreen");
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
        Respawn(Color.black, 5f, false, false);
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
        playerData.hasFlashlight = true;
        flashlight.SetActive(true);
    }

    public void TakeFlashlight() {
        playerData.hasFlashlight = false;
    }

    public void SetHP(float newHP) {
        hp = newHP;
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
            Respawn(Color.black, 1, true, true);
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

    /*public async void RegenerateHP() {
        await Task.Delay((int)(hpRegenWaitTime * 1000));
        if (!canRegenHP) {return;}
        while (true) {
            await Task.Delay((int)(hpRegenDelay * 1000));
            if (!canRegenHP) {break;}
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
    }*/

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

    public void DoubtedAnswer() {
        playerData.doubtedBio = true;
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
}
