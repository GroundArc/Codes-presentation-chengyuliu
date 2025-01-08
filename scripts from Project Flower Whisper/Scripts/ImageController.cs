using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ImageController : MonoBehaviour
{
    public List<GameObject> imagesToFadeIn; // Ҫ������ʾ��ͼƬ�б�
    public float fadeInDuration = 1f; // ÿ��ͼƬ�ĵ������ʱ��
    public Button controlButton; // ����ͼƬ��ʾ�İ�ť

    public int currentImageIndex = 0; // ��ǰҪ�����ͼƬ����

    void Start()
    {
        // ȷ������ͼƬ��ʼʱ����δ�����
        DisableAllImages();

        // ��Ӱ�ť����¼�
        controlButton.onClick.AddListener(ShowNextImage);
    }

    public void ShowNextImage()
    {
        if (currentImageIndex < imagesToFadeIn.Count)
        {
            GameObject img = imagesToFadeIn[currentImageIndex];

            // ȷ��ͼƬ��ʼʱ�ǲ��ɼ��ģ������ȼ���ͼƬ
            img.SetActive(true);

            // ��ȡ Image ��������ͼƬ�� UI ͼƬ
            //Image imageComponent = img.GetComponent<Image>();

            //if (imageComponent != null)
            //{
                // ��ʼʱ͸����Ϊ 0
            //    Color color = imageComponent.color;
            //    color.a = 0f;
            //    imageComponent.color = color;

                // ʹ��DOTween����ͼƬ
            //    imageComponent.DOFade(1f, fadeInDuration);
            //}
            
            

            // ����������ָ����һ��ͼƬ
            currentImageIndex++;
        }
        else
        {
            // �������ͼƬ���Ѿ���ʾ��ϣ�����ѡ����ð�ť��ִ����������
            controlButton.interactable = false;
        }
    }

    public void DisableAllImages()
    {
        // ��������ͼƬ������͸����
        foreach (GameObject img in imagesToFadeIn)
        {
            // ��ȡ Image ���
            Image imageComponent = img.GetComponent<Image>();

            if (imageComponent != null)
            {
                // ��͸��������Ϊ 0
                //Color color = imageComponent.color;
                //color.a = 0f;
                //imageComponent.color = color;
            }
            
            

            // ����ͼƬ
            img.SetActive(false);
        }

        // ���õ�ǰͼƬ����
        currentImageIndex = 0;

        // ȷ����ť����
        controlButton.interactable = true;
    }
}
