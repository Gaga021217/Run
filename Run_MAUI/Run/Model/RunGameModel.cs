using Run.Persistence;

namespace Run.Model {
    /// <summary>
    /// Game difficulty ENUM type.
    /// </summary>
    public enum GameDifficulty { Easy, Intermediate, Hard }
    public enum PossibleTableSizes { Small = 11, Medium = 15, Large = 21 }

    /// <summary>
    /// Run game type.
    /// </summary>
    public class RunGameModel {

        #region Constants

        private const double GameDifficultyEasy = 10 * 0.15625;
        private const double GameDifficultyMed = 5 * 0.15625;
        private const double GameDifficultyHard = 1 * 0.15625;

        #endregion

        #region Fields

        private IRunDataAccess _dataAccess;
        private GameDifficulty _gameDifficulty;
        private RunTable _table;
        private PossibleTableSizes _gameSize;
        private int _bombCount;
        private int _gameStepCount;
        private int _gameTime;
        private int _chaserSpeed;
        private bool _isPaused;
        private bool _isGameOver;

        #endregion

        #region Properties

        /// <summary>
        /// Getting the number of steps made by the player.
        /// </summary>
        public int GameStepCount { get { return _gameStepCount; } }

        /// <summary>
        /// Getting elapsed game time.
        /// </summary>
        public int GameTime { get { return _gameTime; } }

        /// <summary>
        /// Getting the game table.
        /// </summary>
        public RunTable Table { get { return _table; } }

        /// <summary>
        /// Getting Game Over.
        /// </summary>
        public bool IsGameOver { get { return _isGameOver; } }

        /// <summary>
        /// Getting or setting game difficulty.
        /// </summary>
        public GameDifficulty GameDifficulty { get { return _gameDifficulty; } set { _gameDifficulty = value; } }

        /// <summary>
        /// Getting or setting table size.
        /// </summary>
        public int GameSize { get { return (int)_gameSize; } set { _gameSize = (PossibleTableSizes)value; } }

        /// <summary>
        /// Getting whether the game is paused.
        /// </summary>
        public bool IsPaused { get { return _isPaused; } }

        #endregion

        #region Events

        /// <summary>
        /// Event of the player movement.
        /// </summary>
        public event EventHandler<RunFieldEventArgs>? TableChanged;

        /// <summary>
        /// Event of the game advancing
        /// </summary>
        public event EventHandler<RunEventArgs>? GameAdvanced;

        /// <summary>
        /// Event of the end of game
        /// </summary>
        public event EventHandler<RunEventArgs>? GameOver;

        /// <summary>
        /// Event of game creation
        /// </summary>
        public event EventHandler<RunEventArgs>? GameCreated;

        #endregion

        #region Constructor

        /// <summary>
        /// Contructor for the game model.
        /// </summary>
        /// <param name="dataAccess">Az adatelérés.</param>
        public RunGameModel(IRunDataAccess dataAccess) {
            _dataAccess = dataAccess;
            _gameDifficulty = GameDifficulty.Intermediate;
            _gameSize = PossibleTableSizes.Medium;
            _table = new RunTable((int)_gameSize);
            _gameTime = 0;
            _isGameOver = false;
            _isPaused = true;
        }

        #endregion

        #region Public game methods

        /// <summary>
        /// Starting a new game
        /// </summary>
        public void NewGame() {
            _table = new RunTable((int)_gameSize);
            _gameStepCount = 0;
            _gameTime = 0;
            _isGameOver = false;
            _isPaused = true;

            switch (_gameDifficulty) {
                case GameDifficulty.Easy:
                    _bombCount = Convert.ToInt32(GameDifficultyEasy * (int)_gameSize);
                    _chaserSpeed = 3;
                    break;
                case GameDifficulty.Intermediate:
                    _bombCount = Convert.ToInt32(GameDifficultyMed * (int)_gameSize);
                    _chaserSpeed = 2;
                    break;
                case GameDifficulty.Hard:
                    _bombCount = Convert.ToInt32(GameDifficultyHard * (int)_gameSize);
                    _chaserSpeed = 1;
                    break;
            }
            GenerateFields(_bombCount);

            OnGameCreated();

        }

        /// <summary>
        /// Advancing gametime
        /// </summary>
        public void AdvanceTime() {
            if (IsGameOver) // ha már vége, nem folytathatjuk
                return;
            if (_isPaused)
                return;

            _gameTime++;
            OnGameAdvanced();

            if (_gameTime % _chaserSpeed == 0) {
                ChasersAdvance();
            }

        }

        public void TogglePause() {
            _isPaused = !_isPaused;
        }


        /// <summary>
        /// Processing the movement of the player.
        /// </summary>
        /// <param name="direction">The direction the player moved in.</param>
        public void MovePlayer(String direction) {
            if (IsGameOver) // if the game ended, the player can't move
                return;
            if (IsPaused)
                return;

            int x = _table.PlayerLocation().Item1;
            int y = _table.PlayerLocation().Item2;

            switch (direction[0]) {
                case 'W':
                    _table.MoveUp(x, y);
                    OnFieldChanged(x, y);
                    OnFieldChanged(x, y - 1);
                    break;
                case 'A':
                    _table.MoveLeft(x, y);
                    OnFieldChanged(x, y);
                    OnFieldChanged(x - 1, y);
                    break;
                case 'S':
                    _table.MoveDown(x, y);
                    OnFieldChanged(x, y);
                    OnFieldChanged(x, y + 1);
                    break;
                case 'D':
                    _table.MoveRight(x, y);
                    OnFieldChanged(x, y);
                    OnFieldChanged(x + 1, y);
                    break;
                default:
                    return;
            }

            _gameStepCount++; // lépésszám növelés
            OnGameAdvanced();

            x = _table.PlayerLocation().Item1;

            if (x == -1) {
                _isGameOver = true;
                OnGameOver(false);
            }

        }

