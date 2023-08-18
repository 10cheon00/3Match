namespace Assets.Scripts.TileActions
{
    public abstract class TileAction
    {
        private Tile _tile;
        protected Tile tile
        {
            get { return _tile; }
        }

        private bool _shouldExecuteInFixedUpdate;
        public bool ShouldExecuteInFixedUpdate
        {
            get { return _shouldExecuteInFixedUpdate; }
        }

        public TileAction(Tile tile, bool shouldExecuteInFixedUpdate = false)
        {
            _tile = tile;
        }

        public virtual void Play() { }

        public void ChangeAction(TileAction effect)
        {
            _tile.ChangeAction(effect);
        }

        protected virtual void Stop() { }
    }
}
