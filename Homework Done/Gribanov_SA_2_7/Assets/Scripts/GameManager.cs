using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace Checkers
{
    public class GameManager : MonoBehaviour
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

        private void OnClick(BaseClickComponent component)
        {
            if (_controlsLocked) return;
            //condition, where chip is chosen, but selected object is the chosen chip.
            //(Basically removes highlight from chosen chip and clears chosenChip-related variables)
            if (_isChipChosen && component == _chosenChip | component == _chosenChip.Pair)
            {
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

                for(int i = 0; i < 4; i++)
                {
                    if (_allowedCells.ContainsKey((NeighborType)i) && _allowedCells[(NeighborType)i] == cell)
                    {
                        var neighbor = cell.GetNeighbors((NeighborType)i);
                        if (neighbor == null || neighbor.Pair != null) return;
                        DehighlightAllowedCell();
                        _chosenChip.RemoveAdditionalMaterial(2);

                        StartCoroutine(MoveToCell(_chosenChip, neighbor));

                        _chosenChip.Pair.Pair = null;
                        _chosenChip.Pair = neighbor;
                        _chosenChip.Pair.Pair = _chosenChip;

                        if (cell.Pair.GetColor == ColorType.White) _whiteChipCount--;
                        else if (cell.Pair.GetColor == ColorType.Black) _blackChipCount--;
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
            }

        }

        private void OnFocus(CellComponent component, bool isSelect)
        {
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
            _controlsLocked = true;
            var time = 0f;
            var startPos = chip.transform.position;
            var endPos = cell.transform.position;
            endPos.y = startPos.y;
            while (time < 1f)
            {
                chip.transform.position = Vector3.Lerp(startPos, endPos, time);
                time += Time.deltaTime;
                yield return null;
            }
            chip.transform.position = endPos;
            _isWhiteTurn = !_isWhiteTurn;
            _controlsLocked = false;
        }
    }
}
