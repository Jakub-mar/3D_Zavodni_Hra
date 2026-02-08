using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    public Slider volumeSlider;

    void Start()
    {
        // naèti uloženou hlasitost (nebo 0.5 jako default)
        float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 0.5f);

        AudioListener.volume = savedVolume;
        volumeSlider.value = savedVolume;
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("MasterVolume", volume);
        PlayerPrefs.Save();
    }
}
