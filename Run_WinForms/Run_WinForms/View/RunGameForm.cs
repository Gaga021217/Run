using Run.Persistence;
using Run.Model;

namespace Run_WinForms {
    public partial class RunGameForm : Form {

        #region Fields 

        private IRunDataAccess _dataAccess = null!;
        private RunGameModel _model = null!;
        private Button[,] _buttonGrid = null!;
        private System.Windows.Forms.Timer _timer = null!;

        #endregion

        #region Constructors

        /// <summary>
        /// Contructor for the game window
        /// </summary>
        public RunGameForm() {
            InitializeComponent();

            // Creating the data access
            _dataAccess = new RunFileDataAccess();

            // Creating the model and pairing events
            _model = new RunGameModel(_dataAccess);
            _model.TableChanged += new EventHandler<RunFieldEventArgs>(Game_TableChanged);
            _model.GameAdvanced += new EventHandler<RunEventArgs>(Game_GameAdvanced);
            _model.GameOver += new EventHandler<RunEventArgs>(Game_GameOver);

            // Creating timer
            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 1000;
            _timer.Tick += new EventHandler(Timer_Tick);

            // Initiating game table and menus
            GenerateTable();
            SetupMenus();

            // Starting new game
            _model.NewGame();
            SetupTable();

            _timer.Start();
        }

        #endregion

        #region Game event handlers

        /// <summary>
        /// Handling the model's changes
        /// </summary>
        private void Game_TableChanged(object? sender, RunFieldEventArgs e) {
            if (e.X < 0 || e.X >= _buttonGrid.GetLength(0))
                return;
            if (e.Y < 0 || e.Y >= _buttonGrid.GetLength(0))
                return;
            if (_model.Table.IsEmpty(e.X, e.Y)) {
                _buttonGrid[e.X, e.Y].BackColor = Color.White;
                _buttonGrid[e.X, e.Y].Text = String.Empty;
            }
            else if (_model.Table.IsPlayer(e.X, e.Y)) {
                _buttonGrid[e.X, e.Y].BackColor = Color.Green;
                _buttonGrid[e.X, e.Y].Text = "☺";
            }
            else if (_model.Table.IsBomb(e.X, e.Y)) {
                _buttonGrid[e.X, e.Y].BackColor = Color.Red;
                _buttonGrid[e.X, e.Y].Text = "■";
            }
            else if (_model.Table.IsChaser(e.X, e.Y)) {
                _buttonGrid[e.X, e.Y].BackColor = Color.Yellow;
                _buttonGrid[e.X, e.Y].Text = "▲";
            }
        }

        /// <summary>
        /// Event handler of the game advancing
        /// </summary>
        private void Game_GameAdvanced(Object? sender, RunEventArgs e) {
            _toolLabelGameTime.Text = TimeSpan.FromSeconds(e.GameTime).ToString("g");
            _toolLabelGameSteps.Text = e.GameStepCount.ToString();
            // updating time
        }

        /// <summary>
        /// Event handler of the game ending
        /// </summary>
        private void Game_GameOver(Object? sender, RunEventArgs e) {
            _timer.Stop();

            foreach (Button button in _buttonGrid) // Disable buttons
                button.Enabled = false;

            _menuFileSaveGame.Enabled = false;

            if (e.IsWon) // Showing message based on victory
            {
                MessageBox.Show("Gratulálok, győztél!" + Environment.NewLine +
                                "Összesen " + e.GameStepCount + " lépést tettél meg és " +
                                TimeSpan.FromSeconds(e.GameTime).ToString("g") + " ideig játszottál.",
                                "Menejülj! játék",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Asterisk);
            }
            else {
                MessageBox.Show("Sajnálom, vesztettél!",
                                "Menekülj! játék",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Asterisk);
            }
        }

        #endregion

        #region Grid event handlers

        /// <summary>
        /// Event handler for the buttongrid
        /// </summary>
        private void Form1_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.W) {
                _model.MovePlayer('w');
            }
            else if (e.KeyCode == Keys.S) {
                _model.MovePlayer('s');
            }
            else if (e.KeyCode == Keys.D) {
                _model.MovePlayer('d');
            }
            else if (e.KeyCode == Keys.A) {
                _model.MovePlayer('a');
            }

