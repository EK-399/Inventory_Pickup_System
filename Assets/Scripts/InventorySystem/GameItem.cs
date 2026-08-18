using UnityEngine;

namespace InventorySystem
{
    public class GameItem : MonoBehaviour
    {
        [SerializeField]
        private ItemStack _stack;
        
        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        public ItemStack Stack => _stack;

        private void OnValidate()
        {
            SetupGameObject();
        }

        private void SetupGameObject()
        {
            if (_stack.Item == null) return;
            SetGameSprite();
            AdjustNumberOfItems();
            UpdateGameObjectName();
        }

        private void SetGameSprite()
        {
            _spriteRenderer.sprite = _stack.Item.InGameSprite;
        }

        private void UpdateGameObjectName()
        {
            //ItemName (5)/(ns)Not stackable
            var name = _stack.Item.name;
            var number = _stack.IsStackable ? _stack.NumberOfItems.ToString() : "ns";
            gameObject.name = $"{name} ({number})";
        }

        private void AdjustNumberOfItems()
        {
            _stack.NumberOfItems = _stack.NumberOfItems;
        }

        public ItemStack Pick()
        {
            Destroy(gameObject);
            return _stack;
        }
    }
}
