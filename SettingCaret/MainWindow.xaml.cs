using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace SettingCaret {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            CommandLineBox.Text = "\r\nTASK?>";
        }

        private void CommandLineBox_PreviewKeyDown(object sender, KeyEventArgs e) {
            var text = GetCurrentLineText();
            var t = Regex.Replace(text, @"(^[^\?]*\?>)(.*)", $"$1 {Guid.NewGuid().ToString()}");
            ReplaceCurrentLineText(t);
        }

        private void CommandLineBox_OnLoaded(object sender, RoutedEventArgs e) {
            CommandLineBox.Focus();
            CommandLineBox.CaretIndex = CommandLineBox.Text.Length;
        }

        private void ReplaceCurrentLineText(string newText) {
            var caretIndex = CommandLineBox.CaretIndex;
            var lineIndex = CommandLineBox.GetLineIndexFromCharacterIndex(caretIndex);
            var characterIndex = CommandLineBox.GetCharacterIndexFromLineIndex(lineIndex);
            var textOnLine = CommandLineBox.GetLineText(lineIndex);
            CommandLineBox.Select(characterIndex, textOnLine.Length);
            CommandLineBox.SelectedText = newText;
            CommandLineBox.Focus();
            CommandLineBox.Select(CommandLineBox.Text.Length, 0);
            CommandLineBox.ScrollToLine(lineIndex);
        }

        private string GetCurrentLineText() {
            var caretIndex = CommandLineBox.CaretIndex;
            var lineIndex = CommandLineBox.GetLineIndexFromCharacterIndex(caretIndex);
            return lineIndex >= 0 && lineIndex < CommandLineBox.LineCount ?
                CommandLineBox.GetLineText(lineIndex) : "";
        }
    }
}
