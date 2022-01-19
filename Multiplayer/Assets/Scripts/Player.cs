using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Player : MonoBehaviour
{
    public float speed;
    PhotonView view;

    public Rigidbody2D rb;
    public Camera cam;

    public Vector2 movement;
    Vector2 mousePos;

    public Transform firePoint;
    public GameObject bulletPrefab;

    public float bulletForce = 20f;

    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (view.IsMine)
        {
            /*Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            Vector2 moveAmount = moveInput.normalized * speed * Time.deltaTime;
            transform.position += (Vector3)moveAmount;*/

            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

            rb.velocity = new Vector2(movement.x, movement.y);

            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
    }

    private void FixedUpdate()
    {
        if (view.IsMine)
        {
            rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);

            Vector2 lookDir = mousePos - rb.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = angle;
        }
    }

    void Shoot()
    {
        GameObject bullet = PhotonNetwork.Instantiate(bulletPrefab.name, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
