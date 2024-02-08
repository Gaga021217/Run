using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;



namespace Run.Persistence {
    /// <summary>
    /// The table of the game.
    /// </summary>
    public enum FieldValue { Empty, Bomb, Player, Chaser }

    public class WrongValueException : Exception {
        public WrongValueException(string? message) : base(message) {
        }
    }

    public class RunTable {

        #region Fields

        private FieldValue[,] _fieldValues;

        #endregion

        #region Properties

        /// <summary>
        /// Returns the size of the table.
        /// </summary>
        public int Size { get { return _fieldValues.GetLength(0); } }

        /// <summary>
        /// Returns the value of a field.
        /// </summary>
        /// <param name="x">Horizontal coordinate..</param>
        /// <param name="y">Vertical coordinate.</param>
        /// <returns>The value of the field.</returns>
        public FieldValue this[int x, int y] { get { return GetValue(x, y); } }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for Run table
        /// </summary>
        /// <param name="tableSize">Játéktábla mérete.</param>
        public RunTable(int tableSize) {
            if (tableSize < 0)
                throw new ArgumentOutOfRangeException(nameof(tableSize), "The table size is less than 0.");
            // 5 fields reserved
            // 1 for player 
            // 2 for chasers
            // 2 to make sure that the player can move (one to the left and one to the right)

            _fieldValues = new FieldValue[tableSize, tableSize];
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns whether there is a chaser on the field.
        /// </summary>
        /// <param name="x">Horizontal coordinate</param>
        /// <param name="y">Vertical koordináta.</param>
        /// <returns>True, if the field is occupied by a chaser, otherwise false.</returns>
        public bool IsChaser(int x, int y) {
            if (x < 0 || x >= Size)
                throw new ArgumentOutOfRangeException(nameof(x), "The X coordinate is out of range.");
            if (y < 0 || y >= Size)
                throw new ArgumentOutOfRangeException(nameof(y), "The Y coordinate is out of range.");

            return _fieldValues[x, y] == FieldValue.Chaser;
        }

        /// <summary>
        /// Returns whether the field has a bomb on it
        /// </summary>
        /// <param name="x">Horizontal coordinate</param>
        /// <param name="y">Vertical coordinate.</param></param>
        /// <returns>True, if the field has a bomb on it, otherwise false.</returns>
        public bool IsBomb(int x, int y) {
            if (x < 0 || x >= Size)
                throw new ArgumentOutOfRangeException(nameof(x), "The X coordinate is out of range.");
            if (y < 0 || y >= Size)
                throw new ArgumentOutOfRangeException(nameof(y), "The Y coordinate is out of range.");

            return _fieldValues[x, y] == FieldValue.Bomb;
        }

        /// <summary>
        /// Returns whether the field is occupied by a player.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>True, if the field is occupied by the player, false otherwise.</returns>
        public bool IsPlayer(int x, int y) {
            if (x < 0 || x >= Size)
                throw new ArgumentOutOfRangeException(nameof(x), "The X coordinate is out of range.");
            if (y < 0 || y >= Size)
                throw new ArgumentOutOfRangeException(nameof(y), "The Y coordinate is out of range.");
            return _fieldValues[x, y] == FieldValue.Player;
        }

        /// <summary>
        /// Returns whether the field is empty.
        /// </summary>
        /// <param name="x">Horizontal coordinate</param>
        /// <param name="y">Vertical coordinate.</param></param>
        /// <returns>True, if the field is empty, otherwise false.</returns>
        public bool IsEmpty(int x, int y) {
            if (x < 0 || x >= Size)
                throw new ArgumentOutOfRangeException(nameof(y), "The X coordinate is out of range.");
            if (y < 0 || y >= Size)
                throw new ArgumentOutOfRangeException(nameof(y), "The Y coordinate is out of range.");

            return _fieldValues[x, y] == FieldValue.Empty;
        }

        /// <summary>
        /// Returns the value of a field
        /// </summary>
        /// <param name="x">Horizontal coordinate</param>
        /// <param name="y">Vertical coordinate.</param>
        /// <returns>The value of a field.</returns>
        public FieldValue GetValue(int x, int y) {
            if (x < 0 || x >= Size)
                throw new ArgumentOutOfRangeException(nameof(x), "The X coordinate is out of range.");
            if (y < 0 || y >= Size)
                throw new ArgumentOutOfRangeException(nameof(y), "The Y coordinate is out of range.");

            return _fieldValues[x, y];
        }

        /// <summary>
        /// Sets the value of a field.
        /// </summary>
        /// <param name="x">Horizontal coordinate</param>
        /// <param name="y">Vertical coordinate.</param>
        /// <param name="value">The value to set for the field.</param>
        public void SetValue(int x, int y, FieldValue value) {
            if (x < 0 || x >= Size)
                throw new ArgumentOutOfRangeException(nameof(x), "The X coordinate is out of range.");
            if (y < 0 || y >= Size)
                throw new ArgumentOutOfRangeException(nameof(y), "The Y coordinate is out of range.");
            if (_fieldValues[x, y] == FieldValue.Empty) // ha a beállítás érvénytelen, akkor nem végezzük el
                _fieldValues[x, y] = value;

        }

        /// <summary>
        /// Returns the location of the player.
        /// </summary>
        /// <returns>The location of the player, or (-1,-1) if the player is dead (not found).</returns>
        public Tuple<int, int> PlayerLocation() {
            for (int i = 0; i < Size; i++) {
                for (int j = 0; j < Size; j++) {
                    if (IsPlayer(i, j)) {
                        return new Tuple<int, int>(i, j);
                    }
                }
            }
            return new Tuple<int, int>(-1, -1);
        }

        /// <summary>
        /// Returns the location of both chasers (or as many as there are alive).
        /// </summary>
        /// <returns>A list of tuples containing the locations of the chasers, or an empty list if there are none left.</returns>
        public List<Tuple<int, int>> ChaserLocations() {
            List<Tuple<int, int>> result = new List<Tuple<int, int>>();
            for (int i = 0; i < Size; i++) {
                for (int j = 0; j < Size; j++) {
                    if (_fieldValues[i, j] == FieldValue.Chaser) {
                        result.Add(new Tuple<int, int>(i, j));
                    }
                }

            }
            return result;
        }



        /// <summary>
        /// Move character to the left.
        /// </summary>
        /// <param name="x"> Horizontal coordinate. </param>
        /// <param name="y">Vertical coordinate.</param>
        /// <returns>True, if the movement was successful, false otherwise.</returns>
        public bool MoveLeft(int x, int y) {
            if (x - 1 < 0 || x >= Size)
                return false;
            if (y < 0 || y >= Size)
                return false;
            if (IsChaser(x - 1, y)) {
                return false;
            }
            else if (IsBomb(x - 1, y)) {
                _fieldValues[x, y] = FieldValue.Empty;
                return true;
            }
            else {
                FieldValue tmp = _fieldValues[x, y];
                _fieldValues[x, y] = FieldValue.Empty;
                _fieldValues[x - 1, y] = tmp;
                return true;
            }


        }


        /// <summary>
        /// Move character to the right.
        /// </summary>
        /// <param name="x"> Horizeontal coordinate. </param>
        /// <param name="y">Vertical coordinate.</param>
        /// <returns>True, if the movement was successful, false otherwise.</returns>
        public bool MoveRight(int x, int y) {
            if (x < 0 || x + 1 >= Size)
                return false;
            if (y < 0 || y >= Size)
                throw new ArgumentOutOfRangeException(nameof(y), "The Y coordinate is out of range.");

            if (IsChaser(x + 1, y)) {
                return false;
            }
            else if (IsBomb(x + 1, y)) {
                _fieldValues[x, y] = FieldValue.Empty;
                return true;
            }
            else {
                FieldValue tmp = _fieldValues[x, y];
                _fieldValues[x, y] = FieldValue.Empty;
                _fieldValues[x + 1, y] = tmp;
                return true;
            }

        }


        /// <summary>
        /// Move character to the up.
        /// </summary>
        /// <param name="x"> Horizontal coordinate. </param>
        /// <param name="y">Vertical coordinate.</param>
        /// <returns>True, if the movement was successful, false otherwise.</returns>
        public bool MoveUp(int x, int y) {
            if (x < 0 || x >= Size)
                return false;
            if (y - 1 < 0 || y >= Size)
                return false;
            if (IsChaser(x, y - 1)) {
                return false;
            }
            else if (IsBomb(x, y - 1)) {
                _fieldValues[x, y] = FieldValue.Empty;
                return true;
            }
            else {
                FieldValue tmp = _fieldValues[x, y];
                _fieldValues[x, y] = FieldValue.Empty;
                _fieldValues[x, y - 1] = tmp;
                return true;
            }


        }


        /// <summary>
        /// Move character down
        /// </summary>
        /// <param name="x"> Horizontal coordinate. </param>
        /// <param name="y">Vertical coordinate.</param>
        /// <returns>True, if the movement was successful, false otherwise.</returns>
        public bool MoveDown(int x, int y) {
            if (x < 0 || x >= Size)
                return false;
            if (y < 0 || y + 1 >= Size)
                return false;
            if (IsChaser(x, y + 1)) {
                return false;
            }
            else if (IsBomb(x, y + 1)) {
                _fieldValues[x, y] = FieldValue.Empty;
                return true;
            }
            else {
                FieldValue tmp = _fieldValues[x, y];
                _fieldValues[x, y] = FieldValue.Empty;
                _fieldValues[x, y + 1] = tmp;
                return true;
            }


        }

        #endregion

    }
}
