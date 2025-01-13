using System.Collections;
using UnityEngine;
using TMPro;
using System.Threading;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine.UI;

public class Interaction : MonoBehaviour
{
    Rigidbody2D PlayerBody;
    public Rigidbody2D NpcBody;
    public TextMeshProUGUI Dialogue;
    public TextMeshProUGUI DialogueEndedUi;
    public TextMeshProUGUI InteractText;
    public TextMeshProUGUI AnswerUi;
    public TextMeshProUGUI CancelUi;
    public Button AnswerB;
    public Button CancelB;
    string playerLeft = "Dialogue Ended";
    float distanceX;
    float distanceY;
    bool IsTyping = false;
    int CurrentDialogue = 1;
    public float fadeDuration = 1.0f;
    bool hasAnswered = false;

    void Start()
    {
        PlayerBody = GetComponent<Rigidbody2D>();
        Dialogue.gameObject.SetActive(false);
        DialogueEndedUi.gameObject.SetActive(false);
        InteractText.alpha = 0f;
        AnswerB.interactable = false;
        CancelB.interactable = false;
    }

    private Coroutine currentTypingCoroutine;
    private Coroutine currentHideTextCoroutine;
    private bool dialogueStarted = false;
    void Update()
    {
        distanceX = PlayerBody.transform.position.x - NpcBody.transform.position.x;
        distanceY = PlayerBody.transform.position.y - NpcBody.transform.position.y;

        if ((distanceX <= 2f && distanceX >= -2f) && (distanceY <= 2f && distanceY >= -2f)) //distance
        {
            AnimateText("Hint: Press E to interact");
            if (Input.GetKeyDown(KeyCode.E) && !IsTyping && !hasAnswered)
            {
                var (dialogueText, answer) = GetDialogueAndAnswer(CurrentDialogue);
                if (dialogueText != null)
                {
                    dialogueStarted = true;
                    Dialogue.gameObject.SetActive(true);
                    if (currentTypingCoroutine != null)
                    {
                        StopCoroutine(currentTypingCoroutine);
                    }
                    currentTypingCoroutine = StartCoroutine(TypeText(dialogueText));
                    hasAnswered = true;
                    AnswerUi.text = answer;
                }
            }
        }
        else
        {
            InteractText.alpha = 0f;
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
                Dialogue.gameObject.SetActive(false);
                CurrentDialogue = 1;
                AnswerUi.gameObject.SetActive(false);
                CancelUi.gameObject.SetActive(false);
                AnswerB.interactable = false;
                CancelB.interactable = false;
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

        DialogueEndedUi.gameObject.SetActive(false);
        IsTyping = true;
        Dialogue.text = "";

        foreach (char letter in DialogueText)
        {
            Dialogue.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
        CurrentDialogue += 1;
        IsTyping = false;
        if (CurrentDialogue < 4)
        {
            AnswerUi.gameObject.SetActive(true);
            CancelUi.gameObject.SetActive(true);
            AnswerB.interactable = true;
            CancelB.interactable = true;
        }
        currentTypingCoroutine = null;
    }

    //End Text (Player left)
    IEnumerator HideText(string playerLeft)
    {
        IsTyping = false;
        Dialogue.text = "";
        DialogueEndedUi.gameObject.SetActive(true);
        DialogueEndedUi.text = "";
        DialogueEndedUi.color = Color.red;

        foreach (char letter in playerLeft)
        {
            DialogueEndedUi.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(0.7f);
        DialogueEndedUi.gameObject.SetActive(false);
        currentHideTextCoroutine = null; 
    }
    public void AnimateText(string message)
    {
        ShowText(message);  // Fade-in
    }
    public void ShowText(string message)
    {
        InteractText.text = message;
        StartCoroutine(FadeInText());
    }

    IEnumerator FadeInText()
    {
        InteractText.alpha = 0f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            InteractText.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        InteractText.alpha = 1f;
    }
    IEnumerator FadeOutText()
    {
        InteractText.alpha = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            InteractText.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        InteractText.alpha = 0f;
    }

    //List of all the dialogues
    public (string, string) GetDialogueAndAnswer(int CurrentDialogue)
    {
        Dictionary<int, (string dialogue, string answer)> texts = new Dictionary<int, (string, string)>
    {
        {1, ("Hello there, it's nice to see you", "Hello, how can i help?")}, // Npc dialogue, your reply
        {2, ("There's things lurking nearby... will you help me?", "Sure")},
        {3, ("What are you still doing here then?? Get going", "ok")},
        {10, ("Oh ok.", "None")}
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
            Dialogue.gameObject.SetActive(true);
            if (currentTypingCoroutine != null)
            {
                StopCoroutine(currentTypingCoroutine);
            }
            currentTypingCoroutine = StartCoroutine(TypeText(dialogueText));
            AnswerUi.gameObject.SetActive(false);
            CancelUi.gameObject.SetActive(false);
            AnswerB.interactable = false;
            CancelB.interactable = false;

            AnswerUi.text = answer;
        }
    }

    public void Cancel()
    {
        CurrentDialogue = 10;
        var (dialogueText, answer) = GetDialogueAndAnswer(CurrentDialogue);
        if (dialogueText != null)
        {
            dialogueStarted = true;
            Dialogue.gameObject.SetActive(true);
            if (currentTypingCoroutine != null)
            {
                StopCoroutine(currentTypingCoroutine);
            }
            currentTypingCoroutine = StartCoroutine(TypeText(dialogueText));
            AnswerUi.gameObject.SetActive(false);
            CancelUi.gameObject.SetActive(false);
            AnswerB.interactable = false;
            CancelB.interactable = false;
        }
    }
}