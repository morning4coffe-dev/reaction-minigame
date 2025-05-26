namespace Uno.ReactionMinigame;

public sealed partial class SignInPage : Page
{
    public SignInPage()
    {
        this.InitializeComponent();
    }

    private async void OnSubmitClicked(object sender, RoutedEventArgs e)
    {
        string name = NameTextBox.Text;
        string email = EmailTextBox.Text;

        // Validate input
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
        {
            // Handle validation (maybe show a message to the user)
            return;
        }

        // Get the score from the MainPage
        int score = (int)(App.Current as App)?.Score; // Assuming you store the score globally

        // File path for CSV
        var folder = Windows.Storage.ApplicationData.Current.LocalFolder;
        var file = await folder.CreateFileAsync("users.csv", Windows.Storage.CreationCollisionOption.OpenIfExists);

        // Read all lines in the CSV file
        var lines = await FileIO.ReadLinesAsync(file);

        // If there are existing lines, update the last one with the score
        if (lines.Count > 1) // Skip the header
        {
            lines[lines.Count - 1] = $"{name},{email},{score}"; // Update the last entry
        }
        else
        {
            // If the CSV is empty (only header), add the first line
            lines.Add($"Name,Email,Score");
            lines.Add($"{name},{email},{score}");
        }

        // Write back the updated lines to the file
        await FileIO.WriteLinesAsync(file, lines);

        // Clear the textboxes after submission
        NameTextBox.Text = string.Empty;
        EmailTextBox.Text = string.Empty;

        var entry = new LeaderboardEntry
        {
            Name = name,
            Email = email,
            Time = TimeSpan.Zero,
        };

        var serialized = System.Text.Json.JsonSerializer.Serialize(entry, SerializerContext.Default.LeaderboardEntry);
        Frame.Navigate(typeof(MainPage), serialized);
    }

}
