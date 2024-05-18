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
        float duration = 0.5f; // �̵��� �ɸ��� �ð�

        float height = 2f; // �������� �ִ� ����

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            // ���� ��ġ ���
            Vector3 horizontalPosition = Vector3.Lerp(startPosition, targetPosition, t);

            // ���� ��ġ ���
            float verticalPosition = Mathf.Lerp(startPosition.y, targetPosition.y, t) + height * 4 * t * (1 - t);

            // ���� ��ġ ���
            projectile.transform.position = new Vector3(horizontalPosition.x, verticalPosition, horizontalPosition.z);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        projectile.transform.position = targetPosition;
    }
}
