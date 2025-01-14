using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraCharactersSpawn : MonoBehaviour
{
    [SerializeField] Transform secondUnitStartPos;
    [SerializeField] Transform secondUnitEndPos;
    [SerializeField] Transform thirdUnitStartPos;
    [SerializeField] Transform thirdUnitEndPos;

    [SerializeField] Transform secondUnit;
    [SerializeField] Transform thirdUnit;

    [SerializeField] float moveDuration;


    public void OnStartCombat()
    {
        Debug.Log("Test 1");

        StartCoroutine(MoveUnits());
    }

    public void HideCharacters()
    {
        secondUnit.gameObject.SetActive(false);
        thirdUnit.gameObject.SetActive(false);
    }

    IEnumerator MoveUnits()
    {
        secondUnit.gameObject.SetActive(true);
        thirdUnit.gameObject.SetActive(true);

        Debug.Log("hej");

        float elapsedTime = 0;
        while(elapsedTime <= moveDuration)
        {
            Debug.Log(elapsedTime);
            secondUnit.position = Vector3.Lerp(secondUnitStartPos.position, secondUnitEndPos.position, elapsedTime / moveDuration);
            thirdUnit.position = Vector3.Lerp(thirdUnitStartPos.position, thirdUnitEndPos.position, elapsedTime / moveDuration);

            elapsedTime += Time.deltaTime;

            yield return null;
        }


    }
}
