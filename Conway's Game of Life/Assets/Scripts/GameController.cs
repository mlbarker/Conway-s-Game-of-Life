using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private Button _startButton;
    private bool _initialSetup;
    private bool _activeTimer;

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

        _activeTimer = false;
        _initialSetup = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_initialSetup)
        {
            if (_grid.GetCell(0) != null)
            {
                _initialSetup = false;
                StartSetupState();
            }
        }

        if (_activeTimer)
        {
            RunTimeState();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitGame();
        }
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
        _activeTimer = true;
    }

    public void RunButtonState()
    {
        TurnOffCellMouseInput();
        DetermineCellStatus();
        UpdateCellStatus();
    }

    public void SetupGrid10()
    {
        _grid.GridSize = 10;
        _grid.ResetGrid();
        _initialSetup = true;
    }

    public void SetupGrid20()
    {
        _grid.GridSize = 20;
        _grid.ResetGrid();
        _initialSetup = true;
    }

    public void SetupGrid50()
    {
        _grid.GridSize = 50;
        _grid.ResetGrid();
        _initialSetup = true;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    #endregion

    #region Private Methods

    private void SetupState()
    {
        _activeTimer = false;
        TurnOnCellMouseInput();
    }

    private void RunTimeState()
    {
        if (_secondsPerTick < Time.time - _time)
        {
            // get ready for the next tick
            DetermineCellStatus();
            UpdateCellStatus();

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

    private void RecreateGrid()
    {
        _grid.ResetGrid();
    }

    private void ResetCells()
    {
        for (int index = 0; index < _grid.TotalCells; ++index)
        {
            Cell cell = _grid.GetCell(index);
            if (cell != null)
            {
                cell.Clear();
            }
            else
            {
                Debug.LogError("Cell is null - ResetCells()");
            }
        }
    }

    #endregion
}
