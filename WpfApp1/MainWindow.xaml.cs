using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void startButton_Click(object sender, RoutedEventArgs e)
        {
            string sourceDirectory = sourceTextBox.Text;
            string destinationDirectory = destinationTextBox.Text;

            if (!string.IsNullOrEmpty(sourceDirectory) || string.IsNullOrEmpty(destinationDirectory))
            {
                await CheckAndMoveDuplicatesAsync(sourceDirectory, destinationDirectory);
            }
        }

        private async Task CheckAndMoveDuplicatesAsync(string sourceDirectory, string destinationDirectory)
        {
            reportTextBox.Text = "";

            await Task.Run(() =>
            {
                try
                {
                    var sourceFiles = Directory.GetFiles(sourceDirectory).Select(Path.GetFileName).ToList();
                    var destinationFiles = Directory.GetFiles(destinationDirectory).Select(Path.GetFileName).ToList();

                    var duplicateFiles = sourceFiles.Intersect(destinationFiles).ToList();

                    foreach (var file in sourceFiles)
                    {
                        string sourceFilePath = Path.Combine(sourceDirectory, file);
                        string destinationFilePath = Path.Combine(destinationDirectory, file);

                        if (!duplicateFiles.Contains(file))
                        {
                            File.Move(sourceFilePath, destinationFilePath);
                            WriteReport($"Moved original file: {file}");
                        }
                        else
                        {
                            WriteReport($"Skipped duplicate file: {file}");
                        }
                    }

                    MessageBox.Show("Duplicate check completed.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            });
        }

        private void WriteReport(string reportText)
        {
            Dispatcher.Invoke(() =>
            {
                if (reportTextBox.Text != "") reportTextBox.Text += "\n";
                reportTextBox.Text += reportText;
            });
        }
    }
}