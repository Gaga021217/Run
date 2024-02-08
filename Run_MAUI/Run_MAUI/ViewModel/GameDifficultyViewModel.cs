using Run.Model;

namespace Run_MAUI.ViewModel;

public class GameDifficultyViewModel : ViewModelBase {
    private GameDifficulty _difficulty;

    public GameDifficulty Difficulty {
        get => _difficulty;
        set {
            _difficulty = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(DifficultyText));
        }
    }

    public string DifficultyText => _difficulty.ToString();
}