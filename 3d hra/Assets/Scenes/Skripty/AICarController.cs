using UnityEngine;

public class AICarController : MonoBehaviour
{
    public Transform[] nodes;
    public int currentNode = 0;

    [Header("Physics")]
    public WheelCollider FL;
    public WheelCollider FR;
    public WheelCollider RL;
    public WheelCollider RR;

    public float maxTorque = 2500f;
    public float maxSteer = 30f;
    public float brakeTorque = 3500f;
    public float downforce = 200f;

    [Header("Speed")]
    public float maxSpeed = 200f;   // MAX 200 km/h
    public float slowSpeed = 60f;   // minimální rychlost v zatáčce

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.7f, 0.1f);
    }

    void FixedUpdate()
    {
        if (nodes.Length == 0) return;

        ApplySteer();
        Drive();
        CheckDistance();

        rb.AddForce(-transform.up * downforce * rb.linearVelocity.magnitude);
    }

    void ApplySteer()
    {
        Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position);

        float speed = rb.linearVelocity.magnitude * 3.6f;

        float steerLimit = Mathf.Lerp(maxSteer, 8f, speed / maxSpeed);

        float steer = (relativeVector.x / relativeVector.magnitude) * steerLimit;

        FL.steerAngle = steer;
        FR.steerAngle = steer;
    }

    void Drive()
    {
        float speed = rb.linearVelocity.magnitude * 3.6f;

        Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position);

        // 0 = rovně, 1 = ostrá zatáčka
        float turnFactor = Mathf.Clamp01(Mathf.Abs(relativeVector.x) / 10f);

        float targetSpeed = Mathf.Lerp(maxSpeed, slowSpeed, turnFactor);

        // PLYNULÉ ZRYCHLOVÁNÍ / BRZDĚNÍ
        float speedError = targetSpeed - speed;

        if (speedError > 5f)
        {
            float torque = Mathf.Clamp(speedError * 50f, 0, maxTorque);

            RL.motorTorque = torque;
            RR.motorTorque = torque;

            RL.brakeTorque = 0;
            RR.brakeTorque = 0;
        }
        else
        {
            RL.motorTorque = 0;
            RR.motorTorque = 0;

            RL.brakeTorque = brakeTorque * Mathf.Clamp01(-speedError / 20f);
            RR.brakeTorque = brakeTorque * Mathf.Clamp01(-speedError / 20f);
        }
    }

    void CheckDistance()
    {
        if (Vector3.Distance(transform.position, nodes[currentNode].position) < 6f)
        {
            currentNode++;
            if (currentNode >= nodes.Length) currentNode = 0;
        }
    }
}