using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadOrGoodDecision : MonoBehaviour
{
    int badness;
    int goodness;

    public void BadDecision()
    {
        badness += 1;
        print("Det var ett d�ligt val!");
        
        if (badness >= 4)
        {
            print("Du har gjort flera d�liga val!!");
        }
    }
    public void GoodDecision()
    {
        goodness += 1;
        print("Det vat ett bra val!");

        if (goodness >= 4)
        {
            print("Du har gjort flera bra val!!");
        }
    }
}
