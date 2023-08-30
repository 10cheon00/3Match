using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.TileActions
{
    public class TilePopAction : TileAction
    {
        private Material _flashMaterial;
        private Material _originalTileMaterial;
        private GameObject _popParticleEffectObject;
        private ParticleSystem _popParticleSystem;
        private float _flashingInterval = 0.5f;
        private bool _isPopParticleEffectRunning;

        public TilePopAction(Tile tile, Material flashMaterial, GameObject popParticleEffectPrefab)
            : base(tile)
        {
            if (tile.IsReadyToPlayTileAction() == false)
            {
                return;
            }

            _flashMaterial = flashMaterial;
            _popParticleEffectObject = GameObject.Instantiate(
                popParticleEffectPrefab,
                tile.transform.position,
                Quaternion.identity
            );
            _popParticleEffectObject.transform.SetParent(tile.transform);
            _popParticleSystem = _popParticleEffectObject.GetComponent<ParticleSystem>();

            _originalTileMaterial = tile.SpriteRenderer.material;
            _isPopParticleEffectRunning = false;

            tile.StartCoroutine(PlayFlashEffect());
        }


        public override void Play()
        {
            if(_isPopParticleEffectRunning && _popParticleSystem.isStopped)
            {
                Stop();
            }
        }

        private IEnumerator PlayFlashEffect()
        {
            // in flash effect, tile flashs itself twice quickly.
            // and show particle effect after hide itself.

            ChangeMaterialToFlash();
            yield return new WaitForSeconds(_flashingInterval);
            ChangeMaterialToOriginalTileMaterial();
            yield return new WaitForSeconds(_flashingInterval);
            ChangeMaterialToFlash();
            yield return new WaitForSeconds(_flashingInterval);
            ChangeMaterialToOriginalTileMaterial();
            yield return new WaitForSeconds(_flashingInterval);
            ChangeMaterialToFlash();
            yield return new WaitForSeconds(_flashingInterval);
            PlayPopParticleSystem();
            HideTile();

            yield return null;
        }

        private void ChangeMaterialToFlash()
        {
            tile.SpriteRenderer.material = _flashMaterial;
        }

        private void ChangeMaterialToOriginalTileMaterial()
        {
            tile.SpriteRenderer.material = _originalTileMaterial;
        }

        private void PlayPopParticleSystem()
        {
            _isPopParticleEffectRunning = true;
            _popParticleSystem.Play();
        }

        private void HideTile()
        {
            tile.SpriteRenderer.enabled = false;
        }

        protected override void Stop()
        {
            ChangeAction(new TileReadyAction(tile));
        }
    }
}
