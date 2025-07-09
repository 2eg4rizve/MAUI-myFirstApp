namespace myFirstApp;

public partial class MainPage : ContentPage
{
	string Status= "Ready";
    dynamic server;
    public MainPage()
	{
		InitializeComponent();
        server=new Dimension_Xpand_Plus_V2("COM3");
    }

    //private void OnCounterClicked(object? sender, EventArgs e)
    //{


    //       if (server.portAction())
    //       {
    //           MachineButton.Text = "Disconnect";
    //          // _serverConnected = true;
    //       }
    //       else
    //       {
    //           MachineButton.Text = "Connect";
    //           //_serverConnected = false;
    //       }
    //       SemanticScreenReader.Announce(MachineButton.Text);


    //   }

    private void OnCounterClicked(object? sender, EventArgs e)
    {
        if (server.portAction(StatusLabel))
        {
            MachineButton.Text = "Disconnect";
        }
        else
        {
            MachineButton.Text = "Connect";
        }

        SemanticScreenReader.Announce(MachineButton.Text);
    }



















}

