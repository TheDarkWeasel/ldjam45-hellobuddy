using UnityEngine;

public class NonPlayableAtomMover : MonoBehaviour
{
    public float movementPerSecond = 3f;

    void Update()
    {
        float deltaTime = Time.deltaTime;

        Vector3 movement = new Vector3(0, 0, -movementPerSecond * deltaTime);
        gameObject.transform.Translate(movement);
    }
}
