using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Assets.Scripts.game.InformationBoard;

[System.Serializable]
public class CorrectStickerPair
{
    public StickerInformation.ID sticker1; // ��һ��Sticker��ID
    public StickerInformation.ID sticker2; // �ڶ���Sticker��ID
}

[System.Serializable]
public class StickerPairInformation
{
    public string branchName; // ��֧����
    public TextMeshProUGUI progressText; // ��ʾ��֧���ȵ�UI���
    public List<CorrectStickerPair> correctPairs = new List<CorrectStickerPair>(); // ��ȷ������б�

    private int currentProgress = 0; // ��ǰ�������ȷ������

    // ���·�֧����
    public void UpdateProgress(List<(StickerInformation.ID, StickerInformation.ID)> pairedStickers)
    {
        currentProgress = 0;

        foreach (var correctPair in correctPairs)
        {
            // �������Ƿ���ڣ�����˳����Σ�
            if (pairedStickers.Contains((correctPair.sticker1, correctPair.sticker2)) || pairedStickers.Contains((correctPair.sticker2, correctPair.sticker1)))
            {
                currentProgress++;
            }
        }

        // ����UI�ı�
        progressText.text = $"{branchName} Progress: {currentProgress}/{correctPairs.Count}";
    }
}
