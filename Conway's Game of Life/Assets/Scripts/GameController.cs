using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject _cellGrid;
    private Grid _grid;

    [SerializeField]
    private float _secondsPerTick;

    private float _time;

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

    public void StartSetupState()
    {
        SetupState();
    }

    public void StartRunState()
    {
        _grid.ClickOnCellsOff();
        _grid.PreTick();
        RunTimeState();
    }

    private void SetupState()
    {
        _grid.ClickOnCellsOn();
    }

    private void RunTimeState()
    {
        if (_secondsPerTick < Time.time - _time)
        {
            _grid.Tick();

            // get ready for the next tick
            _grid.PreTick();

            _time = Time.time;
        }
    }

    public void RunButtonState()
    {
        _grid.ClickOnCellsOff();
        _grid.PreTick();
        _grid.Tick();
    }
}
