using EmojiMemoryGameSystem;
using System.Diagnostics;

namespace EmojiMemoryGameApp
{
    public partial class EmojiGame : Form
    {
        Game activegame = new();
        List<Game> lstgames = new() { new Game(), new Game(), new Game()};
        List<Button> lstbuttons;
        public EmojiCard emojicard;

        public EmojiGame()
        {
            InitializeComponent();
            //rdbGame1.Tag = lstgames[0];
            //rdbGame2.Tag = lstgames[1];
            //rdbGame3.Tag = lstgames[2];
            //activegame = lstgames[0];
            //rdbGame1.Checked = true;
            //rdbGame1.CheckedChanged += Game_CheckedChanged;
            //rdbGame2.CheckedChanged += Game_CheckedChanged;
            //rdbGame3.CheckedChanged += Game_CheckedChanged;
           
            btnStart.Click += BtnStart_Click;         

            txtScore1.DataBindings.Add("Text", activegame, "scoreone");
            txtScore2.DataBindings.Add("Text", activegame, "scoretwo");
            txtMsg.Text = "Press Start";
        
        }
        private void SetButtonsValue()
        {
            lstbuttons = new() { btn1, btn2, btn3, btn4, btn5, btn6, btn7, btn8, btn9, btn10, btn11, btn12, btn13, btn14, btn15, btn16, btn17, btn18, btn19, btn20 };
            lstbuttons.ForEach(b => b.Click -= B_Click);
            lstbuttons.ForEach(b => b.Click += B_Click);
            if (lstbuttons.Count < activegame.lstemojicards.Count)
            {
                MessageBox.Show("cannot set up cards. the number of buttons and emoji cards do not match");
                return;
            }
            foreach(Button b in lstbuttons)
            {
                Random rnd = new();
                int rndint = rnd.Next(activegame.lstemojicards.Count);
                EmojiCard randomcard = activegame.lstemojicards[rndint];
                b.Text = randomcard.emoji;
                b.Font = new Font(Button.DefaultFont.FontFamily, 30);
                b.ForeColor = Color.Yellow;
                b.BackColor = Color.Yellow;
                b.Tag = randomcard;
                b.Enabled = true;

                activegame.lstemojicards.RemoveAt(rndint);
                Debug.Print($"{b.ForeColor} {b.Text} {b.Tag}");
            }
           
        }

        private void BtnStart_Click(object? sender, EventArgs e)
        {
            if (btnStart.Text == "START")
            {
                
                btnStart.Text = "End Game";
                activegame.StartGame();
                SetButtonsValue();
                txtMsg.Text = activegame.GameStatus.ToString();
            }
            else if(btnStart.Text == "End Game")
            {
                
                activegame.EndGame();
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
            if (activegame.GameStatus == Game.GameStatusEnum.Playing)
            {
                if (activegame.numtimesclicked >= 2)
                {
                    return;
                }

                activegame.numtimesclicked++;

                Button btn = (Button)sender;
                EmojiCard emj = btn.Tag as EmojiCard;
                if (emj != null)
                {
                    btn.Enabled = false;
                    btn.ForeColor = Color.Black;
                    activegame.ChooseCard(emj);
                }
            }
            if(activegame.CheckForWinner() == true)
            {
                MessageBox.Show($"The winner is {activegame.Winner}!");
                SetButtonsForEndGame();
                activegame.EndGame();
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
                if(activegame.NoMatch == true)
                {
                  MessageBox.Show("No Match");
                    foreach (Button btn in lstbuttons)
                    {
                        btn.Enabled = true;
                        btn.ForeColor = Color.Yellow;
                    }
                }
                else if(activegame.NoMatch == false)
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
            activegame.HideEmoji(activegame.emojicardtwo, activegame.emojicardtwo);
        }
        //private void Game_CheckedChanged(object? sender, EventArgs e)
        //{
        //    if(rdbGame1.Checked)
        //    {
        //        activegame = (Game)rdbGame1.Tag;
        //    }
        //    else if (rdbGame2.Checked)
        //    {
        //        activegame = (Game)rdbGame2.Tag;
        //    }
        //    else if (rdbGame3.Checked)
        //    {
        //        activegame = (Game)rdbGame3.Tag;
        //    }
        //    activegame.PropertyChanged += Game_PropertyChanged; ///add back to initializer
        //}
    }
}
