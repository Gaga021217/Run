using Run.Model;

namespace Run_MAUI.ViewModel;

public class GameSizeViewModel : ViewModelBase {
    private PossibleTableSizes _gameSize;

    public PossibleTableSizes GameSize {
        get => _gameSize;
        set {
            _gameSize = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(SizeText));
        }
    }

    public string SizeText => _gameSize.ToString();
}