            if (e.KeyCode == Keys.Escape) {
                _model.TogglePause();
                if(_model.IsPaused) {
                    _menuFileSaveGame.Enabled = true;
                    _menuFileLoadGame.Enabled = true;
                }
                else {
                    _menuFileSaveGame.Enabled = false;
                    _menuFileLoadGame.Enabled = false;
                }
            }
        }

        #endregion

        #region Menu event handlers

        /// <summary>
        /// Event handler for starting new game.
        /// </summary>
        private void _menuFileNewGame_Click(object sender, EventArgs e) {
            _menuFileSaveGame.Enabled = true;

            TablePurge();
            _model.NewGame();
            GenerateTable();
            SetupTable();
            SetupMenus();

            _timer.Start();
        }

        /// <summary>
        /// Event handler for loading game
        /// </summary>
        private async void _menuFileLoadGame_Click(Object sender, EventArgs e) { 
            Boolean restartTimer = _timer.Enabled;
            _timer.Stop();

            if (_openFileDialog.ShowDialog() == DialogResult.OK) // ha kiválasztottunk egy fájlt
            {
                try {
                    // játék betöltése
                    await _model.LoadGameAsync(_openFileDialog.FileName);
                    _menuFileSaveGame.Enabled = true;
                }
                catch (RunDataException) {
                    MessageBox.Show("Játék betöltése sikertelen!" + Environment.NewLine + "Hibás az elérési út, vagy a fájlformátum.", "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    _model.NewGame();
                    _menuFileSaveGame.Enabled = true;
                }
                TablePurge();

                GenerateTable();
                SetupTable();
                SetupMenus();
            }

            

            if (restartTimer)
                _timer.Start();
        }

        /// <summary>
        /// Játék mentésének eseménykezelője.
        /// </summary>
        private async void _menuFileSaveGame_Click(Object sender, EventArgs e) { 
            Boolean restartTimer = _timer.Enabled;
            _timer.Stop();

            if (_saveFileDialog.ShowDialog() == DialogResult.OK) {
                try {
                    // játé mentése
                    await _model.SaveGameAsync(_saveFileDialog.FileName);
                }
                catch (RunDataException) {
                    MessageBox.Show("Játék mentése sikertelen!" + Environment.NewLine + "Hibás az elérési út, vagy a könyvtár nem írható.", "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (restartTimer)
                _timer.Start();
        }

        /// <summary>
        /// Event handler for quitting.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _menuFileQuit_Click(object sender, EventArgs e) {
            Boolean restartTimer = _timer.Enabled;
            _timer.Stop();

            // megkérdezzük, hogy biztos ki szeretne-e lépni
            if (MessageBox.Show("Biztosan ki szeretne lépni?", "Sudoku játék", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                // ha igennel válaszol
                Close();
            }
            else {
                if (restartTimer)
                    _timer.Start();
            }
        }

        private void _menuSize11_Click(object sender, EventArgs e) {
            _model.GameSize = 11;
            _menuSize11.Checked = true;
            _menuSize15.Checked = false;
            _menuSize21.Checked = false;
        }

        private void _menuSize15_Click(object sender, EventArgs e) {
            _model.GameSize = 15;
            _menuSize11.Checked = false;
            _menuSize15.Checked = true;
            _menuSize21.Checked = false;
        }

        private void _menuSize21_Click(object sender, EventArgs e) {
            _model.GameSize = 21;
            _menuSize11.Checked = false;
            _menuSize15.Checked = false;
            _menuSize21.Checked = true;
        }

        private void _menuDifficultyEasy_Click(object sender, EventArgs e) {
            _model.GameDifficulty = GameDifficulty.Easy;
            _menuDifficultyEasy.Checked = true;
            _menuDifficultyMedium.Checked = false;
            _menuDifficultyHard.Checked = false;
        }

        private void _menuDifficultyMedium_Click(object sender, EventArgs e) {
            _model.GameDifficulty = GameDifficulty.Medium;
            _menuDifficultyEasy.Checked = false;
            _menuDifficultyMedium.Checked = true;
            _menuDifficultyHard.Checked = false;
        }

        private void _menuDifficultyHard_Click(object sender, EventArgs e) {
            _model.GameDifficulty = GameDifficulty.Hard;
            _menuDifficultyEasy.Checked = false;
            _menuDifficultyMedium.Checked = false;
            _menuDifficultyHard.Checked = true;
        }


        #endregion

        #region Timer event handlers

        /// <summary>
        /// Event handler for the timer
        /// </summary>
        private void Timer_Tick(Object? sender, EventArgs e) {
            _model.AdvanceTime(); // játék léptetése
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Creating new table
        /// </summary>
        private void GenerateTable() {
            Height = 50 + 50 + 31 * _model.Table.Size;
            Width = 15 + 15 + 31 * _model.Table.Size;

            _buttonGrid = new Button[_model.Table.Size, _model.Table.Size];
            for (Int32 i = 0; i < _model.Table.Size; i++)
                for (Int32 j = 0; j < _model.Table.Size; j++) {
                    _buttonGrid[i, j] = new Button();
                    _buttonGrid[i, j].Location = new Point(15 + 30 * i, 40 + 30 * j); // elhelyezkedés
                    _buttonGrid[i, j].Size = new Size(30, 30); // méret
                    _buttonGrid[i, j].Font = new Font(FontFamily.GenericSansSerif, 15); // betűtípus
                    _buttonGrid[i, j].Enabled = false; // kikapcsolt állapot
                    _buttonGrid[i, j].TabIndex = 100 + i * _model.Table.Size + j; // a gomb számát a TabIndex-ben tároljuk
                    _buttonGrid[i, j].FlatStyle = FlatStyle.Flat; // lapított stípus

                    Controls.Add(_buttonGrid[i, j]);
                    // felvesszük az ablakra a gombot
                }
        }

        /// <summary>
        /// Setting up the table.
        /// </summary>
        private void SetupTable() {
            for (Int32 i = 0; i < _buttonGrid.GetLength(0); i++) {
                for (Int32 j = 0; j < _buttonGrid.GetLength(1); j++) {

                    if (_model.Table.IsEmpty(i, j)) {
                        _buttonGrid[i, j].BackColor = Color.White;
                        _buttonGrid[i, j].Text = String.Empty;
                    }
                    else if (_model.Table.IsPlayer(i, j)) {
                        _buttonGrid[i, j].BackColor = Color.Green;
                        _buttonGrid[i, j].Text = "☺";
                    }
                    else if (_model.Table.IsBomb(i, j)) {
                        _buttonGrid[i, j].BackColor = Color.Red;
                        _buttonGrid[i, j].Text = "■";
                    }
                    else if (_model.Table.IsChaser(i, j)) {
                        _buttonGrid[i, j].BackColor = Color.Yellow;
                        _buttonGrid[i, j].Text = "▲";
                    }


                }
            }

            _toolLabelGameSteps.Text = _model.GameStepCount.ToString();
            _toolLabelGameTime.Text = TimeSpan.FromSeconds(_model.GameTime).ToString("g");
        }

        /// <summary>
        /// Removing the table from the form.
        /// </summary>
        private void TablePurge() {
            for (int i = 0; i < _buttonGrid.GetLength(0); i++) {
                for (int j = 0; j < _buttonGrid.GetLength(1); j++) {
                    Controls.Remove(_buttonGrid[i, j]);
                }
            }
        }

        /// <summary>
        /// Setting up te menu items.
        /// </summary>
        private void SetupMenus() {
            _menuDifficultyEasy.Checked = (_model.GameDifficulty == GameDifficulty.Easy);
            _menuDifficultyMedium.Checked = (_model.GameDifficulty == GameDifficulty.Medium);
            _menuDifficultyHard.Checked = (_model.GameDifficulty == GameDifficulty.Hard);
            _menuSize11.Checked = (_model.GameSize == 11);
            _menuSize15.Checked = (_model.GameSize == 15);
            _menuSize21.Checked = (_model.GameSize == 21);

        }

        #endregion

    }
}