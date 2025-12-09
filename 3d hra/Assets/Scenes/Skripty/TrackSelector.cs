using UnityEngine;
using UnityEngine.SceneManagement;

public class TrackSelector : MonoBehaviour
{
    public void LoadTrack1()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void LoadTrack2()
    {
        SceneManager.LoadScene("Track2");
    }

    public void LoadTrack3()
    {
        SceneManager.LoadScene("Track3");
    }
}
