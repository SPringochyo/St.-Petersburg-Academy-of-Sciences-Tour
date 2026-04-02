using UnityEngine;

public class MuseumController : MonoBehaviour
{
    [Header("References")]
    public Transform cameraTransform;
    public Transform jumpMarker;
    public float markerHeight = 0.01f;

    [Header("Rotation")]
    public float sensitivity = 2f;
    private float verticalRotation = 0f;

    [Header("Movement")]
    public float moveSpeed = 3f;
    public float maxClickDistance = 5f;
    private Vector3 targetPosition;
    private bool isMoving = false;

    private CharacterController controller;

    private float yaw;   
    private float pitch;
    private float roll;
    public float rotationSmoothTime = 0.1f;
    private float yawVelocity, pitchVelocity, rollVelocity;


    void Start()
    {
        controller = GetComponent<CharacterController>();
        targetPosition = transform.position;

        if (jumpMarker)
        {
            Vector3 markerPos = targetPosition;
            markerPos.y = markerHeight;
            jumpMarker.position = markerPos;
        }

        Vector3 angles = cameraTransform.localEulerAngles;
        pitch = angles.x;
        yaw = transform.eulerAngles.y;
        roll = angles.z;
    }

    void Update()
    {
        HandleRotation();
        HandleMovementInput();
        MoveToTarget();
        UpdateJumpMarker();
    }

    void HandleRotation()
    {
        if (Input.GetMouseButton(0))
        {
            yaw += Input.GetAxis("Mouse X") * sensitivity;
            pitch -= Input.GetAxis("Mouse Y") * sensitivity;
            pitch = Mathf.Clamp(pitch, -80f, 80f);
        }

        float smoothYaw = Mathf.SmoothDampAngle(transform.eulerAngles.y, yaw, ref yawVelocity, rotationSmoothTime);
        float smoothPitch = Mathf.SmoothDampAngle(cameraTransform.localEulerAngles.x, pitch, ref pitchVelocity, rotationSmoothTime);
        float smoothRoll = Mathf.SmoothDampAngle(cameraTransform.localEulerAngles.z, roll, ref rollVelocity, rotationSmoothTime);

    
        transform.rotation = Quaternion.Euler(0f, smoothYaw, 0f); 
        cameraTransform.localRotation = Quaternion.Euler(smoothPitch, 0f, smoothRoll); 
    }

    void HandleMovementInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Floor"))
                {
                    Vector3 rawTarget = hit.point;
                    Vector3 flatTarget = new Vector3(rawTarget.x, transform.position.y, rawTarget.z);
                    
                    // Clamp distance
                    Vector3 direction = flatTarget - transform.position;
                    if (direction.magnitude > maxClickDistance)
                    {
                        flatTarget = transform.position + direction.normalized * maxClickDistance;
                    }

                    targetPosition = flatTarget;
                    isMoving = true;
                }
            }
        }
    }

    void MoveToTarget()
    {
        if (!isMoving) return;

        Vector3 move = targetPosition - transform.position;
        move.y = 0;

        if (move.magnitude < 0.5f)
        {
            isMoving = false;
            return;
        }

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, transform.position + move, moveSpeed * Time.deltaTime);
        controller.Move(smoothedPosition - transform.position);
    }

     void UpdateJumpMarker()
    {
        if (!jumpMarker) return;

        if (Input.GetMouseButton(0))
        {
            jumpMarker.gameObject.SetActive(false);
            return;
        }
        jumpMarker.gameObject.SetActive(true);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit) && !isMoving)
        {
            if (hit.collider.CompareTag("Floor"))
            {
                Vector3 rawTarget = hit.point;
                Vector3 direction = rawTarget - transform.position;
                direction.y = 0;

                if (direction.magnitude > maxClickDistance)
                    direction = direction.normalized * maxClickDistance;

                Vector3 nextJumpPos = transform.position + direction;
                nextJumpPos.y = markerHeight;

                jumpMarker.position = Vector3.Lerp(jumpMarker.position, nextJumpPos, Time.deltaTime * 10f);
            }
        }
    }
}