        /// <summary>
        /// Játék betöltése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        public async Task LoadGameAsync(String path) {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");

            _table = await _dataAccess.LoadAsync(path);
            _gameStepCount = 0;

            switch (_gameDifficulty) {
                case GameDifficulty.Easy:
                    _chaserSpeed = 3;
                    break;
                case GameDifficulty.Intermediate:
                    _chaserSpeed = 2;
                    break;
                case GameDifficulty.Hard:
                    _chaserSpeed = 1;
                    break;
            }

            OnGameCreated();

        }

        /// <summary>
        /// Játék mentése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        public async Task SaveGameAsync(String path) {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");

            await _dataAccess.SaveAsync(path, _table);
        }

        #endregion

        #region Private game methods

        /// <summary>
        /// Generating fields.
        /// </summary>
        /// <param name="bombCount">Number of bombs</param>
        private void GenerateFields(int bombCount) {
            Random random = new Random();

            //placing player
            _table.SetValue(GameSize / 2, 0, FieldValue.Player);

            //placing chasers
            _table.SetValue(GameSize - 1, GameSize - 1, FieldValue.Chaser);
            _table.SetValue(0, GameSize - 1, FieldValue.Chaser);

            //generating bombs
            for (int i = 0; i < bombCount; i++) {
                int x, y;

                do {
                    x = random.Next(_table.Size);
                    y = random.Next(_table.Size);
                }
                while (!_table.IsEmpty(x, y));

                _table.SetValue(x, y, FieldValue.Bomb);
            }
        }

        /// <summary>
        /// Chasers moving closer to the player.
        /// </summary>
        private void ChasersAdvance() {
            int px = _table.PlayerLocation().Item1;
            int py = _table.PlayerLocation().Item2;

            List<Tuple<int, int>> chasers = _table.ChaserLocations();

            for (int i = 0; i < chasers.Count; i++) {
                int cx = chasers[i].Item1;  //horizontal coordinate of the chaser
                int cy = chasers[i].Item2;  //vertical coordinate of the chaser

                //left/right bigger than up/down distance
                if (Math.Abs(px - cx) < Math.Abs(py - cy)) {
                    // player is above chaser
                    if (py < cy) {
                        _table.MoveUp(cx, cy);
                    }
                    //player is below chaser
                    else {
                        _table.MoveDown(cx, cy);
                    }
                }
                //up/down is closer than left/right
                else {
                    // player is to the left of chaser
                    if (px < cx) {
                        _table.MoveLeft(cx, cy);
                    }
                    //player is to the right of chaser
                    else {
                        _table.MoveRight(cx, cy);
                    }
                }

                if (cx >= 0 && cx < _table.Size && cy >= 0 && cy < _table.Size)
                    OnFieldChanged(cx, cy);
                if (cx - 1 > 0 && cx - 1 < _table.Size && cy > 0 && cy < _table.Size)
                    OnFieldChanged(cx - 1, cy);
                if (cx + 1 >= 0 && cx + 1 < _table.Size && cy >= 0 && cy < _table.Size)
                    OnFieldChanged(cx + 1, cy);
                if (cx >= 0 && cx < _table.Size && cy - 1 >= 0 && cy - 1 < _table.Size)
                    OnFieldChanged(cx, cy - 1);
                if (cx >= 0 && cx < _table.Size && cy + 1 >= 0 && cy + 1 < _table.Size)
                    OnFieldChanged(cx, cy + 1);


            }

            //checking if the players and chasers are still alive
            px = _table.PlayerLocation().Item1;
            int count = _table.ChaserLocations().Count();


            if (px == -1) {
                _isGameOver = true;
                OnGameOver(false);
            }
            if (count == 0) {
                _isGameOver = true;
                OnGameOver(true);
            }



        }


        #endregion

        #region Private event methods

        /// <summary>
        /// Mező változás eseményének kiváltása.
        /// <param name="x">Mező X koordináta.</param>
        /// <param name="y">Mező Y koordináta.</param>
        /// </summary>
        private void OnFieldChanged(int x, int y) {
            TableChanged?.Invoke(this, new RunFieldEventArgs(x, y));
        }

        /// <summary>
        /// Játékidő változás eseményének kiváltása.
        /// </summary>
        private void OnGameAdvanced() {
            GameAdvanced?.Invoke(this, new RunEventArgs(false, _gameStepCount, _gameTime));
        }

        /// <summary>
        /// Játék vége eseményének kiváltása.
        /// </summary>
        /// <param name="isWon">Győztünk-e a játékban.</param>
        private void OnGameOver(bool isWon) {
            GameOver?.Invoke(this, new RunEventArgs(isWon, _gameStepCount, _gameTime));
        }

        /// <summary>
        /// Játék létrehozás eseményének kiváltása.
        /// </summary>
        private void OnGameCreated() {

            GameCreated?.Invoke(this, new RunEventArgs(false, _gameStepCount, _gameTime));
        }

        #endregion
    }
}
