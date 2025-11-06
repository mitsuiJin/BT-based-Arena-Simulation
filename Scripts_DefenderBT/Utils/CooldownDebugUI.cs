using UnityEngine;
using UnityEngine.UI;

public class CooldownDebugUI : MonoBehaviour
{
    public AgentBlackboard blackboard;
    public Text debugText;

    void Update()
    {
        if (blackboard == null || debugText == null)
            return;

        string info = "";
        info += $"회피 가능: {blackboard.IsCooldownReady("Evade", blackboard.evadeCooldown)}\n";
        info += $"방어 가능: {blackboard.IsCooldownReady("Defend", blackboard.defendCooldown)}\n";
        info += $"Already Reacted: {blackboard.alreadyReacted}\n";
        info += $"멀티샷: {blackboard.isMultiShotIncoming}\n";
        //info += $"Current HP: {blackboard.currentHp:F1}";

        debugText.text = info;
    }
}
