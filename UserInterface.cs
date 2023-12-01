using System;
using InventoryList.Data;
using System.Globalization;
using System.Text.Json;
using InventoryList.Logic;

namespace InventoryList.UI
{


    static public class UserInterface
    {

        static string GetUserInput()
        {
            Console.Write("-> ");
            string userInput = Console.ReadLine();
            Console.WriteLine();
            return userInput;
        }


        static public void Menu(IInventoryLogic inventoryLogic)
        {
            bool exitCondition = false;
            while (!exitCondition)
            {
                DisplayMenu();
                string choice = GetUserInput().ToLower();
                switch (choice)
                {
                    case "1":
                        InventoryItemMenu(inventoryryLogic);
                        break;
                    case "2":
                        InventoryListMenu(inventoryryLogic);
                        break;
                    case "exit":
                        exitCondition = true;
                        break;
                    default:
                        Console.WriteLine("Invalid selection");
                        break;
                }
            }
        }

        static void DisplayMenu()
        {
            Console.WriteLine("Press 1 to View, Add, Remove, or Update the Medical Inventory");
            Console.WriteLine("Press 2 to View, Add to, or Remove from your Medical Inventory");
            Console.WriteLine("Type 'exit' to quit");
        }

        static void GroceryItemMenu(IInventoryLogic inventoryLogic)
        {
            bool exitCondition = false;
            while (!exitCondition)
            {
                DisplayInventoryItemMenu();
                string choice = GetUserInput().ToLower();
                switch (choice)
                {
                    case "1":
                        Console.WriteLine("The inventory has the following items: ");
                        Console.WriteLine();
                        var allItems = inventoryLogic.GetAllInventoryItems();
                        foreach (var item in allItems)
                        {
                            Console.WriteLine(JsonSerializer.Serialize(item));
                        }
                        Console.WriteLine();
                        break;
                    case "2":
                        var textinfo = new CultureInfo("en-US", false).TextInfo;
                        Console.WriteLine("Enter the Type.");
                        var type = textinfo.ToTitleCase(GetUserInput().ToLower());
                        var selectedItems = inventoryLogic.GetInventoryItemsByType(type);
                        foreach (var item in selectedItems)
                        {
                            Console.WriteLine(JsonSerializer.Serialize(item));
                        }
                        Console.WriteLine();
                        break;
                    case "3":
                        Console.WriteLine("Enter the item in JSON format:");
                        var inventoryItemAsJSON = GetUserInput();
                        var inventoryItem = JsonSerializer.Deserialize<InventoryItem>(inventoryItemAsJSON);
                        inventoryLogic.AddInventoryItem(inventoryItem);
                        Console.WriteLine();
                        Console.WriteLine($"Added {inventoryItem.Name} to medical items.");
                        Console.WriteLine();
                        break;
                    case "4":
                        Console.WriteLine("What is the name of the item you would like to update?");
                        var inventoryItemToUpdateName = GetUserInput();
                        var namedItemsToUpdate = inventoryLogic.GetInventoryItemsByName(inventoryItemToUpdateName);
                        if (namedItemsToUpdate.Count == 0)
                            Console.WriteLine("We do not carry that item.");
                        else if (namedItemsToUpdate.Count == 1)
                        {
                            Console.WriteLine(JsonSerializer.Serialize(namedItemsToUpdate[0]));
                            Console.WriteLine();
                            Console.Write("Enter the updated Name of the item: ");
                            namedItemsToUpdate[0].Name = Console.ReadLine();
                            Console.Write("Enter the updated Type of the item(DME, Anesthesia, Oxygen, or General Surgery): ");
                            namedItemsToUpdate[0].Type = Console.ReadLine();
                            Console.Write("Enter the updated Location of the item: ");
                            namedItemsToUpdate[0].Location = int.Parse(Console.ReadLine());
                            Console.Write("Enter the updated Price of the item: ");
                            namedItemsToUpdate[0].Price = decimal.Parse(Console.ReadLine());
                            inventoryLogic.UpdateInventoryItem(namedItemsToUpdate[0]);
                        }
                        else
                        {
                            foreach (var item in namedItemsToUpdate)
                            {
                                Console.WriteLine(JsonSerializer.Serialize(item));
                            }
                            Console.WriteLine();
                            Console.WriteLine($"Enter the Id of the {inventoryItemToUpdateName} you would like to update.");
                            var inventoryItemId = int.Parse(GetUserInput());
                            var namedItemsIds = namedItemsToUpdate.Select(x => x.InventoryItemId);
                            if (namedItemsIds.Contains(inventoryItemId))
                            {
                                var itemToUpdate = inventoryLogic.GetInventoryryItemById(inventoryItemId);
                                Console.WriteLine(JsonSerializer.Serialize(itemToUpdate));
                                Console.WriteLine();
                                Console.Write("Enter the updated Name of the item: ");
                                itemToUpdate.Name = Console.ReadLine();
                                Console.Write("Enter the updated Type of the item: ");
                                itemToUpdate.Type = Console.ReadLine();
                                Console.Write("Enter the updated Location of the item: ");
                                itemToUpdate.Location = int.Parse(Console.ReadLine());
                                Console.Write("Enter the updated Price of the item: ");
                                itemToUpdate.Price = decimal.Parse(Console.ReadLine());
                                inventoryLogic.UpdateInventoryItem(itemToUpdate);
                            }
                            else Console.WriteLine($"There is no {namedItemsToUpdate} with Id {inventoryItemId}.");
                        }
                        Console.WriteLine();
                        break;
                    case "5":
                        Console.WriteLine("What is the name of the medical item you would like to remove? ");
                        var inventoryItemToRemoveName = GetUserInput();
                        var namedItemsToRemove = inventoryLogic.GetInventoryItemsByName(inventoryItemToRemoveName);
                        if (namedItemsToRemove.Count == 0)
                            Console.WriteLine("We do not have that item.");
                        else if (namedItemsToRemove.Count == 1) inventoryLogic.RemoveInventoryItem(namedItemsToRemove[0]);
                        else
                        {
                            foreach (var item in namedItemsToRemove)
                            {
                                Console.WriteLine(JsonSerializer.Serialize(item));
                            }
                            Console.WriteLine();
                            Console.WriteLine($"Enter the Id of the {inventoryItemToRemoveName} you would like to remove.");
                            var inventoryItemId = int.Parse(GetUserInput());
                            var namedItemsIds = namedItemsToRemove.Select(x => x.InventoryItemId);
                            if (namedItemsIds.Contains(inventoryItemId))
                            {
                                var itemToRemove = inventoryLogic.GetInventoryItemById(inventoryItemId);
                                inventoryLogic.RemoveInventoryItem(itemToRemove);
                            }
                            else Console.WriteLine($"There is no {inventoryItemToRemoveName} with Id {inventoryItemId}.");
                        }
                        Console.WriteLine();
                        break;
                    case "back":
                        exitCondition = true;
                        break;
                    default:
                        Console.WriteLine("Invalid selection.");
                        break;
                }
            }
        }

