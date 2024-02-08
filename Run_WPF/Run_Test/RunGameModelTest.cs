using Run.Persistence;
using Run.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Run_Test {
    [TestClass]
    public class RunGameModelTest {

        private RunGameModel _model = null!;
        private RunTable _mockedTable = null!;
        private Mock<IRunDataAccess> _mock = null!;

        [TestInitialize]
        public void Initialize() {
            _mockedTable = new RunTable(11);
            for(int i = 0; i < 11; i++) {
                for(int j = 0; j < 11; j++) { 
                    _mockedTable.SetValue(i, j, FieldValue.Empty);
                    
                }
            }
            _mockedTable.SetValue(5, 0, FieldValue.Player);

            _mockedTable.SetValue(0, 10, FieldValue.Chaser);
            _mockedTable.SetValue(10, 10, FieldValue.Chaser);

            _mockedTable.SetValue(1, 10, FieldValue.Bomb);
            _mockedTable.SetValue(2, 7, FieldValue.Bomb);
            _mockedTable.SetValue(3, 6, FieldValue.Bomb);
            _mockedTable.SetValue(3, 8, FieldValue.Bomb);
            _mockedTable.SetValue(6, 7, FieldValue.Bomb);
            _mockedTable.SetValue(6, 10, FieldValue.Bomb);
            _mockedTable.SetValue(7, 7, FieldValue.Bomb);
            _mockedTable.SetValue(9, 3, FieldValue.Bomb);
            _mockedTable.SetValue(10, 1, FieldValue.Bomb);

            // előre definiálunk egy játéktáblát a perzisztencia mockolt teszteléséhez

            _mock = new Mock<IRunDataAccess>();
            _mock.Setup(mock => mock.LoadAsync(It.IsAny<String>()))
                .Returns(() => Task.FromResult(_mockedTable));

            _model = new RunGameModel(_mock.Object);
            // példányosítjuk a modellt a mock objektummal

            _model.GameAdvanced += new EventHandler<RunEventArgs>(Model_GameAdvanced);
            _model.GameOver += new EventHandler<RunEventArgs>(Model_GameOver);
        }

        [TestMethod]
        public void RunGameModelNewGameSmallEasyTest() {
            _model.GameDifficulty = GameDifficulty.Easy;
            _model.GameSize = 11;
            _model.NewGame();


            Assert.AreEqual(0, _model.GameStepCount);
            Assert.AreEqual(GameDifficulty.Easy, _model.GameDifficulty);
            Assert.AreEqual(0, _model.GameTime);
            Assert.AreEqual(11, _model.GameSize);

            Int32 bombFields = 0;
            for (Int32 i = 0; i < 11; i++)
                for (Int32 j = 0; j < 11; j++)
                    if (_model.Table.IsBomb(i, j))
                        bombFields++;

            Assert.AreEqual(17, bombFields);
        }

        [TestMethod]
        public void RunGameModelNewGameMediumMediumTest() {
            _model.GameDifficulty = GameDifficulty.Medium;
            _model.GameSize = 15;
            _model.NewGame();


            Assert.AreEqual(0, _model.GameStepCount);
            Assert.AreEqual(GameDifficulty.Medium, _model.GameDifficulty);
            Assert.AreEqual(0, _model.GameTime);
            Assert.AreEqual(15, _model.GameSize);

            Int32 bombFields = 0;
            for (Int32 i = 0; i < 15; i++)
                for (Int32 j = 0; j < 15; j++)
                    if (_model.Table.IsBomb(i, j))
                        bombFields++;

            Assert.AreEqual(12, bombFields);
        }

        [TestMethod]
        public void RunGameModelNewGameLargeHardTest() {
            _model.GameDifficulty = GameDifficulty.Hard;
            _model.GameSize = 21;
            _model.NewGame();


            Assert.AreEqual(GameDifficulty.Hard, _model.GameDifficulty);
            Assert.AreEqual(21, _model.GameSize);
            Assert.AreEqual(0, _model.GameStepCount);
            Assert.AreEqual(0, _model.GameTime);

            Int32 bombFields = 0;
            for (Int32 i = 0; i < 21; i++)
                for (Int32 j = 0; j < 21; j++)
                    if (_model.Table.IsBomb(i, j))
                        bombFields++;

            Assert.AreEqual(3, bombFields);
        }

        [TestMethod]
        public void RunGameModelPauseTest() {
            Assert.AreEqual(0, _model.GameStepCount);
            _model.NewGame();

            _model.TogglePause();

            _model.MovePlayer("S");    

            Assert.AreEqual(0, _model.GameStepCount);

            _model.TogglePause();

            _model.MovePlayer("A");

            Assert.AreEqual(1, _model.GameStepCount);
            Assert.AreEqual(0, _model.GameTime); 
        }

        [TestMethod]
        public void RunGameModelMovementTest() {
            _model.NewGame();
            //move up -> we are in the top row, so should stay in place
            Tuple<Int32, Int32> player = _model.Table.PlayerLocation();
            _model.MovePlayer("W");
            Assert.AreEqual(FieldValue.Player, _model.Table.GetValue(player.Item1, player.Item2));

            //move to the left
            player = _model.Table.PlayerLocation();
            _model.MovePlayer("A");
            Assert.AreEqual(FieldValue.Player, _model.Table.GetValue(player.Item1 - 1, player.Item2));

            //move down
            player = _model.Table.PlayerLocation();
            _model.MovePlayer("S");
            Assert.AreEqual(FieldValue.Player, _model.Table.GetValue(player.Item1, player.Item2 + 1));

            //move right
            player = _model.Table.PlayerLocation();
            _model.MovePlayer("D");
            Assert.AreEqual(FieldValue.Player, _model.Table.GetValue(player.Item1 + 1, player.Item2));

            //move up -> not in the top row anymore
            player = _model.Table.PlayerLocation();
            _model.MovePlayer("W");
            Assert.AreEqual(FieldValue.Player, _model.Table.GetValue(player.Item1, player.Item2 - 1));

        }

        [TestMethod]
        public void RunGameOverTest() {
            _model.NewGame();

            //place a bomb right below the player, to speed things up
            _model.Table.SetValue(_model.Table.PlayerLocation().Item1, _model.Table.PlayerLocation().Item2 + 1, FieldValue.Bomb);

            //walk the player into the bomb
            _model.MovePlayer("S");

            //the player is dead, so the coordinates should be -1, -1
            Assert.AreEqual(-1, _model.Table.PlayerLocation().Item1);

        }

        [TestMethod]
        public async Task RunGameModelLoadTest() {
            // kezdünk egy új játékot
            _model.NewGame();

            // majd betöltünk egy játékot
            await _model.LoadGameAsync(String.Empty);


            bool allFilled = true;
            for (Int32 i = 0; i < 3; i++) {
                for (Int32 j = 0; j < 3; j++) {
                    switch (_model.Table.GetValue(i, j)) {
                        case FieldValue.Bomb:
                            allFilled &= true;
                            break;
                        case FieldValue.Player:
                            allFilled &= true;
                            break;
                        case FieldValue.Empty:
                            allFilled &= true;
                            break;
                        case FieldValue.Chaser:
                            allFilled &= true;
                            break;
                        default:
                            allFilled &= false;
                            break;
                    }
                }
            }
            Assert.IsTrue(allFilled);

            // a lépésszám 0-ra áll vissza
            Assert.AreEqual(0, _model.GameStepCount);

            // ellenőrizzük, hogy meghívták-e a Load műveletet a megadott paraméterrel
            _mock.Verify(dataAccess => dataAccess.LoadAsync(String.Empty), Times.Once());
        }


        private void Model_GameAdvanced(Object? sender, RunEventArgs e) {
            Assert.IsTrue(_model.GameTime >= 0);
            Assert.AreEqual(_model.GameTime != 0, _model.IsGameOver); //the game can't be over before it begins

            Assert.AreEqual(e.GameStepCount, _model.GameStepCount); // a két értéknek egyeznie kell
            Assert.AreEqual(e.GameTime, _model.GameTime); // a két értéknek egyeznie kell
            Assert.IsFalse(e.IsWon); // még nem nyerték meg a játékot
        }

        private void Model_GameOver(Object? sender, RunEventArgs e) {
            Assert.IsTrue(_model.IsGameOver); // the game has to be over
            Assert.IsFalse(e.IsWon);
        }








    }
}