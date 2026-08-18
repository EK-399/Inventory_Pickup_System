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
    }
}