        static void DisplayInventoryItemMenu()
        {
            Console.WriteLine("Press 1 to View the Medical Items");
            Console.WriteLine("Press 2 to View the Medical Items by Section");
            Console.WriteLine("Press 3 to Add a Medical Item as JSON");
            Console.WriteLine("Press 4 to Update a Medical Item");
            Console.WriteLine("Press 5 to Remove a Medical Item");
            Console.WriteLine("Type 'back' to return to the Menu");
            Console.Write("Choice: ");
        }

        static void InventoryListMenu(IInventoryLogic inventoryLogic)
        {
            bool exitCondition = false;
            while (!exitCondition)
            {
                DisplayInventoryListMenu();
                string choice = GetUserInput().ToLower();
                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Medical Inventory: ");
                        var inventoryList = inventoryLogic.GetInventoryList();
                        foreach (var item in inventoryList.InventoryItems)
                        {
                            Console.WriteLine(JsonSerializer.Serialize(item));
                        }
                        Console.WriteLine();
                        Console.WriteLine($"Total Price: {inventoryList.TotalPrice}");
                        Console.WriteLine();
                        break;
                    case "2":
                        Console.WriteLine("What is the name of the Medical item you would like to add? ");
                        var inventoryItemToAddName = GetUserInput();
                        var namedItemsToAdd = inventoryLogic.GetInventoryItemsByName(inventoryItemToAddName);
                        if (namedItemsToAdd.Count == 0) Console.WriteLine("We do not have that item.");
                        else if (namedItemsToAdd.Count == 1)
                        {
                            inventoryLogic.AddItemToInventoryList(namedItemsToAdd[0]);
                            Console.WriteLine($"Added {inventoryItemToAddName} to Medical items.");
                        }
                        else
                        {
                            foreach (var item in namedItemsToAdd)
                            {
                                Console.WriteLine(JsonSerializer.Serialize(item));
                            }
                            Console.WriteLine();
                            Console.WriteLine($"Enter the Id of the {inventoryItemToAddName} you would like to add.");
                            var inventoryItemId = int.Parse(GetUserInput());
                            var namedItemsIds = namedItemsToAdd.Select(x => x.InventoryItemId);
                            if (namedItemsIds.Contains(inventoryItemId))
                            {
                                inventoryLogic.AddItemToInventoryListById(inventoryItemId);
                                Console.WriteLine($"Added {inventoryItemToAddName} to medical items.");
                            }
                            else Console.WriteLine($"There is no {inventoryItemToAddName} with Id {inventoryItemId}.");
                        }
                        Console.WriteLine();
                        break;
                    case "3":
                        Console.WriteLine("What is the name of the medical item you would like to remove? ");
                        var inventoryItemToRemoveName = GetUserInput();
                        var namedItemsToRemove = inventoryLogic.GetInventoryItemsByName(inventoryItemToRemoveName);
                        if (namedItemsToRemove.Count == 0)
                            Console.WriteLine("We do not have that item.");
                        else if (namedItemsToRemove.Count == 1)
                        {
                            bool wasRemoved = inventoryLogic.RemoveInventoryItemFromListById(namedItemsToRemove[0].InventoryItemId);
                            if (wasRemoved)
                                Console.WriteLine($"{inventoryItemToRemoveName} was removed from your list.");
                            else
                                Console.WriteLine($"{inventoryItemToRemoveName} was not in your list.");
                        }
                        else
                        {
                            foreach (var item in namedItemsToRemove)
                            {
                                Console.WriteLine(JsonSerializer.Serialize(item));
                            }
                            Console.WriteLine();
                            Console.WriteLine($"Enter the Id of the {inventoryItemToRemoveName} you would like to remove.");
                            var inventoryItemId = int.Parse(GetUserInput());
                            var namedItemsIds = namedItemsToRemove.Select(x => x.InventoryItemId);
                            if (namedItemsIds.Contains(inventoryItemId))
                            {
                                bool wasRemoved = inventoryLogic.RemoveInventoryItemFromListById(inventoryItemId);
                                if (wasRemoved)
                                    Console.WriteLine($"{inventoryItemToRemoveName} was removed from your list.");
                                else
                                    Console.WriteLine($"{inventoryItemToRemoveName} was not in your list.");
                            }
                            else Console.WriteLine($"There is no {inventoryItemToRemoveName} with Id {inventoryItemId}.");
                        }
                        Console.WriteLine();
                        break;
                    case "4":
                        Console.WriteLine("The most expensive item in your grocery list is:");
                        Console.WriteLine();
                        var mostExpensiveItem = inventoryLogic.GetMostExpensiveItemInList();
                        Console.WriteLine(JsonSerializer.Serialize(mostExpensiveItem));
                        Console.WriteLine();
                        break;
                    case "5":
                        string emailAddress = EmailLogic.GetValidEmailAddress();
                        Console.WriteLine($"Your email address is : {emailAddress}");
                        Console.WriteLine();
                        Console.WriteLine("The email did not send because this feature has not yet implemented.");
                        Console.WriteLine();
                        break;
                    case "6":
                        Task saveFileTask = SaveInventoryListLogic.SaveInventoryListToFile(inventoryLogic.GetInventoryList());
                        Console.WriteLine("Your Inventory was saved. Look for it in the bin as inventorylist.txt");
                        Console.WriteLine();
                        break;
                    case "back":
                        exitCondition = true;
                        break;
                    default:
                        Console.WriteLine("Invalid selection.");
                        break;
                }
            }
        }

