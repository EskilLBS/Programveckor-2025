using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GoodOrBadDecision : MonoBehaviour
{
    // Goodness and badness variables, used to determine if the player is good or evil
    int evilness;
    int goodness;

    public static GoodOrBadDecision Instance;

    [SerializeField] GameObject badDecisionExplosion;

    [SerializeField] Light2D globalLight;
    [SerializeField] float intensityMultiplier;
    [SerializeField] Gradient colorChangeGradient;
    [SerializeField] int maximumEvilness;

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

    void Start()
    {
        evilness = 0;
        goodness = 0;
    }

    // Called when a bad decision is made
    public void BadDecision(int increaseAmount)
    {
        //1 badness is added every time you make a bad decision
        evilness += increaseAmount;
        print("Det var ett dåligt val!");

        globalLight.color = colorChangeGradient.Evaluate(intensityMultiplier / evilness);
        globalLight.intensity = intensityMultiplier / evilness;

        //You get a warning after 3 bad decision
        if (evilness == 5)
        {
            SceneManager.LoadScene(1);
        }
    }

    // Called when a good decision is made
    public void GoodDecision(int increaseAmount)
    {
        //1 goodness is added every time you make a good decision
        goodness += increaseAmount;
        print("Det var ett bra val!");

        //You get a notification when you have made 3 good decisions
        if (goodness == 3)
        {
            print("Du har gjort flera bra val!!");
        }

        //After you make 4 good decisions, you save the world
        if (goodness == 4)
        {
            print("DU RÄDDADE VÄRLDEN!!");
        }
    }
}
