using UnityEngine;

public class NonPlayableAtomMover : MonoBehaviour
{
    [SerializeField]
    private float movementPerSecond = 3f;

    void Update()
    {
        float deltaTime = Time.deltaTime;

        Vector3 movement = new Vector3(0, 0, -movementPerSecond * deltaTime);
        gameObject.transform.Translate(movement);

        //Destroy unneeded object
        if(gameObject.transform.position.z < -19)
        {
            Destroy(gameObject);
        }
    }
}
