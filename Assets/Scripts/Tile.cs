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
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private TileColor _tileColor;
    public TileColor Color
    {
        get { return _tileColor; }
        set { _tileColor = value; }
    }

    private bool _canRotate = false;
    public bool IsSwappingEffectFinished { get { return _canRotate == false; } }
    private Vector3 _rotationPoint;
    private float _rotatedAngle = 0f;
    private readonly float _angle = 15f;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (_canRotate)
        {
            Rotate();

            if (_rotatedAngle >= 180f)
            {
                End180DegreeRotation();
            }
        }
    }

    private void Rotate()
    {
        transform.RotateAround(_rotationPoint, Vector3.forward, _angle);
        _rotatedAngle += _angle;

        ResetRotation();
    }

    private void ResetRotation()
    {
        Quaternion quaternion = transform.rotation;
        quaternion.eulerAngles = Vector3.zero;
        transform.rotation = quaternion;
    }

    public void Start180DegreeRotation(Vector3 rotationPoint)
    {
        _rotationPoint = rotationPoint;
        _rotatedAngle = 0;
        _canRotate = true;
        _spriteRenderer.sortingOrder = (int)TileBoardSortingOrder.SwappingTile;
    }

    private void End180DegreeRotation()
    {
        _canRotate = false;
        _spriteRenderer.sortingOrder = (int)TileBoardSortingOrder.Default;
    }
}
