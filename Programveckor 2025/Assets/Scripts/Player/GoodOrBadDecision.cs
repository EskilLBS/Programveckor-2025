using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GoodOrBadDecision : MonoBehaviour
{
    // Goodness and badness variables, used to determine if the player is good or evil
    int karma;

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
        karma = 0;
    }

    // Called when a bad decision is made
    public void BadDecision(int increaseAmount)
    {
        //1 badness is added every time you make a bad decision
        karma -= increaseAmount;
        print("Det var ett dåligt val!");

        if(karma < 0)
        {
            globalLight.color = colorChangeGradient.Evaluate(intensityMultiplier / -karma);
            globalLight.intensity = intensityMultiplier / -karma;
        }
        

        //You get a warning after 3 bad decision
        if (karma == -maximumEvilness)
        {
            SceneManager.LoadScene(1);
        }
    }

    // Called when a good decision is made
    public void GoodDecision(int increaseAmount)
    {
        //1 goodness is added every time you make a good decision
        karma += increaseAmount;
        print("Det var ett bra val!");

        //You get a notification when you have made 3 good decisions
        if (karma == 3)
        {
            print("Du har gjort flera bra val!!");
        }

        //After you make 4 good decisions, you save the world
        if (karma == 4)
        {
            print("DU RÄDDADE VÄRLDEN!!");
        }
    }
}
