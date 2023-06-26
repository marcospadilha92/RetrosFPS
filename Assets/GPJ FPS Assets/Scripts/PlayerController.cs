using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController playerController;

    public Rigidbody2D rigidBody2D;

    public float speed = 5f;
    public float mouseSensitive = 1f;
    public Camera viewCamera;

    private Vector2 moveInput;
    private Vector2 mouseInput;

    float maxAngle = 160;
    float minAngle = 10;

    public GameObject bulletImpact;
    public int currentAmmo;

    public Animator gunAnimation;
    
    private void Awake()
    {
        playerController = this;    
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        //player movement
        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        Vector3 movHorizontal = transform.up * -moveInput.x;
        Vector3 movVertical = transform.right * moveInput.y;

        rigidBody2D.velocity = (movHorizontal + movVertical) * speed;

        //player pov
        mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitive;

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,
            transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z - mouseInput.x);

        // usando min angle e max angle para não acontecer bug da visão 360 graus
        Vector3 RotAmount = viewCamera.transform.localRotation.eulerAngles + new Vector3(0f, mouseInput.y, 0f);
        viewCamera.transform.localRotation = Quaternion.Euler(RotAmount.x, Mathf.Clamp(RotAmount.y, minAngle, maxAngle), RotAmount.z);

        //shooting
        if (Input.GetMouseButtonDown(0))
        {
            if (currentAmmo > 0)
            {
                Ray ray = viewCamera.ViewportPointToRay(new Vector3(.5f, .5f, 0f));
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    // Debug.Log("tiro " + hit.transform.name);
                    Instantiate(bulletImpact, hit.point, transform.rotation);

                    if (hit.transform.tag == "Enemy") {
                        hit.transform.parent.GetComponent<EnemyController>().TakeDamage();
                    }
                }
                else
                {
                    Debug.Log("nada");
                }

                currentAmmo--;
                gunAnimation.SetTrigger("shoot");
            }
        }
    }
}
