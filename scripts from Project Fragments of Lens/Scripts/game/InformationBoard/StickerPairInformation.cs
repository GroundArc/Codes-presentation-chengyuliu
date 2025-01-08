using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Assets.Scripts.game.InformationBoard;

[System.Serializable]
public class CorrectStickerPair
{
    public StickerInformation.ID sticker1; // 第一个Sticker的ID
    public StickerInformation.ID sticker2; // 第二个Sticker的ID
}

[System.Serializable]
public class StickerPairInformation
{
    public string branchName; // 分支名称
    public TextMeshProUGUI progressText; // 显示分支进度的UI组件
    public List<CorrectStickerPair> correctPairs = new List<CorrectStickerPair>(); // 正确的配对列表

    private int currentProgress = 0; // 当前已配对正确的数量

    // 更新分支进度
    public void UpdateProgress(List<(StickerInformation.ID, StickerInformation.ID)> pairedStickers)
    {
        currentProgress = 0;

        foreach (var correctPair in correctPairs)
        {
            // 检查配对是否存在（无论顺序如何）
            if (pairedStickers.Contains((correctPair.sticker1, correctPair.sticker2)) || pairedStickers.Contains((correctPair.sticker2, correctPair.sticker1)))
            {
                currentProgress++;
            }
        }

        // 更新UI文本
        progressText.text = $"{branchName} Progress: {currentProgress}/{correctPairs.Count}";
    }
}
