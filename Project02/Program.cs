using System;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Project02
{
    class Program
    {
        static string GreedyChange(int[] coins, int change)
        {
            if(change <= 0)
            {
                return "Input should be a positive integer";
            }

            string result = "Greedy exchange: ";
            int quantity = 0;

            Array.Sort(coins);
            Array.Reverse(coins);

            foreach (var coin in coins)
            {
                quantity = change / coin;
                if(quantity <= 0)
                {
                    continue;
                }
                result += $"{quantity}x{coin}, ";
                change -= quantity * coin;

                if (change <= 0)
                {
                    break;
                }
            }

            Console.Clear();
            return result;
        }

        static string DynamicChange(int[] coins, int change)
        {
            Array.Sort(coins);

            int firstCoinIndex = 0;
            int quantity;
            string result = "Dynamic algorithm exchange: ";

            var minCoins = new int[change + 1];
            minCoins[0] = 0;

            for (int i = 1; i < minCoins.Length; i++)
            {
                minCoins[i] = change + 1;
            }

            for (int i = 1; i < minCoins.Length; i++)
            {
                for (int j = 0; j < coins.Length; j++)
                {

                    if (coins[j] <= i)
                    {
                        if (1+ minCoins[i - coins[j]] < minCoins[i])
                        {
                            minCoins[i] = Math.Min(1 + minCoins[i - coins[j]], minCoins[i]);
                            firstCoinIndex = j;
                        }
                    }
                }
            }

            for (int i = firstCoinIndex; i >= 0; i--)
            {
                quantity = change / coins[i];
                if (quantity <= 0)
                {
                    continue;
                }
                result += $"{quantity}x{coins[i]}, ";
                change -= quantity * coins[i];

                if (change <= 0)
                {
                    break;
                }
            }

            Console.Clear();
            return result;
        }
        static bool UserInputCheck(string userInput) //Test for user input
        {
            string LastDigit = "";      // String to check if there are any repeating coins
            userInput = userInput.Replace(" ", "");
            if (userInput.Any(char.IsLetter))
            {
                Console.WriteLine("No Letters are allowed");
                return false;
            }

            string[] coins = userInput.Split(',');
            Array.Sort(coins);

            for (int i = 0; i < coins.Length; i++)
            {
                foreach(char c in coins[i])
                {
                    if(!char.IsDigit(c))
                    {
                        Console.WriteLine($"No special symbols are allowed ({c})");
                        return false;
                    }
                }
                if (int.Parse(coins[i]) <= 0)
                {
                    Console.WriteLine("Coin should be positive integer");
                    return false;
                }
                if (LastDigit == coins[i])
                {
                    Console.WriteLine("Coins should have unique values");
                    return false;

                }
                LastDigit = coins[i];
            }
            if (!coins.Contains("1"))
            {
                Console.WriteLine("One coin must have value of 1");
                return false;

            }

            return true;
        }

        static void Main(string[] args)
        {
            int change = 0;
            bool check2 = false;    // boolean value for checking change amount
            bool check1 = false;    // boolean value for checking set of coins
            string userInput1 = ""; // string value for set of coins
            char userInput2;        // char value for choosing type of algorithm

            do    // user input - set of coins
            {
                Console.Clear();
                Console.WriteLine("Write a set of coins (use coma (,) as split; i.e. 1, 2, 5, 10, 20, 50, 100");
                userInput1 = Console.ReadLine();

                check1 = UserInputCheck(userInput1);
                if(!check1)
                {
                    Thread.Sleep(1000);
                }
            } while (!check1);

            userInput1 = userInput1.Replace(" ", "");
            int[] coins = Array.ConvertAll(userInput1.Split(','), int.Parse); //create an array of coins from userInput1

            do    // user input - change amount
            {
                Console.Clear();
                Console.Write("Write change amount: ");
                check2 = int.TryParse(Console.ReadLine(), out change);
                if (change < 50)
                {
                    Console.WriteLine("Change must be greater than zero");
                    Thread.Sleep(1000);
                }
            } while (!check2 || change <= 0);

            do    // user input - type of algorithm
            {
                Console.Clear();
                Console.WriteLine("Choose:\n'1' for Greedy Algorithm;\n'2' for Dynamic Algorithm;");
                userInput2 = Console.ReadKey().KeyChar;

            } while (!(userInput2 == '1' || userInput2 == '2'));

            switch(userInput2)
            {
                case '1': Console.WriteLine(GreedyChange(coins, change));break;
                case '2': Console.WriteLine(DynamicChange(coins, change));break;
            }

            Console.ReadLine();
        }
    }
}
