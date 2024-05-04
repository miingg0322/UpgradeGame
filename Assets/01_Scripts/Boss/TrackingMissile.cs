using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingMissile : MonoBehaviour
{
    private GameObject boss;
    void Start()
    {
        boss = GameObject.FindGameObjectWithTag("Boss");
        transform.position = boss.transform.position;
        gameObject.SetActive(false);
    }

    public void TrackTarget(GameObject target, float trackSpeed = 0.5f, float trackTime = 5f)
    {
        StartCoroutine(TrackCoroutine(target, trackSpeed, trackTime));
    }

    IEnumerator TrackCoroutine(GameObject target, float trackSpeed,float trackTime)
    {
        transform.position = boss.transform.position;
        gameObject.SetActive(true);
        float trackTimer = 0;
        while(trackTimer < trackTime)
        {
            trackTimer += Time.deltaTime;
            transform.Translate(target.transform.position * trackSpeed * Time.deltaTime);
            //transform.LookAt(target.transform);
            Vector2 dir = target.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            yield return new WaitForEndOfFrame();
        }
        // 비활성화
        gameObject.SetActive(false);
    }
}
