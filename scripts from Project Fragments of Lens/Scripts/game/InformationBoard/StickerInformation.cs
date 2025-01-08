using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.game.InformationBoard
{
    [CreateAssetMenu]
    public class StickerInformation : ScriptableObject
    {
        public enum ID
        {
            None = 0,
            [InspectorName("Photo/PhotoOfDaughter")]
            PhotoOfDaughter = 1,
            [InspectorName("Photo/PhotoOfDad")]
            PhotoOfDad = 2,
            [InspectorName("Photo/PhotoOfMum")]
            PhotoOfMum = 3,
            [InspectorName("Photo/FamilyPhoto")]
            FamilyPhoto = 4,
            [InspectorName("Photo/TicketPhoto")]
            TicketPhoto = 5,
            [InspectorName("Photo/TableDirnkBottle")]
            TableDirnkBottle = 6,
            [InspectorName("Photo/PhotoOfMistress")]
            PhotoOfMistress = 7,
            [InspectorName("Photo/SleepingPillImage")]
            SleepingPillImage = 8,
            [InspectorName("Photo/DaughterPhoneImage")]
            DaughterPhoneImage = 9,
            [InspectorName("Photo/SomeoneBurried")]
            SomeoneBurried = 10,
            [InspectorName("Photo/PhoneOnDaughterDesk")]
            PhoneOnDaughterDesk = 11,
            [InspectorName("Photo/MouseLeftButton")]
            MouseLeftButton =12,
            [InspectorName("Photo/MouseRightButton")]
            MouseRightButton = 13,
            [InspectorName("Photo/MouseMidButtonA")]
            MouseMidButtonA = 14,
            [InspectorName("Photo/MouseMidButtonB")]
            MouseMidButtonB = 15,

            [InspectorName("Item/TeaCup")]
            TeaCup = 51,
            [InspectorName("Item/Ring")]
            Ring = 52,
            [InspectorName("Item/DadIsOnThePhone")]
            DadIsOnThePhone = 53,
            [InspectorName("Item/MumSpiked")]
            MumSpiked = 54,
            [InspectorName("Item/MumArgue")]
            MumArgue = 55,
            [InspectorName("Item/DadReplaced")]
            DadReplaced = 56,
            [InspectorName("Item/MumTryCheck")]
            MumTryCheck = 57,
            [InspectorName("Item/Daughterluggage")]
            Daughterluggage = 58,
            [InspectorName("Item/MumFindPlan")]
            MumFindPlan = 60,
            [InspectorName("Item/DaughterFoundLoveAffair")]
            DaughterFoundLoveAffair = 61,
            [InspectorName("Item/MumScared")]
            MumScared = 63,
            [InspectorName("Item/DaughterMissed")]
            DaughterMissed = 64,
            [InspectorName("Item/DaughterDead")]
            DaughterDead = 65,
            [InspectorName("Item/LeftDes")]
            LeftDes = 66,
            [InspectorName("Item/RightDes")]
            RightDes = 67,
            [InspectorName("Item/MidDesA")]
            MidDesA = 68,
            [InspectorName("Item/MidDesB")]
            MidDesB = 71,
            [InspectorName("Item/StartGame")]
            StartGame =70,

            [InspectorName("Question/WhyMistress")]
            WhyMistress = 101,
            [InspectorName("Question/WhoCalling")]
            WhoCalling = 102,
            [InspectorName("Question/WhyArugue")]
            WhyArugue = 103,
            [InspectorName("Question/MumBurried")]
            MumBurried = 104,
            [InspectorName("Question/DadBurried")]
            DadBurried = 105,
            [InspectorName("Question/OriginInBottle")]
            OriginInBottle = 106,
            [InspectorName("Question/WhatBottleMum")]
            WhatBottleMum = 107,
            [InspectorName("Question/WhyMumWant")]
            WhyMumWant = 108,
            [InspectorName("Question/WhatDrugMum")]
            WhatDrugMum = 109,
            [InspectorName("Question/IsMumCorrect")]
            IsMumCorrect = 110,
            [InspectorName("Question/WhosePhone")]
            WhosePhone = 111,
            [InspectorName("Question/WhyMumScared")]
            WhyMumScared = 112,
            [InspectorName("Question/WhyDaughterPrepared")]
            WhyDaughterPrepared = 113,
            [InspectorName("Question/FindOutTruth")]
            FindOutTruth = 114,
        }

        public enum Type//决定用什么贴纸
        {
            Item,//道具
            Question,//问题
            Hypothesis,//猜测
        }


        public enum PlaceMethod
        {
            Auto,//得到了就自动放置在面板上
            Manual,//得到了可以进行推理
        }

        [System.Serializable]
        public class QuestionData//合法的推理目标信息
        {
            public ID id;
        }

        public ID id;
        public StickerBehaviour stickerPrefab;
        public GameObject PinPrefab;
        [Header("如果这个贴纸可生成的问题，则指定问题")]
        public QuestionData[] targetIds;
        public Type type;
        public PlaceMethod placeMethod;
        [Header("如果这个是自动出现在面板，则需要制定初始位置")]
        public Vector2 initialPosition;
        [Header("如果这个贴纸有图片，则指定一个图片")]
        public Sprite sp;
        [Header("如果这个贴纸显示的文字内容")]
        public string txt;
        
    }
}