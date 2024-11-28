using EmojiMemoryGameSystem;
using System.Diagnostics;
namespace MAUI_Emoji_Matching_Game;

public partial class EmojiMatching : ContentPage
{
    Game activegame = new();
    List<Button> lstbuttons;
    List<Game> lstgames = new() { new Game(), new Game(), new Game() };
    List<EmojiCard> persistedlstemojicards = new();
    List<EmojiCard> lstpersistedhiddenemoji = new();


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
        if (btnStart.Text == "Start" && (activegame.GameStatus == Game.GameStatusEnum.NotStarted || activegame.GameStatus == Game.GameStatusEnum.GameOver))
        {
            SetBoardForStartGame();
        }

        else
        {  
            SetBoardForEndGame(); 
        }
    }
    private void SetBoardForStartGame()
    {
        btnStart.Text = "End Game";
        activegame.StartGame();
        SetButtonsValue();
        Game1rdb.IsEnabled = true;
        Game2rdb.IsEnabled = true;
        Game3rdb.IsEnabled = true;
    }
    private void SetBoardForEndGame()
    {
        activegame.EndGame();
        btnStart.Text = "Start";
        foreach (Button b in lstbuttons)
        {
            EmojiCard emj = b.BindingContext as EmojiCard;
            emj.IsTextVisible = false;
            b.IsEnabled = false;
            b.IsVisible = true;
        }
        Game1rdb.IsEnabled = false;
        Game2rdb.IsEnabled = false;
        Game3rdb.IsEnabled = false;
    }
    

    private async void SetButtonsValue()
    {
        persistedlstemojicards.Clear();
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
            await Application.Current.MainPage.DisplayAlert("WINNER!", $"The winner is {activegame.Winner}!", "OK");
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
        else  if (activegame.NoMatch == false)
        {
            await Application.Current.MainPage.DisplayAlert("Match Status", "You Got a Match!", "OK");
            activegame.lsthiddenemojicard.Add(activegame.emojicardone);
            activegame.lsthiddenemojicard.Add(activegame.emojicardtwo);
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
            RadioButton rdb = (RadioButton)sender;
            if (rdb.IsChecked == true && rdb.BindingContext != null)
            {
                activegame = (Game)rdb.BindingContext;
                this.BindingContext = activegame;
                activegame.PropertyChanged -= Game_PropertyChanged;
                activegame.PropertyChanged += Game_PropertyChanged;
                if (activegame._gamestatus == Game.GameStatusEnum.NotStarted)
                {
                    activegame.GameStatus = Game.GameStatusEnum.Playing;
                    activegame.lsthiddenemojicard = new List<EmojiCard>(lstpersistedhiddenemoji);
                    activegame.lstemojicards = new List<EmojiCard> ( persistedlstemojicards);
                    activegame.lsthiddenemojicard.Clear();
                    SetButtonsValue();
                }
            foreach(Button b in lstbuttons)
            {
                EmojiCard emj = b.BindingContext as EmojiCard;
                b.IsVisible = true;
                b.IsEnabled = true;
                emj.IsTextVisible = false;
                if (emj != null && activegame.lsthiddenemojicard.Contains(emj))
                {
                    b.IsVisible = false;
                    b.IsEnabled = false;
                    emj.IsTextVisible = false;
                }

            }

        }
        }
    }
