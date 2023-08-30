using System;
using System.Collections.Generic;
using UnityEngine;

public class TileBoardFactory : MonoBehaviour
{
    [SerializeField]
    private GameObject _tileBoardObject;
    [SerializeField]
    private List<GameObject> _tilePrefabList;

    private float _tileWidth;
    private float _tileHeight;
    private Coord _tileBoardSize;
    private Vector3 _offset;
    private Vector3 _tileBoardTopLeftPosition;

    private void Awake()
    {
        SpriteRenderer tileSpriteRenderer = _tilePrefabList[0].GetComponent<SpriteRenderer>();
        _tileWidth = tileSpriteRenderer.bounds.size.x;
        _tileHeight = tileSpriteRenderer.bounds.size.y;
        _offset = new();
    }

    public TileBoard CreateTileBoard(Coord tileBoardSize)
    {
        // todo
        // improve calculation of initial spawn point.
        _tileBoardSize = tileBoardSize;
        TileBoard tileBoard = new();

        CalculateTileBoardTopLeftPosition();
        for(Coord coord = new(0, 0); coord.y < _tileBoardSize.y; coord.y++)
        {
            List<Tile> tileList = new();

            for(coord.x = 0; coord.x < _tileBoardSize.x; coord.x++)
            {
                GameObject tileObject = CreateRandomTileObject();
                tileObject.transform.localPosition = GetPositionByCoord(coord);

                Tile tile = tileObject.GetComponent<Tile>();
                tileList.Add(tile);
                tileObject.name = $"({coord.y}, {coord.x})::{tile.Color}Tile";

            }

            tileBoard.Add(tileList);
        }

        return tileBoard;
    }

    private void CalculateTileBoardTopLeftPosition()
    {
        CalculateOffset();
        _tileBoardTopLeftPosition = new(
            -1 * _tileWidth * (_tileBoardSize.x / 2 + _offset.x),
            _tileHeight * (_tileBoardSize.y / 2 + _offset.y),
            0
        );
    }


    private void CalculateOffset()
    {
        if (_tileBoardSize.x % 2 == 0)
        {
            _offset.x = -0.5f;
        }
        else
        {
            _offset.x = 0;
        }
        if (_tileBoardSize.y % 2 == 0)
        {
            _offset.y = -0.5f;
        }
        else
        {
            _offset.y = 0f;
        }
    }
    
    public Vector3 GetPositionByCoord(Coord coord)
    {
        // Vector3 tilePos =
        //     new(-_width * (_tileBoardSize.x / 2 + _offset.x),
        //         _height * (_tileBoardSize.y / 2 + _offset.y), 0);
        
        // tileObject.transform.localPosition =
        //     tilePos + new Vector3(_width * coord.x, -_height * coord.y, 0);
        
        return _tileBoardTopLeftPosition + new Vector3(_tileWidth * coord.x, -1 * _tileHeight * coord.y, 0);
    }

    public GameObject CreateRandomTileObject()
    {
        System.Random random = new();

        int index = random.Next(_tilePrefabList.Count);
        GameObject tileObject = Instantiate(
            _tilePrefabList[index],
            transform.position,
            Quaternion.identity
        );

        tileObject.transform.SetParent(_tileBoardObject.transform);

        return tileObject;
    }
}
