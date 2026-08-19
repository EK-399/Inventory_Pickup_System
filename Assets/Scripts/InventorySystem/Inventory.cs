using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace InventorySystem
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField]
        private int _size = 8;

        [SerializeField]
        private List<InventorySlot> _slots;

        private int _activeSlotIndex;

        public int Size => _size;
        public List<InventorySlot> Slots => _slots;

        public int ActiveSlotIndex
        {
            get => _activeSlotIndex;
            private set
            {
                _slots[_activeSlotIndex].Active = false;
                _activeSlotIndex = value < 0 ? _size - 1 : value % Size;
                _slots[_activeSlotIndex].Active = true;

                //ammount of the inventory slots = 3 [Showcase of scrolling between slots]
                //0 -> 1 || 1 % 3 = 1 => 3 * 0 = 0 || 1 - 0 = 1
                //1 -> 2 || 2 % 3 = 2 => 3 * 0 = 0 || 2 - 0 = 2
                //1 -> 3 || 3 % 3 = 0 => 3 * 1 = 3 || 3 - 3 = 0
                //0 -> 1
            }
        }

        private void Awake()
        {
            if (_size > 0)
            {
                _slots[0].Active = true;
            }
        }

        private void OnValidate()
        {
            AdjustSize();
        }

        //Making the Size of the Slots
        private void AdjustSize()
        {
            _slots ??= new List<InventorySlot>();
            if (_slots.Count > _size) _slots.RemoveRange(_size, _slots.Count - _size);
            //_slots.Count = 12, _size = 8, Remove Items starting at position 8, 12 - 8 = 4
            if (_slots.Count < _size) _slots.AddRange(new InventorySlot[_size - _slots.Count]);
            //_slots.Count = 3, _size = 8, Hey, add to the list 8 - 3 = 5 items
        }

        //Defing is item is stackable or not
        public bool IsFull()
        {
            return _slots.Count(slot => slot.HasItem) >= _size;
        }

        //Can store stackable items in the same slot, so we need to check if the item is stackable or not
        public bool CanAcceptItem(ItemStack itemStack)
        {
            var slotWithStackableItem = FindSlot(itemStack.Item, true);
            return !IsFull() || slotWithStackableItem != null;
        }

        private InventorySlot FindSlot(ItemDefinition item, bool onlyStackable = false)
        {
            //planks, true -> true // xxxx
            //health potion, true -> false // false
            // health potion, false -> false // true
            return _slots.FirstOrDefault(slot => slot.Item == item && 
                                                 item.IsStackable || 
                                                 !onlyStackable);
        }

        public bool HasItem(ItemStack itemStack, bool checkNumberOfItems = false)
        {
            var itemSlot = FindSlot(itemStack.Item);
            if (itemSlot == null) return false;
            if (!checkNumberOfItems) return false;
            if (itemStack.Item.IsStackable)
            {
                return itemSlot.NumberOfItems >= itemStack.NumberOfItems;
            }
            
            return _slots.Count(slot => slot.Item == itemStack.Item) >= itemStack.NumberOfItems;
        }

        public ItemStack AddItem(ItemStack itemStack)
        {
            var relevantSlot = FindSlot(itemStack.Item, true);
            if (IsFull() && relevantSlot == null)
            {
                //Full Inventory, OH NOOOO
                throw new InventoryException(InventoryOperation.Add, "Inventory is full");
            }

            if (relevantSlot != null)
            { 
                relevantSlot.NumberOfItems += itemStack.NumberOfItems;
            }
            else
            {
                relevantSlot = _slots.First(slot => !slot.HasItem);
                relevantSlot.State = itemStack;
            }

            return relevantSlot.State; 
        }

        public ItemStack RemoveItem(int atIndex, bool spawn = false)
        {
            if (!_slots[atIndex].HasItem)
                throw new InventoryException(InventoryOperation.Remove, "Slot is Emtpy");

            if (spawn)
            {
                //
            }

            ClearSlot(atIndex);
            return new ItemStack();
        }

        public ItemStack RemoveItem(ItemStack itemStack)
        {
            var itemSlot = FindSlot(itemStack.Item);
            if (itemSlot == null)
                throw new InventoryException(InventoryOperation.Remove, "No Item in the Inventory");
            if (itemSlot.Item.IsStackable && itemSlot.NumberOfItems < itemStack.NumberOfItems)
                throw new InventoryException(InventoryOperation.Remove, "Not enough Items");

            itemSlot.NumberOfItems -= itemStack.NumberOfItems;
            if (itemSlot.Item.IsStackable && itemSlot.NumberOfItems > 0)
            {
                return itemSlot.State;
            }

            itemSlot.Clear();
            return new ItemStack();
        }

        public void ClearSlot(int atIndex)
        {
            _slots[atIndex].Clear();
        }

        public void ActivateSlot(int atIndex)
        {
            ActiveSlotIndex = atIndex;
        }
    }
}
