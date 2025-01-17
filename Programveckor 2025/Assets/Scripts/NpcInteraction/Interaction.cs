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

    [SerializeField] GameObject karmBar;

    string playerLeft = "Bye";
    float distanceX;
    float distanceY;

    bool IsTyping = false;
    int currentDialogue = 1;

    public float fadeDuration = 1.0f;
    bool hasAnswered = false;
    bool dialogueStarted = false;

    void Start()
    {
        playerBody = GetComponent<Rigidbody2D>();
        dialogue.transform.parent.gameObject.SetActive(false);
        dialogueEndedUi.transform.parent.gameObject.SetActive(false);
        interactText.alpha = 0f;
        answerB.gameObject.SetActive(false);
        cancelB.gameObject.SetActive(false);
    }

    private Coroutine currentTypingCoroutine;
    private Coroutine currentHideTextCoroutine;

    void Update()
    {
        distanceX = playerBody.transform.position.x - npcBody.transform.position.x;
        distanceY = playerBody.transform.position.y - npcBody.transform.position.y;

        if ((distanceX <= interactionDistance && distanceX >= -interactionDistance) && (distanceY <= interactionDistance && distanceY >= -interactionDistance)) //distance
        {
            AnimateText("Press E to interact");
            if (Input.GetKeyDown(KeyCode.E) && !IsTyping && !hasAnswered)
            {
                var (dialogueText, answer) = GetDialogueAndAnswer(currentDialogue);
                if (dialogueText != null)
                {
                    dialogueStarted = true;
                    dialogue.transform.parent.gameObject.SetActive(true);
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
                dialogue.transform.parent.gameObject.SetActive(false);
                currentDialogue = 1;
                answerUi.transform.parent.gameObject.SetActive(false);
                cancelUi.transform.parent.gameObject.SetActive(false);
                answerB.gameObject.SetActive(false);
                cancelB.gameObject.SetActive(false);
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

        dialogueEndedUi.transform.parent.gameObject.SetActive(false);
        IsTyping = true;
        dialogue.text = "";

        foreach (char letter in DialogueText)
        {
            dialogue.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
        currentDialogue += 1;
        IsTyping = false;
        if (currentDialogue < 5)
        {
            answerUi.transform.parent.gameObject.SetActive(true);
            cancelUi.transform.parent.gameObject.SetActive(true);
            answerB.gameObject.SetActive(true);
            cancelB.gameObject.SetActive(true);
        }
        currentTypingCoroutine = null;
    }

    //End Text (Player left)
    IEnumerator HideText(string playerLeft)
    {
        IsTyping = false;
        dialogue.text = "";
        dialogueEndedUi.transform.parent.gameObject.SetActive(true);
        dialogueEndedUi.text = "";

        // Write one letter every 0.05 seconds
        foreach (char letter in playerLeft)
        {
            dialogueEndedUi.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(0.7f);
        dialogueEndedUi.transform.parent.gameObject.SetActive(false);
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
        {1, ("Hello there Kami.", "Hello, what were those monsters over there?")}, // Npc dialogue, your reply
        {2, ("Oh those? Don't worry about it, just make sure you don't kill too many", "What do you mean?")},
        {3, ("Eh, nothing really, worst case the world explodes...", "Oh god, I might need some rest before continuing")},
        {4, ("Hold still, I'll heal you up", "ok")},
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
        if (currentDialogue == 2)
        {
            karmBar.SetActive(true);
        }

        if(currentDialogue == 4)
        {
            foreach (PlayerUnit unit in CombatManager.Instance.playerCharacters)
            {
                unit.FullHeal();
            }
        }

        var (dialogueText, answer) = GetDialogueAndAnswer(currentDialogue);
        if (dialogueText != null)
        {
            dialogueStarted = true;
            dialogue.transform.parent.gameObject.SetActive(true);
            if (currentTypingCoroutine != null)
            {
                StopCoroutine(currentTypingCoroutine);
            }
            currentTypingCoroutine = StartCoroutine(TypeText(dialogueText));
            answerUi.transform.parent.gameObject.SetActive(false);
            cancelUi.transform.parent.gameObject.SetActive(false);
            answerB.gameObject.SetActive(false);
            cancelB.gameObject.SetActive(false);

            answerUi.text = answer;
        }
    }

    public void Cancel()
    {
        currentDialogue = 10;
        var (dialogueText, answer) = GetDialogueAndAnswer(currentDialogue);
        if (dialogueText != null)
        {
            dialogueStarted = true;
            dialogue.gameObject.SetActive(true);
            if (currentTypingCoroutine != null)
            {
                StopCoroutine(currentTypingCoroutine);
            }
            currentTypingCoroutine = StartCoroutine(TypeText(dialogueText));
            answerUi.transform.parent.gameObject.SetActive(false);
            cancelUi.transform.parent.gameObject.SetActive(false);
            answerB.gameObject.SetActive(false);
            cancelB.gameObject.SetActive(false);
        }
    }
}