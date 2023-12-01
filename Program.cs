using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using InventoryList.Logic;
using InventoryList.Data;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Globalization;
using InventoryList.UI;

internal class Program
{
    private static void Main(string[] args)
    {
        var services = CreateServiceCollection();

        var groceryLogic = services.GetService<IGroceryLogic>();

        UserInterface.WelcomeBanner();

        UserInterface.MainMenu(groceryLogic);
    }

    static IServiceProvider CreateServiceCollection()
    {
        return new ServiceCollection()
            .AddTransient<IGroceryLogic, GroceryLogic>()
            .AddTransient<IGroceryItemRepository, GroceryItemRepository>()
            .BuildServiceProvider();
    }

}


