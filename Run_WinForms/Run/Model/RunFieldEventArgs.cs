using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Run.Model {
    /// <summary>
    /// Event argument type of the Run field.
    /// </summary>
    public class RunFieldEventArgs : EventArgs {
        private Int32 _changedFieldX;
        private Int32 _changedFieldY;

        /// <summary>
        /// Getting the X coordinate of the changed field.
        /// </summary>
        public Int32 X { get { return _changedFieldX; } }

        /// <summary>
        /// Getting the Y coordinate of the changed field.
        /// </summary>
        public Int32 Y { get { return _changedFieldY; } }

        /// <summary>
        /// Constructor of the RunFieldEventArgs class.
        /// </summary>
        /// <param name="x">X coordinate of the changed field.</param>
        /// <param name="y">Y coordinate of the changed field. </param>
        public RunFieldEventArgs(Int32 x, Int32 y) {
            _changedFieldX = x;
            _changedFieldY = y;
        }
    }
}
