using UnityEngine;

namespace Assets.Scripts.game.InformationBoard
{
    public class ClickPin : MonoBehaviour
    {
        public Outline outline;
        public bool isClicked = false;

        private void Awake()
        {
            // ��ȡ Outline ���
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
            // ͨ�� Raycast ������� BoxCollider �Ĺ�ϵ
            if (!isClicked)
            {
                CheckMouseHover();
            }

            // �������������¼�
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
                // ����Ƿ���ײ����ǰ����
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
