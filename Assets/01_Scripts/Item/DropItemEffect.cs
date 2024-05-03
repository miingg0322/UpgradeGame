using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemEffect : MonoBehaviour
{
    public float rotationSpeed = 360f; 
    public float floatSpeed = 1f; 
    public float lifeTime = 1f; 

    private void OnEnable()
    {
        StartCoroutine(FloatingAndRotating());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator FloatingAndRotating()
    {
        float elapsedTime = 0f;
        while (elapsedTime < lifeTime)
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            transform.Translate(Vector3.up * floatSpeed * Time.deltaTime, Space.World);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        gameObject.SetActive(false);
    }
}
