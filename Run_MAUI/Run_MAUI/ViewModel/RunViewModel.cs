using Run.Model;
using System.Collections.ObjectModel;

namespace Run_MAUI.ViewModel {
    public class RunViewModel : ViewModelBase {
        #region Fields

        private RunGameModel _model;
        private GameSizeViewModel _gameSize;
        private GameDifficultyViewModel _difficulty = null!;
        public string GameText = "I was reached";

        #endregion

        #region Properties

        public DelegateCommand StartGameCommand { get; private set; }

        public DelegateCommand NewGameCommand { get; private set; }

        public DelegateCommand LoadGameCommand { get; private set; }

        public DelegateCommand SaveGameCommand { get; private set; }

        public DelegateCommand MoveCommand { get; private set; }

        public DelegateCommand ExitCommand { get; private set; }

        public ObservableCollection<RunField> Fields { get; set; }

        public int GameStepCount { get { return _model.GameStepCount; } }

        public String GameTime { get { return TimeSpan.FromSeconds(_model.GameTime).ToString("g"); } }

        public ObservableCollection<GameDifficultyViewModel> DifficultyLevels { get; set; }

        public GameDifficultyViewModel Difficulty {
            get => _difficulty;
            set {
                _difficulty = value;
                _model.GameDifficulty = value.Difficulty;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<GameSizeViewModel> GameSizes { get; set; }

        public GameSizeViewModel GameSize {
            get => _gameSize;
            set {
                _gameSize = value;
                _model.GameSize = (int)value.GameSize;
                OnPropertyChanged();
                OnPropertyChanged(nameof(GameTableRows));
                OnPropertyChanged(nameof(GameTableColumns));
            }
        }

        public RowDefinitionCollection GameTableRows {
            get => new RowDefinitionCollection(Enumerable.Repeat(new RowDefinition(GridLength.Star), Fields.Count).ToArray());
        }

        public ColumnDefinitionCollection GameTableColumns {
            get => new ColumnDefinitionCollection(Enumerable.Repeat(new ColumnDefinition(GridLength.Star), Fields.Count).ToArray());
        }

        #endregion

        #region Events

        public event EventHandler? NewGame;

        public event EventHandler? LoadGame;

        public event EventHandler? SaveGame;

        public event EventHandler? ExitGame;

        public event EventHandler? StartGame;

        public event EventHandler? MovePlayer;

        #endregion

        #region Constructors

        public RunViewModel(RunGameModel model) {
            _model = model;
            _model.TableChanged += new EventHandler<RunFieldEventArgs>(Model_TableChanged);
            _model.GameAdvanced += new EventHandler<RunEventArgs>(Model_GameAdvanced);
            _model.GameOver += new EventHandler<RunEventArgs>(Model_GameOver);
            _model.GameCreated += new EventHandler<RunEventArgs>(Model_GameCreated);

            StartGameCommand = new DelegateCommand(param => OnStartGame());
            NewGameCommand = new DelegateCommand(param => OnNewGame());
            LoadGameCommand = new DelegateCommand(param => OnLoadGame());
            SaveGameCommand = new DelegateCommand(param => OnSaveGame());
            ExitCommand = new DelegateCommand(param => OnExitGame());
            MoveCommand = new DelegateCommand(param => {
                if (param is String str)
                    OnKeyDown(str);
            });

            DifficultyLevels = new ObservableCollection<GameDifficultyViewModel> {
                new GameDifficultyViewModel { Difficulty = GameDifficulty.Easy },
                new GameDifficultyViewModel { Difficulty = GameDifficulty.Intermediate },
                new GameDifficultyViewModel { Difficulty = GameDifficulty.Hard }
            };
            Difficulty = DifficultyLevels[1];

            GameSizes = new ObservableCollection<GameSizeViewModel> {
                new GameSizeViewModel {GameSize = PossibleTableSizes.Small },
                new GameSizeViewModel {GameSize = PossibleTableSizes.Medium },
                new GameSizeViewModel {GameSize = PossibleTableSizes.Large },
            };
            GameSize = GameSizes[1];

            Fields = new ObservableCollection<RunField>();
            for (int i = 0; i < (int)GameSize.GameSize; i++) {
                for (int j = 0; j < (int)GameSize.GameSize; j++) {
                    Fields.Add(new RunField {
                        Color = "White",
                        X = j,
                        Y = i
                    });
                }
            }

            RefreshTable();
        }

        #endregion

        #region Private methods
        private void RefreshTable() {
            if (_model.IsPaused)
                return;
            OnPropertyChanged(nameof(GameSize));

            Fields.Clear();
            for (int i = 0; i < (int)GameSize.GameSize; i++) {
                for (int j = 0; j < (int)GameSize.GameSize; j++) {
                    Fields.Add(new RunField {
                        Color = "White",
                        X = j,
                        Y = i
                    });
                }
            }
            foreach (RunField field in Fields) {
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

        private void OnKeyDown(String key) {
            if (key == "Escape") {
                _model.TogglePause();
            }
            _model.MovePlayer(key);

        }

        #endregion

        #region Game event handlers

        private void Model_TableChanged(object? sender, RunFieldEventArgs e) {

            foreach (RunField field in Fields) {
                field.Color = _model.Table.IsEmpty(field.X, field.Y) ?
                    "White" :
                    _model.Table.IsPlayer(field.X, field.Y) ?
                        "Green" :
                        _model.Table.IsChaser(field.X, field.Y) ?
                            "Yellow" :
                            "Red";
            }

            OnPropertyChanged(nameof(GameStepCount));
        }

        private void Model_GameOver(object? sender, RunEventArgs e) {

        }

        private void Model_GameAdvanced(object? sender, RunEventArgs e) {
            OnPropertyChanged(nameof(GameTime));
        }

        private void Model_GameCreated(object? sender, RunEventArgs e) {
            RefreshTable();
        }

        #endregion

        #region Event methods

        private void OnStartGame() {
            StartGame?.Invoke(this, EventArgs.Empty);
        }

        private void OnNewGame() {
            NewGame?.Invoke(this, EventArgs.Empty);
        }

        private void OnLoadGame() {
            LoadGame?.Invoke(this, EventArgs.Empty);
        }

        private void OnSaveGame() {
            SaveGame?.Invoke(this, EventArgs.Empty);
        }

        private void OnExitGame() {
            ExitGame?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
