using UnityEngine;

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
            else
            {
                Reset();
            }
        }
    }

    private void RaycastToTileBoard()
    {
        _selectedTile = null;
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
        if tileA is null:
            tileA = selectedTile
        else:
            if tileA != selectedTile:
                tileB = selectedTile
            else:
                reset
        */

        if (_tiles.tileA is null)
        {
            _tiles.tileA = _selectedTile;
            transform.position = _tiles.tileA.transform.position;
            Show();
        }
        else
        {
            if (_tiles.tileA != _selectedTile)
            {
                _tiles.tileB = _selectedTile;
            }
            else
            {
                Reset();
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
        _tiles.tileA = _tiles.tileB = _selectedTile = null;
    }

    public bool IsPlayerSelectedTwoTiles()
    {
        return _tiles.tileA != null && _tiles.tileB != null;
    }

    public TilePair GetSelectedTiles()
    {
        return _tiles;
    }
}
