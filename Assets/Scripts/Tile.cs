using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using Assets.Scripts.TileEffects;

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

    private TileEffect _effect;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _effect = new TileEffectReadyState(this);
    }

    private void FixedUpdate()
    {
        if (_effect.ShouldExecuteInFixedUpdate)
        {
            _effect.Play();
        }
    }

    private void Update()
    {
        if (_effect.ShouldExecuteInFixedUpdate == false)
        {
            _effect.Play();
        }
    }

    public void PlayRotationEffect(Vector3 rotationPoint)
    {
        ChangeEffect(new TileRotationEffect(this, rotationPoint));
    }

    public void PlayPopEffect() 
    { 
        ChangeEffect(new TilePopEffect(this));
    }

    public void ChangeEffect(TileEffect effect)
    {
        _effect = effect;
    }

    public bool IsReadyToPlayTileEffect()
    {
        return _effect is TileEffectReadyState;
    }

    public void SetSpriteSortingOrder(int order)
    {
        _spriteRenderer.sortingOrder = order;
    }
}
