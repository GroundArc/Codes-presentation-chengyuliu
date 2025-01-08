using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; 
using static Assets.Scripts.game.InformationBoard.StickerInformation;
using System.Linq;

namespace Assets.Scripts.game.InformationBoard
{
    public class InformationBoardSystem : MonoBehaviour
    {
        public static InformationBoardSystem instance { get; private set; }

        public List<StickerInformation> prototypes;
        public Transform plane; // plane 的引用，用于确定生成区域
        public Transform boardPlane; // 用于拖拽的 plane
        public GameObject pinPrefab; // Pin 预制体
        public Transform stickerInitialPosition;

        public Image clueAlreadyObtainedText;
        public Image newStickerImageUI;

        private Vector3 _planeSize;
        private float _minX;
        private float _maxX;
        private float _minY;
        private float _maxY;

        public CaptureVideoFeedback cvf;

        // 新增的列表，用于存储已经生成的Sticker ID
        public List<StickerInformation.ID> generatedStickers = new List<StickerInformation.ID>();

        private void Awake()
        {
            instance = this;

            Renderer planeRenderer = plane.GetComponent<Renderer>();
            Vector3 planeSize = planeRenderer.bounds.size;
            _minX = plane.position.x - planeSize.x / 2;
            _maxX = plane.position.x + planeSize.x / 2;
            _minY = plane.position.y - planeSize.y / 2;
            _maxY = plane.position.y + planeSize.y / 2;

            // 确保UI组件初始为不显示状态
            if (clueAlreadyObtainedText != null)
            {
                clueAlreadyObtainedText.gameObject.SetActive(false);
            }
        }

        public GameObject GetBoardPlane()
        {
            return boardPlane.gameObject;
        }

        public StickerInformation GetPrototype(StickerInformation.ID id)
        {
            foreach (var p in prototypes)
            {
                if (p.id == id)
                    return p;
            }
            return null;
        }

        public bool ContainsPrototype(StickerInformation.ID id)
        {
            return prototypes.Exists(p => p.id == id);
        }

        public void AddPrototype(StickerInformation stickerInfo)
        {
            if (!ContainsPrototype(stickerInfo.id))
            {
                prototypes.Add(stickerInfo);
            }
        }

        public void DiscoverNewSticker(ID id, bool ifvfx, bool neednotice)
        {
            Debug.Log("DiscoverNewSticker " + id);
            CreateSticker(GetPrototype(id), ifvfx, neednotice);
           
        }

        public void GenerateAllStickers()
        {
            foreach (var proto in prototypes)
            {
                CreateSticker(proto, false, false);
            }
        }

        private void CreateSticker(StickerInformation proto, bool alsoCreateStickerVfx, bool alsoNeedNotice)
        {
            if (proto == null)
            {
                Debug.LogError("StickerInformation is null. Cannot create sticker.");
                return;
            }

            if (proto.stickerPrefab == null)
            {
                Debug.LogError("Sticker prefab is null in StickerInformation.");
                return;
            }

            // 检查是否已经生成过此Sticker
            if (generatedStickers.Contains(proto.id))
            {
                Debug.LogWarning("Sticker with ID " + proto.id + " has already been generated. Skipping.");
                TriggerClueAlreadyObtainedUI(); // 触发UI
                return;
            }

            Debug.Log("CreateSticker " + proto.id);
            Vector3 stickerPosition;

            if (proto.placeMethod == PlaceMethod.Auto)
            {
                // 根据初始位置生成
                stickerPosition = new Vector3(proto.initialPosition.x, proto.initialPosition.y, (float)-1.536);
            }
            else if (proto.placeMethod == PlaceMethod.Manual)
            {
                // 随机生成一个位置在区域内
                stickerPosition = new Vector3(Random.Range(_minX, _maxX), Random.Range(_minY, _maxY), (float)-1.536);
            }
            else
            {
                // 默认位置
                stickerPosition = new Vector3(0, 0, (float)-1.536);
            }

            StickerBehaviour sticker = Instantiate(proto.stickerPrefab, stickerPosition, Quaternion.identity);
            sticker.Init(proto); // 设置贴纸的初始属性

            // 生成问题贴纸（QuestionSticker）
            if (proto.targetIds != null && proto.targetIds.Length > 0)
            {
                GenerateQuestionStickers(proto, stickerPosition, sticker);
            }

            // Instantiate the Pin as a child of the Sticker
            if (pinPrefab != null && sticker.PinPosition != null)
            {
                GameObject pin = Instantiate(pinPrefab, sticker.model);
                pin.transform.position = sticker.PinPosition.position; // Set the position of the Pin to match PinPosition
                pin.transform.rotation = sticker.PinPosition.rotation; // Optionally set the rotation to match PinPosition
                pin.transform.localScale = sticker.PinPosition.localScale; // Optionally set the scale to match PinPosition

                // Get the PinBehaviour component and set the StickerId
                PinBehaviour pinBehaviour = pin.GetComponent<PinBehaviour>();
                if (pinBehaviour != null)
                {
                    pinBehaviour.Initialize(proto.id); // Set the StickerId to the current Sticker's ID
                }
                else
                {
                    Debug.LogWarning("PinPrefab is missing PinBehaviour component.");
                }
                // 调用 RopeManager 尝试自动连接 Sticker
                RopeManager.Instance.TryAutoConnectStickers();
            }

            // 生成成功后，将Sticker ID存储到 generatedStickers 列表中
            generatedStickers.Add(proto.id);

            if (alsoCreateStickerVfx)
                cvf.Capture(proto);
            if (alsoNeedNotice)
                TriggerNewStickerUI();
        }

