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

    private List<AtomCreateCommand> registeredAtomCreateCommands = new List<AtomCreateCommand>();
    private float accumulatedDelta = 0;

    private const float zTopOfPlayfield = 17.63f;
    //margin so atoms are not spawned in the wall
    private const float horizontalSafetyMargin = 1.5f;
    private float xLeftOfPlayfield;
    private float xRightOfPlayfield;

    void Start()
    {
        registeredAtomCreateCommands.Add(new EnemyAtomCreateCommand(AtomPool.GetInstance()));
        registeredAtomCreateCommands.Add(new FriendlyAtomCreateCommand(AtomPool.GetInstance()));

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
            AtomCreateCommand atomCreateCommand = registeredAtomCreateCommands[Random.Range(0, registeredAtomCreateCommands.Count)];
            GameObject instantiatedObject = atomCreateCommand.Create();

            Vector3 spawn = new Vector3(Random.Range(xLeftOfPlayfield, xRightOfPlayfield), 0, zTopOfPlayfield);
            instantiatedObject.transform.position = spawn;

            //Downward movement
            instantiatedObject.AddComponent<NonPlayableAtomMover>();
        }
    }
}
