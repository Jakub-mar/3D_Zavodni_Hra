using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    [Header("Wheel Colliders")]
    public WheelCollider frontLeftCollider;
    public WheelCollider frontRightCollider;
    public WheelCollider rearLeftCollider;
    public WheelCollider rearRightCollider;

    [Header("Wheel Meshes (optional, for visual)")]
    public Transform frontLeftMesh;
    public Transform frontRightMesh;
    public Transform rearLeftMesh;
    public Transform rearRightMesh;

    [Header("Settings")]
    public float motorTorque = 3000f;
    public float maxSteerAngle = 30f;
    public float brakeTorque = 5000f;
    public float handBrakeTorque = 8000f;
    public float maxSpeed = 200f; // km/h
    public float downforce = 100f;

    private Rigidbody rb;
    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass += Vector3.down * 0.5f; // nízké těžiště pro stabilitu
        
    }

    void Update()
    {
        HandleInput();
        UpdateWheelMeshes();
    }

    void FixedUpdate()
    {
        ApplyPhysics();
    }

    float inputMotor;
    float inputSteer;
    bool isBraking;
    bool isHandbrake;

    void HandleInput()
    {
        inputMotor = Input.GetAxis("Vertical");   // W/S nebo šipky
        inputSteer = Input.GetAxis("Horizontal"); // A/D nebo šipky
        isBraking = Input.GetKey(KeyCode.Space);
        isHandbrake = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.C);
    }

    void ApplyPhysics()
    {
        // Motor / zrychlení
        if (rb.linearVelocity.magnitude < (maxSpeed / 3.6f)) // převod km/h → m/s
        {
            rearLeftCollider.motorTorque = inputMotor * motorTorque;
            rearRightCollider.motorTorque = inputMotor * motorTorque;
        }
        else
        {
            rearLeftCollider.motorTorque = 0;
            rearRightCollider.motorTorque = 0;
        }

        // Řízení
        float steerAngle = maxSteerAngle * inputSteer;
        frontLeftCollider.steerAngle = steerAngle;
        frontRightCollider.steerAngle = steerAngle;

        // Brzda
        if (isBraking)
        {
            frontLeftCollider.brakeTorque = brakeTorque;
            frontRightCollider.brakeTorque = brakeTorque;
            rearLeftCollider.brakeTorque = brakeTorque * 0.5f;
            rearRightCollider.brakeTorque = brakeTorque * 0.5f;
        }
        else if (isHandbrake)
        {
            rearLeftCollider.brakeTorque = handBrakeTorque;
            rearRightCollider.brakeTorque = handBrakeTorque;
        }
        else
        {
            frontLeftCollider.brakeTorque = 0;
            frontRightCollider.brakeTorque = 0;
            rearLeftCollider.brakeTorque = 0;
            rearRightCollider.brakeTorque = 0;
        }

        // Downforce pro stabilitu ve vysoké rychlosti
        rb.AddForce(-transform.up * downforce * rb.linearVelocity.magnitude);
    }

    void UpdateWheelMeshes()
    {
        UpdateWheel(frontLeftCollider, frontLeftMesh);
        UpdateWheel(frontRightCollider, frontRightMesh);
        UpdateWheel(rearLeftCollider, rearLeftMesh);
        UpdateWheel(rearRightCollider, rearRightMesh);
    }

    void UpdateWheel(WheelCollider col, Transform mesh)
    {
        if (mesh == null) return;
        Vector3 pos;
        Quaternion rot;
        col.GetWorldPose(out pos, out rot);
        mesh.position = pos;
        mesh.rotation = rot;
    }
}