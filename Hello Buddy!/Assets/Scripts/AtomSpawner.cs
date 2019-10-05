using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomSpawner : MonoBehaviour
{
    public float millisTillSpawn = 3000;

    private List<string> registeredAtomPrefabs = new List<string>();
    private float accumulatedDelta = 0;

    void Start()
    {
        registeredAtomPrefabs.Add("Prefabs/EnemyAtom");
        registeredAtomPrefabs.Add("Prefabs/FriendlyAtom");
    }

    void Update()
    {
        float timeChangeInMillis = Time.deltaTime * 1000;
        accumulatedDelta += timeChangeInMillis;

        if (accumulatedDelta > millisTillSpawn)
        {
            Debug.Log("Delta:" + accumulatedDelta);
            accumulatedDelta = 0;
            string atomPrefabPath = registeredAtomPrefabs[Random.Range(0, registeredAtomPrefabs.Count)];
            GameObject instantiatedObject = Instantiate(Resources.Load<GameObject>(atomPrefabPath));

            Vector3 spawn = new Vector3(Random.Range(-10.4f, 10.3f), 0, 15.63f);
            instantiatedObject.transform.position = spawn;
        }
    }
}
