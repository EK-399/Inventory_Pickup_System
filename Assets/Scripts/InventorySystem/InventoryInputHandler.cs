using UnityEngine;
using UnityEngine.InputSystem;

namespace InventorySystem
{
    public class InventoryInputHandler : MonoBehaviour
    {
        private Inventory _inventory;

        private void Awake()
        {
            _inventory = GetComponent<Inventory>();
        }

        private void OnEnable()
        {
            PlayerMovement.Instance.Game.ThrowItem.performed += OnThrowItem;
            PlayerMovement.Instance.Game.NextItem.performed += OnNextItem;
            PlayerMovement.Instance.Game.PreviousItem.performed += OnPreviousItem;
        }

        private void OnDisable()
        {
            PlayerMovement.Instance.Game.ThrowItem.performed += OnThrowItem;
            PlayerMovement.Instance.Game.NextItem.performed += OnNextItem;
            PlayerMovement.Instance.Game.PreviousItem.performed += OnPreviousItem;
        }

        private void OnThrowItem(InputAction.CallbackContext context)
        {
            //
        }

        private void OnNextItem(InputAction.CallbackContext context)
        {
            _inventory.ActivateSlot(_inventory.ActiveSlotIndex - 1);
        }

        private void OnPreviousItem(InputAction.CallbackContext context)
        {
            _inventory.ActivateSlot(_inventory.ActiveSlotIndex - 1);
        }
    }
}
