using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomSpawner : MonoBehaviour
{
    [SerializeField]
    private float millisTillSpawn = 3000;

    [SerializeField]
    private GameObject leftWall;
    [SerializeField]
    private GameObject rightWall;

    private List<string> registeredAtomPrefabs = new List<string>();
    private float accumulatedDelta = 0;

    private const float zTopOfPlayfield = 17.63f;
    //margin so atoms are not spawned in the wall
    private const float horizontalSafetyMargin = 1.5f;
    private float xLeftOfPlayfield = -10.4f;
    private float xRightOfPlayfield = 10.3f;

    void Start()
    {
        registeredAtomPrefabs.Add("Prefabs/EnemyAtom");
        registeredAtomPrefabs.Add("Prefabs/FriendlyAtom");

        Collider leftCollider = leftWall.GetComponent<Collider>();
        Collider rightCollider = rightWall.GetComponent<Collider>();
        xLeftOfPlayfield = leftWall.transform.position.x + leftCollider.bounds.extents.x + horizontalSafetyMargin;
        xRightOfPlayfield = rightWall.transform.position.x - rightCollider.bounds.extents.x - horizontalSafetyMargin;
    }

    void Update()
    {
        float timeChangeInMillis = Time.deltaTime * 1000;
        accumulatedDelta += timeChangeInMillis;

        if (accumulatedDelta > millisTillSpawn)
        {
            accumulatedDelta = 0;
            string atomPrefabPath = registeredAtomPrefabs[Random.Range(0, registeredAtomPrefabs.Count)];
            GameObject instantiatedObject = Instantiate(Resources.Load<GameObject>(atomPrefabPath));

            Vector3 spawn = new Vector3(Random.Range(xLeftOfPlayfield, xRightOfPlayfield), 0, zTopOfPlayfield);
            instantiatedObject.transform.position = spawn;

            //Downward movement
            instantiatedObject.AddComponent<NonPlayableAtomMover>();
        }
    }
}
