using System;
using System.Collections.Generic;
using UnityEngine;

public class TileBoardFactory : MonoBehaviour
{
    [SerializeField]
    private GameObject _tileBoardObject;
    [SerializeField]
    private List<GameObject> _tilePrefabList;

    private float _width;
    private float _height;

    private void Awake()
    {
        SpriteRenderer tileSpriteRenderer = _tilePrefabList[0].GetComponent<SpriteRenderer>();
        _width = tileSpriteRenderer.bounds.size.x;
        _height = tileSpriteRenderer.bounds.size.y;
    }

    public TileBoard CreateTileBoard(int xSize, int ySize, TileSwapHandler _tileSwapHandler)
    {
        // todo
        // improve calculation of initial spawn point.

        Vector3 offset = new(xSize % 2 == 0 ? -0.5f : 0f, ySize % 2 == 0 ? -0.5f : 0f, 0);
        Vector3 tilePos =
            new(-_width * (xSize / 2 + offset.x), -_height * (ySize / 2 + offset.y), 0);

        TileBoard tileBoard = new();

        for (int i = 0; i < ySize; i++)
        {
            List<Tile> tileList = new();

            for (int j = 0; j < xSize; j++)
            {
                GameObject tileObject = CreateRandomTileObject();
                tileObject.transform.SetParent(_tileBoardObject.transform);
                tileObject.transform.localPosition =
                    tilePos + new Vector3(_width * j, _height * i, 0);

                Tile tile = tileObject.GetComponent<Tile>();
                tile.Initialize(_tileSwapHandler);
                tileList.Add(tile);
            }

            tileBoard.Add(tileList);
        }

        return tileBoard;
    }

    private GameObject CreateRandomTileObject()
    {
        System.Random random = new();

        int index = random.Next(_tilePrefabList.Count);
        GameObject tileObject = Instantiate(
            _tilePrefabList[index],
            transform.position,
            Quaternion.identity
        );

        return tileObject;
    }
}
