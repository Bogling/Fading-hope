using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;

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

    private bool choicesPresent = false;

    private Coroutine typeTextCoroutine;

    public bool dialogueIsPlaying { get; private set; }

    public bool isLocked = false;

    private static DreamDialogueController instance;
    

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
        DisplayNextParagraph(currentInkJSON);
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
}

    private void DisplayNextParagraph(TextAsset inkJSON) {
        if (!isTyping) {
            if (currentStory.canContinue) {
                if (choicesPresent) {
                    return;
                }
                p = currentStory.Continue();
                typeTextCoroutine = StartCoroutine(TypeDialogueText(p));
                DisplayChoices();
            }
            else if (!choicesPresent) {
                ExitDialogue();
            }
        }

        else {
            FinishParagraphEarly();
        }
    }

    
    private IEnumerator TypeDialogueText(string p) {
        isTyping = true;
        dialogueText.text = p;
        
        int maxVisibleChars = 0;

        foreach (char c in p.ToCharArray()) {
            maxVisibleChars++;
            dialogueText.maxVisibleCharacters = maxVisibleChars;

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
}
