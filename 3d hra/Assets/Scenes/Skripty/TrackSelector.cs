using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TrackSelection : MonoBehaviour
{
    [Header("Track Buttons")]
    public Button[] trackButtons;

    [Header("Start Button")]
    public GameObject startButton;

    private Vector3 normalScale = Vector3.one;
    private Vector3 hoverScale = new Vector3(1.05f, 1.05f, 1.05f);
    private Vector3 selectedScale = new Vector3(1.15f, 1.15f, 1.15f);

    private int selectedIndex = -1;

    void Start()
    {
        startButton.SetActive(false);

        for (int i = 0; i < trackButtons.Length; i++)
        {
            int index = i;

            ResetButton(trackButtons[i]);

            trackButtons[i].onClick.RemoveAllListeners();
            trackButtons[i].onClick.AddListener(() => SelectTrack(index));
        }
    }

    void SelectTrack(int index)
    {
        selectedIndex = index;
        GameManager.Instance.selectedTrack = index;
        startButton.SetActive(true);

        for (int i = 0; i < trackButtons.Length; i++)
        {
            if (i == index)
                HighlightButton(trackButtons[i]);
            else
                ResetButton(trackButtons[i]);
        }
    }

    void HighlightButton(Button btn)
    {
        StopAllCoroutines();
        StartCoroutine(ScaleTo(btn.transform, selectedScale));

        Outline outline = btn.GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = true;
            outline.effectColor = new Color(1f, 0.8f, 0f); // gold
        }

        Image img = btn.GetComponent<Image>();
        if (img != null)
        {
            img.color = Color.white;
        }
    }

    void ResetButton(Button btn)
    {
        StopAllCoroutines();
        StartCoroutine(ScaleTo(btn.transform, normalScale));

        Outline outline = btn.GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = false;
        }

        Image img = btn.GetComponent<Image>();
        if (img != null)
        {
            img.color = new Color(0.9f, 0.9f, 0.9f);
        }
    }
    public void OnHoverEnter(Button btn)
    {
        if (selectedIndex == -1 || btn != trackButtons[selectedIndex])
        {
            StartCoroutine(ScaleTo(btn.transform, hoverScale));
        }
    }

    public void OnHoverExit(Button btn)
    {
        if (selectedIndex == -1 || btn != trackButtons[selectedIndex])
        {
            StartCoroutine(ScaleTo(btn.transform, normalScale));
        }
    }
    IEnumerator ScaleTo(Transform target, Vector3 targetScale)
    {
        float time = 0f;
        Vector3 startScale = target.localScale;

        while (time < 0.2f)
        {
            target.localScale = Vector3.Lerp(startScale, targetScale, time / 0.2f);
            time += Time.deltaTime;
            yield return null;
        }

        target.localScale = targetScale;
    }

    public void StartGame()
    {
        GameManager.Instance.GoToGarage();
    }
}