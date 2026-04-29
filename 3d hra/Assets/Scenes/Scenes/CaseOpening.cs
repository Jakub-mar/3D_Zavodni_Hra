using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CaseOpening : MonoBehaviour
{
    [Header("UI Reference")]
    public RectTransform content;       // Pøiøaï objekt "Content"
    public Button openButton;         // Pøiøaï tlaèítko "Open"

    [Header("Nastavení toèení")]
    public float spinDuration = 8f;    // Délka toèení (8 sekund pro napìtí)
    public float itemWidth = 200f;     // Šíøka tvých obrázkù aut

    private bool isSpinning = false;
    private Vector2 startPosition;

    void Start()
    {
        // Uložíme si poèáteèní pozici (vlevo)
        startPosition = content.anchoredPosition;

        // Pokud zapomeneš pøiøadit tlaèítko v inspektoru, zkusíme ho najít
        if (openButton != null)
            openButton.onClick.AddListener(OpenCase);
    }

    public void OpenCase()
    {
        if (isSpinning) return;

        // "Resetuje" náhodu pokaždé jinak, aby nepadala stejná auta
        Random.InitState(System.Environment.TickCount);

        StartCoroutine(SpinRoutine());
    }

    IEnumerator SpinRoutine()
    {
        isSpinning = true;
        if (openButton != null) openButton.interactable = false;

        // 1. Reset pozice na zaèátek
        content.anchoredPosition = startPosition;

        // 2. Výpoèet mezer a cíle
        float spacing = 0;
        HorizontalLayoutGroup layout = content.GetComponent<HorizontalLayoutGroup>();
        if (layout != null) spacing = layout.spacing;

        float totalStep = itemWidth + spacing;
        int totalItems = content.childCount;

        // VYBÍRÁME VÍTÌZE:
        // Míøíme na náhodné auto mezi 15. a pøedposledním v seznamu
        // (Ujisti se, že máš v Content aspoò 25-30 aut!)
        int winnerIndex = Random.Range(15, totalItems - 2);

        float targetX = winnerIndex * totalStep;

        // Náhodný offset (èára nebude vždycky pøesnì na støed auta)
        float randomOffset = Random.Range(-itemWidth / 2.5f, itemWidth / 2.5f);
        Vector2 targetPos = new Vector2(-(targetX + randomOffset), startPosition.y);

        // 3. TOÈENÍ (Animace)
        float elapsed = 0;
        while (elapsed < spinDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / spinDuration;

            // CS2 Easing (Quintic Ease Out)
            // Zaène velmi rychle a ke konci se línì doplazí k cíli
            t = 1f - Mathf.Pow(1f - t, 5f);

            content.anchoredPosition = Vector2.Lerp(startPosition, targetPos, t);
            yield return null;
        }

        // 4. KONEC
        isSpinning = false;
        if (openButton != null) openButton.interactable = true;

        // Zjistíme, jaké auto je pod èárou
        string winnerName = content.GetChild(winnerIndex).name;
        Debug.Log("<color=gold>GRATULACE!</color> Vyhrál jsi: " + winnerName);
    }
}