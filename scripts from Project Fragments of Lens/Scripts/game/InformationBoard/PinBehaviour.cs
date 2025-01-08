using UnityEngine;

namespace Assets.Scripts.game.InformationBoard
{
    public class PinBehaviour : MonoBehaviour
    {
       
        public StickerInformation.ID StickerId;

        // 初始化方法，用于设置 StickerId
        public void Initialize(StickerInformation.ID stickerId)
        {
            StickerId = stickerId;
        }

        
    }
}
