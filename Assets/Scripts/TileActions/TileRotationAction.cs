using UnityEngine;

namespace Assets.Scripts.TileActions
{
    public class TileRotationAction : TileAction
    {
        private Vector3 _rotationPoint;
        private float _rotatedAngle;
        private readonly float _angle = 10f;

        public TileRotationAction(Tile tile, Vector3 rotationPoint) : base(tile, true)
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
                Stop();
            }
        }

        private void ResetRotation()
        {
            Quaternion quaternion = tile.transform.rotation;
            quaternion.eulerAngles = Vector3.zero;
            tile.transform.rotation = quaternion;
        }

        protected override void Stop()
        {
            tile.SetSpriteSortingOrder((int)TileBoardSortingOrder.Default);
            ChangeAction(new TileReadyAction(tile));
        }
    }
}
