using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

namespace Assets.Scripts.game.InformationBoard
{
    public class StickerGenerator : MonoBehaviour
    {
        public StickerInformation stickerInfo;
        [Header("�Ƿ���Ҫ��Ч")]
        public bool requiresVFX = true;

        [Header("�Ƿ���Ҫ��ʾ UI ��ʾ")]
        public bool showUI = false;
        public bool needNotice = true;

        public TextMeshProUGUI uiText; // TextMeshPro ���
        public Image uiImage; // ������ Image ���

        private bool uiHasFaded = false; // ��� UI �Ƿ��Ѿ���ʾ��

        public void GenerateSticker()
        {
            Debug.Log("StickerGenerator: GenerateSticker called.");

            if (stickerInfo == null)
            {
                Debug.LogError("StickerInformation is not assigned.");
                return;
            }

            // ���� InformationBoardSystem ���� Sticker ��ִ����Ч
            InformationBoardSystem.instance.DiscoverNewSticker(stickerInfo.id, requiresVFX, needNotice);

            // ��鲢��ʾ UI ��ʾ������ Text �� Image��
            if (showUI && !uiHasFaded)
            {
                FadeInUIElements();
            }
        }

        // ������ UI Ԫ�ظ��ַ���
        private void FadeInUIElements()
        {
            if (uiText != null)
            {
                uiText.alpha = 0f; // ��ʼ�� Text ��͸����
                uiText.DOFade(1f, 1f); // Text 1���ڵ���
            }

            if (uiImage != null)
            {
                
                uiImage.color = new Color(uiImage.color.r, uiImage.color.g, uiImage.color.b, 0); // ��ʼ�� Image ͸����
                uiImage.DOFade(1f, 1f); // Image 1���ڵ���
            }

            // ��� UI ����ʾ��
            uiHasFaded = true;
            Debug.Log("UI elements have been revealed.");
        }

        public void RemoveSticker()
        {
            Debug.Log("Attempting to remove previous sticker.");

            // ���� InformationBoardSystem �� RemoveStickerByID ����ɾ�� Sticker
            InformationBoardSystem.instance.RemoveStickerByID(stickerInfo.id);
        }
    }
}
