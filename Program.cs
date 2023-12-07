using System;
using System.Collections.Generic;

class InventoryItem
{
    public string Name { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
}

class InventoryManagementSystem
{
    private List<InventoryItem> inventory;

    public InventoryManagementSystem()
    {
        inventory = new List<InventoryItem>();
    }

    public void AddItem(string name, int quantity, double price)
    {
        InventoryItem newItem = new InventoryItem
        {
            Name = name,
            Quantity = quantity,
            Price = price
        };

        inventory.Add(newItem);
        //Console.WriteLine("Item added to inventory.");
    }

    public void UpdateItem(string name, int newQuantity, double newPrice)
    {
        InventoryItem itemToUpdate = inventory.Find(item => item.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        if (itemToUpdate != null)
        {
            itemToUpdate.Quantity = newQuantity;
            itemToUpdate.Price = newPrice;
            Console.WriteLine($"Item '{name}' updated in inventory.");
        }
        else
        {
            Console.WriteLine($"Item '{name}' not found in inventory. ");
        }
    }

    public void DeleteItem(string name)
    {
        InventoryItem itemToRemove = inventory.Find(item => item.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        if (itemToRemove != null)
        {
            inventory.Remove(itemToRemove);
            Console.WriteLine($"Item '{name}' deleted from inventory.");
        }
        else
        {
            Console.WriteLine($"Item '{name}' not found in inventory.");
        }
    }

    public void DisplayInventory()
    {
        Console.WriteLine("Inventory:");

        foreach (var item in inventory)
        {
            Console.WriteLine($"Name: {item.Name}, Quantity: {item.Quantity}, Price: {item.Price}");
        }
    }
}

class Program
{
    static void Main()
    {
        InventoryManagementSystem ims = new InventoryManagementSystem();

        ims.AddItem("Anesthesia Machine", 12, 2750.25);
        ims.AddItem("Oxygen Concentrator", 5, 1400.25);
        ims.AddItem("Suction Machine", 2, 275.15);
        ims.AddItem("Vital signs monitor", 25, 300.50);

        ims.DisplayInventory();

        // Console Options
        Console.WriteLine("\nChoose an option:");
        Console.WriteLine("1. Add Item");
        Console.WriteLine("2. Update Item");
        Console.WriteLine("3. Delete Item");
        Console.WriteLine("4. Display Inventory");
        Console.WriteLine("0. Exit");

        int choice;
        while (true)
        {
            Console.Write("Enter your choice: ");
            if (int.TryParse(Console.ReadLine(), out choice))
            {
                switch (choice)
                {
                    case 1:
                        Console.WriteLine("\nEnter new item details:");
                        Console.Write("Enter item name: ");
                        string newItemName = Console.ReadLine();

                        Console.Write("Enter quantity: ");
                        int newItemQuantity = int.Parse(Console.ReadLine());

                        Console.Write("Enter price: ");
                        double newItemPrice = double.Parse(Console.ReadLine());

                        ims.AddItem(newItemName, newItemQuantity, newItemPrice);
                        ims.DisplayInventory();
                        Console.WriteLine("Item added to inventory.");
                        break;

                    case 2:
                        Console.Write("Enter the name of the item to update: ");
                        string updateItemName = Console.ReadLine();

                        Console.Write("Enter new quantity: ");
                        int updateItemQuantity = int.Parse(Console.ReadLine());

                        Console.Write("Enter new price: ");
                        double updateItemPrice = double.Parse(Console.ReadLine());

                        ims.UpdateItem(updateItemName, updateItemQuantity, updateItemPrice);
                        ims.DisplayInventory();
                        Console.WriteLine("Item updated.");
                        break;

                    case 3:
                        Console.Write("Enter the name of the item to delete: ");
                        string deleteItemName = Console.ReadLine();
                        ims.DeleteItem(deleteItemName);
                        break;

                    case 4:
                        ims.DisplayInventory();
                        break;

                    case 0:
                        Console.WriteLine("Exiting the program.");
                        return;

                    default:
                        Console.WriteLine("Invalid choice. Please enter a valid option.");
                        Console.WriteLine("Item deleted out of inventory.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a number.");
            }
        }
    }
}


