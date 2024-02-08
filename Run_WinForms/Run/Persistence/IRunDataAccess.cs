using Run.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Run.Persistence {
    /// <summary>
    /// File handler interface for the game.
    /// </summary>
    public interface IRunDataAccess {
        /// <summary>
        /// Load file.
        /// </summary>
        /// <param name="path">Path to file.</param>
        /// <returns>The playing field saved in the file.</returns>
        Task<RunTable> LoadAsync(String path);

        /// <summary>
        /// Save file.
        /// </summary>
        /// <param name="path">Path to file.</param>
        /// <param name="table">The table to be saved.</param>
        Task SaveAsync(String path, RunTable table);
    }
}
