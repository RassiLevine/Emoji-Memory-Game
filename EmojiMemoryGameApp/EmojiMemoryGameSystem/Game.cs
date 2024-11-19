using System.Reflection;
using static System.Formats.Asn1.AsnWriter;

namespace EmojiMemoryGameSystem
{
    public class Game
    {
        public List<EmojiCard> lstemojicards { get; private set; }
        private List<string> lstemojis = new() { "😊", "😂", "😍", "😒", "😘", "😁", "😢", "😜", "😎", "😉", "😊", "😂", "😍", "😒", "😘", "😁", "😢", "😜", "😎", "😉" };
        public EmojiCard cardone;
        public EmojiCard cardtwo;
        public bool gamestarted = false;
        public bool nomatch = true;
        public enum GameStatesEnum { Playing, GameOver, Winner, NotStarted}; //maybe dont need
        public enum PlayerUpEnum { playerone, playertwo };
        public enum WinnerEnum { PlayerOne, PlayerTwo, Tie};
        public int numtimesclicked { get; set; } = 0;
        public int nonvisible { get; set; } = 0;
        private string _winner;
        public string winner { get => _winner; set { _winner = _winnerenum.ToString(); } }
        public int scoreone { get; private set; } = 0;
        public int scoretwo { get; private set; } = 0;
        private PlayerUpEnum _playerup = PlayerUpEnum.playerone;
        private GameStatesEnum gamestatus = GameStatesEnum.NotStarted;
        private WinnerEnum _winnerenum = WinnerEnum.Tie;


        //public Game()
        public void StartGame()
        {
            gamestatus = GameStatesEnum.Playing;
            _playerup = PlayerUpEnum.playerone;
            numtimesclicked  =0;
            nonvisible = 0; //not sure if i need
            lstemojicards = new List<EmojiCard>();
            InitializeGameCards();
        }

        private void InitializeGameCards()
        {
            List<string> lstshuffledemojis = new List<string>(lstemojis);
            lstemojis.AddRange(lstshuffledemojis);
            Random rnd = new();
            lstemojis = lstemojis.OrderBy(lst => rnd.Next().ToString()).ToList();
            foreach(var emoji in lstshuffledemojis)
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
                gamestatus = GameStatesEnum.GameOver; //maybo dont need
                
            }
            else if (scoreone == 8 || scoretwo == 8)
            {
                //MessageBox.Show(winner);
                gamestatus = GameStatesEnum.GameOver; //maybo dont need
                
            }
        }

        private void SwitchTurn()
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
            //cardone = null;  need to add in somehow
            //cardtwo = null;   need to add in somehow
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

        private void CheckForMatch()
        {
            if (cardone == cardtwo)
            {
//                MessageBox.Show("YOU GOT A MATCH!");
                //cardone.Visible = false;
                //cardtwo.Visible = false;
                nonvisible = nonvisible + 2;

                if (_playerup == PlayerUpEnum.playerone)
                {
                    //int number = 0;
                    //bool prs = int.TryParse(txtScore1.Text, out number);
                    //txtScore1.Text = (number + 1).ToString();
                    _winnerenum = WinnerEnum.PlayerOne;
                    scoreone = scoreone + 1;
                }
                else if (_playerup == PlayerUpEnum.playertwo)
                {
                    //int number2 = 0;
                    //bool prs2 = int.TryParse(txtScore2.Text, out number2);
                    //txtScore2.Text = (number2 + 1).ToString();
                    _winnerenum = WinnerEnum.PlayerTwo;
                    scoretwo = scoretwo + 1;
                }
            }
            else if (cardone != cardtwo)
            {
                //MessageBox.Show("NO MATCH!");
                cardone = null;
                cardtwo = null;
                nomatch = true;
            }
        }


    }

}
