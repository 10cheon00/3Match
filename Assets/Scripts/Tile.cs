using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/*
Tile의 역할
1. 자료구조로부터 현재 타일에 어떤 블록이 담겨있는지 갖고와 보여준다.
2. 사용자의 입력을 받아 어떤 타일과 교환되는지 확인 후 교환한다.
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
