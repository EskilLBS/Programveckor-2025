using System.Collections;
using UnityEngine;
using TMPro;
using System.Threading;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine.UI;

public class Interaction : MonoBehaviour
{
    Rigidbody2D playerBody;
    [SerializeField] Rigidbody2D npcBody;

    [SerializeField] TextMeshProUGUI dialogue;
    [SerializeField] TextMeshProUGUI dialogueEndedUi;

    [SerializeField] TextMeshProUGUI interactText;
    [SerializeField] float interactionDistance = 3.5f;

    [SerializeField] TextMeshProUGUI answerUi;
    [SerializeField] TextMeshProUGUI cancelUi;
    [SerializeField] Button answerB;
    [SerializeField] Button cancelB;

    string playerLeft = "Bye";
    float distanceX;
    float distanceY;

    bool IsTyping = false;
    int CurrentDialogue = 1;

    public float fadeDuration = 1.0f;
    bool hasAnswered = false;
    bool dialogueStarted = false;

    void Start()
    {
        playerBody = GetComponent<Rigidbody2D>();
        dialogue.gameObject.SetActive(false);
        dialogueEndedUi.gameObject.SetActive(false);
        interactText.alpha = 0f;
        answerB.interactable = false;
        cancelB.interactable = false;
    }

    private Coroutine currentTypingCoroutine;
    private Coroutine currentHideTextCoroutine;
    
    void Update()
    {
        distanceX = playerBody.transform.position.x - npcBody.transform.position.x;
        distanceY = playerBody.transform.position.y - npcBody.transform.position.y;

        if ((distanceX <= interactionDistance && distanceX >= -interactionDistance) && (distanceY <= interactionDistance && distanceY >= -interactionDistance)) //distance
        {
            AnimateText("Hint: Press E to interact");
            if (Input.GetKeyDown(KeyCode.E) && !IsTyping && !hasAnswered)
            {
                var (dialogueText, answer) = GetDialogueAndAnswer(CurrentDialogue);
                if (dialogueText != null)
                {
                    dialogueStarted = true;
                    dialogue.gameObject.SetActive(true);
                    if (currentTypingCoroutine != null)
                    {
                        StopCoroutine(currentTypingCoroutine);
                    }
                    currentTypingCoroutine = StartCoroutine(TypeText(dialogueText));
                    hasAnswered = true;
                    answerUi.text = answer;
                }
            }
        }
        else
        {
            interactText.alpha = 0f;
            hasAnswered = false;

            if (dialogueStarted)
            {
                dialogueStarted = false; //reset dialogue
                if (currentTypingCoroutine != null)
                {
                    StopCoroutine(currentTypingCoroutine);
                    currentTypingCoroutine = null;
                }
                if (currentHideTextCoroutine == null)
                {
                    currentHideTextCoroutine = StartCoroutine(HideText(playerLeft));
                }
                dialogue.gameObject.SetActive(false);
                CurrentDialogue = 1;
                answerUi.gameObject.SetActive(false);
                cancelUi.gameObject.SetActive(false);
                answerB.interactable = false;
                cancelB.interactable = false;
            }
        }
    }
    IEnumerator TypeText(string DialogueText)
    {
        if (currentHideTextCoroutine != null)
        {
            StopCoroutine(currentHideTextCoroutine);
            currentHideTextCoroutine = null;
        }

        dialogueEndedUi.gameObject.SetActive(false);
        IsTyping = true;
        dialogue.text = "";

        foreach (char letter in DialogueText)
        {
            dialogue.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
        CurrentDialogue += 1;
        IsTyping = false;
        if (CurrentDialogue < 4)
        {
            answerUi.gameObject.SetActive(true);
            cancelUi.gameObject.SetActive(true);
            answerB.interactable = true;
            cancelB.interactable = true;
        }
        currentTypingCoroutine = null;
    }

    //End Text (Player left)
    IEnumerator HideText(string playerLeft)
    {
        IsTyping = false;
        dialogue.text = "";
        dialogueEndedUi.gameObject.SetActive(true);
        dialogueEndedUi.text = "";

        // Write one letter every 0.05 seconds
        foreach (char letter in playerLeft)
        {
            dialogueEndedUi.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(0.7f);
        dialogueEndedUi.gameObject.SetActive(false);
        currentHideTextCoroutine = null; 
    }
    public void AnimateText(string message)
    {
        ShowText(message);  // Fade-in
    }
    public void ShowText(string message)
    {
        interactText.text = message;
        StartCoroutine(FadeInText());
    }

    IEnumerator FadeInText()
    {
        interactText.alpha = 0f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            interactText.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        interactText.alpha = 1f;
    }
    IEnumerator FadeOutText()
    {
        interactText.alpha = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            interactText.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        interactText.alpha = 0f;
    }

    //List of all the dialogues
    public (string, string) GetDialogueAndAnswer(int CurrentDialogue)
    {
        Dictionary<int, (string dialogue, string answer)> texts = new Dictionary<int, (string, string)>
    {
        {1, ("Hello there, it's nice to see you", "Hello, what were those monsters over there?")}, // Npc dialogue, your reply
        {2, ("Oh those? Don't worry about it, you did the right thing by not killing them", "What do you mean?")},
        {3, ("Eh, nothing really, you only made sure the world didn't explode", "ok")},
        {10, ("Oh ok, bye.", "None")}
    };

        if (texts.ContainsKey(CurrentDialogue))
        {
            var text = texts[CurrentDialogue];
            return (text.dialogue, text.answer);
        }

        return (null, null);
    }


    public void Answer()
    {
        var (dialogueText, answer) = GetDialogueAndAnswer(CurrentDialogue);
        if (dialogueText != null)
        {
            dialogueStarted = true;
            dialogue.gameObject.SetActive(true);
            if (currentTypingCoroutine != null)
            {
                StopCoroutine(currentTypingCoroutine);
            }
            currentTypingCoroutine = StartCoroutine(TypeText(dialogueText));
            answerUi.gameObject.SetActive(false);
            cancelUi.gameObject.SetActive(false);
            answerB.interactable = false;
            cancelB.interactable = false;

            answerUi.text = answer;
        }
    }

    public void Cancel()
    {
        CurrentDialogue = 10;
        var (dialogueText, answer) = GetDialogueAndAnswer(CurrentDialogue);
        if (dialogueText != null)
        {
            dialogueStarted = true;
            dialogue.gameObject.SetActive(true);
            if (currentTypingCoroutine != null)
            {
                StopCoroutine(currentTypingCoroutine);
            }
            currentTypingCoroutine = StartCoroutine(TypeText(dialogueText));
            answerUi.gameObject.SetActive(false);
            cancelUi.gameObject.SetActive(false);
            answerB.interactable = false;
            cancelB.interactable = false;
        }
    }
}