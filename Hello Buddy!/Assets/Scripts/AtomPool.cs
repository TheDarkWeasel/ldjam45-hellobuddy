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
        enemyAtomPool = new ObjectPool<GameObject>(() => InternalCreateEnemyAtom(), (i) => InternalActivateEnemyAtom(i), (i) => InternalDeactivateEnemyAtom(i));
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
        enemyAtom.GetComponentInChildren<EnemyController>().resetDestroy();
        enemyAtom.SetActive(false);
    }

    public GameObject CreateFriendlyAtom()
    {
        GameObject atom = CreateAtom("Prefabs/FriendlyAtom");
        //Downward movement
        atom.AddComponent<NonPlayableAtomMover>();
        return atom;
    }

    public GameObject CreateEnemyAtom()
    {
        return enemyAtomPool.GetObject();
    }

    public void DestroyFriendlyAtom(GameObject friendlyAtom, int secondsTillDestruction)
    {
        Object.Destroy(friendlyAtom, secondsTillDestruction);
    }

    public void DestroyEnemyAtom(GameObject enemyAtom, int secondsTillDestruction)
    {
        //Destroy collider, so we don't lose when this object hits an enemy
        SetChildCollidersEnabled(enemyAtom, false);
        enemyAtom.GetComponentInChildren<EnemyController>().StartCoroutine(ReturnToPoolAfterXSeconds(secondsTillDestruction, enemyAtom));
    }

    private GameObject CreateAtom(string atomPrefabPath)
    {
        GameObject instantiatedObject = Object.Instantiate(Resources.Load<GameObject>(atomPrefabPath));
        return instantiatedObject;
    }

    private IEnumerator ReturnToPoolAfterXSeconds(float time, GameObject gameObject)
    {
        yield return new WaitForSeconds(time);
        enemyAtomPool.PutObject(gameObject);
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
