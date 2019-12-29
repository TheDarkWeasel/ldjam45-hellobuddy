using UnityEngine;
using System.Collections;

public class AtomPool
{
    private static AtomPool instance;

    private ObjectPool<GameObject> friendlyAtomPool;
    private ObjectPool<GameObject> enemyAtomPool;

    public static AtomPool GetInstance()
    {
        if (instance == null)
        {
            instance = new AtomPool();
        }

        return instance;
    }

    public static void ResetAtomPool()
    {
        instance = null;
    }

    private AtomPool()
    {
        friendlyAtomPool = new ObjectPool<GameObject>(() => InternalCreateFriendlyAtom(), (i) => InternalActivateFriendlyAtom(i), (i) => InternalDeactivateFriendlyAtom(i));
        enemyAtomPool = new ObjectPool<GameObject>(() => InternalCreateEnemyAtom(), (i) => InternalActivateEnemyAtom(i), (i) => InternalDeactivateEnemyAtom(i));
    }

    private GameObject InternalCreateFriendlyAtom()
    {
        GameObject atom = CreateAtom("Prefabs/FriendlyAtom");
        //Downward movement
        atom.AddComponent<NonPlayableAtomMover>();
        //Script for becoming a docker later
        atom.GetComponent<Docker>().Active = false;
        return atom;
    }

    private void InternalActivateFriendlyAtom(GameObject friendlyAtom)
    {
        friendlyAtom.transform.position = new Vector3(0, 0, 0);
        friendlyAtom.transform.rotation = new Quaternion(0, 0, 0, 0);
        friendlyAtom.transform.localScale = new Vector3(1, 1, 1);

        friendlyAtom.GetComponent<Rigidbody>().velocity = Vector3.zero;
        friendlyAtom.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        SetChildCollidersEnabled(friendlyAtom, true);
        friendlyAtom.GetComponent<NonPlayableAtomMover>().Active = true;
        friendlyAtom.GetComponent<Docker>().Active = false;
        friendlyAtom.SetActive(true);
    }

    private void InternalDeactivateFriendlyAtom(GameObject friendlyAtom)
    {
        //enable collision scripts
        friendlyAtom.GetComponentInChildren<Dockable>().ResetDestroy();
        friendlyAtom.SetActive(false);
    }

    private GameObject InternalCreateEnemyAtom()
    {
        GameObject atom = CreateAtom("Prefabs/EnemyAtom");
        //Downward movement
        atom.AddComponent<NonPlayableAtomMover>();
        return atom;
    }

    private void InternalActivateEnemyAtom(GameObject enemyAtom)
    {
        enemyAtom.transform.position = new Vector3(0, 0, 0);
        enemyAtom.transform.GetChild(0).localScale = new Vector3(1.5f, 1.5f, 1.5f);
        SetChildCollidersEnabled(enemyAtom, true);
        enemyAtom.SetActive(true);
    }

    private void InternalDeactivateEnemyAtom(GameObject enemyAtom)
    {
        //enable collision scripts for next use
        enemyAtom.GetComponentInChildren<EnemyController>().ResetDestroy();
        enemyAtom.SetActive(false);
    }

    public GameObject CreateFriendlyAtom()
    {
        return friendlyAtomPool.GetObject();
    }

    public GameObject CreateEnemyAtom()
    {
        return enemyAtomPool.GetObject();
    }

    public void DestroyFriendlyAtom(GameObject friendlyAtom, int secondsTillDestruction)
    {
        //Destroy collider, so we don't lose when this object hits an enemy
        SetChildCollidersEnabled(friendlyAtom, false);
        friendlyAtom.GetComponentInChildren<Docker>().StartCoroutine(ReturnToPoolAfterXSeconds(secondsTillDestruction, friendlyAtom, friendlyAtomPool));
    }

    public void DestroyEnemyAtom(GameObject enemyAtom, int secondsTillDestruction)
    {
        //Destroy collider, so we don't lose when this object hits an enemy
        SetChildCollidersEnabled(enemyAtom, false);
        enemyAtom.GetComponentInChildren<EnemyController>().StartCoroutine(ReturnToPoolAfterXSeconds(secondsTillDestruction, enemyAtom, enemyAtomPool));
    }

    private GameObject CreateAtom(string atomPrefabPath)
    {
        GameObject instantiatedObject = Object.Instantiate(Resources.Load<GameObject>(atomPrefabPath));
        return instantiatedObject;
    }

    private IEnumerator ReturnToPoolAfterXSeconds(float time, GameObject gameObject, ObjectPool<GameObject> pool)
    {
        yield return new WaitForSeconds(time);
        pool.PutObject(gameObject);
    }

    private void SetChildCollidersEnabled(GameObject gameObject, bool enable)
    {
        if (gameObject.TryGetComponent<Collider>(out Collider foundCollider))
        {
            foundCollider.enabled = enable;
        }
        Collider[] childColliders = gameObject.GetComponentsInChildren<Collider>();
        foreach (Collider collider in childColliders)
        {
            collider.enabled = enable;
        }
    }
}
