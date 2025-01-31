using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Experience : MonoBehaviour
{
    int xp;
    public static Experience Instance;
    [SerializeField] TextMeshProUGUI levelText;

    [SerializeField] int levelUpThreshold;
    [SerializeField] Slider xpSlider;

    [SerializeField] GameObject levelUpUI;
    public int playerLevel { get; private set; }

    public Experience()
    {
        playerLevel = 0;
    }

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

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        xp = 0;
        levelText.text = "Level: " + playerLevel;
        xpSlider.maxValue = levelUpThreshold;
    }

    public void GainExperience(int minimumAmount, int maximumAmount)
    {
        // Add a random amount of xp to the player, based on the provided min and max amounts
        xp += Random.Range(minimumAmount, maximumAmount);
        
        xpSlider.value = xp;

        if (xp >= levelUpThreshold)
        {
            LevelUp();
        }
    }

    // Level up the player and update UI
    void LevelUp() 
    {
        xp -= levelUpThreshold;
        playerLevel++;
        levelText.text = "Level: " + playerLevel;
        xpSlider.value = xp;

        StartCoroutine(FlashLevelUpUI());

        foreach (PlayerUnit playerUnit in CombatManager.Instance.playerCharacters)
        {
            playerUnit.IncreaseMaxHealth();
        }
    }

    IEnumerator FlashLevelUpUI()
    {
        for (int i = 0; i < 4; i++)
        {
            levelUpUI.SetActive(true);

            yield return new WaitForSeconds(0.4f);

            levelUpUI.SetActive(false);
        }
    }
}
