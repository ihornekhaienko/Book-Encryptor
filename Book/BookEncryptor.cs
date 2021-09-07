using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace Book
{
    class BookEncryptor
    {
        static Random random;

        static readonly string english = "abcdefghijklmnopqrstuvwxyz";
        static readonly string ukrainian = "абвгґдеєжзиіїйклмнопрстуфхцчшщьюя";
        public string[] Verse { get; set; }
        public Dictionary<(int, int), string> Key { get; set; }

        static BookEncryptor()
        {
            random = new Random();
        }

        public BookEncryptor()
        {
            Key = new Dictionary<(int, int), string>();
        }

        public BookEncryptor(string verse)
        {
            Verse = string.Join("", Regex.Matches(verse, @"[a-zA-Zа-яА-Яіїє\n]+").Cast<Match>().Select(v => v.Value).ToArray()).ToLower().Split('\n');
            Key = new Dictionary<(int, int), string>();
        }

        public void GenerateKey(int size)
        {
            if (Verse == null || size == 0)
                return;

            Key = new Dictionary<(int, int), string>();

            int i = 0, j;

            foreach (string row in Verse)
            {
                if (i == size)
                    break;
                j = 0;
                foreach (char sym in row)
                {
                    if (j == size || sym == '\n')
                        break;
                    Key.Add((i, j), sym.ToString());
                    j++;
                }
                i++;
            }

            KeyWindow.Display(Key, size);
        }

        static (int, int) RandomKey(List<(int, int)> list) => list[random.Next(list.Count)];

        public string Encrypt(string text, int language)
        {
            if (Key == null || text == string.Empty)
            {
                return text;
            }

            string buff = string.Empty;
            text = text.ToLower();

            string alphabet;
            if (language == 0)
            {
                alphabet = english;
            }
            else
            {
                alphabet = ukrainian;
            }

            foreach (var sym in text)
            {
                if (alphabet.Contains(sym))
                {
                    int idx = alphabet.IndexOf(sym);
                    for (int i = 0; i < alphabet.Length; i++)
                    {
                        if (idx == alphabet.Length)
                            idx = 0;

                        if (Key.ContainsValue(alphabet[idx].ToString()))
                        {
                            var key = RandomKey(Key.Where(k => k.Value == alphabet[idx].ToString()).Select(d => d.Key).ToList());

                            buff += key.Item1.ToString() + "/" + key.Item2.ToString() + ";";
                            break;
                        }
                        idx++;
                    }
                }
                else
                {
                    buff += sym;
                }
            }
            return buff;
        }

        string GetChar(Match match)
        {
            if (Key == null)
                return null;

            string[] key = match.Value.Trim(';').Split('/');

            int i = Convert.ToInt32(key[0]);
            int j = Convert.ToInt32(key[1]);

            try
            {
                return Key[(i, j)].ToString();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }
        public string Decrypt(string text)
        {
            if (Key == null || text == string.Empty)
            {
                return text;
            }

            MatchCollection matches = Regex.Matches(text, @"[0-9]+/[0-9]+;");
            if (matches.Count != 0)
            {
                foreach (Match match in matches)
                {
                    string ch = GetChar(match);
                    if (ch != null)
                    {
                        Regex regex = new Regex(match.Value);
                        text = regex.Replace(text, ch);
                    }
                }
            }

            return text;
        }
    }
}
