using System.Text.Json;

namespace Uno.ReactionMinigame;

public class DataStore
{
    public static async Task SaveLeaderboardEntries(LeaderboardEntry[] entries)
    {
        var data = JsonSerializer.Serialize(entries, SerializerContext.Default.LeaderboardEntryArray);
        var folder = ApplicationData.Current.LocalFolder;
        var currentTime = DateTime.Now;
        var backupFileName = "leaderboard" + currentTime.ToFileTime() +  ".json";
        var file = await folder.CreateFileAsync(backupFileName, CreationCollisionOption.ReplaceExisting);
        await FileIO.WriteTextAsync(file, data);

        var mainFileName = "leaderboard.json";
        var mainFile = await folder.CreateFileAsync(mainFileName, CreationCollisionOption.ReplaceExisting);
        await FileIO.WriteTextAsync(mainFile, data);
    }

    public static async Task<LeaderboardEntry[]> LoadLeaderboardEntries()
    {
        var folder = ApplicationData.Current.LocalFolder;
        var file = await folder.CreateFileAsync("leaderboard.json", CreationCollisionOption.OpenIfExists);
        var data = await FileIO.ReadTextAsync(file);
        if (string.IsNullOrEmpty(data))
        {
            return Array.Empty<LeaderboardEntry>();
        }
        return JsonSerializer.Deserialize(data, SerializerContext.Default.LeaderboardEntryArray) ?? Array.Empty<LeaderboardEntry>();
    }
}
