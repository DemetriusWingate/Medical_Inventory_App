using System;
using InventoryList.Validators;
using InventoryList.Data;
using System.ComponentModel.DataAnnotations;

namespace InventoryList.Logic
{
    public class InventoryLogic : IInventoryLogic
    {
        private readonly IInventoryItemRepository _inventoryItemRepo;
        public InventoryList _inventoryList;

        public InventoryLogic(IInventoryItemRepository inventoryItemRepo)
        {
            _inventoryItemRepo = inventoryItemRepo;
            _inventoryItemRepo.SeedInventoryItems();
            _inventoryList = new InventoryList();
        }

        public void AddInventoryItem(InventoryItem item)
        {
            var validator = new InventoryItemValidator();
            if (validator.Validate(item).IsValid)
            {
                _inventoryItemRepo.AddInventoryItem(item);
            }
            else
            {
                throw new ValidationException("The item is not a valid inventory item");
            }
        }

        public void RemoveInventoryItem(InventoryItem item)
        {
            _inventoryItemRepo.RemoveInventoryItem(item);
        }

        public void UpdateIventoryItem(InventoryItem item)
        {
            _inventoryItemRepo.UpdateInventoryItem(item);
        }

        public List<InventoryItem> GetAllInventoryItems()
        {
            return _inventoryItemRepo.GetAllInventoryItems();
        }

        public InventoryItem GetInventoryItemById(int id)
        {
            return _inventoryItemRepo.GetInventoryItemById(id);
        }

        // Adds the item to the sorted inventory list in the correct index
        public void AddItemToInventoryList(InventoryItem item)
        {
            int index = GetIndexToInsertByTypeLocation(item);
            _inventoryList.InventoryItems.Insert(index, item);
            _inventoryList.TotalPrice += item.Price;
        }

        public void AddItemToInventoryListById(int id)
        {
            AddItemToInventoryList(GetInventoryItemById(id));
        }

        public bool RemoveInventoryItemFromListById(int id)
        {
            var item = GetInventoryItemById(id);
            var wasRemoved = _inventoryList.InventoryItems.Remove(item);
            if (wasRemoved) _inventoryList.TotalPrice -= item.Price;
            return wasRemoved;
        }

        public InventoryList GetInventoryList()
        {
            return _inventoryList;
        }

        // Finds the correct index to insert the new item
        public int GetIndexToInsertByTypeLocation(InventoryItem item)
        {
            if (_inventoryList.InventoryItems.Count == 0) return 0;
            List<string> sections = new List<string> { "DME", "Anesthesia", "Oxygen", "General Surgery" };
            for (int index = 0; index < _inventoryList.InventoryItems.Count; index++)
            {
                if (types.IndexOf(item.Type) > sections.IndexOf(_inventoryList.InventoryItems[index].Type)) continue;
                if (types.IndexOf(item.Type) == sections.IndexOf(_inventoryList.InventoryItems[index].Type)
                    && item.Location > _inventoryList.InventoryItems[index].Location) continue;
                return index;
            }
            return _inventoryList.InventoryItems.Count;
        }

        public List<InventoryItem> GetInventoryItemsByName(string name)
        {
            var namedItems = _inventoryItemRepo.GetInventoryItemsByName(name);
            return namedItems;
        }

        public List<InventoryItem> GetInventoryItemsByType(string type)
        {
            var typeItems = _inventoryItemRepo.GetInventoryItemsBySection(type);
            return typeItems;
        }

        public InventoryItem GetMostExpensiveItemInList()
        {
            if (_inventoryList.InventoryItems.Count == 0) return null;
            InventoryItem expensiveItem = _inventoryList.InventoryItems[0];
            foreach (var item in _inventoryList.InventoryItems)
            {
                if (item.Price > expensiveItem.Price) expensiveItem = item;
            }
            return expensiveItem;
        }
    }
}
