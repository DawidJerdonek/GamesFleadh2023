using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class GunScript : MonoBehaviour
{
    public GameObject gun;
    public GameObject bullet;
    public float bulletFireTime;
    public float duration;
    private BulletScript bulletScript;
    private SoundEffectScript soundEffectScript;
    private Button shootButton;
    private float bulletYPositionDeviation = 0.6f;
    // Start is called before the first frame update
    void Start()
    {
        
        shootButton = GameObject.FindGameObjectWithTag("ShootButton").GetComponent<Button>();
        shootButton.onClick.AddListener(() => shootGun());
        soundEffectScript = GameObject.Find("SoundEffectManager").GetComponent<SoundEffectScript>();
        //gun = GameObject.Find("Gun");
        //gun.SetActive(true);
        //bulletScript = GameObject.Find("Bullet").GetComponent<BulletScript>();
    }

    //IEnumerator fireBullet()
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(bulletFireTime);
    //        shootGun();
    //    }
    //}

    public void shootGun()
    {
        soundEffectScript.playGunSoundEffect();
        Instantiate(bullet, new Vector2 (this.transform.position.x, this.transform.position.y - bulletYPositionDeviation), Quaternion.identity);
        //Instantiate(bullet, this.transform.position, Quaternion.identity);
        //Instantiate(bullet, this.transform.position, Quaternion.identity);
    }
}
