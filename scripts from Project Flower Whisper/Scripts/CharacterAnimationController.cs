using System.Collections;
using UnityEngine;

public class NPCAnimationController : MonoBehaviour
{
    private Animator animator;
    public ModeSwitcher modeSwitcher;  // ��Ӷ�ModeSwitcher������

    public GameObject player;  // ���ǵ�Transform
    public Transform playerTargetPosition; // ����Ŀ��λ�õ�Transform
    public Transform npcTargetPosition; // NPCĿ��λ�õ�Transform
    public Transform hostPosition; // ������ǵ� HostPosition
    public Transform containerTargetPosition; // Container���Ƶ�Ŀ��λ��

    private bool isGivingFlower = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetTrigger("SitDown");
    }

    public void GiveFlower()
    {
        if (!isGivingFlower)
        {
            isGivingFlower = true;

            // ��NPC�ƶ���Ŀ��λ��
            transform.position = npcTargetPosition.position;
            transform.rotation = npcTargetPosition.rotation;

            
            player.SetActive(false);

            // ����HostPosition��TagΪContainer���Ӷ���Ŀ��λ��
            CopyContainerObject();

            // �����׻�����
            animator.SetTrigger("GiveFlower");

            // ����Э�̣����׻������������л���վ��״̬
            StartCoroutine(TransitionToStanding());
        }
    }

    private void CopyContainerObject()
    {
        foreach (Transform child in hostPosition)
        {
            if (child.CompareTag("Container"))
            {
                GameObject copiedContainer = Instantiate(child.gameObject, containerTargetPosition.position, containerTargetPosition.rotation);
                copiedContainer.transform.SetParent(containerTargetPosition);
                break;
            }
        }
    }

    private IEnumerator TransitionToStanding()
    {
        // �ȴ��׻�������ɣ������׻���������ʱ��Ϊ3�룬����ʵ�ʶ���ʱ��������
        yield return new WaitForSeconds(3f);

        // �л���վ��״̬
        animator.SetTrigger("StandUp");

        // �ȴ�վ����������
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        player.SetActive(true);

        // վ�����������󣬵���ModeSwitcher��SetMainMode����
        if (modeSwitcher != null)
        {
            modeSwitcher.SetMainMode();
            Debug.Log("Switched to Main Mode after standing.");
        }

        isGivingFlower = false;
    }
}
