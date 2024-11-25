using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
namespace EmojiMemoryGameSystem
{
    public class Game : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler? ScoreChanged;

        private PlayerUpEnum _playerup = PlayerUpEnum.playerone;
        public GameStatusEnum _gamestatus = GameStatusEnum.NotStarted;
        public WinnerEnum _winnerenum = WinnerEnum.Tie;
        private bool _nomatch;
        private int _scoreone = 0;
        private int _scoretwo = 0;
        private static int playeronewins;
        private static int playertwowins;
        private static int scoreties;
        
        public List<EmojiCard> lstemojicards { get; private set; }
        private List<string> lstemojis;
        public EmojiCard emojicardone;
        public EmojiCard emojicardtwo;

        public enum GameStatusEnum { Playing, GameOver, Winner, NotStarted };
        public enum PlayerUpEnum { playerone, playertwo };
        public enum WinnerEnum { PlayerOne, PlayerTwo, Tie };
        public int numtimesclicked { get; set; } = 0;
        public int nonvisible { get; set; } = 0;

        public bool NoMatch
        {
            get => _nomatch;
            set
            {
                _nomatch = value;
                this.InvokePropertyChanged("NoMatch");
            }
        }
        public int scoreone
        {
            get => _scoreone; private set
            {
                _scoreone = value;
                Debug.WriteLine($"scoreONE change to {value}");
                this.InvokePropertyChanged("scoreone");
            }
        }
        public int scoretwo
        {
            get => _scoretwo; private set
            {
                _scoretwo = value;
                Debug.WriteLine($"scoreTWO change to {value}");
                this.InvokePropertyChanged("scoretwo");
            }
        }
        public GameStatusEnum GameStatus { get => _gamestatus; set { _gamestatus = value; } }
        public PlayerUpEnum PlayerUp { get => _playerup; set { _playerup = value; } }

        public string Score { get => $"Player One wins = {playeronewins}; Player Two wins = {playertwowins}; Ties = {scoreties}"; }


        public void StartGame()
        {
            emojicardone = null;
            emojicardtwo = null;
            lstemojis = new() { "😊", "😂", "😍", "😒", "😘", "😁", "😢", "😜", "😎", "😉" };
            _gamestatus = GameStatusEnum.Playing;
            _playerup = PlayerUpEnum.playerone;
            numtimesclicked = 0;
            nonvisible = 0;
            lstemojicards = new List<EmojiCard>();
            InitializeGameCards();
        }

        private void InitializeGameCards()
        {
            int i = 0;
            lstemojicards.Clear();
            List<string> lstshuffledemojis = new List<string>(lstemojis);
            lstemojis.AddRange(lstshuffledemojis);
            Random rnd = new();
            lstemojis = lstemojis.OrderBy(lst => rnd.Next().ToString()).ToList();
            foreach (var emoji in lstemojis)
            {
                lstemojicards.Add(new EmojiCard(emoji));
            }
            foreach(EmojiCard e in lstemojicards)
            {
                e.IsEnabled = true;
                e.IsTextVisible = false;
                i++;
                Debug.Print(i.ToString());
            }
        }

        public bool CheckForWinner()
        {
            bool gameover = false;
            if (nonvisible == 20)
            {
                _gamestatus = GameStatusEnum.GameOver;

            }
            else if (scoreone == 8 || scoretwo == 8)
            {
                _gamestatus = GameStatusEnum.GameOver;
            }
            if (_gamestatus == GameStatusEnum.GameOver)
            {
                gameover = true;
                if (scoreone > scoretwo == true)
                {
                    _winnerenum = WinnerEnum.PlayerOne;
                    playeronewins++;
                }
                else if (scoretwo > scoreone)
                {
                    _winnerenum = WinnerEnum.PlayerTwo;
                    playertwowins++;
                }
                else if (scoreone == scoretwo)
                {
                    _winnerenum = WinnerEnum.Tie;
                    scoreties++;
                }
            }
            ScoreChanged?.Invoke(this, new EventArgs());
            return gameover;

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
        }

        public void ChooseCard(EmojiCard emoji)
        {
            switch (numtimesclicked)
            {
                case 1:
                    emojicardone = emoji;
                    break;
                case 2:
                    emojicardtwo = emoji;
                    break;
            }
            emoji.IsTextVisible = true;
            if (emojicardtwo != null)
            {
                CheckForMatch(emojicardone, emojicardtwo);
                SwitchTurn();
            }
        }

        public void CheckForMatch(EmojiCard cardone, EmojiCard cardtwo)
        {
            emojicardone = cardone;
            emojicardtwo = cardtwo;
            if (emojicardone.emoji.ToString() == emojicardtwo.emoji.ToString())
            {
                nonvisible = nonvisible + 2;

                if (_playerup == PlayerUpEnum.playerone)
                {
                    scoreone = scoreone + 1;
                }
                else if (_playerup == PlayerUpEnum.playertwo)
                {
                    scoretwo = scoretwo + 1;
                }
                NoMatch = false;

            }
            else if (emojicardone.emoji.ToString() != emojicardtwo.emoji.ToString())
            {
                NoMatch = true;
            }
            numtimesclicked = 0;
        }
        public void HideEmoji(EmojiCard emoji1, EmojiCard emoji2)
        {
            emoji1.IsTextVisible = false;
            emoji2.IsTextVisible = false;
            emojicardone = null;
            emojicardtwo = null;
        }
        public void EndGame()
        {
            GameStatus = GameStatusEnum.GameOver;
            numtimesclicked = 0;
            nonvisible = 0;
            scoreone = 0;
            scoretwo = 0;
            lstemojicards.ForEach(e => e.IsTextVisible = false);
            lstemojicards.Clear();
            lstemojis.Clear();
            emojicardone = null;
            emojicardtwo = null;

        }

        private void InvokePropertyChanged([CallerMemberName] string propertyname = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

    }

}
