using System.Collections.Generic;
using System.Windows;

namespace Book
{
    /// <summary>
    /// Логика взаимодействия для KeyWindow.xaml
    /// </summary>
    public partial class KeyWindow : Window
    {
        static string key;
        public KeyWindow()
        {
            InitializeComponent();
        }

        public static void Display(Dictionary<(int, int), string> dict, int size)
        {
            key = string.Empty;
            key += "  \t";
            for (int j = 0; j < size; j++)
            {
                key += j + "\t";
            }
            key += "\r\n" + new string('-', 148) + "\r\n";
            for (int i = 0; i < size; i++)
            {
                key += i + "|\t";
                for (int j = 0; j < size; j++)
                {
                    try
                    {
                        key += dict[(i, j)] + "\t";
                    }
                    catch
                    {
                        key += ' ' + "\t";
                    }
                }
                key += "\r\n";
            }
        }

        private void KeyWindow_Load(object sender, RoutedEventArgs e)
        {
            keyTB.Text = key;
        }
    }
}
