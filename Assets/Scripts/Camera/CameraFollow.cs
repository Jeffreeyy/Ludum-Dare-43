using System;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    private Camera m_Camera;
    public Transform target;

    [SerializeField] private float m_SmoothSpeed = 0.1f;
    [SerializeField] private Vector3 m_Offset;

    private Vector3 m_DefaultPosition;
    private Vector3 m_DefaultOffset;
    private float m_DefaultSmoothSpeed;

    private void Awake()
    {
        m_Camera = GetComponent<Camera>();
        m_DefaultPosition = target.position + m_Offset;
        m_DefaultOffset = m_Offset;
        m_DefaultSmoothSpeed = m_SmoothSpeed;

        GameEvents.OnResetGame += OnResetGame;
        GameEvents.OnGameOver += OnGameOver;
    }

    private void OnDestroy()
    {
        GameEvents.OnResetGame -= OnResetGame;
        GameEvents.OnGameOver -= OnGameOver;
    }

    private void OnGameOver(int score)
    {
        m_Camera.DOOrthoSize(7f, 2f).SetEase(Ease.OutExpo);
        m_Offset.y = 22;
        m_SmoothSpeed = 0.05f;
    }

    private void OnResetGame()
    {
        m_Camera.DOKill();
        m_Camera.orthographicSize = 14;
        transform.position = m_DefaultPosition;
        m_Offset = m_DefaultOffset;
        m_SmoothSpeed = m_DefaultSmoothSpeed;
    }

    private void LateUpdate()
    {

        Vector3 desiredPosition = target.position + m_Offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, m_SmoothSpeed);
        transform.position = smoothedPosition;//new Vector3(smoothedPosition.x, transform.position.y, smoothedPosition.z);
    }
}
