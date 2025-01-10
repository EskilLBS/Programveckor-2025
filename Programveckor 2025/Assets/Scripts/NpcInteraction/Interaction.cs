using System.Collections;
using UnityEngine;
using TMPro;
using System.Threading;
using System.Collections.Generic;
using UnityEditor.Rendering;

public class Interaction : MonoBehaviour
{
    Rigidbody2D PlayerBody;
    public Rigidbody2D NpcBody;
    public TextMeshProUGUI Dialogue;
    public TextMeshProUGUI DialogueEndedUi;
    public TextMeshProUGUI InteractText;
    string playerLeft = "Dialogue Ended";
    float distanceX;
    float distanceY;
    bool IsTyping = false;
    int CurrentDialogue = 1;
    public float fadeDuration = 1.0f;

    void Start()
    {
        PlayerBody = GetComponent<Rigidbody2D>();
        Dialogue.gameObject.SetActive(false);
        DialogueEndedUi.gameObject.SetActive(false);
        InteractText.alpha = 0f;
    }

    private Coroutine currentTypingCoroutine;
    private Coroutine currentHideTextCoroutine;
    private bool dialogueStarted = false;
    void Update()
    {
        distanceX = PlayerBody.transform.position.x - NpcBody.transform.position.x;
        distanceY = PlayerBody.transform.position.y - NpcBody.transform.position.y;

        if ((distanceX <= 2f && distanceX >= -2f) && (distanceY <= 2f && distanceY >= -2f))
        {
            AnimateText("Press E to interact");
            if (Input.GetKeyDown(KeyCode.E) && !IsTyping)
            {
                string dialogueText = CheckLog(CurrentDialogue);
                if (dialogueText != null)
                {
                    dialogueStarted = true;
                    Dialogue.gameObject.SetActive(true);
                    if (currentTypingCoroutine != null)
                    {
                        StopCoroutine(currentTypingCoroutine);
                    }
                    currentTypingCoroutine = StartCoroutine(TypeText(dialogueText));
                }
            }
        }
        else
        {
            InteractText.alpha = 0f;

            // Stop the fade-in

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
        currentTypingCoroutine = null;
    }

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
        yield return new WaitForSeconds(1);
        DialogueEndedUi.gameObject.SetActive(false);
        currentHideTextCoroutine = null;  // Clear reference after finishing
    }
    public void AnimateText(string message)
    {
        ShowText(message);  // Fade-in
        //StartCoroutine(ScaleUpText(1.0f));
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


    //  IEnumerator ScaleUpText(float duration)
    // {
    //  InteractText.transform.localScale = Vector3.zero;
    //Vector3 targetScale = Vector3.one;
    //float elapsedTime = 0f;

    //while (elapsedTime < duration)
    //{
    //  InteractText.transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, elapsedTime / duration);
    //elapsedTime += Time.deltaTime;
    //yield return null;
    // }
    //InteractText.transform.localScale = targetScale;
    //}

    public string CheckLog(int CurrentDialogue)
    {
        Dictionary<int, string> texts = new Dictionary<int, string>
        {
            {1, "Hello there, it's nice to see you [Press E to continue]" },
            {2, "We meet again [Press E to continue]" },
            {3, "What are you still doing here? get going" }
        };

        foreach (KeyValuePair<int, string> kvp in texts)
        {
            if (kvp.Key == CurrentDialogue)
            {
                print(kvp.Value);
                return kvp.Value;
            }
        }
        return null;

    }
}