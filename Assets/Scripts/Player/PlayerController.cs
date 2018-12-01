using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float m_MovementSpeed = 5f;
    private Vector3 m_Direction;

    private Colors m_CurrentColor = Colors.White;

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

    private void OnTriggerEnter(Collider other)
    {
        ColorPickup pickup = other.gameObject.GetComponent<ColorPickup>();
        if (pickup != null)
        {
            Colors newColor = m_CurrentColor == Colors.White ? pickup.GetColor() : ColorCombinations.GetCombinedColor(m_CurrentColor, pickup.GetColor());

            Material material = ColorLibrary.Instance.GetMaterial(newColor);
            if (material != null)
                gameObject.GetComponent<Renderer>().material = material;

            m_CurrentColor = pickup.GetColor();
            pickup.OnPickup();
        }
    }
}
