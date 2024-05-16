using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOE : MonoBehaviour
{
    SpriteRenderer spriter;
    public float aoeRadius;
    public float aoeAngle;
    public float activeDelay;
    public float deactiveDelay;
    RaycastHit2D[] playerHits;

    void Awake()
    {
        spriter = GetComponent<SpriteRenderer>();
        spriter.enabled = false;
    }

    /// <summary>
    /// 보여줄 범위 각도 조절
    /// </summary>
    /// <param name="angle"></param>
    public void SetAngle(float angle)
    {
        aoeAngle = angle;
        spriter.material.SetFloat("_Arc1", (360 - angle) / 2);
        spriter.material.SetFloat("_Arc2", (360 - angle) / 2);
    }

    public void SetRadius(float radius)
    {
        transform.localScale = new Vector3(radius*2, radius*2, 1f);
        aoeRadius = radius;
    }

    public void SetDelay(float activeDelay, float deactiveDelay)
    {
        this.activeDelay = activeDelay;
        this.deactiveDelay = deactiveDelay;
    }
    public void FindPlayer()
    {
        int iterator = Convert.ToInt32(aoeAngle/ 10);
        Vector2 dir = Quaternion.Euler(0, 0, aoeAngle / 2) * transform.right * aoeRadius;
        for (int i = 0; i <= iterator; i++)
        {
            Vector2 rayDir = Quaternion.Euler(0, 0, -i*10 ) * dir;
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, rayDir, aoeRadius);
            //Debug.Log(hits.Length);
            if (CheckPlayerExists(hits))
            {
                playerHits = hits;
                break;
            }
        }
    }
    private void OnDrawGizmos()
    {
        int iterator = Convert.ToInt32(aoeAngle / 10);
        Vector2 dir = Quaternion.Euler(0, 0, aoeAngle / 2) * transform.right * aoeRadius;
        for (int i = 0; i <= iterator; i++)
        {
            Vector2 rayDir = Quaternion.Euler(0, 0, -i * 10) * dir;
            Debug.DrawRay(transform.position, rayDir);
        }
    }

    private bool CheckPlayerExists(RaycastHit2D[] hits)
    {
        foreach (var hit in hits)
        {
            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("Find Player");
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// activeDealy 초 후 범위 표시, deactiveDealy 초 후 범위 사라지며 Player 감지
    /// </summary>
    /// <param name="activeDelay"></param>
    /// <param name="deactiveDelay"></param>
    /// <returns></returns>
    public IEnumerator AOECoroutine()
    {
        yield return new WaitForSeconds(activeDelay);
        spriter.enabled = true;
        yield return new WaitForSeconds(deactiveDelay);
        spriter.enabled = false;
        FindPlayer();
    }
}
