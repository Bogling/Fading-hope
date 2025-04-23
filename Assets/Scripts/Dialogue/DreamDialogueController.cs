using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.SearchService;

public class DreamDialogueController : MonoBehaviour
{
    [SerializeField] private DreamPlayerInputController playerInput;
    [Header("Dialogue UI")]
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private float speed = 10;
    [SerializeField] private float MAX_TYPE_TIME = 0.1f;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;

    private TextMeshProUGUI[] choicesText;
    private Story currentStory;

    private bool isTyping;
    private string p;

    private TextAsset currentInkJSON;
    private ITalkable currentObject;

    private bool choicesPresent = false;
    private bool isInDelay = false;

    private Coroutine typeTextCoroutine;

    public bool dialogueIsPlaying { get; private set; }

    public bool isLocked = false;
    private bool buttonPressed = false;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] audioClips;
    private static DreamDialogueController instance;

    private const string COLOR_TAG = "color";
    private const string IMAGE_TAG = "image";
    private const string DELAY_TAG = "wait";
    private const string SOUND_TAG = "sound";

    private void Awake() {
        if (instance != null) {
            Debug.Log("How");
        }
        instance = this;
        dialogueIsPlaying = false;
        gameObject.SetActive(false);
    }

    public static DreamDialogueController GetInstance() {
        return instance;
    }

private void Start() {
    choicesText = new TextMeshProUGUI[choices.Length];
    for (int i = 0; i < choices.Length; i++) {
        choicesText[i] = choices[i].GetComponentInChildren<TextMeshProUGUI>();
    }
}

private void Update() {
    if (!dialogueIsPlaying) {
        return;
    }

    if (DreamPlayerInputController.GetInstance().GetSubmitPressed()) {
        if (!buttonPressed) {
            DisplayNextParagraph(currentInkJSON);
            buttonPressed = true;
        }
    }
    else {
        buttonPressed = false;
    }
}

public void EnterDialogue(TextAsset inkJSON, ITalkable obj) {
    currentInkJSON = inkJSON;
    currentStory = new Story(inkJSON.text);
    dialogueIsPlaying = true;
    playerInput.DisableInput();
    gameObject.SetActive(true);
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;

    currentObject = obj;
    currentStory.BindExternalFunction("choiceMade", (int qID, int cID) => {
        obj.OperateChoice(qID, cID);
    });
    currentStory.BindExternalFunction("focusCamera", () => {
        obj.Focus();
    });

    DisplayNextParagraph(inkJSON);
}


public void EnterDialogue(TextAsset inkJSON, ITalkable obj, string varName, string var) {
    currentInkJSON = inkJSON;
    currentStory = new Story(inkJSON.text);
    currentStory.variablesState[varName] = var;
    dialogueIsPlaying = true;
    gameObject.SetActive(true);
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;

    currentObject = obj;
    currentStory.BindExternalFunction("choiceMade", (int qID, int cID) => {
        obj.OperateChoice(qID, cID);
    });

    DisplayNextParagraph(inkJSON);
}


private void ExitDialogue() {
    dialogueIsPlaying = false;
    gameObject.SetActive(false);
    dialogueText.text = "";
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
    playerInput.EnableInput();

    currentStory.UnbindExternalFunction("choiceMade");
    currentStory.UnbindExternalFunction("focusCamera");
    currentObject.UponExit();
}

    private void DisplayNextParagraph(TextAsset inkJSON) {
        if (!isTyping) {
            if (currentStory.canContinue) {
                if (choicesPresent) {
                    return;
                }
                if (isInDelay) {
                    gameObject.transform.localScale = new Vector3(0, 0, 0);
                    return;
                }
                p = currentStory.Continue();
                if (p == "") {
                    DisplayNextParagraph(currentInkJSON);
                    return;
                }
                typeTextCoroutine = StartCoroutine(TypeDialogueText(p));
                DisplayChoices();
                HandleTags(currentStory.currentTags);
            }
            else if (!choicesPresent) {
                ExitDialogue();
            }
        }

        else {
            FinishParagraphEarly();
        }
    }

    private void HandleTags(List<string> currentTags) {
        foreach (string tag in currentTags) {
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2) {
                Debug.LogError("Tag could not be appropriately parsed: " + tag);
            }

            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            switch (tagKey) {
                case COLOR_TAG:
                    Debug.Log("color=" + tagValue);
                    Color newColor;
                    ColorUtility.TryParseHtmlString("#" + tagValue, out newColor);
                    dialogueText.color = newColor;
                    break;
                case IMAGE_TAG:
                    Debug.Log("image=" + tagValue);
                    break;
                case DELAY_TAG:
                    Debug.Log("wait=" + tagValue);
                    StartCoroutine(Delay(int.Parse(tagValue)));
                    break;
                case SOUND_TAG:
                    Debug.Log("sound=" + tagValue);
                    if (tagValue == "null") {
                        audioSource.clip = null;
                    }
                    else {
                        audioSource.clip = audioClips[int.Parse(tagValue)];
                    }
                    break;
                default:
                    Debug.Log("No such key");
                    break;
            }
        }
    }
    
    private IEnumerator TypeDialogueText(string p) {
        isTyping = true;
        dialogueText.text = p;
        
        int maxVisibleChars = 0;

        foreach (char c in p.ToCharArray()) {
            maxVisibleChars++;
            dialogueText.maxVisibleCharacters = maxVisibleChars;
            audioSource.Stop();
            audioSource.pitch = Random.Range(0.7f, 1.3f);
            audioSource.Play();
            
            yield return new WaitForSeconds(MAX_TYPE_TIME / speed);
        }

        isTyping = false;
    }

    private void FinishParagraphEarly()
{
    if (dialogueText.maxVisibleCharacters >= 5) {
        StopCoroutine(typeTextCoroutine);
        dialogueText.maxVisibleCharacters = p.Length; 
        isTyping = false;
    }
}

    private void DisplayChoices() {
        List<Choice> currentChoices = currentStory.currentChoices;

        int index = 0;
        foreach (Choice choice in currentChoices) {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }

        if (index > 0) {
            choicesPresent = true;
        }
        else {
            choicesPresent = false;
        }

        for (int i = index; i < choices.Length; i++) {
            choices[i].gameObject.SetActive(false);
        }
    }

    public void MakeChoice(int choiceIndex) {
        currentStory.ChooseChoiceIndex(choiceIndex);
        choicesPresent = false;
        DisplayNextParagraph(currentInkJSON);
        Debug.Log(currentStory.variablesState);
    }

    private IEnumerator Delay(int time) {
        isInDelay = true;
        yield return new WaitForSeconds(time);
        isInDelay = false;
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        DisplayNextParagraph(currentInkJSON);
    }

    public void forceEndDialogue() {
        ExitDialogue();
    }
}
