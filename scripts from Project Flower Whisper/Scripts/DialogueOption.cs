using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewDialogueOption", menuName = "Dialogue System/Option")]
public class DialogueOption : ScriptableObject
{
    public string optionText; // 选项文字
    public UnityEvent optionEvent; // 选项触发的事件
    public Dialogue nextDialogue; // 下一个对话
    public bool exitDialogue; // 是否退出对话
    public bool isHiddenInitially = false; // 选项是否初始隐藏
    public bool showOptionsAfterSelection = false; // 选择后是否显示选项
}