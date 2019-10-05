using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 1;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float left = (Input.GetKey("a") ? -1f : 0f) * speed;
        float right = (Input.GetKey("d") ? 1f : 0f) * speed;
        float forward = (Input.GetKey("w") ? 1f : 0f) * speed;
        float backward = (Input.GetKey("s") ? -1f : 0f) * speed;

        Vector3 forceVector = new Vector3(left + right, 0, forward + backward);

        rb.AddForce(forceVector);
    }
}