        static void DisplayIventoryListMenu()
        {
            Console.WriteLine("Press 1 to View the items in your  Medical Inventory");
            Console.WriteLine("Press 2 to Add an item to your Medical Inventory");
            Console.WriteLine("Press 3 to Remove an item from your Medical Inventory");
            Console.WriteLine("Press 4 to View the most expensive item from your Medical Inventory");
            Console.WriteLine("Press 5 to send your Medical Inventory to your email");
            Console.WriteLine("Press 6 to save your Medical Inventory to a text file");
            Console.WriteLine("Type 'back' to return to the Menu");
            Console.Write("Choice: ");
        }
    }
}
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


            public void AddItem(string name, int quantity, double price)
            {
                InventoryItem newItem = new InventoryItem
                {
                    Name = name,
                    Quantity = quantity,
                    Price = price
                };

                inventory.Add(newItem);

            }

            public void DisplayInventory()
            {
                Console.WriteLine("Inventory:");

                foreach (var item in inventory)
                {
                    Console.WriteLine($"Name: {item.Name}, Quantity: {item.Quantity}, Price: {item.Price}");
                }
            }

            // You can add more methods for updating, deleting, searching, etc.

        }


        class Program
        {
            static void Main()
            {
                InventoryManagementSystem ims = new InventoryManagementSystem();

                ims.AddItem("Exam Table", 10, 350.99);
                ims.AddItem("Anesthesia Machine", 5, 2599.99);

                // Displaying the current inventory
                ims.DisplayInventory();

                // Allowing the user to add more items
                Console.WriteLine("\nEnter new item details:");

                Console.Write("Enter item name: ");
                string itemName = Console.ReadLine();

                Console.Write("Enter quantity: ");
                int itemQuantity = int.Parse(Console.ReadLine());

                Console.Write("Enter price: ");
                double itemPrice = double.Parse(Console.ReadLine());

                ims.AddItem(itemName, itemQuantity, itemPrice);


                // Displaying the updated inventory
                ims.DisplayInventory();
                Console.WriteLine("Item added to inventory.");

                // Deleting an item
                Console.WriteLine("\nEnter the name of the item to delete:");
                string itemToDelete = Console.ReadLine();
                ims.DeleteItem(itemToDelete);

                // Displaying the updated inventory after deletion
                ims.DisplayInventory();

            }
        }
    }
}

