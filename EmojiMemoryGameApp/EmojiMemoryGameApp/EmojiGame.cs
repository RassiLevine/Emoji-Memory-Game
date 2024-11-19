using System.ComponentModel.Design;
using System.Diagnostics;

namespace EmojiMemoryGameApp
{
    public partial class EmojiGame : Form
    {
        bool gamestarted = false;
        List<string> lstemojis;
        List<Button> lstbuttons;

        enum PlayerUpEnum { playerone, playertwo };
        PlayerUpEnum playerup = PlayerUpEnum.playerone;

        Button cardone;
        Button cardtwo;

        int numtimesclicked =0;
        int nonvisible = 0;
        string winner;

        public EmojiGame()
        {
            InitializeComponent();
            btnStart.Click += BtnStart_Click;
            
            lstbuttons = new() { btn1, btn2, btn3 , btn4, btn5, btn6, btn7, btn8, btn9, btn10, btn11, btn12, btn13, btn14, btn15, btn16, btn17, btn18, btn19, btn20 };
            lstbuttons.ForEach(b => b.Click += B_Click);
            txtMsg.Text = "Press Start";
        
        }

        //😊😂😍😒😘😁😢😜😎😉
 
        private void CheckForWinner()
        {
            int score1;
            int score2;
            bool scoreB1 = int.TryParse(txtScore1.Text, out score1);
            bool scoreB2 = int.TryParse(txtScore2.Text, out score2);
            if(score1 > score2 == true)
            {
                winner = "The Winner is Player One";
            }
            else if (score2 > score1)
            {
                winner = "The Winner is Player Two";

            }
            else if (score1 == score2)
            {
                winner = "It's a Tie";
             
            }
            if(nonvisible == 20)
            {
                MessageBox.Show(winner);
                EndGame();
            }
            else if (txtScore1.Text == "8" || txtScore2.Text == "8")
            {
                MessageBox.Show(winner);
                EndGame();
            }
        }
        private void SwitchTurn()
        {
            if (playerup == PlayerUpEnum.playerone)
            {
                playerup = PlayerUpEnum.playertwo;
            }
            else
            {
                playerup = PlayerUpEnum.playerone;
            }

            numtimesclicked = 0;
            cardone = null;
            cardtwo = null;
        }
        private void ChooseCard(Button btn)
        {
            btn.Enabled = false;
            if (numtimesclicked < 3)
            {
                Random rnd = new();
                int n = rnd.Next(lstemojis.Count());
               if (btn.Text == "") 
               {
                    btn.Text = lstemojis[n];
                    lstemojis.RemoveAt(n);
                    Font emojifont = new("Arial", 30);
                    btn.Font = emojifont;
                    btn.ForeColor = Color.Black;
               }
               else if(btn.ForeColor == Color.Yellow)
               {
                    btn.ForeColor = Color.Black;
               }

                switch (numtimesclicked)
                {
                    case 1:
                        cardone = btn;
                        break;
                    case 2:
                        cardtwo = btn;
                        break;
                }

                if (cardtwo != null)
                {
                    CheckForMatch();
                    cardone.Enabled = true;
                    cardtwo.Enabled = true;
                }
            }
        }

        private void CheckForMatch()
        {
            if(cardone.Text == cardtwo.Text)
            {
                MessageBox.Show("YOU GOT A MATCH!");
                cardone.Visible = false;
                cardtwo.Visible = false;
                nonvisible = nonvisible + 2;

                if (playerup == PlayerUpEnum.playerone)
                {
                    int number = 0;
                    bool prs = int.TryParse(txtScore1.Text, out number);
                    txtScore1.Text = (number + 1).ToString();
                }
                else if(playerup == PlayerUpEnum.playertwo)
                {
                    int number2 = 0;
                    bool prs2 = int.TryParse(txtScore2.Text, out number2);
                    txtScore2.Text = (number2 + 1).ToString();
                }
            }
            else if(cardone.Text != cardtwo.Text)
            {
                MessageBox.Show("NO MATCH!");
                cardone.ForeColor = Color.Yellow;
                cardtwo.ForeColor = Color.Yellow;
                
            }
        }

        private void PlayerUp()
        {
           
            if (playerup == PlayerUpEnum.playerone)
            {
                txtMsg.Text = "Player One";
            }
            else if( playerup == PlayerUpEnum.playertwo)
            {
                txtMsg.Text = "Player Two";
            }
        }

        private void BtnStart_Click(object? sender, EventArgs e)
        {
            if (btnStart.Text == "START")
            {
                StartGame();
            }
            else if(btnStart.Text == "End Game")
            {
                EndGame();
            }
  
        }
        private void StartGame()
        {
            lstemojis = new() { "😊", "😂", "😍", "😒", "😘", "😁", "😢", "😜", "😎", "😉", "😊", "😂", "😍", "😒", "😘", "😁", "😢", "😜", "😎", "😉" };
            PlayerUp();
            lstbuttons.ForEach(l => l.Text = "");
            gamestarted = true;
            btnStart.Text = "End Game";

        }
        private void EndGame()
        {
            btnStart.Text = "START";
            gamestarted = false;
            txtScore1.Text = "";
            txtScore2.Text = "";
            txtMsg.Text = "Press Start";
            numtimesclicked = 0;
            nonvisible = 0;

            foreach (Button b in lstbuttons)
            {
                b.Visible = true;
                b.Text = "";
            }
        }
        private void B_Click(object? sender, EventArgs e)
        {
            if (gamestarted == true)
            {
                
                numtimesclicked = numtimesclicked +1;
                Button btn = (Button)sender;
                ChooseCard(btn);
                if (cardtwo != null)
                {
                    SwitchTurn();
                }
                PlayerUp();
                CheckForWinner();
            }
        }

    }
}
