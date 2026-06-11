using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TrackHoverInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject infoPanel;

    public TMP_Text trackNameText;
    public TMP_Text lapsText;
    public TMP_Text aiText;
    

    [Header("Track Data")]
    public string trackName;
    public int laps;
    public int aiCars;
    

    public void OnPointerEnter(PointerEventData eventData)
    {
        infoPanel.SetActive(true);

        trackNameText.text = trackName;
        lapsText.text = "Kola: " + laps;
        aiText.text = "AI: " + aiCars;
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        infoPanel.SetActive(false);
    }
}