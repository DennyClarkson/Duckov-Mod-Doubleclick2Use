using Duckov.UI;
using Duckov.Utilities;
using ItemStatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace Double
{
    public class ModBehaviour : Duckov.Modding.ModBehaviour
    {
        private float lastClickTime = 0f;
        private const float doubleClickThreshold = 0.3f; // 双击时间阈值（秒）

        void Awake()
        {
            Debug.Log("Double Loaded");
        }

        private void Update()
        {
            HandleRightClickEvents();
            HandleMiddleClick(); // 处理中键点击
        }

        private void HandleMiddleClick()
        {
            // 检测鼠标中键按下
            if (Mouse.current.middleButton.wasPressedThisFrame)
            {
                // 通过事件系统获取鼠标下的ItemDisplay
                ItemDisplay itemDisplay = GetItemDisplayUnderMouse();
                if (itemDisplay != null && itemDisplay.Target != null)
                {
                    OnMiddleClickItem(itemDisplay);
                }
            }
        }

        private ItemDisplay GetItemDisplayUnderMouse()
        {
            // 使用事件系统检测鼠标下的UI元素
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = Mouse.current.position.ReadValue();

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            foreach (RaycastResult result in results)
            {
                ItemDisplay itemDisplay = result.gameObject.GetComponent<ItemDisplay>();
                if (itemDisplay != null)
                {
                    return itemDisplay;
                }
                
                // 如果直接没找到，可能在子对象中
                itemDisplay = result.gameObject.GetComponentInParent<ItemDisplay>();
                if (itemDisplay != null)
                {
                    return itemDisplay;
                }
            }

            return null;
        }

        private void OnMiddleClickItem(ItemDisplay itemDisplay)
        {
            Debug.Log("中键点击物品，尝试快速丢弃");
            
            Item targetItem = itemDisplay.Target;
            if (targetItem == null)
            {
                Debug.Log("目标物品为空");
                return;
            }

            // 检查物品是否可以丢弃
            if (!targetItem.CanDrop)
            {
                Debug.Log("物品不可丢弃");
                return;
            }

            // 检查物品的根物品是否是当前角色的物品
            Item characterItem = LevelManager.Instance?.MainCharacter?.CharacterItem;
            if (targetItem.GetRoot() != characterItem)
            {
                Debug.Log("物品不在角色身上，无法丢弃");
                return;
            }

            // 直接调用物品的Drop方法，模拟Dump功能
            if (LevelManager.Instance?.MainCharacter != null)
            {
                targetItem.Drop(LevelManager.Instance.MainCharacter, true);
                Debug.Log($"中键快速丢弃: {targetItem.DisplayName}");
            }
        }

        private void HandleRightClickEvents()
        {
            // 检测鼠标右键按下
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                float currentTime = Time.time;

                // 检查是否是双击
                if (currentTime - lastClickTime < doubleClickThreshold)
                {
                    OnDoubleClick();
                }

                lastClickTime = currentTime;
            }
        }

        private void OnDoubleClick()
        {
            Debug.Log("检测到双击右键，模拟使用物品");
    
            ItemOperationMenu menu = ItemOperationMenu.Instance;
            if (menu == null) return;

            // 使用反射调用私有Use方法
            var useMethod = typeof(ItemOperationMenu).GetMethod("Use", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
    
            useMethod?.Invoke(menu, null);
        }
    }
}