namespace myFirstApp;

public partial class CountPage : ContentPage
{
    int count = 0;
    bool isPaused = false;
    bool isCounting = false;

    public CountPage()
    {
        InitializeComponent();
    }

    private async void OnStartClicked(object sender, EventArgs e)
    {
        isCounting = true;
        StartButton.IsEnabled = false;
        PauseButton.IsEnabled = true;

        while (count < 20 && isCounting)
        {
            if (!isPaused)
            {
                count++;
                CountLabel.Text = $"Count ---------: {count}";
                await Task.Delay(500);
            }
            else
            {
                await Task.Delay(100);
            }
        }

        if (count >= 20)
        {
            StartButton.IsEnabled = true;
            PauseButton.IsEnabled = false;
            count = 0;
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
