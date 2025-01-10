using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experience : MonoBehaviour
{
    int xp;

    // Start is called before the first frame update
    void Start()
    {
        xp = 0;
    }

   public void GainExperience(int minimumAmount, int maximumAmount)
    {
        xp += Random.Range(minimumAmount, maximumAmount);
    }
}
