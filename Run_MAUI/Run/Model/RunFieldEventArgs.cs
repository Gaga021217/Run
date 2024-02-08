using System;

namespace Run.Model {
    /// <summary>
    /// Event argument type of the Run field.
    /// </summary>
    public class RunFieldEventArgs : EventArgs {
        private int _changedFieldX;
        private int _changedFieldY;

        /// <summary>
        /// Getting the X coordinate of the changed field.
        /// </summary>
        public int X { get { return _changedFieldX; } }

        /// <summary>
        /// Getting the Y coordinate of the changed field.
        /// </summary>
        public int Y { get { return _changedFieldY; } }

        /// <summary>
        /// Constructor of the RunFieldEventArgs class.
        /// </summary>
        /// <param name="x">X coordinate of the changed field.</param>
        /// <param name="y">Y coordinate of the changed field. </param>
        public RunFieldEventArgs(int x, int y) {
            _changedFieldX = x;
            _changedFieldY = y;
        }
    }
}
