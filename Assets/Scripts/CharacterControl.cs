using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    float horizontal, vertical;
    float mouseX, mouseY;
    float speed;
    int bulletPoolCount = 0;
    public Vector3 offset;
    Vector3 headCamPos;
    bool aimCtrl = false;

    Rigidbody phys;
    Animator animator;
    public GameObject headPos;
    public GameObject camPos1, camPos2, mainCamera;
    public GameObject gun, gunPosition1, gunPosition2, bullet;
    public RuntimeAnimatorController standartMove, aimingMove;
    GameObject[] bulletPool;
    Transform skeleton;

    
    RaycastHit camHit, aimHit;

    void Start()
    {
        Cursor.visible = false;
        animator = GetComponent<Animator>();
        phys = GetComponent<Rigidbody>();
        headCamPos = headPos.transform.position - transform.position;
        speed = 2;
        skeleton = animator.GetBoneTransform(HumanBodyBones.Chest);
        BulletPool();
    }

    void BulletPool()
    {
        bulletPool = new GameObject[10];
        for (int i = 0; i < bulletPool.Length; i++)
        {
            bulletPool[i] = Instantiate(bullet);
            bulletPool[i].SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            aimCtrl = true;
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            aimCtrl = false;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && aimCtrl)
        {
            animator.SetBool("FireParam", true);
        }
        if (Input.GetKeyUp(KeyCode.Mouse0) && aimCtrl)
        {
            animator.SetBool("FireParam", false);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && !aimCtrl)
        {
            speed *= 2;
            animator.SetBool("RunParam", true);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) && !aimCtrl)
        {
            speed /= 2;
            animator.SetBool("RunParam", false);
        }
       
    }

    void FixedUpdate()
    {
        CharacterPosition();
        CharacterRotation();
        GunPosition();
        if (aimCtrl)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, camPos2.transform.position, 0.1f);
            animator.runtimeAnimatorController = aimingMove;
        }
        else
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, camPos1.transform.position, 0.1f);
            animator.runtimeAnimatorController = standartMove;
        }
    }
    
    private void LateUpdate()
    {
        if (aimCtrl && aimHit.distance > 3)
        {
            skeleton.LookAt(aimHit.point);
            skeleton.rotation = skeleton.rotation * Quaternion.Euler(offset); 
        }
        
    }
    void GunPosition()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0) && aimCtrl)
        {
            gun.transform.position = gunPosition1.transform.position;
            gun.transform.rotation = gunPosition1.transform.rotation;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && aimCtrl)
        {
            gun.transform.position = gunPosition2.transform.position;
            gun.transform.rotation = gunPosition2.transform.rotation;
        }
    }
    void Fire()
    {
        bulletPool[bulletPoolCount].SetActive(true);
        bulletPool[bulletPoolCount].transform.position = gunPosition2.transform.position;
        //bulletPool[bulletPoolCount].transform.rotation = gunPosition2.transform.rotation;
        bulletPool[bulletPoolCount].GetComponent<Rigidbody>().velocity = Vector3.zero;
        bulletPool[bulletPoolCount++].GetComponent<Rigidbody>().AddForce((aimHit.point - gunPosition2.transform.position).normalized * 10000);
        GetComponent<AudioSource>().Play();
        if (bulletPoolCount == bulletPool.Length)
        {
            bulletPoolCount = 0;
        }
    }
    void CharacterPosition()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);

        Vector3 pos = new Vector3(horizontal, 0, vertical);
        pos = transform.TransformDirection(pos);
        pos.Normalize();
        phys.position += pos * Time.fixedDeltaTime * speed;
    }
    void CharacterRotation()
    {
        headPos.transform.position = transform.position + headCamPos;
        mouseY += Input.GetAxis("Mouse Y") * Time.fixedDeltaTime * -150;
        mouseX += Input.GetAxis("Mouse X") * Time.fixedDeltaTime * 150;
        mouseY = Mathf.Clamp(mouseY, -20, 20);
        headPos.transform.rotation = Quaternion.Euler(mouseY, mouseX, transform.eulerAngles.z);
        if (aimCtrl)
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));
            Physics.Raycast(ray, out aimHit);
            Debug.DrawLine(ray.origin, aimHit.point, Color.blue);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(aimHit.point),0.2f);
        }
        else
        {
            Physics.Raycast(Vector3.zero, headPos.transform.GetChild(0).forward, out camHit);
            transform.rotation = Quaternion.LookRotation(new Vector3(camHit.point.x, 0, camHit.point.z));
            Debug.DrawLine(Vector3.zero, camHit.point);
        }



    }
}
