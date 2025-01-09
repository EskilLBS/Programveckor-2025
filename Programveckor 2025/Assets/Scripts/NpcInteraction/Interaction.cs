using System.Collections;
using UnityEngine;
using TMPro;
using System.Threading;

public class Interaction : MonoBehaviour
{
    Rigidbody2D PlayerBody;
    public Rigidbody2D NpcBody;
    public TextMeshProUGUI Dialogue;
    public TextMeshProUGUI DialogueEndedUi;
    public string DialogueText;
    string playerLeft = "Dialogue Ended";
    float distanceX;
    float distanceY;
    bool IsTyping = false;

    void Start()
    {
        PlayerBody = GetComponent<Rigidbody2D>();
        Dialogue.gameObject.SetActive(false);
        DialogueEndedUi.gameObject.SetActive(false);
    }

    void Update()
    {
        distanceX = PlayerBody.transform.position.x - NpcBody.transform.position.x;
        distanceY = PlayerBody.transform.position.y - NpcBody.transform.position.y;

        if ((distanceX <= 2f && distanceX >= -2f) && (distanceY <= 2f && distanceY >= -2f))
        {
            if (Input.GetKeyDown(KeyCode.E) && !IsTyping)
            {
                print("Player Is Close");
                Dialogue.gameObject.SetActive(true);

                if (!IsTyping)
                {
                    StartCoroutine(TypeText(DialogueText));
                }
            }
        }
        else
        {
            if (IsTyping)
            {
                StopCoroutine(TypeText(DialogueText));
                StartCoroutine(HideText(playerLeft));
            }
            Dialogue.gameObject.SetActive(false);
        }
    }

    // Coroutine for typing dialogue text
    IEnumerator TypeText(string DialogueText)
    {
        IsTyping = true;
        Dialogue.text = "";
        foreach (char letter in DialogueText)
        {
            Dialogue.text += letter;
            yield return new WaitForSeconds(0.05f); 
        }
        IsTyping = false;
    }

    // Hide Text (player left)
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
    }
}