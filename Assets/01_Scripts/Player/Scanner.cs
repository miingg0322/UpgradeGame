using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public float scanRange;
    public float scanAngle;
    public LayerMask targetLayer;
    public LayerMask targetLayerBoss;
    public RaycastHit2D[] targets;
    public RaycastHit2D[] boss;
    public Transform nearestTarget;

    bool bossFound;

    private void FixedUpdate()
    {
        if(nearestTarget == null || !nearestTarget.gameObject.activeInHierarchy || nearestTarget.position.x > transform.position.x)
        {
            targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);
            boss = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayerBoss);
            nearestTarget = GetNearest();
        }     
    }

    Transform GetNearest()
    {
        Transform result = null;
        float diff = 100;

        foreach (RaycastHit2D hit in boss)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Boss"))
            {
                bossFound = true;
                break;
            }
        }
        if (bossFound && boss.Length != 0)
        {
            result = boss[0].transform;
            return result;
        }
                      
        foreach (RaycastHit2D target in targets)
        {
            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;
            float angle = Vector3.Angle(myPos, targetPos);
            if (angle > scanAngle && myPos.x > targetPos.x)
            {
                float curDiff = Vector3.Distance(myPos, targetPos);

                if (curDiff < diff)
                {
                    diff = curDiff;
                    result = target.transform;
                }
            }          
        }

        return result;
    }
}
