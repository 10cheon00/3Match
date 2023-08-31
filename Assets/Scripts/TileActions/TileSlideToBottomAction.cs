using UnityEngine;

namespace Assets.Scripts.TileActions
{
    public class TileSlideToBottomAction : TileAction
    {
        private Vector3 _to;
        private float _speed = 5f;
        public TileSlideToBottomAction(Tile tile, Vector3 from, Vector3 to) : base(tile) 
        {
            tile.transform.position = from;
            _to = to;
        }

        public override void Play() 
        {
            tile.transform.position += Vector3.down * _speed * Time.deltaTime;
            if ((tile.transform.position - _to).sqrMagnitude < 0.01f)
            {
                Stop();
            }
        }

        protected override void Stop()
        {
            tile.transform.position = _to;
            ChangeAction(new TileReadyAction(tile));
        }
    }
}
