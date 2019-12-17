using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 0.75f;
    public float rotationSpeed = 0.1f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        float rotationLeft = (Input.GetKey(KeyCode.Q) ? -1f : 0f) * rotationSpeed;
        float rotationRight = (Input.GetKey(KeyCode.E) ? 1f : 0f) * rotationSpeed;

        Vector3 rotationVector = new Vector3(0, rotationLeft + rotationRight, 0);

        gameObject.transform.Rotate(rotationVector);
    }

    void FixedUpdate()
    {
        float left = (Input.GetKey(KeyCode.A) ? -1f : 0f) * speed;
        float right = (Input.GetKey(KeyCode.D) ? 1f : 0f) * speed;

        Vector3 forceVector = new Vector3(left + right, 0, 0);

        rb.AddForce(forceVector);
    }
}
