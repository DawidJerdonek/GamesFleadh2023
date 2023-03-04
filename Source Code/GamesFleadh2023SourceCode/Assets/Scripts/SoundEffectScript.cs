using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectScript : MonoBehaviour
{
    public AudioSource hitSoundEffect;
    public AudioSource gunSoundEffect;
    public AudioSource powerupSoundEffect;
    public AudioSource enemyHitSoundEffect;


    public void playHitSoundEffect()
    {
        hitSoundEffect.Play();
    }

    public void playGunSoundEffect()
    {
        gunSoundEffect.Play();
    }

    public void playPowerupSoundEffect()
    {
        powerupSoundEffect.Play();
    }

    public void playEnemyHitSoundEffect()
    {
        enemyHitSoundEffect.Play();
    }
}
