using Duckov.UI;
using Duckov.Utilities;
using ItemStatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
            // 检测鼠标左键按下（类似于Python的if input.get_mouse_button_down(0)）
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