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

            //1. View Products and add to cart
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

                if (cartCount >= cart.Length)
                {
                    Console.WriteLine("Cart is full!");
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

            //2. Product search
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

            //3. Category Filter
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

            //4. Cart Menu
            else if (choice == 4)
            {
                bool cartMenu = true;

                while (cartMenu)
                {
                    Console.WriteLine("\n===== CART MENU =====");
                    Console.WriteLine("1. View cart");
                    Console.WriteLine("2. Remove Item");
                    Console.WriteLine("3. Update Quantity");
                    Console.WriteLine("4. Clear Cart");
                    Console.WriteLine("5. Checkout");
                    Console.WriteLine("6. Back");

                    Console.Write("Enter choice: ");
                    
                    if (!int.TryParse(Console.ReadLine(), out int c))
                    {
                        Console.WriteLine("Invalid input!");
                        continue;
                    }

                    //View Cart
                    if (c == 1)
                    {
                        double total = 0;

                        for (int i = 0; i < cartCount; i++)
                        {
                            Console.WriteLine(cart[i].Name + " x" + cartQty[i] + " = PHP " + total + cartSubtotal[i]);
                            total += cartSubtotal[i];
                        }

                        Console.WriteLine("Total: PHP " + total);
                    }

                    //Remove
                    else if (c == 2)
                    {
                        if (cartCount == 0)
                        {
                            Console.WriteLine("Cart is empty!");
                            continue;
                        }

                        for (int i = 0; i < cartCount; i++)
                            Console.WriteLine((i + 1) + ". " + cart[i].Name);

                        Console.Write("Enter item number to remove: ");
                        int r;

                        if (!int.TryParse(Console.ReadLine(), out r) || r < 1 || r > cartCount)
                        {
                            Console.WriteLine("Invalid item!");
                            continue;
                        }

                        r--;

                        cart[r].RemainingStock += cartQty[r];

                        for (int i = r; i < cartCount - 1; i++)
                        {
                            cart[i] = cart[i + 1];
                            cartQty[i] = cartQty[i + 1];
                            cartSubtotal[i] = cartSubtotal[i + 1];
                        }

                        cartCount--;
                        Console.WriteLine("Item removed!");
                    }

                    //Update Quantity
                    else if (c == 3)
                    {
                        if (cartCount == 0)
                        {
                            Console.WriteLine("Cart is empty!");
                            continue;
                        }

                        for (int i = 0; i < cartCount; i++)
                            Console.WriteLine((i + 1) + ". " + cart[i].Name);

                        Console.Write("Enter item number: ");
                        int i2;

                        if (!int.TryParse(Console.ReadLine(), out i2) || i2 < 1 || i2 > cartCount)
                        {
                            Console.WriteLine("Invalid item!");
                            continue;
                        }

                        i2--;

                        Console.Write("Enter new quantity: ");
                        
                        if (!int.TryParse(Console.ReadLine(), out int newQty) || newQty <= 0)
                        {
                            Console.WriteLine("Invalid quantity!");
                            continue;
                        }

                        cart[i2].RemainingStock += cartQty[i2];
                        
                        if (newQty > cart[i2].RemainingStock)
                        {
                            Console.WriteLine("Not enough stock!");
                            cart[i2].RemainingStock -= cartQty[i2];
                            continue;
                        }

                        cartQty[i2] = newQty;
                        cart[i2].RemainingStock -= newQty;
                        cartSubtotal[i2] = cart [i2].GetItemTotal(newQty);

                        Console.WriteLine("Updated!");
                    }

                    //Clear
                    else if (c == 4)
                    {
                        cartCount = 0;
                        Console.WriteLine("Cart cleared!");
                    }

                    //Checkout
                    else if (c == 5)
                    {
                        double grandTotal = 0;
                        
                        Console.WriteLine("\n===== RECEIPT =====");
                        Console.WriteLine("Receipt No: " + receiptNo.ToString("0000"));
                        Console.WriteLine("Date: " + DateTime.Now);
                        
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
    
                        //Payment Validation
                        double payment;

                        while (true)
                        {
                            Console.Write("Enter payment: ");
                            payment = double.Parse(Console.ReadLine());
                    
                            if (payment >= finalTotal)
                            break;

                            Console.WriteLine("Insufficient payment!");
                        }

                        Console.WriteLine("Change: PHP " + (payment - finalTotal));

                        if (historyCount < orderHistory.Length)
                        {
                            orderHistory[historyCount++] = "Receipt #" + receiptNo.ToString("0000") + " - PHP " + finalTotal;
                        }
                        
                        receiptNo++;

                        //Low Stock
                        Console.WriteLine("\nLOW STOCK ALERT:");
                        for (int i = 0; i < products.Length; i++)
                        {
                            if (products[i].RemainingStock <= 5)
                            Console.WriteLine(products[i].Name + " low stock: " + products[i].RemainingStock);
                        }

                        cartCount = 0;
                        cartMenu = false;

                        //Y/N
                        string ans;

                        while (true)
                        {
                            Console.Write("\nDo you want to continnue shopping? (Y/N): ");
                            ans = Console.ReadLine().ToUpper();

                            if (ans == "Y" || ans == "N")
                                break;

                            Console.WriteLine("Invalid input. Please Y or N only.");
                        }

                        if (ans == "N")
                        {
                            exit = true;
                            Console.WriteLine("Thankyou for Shopping!");
                        }
                    }

                    else if (c == 6)
                    {
                        cartMenu = false;
                    }
                }
            }

            //5. Order History
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

            //6. Exit
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
        
