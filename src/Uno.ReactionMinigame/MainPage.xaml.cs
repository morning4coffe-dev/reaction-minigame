using System.Diagnostics;
using System.Text.Json;
using Microsoft.UI;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Shapes;

namespace Uno.ReactionMinigame;

public sealed partial class MainPage : Page
{
    private Random _random = new Random();
    private int _score = 0;
    private int _clicksRequired = 15;
    private DateTime _startTime;
    private DispatcherQueueTimer _countdownTimer;
    private int _countdownValue;
    private LeaderboardEntry _player;
    private Stopwatch _timer = new Stopwatch();

    public MainPage()
    {
        this.InitializeComponent();
        StartCountdown();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        _player = JsonSerializer.Deserialize((string)e.Parameter, SerializerContext.Default.LeaderboardEntry);
    }

    private void StartCountdown()
    {
        _countdownValue = 3;
        CountdownTextBlock.Text = _countdownValue.ToString();
        CountdownTextBlock.Visibility = Visibility.Visible;

        _countdownTimer = DispatcherQueue.GetForCurrentThread().CreateTimer();
        _countdownTimer.Interval = TimeSpan.FromSeconds(1);
        _countdownTimer.Tick += CountdownTimer_Tick;
        _countdownTimer.Start();
    }

    private void CountdownTimer_Tick(DispatcherQueueTimer sender, object args)
    {
        _countdownValue--;

        if (_countdownValue > 0)
        {
            CountdownTextBlock.Text = _countdownValue.ToString();
        }
        else
        {
            _countdownTimer.Stop();
            CountdownTextBlock.Visibility = Visibility.Collapsed;
            StartReactionGame();
        }
    }

    private void StartReactionGame()
    {
        _timer.Start();

        _startTime = DateTime.Now;

        Progress.Maximum = _clicksRequired;
        StartNewRound();
    }

    private void StartNewRound()
    {
        var ellipse = CreateRandomEllipse();
        MyCanvas.Children.Add(ellipse);

        ellipse.PointerPressed += Ellipse_PointerPressed;
    }

    private void Ellipse_PointerPressed(object sender, PointerRoutedEventArgs e)
    {
        _score++;
        Progress.Value++;
        MyCanvas.Children.Remove(sender as Ellipse);

        if (_score == _clicksRequired)
        {
            _timer.Stop();
            DisplayFinalScore();
            return;
        }
        else
        {
            StartNewRound();
        }
    }

    private Ellipse CreateRandomEllipse()
    {
        var vh = _random.Next(20, 50);
        var ellipse = new Ellipse
        {
            Width = vh,
            Height = vh,
            Fill = new SolidColorBrush(Colors.Red)
        };

        double left = _random.NextDouble() * (MyCanvas.ActualWidth - ellipse.Width);
        double top = _random.NextDouble() * (MyCanvas.ActualHeight - ellipse.Height);

        Canvas.SetLeft(ellipse, left);
        Canvas.SetTop(ellipse, top);

        return ellipse;
    }

    private async void DisplayFinalScore()
    {
        ResultsGrid.Visibility = Visibility.Visible;
        var entry = _player with
        {
            Time = _timer.Elapsed,
        };

        FinalTimeTextBlock.Text = $"{entry.Time.TotalSeconds:F3} s";
        var leaderboardViewModel = ShellPage.Instance.LeaderboardViewModel;
        var leaderboard = leaderboardViewModel.LeaderboardEntries;
        string position = "#";
        if (leaderboard.Count == 0)
        {
            position += "1";
        }
        else
        {
            var numberOfFasterPlayers = leaderboard.Count(x => x.Time < entry.Time);
            position += (numberOfFasterPlayers + 1).ToString();
        }
        CurrentRankTextBlock.Text = position;

        await ShellPage.Instance.LeaderboardViewModel.AddTimeAsync(entry);

        MyCanvas.Children.Clear();
    }

    private void OnPlayAgainClicked(object sender, RoutedEventArgs e)
    {
        Frame.GoBack();
    }
}
