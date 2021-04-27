using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace Compression
{
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();
        }

        string text = "";
        string nextChar = "";
        int pointer = 0;
        List<string> dic = new List<string>();//словарь префиксов

        // сжатие
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            screen_Copy.Clear();
            string CompChar = "";
            int index = 0, retrn = 0; //retrn показывает, из скольки символов состоит текущий префикс в тексте(1-несколько,0-один символ)
            text = screen.Text;
            if (text == "")
            {
                screen.Text = "Введите текст для сжатия!";
            }
            else
            {
                screen.Text = "0 " + text[0] + "\n"; // кодируем первый символ
                dic.Add(""); // первый элемент словаря ""
                dic.Add(text[0] + "");// добавляем в словарь первый символ

                for (int indexText = 1; indexText < text.Length; indexText++)// по каждому символу в тексте
                {

                    CompChar += text[indexText];//добавляем к префиксу текущий символ
                    if (CompChar == " ") // заменяем пробел символом _ для возможности декодирования
                        CompChar = "_";
                    if (dic.IndexOf(CompChar) != -1)//если такой префикс есть в словаре
                    {
                        index = dic.IndexOf(CompChar);

                        retrn = 1;
                        if (indexText + 1 == text.Length)//для случая конца текста
                            screen.Text += index + " null\n";

                    }
                    else
                    {
                        if (retrn == 1) //если такой префикс, за исключением последнего символа, есть в словаре
                        {
                            if (CompChar[CompChar.Length - 1] == ' ')
                                screen.Text += index + " " + '_' + "\n";
                            else
                                screen.Text += index + " " + CompChar[CompChar.Length - 1] + "\n";
                        }
                        else
                            screen.Text += "0 " + CompChar + "\n";

                        dic.Add(CompChar);// добавить в словарь
                        CompChar = "";


                        retrn = 0;

                    }

                }
                for (int i = 1; i < dic.Count; i++)
                {
                    screen_Copy.Text += i + "  " + dic[i] + "\n";
                }
            }
        }

        // декомпрессия
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            text = screen.Text;
            if (text == "")
            {
                screen.Text = "Введите текст для разжатия!";
            }
            else
            {
                string[] CompRslt = screen.Text.Split(); //разбиваем сжатый текст на лексемы(для этого меняли пробел на _)
                for (int i = 0; i < CompRslt.Length; i++)//меняем обратно
                {
                    if (CompRslt[i] == "_")
                        CompRslt[i] = " ";
                }
                screen.Text = "";
                for (int i = 0; i < text.Length; i += 2)//префикс кодируется двумя словами-индекс в словаре и символ расхождения, поэтому +2
                {
                    if (CompRslt[i].Length == 0)
                        break;
                    try { pointer = int.Parse(CompRslt[i]); } //перевод индекса в словаре из строки в число
                    catch
                    {
                        screen.Text = "Неверный формат";
                        break;
                    }
                    nextChar = CompRslt[i + 1];//получаем символ расхождения
                    if (dic[pointer] == "_")//меняем _ на пробел
                        dic[pointer] = " ";
                    if (nextChar != "null")
                    {
                        screen.Text += dic[pointer] + nextChar;
                    }

                    else
                        screen.Text += dic[pointer];// когда конец текста

                    pointer = 0;
                    nextChar = "";

                }

                // обнуляемся 
                pointer = 0;
                nextChar = "";
                dic.Clear();
            }
        }
    }
}
