using System.IO;
using System.Text;
using UnityEngine;

public class BTLogger : MonoBehaviour
{
    public static BTLogger Instance;

    [Header("시뮬레이션 배속 설정 (기본 1.0 = 실시간)")]
    public float timeScale = 3.0f;

    private StringBuilder csv = new StringBuilder();
    private int matchCount = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        // ✅ 새로운 포맷으로 CSV 헤더 생성
        csv.AppendLine("Match#,Winner," +
                       "Attacker_Attack,Attacker_Defend,Attacker_Evade," +
                       "Defender_Attack,Defender_Defend,Defender_Evade");
    }

    private void Start()
    {
        Time.timeScale = timeScale;
        Debug.Log($"⏩ 시뮬레이션 {timeScale}배속으로 실행 중");
    }

    public void LogMatch(string winner, AgentBlackboard attacker, AgentBlackboard defender, float matchTime)
    {
        matchCount++;

        string row = $"{matchCount},{winner}," +
                     $"{attacker.attackSucc},{attacker.defendSucc},{attacker.evadeSucc}," +
                     $"{defender.attackSucc},{defender.defendSucc},{defender.evadeSucc}";

        csv.AppendLine(row);
        Debug.Log($"📊 [{matchCount}] 경기 기록 완료 ({winner} 승)");
    }

    public void ExportCSV()
    {
        string path = Path.Combine(Application.dataPath, "SimulationResults.csv");
        File.WriteAllText(path, csv.ToString());
        Debug.Log($"✅ 시뮬레이션 CSV 저장 완료: {path}");
    }
}
