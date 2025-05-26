using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Storage;

namespace Uno.ReactionMinigame;

public sealed partial class LeaderboardsPage : Page
{
    public LeaderboardsPage()
    {
        this.InitializeComponent();
        LoadLeaderboards();
    }

    private async void LoadLeaderboards()
    {
        //var folder = ApplicationData.Current.LocalFolder;
        //try
        //{
        //    // Try to open the CSV file
        //    var file = await folder.GetFileAsync("users.csv");
        //    var lines = await FileIO.ReadLinesAsync(file);

        //    // Skip the header row and parse the remaining lines
        //    var leaderboardData = lines.Skip(1)
        //                                .Select(line => line.Split(','))
        //                                .Select(parts => new LeaderboardEntry
        //                                {
        //                                    Name = parts[0],
        //                                    Email = parts[1]
        //                                })
        //                                .ToList();

        //    // Bind the data to the ListView
        //    LeaderboardsListView.ItemsSource = leaderboardData;
        //}
        //catch (FileNotFoundException)
        //{
        //    // If the file doesn't exist, show a message or handle accordingly
        //    LeaderboardsListView.ItemsSource = new List<LeaderboardEntry>();
        //}
    }
}
