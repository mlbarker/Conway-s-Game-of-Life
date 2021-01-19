using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Cell : MonoBehaviour
{
    #region Read Only Members

    private const int MAX_NEIGHBORS = 8;

    #endregion

    #region Private Members

    private Animator m_animator;
    private bool _dead;
    private Queue<bool> _nextStatus = new Queue<bool>();
    private Cell[] _neighbors = new Cell[MAX_NEIGHBORS];
    private int _aliveNeighbors;
    private int _neighborIndex;
    private bool _click = false;

    #endregion

    #region Properties

    public bool Dead
    {
        get => _dead;
        private set => _dead = value;
    }

    public bool NextStatus
    {
        get => _nextStatus.Peek();
        private set => _nextStatus.Enqueue(value);
    }

    public int AliveNeighbors
    {
        get => _aliveNeighbors;
        private set => _aliveNeighbors = value;
    }

    public bool Click
    {
        get => _click;
        set => _click = value;
    }

    #endregion

    #region Unity Methods

    // Start is called before the first frame update
    void Start()
    {
        m_animator = gameObject.GetComponent<Animator>();
        if (m_animator != null)
        {
            _dead = true;
            m_animator.SetBool("CellDead", _dead);
        }
        else
        {
            Debug.LogError("Animator is null");
        }

        _neighborIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && Click)
        {
            _dead = !_dead;
            m_animator.SetBool("CellDead", _dead);

            Debug.Log("Cell Dead : " + _dead);
        }
    }

    #endregion

    #region Public Methods

    public void Clear()
    {
        _dead = true;
        m_animator.SetBool("CellDead", _dead);
    }

    public void AddNeighbor(Cell neighbor)
    {
        if (neighbor != null)
        {
            if (_neighborIndex < MAX_NEIGHBORS)
            {
                _neighbors[_neighborIndex] = neighbor;
                _neighborIndex++;
            }
            else
            {
                Debug.LogWarning("Cell neighbors amount is maxed - AddNeighbor()");
            }
        }
        else
        {
            Debug.LogError("neighbor is null - AddNeighbor()");
        }
    }

    public void DetermineNextStatus()
    {
        // check for neighbors that are alive
        int aliveNeighbors = 0;
        foreach (var neighbor in _neighbors)
        {
            if (neighbor != null)
            {
                if (!neighbor.Dead)
                {
                    aliveNeighbors++;
                }
            }
        }

        if (!_dead)
        {
            // fewer than two live neighbors or 
            // four or more neighbors kills this cell
            if (aliveNeighbors < 2 || aliveNeighbors >= 4)
            {
                NextStatus = true;
            }
            else
            {
                NextStatus = false;
            }
        }
        // resurrection
        else if (_dead && aliveNeighbors == 3)
        {
            NextStatus = false;
        }
        // status stays the same
        else
        {
            NextStatus = true;
        }
    }

    public void ExecuteStatus()
    {
        if (_nextStatus.Count > 0)
        {
            _dead = _nextStatus.Dequeue();
        }

        m_animator.SetBool("CellDead", _dead);
    }

    #endregion
}
