using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 1;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        float rotationLeft = (Input.GetKey("q") ? -1f : 0f) * speed;
        float rotationRight = (Input.GetKey("e") ? 1f : 0f) * speed;

        Vector3 rotationVector = new Vector3(0, rotationLeft + rotationRight, 0);

        gameObject.transform.Rotate(rotationVector);
    }

    void FixedUpdate()
    {
        float left = (Input.GetKey("a") ? -1f : 0f) * speed;
        float right = (Input.GetKey("d") ? 1f : 0f) * speed;
        //float forward = (Input.GetKey("w") ? 1f : 0f) * speed;
        //float backward = (Input.GetKey("s") ? -1f : 0f) * speed;

        Vector3 forceVector = new Vector3(left + right, 0, 0);

        rb.AddForce(forceVector);
    }
}
