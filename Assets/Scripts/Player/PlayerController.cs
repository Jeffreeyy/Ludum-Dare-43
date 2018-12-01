using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float m_MovementSpeed = 5f;
    private Vector3 m_Direction;

    private Colors m_CurrentColor = Colors.White;

    private int m_Score;

    private void Start()
    {
        m_Direction = Vector3.left;
    }

    void Update()
    {
        // Toggle direction
        if (Input.GetKeyDown(KeyCode.Space))
            m_Direction = m_Direction == Vector3.left ? Vector3.right : Vector3.left;

        // Move player
        transform.position += new Vector3(m_Direction.x * m_MovementSpeed * Time.deltaTime, 0, m_MovementSpeed * Time.deltaTime);
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
        Colors newColor = m_CurrentColor == Colors.White ? collidable.GetColor() : ColorCombinations.GetCombinedColor(m_CurrentColor, collidable.GetColor());

        Material material = ColorLibrary.Instance.GetMaterial(newColor);
        if (material != null)
            gameObject.GetComponent<Renderer>().material = material;

        m_CurrentColor = newColor;
        collidable.OnHit();
    }

    private void HandleObjectiveHit(ICollidable collidable)
    {
        Colors objectiveColor = collidable.GetColor();

        print(objectiveColor.ToString() + " | " + m_CurrentColor.ToString());

        if (m_CurrentColor == objectiveColor)
            m_Score++;
        else
            m_MovementSpeed = 0;
    }
}
