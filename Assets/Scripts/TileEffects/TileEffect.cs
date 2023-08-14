namespace Assets.Scripts.TileEffects
{
    public abstract class TileEffect
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

        public TileEffect(Tile tile, bool shouldExecuteInFixedUpdate = false)
        {
            _tile = tile;
            Start();
        }

        protected virtual void Start() { }

        public abstract void Play();

        public void ChangeEffect(TileEffect effect)
        {
            _tile.ChangeEffect(effect);
        }
    }
}
