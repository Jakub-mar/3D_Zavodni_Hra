using UnityEngine;

public class LapSystem : MonoBehaviour
{
    public int totalCheckpoints;
    public int totalLaps = 1;

    public RaceFinishManager raceFinishManager;

    public string racerName = "Car";
    public bool isPlayer = false;

    private int currentCheckpoint = 0;
    private int currentLap = 1;
    private bool raceFinished = false;

    private float timer = 0f;

    void Start()
    {
        raceFinishManager = FindFirstObjectByType<RaceFinishManager>();
        raceFinishManager?.RegisterRacer(this);
    }

    void Update()
    {
        if (!raceFinished)
            timer += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (raceFinished) return;

        if (other.CompareTag("Checkpoint"))
        {
            CheckPoint cp = other.GetComponent<CheckPoint>();

            if (cp != null && cp.checkPointIndex == currentCheckpoint)
            {
                currentCheckpoint++;
            }
        }

        if (other.CompareTag("Finish"))
        {
            if (currentCheckpoint >= totalCheckpoints)
            {
                if (currentLap < totalLaps)
                {
                    currentLap++;
                    currentCheckpoint = 0;
                }
                else
                {
                    FinishRace();
                }
            }
        }
    }

    void FinishRace()
    {
        if (raceFinished) return;

        raceFinished = true;

        Debug.Log("FINISH: " + racerName + " time: " + timer);

        raceFinishManager.FinishRace(timer, this);
    }
    public int GetLap() => currentLap;

    public int GetCheckpoint()
    {
        return currentCheckpoint;
    }

    public int GetCurrentLap()
    {
        return currentLap;
    }
}