using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace KottansProject
{
    class Program
    {
        /* Функція GetCreditCardVendor, яка визначає вендора кредитних карток:
        American Express
        Maestro
        MasterCard
        VISA
        JCB */
        static int sum;
        static Random rand = new Random();
        static string GetCreditCardVendor(string cardNumber)
        {
            try
            {
                cardNumber = cardNumber.Replace(" ", null);

                if (!LuhnAlgorithm(cardNumber))
                    return "Input card number is not valid";

                if (cardNumber.Length == 15)
                    if (cardNumber[0] == '3' && cardNumber[1] == '4' || cardNumber[0] == '3' && cardNumber[1] == '7')
                        return "American Express";

                if (cardNumber.Length >= 12 && cardNumber.Length <= 19)
                {
                    int number = int.Parse(cardNumber.Substring(0, 2));
                    if (number == 50)
                        return "Maestro";
                    if (number >= 56 && number <= 69)
                        return "Maestro";
                }

                if (cardNumber.Length == 16)
                {
                    int number = int.Parse(cardNumber.Substring(0, 2));
                    if (number >= 51 && number <= 55)
                        return "Master Card";
                }

                if (cardNumber.Length == 13 || cardNumber.Length == 16 || cardNumber.Length == 19)
                    if (cardNumber[0] == '4')
                        return "Visa";

                if (cardNumber.Length == 16)
                {
                    int number = int.Parse(cardNumber.Substring(0, 4));
                    if (number >= 3528 && number <= 3589)
                        return "JCB";
                }
            }
            catch { return "Unknown!"; }
            return "Unknown!";
        }


        //Алгоритм Луна
        private static bool LuhnAlgorithm(string cardNumber)
        {
            cardNumber = cardNumber.Replace(" ", null);

            int pairCheck = sum = 0;

            pairCheck = cardNumber.Length % 2 == 0 ? 0 : 1;

            for (int i = pairCheck; i < cardNumber.Length; i += 2)
            {
                int digit = int.Parse(cardNumber[i].ToString());
                if (digit * 2 > 9)
                    digit = digit * 2 - 9;
                else
                    digit *= 2;
                sum += digit;
            }

            pairCheck = pairCheck == 1 ? 0 : 1;

            for (int i = pairCheck; i < cardNumber.Length; i += 2)
            {
                int digit = int.Parse(cardNumber[i].ToString());
                sum += digit;
            }

            return sum % 10 == 0;
        }

        //Функція IsCreditCardNumberValid, яка за допомогою алгоритму Луна 
        //перевіряє коректність переданого в параметрі номера картки.
        static bool IsCreditCardNumberValid(string cardNumber)
        {
            cardNumber = cardNumber.Replace(" ", null);

            if (GetCreditCardVendor(cardNumber) == "Unknown!")
                return false;

            return LuhnAlgorithm(cardNumber);
        }

        //Функція GenerateNextCreditCardNumber, яка для переданого в параметрі номера картки визначить наступний номер. 
        static string GenerateNextCreditCardNumber(string cardNumber)
        {
            if (!IsCreditCardNumberValid(cardNumber))
                return "Input card number is not valid";

            cardNumber = cardNumber.Replace(" ", null);

            /*Можна зробити перевірку на вендора, 
            наприклад віза має тільки першу цифру, а інші мають 2 і 4 цифри,
            по форумах читав, написано робити по принципу: перші 4 цифри вендор, наступні 4 код банку,
            дальше номер картки, по такому принципу написав функцію, перші 8 цифр зберігаю,
            дальше вираховую наступний номер*/
            int vendor = int.Parse(cardNumber.Substring(0, 8));

            int[] number = new int[cardNumber.Length - 8];
            int j = 0;

            for (int i = 8; i < cardNumber.Length; i++)
                number[j++] = int.Parse(cardNumber[i].ToString());

            long nextNumber = Int64.Parse(string.Join("", number));
            nextNumber++;

            while (!LuhnAlgorithm(Convert.ToString(string.Join("", vendor) + nextNumber)))
                nextNumber++;

            int check = int.Parse(Convert.ToString(string.Join("", vendor).Length));
            check += int.Parse(Convert.ToString(string.Join("", nextNumber).Length));

            if (check > cardNumber.Length)
                return "No more card numbers available";

            return Convert.ToString(Convert.ToString(string.Join("", vendor) + nextNumber));
        }

        static void Main(string[] args)
        {
            string str = "4999999999999996";
            Console.WriteLine(GetCreditCardVendor(str));
            Console.WriteLine(IsCreditCardNumberValid(str));
            Console.WriteLine(GenerateNextCreditCardNumber(str));
            Console.ReadKey();
        }
    }
}
