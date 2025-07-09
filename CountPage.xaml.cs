namespace myFirstApp;

public partial class CountPage : ContentPage
{
    int count = 0;
    int startValue = 0;
    int endValue = 20;
    bool isPaused = false;
    bool isCounting = false;

    public CountPage()
    {
        InitializeComponent();
    }

    private async void OnStartClicked(object sender, EventArgs e)
    {
        // Parse input values
        if (!int.TryParse(StartEntry.Text, out startValue) || !int.TryParse(EndEntry.Text, out endValue))
        {
            await DisplayAlert("Input Error", "Please enter valid numbers for start and end.", "OK");
            return;
        }

        if (endValue <= startValue)
        {
            await DisplayAlert("Range Error", "End must be greater than Start.", "OK");
            return;
        }

        count = startValue;
        isCounting = true;
        isPaused = false;

        StartButton.IsEnabled = false;
        PauseButton.IsEnabled = true;

        while (count <= endValue && isCounting)
        {
            if (!isPaused)
            {
                CountLabel.Text = $"Count: {count}";
                count++;
                await Task.Delay(500);
            }
            else
            {
                await Task.Delay(100);
            }
        }

        if (count > endValue)
        {
            StartButton.IsEnabled = true;
            PauseButton.IsEnabled = false;
            isCounting = false;
        }
    }

    private void OnPauseClicked(object sender, EventArgs e)
    {
        isPaused = !isPaused;
        PauseButton.Text = isPaused ? "Resume" : "Pause";
    }

    private void OnResetClicked(object sender, EventArgs e)
    {
        count = 0;
        isPaused = false;
        isCounting = false;

        CountLabel.Text = "Count: 0";
        StartButton.IsEnabled = true;
        PauseButton.IsEnabled = false;
        PauseButton.Text = "Pause";
    }
}
