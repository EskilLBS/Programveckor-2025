using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Karma : MonoBehaviour
{
    // Goodness and badness variables, used to determine if the player is good or evil
    [HideInInspector] public int karma;

    public static Karma Instance;

    [SerializeField] GameObject badDecisionExplosion;

    [SerializeField] Light2D globalLight;
    [SerializeField] float intensityMultiplier;
    [SerializeField] Gradient colorChangeGradient;
    [SerializeField] Slider karmaBar;
    public float maximumEvilness;

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

        if(karma < 0)
        {
            globalLight.color = colorChangeGradient.Evaluate(intensityMultiplier / -karma);
            globalLight.intensity = intensityMultiplier / -karma;

            if(globalLight.intensity < 0.3f)
            {
                globalLight.intensity = 0.3f;
            }
        }
        else
        {
            globalLight.color = colorChangeGradient.Evaluate(1);
            globalLight.intensity = 1;
        }

        Debug.Log(karma);

        if(karma < 0)
        {
            karmaBar.value = -karma / maximumEvilness;
        }
        
    }

    // Called when a good decision is made
    public void GoodDecision(int increaseAmount)
    {
        //1 goodness is added every time you make a good decision
        karma += increaseAmount;

        if (karma < 0)
        {
            karmaBar.value = -karma / maximumEvilness;
        }
    }
}
