using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameManagerStates.GameManagerTasks
{
    public class GameManagerFillTileBoardTask : GameManagerTask
    {
        private TileBoardManager _tileBoardManager;
        private TileBoard _tileBoardSnapshot;
        private List<(Tile, Coord, Coord)> _tileListForSlideAction;

        public GameManagerFillTileBoardTask(
            GameManagerState gameManagerState,
            TileBoardManager tileBoardManager
        ) : base(gameManagerState)
        {
            _tileBoardManager = tileBoardManager;
            _tileListForSlideAction = new();
        }

        public override void InitializeTask()
        {
            MoveTilesToBottomAndPlaySlideAction();
            InsertNewTilesAndPlaySlideAction();
            PlayTileSlideAction();
        }

        public override void RunTask()
        {
            bool isSlideActionFinished = true;
            foreach (var tuple in _tileListForSlideAction)
            {
                Tile tile = tuple.Item1;
                if (tile.IsReadyToPlayTileAction() == false)
                {
                    isSlideActionFinished = false;
                }
            }
            if (isSlideActionFinished)
            {
                FinishTask();
            }
        }

        private void MoveTilesToBottomAndPlaySlideAction()
        {
            CreateSnapshot();

            _tileBoardManager.MoveTileToBottom();
            FindMovedTileByComparingWithSnapshot();
        }

        private void CreateSnapshot()
        {
            _tileBoardSnapshot = _tileBoardManager.CreateTileBoardSnapshot();
        }

        private void FindMovedTileByComparingWithSnapshot()
        {
            for (Coord coord = new(0, 0); coord.y < _tileBoardSnapshot.Count; coord.y++)
            {
                for (coord.x = 0; coord.x < _tileBoardSnapshot[coord.y].Count; coord.x++)
                {
                    // check if snapshot tile is equal with current tile.
                    // if it is, it should be moved to bottom.
                    // so it should play slide action.

                    if (
                        _tileBoardSnapshot[coord.y][coord.x]
                        == _tileBoardManager.TileBoard[coord.y][coord.x] ||
                        _tileBoardManager.IsNullTile(_tileBoardManager.TileBoard[coord.y][coord.x])
                    )
                    {
                        continue;
                    }

                    Coord currentCoord = _tileBoardManager.GetTileCoord(
                        _tileBoardSnapshot[coord.y][coord.x]
                    );
                    _tileListForSlideAction.Add(
                        (_tileBoardSnapshot[coord.y][coord.x], coord, currentCoord)
                    );
                }
            }
        }

        private void InsertNewTilesAndPlaySlideAction()
        {
            CreateSnapshot();

            _tileBoardManager.CreateNewTilesOnNullTiles();
            FindNewTileByComparingWithSnapshot();
        }

        private void FindNewTileByComparingWithSnapshot()
        {
            for (Coord coord = new(0, 0); coord.y < _tileBoardSnapshot.Count; coord.y++)
            {
                for (coord.x = 0; coord.x < _tileBoardSnapshot[coord.y].Count; coord.x++)
                {
                    // check if snapshot tile is null tile.
                    // if it is, it should be replaced with new tile.
                    // so it should play slide action.
                    if (_tileBoardManager.IsNullTile(_tileBoardSnapshot[coord.y][coord.x]))
                    {
                        Coord previousCoord = coord;
                        previousCoord.y -= _tileBoardManager.TileBoardSize.y;
                        _tileListForSlideAction.Add(
                            (
                                _tileBoardManager.TileBoard[coord.y][coord.x],
                                previousCoord,
                                coord
                            )
                        );
                    }
                }
            }
        }

        private void PlayTileSlideAction()
        {
            foreach ((Tile tile, Coord coordFrom, Coord coordTo) in _tileListForSlideAction)
            {
                Vector3 from = _tileBoardManager.TileBoardFactory.GetPositionByCoord(coordFrom);
                Vector3 to = _tileBoardManager.TileBoardFactory.GetPositionByCoord(coordTo);
                tile.PlayTileSlideToBottomAction(from, to);
            }
        }
    }
}
