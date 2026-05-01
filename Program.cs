using System;

class Product
{
    public int Id;
    public string Name;
    public string Category;
    public double Price;
    public int RemainingStock;

    // Display product
    public void DisplayProduct()
    {
        Console.WriteLine(Id + ". " + Name + " - " + Category + " - PHP " + Price + " (Stock: " + RemainingStock + ")");
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
          new Product { Id = 1, Name = "Hotdog", Category = "Food", Price = 65, RemainingStock = 25 },
          new Product { Id = 2, Name = "Tocino", Category = "Food", Price = 75, RemainingStock = 15 },
          new Product { Id = 3, Name = "ChickenNuggets", Category = "Food", Price = 70, RemainingStock = 17 },
          new Product { Id = 4, Name = "Longganisa", Category = "Food", Price = 85, RemainingStock = 10 },
          new Product { Id = 5, Name = "Ham", Category = "Food", Price = 150, RemainingStock = 15 },
          new Product { Id = 6, Name = "Drummets", Category = "Food", Price = 215, RemainingStock = 13 },
          new Product { Id = 7, Name = "ShrimpTempura", Category = "Food", Price = 415, RemainingStock = 8 },
          new Product { Id = 8, Name = "Mouse", Category = "Electronics", Price = 500, RemainingStock = 35 },
          new Product { Id = 9, Name = "Keyboard", Category = "Electronics", Price = 550, RemainingStock = 13 },
          new Product { Id = 10, Name = "Tshirt", Category = "Clothing", Price = 300, RemainingStock = 66 },
          new Product { Id = 11, Name = "Pants", Category = "Clothing", Price = 350, RemainingStock = 38 },
        };

        //Cart System
        Product[] cart = new Product[10];
        int[] cartQty = new int[10];
        double[] cartSubtotal = new double[10];
        int cartCount = 0;

        //Order History
        string[] orderHistory = new string[20];
        int historyCount = 0;

        int receiptNo = 1;
        bool exit = false;
        
        while (!exit)
        {
            //Main Menu
            Console.WriteLine("\n===== MAIN MENU =====");
            Console.WriteLine("1. View products");
            Console.WriteLine("2. Search Product");
            Console.WriteLine("3. Filter by Category");
            Console.WriteLine("4. Checkout");
            Console.WriteLine("5. Order History");
            Console.WriteLine("6. Exit");

            Console.Write("Enter Choice: ");
            int choice;

            if (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Invalid input!");
                continue;
            }

            //Vie Products and add to cart
            if (choice == 1)
            {
                Console.WriteLine("\n===== Products =====");
                
                for (int i = 0; i < products.Length; i++)
                    products[i].DisplayProduct();

                Console.Write("\nEnter Product ID: ");
                int id;

                if (!int.TryParse(Console.ReadLine(), out id) || id < 1 || id > products.Length)
                {
                    Console.WriteLine("Invalid product!");
                    continue;
                }

                Product selected = products[id - 1];

                Console.Write("Enter quantity: ");
                int qty;

                if (!int.TryParse(Console.ReadLine(), out qty) || qty <= 0 )
                {
                    Console.WriteLine("Invalid quantity!");
                    continue;
                }

                if (qty > selected.RemainingStock)
                {
                    Console.WriteLine("Not enough stock!");
                    continue;
                }

                double itemTotal = selected.GetItemTotal(qty);

                cart[cartCount] = selected;
                cartQty[cartCount] = qty;
                cartSubtotal[cartCount] = itemTotal;
                cartCount++;

                selected.RemainingStock -= qty;

                Console.WriteLine("Added to cart!");
            }

            //Product search
            else if (choice == 2)
            {
                Console.Write("\nSearch product name: ");
                string search = Console.ReadLine().ToLower();

                bool found = false;

                for (int i = 0; i < products.Length; i++)
                {
                    if (products[i].Name.ToLower().Contains(search))
                    {
                        products[i].DisplayProduct();
                        found = true;
                    }
                }

                if (!found)
                    Console.WriteLine("No product found.");
            }

            //Category Filter
            else if (choice == 3)
            {
                Console.WriteLine("\nCategories: Food, Electronics, Clothing");
                Console.Write("Enter category: ");
                string cat = Console.ReadLine().ToLower();

                bool found = false;

                for (int i = 0; i < products.Length; i++)
                {
                    if (products[i].Category.ToLower() == cat)
                    {
                        products[i].DisplayProduct();
                        found = true;
                    }
                }

                if (!found)
                    Console.WriteLine("No products in this category.");
            }

            //Checkout
            else if (choice == 4)
            {
                if (cartCount == 0)
                {
                    Console.WriteLine("Cart is empty!");
                    continue;
                }

                double grandTotal = 0;

                Console.WriteLine("\n===== RECEIPT =====");
                Console.WriteLine("Receipt No: " + receiptNo.ToString("0000"));
                Console.WriteLine("Date: " + DateTime.Now);

                for (int i = 0; i < cartCount; i++)
                {
                    Console.WriteLine(cart[i].Name +
                    " x" + cartQty[i] +
                    " = PHP " + cartSubtotal[i]);

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
    
                //Payment Validation
                double payment;

                while (true)
                {
                    Console.Write("Enter payment: ");

                    if (!double.TryParse(Console.ReadLine(), out payment))
                    {
                        Console.WriteLine("Invalid input!");
                        continue;
                    }

                    if (payment < finalTotal)
                    {
                        Console.WriteLine("Insufficient payment!");
                        continue;
                    }

                    break;
                }

                double change = payment - finalTotal;

                Console.WriteLine("Payment: PHP " + payment);
                Console.WriteLine("Change: PHP " + change);

                //Order History
                orderHistory[historyCount] =
                "Receipt #" + receiptNo.ToString("0000") +
                " - Total: PHP " + finalTotal;

                historyCount++;
                receiptNo++;

                //Low Stock Alert
                Console.WriteLine("\n===== LOW STOCK ALERT =====");

                for (int i = 0; i < products.Length; i++)
                {
                    if (products[i].RemainingStock <= 5)
                    {
                        Console.WriteLine(products[i].Name +
                        " has only " +
                        products[i].RemainingStock +
                        " stock(s) left.");
                    }
                }

                //Clear cart after checkout
                cartCount = 0;
            }

            //Order History
            else if (choice == 5)
            {
                Console.WriteLine("\n===== ORDER HISTORY =====");

                if (historyCount == 0)
                {
                    Console.WriteLine("No orders yet.");
                }

                for (int i = 0; i < historyCount; i++)
                {
                    Console.WriteLine(orderHistory[i]);
                }
            }

            //Exit
            else if (choice == 6)
            {
                if (cartCount > 0)
                {
                    Console.WriteLine("You still have items in your cart!");
                    Console.WriteLine("Please checkout first before exiting.");
                }
                else
                {
                    exit = true;
                    Console.WriteLine("Thankyou for Shopping!");
                }
            }
        }
    }
}
