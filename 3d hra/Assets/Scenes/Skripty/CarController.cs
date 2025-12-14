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
        // rychlost v m/s a km/h
        float speedMS = rb.linearVelocity.magnitude;
        float speedKmh = speedMS * 3.6f;

        // Motor / zrychlení (umožnit zpětný chod i nad maxSpeed omezit dálkově)
        if (speedKmh < maxSpeed || inputMotor < 0f)
        {
            frontLeftCollider.motorTorque = inputMotor * motorTorque;
            frontRightCollider.motorTorque = inputMotor * motorTorque;
            rearLeftCollider.motorTorque = inputMotor * motorTorque;
            rearRightCollider.motorTorque = inputMotor * motorTorque;
        }
        else
        {
            rearLeftCollider.motorTorque = 0f;
            rearRightCollider.motorTorque = 0f;
        }

        // Řízení - citlivost závislá na rychlosti + vyhlazení
        float speedFactor = Mathf.InverseLerp(0f, 140f, speedKmh);
        // nikdy nespadne pod 55 % úhlu
        float steerLimit = Mathf.Lerp(maxSteerAngle, maxSteerAngle * 0.55f, speedFactor);

        // plynulejší náběh
        float targetSteer = steerLimit * inputSteer;
        currentSteerAngle = Mathf.Lerp(currentSteerAngle, targetSteer, Time.fixedDeltaTime * 6f);

        frontLeftCollider.steerAngle = currentSteerAngle;
        frontRightCollider.steerAngle = currentSteerAngle;

        // Brzda / handbrake
        if (isBraking)
        {
            frontLeftCollider.brakeTorque = brakeTorque;
            frontRightCollider.brakeTorque = brakeTorque;
            rearLeftCollider.brakeTorque = brakeTorque * 0.5f;
            rearRightCollider.brakeTorque = brakeTorque * 0.5f;
        }
        else if (isHandbrake)
        {
            // handbrake obvykle pouze zadní kola
            rearLeftCollider.brakeTorque = handBrakeTorque;
            rearRightCollider.brakeTorque = handBrakeTorque;

            // uvolnit přední brzdy pokud nebyly drženy
            frontLeftCollider.brakeTorque = 0f;
            frontRightCollider.brakeTorque = 0f;
        }
        else
        {
            // žádná brzda - reset
            frontLeftCollider.brakeTorque = 0f;
            frontRightCollider.brakeTorque = 0f;
            rearLeftCollider.brakeTorque = 0f;
            rearRightCollider.brakeTorque = 0f;
        }

        // Downforce pro stabilitu ve vysoké rychlosti (mírně škálováno rychlostí)
        rb.AddForce(-transform.up * downforce * speedMS);
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