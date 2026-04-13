using UnityEngine;
public class LapSystem : MonoBehaviour
{
    public int totalCheckpoints;
    public int totalLaps = 1;
    public RaceTimer raceTimer;
    public RaceUi raceUi;
    public RaceFinishManager raceFinishManager;    
    private int currentCheckpoint = 0;
    private int currentLap = 1;
    private bool raceFinished = false;
    public string racerName = "Auto 1";
    public bool isPlayer = false;

    private void OnTriggerEnter(Collider other)
    {
        if (raceFinished) return;

        // --- CHECKPOINT ---
        if (other.CompareTag("Checkpoint"))
        {
            CheckPoint cp = other.GetComponent<CheckPoint>();
            if (cp != null && cp.checkPointIndex == currentCheckpoint)
            {
                raceUi.ShowCheckpointTime(raceTimer.lapTime, currentCheckpoint);
                currentCheckpoint++;
            }
        }

        // --- CÍL (FINISH) ---
        if (other.CompareTag("Finish"))
        {
            if (currentCheckpoint >= totalCheckpoints)
            {
                raceUi.ShowCheckpointTime(raceTimer.lapTime, totalCheckpoints);

                if (currentLap < totalLaps)
                {
                    currentLap++;
                    currentCheckpoint = 0;
                    raceTimer.FinishLap();
                }
                else
                {
                    // KONEC ZÁVODU
                    Debug.Log("ZÁVOD DOKONČEN!");
                    FinishRace(); // Zavolá tvoji metodu pro stopnutí času

                    if (raceFinishManager != null)
                    {
                        Debug.Log("Volám tabulku pro: " + racerName);
                        // Tady posíláme jméno, které máš v proměnné racerName
                        raceFinishManager.FinishRace(raceTimer.totalTime, this);
                    }
                    else
                    {
                        // POKUD SE TI VYPÍŠE TOHLE, NEMÁŠ PŘIŘAZENÝ MANAGER V INSPECTORU!
                        Debug.LogError("CHYBA: V LapSystemu ti chybí přetažený RaceFinishManager!");
                    }
                }
            }
            else if (currentCheckpoint > 0)
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
