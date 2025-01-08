using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;

    // The unit that is currently targeted
    public UnitBase currentTarget;

    // Enum for different combat states
    public enum CombatState
    {
        Start,
        PlayerTurn,
        EnemyTurn,
        Win,
        Lose
    }

    public CombatState currentCombatState;

    // A list of the player characters
    [SerializeField] List<PlayerUnit> playerCharacters;
    //The current player unit that has to attack
    PlayerUnit currentPlayerUnit;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(currentCombatState == CombatState.Start)
        {
            CombatStart();
        }
    }

    void CombatStart()
    {
        currentCombatState = CombatState.PlayerTurn;
        StartCoroutine(PlayerTurn());
    }

    IEnumerator PlayerTurn()
    {
        foreach (PlayerUnit unit in playerCharacters)
        {
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Mouse0));

            unit.Attack();

            yield return new WaitForSeconds(1f);
        }


    }
}
