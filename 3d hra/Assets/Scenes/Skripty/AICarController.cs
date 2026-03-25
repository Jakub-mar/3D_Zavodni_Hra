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

    public float maxTorque = 1500f;
    public float maxSteer = 30f;
    public float brakeTorque = 3000f;
    public float downforce = 100f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Fix těžiště - tohle zabrání převracení
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
        // Převedeme pozici waypointu na lokální prostor auta
        Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position);

        // Výpočet úhlu: x / magnitude nám dá hodnotu od -1 do 1
        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteer;

        FL.steerAngle = newSteer;
        FR.steerAngle = newSteer;
    }

    void Drive()
    {
        float currentSpeed = rb.linearVelocity.magnitude * 3.6f;

        // Pokud je waypoint víceméně před námi, jeď
        if (currentSpeed < 100f)
        {
            RL.motorTorque = maxTorque;
            RR.motorTorque = maxTorque;
            RL.brakeTorque = 0;
            RR.brakeTorque = 0;
        }
        else // Omezovač rychlosti
        {
            RL.motorTorque = 0;
            RR.motorTorque = 0;
            RL.brakeTorque = 500;
            RR.brakeTorque = 500;
        }
    }

    void CheckDistance()
    {
        // Pokud jsme blízko waypointu, přepni na další
        if (Vector3.Distance(transform.position, nodes[currentNode].position) < 6f)
        {
            currentNode++;
            if (currentNode >= nodes.Length) currentNode = 0;
        }
    }
}