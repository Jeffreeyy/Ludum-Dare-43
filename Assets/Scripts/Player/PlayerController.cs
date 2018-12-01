using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float m_MovementSpeed = 5f;
    private Vector3 m_Direction;

    private Colors m_CurrentColor = Colors.White;
    [SerializeField] private Renderer m_Renderer;
    private int m_Score;

    private float m_ToggleCooldown;

    private void Awake()
    {
        m_Direction = Vector3.left;
        RotateCharacter();

        GameEvents.OnMapBoundHit += OnMapBoundHit;
    }

    private void OnDestroy()
    {
        GameEvents.OnMapBoundHit -= OnMapBoundHit;
    }

    private void OnMapBoundHit()
    {
        ToggleDirection();
        m_ToggleCooldown = 0.2f;
    }

    void Update()
    {
        if (m_ToggleCooldown > 0)
            m_ToggleCooldown -= Time.deltaTime;

        // Toggle direction
        if (Input.GetKeyDown(KeyCode.Space))
            ToggleDirection();

        // Move player
        transform.position += new Vector3(m_Direction.x * m_MovementSpeed * Time.deltaTime, 0, m_MovementSpeed * Time.deltaTime);
    }

    private void ToggleDirection()
    {
        if(m_ToggleCooldown <= 0)
        {
            m_Direction = m_Direction == Vector3.left ? Vector3.right : Vector3.left;
            RotateCharacter();
        }
    }

    private void RotateCharacter()
    {
        float rotation = m_Direction == Vector3.left ? -45f : 45f;
        transform.DOKill(false);
        transform.DOLocalMoveY(0.5f, 0.1f);
        transform.DOLocalMoveY(0, 0.1f).SetDelay(0.1f);
        transform.DOLocalRotate(new Vector3(0f, rotation, 0f), 0.2f);
    }

    private void OnGUI()
    {
        // TEMP!!!
        GUILayout.Label("Score: " + m_Score + " | Color: " + m_CurrentColor.ToString());
    }

    private void OnTriggerEnter(Collider other)
    {
        ICollidable collidable = other.gameObject.GetComponent<ICollidable>();

        if (collidable != null)
        {
            switch (collidable.Type)
            {
                case CollidableType.Pickup:     HandlePickupHit(collidable);    break;
                case CollidableType.Objective:  HandleObjectiveHit(collidable); break;
            }
        }
    }

    private void HandlePickupHit(ICollidable collidable)
    {
        if (m_CurrentColor == collidable.Color) return;

        Colors newColor = m_CurrentColor == Colors.White ? collidable.Color : ColorCombinations.GetCombinedColor(m_CurrentColor, collidable.Color);

        // Only happens if we already have a combination
        if (newColor == Colors.White)
            newColor = collidable.Color;

        Material material = ColorLibrary.Instance.GetMaterial(newColor);
        if (material != null)
            m_Renderer.material.color = material.color;

        m_CurrentColor = newColor;
        collidable.OnHit();
    }

    private void HandleObjectiveHit(ICollidable collidable)
    {
        Colors objectiveColor = collidable.Color;

        print(objectiveColor.ToString() + " | " + m_CurrentColor.ToString());

        if (m_CurrentColor == objectiveColor)
        {
            m_Score++;
            GameEvents.OnScoreUpdated(m_Score);
        }
        else
            m_MovementSpeed = 0;
    }
}