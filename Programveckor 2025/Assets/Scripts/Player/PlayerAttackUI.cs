using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttackUI : MonoBehaviour
{
    public static PlayerAttackUI Instance;

    PlayerUnit currentPlayerUnit;

    [SerializeField] GameObject attackUIPrefab;
    [SerializeField] GameObject attackUIParent;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void UpdateOptions()
    {
        foreach (GameObject child in attackUIParent.transform)
        {
            Destroy(child);
        }

        currentPlayerUnit = CombatManager.Instance.currentPlayerUnit;

        foreach (AttackBase attack in currentPlayerUnit.attacks)
        {
            GameObject go = Instantiate(attackUIPrefab);
            attackUIPrefab.transform.SetParent(attackUIParent.transform);

            go.GetComponentInChildren<TextMeshProUGUI>().text = attack.attackName;
            go.GetComponent<Button>().onClick.AddListener(() => CombatManager.Instance.AttackWithCurrentPlayerUnit(attack));
        }
       
    }
}
