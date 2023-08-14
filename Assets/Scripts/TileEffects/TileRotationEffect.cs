using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.TileEffects
{
    public class TileRotationEffect : TileEffect
    {
        private Vector3 _rotationPoint;
        private float _rotatedAngle;
        private readonly float _angle = 10f;

        public TileRotationEffect(Tile tile, Vector3 rotationPoint) : base(tile, true)
        {
            _rotationPoint = rotationPoint;
            _rotatedAngle = 0;
            tile.SetSpriteSortingOrder((int)TileBoardSortingOrder.SwappingTile);
        }

        public override void Play()
        {
            tile.transform.RotateAround(_rotationPoint, Vector3.forward, _angle);
            _rotatedAngle += _angle;

            ResetRotation();
            if (_rotatedAngle >= 180f)
            {
                tile.SetSpriteSortingOrder((int)TileBoardSortingOrder.Default);
                ChangeEffect(new TileEffectReadyState(tile));
            }
        }

        private void ResetRotation()
        {
            Quaternion quaternion = tile.transform.rotation;
            quaternion.eulerAngles = Vector3.zero;
            tile.transform.rotation = quaternion;
        }
    }
}
