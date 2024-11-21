using EmojiMemoryGameSystem;
namespace MAUI_Emoji_Matching_Game;

public partial class EmojiMatching : ContentPage
{
	Game game = new();
    List<Button> lstbuttons;
    public EmojiMatching()
	{
		InitializeComponent();
		this.BindingContext = game;
        btnStart.Clicked += BtnStart_Clicked;
        game.PropertyChanged += Game_PropertyChanged;
	}

    private void BtnStart_Clicked(object sender, EventArgs e)
    {
        switch (btnStart.Text)
        {
            case "Start":
                btnStart.Text = "End Game";
                game.StartGame();
                SetButtonsValue();
                break;
            case "End Game":
                game.EndGame();
                SetButtonForEndGame();
                break;
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
        if (lstbuttons.Count < game.lstemojicards.Count)
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Cannot set up cards. The number of buttons and emoji cards do not match", "OK");
            return;
        }
        foreach (Button b in lstbuttons)
        {
            Random rnd = new();
            int rndint = rnd.Next(game.lstemojicards.Count);
            EmojiCard randomcard = game.lstemojicards[rndint];
            b.IsEnabled = true;
            b.BindingContext = randomcard;

            game.lstemojicards.RemoveAt(rndint);
        }

    }

    private async void B_Clicked(object sender, EventArgs e)
    {
        if (game.GameStatus == Game.GameStatusEnum.Playing)
        {
            if (game.numtimesclicked >= 2)
            {
                return;
            }

            game.numtimesclicked++;
            Button btn = (Button)sender;

            EmojiCard emj = btn.BindingContext as EmojiCard;
            if (emj != null)
            {
                btn.IsEnabled = false;
                game.ChooseCard(emj);

            }
        }
        if (game.CheckForWinner() == true)
        {
            await Application.Current.MainPage.DisplayAlert("WINNER!", $"The winner is {game._winnerenum}!", "OK");
            SetButtonForEndGame();
            game.EndGame();
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
        if (game.NoMatch == true)
        {
            await Application.Current.MainPage.DisplayAlert("Match Status", "No Match!", "OK");
            lstbuttons.ForEach(b => b.IsEnabled = true);
            game.HideEmoji(game.emojicardone, game.emojicardtwo);
        }
        else if (game.NoMatch == false)
        {
            await Application.Current.MainPage.DisplayAlert("Match Status", "You Got a Match!", "OK");
            game.HideEmoji(game.emojicardtwo, game.emojicardtwo);
           foreach(Button b in lstbuttons)
            {
                if(b.IsEnabled == false)
                {
                    b.IsVisible = false;
                }
            }
    }
}
}