using UnityEngine;

public class MotherShip : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 60f;
    [SerializeField] private float rotateSpeed = 100f;
    public Rigidbody rb;
    public Transform childObject;

    private Quaternion targetRotation;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        targetRotation = childObject.rotation;
    }

    private void FixedUpdate()
    {
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(hAxis, 0, vAxis) * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(transform.position + movement);

        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement.normalized, Vector3.up);
            childObject.rotation = Quaternion.RotateTowards(childObject.rotation, targetRotation, rotateSpeed * Time.fixedDeltaTime);
        }
    }
}