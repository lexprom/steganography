using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steganography
{
    class Program
    {
        public static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            StreamReader sr = new StreamReader("../../text.txt");

            Console.WriteLine("Введите слово:");
            var word = Console.ReadLine();

            byte[] wordBytes = Encoding.UTF8.GetBytes(word);
            List<string> srWords = sr.ReadToEnd().Split(new char[] {'\n'}, StringSplitOptions.RemoveEmptyEntries).ToList();

            var textWithWord = Steganography.Encoded(srWords, wordBytes);
            Console.WriteLine(textWithWord);

            var hiddenWord = Steganography.Decoded(textWithWord);
            Console.WriteLine(hiddenWord);
        }

    }
}
