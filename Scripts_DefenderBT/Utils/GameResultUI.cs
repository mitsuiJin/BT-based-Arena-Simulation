using UnityEngine;
using TMPro;

public class GameResultUI : MonoBehaviour
{
    [Header("에이전트 상태")]
    [SerializeField] private AgentBlackboard agentA;
    [SerializeField] private AgentBlackboard agentB;

    [Header("UI 구성 요소")]
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private TextMeshProUGUI resultText;

    private bool gameOver = false;

    void Update()
    {
        if (gameOver) return;

        if (agentA.currentHp <= 0 && agentB.currentHp <= 0)
        {
            ShowResult("Draw!");
            gameOver = true;
        }
        else if (agentA.currentHp <= 0)
        {
            ShowResult($"{agentB.name} WINS!");
            gameOver = true;
        }
        else if (agentB.currentHp <= 0)
        {
            ShowResult($"{agentA.name} WINS!");
            gameOver = true;
        }
    }

    void ShowResult(string message)
    {
        resultPanel.SetActive(true);
        resultText.text = message;
    }
}
