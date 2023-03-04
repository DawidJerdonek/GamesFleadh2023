using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    public GameObject gun;
    public GameObject bullet;
    public float bulletFireTime;
    public float duration;
    private BulletScript bulletScript;
    private SoundEffectScript soundEffectScript;
    // Start is called before the first frame update
    void Start()
    {
        //gun = GameObject.Find("Gun");
        //gun.SetActive(true);
        //bulletScript = GameObject.Find("Bullet").GetComponent<BulletScript>();

    }

    void OnEnable()
    {
        shootGun();
        StartCoroutine(disable());
        StartCoroutine(fireBullet());
        soundEffectScript = GameObject.Find("SoundEffectManager").GetComponent<SoundEffectScript>();
    }

    IEnumerator fireBullet()
    {
        while (true)
        {
            yield return new WaitForSeconds(bulletFireTime);
            shootGun();
        }
    }

    IEnumerator disable()
    {
        yield return new WaitForSeconds(duration);
        this.gameObject.SetActive(false);
    }

    public void shootGun()
    {
      //  soundEffectScript.playGunSoundEffect();
        Instantiate(bullet, this.transform.position, Quaternion.identity);
        Instantiate(bullet, this.transform.position, Quaternion.identity);
        Instantiate(bullet, this.transform.position, Quaternion.identity);
    }
}
