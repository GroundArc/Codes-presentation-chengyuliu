using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue System/Dialogue")]
public class Dialogue : ScriptableObject
{
    public string dialogueName; // �Ի��Ķ�������
    public string characterName; // ˵����ɫ������
    [TextArea(3, 10)]
    public string[] sentences; // �Ի�����
    public DialogueEndAction endAction; // �Ի����������Ϊ
    public UnityEvent endEvent; // �Ի������������¼�
    public DialogueOption[] options; // �Ի�ѡ��
}

public enum DialogueEndAction
{
    TriggerEvent,
    ExitDialogue,
    ShowOptions
}
