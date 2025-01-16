using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // The object that should be tracked and the speed that it should be tracked at
    [SerializeField] Transform trackedObject;
    [SerializeField] float trackingSpeed;

    [SerializeField] Transform middleObject;

    Transform playerTransform;

    private void Start()
    {
        playerTransform = trackedObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(CombatManager.Instance.currentCombatState != CombatManager.CombatState.OutOfCombat)
        {
            float biggestDistance = 0;

            foreach (PlayerUnit playerUnit in CombatManager.Instance.playerCharacters)
            {
                foreach (EnemyUnit enemyUnit in CombatManager.Instance.enemyCharacters)
                {
                    if(Vector2.Distance(playerUnit.transform.position, enemyUnit.transform.position) > biggestDistance)
                    {
                        biggestDistance = Vector2.Distance(playerUnit.transform.position, enemyUnit.transform.position);

                        middleObject.position =
                            new Vector2((playerUnit.transform.position.x + enemyUnit.transform.position.x) / 2,
                            middleObject.position.y);
                    }
                }
            }

            Vector2 velocity = Vector2.zero;

            // Smoothdamp the position of the camera so that if follows the player smoothly
            transform.position = Vector2.SmoothDamp(transform.position, trackedObject.position, ref velocity, trackingSpeed);
            transform.position += new Vector3(0, 0, -10);

            trackedObject = middleObject;
        }
        else
        {

            trackedObject = playerTransform;
            Vector2 velocity = Vector2.zero;

            // Smoothdamp the position of the camera so that if follows the player smoothly
            transform.position = Vector2.SmoothDamp(transform.position, trackedObject.position, ref velocity, trackingSpeed);
            transform.position += new Vector3(0, 0, -10);
        }    
    }
}
