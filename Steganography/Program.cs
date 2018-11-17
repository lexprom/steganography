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
        static void Main(string[] args)
        {
            StreamReader sr = new StreamReader("text.txt");
            Console.WriteLine("Введите слово:");
            var word = Console.ReadLine();
            byte[] wordBytes = Encoding.Default.GetBytes(word); // Преобразование слова в массив байтов
            List<string> srWords = sr.ReadToEnd().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var textWithWord = Encoded(srWords, wordBytes);
            var hiddenWord = Decoded(textWithWord);
            Console.WriteLine(hiddenWord);
        }

        static string Encoded(List<string> text, byte[] wordBytes)
        {
            string ret = text[0]; // Выходное значение - записываем первое слово
            text.RemoveAt(0); // Удаляем первое слово
            for(int j = 0; j < wordBytes.Length; j++) // Цикл по байтам
            {
                for (int i = 0; i < 8; i++)    // Цикл по битам
                {
                    if ((wordBytes[j] & 0b1) == 0) // Проверяем младший бит
                        ret += " "; // Если 0 - записываем 1 пробел
                    else
                        ret += "  "; // Если 1 - 2 пробела
                    ret += text[0]; // Добавляем следующее слово
                    text.RemoveAt(0); // Удаляем это слово
                    wordBytes[j] = (byte)(wordBytes[j] >> 1); // Сдвигаем биты байта
                }
            }
            ret += "   "; // записываем 3 пробела, когда байты закончились
            ret += string.Join(" ", text); // Добавляем оставшиеся слова исходного списка
            return ret;
        }

        static string Decoded(string codedText)
        {
            List<byte> wordB = new List<byte>(); // Подготовка списка для байтов слова
            byte wB = 0; // Для накопления битов
            int ind = 0; // индекс символов
            int indB = 0; // индекс битов
            while (ind < codedText.Length) // Цикл по символам входной строки
            {
                if (codedText[ind] == ' ') // Проверка на пробел
                {
                    if (codedText[ind + 1] != ' ') // Проверка на одинарный пробел
                    {
                        indB++; // Если пробел одиночный - это бит нулевой
                    }
                    else if (codedText[ind + 2] != ' ') // Проверка на двойной пробел
                    {
                        wB = (byte)(wB | 0b1); // Запись 1 в старший бит
                        indB++;
                    }
                    else break; // Выход из цикла, если тройной пробел
                    if(indB == 8) // Проверка накопленных битов
                    {
                        wordB.Add(wB); // Если накоплен бай то его запись
                        wB = 0;
                        indB = 0;
                    }
                    wB = (byte)(wB >> 1);
                }
                ind++;
            }
            string word = Encoding.UTF8.GetString(wordB.ToArray());

            return word;
        }
    }
}
