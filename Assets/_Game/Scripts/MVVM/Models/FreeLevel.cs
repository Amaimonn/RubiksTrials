using System;

namespace Assets._Game.Scripts.MVVM.Models
{
    public class FreeLevel : AbstractLevelModel
    {
        public FreeLevel(CommonRubiksCube rubiksCube, Action onLevelExit, Action refresh) : base(rubiksCube, onLevelExit)
        {
            OnRefresh = refresh;
        }

        public override void Finish()
        {
            OnComplete?.OnNext(true);

            var gameOverViewParameters = new GameOverViewParameters("Удачи ^-^", 
                () => OnLevelExit?.OnNext(true));
            _resultsWindow?.OnNext(gameOverViewParameters);
        }

        public override void TryComplete()
        {
            if (_rubiksCube.CheckIsSolved())
            {
                OnComplete?.OnNext(true);
            }
        }

        public void RefreshPazzle()
        {
            _rubiksCube.StopAllCoroutines();
            OnRefresh();
        }

        public void MixPazzle()
        {
            RefreshPazzle();
            _rubiksCube.StartCoroutine(_rubiksCube.MixCube(35, _rubiksCube.CheckIsSolved));
        }
    }
}