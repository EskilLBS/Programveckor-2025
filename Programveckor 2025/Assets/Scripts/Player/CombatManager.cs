using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;

    // The unit that is currently targeted
    public UnitBase currentTarget { get; private set; }

    // Enum for different combat states
    public enum CombatState
    {
        OutOfCombat,
        Start,
        PlayerTurn,
        EnemyTurn,
        Win,
        Lose
    }

    public CombatState currentCombatState;

    // A list of the player characters
    public List<PlayerUnit> playerCharacters;
    // The current player unit that has to attack
    [HideInInspector] public PlayerUnit currentPlayerUnit;
    // The 
    bool awaitingPlayerInput = false;

    // A list of the enemy characters
    [SerializeField] List<EnemyUnit> enemyCharacters;
    // The current enemy character
    EnemyUnit currentEnemyUnit;
    // A marker on the current target to show which unit is targeted
    [HideInInspector] public GameObject currentTargetMarker { get; private set; }

    [SerializeField] PlayerMovement playerMovement;

    [SerializeField] GameObject playerAttackUI;
    [SerializeField] GameObject spareButton;

    private void Awake()
    {
        if (Instance == null)
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
        if(enemyCharacters.Count <= 0 && currentCombatState != CombatState.OutOfCombat)
        {
            currentCombatState = CombatState.Win;
        }

        if(currentCombatState == CombatState.Win)
        {
            Win();
        }
    }

    // Set up the start of combat
    public void StartCombat()
    {
        currentCombatState = CombatState.Start;

        if (currentTarget == null)
        {
            currentTarget = enemyCharacters[0];
        }

        playerAttackUI.SetActive(true);
        playerMovement.SetPauseMovement(true);

        StartCoroutine(PlayerTurn());
    }

    // Assigns the selected attack to the current player unit and uses it
    public void AttackWithCurrentPlayerUnit(AttackBase newAttack)
    {
        if (currentCombatState == CombatState.PlayerTurn && awaitingPlayerInput == true)
        {
           
            if (currentTarget == null)
            {
                SetCurrentTarget(enemyCharacters[0]);
            }

            currentPlayerUnit.AssignNewAttack(newAttack);
            currentPlayerUnit.Attack();

            if(currentTarget != null)
            {
                if (currentTarget.health <= 1)
                {
                    spareButton.SetActive(true);
                }
            }
            

            awaitingPlayerInput = false;
        }
    }

    // Perform the player turn
    IEnumerator PlayerTurn()
    {
        playerAttackUI.SetActive(true);

        currentCombatState = CombatState.PlayerTurn;

        foreach (PlayerUnit unit in playerCharacters)
        {
            currentPlayerUnit = unit;

            PlayerAttackUI.Instance.UpdateOptions();

            awaitingPlayerInput = true;

            yield return new WaitUntil(() => awaitingPlayerInput == false);

            yield return new WaitForSeconds(1f);
        }

        StartCoroutine(EnemyTurn());
    }

    // Perform the enemy turn
    IEnumerator EnemyTurn()
    {
        playerAttackUI.SetActive(false);

        currentCombatState = CombatState.EnemyTurn;

        foreach (EnemyUnit unit in enemyCharacters)
        {
            unit.Attack();

            yield return new WaitForSeconds(1f);
        }

        StartCoroutine(PlayerTurn());
    }

    // Called when the player wins
    void Win()
    {
        playerAttackUI.SetActive(false);

        Debug.Log("You won!");
        currentCombatState = CombatState.OutOfCombat;

        playerMovement.SetPauseMovement(false);
    }

    public void SetCurrentTarget(UnitBase unit)
    {  
        if(currentCombatState == CombatState.OutOfCombat)
        {
            return;
        }

        currentTarget = unit;
        Destroy(currentTargetMarker);

        if (unit == null)
        {
            return;
        }

        if(currentTarget.health <= 1)
        {
            spareButton.SetActive(true);
        }
        else
        {
            spareButton.SetActive(false);
        }
       
        currentTargetMarker = Instantiate(new GameObject(), unit.transform.position, Quaternion.identity);
        SpriteRenderer spriteRenderer = currentTargetMarker.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = unit.GetComponent<SpriteRenderer>().sprite;
        currentTargetMarker.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        currentTargetMarker.transform.position += new Vector3(0, 0, 1);
    }

    public void SpareEnemy()
    {
        if (currentCombatState == CombatState.PlayerTurn && awaitingPlayerInput == true)
        {

            if (currentTarget == null)
            {
                SetCurrentTarget(enemyCharacters[0]);
            }

            if(currentTarget.health == 1)
            {
                currentTarget.GetComponent<EnemyUnit>().spared = true;
                currentTarget.GetComponent<EnemyUnit>().OnSpared();
                RemoveEnemyFromList(currentTarget.GetComponent<EnemyUnit>());
            }

            awaitingPlayerInput = false;
        }
    }

    public void RemoveEnemyFromList(EnemyUnit enemyToRemove)
    {
        if (enemyToRemove == currentTarget)
        {
            currentTarget = null;
        }

        enemyCharacters.Remove(enemyToRemove);
    }
}
