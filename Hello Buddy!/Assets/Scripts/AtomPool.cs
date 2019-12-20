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

    public GameObject CreateFriendlyAtom()
    {
        return CreateAtom("Prefabs/FriendlyAtom");
    }

    public GameObject CreateEnemyAtom()
    {
        return CreateAtom("Prefabs/EnemyAtom");
    }

    public void DestroyFriendlyAtom(GameObject friendlyAtom, int secondsTillDestruction)
    {
        Object.Destroy(friendlyAtom, secondsTillDestruction);
    }

    public void DestroyEnemyAtom(GameObject enemyAtom, int secondsTillDestruction)
    {
        Object.Destroy(enemyAtom, secondsTillDestruction);
    }

    private GameObject CreateAtom(string atomPrefabPath)
    {
        GameObject instantiatedObject = Object.Instantiate(Resources.Load<GameObject>(atomPrefabPath));
        return instantiatedObject;
    }
}
