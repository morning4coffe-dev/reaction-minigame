
namespace Uno.ReactionMinigame;

public partial class LeaderboardViewModel : ObservableObject
{
    private readonly List<LeaderboardEntry> _leaderboardEntries = new List<LeaderboardEntry>();

    [ObservableProperty]
    public partial LeaderboardItemViewModel? CurrentBest { get; private set; }

    public async Task AddTimeAsync(LeaderboardEntry entry)
    {
        _leaderboardEntries.Add(entry);
        _leaderboardEntries.Sort((x, y) => x.Time.CompareTo(y.Time));

        await DataStore.SaveLeaderboardEntries(_leaderboardEntries.ToArray());

        Leaderboard = _leaderboardEntries.Select((entry, index) => new LeaderboardItemViewModel(index + 1, entry)).ToArray();
        CurrentBest = Leaderboard.FirstOrDefault();
    }

    internal async Task InitializeAsync()
    {
        var entries = await DataStore.LoadLeaderboardEntries();
        _leaderboardEntries.AddRange(entries);
        _leaderboardEntries.Sort((x, y) => x.Time.CompareTo(y.Time));
        Leaderboard = _leaderboardEntries.Select((entry, index) => new LeaderboardItemViewModel(index + 1, entry)).ToArray();
        CurrentBest = Leaderboard.FirstOrDefault();
    }

    [ObservableProperty]
    public partial LeaderboardItemViewModel[] Leaderboard { get; set; }

    public List<LeaderboardEntry> LeaderboardEntries => _leaderboardEntries;
}
