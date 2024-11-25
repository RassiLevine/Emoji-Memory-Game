using EmojiMemoryGameSystem;
using System.Diagnostics;
using System.Runtime.Intrinsics.Arm;
namespace MAUI_Emoji_Matching_Game;

public partial class EmojiMatching : ContentPage
{
    Game activegame = new();
    List<Button> lstbuttons;
    List<Game> lstgames = new() { new Game(), new Game(), new Game() };

    //list of all emojiscards that survices each reset of game
    List<EmojiCard> persistedlstemojicards = new();


    public EmojiMatching()
    {
        InitializeComponent();
        lstgames.ForEach(g => g.ScoreChanged += G_ScoreChanged);
        Game1rdb.BindingContext = lstgames[0];
        Game2rdb.BindingContext = lstgames[1];
        Game3rdb.BindingContext = lstgames[2];
        activegame = lstgames[0];
        this.BindingContext = activegame;
        btnStart.Clicked += BtnStart_Clicked;
        activegame.PropertyChanged += Game_PropertyChanged;
    }

    private void G_ScoreChanged(object sender, EventArgs e)
    {
        ScoreLbl.Text = Game.Score;
    }

    private void BtnStart_Clicked(object sender, EventArgs e)
    {
        if (btnStart.Text == "Start" && activegame._gamestatus == Game.GameStatusEnum.NotStarted)
        {
            btnStart.Text = "End Game";
            activegame.StartGame();

            SetButtonsValue();

        }

        else
        {
            activegame.EndGame();
            SetButtonForEndGame();
        }
    }
    private void SetButtonForEndGame()
    {
        btnStart.Text = "Start";
        foreach (Button b in lstbuttons)
        {
            EmojiCard emj = b.BindingContext as EmojiCard;
            emj.IsTextVisible = false;
            b.IsEnabled = false;
            b.IsVisible = true;
        }
    }

    private async void SetButtonsValue()
    {
      
        lstbuttons = new() { btn1, btn2, btn3, btn4, btn5, btn6, btn7, btn8, btn9, btn10, btn11, btn12, btn13, btn14, btn15, btn16, btn17, btn18, btn19, btn20 };
        lstbuttons.ForEach(b => b.Clicked -= B_Clicked);
        lstbuttons.ForEach(b => b.Clicked += B_Clicked);
        if (lstbuttons.Count < activegame.lstemojicards.Count)
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Cannot set up cards. The number of buttons and emoji cards do not match", "OK");
            return;
        }
        foreach (Button b in lstbuttons)
        {
            Random rnd = new();
            int rndint = rnd.Next(activegame.lstemojicards.Count);
            EmojiCard randomcard = activegame.lstemojicards[rndint];
            b.IsEnabled = true;
            b.IsVisible = true;
            b.BindingContext = randomcard;

            activegame.lstemojicards.Remove(randomcard);
            persistedlstemojicards.Add(randomcard);
        }
        persistedlstemojicards.ForEach(e => e.IsTextVisible = false);

    }

    private async void B_Clicked(object sender, EventArgs e)
    {
        if (activegame.GameStatus == Game.GameStatusEnum.Playing)
        {
            if (activegame.numtimesclicked >= 2)
            {
                return;
            }

            activegame.numtimesclicked++;
            Button btn = (Button)sender;

            EmojiCard emj = btn.BindingContext as EmojiCard;
            if (emj != null)
            {
                btn.IsEnabled = false;
                activegame.ChooseCard(emj);

            }
        }
        if (activegame.CheckForWinner() == true)
        {
            await Application.Current.MainPage.DisplayAlert("WINNER!", $"The winner is {activegame._winnerenum}!", "OK");
            SetButtonForEndGame();
            activegame.EndGame();
        }
    }
    private void Game_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == "NoMatch")
        {
            UpdateButton();
        }
    }

    private async void UpdateButton()
    {
        if (activegame.NoMatch == true)
        {
            await Application.Current.MainPage.DisplayAlert("Match Status", "No Match!", "OK");
            lstbuttons.ForEach(b => b.IsEnabled = true);
            activegame.HideEmoji(activegame.emojicardone, activegame.emojicardtwo);
        }
        else if (activegame.NoMatch == false)
        {
            await Application.Current.MainPage.DisplayAlert("Match Status", "You Got a Match!", "OK");
            activegame.HideEmoji(activegame.emojicardtwo, activegame.emojicardtwo);
            foreach (Button b in lstbuttons)
            {
                if (b.IsEnabled == false)
                {
                    b.IsVisible = false;
                }
            }
        }
    }

    private async void Game_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (btnStart.Text == "Start")
        {
            await Application.Current.MainPage.DisplayAlert("START", "Please press the Start button", "OK");
            //await Application.Current.MainPage.DisplayAlert("WINNER!", $"The winner is {activegame._winnerenum}!", "OK");
        }
        else
        {
            RadioButton rdb = (RadioButton)sender;
            if (rdb.IsChecked == true && rdb.BindingContext != null)
            {
                activegame = (Game)rdb.BindingContext;
                this.BindingContext = activegame;
                activegame.PropertyChanged -= Game_PropertyChanged;
                activegame.PropertyChanged += Game_PropertyChanged;
                if (activegame._gamestatus == Game.GameStatusEnum.NotStarted)
                {
                    activegame.lstemojicards = persistedlstemojicards;
                   // activegame.StartGame();
                    SetButtonsValue();
                    int i = 0;
                    int x = 0;
                    foreach (Button b in lstbuttons)
                    {
                        if (b.IsEnabled == true)
                        {
                            i++;
                            Debug.Print(i.ToString());
                        }
                    }
                    foreach (EmojiCard em in activegame.lstemojicards)
                    {
                        if (em.IsTextVisible == true)
                        {
                            x++;
                            Debug.Print(x.ToString());
                        }
                    }
                }
            }
        }
    }


}