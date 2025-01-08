using DG.Tweening;
using System.Text.RegularExpressions;
using UnityEngine;

public class CameraFocusSystem : MonoBehaviour
{
    public static CameraFocusSystem instance;

    public Transform cameraTrans;
    public Transform cameraTrans_phone;
    public Transform cameraTrans_laptop;
    public Transform cameraTrans_board;
    public Transform cameraTrans_table;
    bool _disableBoardClick;

    public float transitDuration = 2;
    private Vector3 cameraStartPos;
    private Vector3 cameraStartEular;

    public bool isTransiting { get; private set; }

    [Header("UI Settings")]
    public RectTransform uiElement;
    public Vector2 uiShowPosition;
    public Vector2 uiOriginalPosition;
    private bool isUIShown = false;

    public FreeZoomCamera freeZoomCamera;
    public CaptureVideoFeedback capture;
    public RectTransform phoneUIElement; // Phone UI
    public Vector2 phoneUIShowPosition; // 显示位置
    public Vector2 phoneUIHidePosition; // 隐藏位置

    private bool isGameInStartPhase = true; // 是否处于StartGamePhase阶段

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        cameraStartPos = cameraTrans.position;
        cameraStartEular = cameraTrans.eulerAngles;
        freeZoomCamera.enabled = false;

        uiElement.anchoredPosition = uiOriginalPosition;

        // 初始化UI位置
        uiElement.anchoredPosition = uiOriginalPosition;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            ResumeCamera();
        }
    }

    void OnTransitFinish()
    {
        isTransiting = false;
        CheckPhonePosition(); // 检查是否在 Phone 位置并显示/隐藏 UI
    }

    void OnTransitFinish_board()
    {
        isTransiting = false;
        freeZoomCamera.Init();
    }

    public void SetGameStartPhaseComplete()
    {
        isGameInStartPhase = false; // 标记游戏已进入正式阶段
    }

    private bool CanMoveCamera()
    {
        return !isGameInStartPhase && !isTransiting;
    }

    public void ResumeCamera()
    {
        if (!CanMoveCamera())
            return;

        _disableBoardClick = false;
        freeZoomCamera.enabled = false;
        isTransiting = true;

        if (isUIShown)
        {
            uiElement.DOAnchorPos(uiOriginalPosition, transitDuration).OnComplete(() => isUIShown = false);
        }

        if (Vector3.Distance(cameraTrans.position, cameraTrans_phone.position) < 0.1f ||
            Vector3.Distance(cameraTrans.position, cameraTrans_laptop.position) < 0.1f ||
            Vector3.Distance(cameraTrans.position, cameraTrans_board.position) < 1.9f ||
            Vector3.Distance(cameraTrans.position, cameraTrans_table.position) < 0.1f)
        {
            cameraTrans.DOMove(cameraStartPos, transitDuration);
            cameraTrans.DORotate(cameraStartEular, transitDuration).OnComplete(OnTransitFinish);
        }
        else
        {
            cameraTrans.position = cameraStartPos;
            cameraTrans.eulerAngles = cameraStartEular;
            OnTransitFinish();
        }
    }

    public void OnClickLaptop()
    {
        if (!CanMoveCamera())
            return;

        freeZoomCamera.enabled = false;
        isTransiting = true;
        cameraTrans.DOMove(cameraTrans_laptop.position, transitDuration);
        cameraTrans.DORotate(cameraTrans_laptop.eulerAngles, transitDuration).OnComplete(OnTransitFinish);
    }

    public void OnClickPhone()
    {
        if (!CanMoveCamera())
            return;

        freeZoomCamera.enabled = false;
        isTransiting = true;
        cameraTrans.DOMove(cameraTrans_phone.position, transitDuration);
        cameraTrans.DORotate(cameraTrans_phone.eulerAngles, transitDuration).OnComplete(OnTransitFinish);
    }

    public void OnClickTable()
    {
        if (!CanMoveCamera())
            return;

        freeZoomCamera.enabled = false;
        isTransiting = true;
        cameraTrans.DOMove(cameraTrans_table.position, transitDuration);
        cameraTrans.DORotate(cameraTrans_table.eulerAngles, transitDuration).OnComplete(OnTransitFinish);
    }

    public void OnClickBoard()
    {

        _disableBoardClick = true;
        freeZoomCamera.enabled = false;
        isTransiting = true;
        cameraTrans.DOMove(cameraTrans_board.position, transitDuration);
        cameraTrans.DORotate(cameraTrans_board.eulerAngles, transitDuration).OnComplete(OnTransitFinish_board);

        uiElement.DOAnchorPos(uiShowPosition, transitDuration).OnComplete(() => isUIShown = true);
    }

    // 检查相机是否在 phone 位置
    private void CheckPhonePosition()
    {
        if (Vector3.Distance(cameraTrans.position, cameraTrans_phone.position) < 0.1f)
        {
            // 移动 UI 到显示位置
            phoneUIElement.DOAnchorPos(phoneUIShowPosition, transitDuration).SetEase(Ease.OutCubic);
        }
        else
        {
            // 移动 UI 到隐藏位置
            phoneUIElement.DOAnchorPos(phoneUIHidePosition, transitDuration).SetEase(Ease.InCubic);
        }
    }
}
