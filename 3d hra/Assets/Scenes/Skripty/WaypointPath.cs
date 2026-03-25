using UnityEngine;

public class WaypointPath : MonoBehaviour
{
    public Transform[] waypoints;

    void OnDrawGizmos()
    {
        // Tohle vykreslí èáru trasy v editoru, abys ji vidìl
        if (waypoints == null || waypoints.Length < 2) return;
        for (int i = 0; i < waypoints.Length; i++)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(waypoints[i].position, 0.5f);
            if (i < waypoints.Length - 1)
                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
            else
                Gizmos.DrawLine(waypoints[i].position, waypoints[0].position);
        }
    }
}
