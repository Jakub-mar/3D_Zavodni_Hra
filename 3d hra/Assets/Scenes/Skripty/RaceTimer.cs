using UnityEngine;

public class RaceTimer : MonoBehaviour
{
    public float lapTime;
    public float totalTime;
    public float lastCheckpointTime;
    public float bestTime;

    private bool running = true;

    void Start()
    {
        // naètení nejlepšího èasu pøi startu závodu
        bestTime = PlayerPrefs.GetFloat("BestTime", 9999f);
    }

    void Update()
    {
        if (!running) return;

        lapTime += Time.deltaTime;
        totalTime += Time.deltaTime;
    }

    public void SaveCheckpointTime()
    {
        lastCheckpointTime = lapTime;
    }

    public void FinishLap()
    {
        lapTime = 0f;
    }

    //TADY SE ULOŽÍ NEJLEPŠÍ ÈAS
    public void FinishRace()
    {
        running = false;

        if (totalTime < bestTime)
        {
            bestTime = totalTime;
            PlayerPrefs.SetFloat("BestTime", bestTime);
            PlayerPrefs.Save(); //dùležité
        }
    }
}
