using UnityEngine;

public class RaceFinishManager : MonoBehaviour
{
    public Rigidbody carRb;
    public RaceTimer raceTimer; //  SEM

    public void FinishRace(float finalTime)
    {
        Debug.Log("Závod dokonèen");

        // TADY SE ULOŽÍ NEJLEPŠÍ ÈAS
        if (raceTimer != null)
        {
            raceTimer.FinishRace();
        }

        if (carRb != null)
        {
            carRb.linearVelocity = Vector3.zero;
            carRb.angularVelocity = Vector3.zero;
            carRb.isKinematic = true;
        }

        Time.timeScale = 0f;
    }
}

    

