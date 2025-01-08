using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue System/Dialogue")]
public class Dialogue : ScriptableObject
{
    public string dialogueName; // 对话的独特名称
    public string characterName; // 说话角色的名字
    [TextArea(3, 10)]
    public string[] sentences; // 对话内容
    public DialogueEndAction endAction; // 对话结束后的行为
    public UnityEvent endEvent; // 对话结束触发的事件
    public DialogueOption[] options; // 对话选项
}

public enum DialogueEndAction
{
    TriggerEvent,
    ExitDialogue,
    ShowOptions
}
