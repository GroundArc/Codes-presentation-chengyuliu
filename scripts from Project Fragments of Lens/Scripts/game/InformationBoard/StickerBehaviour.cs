using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Assets.Scripts.game.InformationBoard
{
    public class StickerBehaviour : MonoBehaviour
    {
        public TextMeshProUGUI txt;
        public UnityEngine.UI.Image image;
        public enum State
        {
            None,
            DiscoverAnimation,
            OnBoard,
        }

        public State state;

        public Transform model; // 指向 model 子对象
        public Transform PinPosition;

        public bool movable;
        public bool canReceiveDragTarget;

        public StickerInformation.ID id;

        public void Init(StickerInformation proto)
        {
            // 获取并设置 TextMeshProUGUI 组件
            txt = GetComponentInChildren<TextMeshProUGUI>();
            if (txt != null)
            {
                txt.text = proto.txt;
            }
            else
            {
                Debug.LogWarning("TextMeshProUGUI component is missing on this Sticker prefab.");
            }

            // 获取并设置 Image 组件
            image = GetComponentInChildren<Image>();
            if (image != null)
            {
                image.sprite = proto.sp;
            }
            else
            {
                Debug.Log("Image component is missing on this Sticker prefab. Only text will be displayed.");
            }

            // 设置其他属性
            id = proto.id;
            state = StickerBehaviour.State.OnBoard;
        }
    }
}
