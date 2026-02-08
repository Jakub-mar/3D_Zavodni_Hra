using UnityEngine;
public class LapSystem : MonoBehaviour
{
    public int totalCheckpoints;
    public int totalLaps = 5;
    public RaceTimer raceTimer;
    public RaceUi raceUi;
    public RaceFinishManager raceFinishManager;    

    private int currentCheckpoint = 0;
    private int currentLap = 1;
    private bool raceFinished = false;

    private void OnTriggerEnter(Collider other)
    {
        if (raceFinished) return;

        // CHECKPOINT
        if (other.CompareTag("Checkpoint"))
        {
            CheckPoint cp = other.GetComponent<CheckPoint>();

            // Kontrola pořadí checkpointù
            if (cp != null && cp.checkPointIndex == currentCheckpoint)
            {
                // TADY JE ZMÌNA: Pøedáváme èas I index aktuálního checkpointu
                raceUi.ShowCheckpointTime(raceTimer.lapTime, currentCheckpoint);

                currentCheckpoint++;
            }
        }

        // CÍL (FINISH)
        if (other.CompareTag("Finish"))
        {
            if (currentCheckpoint >= totalCheckpoints)
            {
                // Zobrazíme cas v cíli jako poslední checkpoint (index rovný totalCheckpoints)
                raceUi.ShowCheckpointTime(raceTimer.lapTime, totalCheckpoints);

                if (currentLap < totalLaps)
                {
                    currentLap++;
                    currentCheckpoint = 0;
                    raceTimer.FinishLap();
                }
                else
                {
                    raceFinishManager.FinishRace(raceTimer.totalTime);
                }
            }
            else
            {
                Debug.Log("Chybí ti checkpointy! Máš: " + currentCheckpoint + "/" + totalCheckpoints);
            }
        }
    }

    void FinishRace()
    {
        raceFinished = true;
        raceTimer.FinishRace();
        Debug.Log("Závod dokonèen!");
    }

    public int GetLap() => currentLap;
}
