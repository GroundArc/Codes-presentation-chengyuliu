using UnityEngine;
using static Assets.Scripts.game.InformationBoard.StickerInformation;

namespace Assets.Scripts.game.InformationBoard
{
    using UnityEngine;

    public class StickerInteraction : MonoBehaviour
    {
        public StickerBehaviour stickerBehaviour;
        public LayerMask layerMask;
        public float rayDistance = 30f;
        public Outline pinOutline;

        private RopeManager ropeManager;

        void Start()
        {
            ropeManager = FindObjectOfType<RopeManager>();
            if (ropeManager == null)
            {
                Debug.LogError("RopeManager not found in the scene.");
            }
        }

        private Outline FindPinOutline()
        {
            foreach (Transform child in transform)
            {
                if (child.CompareTag("Pin"))
                {
                    return child.GetComponent<Outline>();
                }
            }
            return null;
        }

        void Update()
        {
            // 持续检查并寻找 Pin 的 Outline 组件
            if (pinOutline == null)
            {
                pinOutline = FindPinOutline();
            }

            DetectRightClick();
        }

        private void DetectRightClick()
        {
            if (Input.GetMouseButtonDown(2)) 
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, rayDistance, layerMask))
                {
                    if (hit.transform.gameObject == this.gameObject)
                    {
                        Debug.Log($"Sticker model {gameObject.name} was right-clicked.");

                        if (stickerBehaviour != null)
                        {
                            Transform pinPosition = stickerBehaviour.PinPosition;
                            StickerInformation.ID stickerId = stickerBehaviour.id;

                            if (RopeManager.Instance.ReceivePinInfo(pinPosition, stickerId, stickerBehaviour, pinOutline))
                            {
                                Debug.Log($"PinPosition: {stickerBehaviour.PinPosition.position}, StickerID: {stickerBehaviour.id}");
                            }
                        }
                        else
                        {
                            Debug.LogError("StickerBehaviour is not assigned.");
                        }
                    }
                }
            }
        }

    }







}
