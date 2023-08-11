using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSwapHandler : MonoBehaviour
{
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

    public void SelectTile(Tile selectedTile)
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
            _source = selectedTile;
            transform.position = _source.transform.position;
            Show();
        }
        else
        {
            _destination = selectedTile;
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
        _source = _destination = null;
    }
}
