using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SnakeGame
{
    public class SnakeGame
    {
        private int width = 20;
        private int height = 20;
        private int score = 0;
        private List<Point> snake = new List<Point>();
        private Point food;
        private Direction direction = Direction.Right;
        private Random random = new Random();

        public void Start()
        {
            Console.Clear();
            snake.Add(new Point(width / 2, height / 2));
            GenerateFood();

            while (true)
            {
                Thread.Sleep(200);
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    switch (key)
                    {

                        case ConsoleKey.UpArrow:
                            if (direction != Direction.Down) direction = Direction.Up;
                            break;
                        case ConsoleKey.DownArrow:
                            if (direction != Direction.Up) direction = Direction.Down;
                            break;
                        case ConsoleKey.LeftArrow:
                            if (direction != Direction.Right) direction = Direction.Left;
                            break;
                        case ConsoleKey.RightArrow:
                            if (direction != Direction.Left) direction = Direction.Right;
                            break;
                    }
                }

                MoveSnake();
                if (CheckCollision())
                {
                    Console.Clear();
                    Console.WriteLine("Oyun bitti! Skorunuz: " + score);
                    break;
                }

                if (snake[0].Equals(food))
                {
                    score++;
                    GenerateFood();
                }
                else
                {
                    snake.RemoveAt(snake.Count - 1);
                }

                Draw();
                Thread.Sleep(100);
            }
        }

        private void MoveSnake()
        {
            Point head = snake[0];
            Point newHead = head;

            switch (direction)
            {
                case Direction.Up:
                    newHead = new Point(head.X, head.Y - 1);
                    break;
                case Direction.Down:
                    newHead = new Point(head.X, head.Y + 1);
                    break;
                case Direction.Left:
                    newHead = new Point(head.X - 1, head.Y);
                    break;
                case Direction.Right:
                    newHead = new Point(head.X + 1, head.Y);
                    break;
            }

            snake.Insert(0, newHead);
        }

        private bool CheckCollision()
        {
            Point head = snake[0];

            if (head.X < 0 || head.X >= width || head.Y < 0 || head.Y >= height)
            {
                return true;
            }

            for (int i = 1; i < snake.Count; i++)
            {
                if (snake[i].Equals(head))
                {
                    return true;
                }
            }

            return false;
        }

        private void GenerateFood()
        {
            int x = random.Next(0, width);
            int y = random.Next(0, height);
            food = new Point(x, y);
        }

        private void Draw()
        {
            Console.Clear();
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (snake.Contains(new Point(x, y)))
                    {
                        Console.Write("O ");
                    }
                    else if (food.Equals(new Point(x, y)))
                    {
                        Console.Write("X ");
                    }
                    else
                    {
                        Console.Write(". ");
                    }
                }
                Console.WriteLine();
            }
        }
    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public struct Point
    {
        public int X { get; }
        public int Y { get; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            if (obj is Point)
            {
                Point other = (Point)obj;
                return X == other.X && Y == other.Y;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }
    }
}