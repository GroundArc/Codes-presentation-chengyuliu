using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewDialogueOption", menuName = "Dialogue System/Option")]
public class DialogueOption : ScriptableObject
{
    public string optionText; // ѡ������
    public UnityEvent optionEvent; // ѡ������¼�
    public Dialogue nextDialogue; // ��һ���Ի�
    public bool exitDialogue; // �Ƿ��˳��Ի�
    public bool isHiddenInitially = false; // ѡ���Ƿ��ʼ����
    public bool showOptionsAfterSelection = false; // ѡ����Ƿ���ʾѡ��
}