using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Experience : MonoBehaviour
{
    int xp;
    public static Experience Instance;
    public TextMeshProUGUI experienceText;

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
    }

    public void GainExperience(int minimumAmount, int maximumAmount)
    {
        xp += Random.Range(minimumAmount, maximumAmount);
        experienceText.text = "XP: " + xp;
    }
}
