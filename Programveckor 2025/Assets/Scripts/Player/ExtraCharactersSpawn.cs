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

    // The move duration for the units in seconds
    [SerializeField] float moveDuration;

    // Called when combat is started, starts the movement
    public void OnStartCombat()
    {
        StartCoroutine(MoveUnits());
    }

    // Called to hide characters at the end of combat
    public void HideCharacters()
    {
        secondUnit.gameObject.SetActive(false);
        thirdUnit.gameObject.SetActive(false);
    }

    // Moves the units
    IEnumerator MoveUnits()
    {
        secondUnit.gameObject.SetActive(true);
        thirdUnit.gameObject.SetActive(true);

        // A while loop that moves the units
        float elapsedTime = 0;
        while(elapsedTime <= moveDuration)
        {
            // Lerps the position of the units over move duration amount of time
            secondUnit.position = Vector3.Lerp(secondUnitStartPos.position, secondUnitEndPos.position, elapsedTime / moveDuration);
            thirdUnit.position = Vector3.Lerp(thirdUnitStartPos.position, thirdUnitEndPos.position, elapsedTime / moveDuration);

            elapsedTime += Time.deltaTime;

            yield return null;
        }


    }
}
