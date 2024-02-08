using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using Run.Model;
using Run.Persistence;
using Run.View;
using Run.ViewModel;
using Microsoft.Win32;

namespace Run_WPF {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {

        #region Fields

        private RunGameModel _model = null!;
        private RunViewModel _viewModel = null!;
        private MainWindow _view = null!;
        private DispatcherTimer _timer = null!;

        #endregion

        #region Constructors

        public App() {
            Startup += new StartupEventHandler(App_Startup);
        }

        #endregion

        #region Application event handlers

        private void App_Startup(object? sender, StartupEventArgs e) {

            _model = new RunGameModel(new RunFileDataAccess());
            _model.GameOver += new EventHandler<RunEventArgs>(Model_GameOver);
            _model.NewGame();


            _viewModel = new RunViewModel(_model);
            _viewModel.NewGame += new EventHandler(ViewModel_NewGame);
            _viewModel.ExitGame += new EventHandler(ViewModel_ExitGame);
            _viewModel.LoadGame += new EventHandler(ViewModel_LoadGame);
            _viewModel.SaveGame += new EventHandler(ViewModel_SaveGame);

            // creating view
            _view = new MainWindow();
            _view.DataContext = _viewModel;
            _view.Closing += new System.ComponentModel.CancelEventHandler(View_Closing); // eseménykezelés a bezáráshoz
            _view.Show();

            // creating timer
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += new EventHandler(Timer_Tick);
            _timer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e) {
            _model.AdvanceTime();
        }

        #endregion

        #region View event handlers

        /// <summary>
        /// Event handler for closing the view.
        /// </summary>
        private void View_Closing(object? sender, CancelEventArgs e) {
            _timer.Stop();
        }

        #endregion

        #region ViewModel event handlers

        /// <summary>
        /// Event handler of starting a new game.
        /// </summary>
        private void ViewModel_NewGame(object? sender, EventArgs e) {
            _model.NewGame();
            _timer.Start();
        }

        /// <summary>
        /// </summary>
        private async void ViewModel_LoadGame(object? sender, System.EventArgs e) {
            Boolean restartTimer = _timer.IsEnabled;

            _timer.Stop();

            try {
                OpenFileDialog openFileDialog = new OpenFileDialog(); // dialógusablak
                openFileDialog.Title = "Menekülj! tábla betöltése";
                if (openFileDialog.ShowDialog() == true) {
                    // játék betöltése
                    await _model.LoadGameAsync(openFileDialog.FileName);

                    _timer.Start();
                }
            }
            catch (RunDataException) {
                MessageBox.Show("A fájl betöltése sikertelen!", "Menekülj!", MessageBoxButton.OK, MessageBoxImage.Error);
            }


            if (restartTimer) // ha szükséges, elindítjuk az időzítőt
                _timer.Start();
        }

        /// <summary>
        /// Játék mentésének eseménykezelője.
        /// </summary>
        private async void ViewModel_SaveGame(object? sender, EventArgs e) {
            Boolean restartTimer = _timer.IsEnabled;

            _timer.Stop();

            try {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Title = "Menekülj! tábla betöltése";
                saveFileDialog.Filter = "Run|*.txt";

                if (saveFileDialog.ShowDialog() == true) {
                    try {
                        // játéktábla mentése
                        await _model.SaveGameAsync(saveFileDialog.FileName);
                    }
                    catch (RunDataException) {
                        MessageBox.Show("Játék mentése sikertelen!" + Environment.NewLine + "Hibás az elérési út, vagy a könyvtár nem írható.", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch {
                MessageBox.Show("A fájl mentése sikertelen!", "Menekülj!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (restartTimer) // ha szükséges, elindítjuk az időzítőt
                _timer.Start();
        }

        /// <summary>
        /// Játékból való kilépés eseménykezelője.
        /// </summary>
        private void ViewModel_ExitGame(object? sender, System.EventArgs e) {
            _view.Close(); // ablak bezárása
        }

        #endregion

        #region Model event handlers

        /// <summary>
        /// </summary>
        private void Model_GameOver(object? sender, RunEventArgs e) {
            _timer.Stop();

            if (e.IsWon) // győzelemtől függő üzenet megjelenítése
            {
                MessageBox.Show("Gratulálok, győztél!" + Environment.NewLine +
                                "Összesen " + e.GameStepCount + " lépést tettél meg és " +
                                TimeSpan.FromSeconds(e.GameTime).ToString("g") + " ideig játszottál.",
                                "Menekülj! játék",
                                MessageBoxButton.OK,
                                MessageBoxImage.Asterisk);
            }
            else {
                MessageBox.Show("Sajnálom, vesztettél!",
                                "Menekülj! játék",
                                MessageBoxButton.OK,
                                MessageBoxImage.Asterisk);
            }
        }

        #endregion
    
    }
}
