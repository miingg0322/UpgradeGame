using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Batch : MonoBehaviour
{
    public int count = 6;
    public int speed = -150;

    float timer;
    float AttackTime = 1.5f;
    void Start()
    {
        BatchMelee();
    }

    private void Update()
    {
        if (Player.Instance.playerId != 2)
        {
            transform.Rotate(Vector3.forward * speed * Time.deltaTime);
        }
        else
        {
            if (!GameManager.Instance.isDungeonClear)
            {
                timer += Time.deltaTime;
            }

            if (timer >= AttackTime)
            {
                timer = 0f;
                BatchMelee();
            }
        }
    }

    void BatchMelee()
    {
        if (Player.Instance.playerId == 2)
        {
            float angleStep = 360 / count;
            Vector3[] positions = new Vector3[count];

            // 육각형의 점 계산
            for (int i = 0; i < count; i++)
            {
                float angle = Mathf.Deg2Rad * (i * angleStep);
                positions[i] = transform.position + new Vector3(Mathf.Cos(angle) * 1.5f, Mathf.Sin(angle) * 1.5f, 0);
            }

            // 프로젝트 생성 및 비행 시작
            for (int i = 0; i < count; i++)
            {
                GameObject projectile = GameManager.Instance.pool.Get(3);
                projectile.transform.parent = transform;
                StartCoroutine(LaunchProjectile(projectile, positions[i]));
            }
        }
        else
        {
            for (int index = 0; index < count; index++)
            {
                float angle = 360 * index / count;
                Transform weapon;

                weapon = GameManager.Instance.pool.Get(3).transform;
                weapon.parent = transform;

                weapon.localPosition = Vector3.zero;
                weapon.localRotation = Quaternion.identity;

                Vector3 rotVec = Vector3.forward * angle;
                Vector3 revRotVec = Vector3.forward * -1 * angle;

                weapon.Rotate(rotVec);
                weapon.Translate(weapon.up * 1.5f, Space.World);

                if (Player.Instance.playerId == 0)
                    weapon.Rotate(revRotVec);
            }
        }       
    }

    private IEnumerator LaunchProjectile(GameObject projectile, Vector3 targetPosition)
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0;
        float duration = 0.5f; // 이동에 걸리는 시간

        float height = 2f; // 포물선의 최대 높이

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            // 수평 위치 계산
            Vector3 horizontalPosition = Vector3.Lerp(startPosition, targetPosition, t);

            // 수직 위치 계산
            float verticalPosition = Mathf.Lerp(startPosition.y, targetPosition.y, t) + height * 4 * t * (1 - t);

            // 현재 위치 계산
            projectile.transform.position = new Vector3(horizontalPosition.x, verticalPosition, horizontalPosition.z);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        projectile.transform.position = targetPosition;
    }
}
