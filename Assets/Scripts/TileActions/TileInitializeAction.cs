using UnityEngine;

namespace Assets.Scripts.TileActions
{
    public class TileInitializeAction : TileAction
    {
        private readonly float _speed = 0.1f;

        public TileInitializeAction(Tile tile) : base(tile)
        {
            tile.transform.localScale = Vector3.zero;
            tile.SpriteRenderer.enabled = true;
        }

        public override void Play()
        {
            // _scale += Time.deltaTime * _speed;
            SetTileScale();
            if (IsTileScaleGreaterThan1())
            {
                tile.transform.localScale = Vector3.one;
                ChangeAction(new TileReadyAction(tile));
            }
        }

        private void SetTileScale()
        {
            tile.transform.localScale = Vector3.Lerp(
                tile.transform.localScale,
                Vector3.one,
                _speed
            );
        }

        private bool IsTileScaleGreaterThan1()
        {
            return tile.transform.localScale.x > 1
                && tile.transform.localScale.y > 1
                && tile.transform.localScale.z > 1;
        }
    }
}
