using System;
using System.IO;
using System.Text;

namespace Core
{
    public class Highscores
    {
        const string path = "high.score";
        private static uint highscore = 0;
        public static uint Highscore { get { return highscore; } }
        /*een simpele checksum zodat het moeilijker wordt om random
        waardes in de text file te stoppen en vals te spelen!*/
        public static void ReadScore()
        {
            if (File.Exists(path))
            {
                string text = System.IO.File.ReadAllText(path);
                string score = text.Substring(0, text.Length / 2);
                string checksum = text.Substring(text.Length / 2, text.Length / 2);
                highscore = Decrypt(score);
                uint check = Decrypt(checksum);
                uint sum = highscore - check;
                if(sum != 10)
                {
                    Console.WriteLine("corrupt");
                    WriteScore();
                }
                highscore -= 100;
            }
            else
                highscore = 0;
        }
        public static void WriteScore()
        {
            highscore += 100;
            uint checknumber = highscore - 10;
            string checksum = Encrypt(checknumber);
            string message = Encrypt(highscore) + checksum;
            System.IO.File.WriteAllText(path, message);
            highscore -= 100;
        }

        public static bool CheckHighScore(uint score)
        {
            if(score > highscore)
            {
                highscore = score;
                WriteScore();
                return true;
            }
            return false;
        }

        //Simpele encryptie zodat gebruikers niet zomaar waardes kunnen aanpassen
        public static string Encrypt(uint score)
        {
            StringBuilder src = new StringBuilder(score.ToString());
            for (int i = 0; i < src.Length; i++)
                src[i] += (char)22;
            return src.ToString();
        }

        public static string test() { return "test"; }

        public static uint Decrypt(string source)
        {
            StringBuilder src = new StringBuilder(source);
            for (int i = 0; i < source.Length; i++)
                src[i] -= (char)22;
            return Decode(src.ToString());
        }

        public static uint Decode(string source)
        {
            uint hscore = 0;
            for (int i = 0; i < source.Length; i++)
                hscore += (uint)((source[i] - 48)) * (uint)(Math.Pow(10, (source.Length - i - 1)));
            return hscore;
        }
    }
}