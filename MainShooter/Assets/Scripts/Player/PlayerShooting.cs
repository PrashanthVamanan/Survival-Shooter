using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.15f;
    public float range = 100f;


    float timer;
    Ray shootRay;               //Cast a ray
    RaycastHit shootHit;        //Return whatever was hit by the ray
    int shootableMask;
    ParticleSystem gunParticles;
    LineRenderer gunLine;
    AudioSource gunAudio;
    Light gunLight;
    float effectsDisplayTime = 0.2f;


    void Awake ()
    {
        shootableMask = LayerMask.GetMask ("Shootable"); //Shoot anything available on the shootable layer
        gunParticles = GetComponent<ParticleSystem> ();
        gunLine = GetComponent <LineRenderer> ();
        gunAudio = GetComponent<AudioSource> ();
        gunLight = GetComponent<Light> ();
    }


    void Update ()
    {
        timer += Time.deltaTime;

        //If left mouse button is clicked and if it is time to fire

		if(Input.GetButton ("Fire1") && timer >= timeBetweenBullets)
        {
            Shoot ();
        }

        //If we have fired and enough time has passed after we have fired then disable the effects

        if(timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects ();
        }
    }


    public void DisableEffects ()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }


    void Shoot ()
    {
        timer = 0f;

        gunAudio.Play ();

        gunLight.enabled = true;

        gunParticles.Stop ();   //Reset the particles
        gunParticles.Play ();   //Start playing them from beginning

        gunLine.enabled = true;
        gunLine.SetPosition (0, transform.position);    //Starting point of the line is the tip of the gun

        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        //Shoot a Ray and if it hits something return that else
        //Cast a Ray of infinitely long length

        if(Physics.Raycast (shootRay, out shootHit, range, shootableMask))
        {
            //Whenever we shoot give us access to the enemyhealth script so that enemy's health can be reduced

            EnemyHealth enemyHealth = shootHit.collider.GetComponent <EnemyHealth> ();
            if(enemyHealth != null)    
            {
                enemyHealth.TakeDamage (damagePerShot, shootHit.point);
            }
            gunLine.SetPosition (1, shootHit.point); //Draw a line from the tip of gun to where the ray hits
        }

         //If we don't hit something

        else
        {
            gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);
        }
    }
}
