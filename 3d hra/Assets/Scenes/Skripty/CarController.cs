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
    public float motorTorque = 4000f;
    public float maxSteerAngle = 30f;
    public float brakeTorque = 5000f;
    public float handBrakeTorque = 8000f;
    public float maxSpeed = 200f; // km/h
    public float downforce = 50f;
    
    [Header("Steering")]
    [Tooltip("Čas pro vyhlazení natočení kol (menší = ostřejší reakce)")]
    public float steerSmoothTime = 0.08f;
    [Tooltip("Kolik procent maximálního úhlu zůstane při vysoké rychlosti (0..1)")]
    [Range(0.1f, 1f)]
    public float highSpeedSteerFactor = 0.35f;

    private Rigidbody rb;

    // interní řízení
    float inputMotor;
    float inputSteer;
    bool isBraking;
    bool isHandbrake;

    // pro smooth steering
    private float currentSteerAngle;
    private float steerVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearDamping = 0.05f;        // odpor vzduchu
        rb.angularDamping = 2.5f;  // stabilita při zatáčení
        rb.centerOfMass = new Vector3(0f, -0.6f, 0f); // níže těžiště pro stabilitu
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

    void HandleInput()
    {
        inputMotor = Input.GetAxis("Vertical");   // W/S nebo šipky
        inputSteer = Input.GetAxis("Horizontal"); // A/D nebo šipky
        isBraking = Input.GetKey(KeyCode.Space);
        isHandbrake = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.C);
    }

    void ApplyPhysics()
    {
        float speedMS = rb.linearVelocity.magnitude;
        float speedKmh = speedMS * 3.6f;

        //výkon klesá s rychlostí (simulace převodů)
        float speedRatio = Mathf.Clamp01(speedKmh / maxSpeed);
        float enginePower = motorTorque * (1f - speedRatio);

        // minimální tah, aby auto neumřelo
        enginePower = Mathf.Max(enginePower, motorTorque * 0.25f);

        //RWD – pohon jen zadních kol
        if (speedKmh < maxSpeed || inputMotor < 0f)
        {
            rearLeftCollider.motorTorque = inputMotor * enginePower;
            rearRightCollider.motorTorque = inputMotor * enginePower;
        }
        else
        {
            rearLeftCollider.motorTorque = 0f;
            rearRightCollider.motorTorque = 0f;
        }

        // předek netlačí
        frontLeftCollider.motorTorque = 0f;
        frontRightCollider.motorTorque = 0f;

        //řízení (necháváme skoro stejné)
        float speedFactor = Mathf.InverseLerp(0f, 140f, speedKmh);
        float steerLimit = Mathf.Lerp(maxSteerAngle, maxSteerAngle * highSpeedSteerFactor, speedFactor);

        float targetSteer = steerLimit * inputSteer;
        currentSteerAngle = Mathf.Lerp(currentSteerAngle, targetSteer, Time.fixedDeltaTime * 6f);

        frontLeftCollider.steerAngle = currentSteerAngle;
        frontRightCollider.steerAngle = currentSteerAngle;

        //brzdy
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
            frontLeftCollider.brakeTorque = 0f;
            frontRightCollider.brakeTorque = 0f;
            rearLeftCollider.brakeTorque = 0f;
            rearRightCollider.brakeTorque = 0f;
        }

        // downforce až ve vyšší rychlosti
        rb.AddForce(-transform.up * downforce * speedMS * 0.5f);
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