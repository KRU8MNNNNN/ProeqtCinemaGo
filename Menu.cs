
using System;
using System.IO;
using System.Linq;

namespace Proeqt
{
    internal class Menu
    {
        string path = "C:\\Users\\595-424-424 Temo\\source\\repos\\Proeqt\\Data\\users.txt";

        public void ShowMenu()
        {
            string choose = "0";

            while (choose != "9")
            {
                Console.WriteLine("1. Add product");
                Console.WriteLine("2. Show all products");
                Console.WriteLine("3. Find product by ID");
                Console.WriteLine("4. Sort by price (cheapest to most expensive)");
                Console.WriteLine("5. Sort by price (most expensive to cheapest");
                Console.WriteLine("9. Exit");

                choose = Console.ReadLine();

                switch (choose)
                {
                    case "1":
                        int maxId = 0;

                        foreach (string line in File.ReadAllLines(path))
                        {
                            string[] elements = line.Split('|');
                            if (elements.Length == 4 && int.TryParse(elements[0], out int currentId) && currentId > maxId)
                                maxId = currentId;
                        }

                        Product product = new Product();
                        product.Id = maxId + 1;

                        Console.WriteLine("Name:");
                        product.Name = Console.ReadLine();

                        Console.WriteLine("Price:");
                        if (!int.TryParse(Console.ReadLine(), out int price) || price < 0)
                        {
                            Console.WriteLine("ERROR");
                            break;
                        }
                        product.Price = price;

                        Console.WriteLine("Description:");
                        product.Description = Console.ReadLine();

                        File.AppendAllLines(path, new[] { product.ToString() });
                        break;

                    case "2":
                        File.ReadAllLines(path).ToList()
                            .ForEach(x => Console.WriteLine(x));
                        break;

                    case "3":
                        Console.WriteLine("Enter ID:");
                        if (!int.TryParse(Console.ReadLine(), out int id))
                        {
                            Console.WriteLine("ERROR");
                            break;
                        }

                        var found = File.ReadAllLines(path)
                            .FirstOrDefault(x => int.TryParse(x.Split('|')[0], out int productId) && productId == id);

                        Console.WriteLine(found ?? "Product not found");
                        break;

                    case "4":
                        string[] products = File.ReadAllLines(path)
                            .Where(x => x.Split('|').Length == 4 && int.TryParse(x.Split('|')[2], out int p))
                            .OrderBy(x => int.Parse(x.Split('|')[2]))
                            .ToArray();

                        foreach (string item in products)
                        {
                            string[] elements = item.Split('|');
                            Console.WriteLine(elements[0] + "|" + elements[1] + "|" + elements[2] + "|" + elements[3]);
                        }
                        break;

                    case "5":
                        string[] expensiveProducts = File.ReadAllLines(path)
                            .Where(x => x.Split('|').Length == 4 && int.TryParse(x.Split('|')[2], out int p))
                            .OrderByDescending(x => int.Parse(x.Split('|')[2]))
                            .ToArray();

                        foreach (string item in expensiveProducts)
                        {
                            string[] elements = item.Split('|');
                            Console.WriteLine(elements[0] + "|" + elements[1] + "|" + elements[2] + "|" + elements[3]);
                        }
                        break;

                    case "9":
                        break;

                    default:
                        Console.WriteLine("ERROR");
                        break;
                }
            }
        }
    }
}
