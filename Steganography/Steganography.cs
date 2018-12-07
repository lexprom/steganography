using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Steganography
{
    class Steganography
    {
        public static string Encoded(List<string> text, byte[] wordBytes)
        {
            string ret = text[0]; //Записываем первое слово
            text.RemoveAt(0); // Удаляем первое слово
            foreach (byte _wB in wordBytes) // Цикл по байтам
            {
                byte wB = _wB;
                // Console.WriteLine(wB);
                for (int i = 0; i < 8; i++)    // Цикл по битам
                {
                    // Console.WriteLine(Convert.ToString(wB, 2));
                    if ((wB & 1) == 0) // Проверяем младший бит
                        ret += " \n"; // Если 0 - записываем 1 пробел
                    else
                        ret += "  \n"; // Если 1 - 2 пробела
                    ret += text[0]; // Добавляем следующее слово
                    text.RemoveAt(0); // Удаляем это слово
                    wB = (byte)(wB >> 1); // Сдвигаем биты байта
                }
            }
            ret += "   \n"; // записываем 3 пробела, когда байты закончились
            ret += string.Join("\n", text); // Добавляем оставшиеся слова исходного списка

            return ret;
        }

        public static string Decoded(string codedText)
        {
            List<byte> wordB = new List<byte>(); // Подготовка списка для байтов слова
            byte wB = 0; // Для накопления битов
            int ind = 0; // индекс символов
            int indB = 0; // индекс битов
            while (ind < codedText.Length) // Цикл по символам входной строки
            {
                if (codedText[ind] == '\n')
                {
                    if (codedText[ind - 1] == ' ') // Проверка на пробел
                    {
                        if (codedText[ind - 2] != ' ') // Проверка на одинарный пробел
                        {
                            indB++; // Если пробел одиночный - это бит нулевой
                        }
                        else if (codedText[ind - 3] != ' ') // Проверка на двойной пробел
                        {
                            wB = (byte)(wB | 128); // Запись 1 в старший бит
                            indB++; ind++;
                        }
                        else break; // Выход из цикла, если тройной пробел
                                    // Console.WriteLine(Convert.ToString(wB, 2).PadLeft(8, '0'));
                        if (indB == 8) // Проверка накопленных битов
                        {
                            wordB.Add(wB); // Если накоплен байт - его запись
                            wB = 0;
                            indB = 0;
                        }
                        wB = (byte)(wB >> 1);
                    }
                }
                ind++;
            }
            string word = Encoding.UTF8.GetString(wordB.ToArray());

            return word;
        }
    }
}
