﻿using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace myFirstApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new NavigationPage(new MainPage()); // <== wrap with NavigationPage
    }
}
