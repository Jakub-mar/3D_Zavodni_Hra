using UnityEngine;

public class RaceFinishManager : MonoBehaviour
{
    public Rigidbody carRb;

    public void FinishRace(float finalTime)
    {
        Debug.Log("Závod dokonèen");

        if (carRb != null)
        {
            carRb.linearVelocity = Vector3.zero;   // když máš Unity 6 a používáš linearVelocity
            carRb.angularVelocity = Vector3.zero;
            carRb.isKinematic = true;              // zmrazí auto úplnì
        }

        Time.timeScale = 0f;
    }
}

    

