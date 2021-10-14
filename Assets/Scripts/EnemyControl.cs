using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    Rigidbody phys;
    GameObject character;
    Animator animator;
    public GameObject headPos;
    RaycastHit hitPoint;

    bool wall = false, dead = false;

    void Start()
    {
        character = GameObject.FindGameObjectWithTag("Player");
        phys = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        Debug.Log(character.transform.position);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Bullet"))
        {
            animator.SetBool("deadParam", true);
            gameObject.GetComponent<MeshCollider>().enabled = false;
            phys.useGravity = false;
            dead = true;
        }
    }

    void FixedUpdate()
    {
        if (!dead)
        {
            NPCRotation();
            NPCPosition();
        }
        /*Physics.Raycast(transform.position, character.transform.position.normalized, out hitPoint);
        NPCRotation();*/
        //Debug.DrawLine(transform.position, hitPoint.point, Color.green);
    }
    void NPCPosition()
    {
        Vector3 pos = new Vector3(0, 0, 1);
        pos = transform.TransformDirection(pos);
        phys.position += pos * Time.fixedDeltaTime * 2.5f;
    }
    void NPCRotation()
    {
        
        if(Physics.Raycast(headPos.transform.position, transform.forward, out hitPoint, 10))
        {
            Debug.DrawLine(headPos.transform.position, hitPoint.point);
            GameObject hitObj = hitPoint.transform.gameObject;
            
            SkipWalls(hitObj, hitPoint);
        }
        if (!wall)
            StartCoroutine(LookCharacter());
            //transform.LookAt(character.transform);

    }
    IEnumerator LookCharacter()
    {
        yield return new WaitForSeconds(1);
        transform.LookAt(character.transform);
    }
    void SkipWalls(GameObject hitObj, RaycastHit hit)
    {
        float rand = Random.Range(0, 10);
        if (hitObj.tag.Equals("trigger"))
        {
            wall = true;
            float dist = Vector3.Distance(hit.point, hitObj.transform.position);
            
            Debug.Log("triggered dist: " + dist);
            if (rand < 5)
            {
                hit.point += new Vector3(dist + 10, 0, 0);
                Debug.Log(hit.point);
                //transform.LookAt(hit.point);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(hitPoint.point.x, hitPoint.point.y, hitPoint.point.z), 0.1f);
            }
            else
            {
                hit.point -= new Vector3(dist + 10, 0, 0);
                //transform.LookAt(hit.point);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(hitPoint.point.x, hitPoint.point.y, hitPoint.point.z), 0.1f);

            }
        }
        else
        {
            wall = false;
        }
            
    }
}
