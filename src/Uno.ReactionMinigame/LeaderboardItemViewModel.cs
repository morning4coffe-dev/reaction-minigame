using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uno.ReactionMinigame;

public class LeaderboardItemViewModel : ObservableObject
{
    private readonly LeaderboardEntry _leaderboardEntry;

    public LeaderboardItemViewModel(int order, LeaderboardEntry leaderboardEntry)
    {
        Order = order;
        _leaderboardEntry = leaderboardEntry;
    }

    public int Order { get; }

    public string Name => _leaderboardEntry.Name;

    public string Email => _leaderboardEntry.Email;

    public string Time => _leaderboardEntry.Time.TotalMilliseconds.ToString("0:00");
}
