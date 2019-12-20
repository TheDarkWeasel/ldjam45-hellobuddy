using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Docker[] dockers;
    private Animator destroyAnimator;

    bool isDestroyed = false;

    void Start()
    {
        dockers = FindObjectsOfType<Docker>();
        destroyAnimator = GetComponent<Animator>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isDestroyed)
            return;

        if (collision.gameObject.GetComponentInChildren<EnemyController>() == null)
        {
            isDestroyed = true;
            foreach (Docker docker in dockers)
            {
                docker.OnHitEnemy();
            }

            destroyAnimator.ResetTrigger("Destroy");
            destroyAnimator.SetTrigger("Destroy");
            //Destroy collider, so we don't lose when this object hits an enemy
            DestroyChildColliders();
            AtomPool.GetInstance().DestroyEnemyAtom(transform.parent.gameObject, 1);
        }
    }

    private void DestroyChildColliders()
    {
        Destroy(gameObject.GetComponent<Collider>());
        Collider[] childColliders = gameObject.GetComponentsInChildren<Collider>();
        foreach (Collider collider in childColliders)
        {
            collider.enabled = false;
        }
    }
}
