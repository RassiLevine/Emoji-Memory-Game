using EmojiMemoryGameSystem;
using System.Reflection.Metadata.Ecma335;

namespace EmojiMemoryGameTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void StartGame()
        {
            Game game = new();
            game.StartGame();
            string msg = $"game status is {game.GameStatus.ToString()} current turn is {game.PlayerUp.ToString()}";
            Assert.IsTrue(game.GameStatus == Game.GameStatesEnum.Playing && game.PlayerUp == Game.PlayerUpEnum.playertwo);
            TestContext.WriteLine(msg);
        }
    }
}