        // 生成QuestionSticker并排列
        private void GenerateQuestionStickers(StickerInformation proto, Vector3 stickerPosition, StickerBehaviour sticker)
        {
            float yOffset = 0.09f; // 每个问题贴纸之间的Y轴偏移量
            float startY = stickerPosition.y - yOffset; // 初始Y轴位置

            for (int i = 0; i < proto.targetIds.Length; i++)
            {
                // 获取目标Sticker的信息
                StickerInformation questionStickerProto = GetPrototype(proto.targetIds[i].id);

                if (questionStickerProto != null && questionStickerProto.stickerPrefab != null)
                {
                    // 计算每个问题贴纸的生成位置
                    Vector3 questionStickerPosition = new Vector3(stickerPosition.x, startY - (i * yOffset), stickerPosition.z);

                    // 生成问题贴纸
                    StickerBehaviour questionSticker = Instantiate(questionStickerProto.stickerPrefab, questionStickerPosition, Quaternion.identity);
                    questionSticker.Init(questionStickerProto); // 设置问题贴纸的初始属性

                    // 生成Pin
                    if (pinPrefab != null && questionSticker.PinPosition != null)
                    {
                        GameObject pin = Instantiate(pinPrefab, questionSticker.model);
                        pin.transform.position = questionSticker.PinPosition.position; // 设置Pin的位置
                        pin.transform.rotation = questionSticker.PinPosition.rotation; // 设置Pin的旋转
                        pin.transform.localScale = questionSticker.PinPosition.localScale; // 设置Pin的缩放

                        // 获取PinBehaviour组件并设置StickerId
                        PinBehaviour pinBehaviour = pin.GetComponent<PinBehaviour>();
                        if (pinBehaviour != null)
                        {
                            pinBehaviour.Initialize(questionStickerProto.id); // 设置Pin的StickerId
                        }
                        else
                        {
                            Debug.LogWarning("PinPrefab is missing PinBehaviour component.");
                        }
                    }

                    // 添加到generatedStickers列表，避免重复生成
                    generatedStickers.Add(questionStickerProto.id);
                }
                else
                {
                    Debug.LogWarning($"Question Sticker with ID {proto.targetIds[i].id} not found or missing prefab.");
                }
            }
        }

        // 生成新的 Sticker 时，触发一个 Image 的移动动画
        private void TriggerNewStickerUI()
        {
            if (newStickerImageUI != null)
            {
                // 设置初始显示位置
                float initialPosX = 1270;
                float targetPosX = initialPosX - 500f; // 向左移动 500 个单位

                // 显示 Image
                newStickerImageUI.gameObject.SetActive(true);

                // 使用 DoTween 让 Image 平滑移动到目标位置
                newStickerImageUI.rectTransform
                    .DOAnchorPosX(targetPosX, 1f) // 移动到目标位置
                    .SetEase(Ease.OutCubic)
                    .OnComplete(() =>
                    {
                        // 保持 4 秒显示时间后返回原来位置
                        DOVirtual.DelayedCall(4f, () =>
                        {
                            newStickerImageUI.rectTransform
                                .DOAnchorPosX(initialPosX, 1f) // 返回原来的位置
                                .SetEase(Ease.OutCubic)
                                .OnComplete(() =>
                                {
                                    // 返回初始位置后隐藏 Image
                                    newStickerImageUI.gameObject.SetActive(false);
                                });
                        });
                    });
            }
        }

        public void DiscoverNewStickerAtPosition(ID id, Vector3 position)
        {
            StickerInformation stickerInfo = GetPrototype(id);
            if (stickerInfo != null)
            {
                CreateStickerAtPosition(stickerInfo, position, false);
            }
            
        }

