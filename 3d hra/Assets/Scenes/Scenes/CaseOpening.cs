using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CaseOpening : MonoBehaviour
{
    
    [Header("UI Reference")]
    public RectTransform content;
    public Button openButton;

    [Header("Nastavení")]
    public float spinDuration = 8f;
    public float itemWidth = 160f;
    public int sequenceLength = 60;

    private bool isSpinning = false;
    private List<GameObject> carPool = new List<GameObject>();

    void Start()
    {
        
        // Uložíme vzory aut a schováme je
        foreach (Transform child in content)
        {
            carPool.Add(child.gameObject);
            child.gameObject.SetActive(false);
        }

        if (openButton != null)
            openButton.onClick.AddListener(OpenCase);
    }

    public void OpenCase()
    {
        if (isSpinning) return;

        // Kontrola bodù (pøedpokládám tvùj PlayerProfile skript)
        if (PlayerProfile.instance != null && !PlayerProfile.instance.SpendPoints(15))
            return;

        StartCoroutine(SpinRoutine());
    }

    IEnumerator SpinRoutine()
    {
   

        isSpinning = true;
        if (openButton != null) openButton.interactable = false;

        // Vyèištìní starých klonù
        foreach (Transform child in content)
        {
            if (!carPool.Contains(child.gameObject)) Destroy(child.gameObject);
        }

        yield return new WaitForEndOfFrame();
        content.anchoredPosition = Vector2.zero;

        // Generování øady
        List<GameObject> activeSequence = new List<GameObject>();
        for (int i = 0; i < sequenceLength; i++)
        {
            GameObject randomPrefab = carPool[Random.Range(0, carPool.Count)];
            GameObject newIcon = Instantiate(randomPrefab, content);
            newIcon.SetActive(true);

            RectTransform rt = newIcon.GetComponent<RectTransform>();
            rt.localScale = Vector3.one;
            rt.sizeDelta = new Vector2(itemWidth, itemWidth);
            rt.anchoredPosition = new Vector2(i * (itemWidth + 10f), 0);
            activeSequence.Add(newIcon);
        }
        float spacing = 10f;
        float totalStep = itemWidth + spacing;

        int winnerIndex = sequenceLength - 5;
        float targetX = winnerIndex * totalStep;
        float randomOffset = Random.Range(-itemWidth / 3f, itemWidth / 3f);
        Vector2 targetPos = new Vector2(-(targetX + randomOffset), 0);

        // Toèení
        float elapsed = 0;
        while (elapsed < spinDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / spinDuration;
            t = 1f - Mathf.Pow(1f - t, 5f);
            content.anchoredPosition = Vector2.Lerp(Vector2.zero, targetPos, t);
            yield return null;
        }

        // --- ULOŽENÍ VÝHRY ---
        string winnerName = activeSequence[winnerIndex].name.Replace("(Clone)", "");
        SaveWin(winnerName);

        isSpinning = false;
        if (openButton != null) openButton.interactable = true;
        Debug.Log("<color=gold>VYHRÁL JSI:</color> " + winnerName);

       
    }

    void SaveWin(string carName)
    {
        string currentCars = PlayerPrefs.GetString("OwnedCars", "");
        if (!currentCars.Contains(carName))
        {
            string newData = string.IsNullOrEmpty(currentCars) ? carName : currentCars + "," + carName;
            PlayerPrefs.SetString("OwnedCars", newData);
            PlayerPrefs.Save();
        }
    }
}