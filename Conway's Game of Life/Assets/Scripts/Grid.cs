using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField]
    private int _gridSize = 5;

    [SerializeField]
    private GameObject _cellPrefab;

    private int _totalCells;
    private GameObject[] _cells;
    //private Cell[] _cells;

    //[SerializeField]
    //private GameObject[,] m_cellsGameObject;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
