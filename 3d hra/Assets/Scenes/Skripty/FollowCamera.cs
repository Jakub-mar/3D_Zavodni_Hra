using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [Header("Target (Auto)")]
    public Transform target;

    [Header("Camera Setting")]
    public Vector3 offset = new Vector3(0f, 5f, -10f);
    public float followSpeed = 5f;
    public float rotationSpeed = 5f;

    void LateUpdate()
    {
        if (target == null) return;

        // Cílová pozice kamery (za autem podle offsetu)
        Vector3 desiredPosition = target.position + target.TransformDirection(offset);

        // Plynulý přechod kamery na novou pozici
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        // Směr, kam se kamera má dívat (na auto)
        Quaternion desiredRotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up);

        // Plynulé otáčení kamery
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
    }
}
