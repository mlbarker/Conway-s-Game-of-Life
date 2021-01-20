using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    #region Editor Fields

    [SerializeField]
    private GameObject _cellGrid;

    [SerializeField]
    private float _secondsPerTick;

    #endregion

    #region Fields

    private Grid _grid;
    private float _time;

    #endregion

    #region Unity Methods

    // Start is called before the first frame update
    void Start()
    {
        _grid = _cellGrid.GetComponent<Grid>();
        if(_grid == null)
        {
            Debug.LogError("Grid is null - Start()");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion

    #region Public Methods

    public void StartSetupState()
    {
        SetupState();
    }

    public void StartRunState()
    {
        TurnOffCellMouseInput();
        DetermineCellStatus();
        RunTimeState();
    }

    public void RunButtonState()
    {
        TurnOffCellMouseInput();
        DetermineCellStatus();
        UpdateCellStatus();
    }

    #endregion

    #region Private Methods

    private void SetupState()
    {
        TurnOnCellMouseInput();
    }

    private void RunTimeState()
    {
        if (_secondsPerTick < Time.time - _time)
        {
            UpdateCellStatus();

            // get ready for the next tick
            DetermineCellStatus();

            _time = Time.time;
        }
    }

    private void TurnOnCellMouseInput()
    {
        _grid.ClickOnCellsOn();
    }

    private void TurnOffCellMouseInput()
    {
        _grid.ClickOnCellsOff();
    }

    private void DetermineCellStatus()
    {
        for (int index = 0; index < _grid.TotalCells; ++index)
        {
            Cell cell = _grid.GetCell(index);

            if (cell != null)
            {
                // check for neighbors that are alive
                int aliveNeighbors = cell.AliveNeighbors;
                if (!cell.Dead)
                {
                    // fewer than two live neighbors or 
                    // four or more neighbors kills this cell
                    if (aliveNeighbors < 2 || aliveNeighbors >= 4)
                    {
                        cell.NextStatus = true;
                    }
                    else
                    {
                        cell.NextStatus = false;
                    }
                }
                // resurrection
                else if (cell.Dead && cell.AliveNeighbors == 3)
                {
                    cell.NextStatus = false;
                }
                // status stays the same
                else
                {
                    cell.NextStatus = true;
                }
            }
            else
            {
                Debug.LogError("Cell is null - DetermineCellStatus()");
            }
        }
    }

    private void UpdateCellStatus()
    {
        for (int index = 0; index < _grid.TotalCells; ++index)
        {
            Cell cell = _grid.GetCell(index);
            if (cell != null)
            {
                cell.ExecuteStatus();
            }
            else
            {
                Debug.LogError("Cell is null - UpdateCellStatus()");
            }
        }
    }

    #endregion
}
