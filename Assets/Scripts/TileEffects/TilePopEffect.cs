using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.TileEffects
{
    public class TilePopEffect : TileEffect
    {
        public TilePopEffect(Tile tile) : base(tile) { }

        public override void Play()
        {
            ChangeMaterialColorToWhite();
            if (IsMaterialColorEqualWhite())
            {
                ChangeEffect(new TileEffectReadyState(tile));
            }
        }

        private void ChangeMaterialColorToWhite()
        {

        }

        private bool IsMaterialColorEqualWhite()
        {
            return false;
        }
    }
}
