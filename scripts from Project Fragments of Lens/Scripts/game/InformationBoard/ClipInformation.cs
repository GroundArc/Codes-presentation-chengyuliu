using System.Collections;
using UnityEngine;

namespace Assets.Scripts.game.InformationBoard
{
    public class ClipInformation : MonoBehaviour
    {
        public Data[] dataArray;


        [System.Serializable]
        public class Data
        {
            public float timeStamp;
            public GameObject clickable;
            public StickerInformation.ID targetSticker;
        }
    }
}