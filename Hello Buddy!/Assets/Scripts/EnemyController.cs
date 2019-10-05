using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Docker player;

    void Start()
    {
        player = FindObjectOfType<Docker>();    
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponentInChildren<EnemyController>() == null)
        {
            Destroy(gameObject);
            player.OnHitEnemy();
        }
    }
}
