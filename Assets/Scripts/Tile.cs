using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/*
Tile의 역할
1. 생성된 타일의 색을 관리한다.
*/

public class Tile : MonoBehaviour
{
    [SerializeField]
    private TileColor _tileColor;
    public TileColor Color
    {
        get { return _tileColor; }
        set { _tileColor = value; }
    }
}
