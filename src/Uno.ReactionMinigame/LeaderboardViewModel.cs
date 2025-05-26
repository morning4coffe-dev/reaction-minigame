namespace Uno.ReactionMinigame;

public partial class LeaderboardViewModel : ObservableObject
{
    private readonly List<LeaderboardEntry> _leaderboardEntries = new List<LeaderboardEntry>();

    [ObservableProperty]
    public partial LeaderboardItemViewModel? CurrentBest { get; private set; }

    public async Task AddTimeAsync(string name, string email, TimeSpan time)
    {
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email))
        {
            throw new ArgumentException("Name and email cannot be empty.");
        }

        _leaderboardEntries.Add(new(name, email, time));
        _leaderboardEntries.Sort((x, y) => x.Time.CompareTo(y.Time));

        await DataStore.SaveLeaderboardEntries(_leaderboardEntries.ToArray());

        Leaderboard = _leaderboardEntries.Select((entry, index) => new LeaderboardItemViewModel(index + 1, entry)).ToArray();
        CurrentBest = Leaderboard.FirstOrDefault();
    }

    [ObservableProperty]
    public partial LeaderboardItemViewModel[] Leaderboard { get; set; }
}
