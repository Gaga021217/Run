namespace Run_MAUI.ViewModel {
    public class RunField : ViewModelBase {
        private String _color = "White";

        public String Color {
            get { return _color; }
            set {
                if (_color != value) {
                    _color = value;
                    OnPropertyChanged();
                }
            }
        }

        public int X { get; set; }

        public int Y { get; set; }

        public Tuple<int, int> XY {
            get { return new(X, Y); }
        }

        public DelegateCommand? MoveCommand { get; set; }
    }
}
