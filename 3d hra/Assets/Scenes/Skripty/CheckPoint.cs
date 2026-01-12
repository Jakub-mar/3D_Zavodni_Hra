using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public int checkPointIndex;
    private void Awake()
    {
        // Nastavíme, aby se checkpoint v editoru tváøil jako trigger
        if (GetComponent<Collider>() != null)
        {
            GetComponent<Collider>().isTrigger = true;
        }
    }
}
