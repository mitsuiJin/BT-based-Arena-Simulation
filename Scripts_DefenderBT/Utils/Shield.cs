using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private AudioClip blockClip;

    [HideInInspector] public Transform shooter; // 방패 만든 주체
    [HideInInspector] public AgentBlackboard ownerBlackboard; // ✅ 방어자 상태 전달받음

    private void Start()
    {
        Destroy(gameObject, 2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Bullet bullet = other.GetComponent<Bullet>();

            if (bullet != null)
            {
                // 자기 총알이면 무시
                if (bullet.shooter == shooter)
                    return;

                // ✅ 방어 성공 기록
                if (ownerBlackboard != null)
                {
                   // ownerBlackboard.defendSucc++;
                    Debug.Log($"🛡 방어 성공! {ownerBlackboard.name} defendSucc: {ownerBlackboard.defendSucc}");
                }

                if (blockClip != null)
                    AudioSource.PlayClipAtPoint(blockClip, transform.position);

                Destroy(gameObject);
            }
        }
    }
}
