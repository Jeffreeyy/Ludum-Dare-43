using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed = 0.125f;
    [SerializeField] private Vector3 m_Offset;

    private void LateUpdate()
    {

        Vector3 desiredPosition = target.position + m_Offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;//new Vector3(smoothedPosition.x, transform.position.y, smoothedPosition.z);
    }
}
