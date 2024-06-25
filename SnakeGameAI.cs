using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SnakeGame
{
    public class SnakeGameAI
    {
        // YEMEK KENDI USTUNDE CIKIYOR****
        private int width = 25;
        private int height = 25;
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
                MoveSnakeAI();
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
                Thread.Sleep(200); // Yılanın hızını yavaşlatmak için süreyi artırın
            }
        }

        private void MoveSnakeAI()
        {
            Point head = snake[0];
            List<Point> path = FindPath(head, food);

            if (path.Count > 1)
            {
                Point nextStep = path[1];
                direction = GetDirection(head, nextStep);
                snake.Insert(0, nextStep);
            }
        }

        private List<Point> FindPath(Point start, Point goal)
        {
            var openSet = new SortedSet<Point>(new PointComparer());
            var cameFrom = new Dictionary<Point, Point>();
            var gScore = new Dictionary<Point, int>();
            var fScore = new Dictionary<Point, int>();

            openSet.Add(start);
            gScore[start] = 0;
            fScore[start] = Heuristic(start, goal);

            while (openSet.Count > 0)
            {
                Point current = openSet.First();
                if (current.Equals(goal))
                {
                    return ReconstructPath(cameFrom, current);
                }

                openSet.Remove(current);
                foreach (var neighbor in GetNeighbors(current))
                {
                    int tentativeGScore = gScore[current] + 1;
                    if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                    {
                        cameFrom[neighbor] = current;
                        gScore[neighbor] = tentativeGScore;
                        fScore[neighbor] = gScore[neighbor] + Heuristic(neighbor, goal);
                        if (!openSet.Contains(neighbor))
                        {
                            openSet.Add(neighbor);
                        }
                    }
                }
            }

            return new List<Point>(); // Yol bulunamadı
        }

        private List<Point> ReconstructPath(Dictionary<Point, Point> cameFrom, Point current)
        {
            var totalPath = new List<Point> { current };
            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                totalPath.Insert(0, current);
            }
            return totalPath;
        }

        private int Heuristic(Point a, Point b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }

        private IEnumerable<Point> GetNeighbors(Point point)
        {
            var neighbors = new List<Point>
            {
                new Point(point.X + 1, point.Y),
                new Point(point.X - 1, point.Y),
                new Point(point.X, point.Y + 1),
                new Point(point.X, point.Y - 1)
            };

            return neighbors.Where(p => p.X >= 0 && p.X < width && p.Y >= 0 && p.Y < height && !snake.Contains(p));
        }

        private Direction GetDirection(Point from, Point to)
        {
            if (to.X > from.X) return Direction.Right;
            if (to.X < from.X) return Direction.Left;
            if (to.Y > from.Y) return Direction.Down;
            return Direction.Up;
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
            int x, y;
            do
            {
                x = random.Next(0, width);
                y = random.Next(0, height);
            } while (snake.Contains(new Point(x, y))); // Yemek yılanın üzerindeyse yeni bir konum oluştur

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

    public class PointComparer : IComparer<Point>
    {
        public int Compare(Point a, Point b)
        {
            if (a.X == b.X && a.Y == b.Y) return 0;
            return a.X == b.X ? a.Y.CompareTo(b.Y) : a.X.CompareTo(b.X);
        }
    }
}