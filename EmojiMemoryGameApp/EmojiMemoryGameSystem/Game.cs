using System.Reflection;
using static System.Formats.Asn1.AsnWriter;

namespace EmojiMemoryGameSystem
{
    public class Game
    {
        public List<EmojiCard> lstemojicards { get; private set; }
        private List<string> lstemojis = new() { "😊", "😂", "😍", "😒", "😘", "😁", "😢", "😜", "😎", "😉" };
        public EmojiCard emojicardone;
        public EmojiCard emojicardtwo;
        public bool gamestarted = false;
        public bool nomatch = true;
        public enum GameStatusEnum { Playing, GameOver, Winner, NotStarted}; //maybe dont need
        public enum PlayerUpEnum { playerone, playertwo };
        public enum WinnerEnum { PlayerOne, PlayerTwo, Tie};
        public int numtimesclicked { get; set; } = 0;
        public int nonvisible { get; set; } = 0;
        public int scoreone { get; private set; } = 0;
        public int scoretwo { get; private set; } = 0;
        private PlayerUpEnum _playerup = PlayerUpEnum.playerone;
        private GameStatusEnum _gamestatus = GameStatusEnum.NotStarted;
        public WinnerEnum _winnerenum = WinnerEnum.Tie;

        public GameStatusEnum GameStatus { get => _gamestatus; set { _gamestatus = value; } }
        public PlayerUpEnum PlayerUp { get => _playerup; set { _playerup = value; } }
        public void StartGame()
        {
            
            _gamestatus = GameStatusEnum.Playing;
            _playerup = PlayerUpEnum.playerone;
            numtimesclicked  =0;
            nonvisible = 0; //not sure if i need
            lstemojicards = new List<EmojiCard>();
            InitializeGameCards();
        }

        public void InitializeGameCards()
        {
            lstemojicards.Clear();
            List<string> lstshuffledemojis = new List<string>(lstemojis);
            lstemojis.AddRange(lstshuffledemojis);
            Random rnd = new();
            lstemojis = lstemojis.OrderBy(lst => rnd.Next().ToString()).ToList();
            foreach(var emoji in lstemojis)
            {
                lstemojicards.Add(new EmojiCard(emoji));
            }
        }
        
        public void CheckForWinner()
        {
            if (scoreone > scoretwo == true)
            {
                _winnerenum = WinnerEnum.PlayerOne;
            }
            else if (scoretwo > scoreone)
            {
                _winnerenum = WinnerEnum.PlayerTwo;

            }
            else if (scoreone == scoretwo)
            {
                _winnerenum = WinnerEnum.Tie;

            }
            if (nonvisible == 20)
            {
                _gamestatus = GameStatusEnum.GameOver; //maybo dont need
                
            }
            else if (scoreone == 8 || scoretwo == 8)
            {
                //MessageBox.Show(winner);
                _gamestatus = GameStatusEnum.GameOver; //maybo dont need
                
            }
        }

        public void SwitchTurn()
        {
            if (_playerup == PlayerUpEnum.playerone)
            {
                _playerup = PlayerUpEnum.playertwo;
            }
            else
            {
                _playerup = PlayerUpEnum.playerone;
            }

            numtimesclicked = 0;
            emojicardone = null;  
            emojicardtwo = null;  
        }

        //private void ChooseCard(EmojiCard emoji)
        //{
        //   // btn.Enabled = false;
        //    if (numtimesclicked < 3)
        //    {
        //        Random rnd = new();
        //        int n = rnd.Next(lstemojis.Count());
        //        if (btn.Text == "")
        //        {
        //            btn.Text = lstemojis[n];
        //            lstemojis.RemoveAt(n);
        //            Font emojifont = new("Arial", 30);
        //            btn.Font = emojifont;
        //            btn.ForeColor = Color.Black;
        //        }
        //        else if (btn.ForeColor == Color.Yellow)
        //        {
        //            btn.ForeColor = Color.Black;
        //        }

        //        switch (numtimesclicked)
        //        {
        //            case 1:
        //                cardone = btn;
        //                break;
        //            case 2:
        //                cardtwo = btn;
        //                break;
        //        }

        //        if (cardtwo != null)
        //        {
        //            CheckForMatch();
        //            cardone.Enabled = true;
        //            cardtwo.Enabled = true;
        //        }
        //    }
        //}

        public void CheckForMatch(EmojiCard cardone, EmojiCard cardtwo)
        {
            emojicardone = cardone;
            emojicardtwo = cardtwo;
            if (emojicardone.emoji.ToString() == emojicardtwo.emoji.ToString())
            {             
                nonvisible = nonvisible + 2;

                if (_playerup == PlayerUpEnum.playerone)
                {
                    _winnerenum = WinnerEnum.PlayerOne;
                    scoreone = scoreone + 1;
                }
                else if (_playerup == PlayerUpEnum.playertwo)
                {
                    _winnerenum = WinnerEnum.PlayerTwo;
                    scoretwo = scoretwo + 1;
                }
            }
            else if (emojicardone.emoji.ToString() != emojicardtwo.emoji.ToString())
            {
                emojicardone = null;
                emojicardtwo = null;
                nomatch = true;
            }
        }


    }

}
