using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SnakeGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Yılan oyununa hoş geldiniz!");
            Console.WriteLine("1. Kendiniz oynayın");
            Console.WriteLine("2. Bilgisayar oynasın");
            Console.Write("Seçiminizi yapın (1 veya 2): ");
            var choice = Console.ReadLine();

            if (choice == "1")
            {
                PlayGame();
            }
            else if (choice == "2")
            {
                PlayGameWithAI();
            }
            else
            {
                Console.WriteLine("Geçersiz seçim. Program sonlandırılıyor.");
            }
            Console.ReadKey();
        }

        static void PlayGame()
        {
            // Kullanıcı tarafından oynanan yılan oyunu
            SnakeGame game = new SnakeGame();
            game.Start();
        }

        static void PlayGameWithAI()
        {
            // Bilgisayar tarafından oynanan yılan oyunu
            SnakeGameAI game = new SnakeGameAI();
            game.Start();
        }
        
    }
}