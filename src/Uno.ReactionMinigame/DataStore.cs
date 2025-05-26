using System.Text.Json;
using System.Text.Json.Serialization;
using Java.Nio.FileNio.Attributes;

namespace Uno.ReactionMinigame;

public class DataStore
{
    public static async Task SaveLeaderboardEntries(LeaderboardEntry[] entries)
    {
        var data = JsonSerializer.Serialize(entries, SerializerContext.Default.LeaderboardEntry);
        var folder = ApplicationData.Current.LocalFolder;
        var currentTime = DateTime.Now;
        var fileName = "leaderboard" + currentTime.ToFileTime() +  ".json";
        var file = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
        await FileIO.WriteTextAsync(file, data);
    }

    public static async Task<LeaderboardEntry[]> LoadLeaderboardEntries(StorageFile file)
    {
        var data = await FileIO.ReadTextAsync(file);
        return JsonSerializer.Deserialize(data, SerializerContext.Default.LeaderboardEntryArray) ?? Array.Empty<LeaderboardEntry>();
    }
}
