using Run.Model;
using Run.Persistence;
using Run_MAUI.ViewModel;

namespace Run_MAUI;
public partial class App : Application {
    private const string SuspendedGameSavePath = "SuspendedGame";

    private readonly AppShell _appShell;
    private readonly IRunDataAccess _runDataAccess;
    private readonly RunGameModel _runGameModel;
    private readonly IStore _runStore;
    private readonly RunViewModel _runViewModel;

    public App() {
        InitializeComponent();

        _runStore = new RunStore();
        _runDataAccess = new RunFileDataAccess(FileSystem.AppDataDirectory);

        _runGameModel = new RunGameModel(_runDataAccess);
        _runViewModel = new RunViewModel(_runGameModel);

        _appShell = new AppShell(_runStore, _runDataAccess, _runGameModel, _runViewModel) {
            BindingContext = _runViewModel
        };
        MainPage = _appShell;
    }

    protected override Window CreateWindow(IActivationState? activationState) {
        Window window = base.CreateWindow(activationState);

        window.Created += (s, e) => {
            _runGameModel.NewGame();
            //_appShell.StartTimer();
        };

        window.Activated += (s, e) => {
            if (!File.Exists(Path.Combine(FileSystem.AppDataDirectory, SuspendedGameSavePath)))
                return;

            Task.Run(async () => {
                try {
                    await _runGameModel.LoadGameAsync(SuspendedGameSavePath);

                    //_appShell.StartTimer();
                }
                catch {
                }
            });
        };

        window.Deactivated += (s, e) => {
            Task.Run(async () => {
                try {
                    //_appShell.StopTimer();
                    await _runGameModel.SaveGameAsync(SuspendedGameSavePath);
                }
                catch {
                }
            });
        };

        return window;
    }
}