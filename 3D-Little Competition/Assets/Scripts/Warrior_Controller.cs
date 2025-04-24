using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Warrior_Controller : MonoBehaviour
{
    public float speed = 0f;
    public float maxSpeed = 5f;
    public float sprintSpeed = 10f;
    public float acceleration = 1f;
    public float deceleration = 2f;
    public Animator animator;

    public Camera_Controller cam; // Kamera referansý

    private float targetSpeed = 0f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        MoveCharacter();
    }

    void MoveCharacter()
    {
        bool isMoving = Input.GetKey(KeyCode.W);

        if (isMoving)
        {
            targetSpeed = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)
                ? sprintSpeed : maxSpeed;
            speed = Mathf.MoveTowards(speed, targetSpeed, acceleration * Time.deltaTime);

            Vector3 camForward = cam.GetCameraRotation() * Vector3.forward;
            camForward.y = 0f;
            if (camForward.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(camForward);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
            }
        }
        else
        {
            speed = Mathf.MoveTowards(speed, 0f, deceleration * Time.deltaTime * 0.5f);
        }
        Vector3 moveDirection = transform.forward * speed * Time.deltaTime;
        rb.MovePosition(rb.position + moveDirection);

        animator.SetFloat("Speed", speed);
    }

}
