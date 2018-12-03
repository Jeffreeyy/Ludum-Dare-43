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
    [SerializeField] private Animator m_Animator;
    private int m_Score;

    private bool m_CanControlCharacter;
    private float m_ToggleCooldown;

    private void Awake()
    {
        m_Direction = Vector3.left;
        RotateCharacter(false);

        GameEvents.OnGameStart += OnGameStart;
        GameEvents.OnResetGame += OnReset;
        GameEvents.OnMapBoundHit += OnMapBoundHit;

        StartCoroutine(RandomBirdSpawning());
    }

    private void OnDestroy()
    {
        GameEvents.OnGameStart -= OnGameStart;
        GameEvents.OnResetGame -= OnReset;
        GameEvents.OnMapBoundHit -= OnMapBoundHit;

        StopCoroutine(RandomBirdSpawning());
    }

    private void OnReset()
    {
        SetMoving(false);
        m_CanControlCharacter = false;
        m_MovementSpeed = 0;
        transform.position = new Vector3(0, 0, -60f);
        m_CurrentColor = Colors.White;
        SetMaterialColor(Color.white, false);

        m_Direction = Vector3.left;
        transform.localScale = Vector3.one;
        transform.localRotation = Quaternion.Euler(0, GetPlayerRotation(), 0);
    }

    private void OnGameStart()
    {
        SetMoving(true);
        m_CanControlCharacter = true;
        m_MovementSpeed = m_BaseMovementSpeed;
    }

    private void OnMapBoundHit()
    {
        ToggleDirection();
        m_ToggleCooldown = 0.2f;
    }

    private void SetMoving(bool moving)
    {
        m_Animator.SetBool("Moving", moving);
    }

    void Update()
    {
        if(m_CanControlCharacter)
        {
            if (m_ToggleCooldown > 0)
                m_ToggleCooldown -= Time.deltaTime;

            // Toggle direction
            if (Input.GetKeyDown(KeyCode.Space))
                ToggleDirection();

            // Move player
            transform.position += new Vector3(m_Direction.x * m_MovementSpeed * Time.deltaTime, 0, m_MovementSpeed * Time.deltaTime);
        }
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
        float rotation = GetPlayerRotation();
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

    private float GetPlayerRotation()
    {
        return m_Direction == Vector3.left ? -45f : 45f;
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
        if (collidable.BeenHit) return;

        if (m_CurrentColor == collidable.Color) return;

        Colors newColor = m_CurrentColor == Colors.White ? collidable.Color : ColorCombinations.GetCombinedColor(m_CurrentColor, collidable.Color);

        // Only happens if we already have a combination
        if (newColor == Colors.White)
            newColor = collidable.Color;

        Material material = ColorLibrary.Instance.GetMaterial(newColor);
        if (material != null)
            SetMaterialColor(material.color, true);

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
            // Handle game over
            HandleGameOver();
        }
    }

    private void HandleGameOver()
    {
        m_CanControlCharacter = false;
        m_MovementSpeed = 0;
        SetMoving(false);
        SplatCharacterAgainstObjective();
        if (GameEvents.OnGameOver != null) GameEvents.OnGameOver(m_Score);
    }
    
    private void SplatCharacterAgainstObjective()
    {
        transform.DOKill();
        transform.DOLocalRotate(Vector3.zero, 0.1f);
        transform.DOScaleZ(0.2f, 0.1f);
        transform.DOMove(new Vector3(transform.position.x + (m_Direction == Vector3.left ? -0.5f : 0.5f), 0, transform.position.z + 0.7f), 0.1f);
    }

    private void ResetColor()
    {
        m_CurrentColor = Colors.White;
        SetMaterialColor(ColorLibrary.Instance.GetMaterial(m_CurrentColor).color, true);
    }

    private void SetMaterialColor(Color color, bool animate)
    {
        m_Renderer.material.DOKill();

        if (animate)
            m_Renderer.material.DOColor(color, 0.33f).SetEase(Ease.InOutSine);
        else
            m_Renderer.material.color = color;
    }


    private IEnumerator RandomBirdSpawning()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(15f, 30f));
        BirdManager.Instance.SpawnBird(transform.position.z);
        StartCoroutine(RandomBirdSpawning());
    }
}