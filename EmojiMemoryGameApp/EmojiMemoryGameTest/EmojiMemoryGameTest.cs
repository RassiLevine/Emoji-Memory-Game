using EmojiMemoryGameSystem;
using System;
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
            Assert.IsTrue(game.GameStatus == Game.GameStatusEnum.Playing && game.PlayerUp == Game.PlayerUpEnum.playerone);
            TestContext.WriteLine(msg);
        }
        [Test]
        public void SetUpCards()
        {
            Game game = new();
            game.StartGame();
            string msg = $"number of cards is {game.lstemojicards.Count.ToString()}";
            Assert.IsTrue(game.lstemojicards.Count == 20);
            TestContext.WriteLine(msg);
        }
       
        
        [Test]
        public void TestMatch()
        {
            Game game = new();
            game.StartGame();
            game.CheckForMatch(new EmojiCard("😊"), new EmojiCard("😊"));
            string msg = $"match! num cards nonvisibe = {game.nonvisible.ToString()} score for {game.PlayerUp.ToString()} is {game.scoreone.ToString()}";
            Assert.IsTrue(game.nonvisible == 2 && game.PlayerUp == Game.PlayerUpEnum.playerone && game.scoreone == 1);
            TestContext.WriteLine(msg);
        }
        [Test]
        public void TestNoMatch()
        {
            Game game = new();
            game.StartGame();
            game.CheckForMatch(new EmojiCard("😊"), new EmojiCard("😍"));
            string msg = $"match! num cards nonvisibe = {game.nonvisible.ToString()} score for {game.PlayerUp.ToString()} is {game.scoreone.ToString()}";
            Assert.IsTrue(game.nonvisible == 0 && game.PlayerUp == Game.PlayerUpEnum.playerone && game.scoreone == 0);
            TestContext.WriteLine(msg);
        }
        [Test]
        public void TestSwitchTurn()
        {
            Game game = new();
            game.StartGame();
            game.SwitchTurn();
            string msg = $"current turn is {game.PlayerUp.ToString()}";
            Assert.IsTrue(game.PlayerUp == Game.PlayerUpEnum.playertwo);
            TestContext.WriteLine(msg);
        }
        [Test]
        public void TestWinner()
        {
            Game game = new();
            game.StartGame();
            game.CheckForMatch(new EmojiCard("😊"), new EmojiCard("😊")); //player one match
            game.SwitchTurn();
            game.CheckForMatch(new EmojiCard("😍"), new EmojiCard("😊")); // player two no match
            game.SwitchTurn();
            game.CheckForMatch(new EmojiCard("😊"), new EmojiCard("😊"));// player one match
            game.CheckForWinner();
            string msg = $"winner is {game._winnerenum} score playerone = {game.scoreone} score playertwo = {game.scoretwo}";
            TestContext.WriteLine(msg);
            Assert.IsTrue(game._winnerenum.ToString() == Game.WinnerEnum.PlayerOne.ToString() && game.scoreone == 2 && game.scoretwo ==0);
            
        }
    }

}