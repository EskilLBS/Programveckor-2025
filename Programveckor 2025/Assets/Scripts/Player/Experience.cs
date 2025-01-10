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
        xp += Random.Range(minimumAmount, maximumAmount);
        
        xpSlider.value = xp;

        if (xp >= levelUpThreshold)
        {
            LevelUp();
        }
    }

    void LevelUp() 
    {
        xp -= levelUpThreshold;
        playerLevel++;
        levelText.text = "Level: " + playerLevel;
        xpSlider.value = xp;
    }
}
