using UnityEngine;
public class MapBound : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if (GameEvents.OnMapBoundHit != null) GameEvents.OnMapBoundHit();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (GameEvents.OnMapBoundLeave != null) GameEvents.OnMapBoundLeave();
        }
    }
}