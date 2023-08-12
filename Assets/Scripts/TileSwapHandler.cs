using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileSwapHandler : MonoBehaviour
{
    private Tile _selectedTile;
    private Tile _source;
    private Tile _destination;

    [SerializeField]
    private TileBoardManager _tileBoardManager;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Hide();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastToTileBoard();
            if (IsMouseClickTile())
            {
                SelectTile();
            }
        }
    }

    private void RaycastToTileBoard()
    {
        Vector2 mousePositionOnScreen = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D raycastHit2D = Physics2D.Raycast(mousePositionOnScreen, Vector2.zero);

        if (raycastHit2D.collider != null)
        {
            GameObject obj = raycastHit2D.collider.gameObject;
            if (obj.CompareTag("Tile"))
            {
                _selectedTile = obj.GetComponent<Tile>();
            }
        }
    }

    private bool IsMouseClickTile()
    {
        return _selectedTile != null;
    }

    private void SelectTile()
    {
        /*
        if source is null:
            selectedTile is source
        else:
            selectedTile is destination
            swap source and destination
            clear
        */

        if (_source is null)
        {
            _source = _selectedTile;
            transform.position = _source.transform.position;
            Show();
        }
        else
        {
            _destination = _selectedTile;
            if (_source != _destination)
            {
                _tileBoardManager.SwapTile(_source, _destination);
            }

            Hide();
            Reset();
        }
    }

    private void Show()
    {
        _spriteRenderer.enabled = true;
    }

    private void Hide()
    {
        _spriteRenderer.enabled = false;
    }

    private void Reset()
    {
        _source = _destination = _selectedTile = null;
    }
}
