using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Run.Model;
using Run.Persistence;

namespace Run.ViewModel {
    /// <summary>
    /// Run viewmodel type
    /// </summary>
    public class RunViewModel : ViewModelBase {
        #region Fields

        private RunGameModel _model;

        #endregion

        #region Properties

        /// <summary>
        /// Getting command for new game
        /// </summary>
        public DelegateCommand NewGameCommand { get; private set; }

        /// <summary>
        /// Getting command for loading game
        /// </summary>
        public DelegateCommand LoadGameCommand { get; private set; }

        /// <summary>
        /// Getting command for saving game
        /// </summary>
        public DelegateCommand SaveGameCommand { get; private set; }

        /// <summary>
        /// Getting command for moving player
        /// </summary>
        public DelegateCommand MoveCommand { get; private set; }

        /// <summary>
        /// Getting command for quitting game
        /// </summary>
        public DelegateCommand ExitCommand { get; private set; }

        /// <summary>
        /// Getting collection of fields
        /// </summary>
        public ObservableCollection<RunField> Fields { get; set; }

        /// <summary>
        /// Getting step count
        /// </summary>
        public Int32 GameStepCount { get { return _model.GameStepCount; } }

        /// <summary>
        /// Getting elapsed times
        /// </summary>
        public String GameTime { get { return TimeSpan.FromSeconds(_model.GameTime).ToString("g"); } }

        /// <summary>
        /// Getting or setting state of easy difficulty.
        /// </summary>
        public Boolean IsGameEasy {
            get { return _model.GameDifficulty == GameDifficulty.Easy; }
            set {
                if (_model.GameDifficulty == GameDifficulty.Easy)
                    return;

                _model.GameDifficulty = GameDifficulty.Easy;
                OnPropertyChanged(nameof(IsGameEasy));
                OnPropertyChanged(nameof(IsGameMedium));
                OnPropertyChanged(nameof(IsGameHard));
            }
        }

        /// <summary>
        /// Getting or setting state of medium difficulty.
        /// </summary>
        public Boolean IsGameMedium {
            get { return _model.GameDifficulty == GameDifficulty.Medium; }
            set {
                if (_model.GameDifficulty == GameDifficulty.Medium)
                    return;

                _model.GameDifficulty = GameDifficulty.Medium;
                OnPropertyChanged(nameof(IsGameEasy));
                OnPropertyChanged(nameof(IsGameMedium));
                OnPropertyChanged(nameof(IsGameHard));
            }
        }

        /// <summary>
        /// Getting or setting state of hard difficulty.
        /// </summary>
        public Boolean IsGameHard {
            get { return _model.GameDifficulty == GameDifficulty.Hard; }
            set {
                if (_model.GameDifficulty == GameDifficulty.Hard)
                    return;

                _model.GameDifficulty = GameDifficulty.Hard;
                OnPropertyChanged(nameof(IsGameEasy));
                OnPropertyChanged(nameof(IsGameMedium));
                OnPropertyChanged(nameof(IsGameHard));
            }
        }

        /// <summary>
        /// Getting or setting small size of game
        /// </summary>
        public Boolean IsGameSizeSmall {
            get { return _model.GameSize == 11; }
            set {
                if (_model.GameSize == 11)
                    return;
                _model.GameSize = 11;
                OnPropertyChanged(nameof(IsGameSizeSmall));
                OnPropertyChanged(nameof(IsGameSizeMedium));
                OnPropertyChanged(nameof(IsGameSizeLarge));
            }
        }

        /// <summary>
        /// Getting or setting medium size of game
        /// </summary>
        public Boolean IsGameSizeMedium {
            get { return _model.GameSize == 15; }
            set {
                if (_model.GameSize == 15)
                    return;
                _model.GameSize = 15;
                OnPropertyChanged(nameof(IsGameSizeSmall));
                OnPropertyChanged(nameof(IsGameSizeMedium));
                OnPropertyChanged(nameof(IsGameSizeLarge));
            }
        }

        /// <summary>
        /// Getting or setting large size of game
        /// </summary>
        public Boolean IsGameSizeLarge {
            get { return _model.GameSize == 21; }
            set {
                if (_model.GameSize == 21)
                    return;
                _model.GameSize = 21;
                OnPropertyChanged(nameof(IsGameSizeSmall));
                OnPropertyChanged(nameof(IsGameSizeMedium));
                OnPropertyChanged(nameof(IsGameSizeLarge));
            }
        }

        public Int32 GameSize {
            get {
                return _model.Table.Size;
            }
            private set { }
        }

        #endregion

        #region Events

        /// <summary>
        /// New game event
        /// </summary>
        public event EventHandler? NewGame;

        /// <summary>
        /// Loading saved game event
        /// </summary>
        public event EventHandler? LoadGame;

        /// <summary>
        /// Save game event
        /// </summary>
        public event EventHandler? SaveGame;

