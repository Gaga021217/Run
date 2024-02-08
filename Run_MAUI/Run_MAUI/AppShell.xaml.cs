using Run.Model;
using Run.Persistence;
using Run_MAUI.View;
using Run_MAUI.ViewModel;

namespace Run_MAUI;

public partial class AppShell : Shell {
    #region Fields

    private IRunDataAccess _runDataAccess;
    private readonly RunGameModel _runGameModel;
    private readonly RunViewModel _runViewModel;

    private readonly IDispatcherTimer _timer;

    private readonly IStore _store;
    private readonly StoredGameBrowserModel _storedGameBrowserModel;
    private readonly StoredGameBrowserViewModel _storedGameBrowserViewModel;

    #endregion

    #region Application methods

    public AppShell(IStore runStore,
        IRunDataAccess runDataAccess,
        RunGameModel runGameModel,
        RunViewModel runViewModel) {

        InitializeComponent();

        _store = runStore;
        _runDataAccess = runDataAccess;
        _runGameModel = runGameModel;
        _runViewModel = runViewModel;

        _timer = Dispatcher.CreateTimer();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += (_, _) => _runGameModel.AdvanceTime();

        _runGameModel.GameOver += RunGameModel_GameOver;

        _runViewModel.StartGame += RunViewModel_StartGame;
        _runViewModel.NewGame += RunViewModel_NewGame;
        _runViewModel.LoadGame += RunViewModel_LoadGame;
        _runViewModel.SaveGame += RunViewModel_SaveGame;
        _runViewModel.ExitGame += RunViewModel_ExitGame;

        _storedGameBrowserModel = new StoredGameBrowserModel(_store);
        _storedGameBrowserViewModel = new StoredGameBrowserViewModel(_storedGameBrowserModel);
        _storedGameBrowserViewModel.GameLoading += StoredGameBrowserViewModel_GameLoading;
        _storedGameBrowserViewModel.GameSaving += StoredGameBrowserViewModel_GameSaving;
    }

    #endregion

    #region Internal methods

    internal void StartTimer() => _timer.Start();

    internal void StopTimer() => _timer.Stop();

    #endregion

    #region Model event handlers

    private async void RunGameModel_GameOver(object? sender, RunEventArgs e) {
        StopTimer();

        if (e.IsWon) {
            await DisplayAlert("Menekülj! játék",
                "Gratulálok, győztél!" + Environment.NewLine +
                "Összesen " + e.GameStepCount + " lépést tettél meg és " +
                TimeSpan.FromSeconds(e.GameTime).ToString("g") + " ideig játszottál.",
                "OK");
        }
        else {
            await DisplayAlert("Menekülj! játék", "Sajnálom, vesztettél!", "OK");
        }
    }

    #endregion

    #region ViewModel event handlers

    private async void RunViewModel_StartGame(object? sender, EventArgs e) {
        await Navigation.PushAsync(new GamePage {
            BindingContext = _runViewModel
        });
    }

    private void RunViewModel_NewGame(object? sender, EventArgs e) {
        _runGameModel.NewGame();

        StartTimer();
    }

    private async void RunViewModel_LoadGame(object? sender, EventArgs e) {
        await _storedGameBrowserModel.UpdateAsync();
        await Navigation.PushAsync(new LoadGamePage {
            BindingContext = _storedGameBrowserViewModel
        });
    }

    private async void RunViewModel_SaveGame(object? sender, EventArgs e) {
        await _storedGameBrowserModel.UpdateAsync();
        await Navigation.PushAsync(new SaveGamePage {
            BindingContext = _storedGameBrowserViewModel
        });
    }

    private async void RunViewModel_ExitGame(object? sender, EventArgs e) {
        await Navigation.PushAsync(new SettingsPage {
            BindingContext = _runViewModel
        });
    }


    private async void StoredGameBrowserViewModel_GameLoading(object? sender, StoredGameEventArgs e) {
        await Navigation.PopAsync();

        try {
            await _runGameModel.LoadGameAsync(e.Name);

            await Navigation.PopAsync();
            await DisplayAlert("Menekülj! játék", "Sikeres betöltés.", "OK");

            StartTimer();
        }
        catch {
            await DisplayAlert("Menekülj! játék", "Sikertelen betöltés.", "OK");
        }
    }

    private async void StoredGameBrowserViewModel_GameSaving(object? sender, StoredGameEventArgs e) {
        await Navigation.PopAsync();
        StopTimer();

        try {
            await _runGameModel.SaveGameAsync(e.Name);
            await DisplayAlert("Menekülj! játék", "Sikeres mentés.", "OK");
        }
        catch {
            await DisplayAlert("Menekülj! játék", "Sikertelen mentés.", "OK");
        }
    }

    #endregion
}