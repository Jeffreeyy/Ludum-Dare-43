using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float m_BaseMovementSpeed = 5f;
    private float m_MovementSpeed = 0;
    private Vector3 m_Direction;

    private Colors m_CurrentColor = Colors.White;
    [SerializeField] private Renderer m_Renderer;
    private int m_Score;

    private float m_ToggleCooldown;

    private void Awake()
    {
        m_Direction = Vector3.left;
        RotateCharacter(false);

        GameEvents.OnGameStart += OnGameStart;
        GameEvents.OnMapBoundHit += OnMapBoundHit;
    }

    private void OnDestroy()
    {
        GameEvents.OnGameStart -= OnGameStart;
        GameEvents.OnMapBoundHit -= OnMapBoundHit;
    }

    private void OnGameStart()
    {
        m_MovementSpeed = m_BaseMovementSpeed;
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
            RotateCharacter(true);
        }
    }

    private void RotateCharacter(bool animated)
    {
        float rotation = m_Direction == Vector3.left ? -45f : 45f;
        transform.DOKill(false);
        if (animated)
        {
            // Scaling
            transform.DOScaleY(1.3f, 0.1f);
            transform.DOScaleY(1f, 0.1f).SetDelay(0.1f);
            transform.DOScaleY(0.75f, 0.1f).SetDelay(0.2f);
            transform.DOScaleY(1f, 0.1f).SetDelay(0.3f);


            // Jumping
            transform.DOLocalMoveY(0.66f, 0.1f);
            transform.DOLocalMoveY(0, 0.1f).SetDelay(0.1f);
        }

        transform.DOLocalRotate(new Vector3(0f, rotation, 0f), animated ? 0.2f : 0);
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
        if (m_CurrentColor == collidable.Color)
        {
            // Update score
            GameManager.Instance.Score++;

            // Call the on hit
            collidable.OnHit();

            // Reset the player back to white
            ResetColor();
        }
        else
        {
            m_MovementSpeed = 0;
            if(GameEvents.OnGameOver != null) GameEvents.OnGameOver(m_Score);
        }

    }

    private void ResetColor()
    {
        m_CurrentColor = Colors.White;
        m_Renderer.material.color = ColorLibrary.Instance.GetMaterial(m_CurrentColor).color;
    }
}