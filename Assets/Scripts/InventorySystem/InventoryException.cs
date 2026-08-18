using System;

namespace InventorySystem
{
    public enum InventoryOperation
    {
        Add,
        Remove
    }

    //Add, Inventory is Full -> Add Error: Inventory is full
    public class InventoryException : Exception
    {
        public InventoryOperation Operation { get; }
        public InventoryException(InventoryOperation operation, string msg) : base($"{operation} Error: { msg}") //tells me if inventory is full
        {
            Operation = operation;
        }

    }
}
