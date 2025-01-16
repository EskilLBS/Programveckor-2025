using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraCharactersSpawn : MonoBehaviour
{
    // The start and end positions for the units
    [SerializeField] Transform secondUnitStartPos;
    [SerializeField] Transform secondUnitEndPos;
    [SerializeField] Transform thirdUnitStartPos;
    [SerializeField] Transform thirdUnitEndPos;

    // The transforms of the units
    [SerializeField] Transform secondUnit;
    [SerializeField] Transform thirdUnit;

    [SerializeField] float hideTime;

    // The move duration for the units in seconds
    [SerializeField] float moveDuration;

    // Called when combat is started, starts the movement
    public void OnStartCombat()
    {
        StartCoroutine(MoveUnits());
    }

    // Called to hide characters at the end of combat
    public IEnumerator HideCharacters()
    {
        float elapsedTime = 0;

        while(elapsedTime < hideTime)
        {
            Color secondUnitColor = secondUnit.GetComponent<SpriteRenderer>().color;
            Color thirdUnitColor = thirdUnit.GetComponent<SpriteRenderer>().color;

            secondUnit.GetComponent<SpriteRenderer>().color = 
                new Color(secondUnitColor.r, secondUnitColor.g, secondUnitColor.b, Mathf.Lerp(1, 0, elapsedTime / hideTime));

            thirdUnit.GetComponent<SpriteRenderer>().color =
                new Color(thirdUnitColor.r, thirdUnitColor.g, thirdUnitColor.b, Mathf.Lerp(1, 0, elapsedTime / hideTime));

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        secondUnit.gameObject.SetActive(false);
        thirdUnit.gameObject.SetActive(false);
    }

    // Moves the units
    IEnumerator MoveUnits()
    {
        Color secondUnitColor = secondUnit.GetComponent<SpriteRenderer>().color;
        Color thirdUnitColor = thirdUnit.GetComponent<SpriteRenderer>().color;

        secondUnit.GetComponent<SpriteRenderer>().color =
                new Color(secondUnitColor.r, secondUnitColor.g, secondUnitColor.b, 1);

        thirdUnit.GetComponent<SpriteRenderer>().color =
            new Color(thirdUnitColor.r, thirdUnitColor.g, thirdUnitColor.b, 1);

        secondUnit.gameObject.SetActive(true);
        thirdUnit.gameObject.SetActive(true);

        // A while loop that moves the units
        float elapsedTime = 0;
        while(elapsedTime <= moveDuration)
        {
            secondUnit.GetComponent<Animator>().SetBool("Walking", true);
            thirdUnit.GetComponent<Animator>().SetBool("Walking", true);

            // Lerps the position of the units over move duration amount of time
            secondUnit.position = Vector3.Lerp(secondUnitStartPos.position, secondUnitEndPos.position, elapsedTime / moveDuration);
            thirdUnit.position = Vector3.Lerp(thirdUnitStartPos.position, thirdUnitEndPos.position, elapsedTime / moveDuration);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        secondUnit.GetComponent<Animator>().SetBool("Walking", false);
        thirdUnit.GetComponent<Animator>().SetBool("Walking", false);
    }
}
