using Microsoft.Win32;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Book
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static BookEncryptor bookEncryptor;
        static public int Length { get; internal set; } = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == true)
                fileTB.Text = File.ReadAllText(openFileDialog.FileName);
        }

        private void shift_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void saveAs_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
                File.WriteAllText(saveFileDialog.FileName, fileTB.Text);
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void about_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Created by\nihornekhaienko\non 01.24.21");
        }

        private void encrypt_Click(object sender, RoutedEventArgs e)
        {
            fileTB.Text = bookEncryptor.Encrypt(fileTB.Text, engRB?.IsChecked ?? false ? 0 : 1);
        }

        private void decrypt_Click(object sender, RoutedEventArgs e)
        {
            fileTB.Text = bookEncryptor.Decrypt(fileTB.Text);
        }

        private void addVerse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == true)
            {
                bookEncryptor = new BookEncryptor(File.ReadAllText(openFileDialog.FileName));
                bookEncryptor.GenerateKey(Length);
            }
        }

        private void sizeTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void sizeTB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sizeTB.Text.Length > 0)
            {
                Length = Convert.ToInt32(sizeTB.Text);
            }
        }
    }
}