        private void CreateStickerAtPosition(StickerInformation proto, Vector3 finalPosition, bool alsoCreateStickerVfx)
        {
            if (proto == null || proto.stickerPrefab == null)
            {
                Debug.LogError("Sticker prefab is null in StickerInformation.");
                return;
            }

            // 检查是否已经生成过此Sticker
            if (generatedStickers.Contains(proto.id))
            {
                Debug.LogWarning("Sticker with ID " + proto.id + " has already been generated. Skipping.");
                TriggerClueAlreadyObtainedUI();
                return;
            }

            // Step 1: 从初始位置生成Sticker
            Vector3 startStickerPosition = stickerInitialPosition.position; // 从初始位置开始
            StickerBehaviour sticker = Instantiate(proto.stickerPrefab, startStickerPosition, Quaternion.identity);
            sticker.Init(proto);

            // Step 2: 让Sticker沿自身Y轴旋转，并飞向最终位置
            sticker.transform.DOMove(finalPosition, 1.5f) // 1.5秒移动到正确位置
                .SetEase(Ease.OutCubic) // 平滑移动
                .OnComplete(() =>
                {
                    Debug.Log("Sticker arrived at final position");

                    // Step 3: 在贴纸到达目标位置后生成并移动Pin
                    if (pinPrefab != null && sticker.PinPosition != null)
                    {
                        // 计算Pin的初始位置：Z方向偏移-1
                        Vector3 startPinPosition = sticker.PinPosition.position + new Vector3(0, 0, -1);

                        // 生成Pin
                        GameObject pin = Instantiate(pinPrefab, sticker.model);
                        pin.transform.position = startPinPosition;
                        pin.transform.rotation = sticker.PinPosition.rotation;
                        pin.transform.localScale = sticker.PinPosition.localScale;

                        // 动画Pin从初始位置移动到正确位置
                        pin.transform.DOMove(sticker.PinPosition.position, 1.5f)
                            .SetEase(Ease.OutCubic) // 平滑移动
                            .OnComplete(() => Debug.Log("Pin arrived at final position"));

                        // PinBehaviour初始化
                        PinBehaviour pinBehaviour = pin.GetComponent<PinBehaviour>();
                        if (pinBehaviour != null)
                        {
                            pinBehaviour.Initialize(proto.id);
                        }
                        else
                        {
                            Debug.LogWarning("PinPrefab is missing PinBehaviour component.");
                        }
                    }
                });

            // 让贴纸在飞行过程中围绕自身Y轴旋转
            sticker.transform.DORotate(new Vector3(0, 360, 0), 1.5f, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear); // 线性旋转，持续1.5秒

            // 将生成的Sticker ID存储
            generatedStickers.Add(proto.id);

            if (alsoCreateStickerVfx)
                cvf.Capture(proto);

            // 显示新生成的Sticker相关UI效果
            TriggerNewStickerUI();
        }

        public void RemoveStickerByID(StickerInformation.ID id)
        {
            // 查找已经生成的贴纸对象
            StickerBehaviour stickerToRemove = FindStickerByID(id);

            if (stickerToRemove != null)
            {
                Debug.Log($"Removing sticker with ID: {id}");

                // 删除对应的 Pin（如果存在）
                Transform pin = stickerToRemove.transform.Find("PinPrefab");
                if (pin != null)
                {
                    Destroy(pin.gameObject);
                }

                // 从场景中删除贴纸对象
                Destroy(stickerToRemove.gameObject);

                // 从生成的贴纸列表中移除 ID
                generatedStickers.Remove(id);
                // 删除与该 Sticker 相关的所有连线
                RopeManager.Instance.RemoveLinesConnectedToSticker(id);
            }
            else
            {
                Debug.LogWarning($"Sticker with ID: {id} not found.");
            }
        }

        // 辅助方法：根据ID查找已经生成的Sticker
        private StickerBehaviour FindStickerByID(StickerInformation.ID id)
        {
            // 获取场景中的所有 StickerBehaviour 对象
            StickerBehaviour[] allStickers = FindObjectsOfType<StickerBehaviour>();

            // 遍历所有贴纸，查找与目标 ID 匹配的贴纸
            foreach (var sticker in allStickers)
            {
                if (sticker != null && sticker.id == id)
                {
                    return sticker; // 返回找到的 StickerBehaviour
                }
            }

            Debug.LogWarning($"Sticker with ID {id} not found.");
            return null; // 未找到时返回 null
        }

        public bool IsStickerGenerated(StickerInformation.ID id)
        {
            return generatedStickers.Contains(id);
        }

        public StickerBehaviour FindStickerExist(StickerInformation.ID id)
        {
            return FindObjectsOfType<StickerBehaviour>().FirstOrDefault(sticker => sticker.id == id);
        }

        // 触发UI显示并进行晃动效果
        private void TriggerClueAlreadyObtainedUI()
        {
            if (clueAlreadyObtainedText != null)
            {
                // 设置初始显示位置
                float initialPosX = 1270;
                float targetPosX = initialPosX - 500f; // 目标位置 X 值减少 500

                // 设置 UI 的文本内容并显示
                clueAlreadyObtainedText.gameObject.SetActive(true);
                //clueAlreadyObtainedText.text = "This clue has already been obtained";

                // 使用 DoTween 让 UI 平滑移动到目标位置
                clueAlreadyObtainedText.rectTransform
                    .DOAnchorPosX(targetPosX, 1f) // 移动到目标位置
                    .SetEase(Ease.OutCubic)
                    .OnComplete(() =>
                    {
                        // 保持 2 秒显示时间后返回原来位置
                        DOVirtual.DelayedCall(2f, () =>
                        {
                            clueAlreadyObtainedText.rectTransform
                                .DOAnchorPosX(initialPosX, 1f) // 返回原来的位置
                                .SetEase(Ease.OutCubic)
                                .OnComplete(() =>
                                {
                                    // 返回初始位置后隐藏 UI
                                    clueAlreadyObtainedText.gameObject.SetActive(false);
                                });
                        });
                    });
            }
        }
    }


}
