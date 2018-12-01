using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float m_MovementSpeed = 5f;
    private Vector3 m_Direction;

    private void Start()
    {
        m_Direction = Vector3.left;
    }


    void Update()
    {
        // Toggle direction
        if (Input.GetKeyDown(KeyCode.Space))
            m_Direction = m_Direction == Vector3.left ? Vector3.right : Vector3.left;

        transform.position += new Vector3(m_Direction.x * m_MovementSpeed * Time.deltaTime, 0, m_MovementSpeed * Time.deltaTime);
    }
}
