using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadOrGoodDecision : MonoBehaviour
{
    int badness;
    int goodness;

    void Start()
    {
        badness = 0;
        goodness = 0;
    }

    public void BadDecision()
    {
        badness += 1;
        print("Det var ett d�ligt val!");
        
        if (badness == 4)
        {
            print("Du har gjort flera d�liga val!!");
        }

        if (badness >= 6)
        {
            print("BOOM!!!!!!!!! Du spr�ngde jorden");
        }
    }
    public void GoodDecision()
    {
        goodness += 1;
        print("Det var ett bra val!");

        if (goodness == 4)
        {
            print("Du har gjort flera bra val!!");
        }
    }
}
