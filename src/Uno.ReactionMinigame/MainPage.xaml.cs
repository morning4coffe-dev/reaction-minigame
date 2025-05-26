using Microsoft.UI;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Shapes;

namespace Uno.ReactionMinigame;

public sealed partial class MainPage : Page
{
    private Random _random = new Random();
    private int _score = 0;
    private int _clicksRequired = 25;
    private DispatcherTimer _gameTimer;
    private DateTime _startTime;
    private bool _gameOver = false;
    private DispatcherQueueTimer _countdownTimer;
    private int _countdownValue;

    public MainPage()
    {
        this.InitializeComponent();
        StartCountdown();
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
        _gameTimer = new DispatcherTimer();
        _gameTimer.Interval = TimeSpan.FromMilliseconds(10);
        _gameTimer.Tick += OnTimerTick;
        _gameTimer.Start();

        _startTime = DateTime.Now;

        Progress.Maximum = _clicksRequired;
        ScoreTextBlock.Visibility = Visibility.Visible;
        StartNewRound();
    }

    private void OnTimerTick(object sender, object e)
    {
        if (!_gameOver)
        {
            if (_score >= _clicksRequired)
            {
                _gameOver = true;
                _gameTimer.Stop();
                DisplayFinalScore();
            }
            else
            {
                UpdateElapsedTime();
            }
        }
    }

    private void StartNewRound()
    {
        if (_gameOver)
        {
            return;
        }

        var ellipse = CreateRandomEllipse();
        MyCanvas.Children.Add(ellipse);

        ellipse.PointerPressed += Ellipse_PointerPressed;
    }

    private void UpdateElapsedTime()
    {
        var elapsedTime = DateTime.Now - _startTime;
        ScoreTextBlock.Text = $"Time: {elapsedTime.TotalSeconds:F3} sec";
    }

    private void Ellipse_PointerPressed(object sender, PointerRoutedEventArgs e)
    {
        if (_gameOver)
        {
            return;
        }

        _score++;
        Progress.Value++;
        MyCanvas.Children.Remove(sender as Ellipse);

        StartNewRound();
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

    private void DisplayFinalScore()
    {
        var timeTaken = DateTime.Now - _startTime;
        var finalScoreText = new TextBlock
        {
            Text = $"Finished!\nTime Taken: {timeTaken.TotalSeconds:F2} seconds\nFinal Score: {_score}",
            FontSize = 24,
            Foreground = new SolidColorBrush(Colors.Black),
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(0, 50, 0, 0)
        };

        // Store the score globally to be used in SignInPage
        var app = (App)Application.Current;
        app.Score = _score; // Assuming _score is the current score

        ScoreTextBlock.Visibility = Visibility.Collapsed;

        MyCanvas.Children.Clear();
        MyCanvas.Children.Add(finalScoreText);
    }

}
