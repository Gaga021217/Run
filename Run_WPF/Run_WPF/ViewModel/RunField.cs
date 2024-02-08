using Run.ViewModel;
using Run.Model;
using Run.Persistence;
using System;

namespace Run.ViewModel {
    /// <summary>
    /// Run field type.
    /// </summary>
    public class RunField : ViewModelBase {
        private String _color = "White";

        /// <summary>
        /// Set of get value.
        /// </summary>
        public String Color {
            get { return _color; }
            set {
                if (_color != value) {
                    _color = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Getting or setting horizontal coordinate
        /// </summary>
        public Int32 X { get; set; }

        /// <summary>
        /// Getting or setting vertical coordinate
        /// </summary>
        public Int32 Y { get; set; }

        /// <summary>
        /// Getting the coordinates.
        /// </summary>
        public Tuple<Int32, Int32> XY {
            get { return new(X, Y); }
        }

        /// <summary>
        /// Getting or setting command to move player
        /// </summary>
        public DelegateCommand? MoveCommand { get; set; }
    }
}
