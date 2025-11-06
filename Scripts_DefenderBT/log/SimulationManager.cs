using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 총 50번 시뮬레이션을 자동으로 실행하며, 경기 종료 시 결과를 기록함
/// </summary>
public class SimulationManager : MonoBehaviour
{
    [Header("에이전트 연결")]
    public AgentBlackboard attacker;
    public AgentBlackboard defender;

    [Header("시뮬레이션 반복 횟수")]
    public int maxSimulations = 3;

    private static int currentSimCount = 0;
    private float startTime;
    private bool matchEnded = false;

    void Start()
    {
        startTime = Time.time;
        matchEnded = false;

        Debug.Log($"🚀 시뮬레이션 시작: {currentSimCount + 1}/{maxSimulations}");
    }

    void Update()
    {
        if (matchEnded) return;

        bool attackerDead = attacker.currentHp <= 0f;
        bool defenderDead = defender.currentHp <= 0f;

        if (attackerDead || defenderDead)
        {
            matchEnded = true;

            string winner;
            if (attackerDead && defenderDead) winner = "Draw";
            else if (attackerDead) winner = "Defender";
            else winner = "Attacker";

            float matchTime = Time.time - startTime;

            // ✅ 결과 기록
            BTLogger.Instance.LogMatch(winner, attacker, defender, matchTime);

            currentSimCount++;

            if (currentSimCount < maxSimulations)
            {
                Debug.Log($"🔁 다음 시뮬레이션 준비 중: {currentSimCount + 1}/{maxSimulations}");
                Invoke(nameof(RestartSimulation), 1f);
            }
            else
            {
                Debug.Log("✅ 모든 시뮬레이션 종료, CSV 저장");
                BTLogger.Instance.ExportCSV();

                // ✅ 게임 정지
                Time.timeScale = 0f;
            }
        }
    }

    private void RestartSimulation()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
