using TMPro;
using UnityEngine;
using System.Collections;

public class RaceUi : MonoBehaviour
{
    public LapSystem lapSystem;
    public RaceTimer raceTimer;

    [Header("Textová pole")]
    public TextMeshProUGUI lapCountText;      // Text pro "LAP 1/5"
    public TextMeshProUGUI lapTimeText;       // Text pro aktuální èas kola
    public TextMeshProUGUI totalTimeText;     // Text pro celkový èas závodu
    public TextMeshProUGUI checkpointText;    // Text pro CP (zùstane svítit)

    void Start()
    {
        // Poèáteèní stav
        checkpointText.text = "--:--.---";
    }

    void Update()
    {
        // 1. Aktualizace kol (vlevo nahoøe)
        lapCountText.text = "LAP " + lapSystem.GetLap() + "/" + lapSystem.totalLaps;

        // 2. Aktuální èas kola (bìžící stopky)
        lapTimeText.text = FormatTime(raceTimer.lapTime);

        // 3. Celkový èas závodu (stále roste)
        totalTimeText.text = /*"TOTAL " +*/ FormatTime(raceTimer.totalTime);
    }

    // Tuto funkci volá LapSystem pøi prùjezdu CP
    public void ShowCheckpointTime(float currentTime, int index)
    {
        // Pøepíše text posledním èasem
        checkpointText.text = FormatTime(currentTime);
    }

    // Pomocná funkce pro formátování èasu na 0:00.000
    string FormatTime(float time)
    {
        int minutes = (int)time / 60;
        int seconds = (int)time % 60;
        int fraction = (int)((time * 1000) % 1000);
        return string.Format("{0}:{1:00}.{2:000}", minutes, seconds, fraction);
    }
}
