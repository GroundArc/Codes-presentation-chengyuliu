using UnityEngine;

namespace Assets.Scripts.game.InformationBoard
{
    public class ClickPin : MonoBehaviour
    {
        public Outline outline;
        public bool isClicked = false;

        private void Awake()
        {
            // 获取 Outline 组件
            outline = GetComponent<Outline>();

            if (outline == null)
            {
                Debug.LogError("Outline component not found on the Pin.");
            }
        }

        private void Update()
        {
            if (isClicked)
            {
                outline.enabled = true;
            }
            // 通过 Raycast 检测鼠标和 BoxCollider 的关系
            if (!isClicked)
            {
                CheckMouseHover();
            }

            // 检测鼠标左键点击事件
            if (Input.GetMouseButtonDown(0))
            {
                isClicked = true;
            }
        }

        private void CheckMouseHover()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // 检查是否碰撞到当前对象
                if (hit.collider == GetComponent<Collider>())
                {
                    if (!isClicked)
                    {
                        outline.enabled = true;
                    }

                }
            }   
        }

        
    }
}
