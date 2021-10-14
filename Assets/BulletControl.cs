using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    /*void Start()
    {
        
    }

    void Update()
    {
        
    }*/
    private void OnTriggerEnter(Collider col)
    {
        StartCoroutine(Bullet());
    }

    IEnumerator Bullet()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}
