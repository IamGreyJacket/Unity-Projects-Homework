using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Checkers
{
    public class GameManager : MonoBehaviour, IReplay, IRecord
    {
        //shows whether any chip is chosen or not
        private bool _isChipChosen = false;
        //reference to a chosen chip
        private ChipComponent _chosenChip = null;
        //basically used to lock controls, while animation is played
        private bool _controlsLocked = false;

        private bool _isWhiteTurn = true;

        private byte _whiteChipCount = 0;
        private byte _blackChipCount = 0;
        private ChipComponent[] _chips;
        private CellComponent[] _cells;
        private Dictionary<NeighborType, CellComponent> _allowedCells = new Dictionary<NeighborType, CellComponent>();

        private static ReplayRecorder _replayRecorder;
        private string _recordCommandLine = "";
        //Works as an easy get for each command in line
        private string[] _recordSteps;
        private float _nextMoveDelay;
        private bool _isReplay = false;
        private bool _isRecording = false;

        [SerializeField]
        private TriggerComponent[] _gameOverTriggers;

        [Space, SerializeField]
        private Material _onFocusMaterial;
        [SerializeField]
        private Material _selectedMaterial;
        [SerializeField]
        private Material _allowedCellsMaterial;

        private void Start()
        {
            _replayRecorder = FindObjectOfType<ReplayRecorder>();
            if (_replayRecorder != null)
            {
                _replayRecorder = ReplayRecorder.Self;
                _isRecording = _replayRecorder.IsRecording;
                _isReplay = _replayRecorder.IsReplay;
                _nextMoveDelay = _replayRecorder.NextMoveDelay;
            }
            else _replayRecorder = null;
            

            _chips = FindObjectsOfType<ChipComponent>();
            _cells = FindObjectsOfType<CellComponent>();
            foreach(var chip in _chips)
            {
                chip.OnClickEventHandler += OnClick;
                chip.OnFocusEventHandler += OnFocus;
                chip.Pair = _cells.First(t => t.transform.position.x == chip.transform.position.x && t.transform.position.z == chip.transform.position.z);
                chip.Pair.Pair = chip;
                switch (chip.GetColor)
                {
                    case ColorType.White:
                        _whiteChipCount++;
                        break;
                    case ColorType.Black:
                        _blackChipCount++;
                        break;
                }
            }
            foreach (var cell in _cells)
            {
                if (cell.GetColor == ColorType.White) continue;
                cell.OnClickEventHandler += OnClick;
                cell.OnFocusEventHandler += OnFocus;
                Dictionary<NeighborType, CellComponent> neighbours = new Dictionary<NeighborType, CellComponent>();
                for(int i = 0; i < 4; i++)
                {
                    neighbours[(NeighborType)i] = null;
                }
                foreach(var c in _cells)
                {
                    var offset = cell.transform.position - c.transform.position;
                    if (c.GetColor == ColorType.White || Vector3.SqrMagnitude(offset) != 2 || cell == c) continue;
                    if (offset.x < 0 && offset.z < 0) neighbours[NeighborType.BottomLeft] = c;
                    else if (offset.x > 0 && offset.z < 0) neighbours[NeighborType.BottomRight] = c;
                    else if (offset.x < 0 && offset.z > 0) neighbours[NeighborType.TopLeft] = c;
                    else if (offset.x > 0 && offset.z > 0) neighbours[NeighborType.TopRight] = c;
                }
                cell.Configuration(neighbours);
            }
            foreach(var trigger in _gameOverTriggers)
            {
                trigger.OnTriggerEnterEvent += GameOver;
            }
            StartCoroutine(GameStateCheck());
            if (_isReplay)
            {
                _controlsLocked = true;
                StartCoroutine(Replay());
            }
            RecordCommand("White");
        }

        private IEnumerator GameStateCheck()
        {
            while (_blackChipCount > 0 && _whiteChipCount > 0)
            {
                yield return null;
            }
            if (_blackChipCount == 0) GameOver(ColorType.White);
            else if (_whiteChipCount == 0) GameOver(ColorType.Black);
        }

        private void GameOver(ColorType winningColor)
        {
            _controlsLocked = true;
            Debug.LogWarning($"{winningColor} side wins!");
        }

        private void OnChoose(BaseClickComponent component)
        {
            if (_controlsLocked && !_isReplay) return;
            //condition, where chip is chosen, but selected object is the chosen chip.
            //(Basically removes highlight from chosen chip and clears chosenChip-related variables)
            if (_isChipChosen && component == _chosenChip | component == _chosenChip.Pair)
            {
                //Добавить запись отмены выбора шашки в повторе
                DehighlightAllowedCell();
                _chosenChip.RemoveAdditionalMaterial(2);
                _chosenChip = null;
                _isChipChosen = false;
            }
            //condition, where chip is chosen, but the selected object is CellComponent
            //(Moves chosen chip to selected Cell)
            else if (_isChipChosen && component.Pair == null && _allowedCells.ContainsValue((CellComponent)component))
            {
                DehighlightAllowedCell();
                _chosenChip.RemoveAdditionalMaterial(2);

                StartCoroutine(MoveToCell(_chosenChip, (CellComponent)component));

                RecordCommand($" {component.name}");

                _chosenChip.Pair.Pair = null;
                _chosenChip.Pair = component;
                _chosenChip.Pair.Pair = _chosenChip;

                _chosenChip = null;
                _isChipChosen = false;
            }
            else if (_isChipChosen && component.Pair != null)
            {
                CellComponent cell = null;
                if (component.GetType() == typeof(ChipComponent)) cell = (CellComponent)component.Pair;
                else if (component.GetType() == typeof(CellComponent)) cell = (CellComponent)component;

                if (cell == null || !_allowedCells.ContainsValue(cell)) return;

                for (int i = 0; i < 4; i++)
                {
                    if (_allowedCells.ContainsKey((NeighborType)i) && _allowedCells[(NeighborType)i] == cell)
                    {
                        var neighbor = cell.GetNeighbors((NeighborType)i);
                        if (neighbor == null || neighbor.Pair != null) return;
                        DehighlightAllowedCell();
                        _chosenChip.RemoveAdditionalMaterial(2);

                        StartCoroutine(MoveToCell(_chosenChip, neighbor));
                        RecordCommand($" {cell.name}");

                        _chosenChip.Pair.Pair = null;
                        _chosenChip.Pair = neighbor;
                        _chosenChip.Pair.Pair = _chosenChip;

                        if (cell.Pair.GetColor == ColorType.White) _whiteChipCount--;
                        else if (cell.Pair.GetColor == ColorType.Black) _blackChipCount--;
                        //Добавить запись в повтор уничтожение шашки
                        Destroy(cell.Pair.gameObject);
                        cell.Pair = null;

                        _chosenChip = null;
                        _isChipChosen = false;
                    }
                }
            }
            //condition, where chip is not chosen
            //(Highlights chosen chip and allowed paths/cells where chip can go)
            else if (!_isChipChosen && _chosenChip == null)
            {
                if (component.Pair == null) return;
                if (component.GetType() == typeof(ChipComponent)) _chosenChip = (ChipComponent)component;
                else if (component.GetType() == typeof(CellComponent)) _chosenChip = (ChipComponent)component.Pair;
                else
                {
                    //Just to be sure
                    Debug.LogWarning("This is neither a chip nor a cell");
                    return;
                }
                if (_isWhiteTurn & _chosenChip.GetColor != ColorType.White || !_isWhiteTurn & _chosenChip.GetColor != ColorType.Black)
                {
                    Debug.LogWarning("You can't make move for this side just yet. Play the chip with different color.");
                    _chosenChip = null;
                    return;
                }
                _isChipChosen = true;
                _chosenChip.AddAdditionalMaterial(_selectedMaterial, 2);
                HighlightAllowedCells((CellComponent)_chosenChip.Pair);
                RecordCommand($" {_chosenChip.Pair.name}");
            }
        }

        private void OnClick(BaseClickComponent component)
        {
            OnChoose(component);
        }

        private void OnFocus(CellComponent component, bool isSelect)
        {
            if (_controlsLocked)
            {
                component.RemoveAdditionalMaterial(2);
                return;
            }
            if (isSelect) component.AddAdditionalMaterial(_onFocusMaterial, 2);
            else component.RemoveAdditionalMaterial(2);
        }

        private void CheckMove(CellComponent cell, NeighborType type)
        {
            CellComponent neighbor = cell.GetNeighbors(type);
            CellComponent furtherNeighbor = null;
            if (neighbor != null)
            {
                furtherNeighbor = neighbor.GetNeighbors(type);

                if (neighbor.Pair == null)
                {
                    AddToAllowedCells(type, neighbor);
                }
                else if (neighbor.Pair != null && neighbor.Pair.GetColor != cell.Pair.GetColor && furtherNeighbor != null)
                {
                    if (furtherNeighbor.Pair == null)
                    {
                        AddToAllowedCells(type, neighbor);
                    }
                }
            }
        }

        private void DehighlightAllowedCell()
        {
            foreach(var cell in _allowedCells.Values)
            {
                cell.RemoveAdditionalMaterial();
            }
            _allowedCells.Clear();
        }

        private void HighlightAllowedCells(CellComponent cell)
        {
            if (cell.Pair.GetColor == ColorType.White)
            {
                CheckMove(cell, NeighborType.TopLeft);

                CheckMove(cell, NeighborType.TopRight);

                if (_allowedCells.Count == 0)
                {
                    _chosenChip.RemoveAdditionalMaterial(2);
                    _chosenChip = null;
                    _isChipChosen = false;
                }
            }
            else if(cell.Pair.GetColor == ColorType.Black)
            {
                CheckMove(cell, NeighborType.BottomLeft);

                CheckMove(cell, NeighborType.BottomRight);

                if (_allowedCells.Count == 0)
                {
                    _chosenChip.RemoveAdditionalMaterial(2);
                    _isChipChosen = false;
                }
            }
        }

        private void AddToAllowedCells(NeighborType type, CellComponent neighbor)
        {
            _allowedCells[type] = neighbor;
            _allowedCells[type].AddAdditionalMaterial(_allowedCellsMaterial);
        }

        private IEnumerator MoveToCell(ChipComponent chip, CellComponent cell)
        {
            float speedMultiplier = 2f;
            _controlsLocked = true;
            var time = 0f;
            var startPos = chip.transform.position;
            var endPos = cell.transform.position;
            endPos.y = startPos.y;
            while (time < 1f / speedMultiplier)
            {
                chip.transform.position = Vector3.Lerp(startPos, endPos, time * speedMultiplier);
                time += Time.deltaTime;
                yield return null;
            }
            chip.transform.position = endPos;
            _isWhiteTurn = !_isWhiteTurn;
            WriteLine(_recordCommandLine);
            if(!_isReplay) _controlsLocked = false;
        }

        private IEnumerator Replay()
        {
            yield return new WaitForSeconds(2f);
            _recordCommandLine = GetNextLine();
            while (_recordCommandLine != null && _recordCommandLine != "" && _recordCommandLine != string.Empty)
            {
                _recordSteps = _recordCommandLine.Split();
                for(int i = 0; i < _recordSteps.Length; i++)
                {
                    string step = _recordSteps[i];
                    if (step == "Black" || step == "White") continue;
                    BaseClickComponent cell = null;
                    for (int j = 0; j < _cells.Length; j++)
                    {
                        if (_cells[j].name == step) cell = _cells[j];
                    }
                    if (cell != null) OnChoose(cell);
                    else Debug.LogError($"Couldn't find the cell by it's name \"{step}\". Please check the names of GameObjects(cells) or the code");
                    yield return new WaitForSeconds(_nextMoveDelay);
                }
                _recordCommandLine = GetNextLine();
                yield return new WaitForSeconds(_nextMoveDelay);
            }
            Debug.LogWarning("The end of the Replay");
            yield return null;
        }

        private void RecordCommand(string command)
        {
            if (_isRecording == false) return;
            _recordCommandLine += command;
        }

        public string GetNextLine()
        {
            if (_replayRecorder == null) return null;
            return _replayRecorder.GetNextLine();
        }

        public void PlayLine(string recordLine)
        {
            if (_replayRecorder == null) return;
            //Распознает данную часть строки и вызывает команду соответственно ей.
            //Например, если написано A3, то производится выбор этой клетки. Дальше передвижение/поедание фишек просчитывает сама игра.
        }

        public void WriteLine(string recordLine)
        {
            if (_replayRecorder == null) return;
            if (_isRecording == false) return;
            _replayRecorder.WriteRecord(_recordCommandLine);
            if (_isWhiteTurn) _recordCommandLine = "White";
            else _recordCommandLine = "Black";
        }
    }
}
