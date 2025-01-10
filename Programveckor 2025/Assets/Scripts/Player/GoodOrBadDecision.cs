using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodOrBadDecision : MonoBehaviour
{
    int badness;
    int goodness;

    public static GoodOrBadDecision Instance;

    [SerializeField] GameObject badDecisionExplosion;
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
        badness = 0;
        goodness = 0;
    }

    public void BadDecision(int increaseAmount)
    {
        //1 badness is added every time you make a bad decision
        badness += increaseAmount;
        print("Det var ett dåligt val!");
        
        //You get a warning after 3 bad decision
        if (badness == 3)
        {
            print("Du har gjort flera dåliga val!!");
            Instantiate(badDecisionExplosion);
        }

        //After you make 4 bad decisions, the world explodes
        if (badness == 4)
        {
            print("BOOM!!!!!!!!! DU SPRÄNGDE VÄRLDEN!");
        }
    }
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