        /// <summary>
        /// Exit game event
        /// </summary>
        public event EventHandler? ExitGame;

        /// <summary>
        /// Moving player event
        /// </summary>
        public event EventHandler? MovePlayer;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="model">The model.</param>
        public RunViewModel(RunGameModel model) {
            // connecting to the model
            _model = model;
            _model.TableChanged += new EventHandler<RunFieldEventArgs>(Model_TableChanged);
            _model.GameAdvanced += new EventHandler<RunEventArgs>(Model_GameAdvanced);
            _model.GameOver += new EventHandler<RunEventArgs>(Model_GameOver);
            _model.GameCreated += new EventHandler<RunEventArgs>(Model_GameCreated);

            // handling events
            NewGameCommand = new DelegateCommand(param => OnNewGame());
            LoadGameCommand = new DelegateCommand(param => OnLoadGame());
            SaveGameCommand = new DelegateCommand(param => OnSaveGame());
            ExitCommand = new DelegateCommand(param => OnExitGame());
            MoveCommand = new DelegateCommand(param => {
                if (param is String str)
                    OnKeyDown(str);
            });
            
            // creating game table
            Fields = new ObservableCollection<RunField>();
            for (Int32 i = 0; i < GameSize; i++) { // init fields
                for (Int32 j = 0; j < GameSize; j++) {
                    Fields.Add(new RunField {
                        Color = "White",
                        X = j,
                        Y = i});
                }
            }

            RefreshTable();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Refresh table
        /// </summary>
        private void RefreshTable() {
            OnPropertyChanged(nameof(GameSize));

            Fields.Clear();
            for (Int32 i = 0; i < GameSize; i++) { // init fields
                for (Int32 j = 0; j < GameSize; j++) {
                    Fields.Add(new RunField {
                        Color = "White",
                        X = j,
                        Y = i
                    });
                }
            }
            foreach (RunField field in Fields) // inicializálni kell a mezőket is
            {
                if (_model.Table.IsEmpty(field.X, field.Y)) {
                    field.Color = "White";
                }
                else if (_model.Table.IsBomb(field.X, field.Y)) {
                    field.Color = "Red";
                }
                else if (_model.Table.IsChaser(field.X, field.Y)) {
                    field.Color = "Yellow";
                }
                else if (_model.Table.IsPlayer(field.X, field.Y)) {
                    field.Color = "Green";
                }
            }

            OnPropertyChanged(nameof(GameTime));
            OnPropertyChanged(nameof(GameStepCount));
        }

        /// <summary>
        /// Triggering the event of moving the player (or pressing escape)
        /// </summary>
        /// <param name="key">String containing the direction the player moves (W, A, S or D) or Escape for pausing.</param>
        private void OnKeyDown(String key){
            if (key == "Escape") {
                _model.TogglePause();
                return;
            }
            _model.MovePlayer(key);
        }

        #endregion

        #region Game event handlers

        /// <summary>
        /// Játékmodell mező megváltozásának eseménykezelője.
        /// </summary>
        private void Model_TableChanged(object? sender, RunFieldEventArgs e) {
            // mező frissítése
            //RunField field = Fields.Single(f => f.X == e.X && f.Y == e.Y);

            foreach (RunField field in Fields) {
                field.Color = _model.Table.IsEmpty(field.X, field.Y) ?
                    "White" :
                    _model.Table.IsPlayer(field.X, field.Y) ?
                        "Green" :
                        _model.Table.IsChaser(field.X, field.Y) ?
                            "Yellow" :
                            "Red"; // give back the value of the field
            }

            OnPropertyChanged(nameof(GameStepCount)); // signal the change of step count
        }

        /// <summary>
        /// Event handler for game over
        /// </summary>
        private void Model_GameOver(object? sender, RunEventArgs e) {

        }

        /// <summary>
        /// Event of game advancing
        /// </summary>
        private void Model_GameAdvanced(object? sender, RunEventArgs e) {
            OnPropertyChanged(nameof(GameTime));
        }

        /// <summary>
        /// Event of game creation
        /// </summary>
        private void Model_GameCreated(object? sender, RunEventArgs e) {
            RefreshTable();
        }

        #endregion

        #region Event methods

        /// <summary>
        /// Triggering the event of starting a new game.
        /// </summary>
        private void OnNewGame() {
            
            NewGame?.Invoke(this, EventArgs.Empty);
        }



        /// <summary>
        /// Triggering the event of loading a saved game.
        /// </summary>
        private void OnLoadGame() {
            
            LoadGame?.Invoke(this, EventArgs.Empty);
            
        }

        /// <summary>
        /// Triggering the event of saving the game.
        /// </summary>
        private void OnSaveGame() {
            SaveGame?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Triggering the event of quitting the game.
        /// </summary>
        private void OnExitGame() {
            ExitGame?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
