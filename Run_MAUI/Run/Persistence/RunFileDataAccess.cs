using System;
using System.IO;
using System.Threading.Tasks;

namespace Run.Persistence {
    /// <summary>
    /// File handler.
    /// </summary>
    public class RunFileDataAccess : IRunDataAccess {

        private String? _directory = String.Empty;

        public RunFileDataAccess(String? saveDirectory = null) {
            _directory = saveDirectory;
        }

        /// <summary>
        /// Load file.
        /// </summary>
        /// <param name="path">Path to file.</param>
        /// <returns>The data read from the file, as follows: the table, step count, elapsed time, game difficulty</returns>
        public async Task<RunTable> LoadAsync(String path) {
            if (!String.IsNullOrEmpty(_directory))
                path = Path.Combine(_directory, path);

            try {
                using (StreamReader reader = new StreamReader(path)) // open file
                {
                    String line = await reader.ReadLineAsync() ?? String.Empty;
                    String[] numbers = line.Split(' ');
                    int tableSize = int.Parse(numbers[0]);
                    RunTable table = new RunTable(tableSize);

                    for (int i = 0; i < tableSize; i++) {
                        line = reader.ReadLine() ?? String.Empty;
                        numbers = line.Split(' ');

                        for (int j = 0; j < tableSize; j++) {
                            if (Char.Parse(numbers[j]) == 'e') {
                                table.SetValue(i, j, FieldValue.Empty);
                            }
                            else if (Char.Parse(numbers[j]) == 'c') {
                                table.SetValue(i, j, FieldValue.Chaser);

                            }
                            else if (Char.Parse(numbers[j]) == 'p') {
                                table.SetValue(i, j, FieldValue.Player);
                            }
                            else if (Char.Parse(numbers[j]) == 'b') {
                                table.SetValue(i, j, FieldValue.Bomb);
                            }
                        }
                    }
                    return table;
                }
            }
            catch {
                throw new RunDataException();
            }
        }

        /// <summary>
        /// Save file.
        /// </summary>
        /// <param name="path">Path to file.</param>
        /// <param name="table">The table to be saved.</param>
        /// <param name="steps">The number of steps made by the player.</param>
        /// <param name="time">Time elapsed since beginning of game.</param>
        public async Task SaveAsync(String path, RunTable table) {
            if (!String.IsNullOrEmpty(_directory))
                path = Path.Combine(_directory, path);

            try {
                using (StreamWriter writer = new StreamWriter(path)) // fájl megnyitása
                {
                    writer.Write(table.Size);
                    await writer.WriteLineAsync();

                    for (int i = 0; i < table.Size; i++) {
                        for (int j = 0; j < table.Size; j++) {
                            if (table[i, j] == FieldValue.Empty) { await writer.WriteAsync("e "); }
                            else if (table[i, j] == FieldValue.Chaser) { await writer.WriteAsync("c "); }
                            else if (table[i, j] == FieldValue.Player) { await writer.WriteAsync("p "); }
                            else if (table[i, j] == FieldValue.Bomb) { await writer.WriteAsync("b "); }
                        }
                        await writer.WriteLineAsync();
                    }
                }
            }
            catch {
                throw new RunDataException();
            }
        }
    }
}
