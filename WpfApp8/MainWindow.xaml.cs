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
using System.Windows.Shapes;

namespace WpfApp8
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isAnalyzing = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void AnalyzeButton_Click(object sender, RoutedEventArgs e)
        {
            isAnalyzing = true;
            while (isAnalyzing)
            {
                string text = new TextRange(TextInput.Document.ContentStart, TextInput.Document.ContentEnd).Text;

                AnalyzeText(text);

                await Task.Delay(1000);
            }
        }

        private async void StopButton_Click(object sender, RoutedEventArgs e)
        {
            isAnalyzing = false;
        }

        private async void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            isAnalyzing = true;
            AnalyzeButton_Click(sender, e);
        }

        private async Task<int> CountSentences(string text)
        {
            return text.Split(new[] { '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries).Length;
        }

        private async Task<int> CountWords(string text)
        {
            return text.Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length;
        }

        private async Task<int> CountQuestionSentences(string text)
        {
            return text.Split('?').Length - 1;
        }

        private async Task<int> CountExclamationSentences(string text)
        {
            return text.Split('!').Length - 1;
        }

        private async Task SaveReportToFile(string report)
        {
            string fileName = "D:\\test.txt";
            File.WriteAllText(fileName, report);
            MessageBox.Show($"Report saved to {fileName}");
            isAnalyzing = false;
        }

        private async Task AnalyzeText(string text)
        {
            int sentenceCount = await CountSentences(text) - 1; //\r\n на початку RichTextBox => -1
            int characterCount = text.Length - 2; //\r\n на початку RichTextBox => -2
            int wordCount = await CountWords(text);
            int questionSentenceCount = await CountQuestionSentences(text);
            int exclamationSentenceCount = await CountExclamationSentences(text);

            string report = "";
            if ((bool)sentencesCheckBox.IsChecked) report += $"Number of sentences: {sentenceCount}\n";
            if ((bool)charsCheckBox.IsChecked) report += $"Number of characters: {characterCount}\n";
            if ((bool)wordsCheckBox.IsChecked) report += $"Number of words: {wordCount}\n";
            if ((bool)qSentencesCheckBox.IsChecked) report += $"Number of question sentences: {questionSentenceCount}\n";
            if ((bool)eSentencesCheckBox.IsChecked) report += $"Number of exclamation sentences: {exclamationSentenceCount}";

            if (ShowReportCheckBox.IsChecked == true)
            {
                if((bool)sentencesCheckBox.IsChecked) sentencesTextBox.Text = sentenceCount.ToString();
                if ((bool)charsCheckBox.IsChecked) charsTextBox.Text = characterCount.ToString();
                if ((bool)wordsCheckBox.IsChecked) wordsTextBox.Text = wordCount.ToString();
                if ((bool)qSentencesCheckBox.IsChecked) qSentencesTextBox.Text = questionSentenceCount.ToString();
                if ((bool)eSentencesCheckBox.IsChecked) eSentencesTextBox.Text = exclamationSentenceCount.ToString();
            }
            else
            {
                await SaveReportToFile(report);
            }
        }
    }
}