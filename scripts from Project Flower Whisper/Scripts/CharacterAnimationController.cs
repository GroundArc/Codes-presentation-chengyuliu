using System.Collections;
using UnityEngine;

public class NPCAnimationController : MonoBehaviour
{
    private Animator animator;
    public ModeSwitcher modeSwitcher;  // 添加对ModeSwitcher的引用

    public GameObject player;  // 主角的Transform
    public Transform playerTargetPosition; // 主角目标位置的Transform
    public Transform npcTargetPosition; // NPC目标位置的Transform
    public Transform hostPosition; // 玩家主角的 HostPosition
    public Transform containerTargetPosition; // Container复制的目标位置

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

            // 将NPC移动到目标位置
            transform.position = npcTargetPosition.position;
            transform.rotation = npcTargetPosition.rotation;

            
            player.SetActive(false);

            // 复制HostPosition中Tag为Container的子对象到目标位置
            CopyContainerObject();

            // 触发献花动画
            animator.SetTrigger("GiveFlower");

            // 启动协程，在献花动画结束后切换到站立状态
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
        // 等待献花动画完成（假设献花动画持续时间为3秒，根据实际动画时长调整）
        yield return new WaitForSeconds(3f);

        // 切换到站立状态
        animator.SetTrigger("StandUp");

        // 等待站立动画结束
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        player.SetActive(true);

        // 站立动画结束后，调用ModeSwitcher的SetMainMode方法
        if (modeSwitcher != null)
        {
            modeSwitcher.SetMainMode();
            Debug.Log("Switched to Main Mode after standing.");
        }

        isGivingFlower = false;
    }
}
