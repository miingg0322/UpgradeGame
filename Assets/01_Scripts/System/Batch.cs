using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Batch : MonoBehaviour
{
    public int count = 6;
    public int speed = -150;
    void Start()
    {
        BatchMelee();
    }

    private void Update()
    {
        transform.Rotate(Vector3.forward * speed * Time.deltaTime);
    }

    void BatchMelee()
    {
        for (int index = 0; index < count; index++)
        {
            Transform weapon;

            weapon = GameManager.Instance.pool.Get(3).transform;
            weapon.parent = transform;

            weapon.localPosition = Vector3.zero;
            weapon.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * index / count;
            weapon.Rotate(rotVec);
            weapon.Translate(weapon.up * 1.5f, Space.World);
        }
    }
}
