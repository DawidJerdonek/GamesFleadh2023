using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectScript : MonoBehaviour
{
    public AudioSource hitSoundEffect;
    public AudioSource gunSoundEffect;
    public AudioSource powerupSoundEffect;
    public AudioSource enemyHitSoundEffect;
    public AudioSource reloadSoundEffect;
    public AudioSource resistanceSoundEffect;
    public AudioSource reviveSoundEffect;

    public AudioClip gunShootClip;
    public AudioClip hitClip;
    public AudioClip powerupClip;
    public AudioClip enemyHitClip;
    public AudioClip resistanceClip;
    public AudioClip reviveClip;

    public void playHitSoundEffect()
    {
        hitSoundEffect.PlayOneShot(hitClip, 1);
    }

    public void playGunSoundEffect()
    {
        gunSoundEffect.PlayOneShot(gunShootClip, 1.0f);
    }

    public void playPowerupSoundEffect()
    {
        powerupSoundEffect.PlayOneShot(powerupClip, 0.2f);
    }

    public void playEnemyHitSoundEffect()
    {
        enemyHitSoundEffect.PlayOneShot(enemyHitClip, 1);
    }

    public void PlayReloadSoundEffect()
    {
        reloadSoundEffect.Play();
    }

    public void PlayResistSoundEffect()
    {
        resistanceSoundEffect.Play();
    }

    public void PlayReviveSoundEffect()
    {
        reviveSoundEffect.Play();
    }
}
