using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField]
    private int _gridSize = 5;

    [SerializeField]
    private GameObject _cellPrefab;

    private int _totalCells;
    private GameObject[] _cells;

    // Start is called before the first frame update
    void Start()
    {
        _totalCells = _gridSize * _gridSize;
        _cells = new GameObject[_totalCells];

        // setup the cells' position in the grid
        // need to do null checks here
        float width = _cellPrefab.GetComponent<BoxCollider2D>().size.x;
        float height = _cellPrefab.GetComponent<BoxCollider2D>().size.y;

        // coordinates to set the position of each cell
        int x = _gridSize / -2;
        int y = _gridSize / -2;
        for (int index = 0; index < _totalCells; ++index)
        {
            // instantiate the cell
            Vector3 position = new Vector3(width * x, height * y, 1);
            _cells[index] = Instantiate(_cellPrefab, position, Quaternion.identity);
            
            // update the x offset
            x++;

            // row was filled, onto the next 
            // row (update the y offset)
            if ((index + 1) % _gridSize == 0 && index != 0)
            {
                x = _gridSize / -2;
                y++;
            }
        }

        // set up neighbors for each cell
        for (int index = 0; index < _cells.Length; ++index)
        {
            Cell cell = _cells[index].GetComponent<Cell>();
            if (cell != null)
            {
                // THE SPECIAL CASE TREE
                // front index
                if (/*index != 0 &&*/ index % _gridSize != 0)
                {
                    cell.AddNeighbor(_cells[index - 1].GetComponent<Cell>());
                }

                // behind index
                if (index != _totalCells - 1 && (index + 1) % _gridSize != 0)
                {
                    cell.AddNeighbor(_cells[index + 1].GetComponent<Cell>());
                }

                // above index
                if (index - _gridSize >= 0)
                {
                    cell.AddNeighbor(_cells[index - _gridSize].GetComponent<Cell>());
                }

                // below index
                if (index + _gridSize <= _totalCells - 1)
                {
                    cell.AddNeighbor(_cells[index + _gridSize].GetComponent<Cell>());
                }

                // above left index
                if ((index - _gridSize) - 1 >= 0 && index % _gridSize != 0)
                {
                    cell.AddNeighbor(_cells[(index - _gridSize) - 1].GetComponent<Cell>());
                }

                // above right index
                if ((index - _gridSize) + 1 >= 0 && (index + 1) % _gridSize != 0)
                {
                    cell.AddNeighbor(_cells[(index - _gridSize) + 1].GetComponent<Cell>());
                }

                // below left index
                if ((index + _gridSize) - 1 <= _totalCells - 1 && index % _gridSize != 0)
                {
                    cell.AddNeighbor(_cells[(index + _gridSize) - 1].GetComponent<Cell>());
                }

                // below right index
                if ((index + _gridSize) + 1 <= _totalCells - 1 && (index + 1) % _gridSize != 0)
                {
                    cell.AddNeighbor(_cells[(index + _gridSize) + 1].GetComponent<Cell>());
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickOnCellsOn()
    {
        foreach(var cellGO in _cells)
        {
            Cell cell = cellGO.GetComponent<Cell>();
            if (cell != null)
            {
                cell.Click = true;
                cell.Clear();
            }
            else
            {
                Debug.LogError("Cell is null - ClickOnCellsOn()");
            }
        }
    }

    public void ClickOnCellsOff()
    {
        foreach (var cellGO in _cells)
        {
            Cell cell = cellGO.GetComponent<Cell>();
            if (cell != null)
            {
                cell.Click = false;
            }
            else
            {
                Debug.LogError("Cell is null - ClickOnCellsOff()");
            }
        }
    }

    public void PreTick()
    {
        for (int index = 0; index < _cells.Length; ++index)
        {
            Cell cell = _cells[index].GetComponent<Cell>();
            if (cell != null)
            {
                cell.DetermineNextStatus();
            }
            else
            {
                Debug.LogError("Cell is null - PreTick()");
            }
        }
    }

    public void Tick()
    {
        for (int index = 0; index < _cells.Length; ++index)
        {
            Cell cell = _cells[index].GetComponent<Cell>();
            if (cell != null)
            {
                cell.ExecuteStatus();
            }
            else
            {
                Debug.LogError("Cell is null - PreTick()");
            }
        }
    }
}
