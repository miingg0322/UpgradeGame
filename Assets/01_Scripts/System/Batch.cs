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
            float angle = 360 * index / count;
            Transform weapon;

            weapon = GameManager.Instance.pool.Get(3).transform;
            weapon.parent = transform;

            weapon.localPosition = Vector3.zero;
            weapon.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * angle;
            Vector3 revRotVec = Vector3.forward * -1 *angle;

            weapon.Rotate(rotVec);
            weapon.Translate(weapon.up * 1.5f, Space.World);          
            
            if(Player.Instance.playerId == 0)
                weapon.Rotate(revRotVec);
        }
    }
}
