using myFirstApp.Models;
using System.Reflection;
using System.Text.Json;

namespace myFirstApp.Views;

public partial class StudentsPage : ContentPage
{
    public StudentsPage()
    {
        InitializeComponent();
        LoadStudentData();
    }

    private async void LoadStudentData()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "myFirstApp.Data.students.json";

        using Stream stream = assembly.GetManifestResourceStream(resourceName);
        using StreamReader reader = new StreamReader(stream);
        string json = await reader.ReadToEndAsync();

        var students = JsonSerializer.Deserialize<List<Student>>(json);
        StudentsCollection.ItemsSource = students;
    }
}
