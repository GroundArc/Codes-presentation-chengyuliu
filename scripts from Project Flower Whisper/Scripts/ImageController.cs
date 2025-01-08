using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ImageController : MonoBehaviour
{
    public List<GameObject> imagesToFadeIn; // 要依次显示的图片列表
    public float fadeInDuration = 1f; // 每张图片的淡入持续时间
    public Button controlButton; // 控制图片显示的按钮

    public int currentImageIndex = 0; // 当前要激活的图片索引

    void Start()
    {
        // 确保所有图片初始时都是未激活的
        DisableAllImages();

        // 添加按钮点击事件
        controlButton.onClick.AddListener(ShowNextImage);
    }

    public void ShowNextImage()
    {
        if (currentImageIndex < imagesToFadeIn.Count)
        {
            GameObject img = imagesToFadeIn[currentImageIndex];

            // 确保图片初始时是不可见的，并且先激活图片
            img.SetActive(true);

            // 获取 Image 组件，如果图片是 UI 图片
            //Image imageComponent = img.GetComponent<Image>();

            //if (imageComponent != null)
            //{
                // 初始时透明度为 0
            //    Color color = imageComponent.color;
            //    color.a = 0f;
            //    imageComponent.color = color;

                // 使用DOTween淡入图片
            //    imageComponent.DOFade(1f, fadeInDuration);
            //}
            
            

            // 更新索引，指向下一个图片
            currentImageIndex++;
        }
        else
        {
            // 如果所有图片都已经显示完毕，可以选择禁用按钮或执行其他操作
            controlButton.interactable = false;
        }
    }

    public void DisableAllImages()
    {
        // 禁用所有图片并重置透明度
        foreach (GameObject img in imagesToFadeIn)
        {
            // 获取 Image 组件
            Image imageComponent = img.GetComponent<Image>();

            if (imageComponent != null)
            {
                // 将透明度重置为 0
                //Color color = imageComponent.color;
                //color.a = 0f;
                //imageComponent.color = color;
            }
            
            

            // 禁用图片
            img.SetActive(false);
        }

        // 重置当前图片索引
        currentImageIndex = 0;

        // 确保按钮可用
        controlButton.interactable = true;
    }
}
