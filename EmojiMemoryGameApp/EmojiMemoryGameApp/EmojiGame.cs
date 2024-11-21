using EmojiMemoryGameSystem;
using System.Diagnostics;

namespace EmojiMemoryGameApp
{
    public partial class EmojiGame : Form
    {
        Game game = new();
        List<Button> lstbuttons;
        public EmojiCard emojicard;

        public EmojiGame()
        {
            InitializeComponent();
            
            game.PropertyChanged += Game_PropertyChanged;
            btnStart.Click += BtnStart_Click;         

            txtScore1.DataBindings.Add("Text", game, "scoreone");
            txtScore2.DataBindings.Add("Text", game, "scoretwo");
            txtMsg.Text = "Press Start";
        
        }
        private void SetButtonsValue()
        {
            lstbuttons = new() { btn1, btn2, btn3, btn4, btn5, btn6, btn7, btn8, btn9, btn10, btn11, btn12, btn13, btn14, btn15, btn16, btn17, btn18, btn19, btn20 };
            lstbuttons.ForEach(b => b.Click -= B_Click);
            lstbuttons.ForEach(b => b.Click += B_Click);
            if (lstbuttons.Count < game.lstemojicards.Count)
            {
                MessageBox.Show("cannot set up cards. the number of buttons and emoji cards do not match");
                return;
            }
            foreach(Button b in lstbuttons)
            {
                Random rnd = new();
                int rndint = rnd.Next(game.lstemojicards.Count);
                EmojiCard randomcard = game.lstemojicards[rndint];
                b.Text = randomcard.emoji;
                b.Font = new Font(Button.DefaultFont.FontFamily, 30);
                b.ForeColor = Color.Yellow;
                b.BackColor = Color.Yellow;
                b.Tag = randomcard;
                b.Enabled = true;

                game.lstemojicards.RemoveAt(rndint);
                Debug.Print($"{b.ForeColor} {b.Text} {b.Tag}");
            }
           
        }

        private void BtnStart_Click(object? sender, EventArgs e)
        {
            if (btnStart.Text == "START")
            {
                
                btnStart.Text = "End Game";
                game.StartGame();
                SetButtonsValue();
                txtMsg.Text = game.GameStatus.ToString();
            }
            else if(btnStart.Text == "End Game")
            {
                
                game.EndGame();
                foreach(Button btn in lstbuttons)
                {
                    SetButtonsForEndGame();
                    Debug.Print($"{btn.Text} {btn.Tag}");
                }
                lstbuttons.Clear();
            }
            
  
        }
        private void SetButtonsForEndGame()
        {
            btnStart.Text = "START";
            txtMsg.Text = "Press Start";
            foreach (Button btn in lstbuttons)
            {
                btn.BackColor = Color.Yellow;
                btn.ForeColor = Color.Yellow;
                btn.Text = "";
                btn.Visible = true;
                btn.Enabled = false;
                btn.Tag = null;
            }
        }
     
        private void B_Click(object? sender, EventArgs e)
      {
            if (game.GameStatus == Game.GameStatusEnum.Playing)
            {
                if (game.numtimesclicked >= 2)
                {
                    return;
                }

                game.numtimesclicked++;

                Button btn = (Button)sender;
                EmojiCard emj = btn.Tag as EmojiCard;
                if (emj != null)
                {
                    btn.Enabled = false;
                    btn.ForeColor = Color.Black;
                    game.ChooseCard(emj);
                }
            }
            if(game.CheckForWinner() == true)
            {
                MessageBox.Show($"The winner is {game._winnerenum}!");
                SetButtonsForEndGame();
                game.EndGame();
            }
        }
        private void Game_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
          {
            if(e.PropertyName == "NoMatch")
            {
                UpdateButton();
            }
        }


        private void UpdateButton()
        {
                if(game.NoMatch == true)
                {
                  MessageBox.Show("No Match");
                    foreach (Button btn in lstbuttons)
                    {
                        btn.Enabled = true;
                        btn.ForeColor = Color.Yellow;
                    }
                }
                else if(game.NoMatch == false)
                {
                  MessageBox.Show("You Got a Match!");
                    foreach (Button btn in lstbuttons)
                    {
                        if (btn.ForeColor == Color.Black)
                        {
                            btn.BackColor = Color.White;
                            btn.Visible = false;
                        }
                    }
            }
            game.HideEmoji(game.emojicardtwo, game.emojicardtwo);
        }
    }
}
