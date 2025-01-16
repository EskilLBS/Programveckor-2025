using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;

    // The unit that is currently targeted
    public UnitBase currentTarget { get; private set; }

    [SerializeField] GameObject playerUnit1;

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
    [SerializeField] List<PlayerUnit> playerCharacters;
    // A list of the player characters currently in combat
    [HideInInspector] public List<PlayerUnit> playersInCombat;
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
    [SerializeField] GameObject currentPlayerMarker;

    // A refernce to the player movement script
    [SerializeField] PlayerMovement playerMovement;

    // Player attacking ui and spare button
    [SerializeField] GameObject playerAttackUI;
    [SerializeField] GameObject spareButton;
    [SerializeField] TextMeshProUGUI currentTurnText;

    // A reference to the script that spawns in the second and third character
    ExtraCharactersSpawn charactersSpawn;

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
        charactersSpawn = GameObject.Find("Player Unit 1").GetComponent<ExtraCharactersSpawn>();
    }

    // Update is called once per frame
    void Update()
    {
        // A bunch of checks to see if the player has won or lost
        if(enemyCharacters.Count <= 0 && currentCombatState != CombatState.OutOfCombat)
        {
            currentCombatState = CombatState.Win;
        }

        if (playersInCombat.Count <= 0 && currentCombatState != CombatState.OutOfCombat)
        {
            currentCombatState = CombatState.Lose;
        }

        if (currentCombatState == CombatState.Win)
        {
            Win();
        }

        if(currentCombatState == CombatState.Lose)
        {
            Lose();
        }
    }

    // Set up the start of combat
    public void StartCombat(List<EnemyUnit> enemiesToFight)
    {
        currentPlayerMarker.SetActive(true);

        // Set the combat state, the players in combat and enemies to fight
        currentCombatState = CombatState.Start;

        if(playersInCombat.Count > 0)
        {
            playersInCombat.Clear();
        }

        playersInCombat = new List<PlayerUnit>(playerCharacters); // Set "playersInCombat" to a new list because
                                                                  // otherwise it'll share the same memory as "playerCharacters

        foreach (PlayerUnit unit in playerCharacters)
        {
            unit.OnStartCombat();
        }

        charactersSpawn.OnStartCombat();

        enemyCharacters = enemiesToFight;

        // Set a default target for the player
        if (currentTarget == null)
        {
            SetCurrentTarget(enemiesToFight[0]);
        }

        // Enable the player's attack ui and pause movement, then start the player turn
        currentTurnText.gameObject.SetActive(true);
        playerAttackUI.SetActive(true);
        playerMovement.SetPauseMovement(true);

        Debug.Log("In ocmbat");

        StartCoroutine(PlayerTurn());
    }

    // Assigns the selected attack to the current player unit and uses it
    public void AttackWithCurrentPlayerUnit(AttackBase newAttack)
    {
        // Make sure that it's the player's turn and that we're waiting for input
        if (currentCombatState == CombatState.PlayerTurn && awaitingPlayerInput == true)
        {
            // If there isn't a target, set the target to the first enemy in the enemy list
            if (currentTarget == null)
            {
                SetCurrentTarget(enemyCharacters[0]);
            }

            // Set the attack of the current player unit to the one that was passed in and attack with it
            currentPlayerUnit.AssignNewAttack(newAttack);
            currentPlayerUnit.Attack();

            // If there is still a target, meaning it didn't die, check if it's health is 1 or less and then enable the spare button
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
        currentPlayerMarker.SetActive(true);

        Debug.Log("Player turn");

        // Set the player attack ui to active and set the correct combat state
        playerAttackUI.SetActive(true);

        currentCombatState = CombatState.PlayerTurn;

        List<PlayerUnit> playersInCombatTemp = new List<PlayerUnit>(playersInCombat);

        // Loop through the players and let them attack
        foreach (PlayerUnit unit in playersInCombatTemp)
        {
            currentPlayerUnit = unit;

            currentPlayerMarker.transform.position = new Vector3(unit.transform.position.x, unit.transform.position.y + 2.2f, 0);

            currentTurnText.text = unit.unitName + "'s turn";

            // Update the avaliable attacks
            PlayerAttackUI.Instance.UpdateOptions();

            //Make sure we're waiting for input
            awaitingPlayerInput = true;

            // Wait until the player has made a decision on what to do, and then wait a second
            yield return new WaitUntil(() => awaitingPlayerInput == false);

            yield return new WaitForSeconds(1f);
        }

        Debug.Log("Enemy turn time");

        StartCoroutine(EnemyTurn());
    }

    // Perform the enemy turn
    IEnumerator EnemyTurn()
    {
        currentPlayerMarker.SetActive(false);

        if(currentCombatState == CombatState.EnemyTurn)
        {
            yield break;
        }

        // Set the player ui to inactive
        playerAttackUI.SetActive(false);

        // Make sure we're in the correct combat state
        currentCombatState = CombatState.EnemyTurn;

        //Loop through the enemies and let them attack
        foreach (EnemyUnit unit in enemyCharacters)
        {
            currentTurnText.text = unit.unitName + "'s turn";

            // Make sure the player loses if there are no more player characters
            if (playersInCombat.Count == 0)
            {
                currentCombatState = CombatState.Lose;
                yield break;
            }

            // Make the enemies attack and wait a second between attacks
            unit.Attack();

            yield return new WaitForSeconds(1f);
        }

       
        StartCoroutine(PlayerTurn());
    }

    // Called when the player wins
    void Win()
    {
        currentPlayerMarker.SetActive(false);
        // Hide the extra characters and the attacking ui, then unpause player movement
        charactersSpawn.HideCharacters();

        currentTurnText.gameObject.SetActive(false);
        playerAttackUI.SetActive(false);

        Debug.Log("You won!");
        currentCombatState = CombatState.OutOfCombat;

        playerMovement.gameObject.SetActive(true);
        playerMovement.SetPauseMovement(false);

        foreach (PlayerUnit unit in playerCharacters)
        {
            unit.OnStartCombat();
        }
    }

    // Called when the player loses
    void Lose()
    {
        currentPlayerMarker.SetActive(false);
        currentCombatState = CombatState.OutOfCombat; // Sets the combat state to out of combat

        // Enables the main player unit so that the player can move again
        playerUnit1.SetActive(true);
        playerUnit1.GetComponent<PlayerMovement>().SetPauseMovement(false);

        currentTurnText.gameObject.SetActive(false);

        // Move the player back a little bit so that they aren't in the aggro range collider anymore
        playerUnit1.transform.position += new Vector3(-1, 0, 0);

        foreach (PlayerUnit unit in playerCharacters)
        {
            unit.OnStartCombat();
        }

        Debug.Log("You lose");

        SceneManager.LoadScene(1);
    }

    // Called to set the current target and spawn a target marker on them
    public void SetCurrentTarget(UnitBase unit)
    {  
        // Return if the player isn't in combat
        if(currentCombatState == CombatState.OutOfCombat)
        {
            return;
        }

        // Set the target to the unit that was passed in and destroy the previous target marker
        currentTarget = unit;
        Destroy(currentTargetMarker);

        // If the unit that was passed in is null we return out of the function
        if (unit == null)
        {
            return;
        }

        // Set the "spareButton" to active if the current targets health is 1 or less, to allow sparing their life
        // otherwise we disable it
        if(currentTarget.health <= 1)
        {
            spareButton.SetActive(true);
        }
        else
        {
            spareButton.SetActive(false);
        }
       
        // Spawn a target marker and set it's sprite to the same one as the current target
        // we also set the scale to be a bit bigger than the target, to make sure that the target marker is visible
        currentTargetMarker = Instantiate(new GameObject(), unit.transform.position, Quaternion.identity);
        SpriteRenderer spriteRenderer = currentTargetMarker.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = unit.GetComponent<SpriteRenderer>().sprite;
        spriteRenderer.color = new Color(0, 0, 0, 0.7f);
        currentTargetMarker.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        currentTargetMarker.transform.position += new Vector3(0, 0, 1);
    }

    // Called when the player wants to spare the enemy
    public void SpareEnemy()
    {
        // Make sure it is actually the players turn and that we are waiting for their input
        if (currentCombatState == CombatState.PlayerTurn && awaitingPlayerInput == true)
        {
            // If there isn't a target, then set the target to the first unit in the enemy unit list
            if (currentTarget == null)
            {
                SetCurrentTarget(enemyCharacters[0]);
            }

            if(currentTarget.health <= 1)
            {
                // Set the enemy state to spared, call the OnSpared functino and then remove them from the enemy list
                currentTarget.GetComponent<EnemyUnit>().hasBeenSpared = true;
                currentTarget.GetComponent<EnemyUnit>().OnSpared();
                RemoveEnemyFromList(currentTarget.GetComponent<EnemyUnit>());
            }

            awaitingPlayerInput = false;
        }
    }

    // Remove an enemy from the enemy list
    public void RemoveEnemyFromList(EnemyUnit enemyToRemove)
    {
        if (enemyToRemove == currentTarget)
        {
            currentTarget = null;
        }

        enemyCharacters.Remove(enemyToRemove);
    }
}
