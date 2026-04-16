using System;

class Product
{
    public int Id;
    public string Name;
    public double Price;
    public int RemainingStock;

    public void DisplayProduct()
    {
        Console.WriteLine(Id + ". " + Name + " - PHP " + Price + " (Stock: " + RemainingStock + ")");
    }

    public double GetItemTotal(int qty)
    {
        return Price * qty;
    }
}

class Program
{
    static void Main()
    {
        // Store MENU
        Product[] products = new Product[]
        {
          new Product { Id = 1, Name = "Hotdog", Price = 65, RemainingStock = 25 },
          new Product { Id = 2, Name = "Tocino", Price = 75, RemainingStock = 15 },
          new Product { Id = 3, Name = "ChickenNuggets", Price = 70, RemainingStock = 17 },
          new Product { Id = 4, Name = "Longganisa", Price = 85, RemainingStock = 10 },
          new Product { Id = 5, Name = "Ham", Price = 150, RemainingStock = 15 },
          new Product { Id = 6, Name = "Drummets", Price = 215, RemainingStock = 13 },
          new Product { Id = 7, Name = "ShrimpTempura", Price = 415, RemainingStock = 8 }
        };

        //Cart System
        Product[] cart = new Product[10];
        int[] cartQty = new int[10];
        double[] cartSubtotal = new double[10];
        int cartCount = 0;
        double runningTotal = 0;

        string choice = "Yes";

        //Buying
        while (choice == "Yes" || choice == "yes")
        {
            Console.WriteLine("\n===== STORE MENU =====");

            for (int i = 0; i < products.Length; i++)
            {
                products[i].DisplayProduct();
            }

            //Product Input
            Console.Write("\nEnter product number: ");
            int num;

            if (!int.TryParse(Console.ReadLine(), out num) || num < 1 || num > products.Length)
            {
                Console.WriteLine("Invalid product number!");
                continue;
            }

            Product selected = products[num - 1];

            //Out of Stock
            if (selected.RemainingStock == 0)
            {
                Console.WriteLine("Out of Stock!");
                continue;
            }

            //Quantity Input
            Console.Write("Enter quantity: ");
            int qty;

            if (!int.TryParse(Console.ReadLine(), out qty) || qty <= 0)
            {
                Console.WriteLine("Invalid Quantity!");
                continue;
            }

            if (qty > selected.RemainingStock)
            {
                Console.WriteLine("Not enough stock available!");
                continue;
            }
            double itemTotal = selected.GetItemTotal(qty);

            //Check Duplicate
            bool found = false;

            for (int i = 0; i < cartCount; i++)
            {
                if (cart[i].Id == selected.Id)
                {
                    cartQty[i] += qty;
                    cartSubtotal[i] += itemTotal;
                    found = true;
                    break;
                }
            }

            //Add new Item
            if (!found)
            {
                if (cartCount >= cart.Length)
                {
                    Console.WriteLine("Cart is full!");
                    continue;
                }

                cart[cartCount] = selected;
                cartQty[cartCount] = qty;
                cartSubtotal[cartCount] = itemTotal;
                cartCount++;
            }
            //Deduct Stock
            selected.RemainingStock -= qty;

            //Update running total
            runningTotal += itemTotal;

            Console.WriteLine("Added to cart! Subtotal: PHP " + itemTotal);
            Console.WriteLine("Current Total: PHP " + runningTotal);

            //Continue?
            Console.Write("\nDo you want to continue? (Yes/No): ");
            choice = Console.ReadLine();
        }

        //Updated Stock
        Console.WriteLine("\n===== UPDATED STOCK =====");

        for (int i = 0; i < products.Length; i++)
        {
            Console.WriteLine(products[i].Name + " - " + products[i].RemainingStock);
        }

        //Receipt
        Console.WriteLine("\n===== RECEIPT ======");

        double grandTotal = 0;

        for (int i = 0; i < cartCount; i++)
        {
            Console.WriteLine(cart[i].Name + " x" + cartQty[i] + " = PHP " + cartSubtotal[i]);
            grandTotal += cartSubtotal[i];
        }

        //Discount
        double discount = 0;

        if (grandTotal >= 5000)
        {
            discount = grandTotal * 0.10;
        }

        double finalTotal = grandTotal - discount;

        Console.WriteLine("Grand Total: PHP " + grandTotal);
        Console.WriteLine("Discount: PHP " + discount);
        Console.WriteLine("Final Total: PHP " + finalTotal);
    
        Console.WriteLine("\nThankyou for Shopping!");
    }
}
