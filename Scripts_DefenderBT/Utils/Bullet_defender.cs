using UnityEngine;

/// <summary>
/// 총알 이동 및 충돌 처리 스크립트
/// </summary>
public class Bullet_defender : MonoBehaviour
{
    public float damage = 10f;
    public float lifeTime = 3f;
    public bool isMultiShot = false;


    private void Start()
    {
        // 총알이 3초 후 자동 파괴됨
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //// 방패(Shield)에 부딪힌 경우 → 그냥 소멸
        //if (collision.gameObject.CompareTag("Shield"))
        //{
        //    Destroy(gameObject);
        //    return;
        //}

        // 🛑 다른 Bullet과 충돌했을 경우 → 무시
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), collision.collider);
            return;
        }

        // 에이전트에 부딪힌 경우 → 데미지 적용
        AgentBlackboard target = collision.gameObject.GetComponent<AgentBlackboard>();
        if (target != null)
        {
            if (isMultiShot)
            {
                target.isMultiShotIncoming = true;
            }
            else
            {
                target.isMultiShotIncoming = false;
            }

            target.TakeDamage(damage);
        }


        // 충돌 시 총알 제거
        Destroy(gameObject);
    }
}
