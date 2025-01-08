using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

namespace Assets.Scripts.game.InformationBoard
{
    public class StickerGenerator : MonoBehaviour
    {
        public StickerInformation stickerInfo;
        [Header("是否需要特效")]
        public bool requiresVFX = true;

        [Header("是否需要显示 UI 提示")]
        public bool showUI = false;
        public bool needNotice = true;

        public TextMeshProUGUI uiText; // TextMeshPro 组件
        public Image uiImage; // 新增的 Image 组件

        private bool uiHasFaded = false; // 标记 UI 是否已经显示过

        public void GenerateSticker()
        {
            Debug.Log("StickerGenerator: GenerateSticker called.");

            if (stickerInfo == null)
            {
                Debug.LogError("StickerInformation is not assigned.");
                return;
            }

            // 调用 InformationBoardSystem 生成 Sticker 并执行特效
            InformationBoardSystem.instance.DiscoverNewSticker(stickerInfo.id, requiresVFX, needNotice);

            // 检查并显示 UI 提示（包括 Text 和 Image）
            if (showUI && !uiHasFaded)
            {
                FadeInUIElements();
            }
        }

        // 新增的 UI 元素浮现方法
        private void FadeInUIElements()
        {
            if (uiText != null)
            {
                uiText.alpha = 0f; // 初始化 Text 的透明度
                uiText.DOFade(1f, 1f); // Text 1秒内淡入
            }

            if (uiImage != null)
            {
                
                uiImage.color = new Color(uiImage.color.r, uiImage.color.g, uiImage.color.b, 0); // 初始化 Image 透明度
                uiImage.DOFade(1f, 1f); // Image 1秒内淡入
            }

            // 标记 UI 已显示过
            uiHasFaded = true;
            Debug.Log("UI elements have been revealed.");
        }

        public void RemoveSticker()
        {
            Debug.Log("Attempting to remove previous sticker.");

            // 调用 InformationBoardSystem 的 RemoveStickerByID 方法删除 Sticker
            InformationBoardSystem.instance.RemoveStickerByID(stickerInfo.id);
        }
    }
}
