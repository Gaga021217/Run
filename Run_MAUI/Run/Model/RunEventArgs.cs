using System;

namespace Run.Model {
    /// <summary>
    /// Run event arguments type.
    /// </summary>
    public class RunEventArgs {
        private int _gameTime;
        private int _steps;
        private bool _isWon;

        /// <summary>
        /// Getting the elapsed time of the game.
        /// </summary>
        public int GameTime { get { return _gameTime; } }

        /// <summary>
        /// Getting the number of steps.
        /// </summary>
        public int GameStepCount { get { return _steps; } }

        /// <summary>
        /// Getting the victory.
        /// </summary>
        public bool IsWon { get { return _isWon; } }

        /// <summary>
        /// Constructor of the RunEventArgs.
        /// </summary>
        /// <param name="isWon">Whether the game has benn won.</param>
        /// <param name="gameStepCount">Number of steps.</param>
        /// <param name="gameTime">Elapsed time.</param>
        public RunEventArgs(bool isWon, int gameStepCount, int gameTime) {
            _isWon = isWon;
            _steps = gameStepCount;
            _gameTime = gameTime;
        }
    }
}
