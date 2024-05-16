using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Batch : MonoBehaviour
{
    public int count = 6;
    public int speed = -150;

    float timer;
    float AttackTime = 2f;
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

            // �������� �� ���
            for (int i = 0; i < count; i++)
            {
                float angle = Mathf.Deg2Rad * (i * angleStep);
                positions[i] = transform.position + new Vector3(Mathf.Cos(angle) * 1.5f, Mathf.Sin(angle) * 1.5f, 0);
            }

            // ������Ʈ ���� �� ���� ����
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
        Vector3 relativeTargetPosition = targetPosition - startPosition;

        while (elapsedTime < 1)
        {
            float t = elapsedTime / 1;
            // ������ ���� ���: y = h * 4 * t * (1 - t)
            Vector3 currentPosition = Vector3.Lerp(Vector3.zero, relativeTargetPosition, t); // ������� �̵����� ������� ����մϴ�.
            currentPosition.y = targetPosition.y + 4 * t * (1 - t);
            projectile.transform.position = startPosition + currentPosition; // ���� ��ġ�� ���Ͽ� ���� ��ġ�� ����մϴ�.

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        projectile.transform.position = targetPosition;
    }
}
