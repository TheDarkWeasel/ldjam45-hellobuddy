using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Docker[] dockers;
    private Animator destroyAnimator;

    private bool isDestroyed = false;

    void Start()
    {
        dockers = FindObjectsOfType<Docker>();
        destroyAnimator = GetComponent<Animator>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isDestroyed)
            return;

        //If the atom we collided with is friendly, we do our stuff
        if (collision.gameObject.GetComponentInChildren<EnemyController>() == null)
        {
            isDestroyed = true;
            foreach (Docker docker in dockers)
            {
                docker.OnHitEnemy();
            }

            destroyAnimator.ResetTrigger("Reset");
            destroyAnimator.SetTrigger("Destroy");
            AtomPool.GetInstance().DestroyEnemyAtom(transform.parent.gameObject, 1);
        }
    }

    public void resetDestroy()
    {
        destroyAnimator.ResetTrigger("Destroy");
        destroyAnimator.SetTrigger("Reset");
        isDestroyed = false;
    }
}
