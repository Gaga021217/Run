using System;

namespace Run.Model {
    /// <summary>
    /// Run event arguments type.
    /// </summary>
    public class RunEventArgs {
        private Int32 _gameTime;
        private Int32 _steps;
        private Boolean _isWon;

        /// <summary>
        /// Getting the elapsed time of the game.
        /// </summary>
        public Int32 GameTime { get { return _gameTime; } }

        /// <summary>
        /// Getting the number of steps.
        /// </summary>
        public Int32 GameStepCount { get { return _steps; } }

        /// <summary>
        /// Getting the victory.
        /// </summary>
        public Boolean IsWon { get { return _isWon; } }

        /// <summary>
        /// Constructor of the RunEventArgs.
        /// </summary>
        /// <param name="isWon">Whether the game has benn won.</param>
        /// <param name="gameStepCount">Number of steps.</param>
        /// <param name="gameTime">Elapsed time.</param>
        public RunEventArgs(Boolean isWon, Int32 gameStepCount, Int32 gameTime) {
            _isWon = isWon;
            _steps = gameStepCount;
            _gameTime = gameTime;
        }
    }
}
