using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private int lives = 3;
    private float speed = 5.0f;
    private int weaponType;

    private GameManager gameManager;

    private float horizontalInput;
    private float verticalInput;

    public AudioSource bonusPickup;
    public AudioSource powerUp;
    public AudioSource powerDown;

    public GameObject bulletPrefab;
    public GameObject explosionPrefab;
    public GameObject thrusterPrefab;
    public GameObject shieldPrefab;

    public bool shieldStatus;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        lives = 3;
        speed = 5.0f;
        weaponType = 1;
        gameManager.ChangeLivesText(lives);
        shieldStatus = false;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Shooting();
    }

    public void Shielding()
    {
        if (shieldStatus == true)
        {
            lives = lives + 1;
            shieldPrefab.SetActive(false);
            shieldStatus = false;
            powerDown.Play();                                                                                                                                                                                                                                                                                                                                                                       
        }
    }
    public void LoseALife()
    {
        //do I have a shield? if yes: do not lose a life, but instead deactivate shield
        //if not lose a life
        lives = lives - 1;
        gameManager.ChangeLivesText(lives);
        if (lives == 0)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            gameManager.GameOver();
            Destroy(this.gameObject);
        }
    }

    IEnumerator SpeedPowerDown()
    {
        yield return new WaitForSeconds(3f);
        speed = 5f;
        thrusterPrefab.SetActive(false);
        gameManager.ManagePowerupText(0);
        powerDown.Play();
    }

    IEnumerator WeaponPowerDown()
    {
        yield return new WaitForSeconds(3f);
        weaponType = 1;
        gameManager.ManagePowerupText(0);
        powerDown.Play();
    }

    private void OnTriggerEnter2D(Collider2D whatDidIHit)
    {
        if (whatDidIHit.tag == "PowerUp")
        {
            Destroy(whatDidIHit.gameObject);
            int whichPowerUp = Random.Range(1, 5);
            powerUp.Play();
            switch (whichPowerUp)
            {
                case 1:
                    //speed
                    speed = 10f;
                    StartCoroutine(SpeedPowerDown());
                    thrusterPrefab.SetActive(true);
                    gameManager.ManagePowerupText(1);
                    break;
                case 2:
                    //double shot
                    weaponType = 2;
                    StartCoroutine(WeaponPowerDown());
                    gameManager.ManagePowerupText(2);
                    break;
                case 3:
                    //triple shot
                    weaponType = 3;
                    StartCoroutine(WeaponPowerDown());
                    gameManager.ManagePowerupText(3);
                    break;
                case 4:
                    //shield
                    shieldPrefab.SetActive(true);
                    shieldStatus = true;
                    gameManager.ManagePowerupText(4);
                    break;

            }
        }
        if (whatDidIHit.tag == "Bonus")
        {
            bonusPickup.Play();
        }
    }

    void Shooting()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (weaponType)
            {
                case 1:
                    Instantiate(bulletPrefab, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
                    break;
                case 2:
                    Instantiate(bulletPrefab, transform.position + new Vector3(-0.5f, 0.5f, 0), Quaternion.identity);
                    Instantiate(bulletPrefab, transform.position + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
                    break;
                case 3:
                    Instantiate(bulletPrefab, transform.position + new Vector3(-0.5f, 0.5f, 0), Quaternion.Euler(0, 0, 45));
                    Instantiate(bulletPrefab, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
                    Instantiate(bulletPrefab, transform.position + new Vector3(0.5f, 0.5f, 0), Quaternion.Euler(0, 0, -45));
                    break;
            }
        }
    }
    void Movement()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * Time.deltaTime * speed);

        float horizontalScreenSize = gameManager.horizontalScreenSize;
        float verticalScreenSize = gameManager.verticalScreenSize;

        if (transform.position.x <= -horizontalScreenSize || transform.position.x > horizontalScreenSize)
        {
            transform.position = new Vector3(transform.position.x * -1, transform.position.y, 0);
        }

        if (transform.position.y <= -verticalScreenSize || transform.position.y > verticalScreenSize)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y * -1, 0);
        }
    }
}