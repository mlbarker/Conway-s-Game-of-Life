using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Cell : MonoBehaviour
{
    Animator m_animator;

    bool m_dead;

    // Start is called before the first frame update
    void Start()
    {
        m_animator = gameObject.GetComponent<Animator>();
        if (m_animator == null)
        {
            Debug.LogError("Animator is null");

            m_dead = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_dead = !m_dead;
            m_animator.SetBool("CellDead", m_dead);

            Debug.Log("Cell Dead : " + m_dead);
        }
    }
}
