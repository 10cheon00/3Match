using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using Assets.Scripts.TileActions;

/*
Tile의 역할
1. 생성된 타일의 색을 관리한다.
*/

public class Tile : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    public SpriteRenderer SpriteRenderer
    {
        get { return _spriteRenderer; }
    }

    [SerializeField]
    private TileColor _tileColor;
    public TileColor Color
    {
        get { return _tileColor; }
        set { _tileColor = value; }
    }

    private TileAction _action;

    [SerializeField]
    private Material _flashMaterial;
    [SerializeField]
    private GameObject _popParticleEffectPrefab;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _action = new TileReadyAction(this);
    }

    private void FixedUpdate()
    {
        if (_action.ShouldExecuteInFixedUpdate)
        {
            _action.Play();
        }
    }

    private void Update()
    {
        if (_action.ShouldExecuteInFixedUpdate == false)
        {
            _action.Play();
        }
    }

    public void PlayRotationAction(Vector3 rotationPoint)
    {
        ChangeAction(new TileRotationAction(this, rotationPoint));
    }

    public void PlayPopAction()
    { 
        ChangeAction(new TilePopAction(this, _flashMaterial, _popParticleEffectPrefab));
    }

    public void PlayNewTileInsertAction()
    {
        ChangeAction(new TileInitializeAction(this));
    }

    public void ChangeAction(TileAction action)
    {
        _action = action;
    }

    public bool IsReadyToPlayTileAction()
    {
        return _action is TileReadyAction;
    }

    public void SetSpriteSortingOrder(int order)
    {
        _spriteRenderer.sortingOrder = order;
    }
}
