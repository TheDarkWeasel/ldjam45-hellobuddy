using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Docker[] dockers;

    void Start()
    {
        dockers = FindObjectsOfType<Docker>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponentInChildren<EnemyController>() == null)
        {
            foreach (Docker docker in dockers)
            {
                docker.OnHitEnemy();
            }
            Destroy(transform.parent.gameObject);
        }
    }
}
