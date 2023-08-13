using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using TilePair = System.ValueTuple<Tile, Tile>;

public class TileSwapHandler : MonoBehaviour
{
    private Tile _selectedTile;
    private TilePair _tiles;

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Reset();
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
            clear
        */

        if (_tiles.Item1 is null)
        {
            _tiles.Item1 = _selectedTile;
            transform.position = _tiles.Item1.transform.position;
            Show();
        }
        else
        {
            if (_tiles.Item1 != _selectedTile)
            {
                _tiles.Item2 = _selectedTile;
            }
        }
    }

    private void Show()
    {
        _spriteRenderer.enabled = true;
    }

    public void Reset()
    {
        _spriteRenderer.enabled = false;
        _tiles = new();
        _selectedTile = null;
    }

    public bool IsPlayerSelectedTwoTiles()
    {
        return _tiles.Item1 != null && _tiles.Item2 != null;
    }

    public TilePair GetSelectedTiles()
    {
        return _tiles;
    }
}
