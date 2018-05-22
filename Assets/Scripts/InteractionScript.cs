using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This class handles the coroutines of every pick ups the cyclist can interact with.
 * The pick ups can also have scripts that does some additional things
 * */
public class InteractionScript : MonoBehaviour {

    public float bonusTime,malusTime;

    private Rigidbody m_rigidBody;
    private SpriteRenderer m_sprite;
    private HealthPlayer m_healthPlayer;

    private float timer=0f;
    private float time_to_blink = 0f;

    void Start () {
        m_rigidBody = GetComponent<Rigidbody>();
        m_sprite = GetComponentInChildren<SpriteRenderer>(); //Cyclist sprite
        m_healthPlayer = GetComponent<HealthPlayer>(); // Vie du Cycliste
	}

	void Update () {

        HandleBlinking();

	}

    //for the interaction with trigger objects (pick ups)
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Boost"))
        {
            Destroy(other.gameObject);
            StartCoroutine("SpeedUp");
        }

    }

    //for the interaction with non trigger objects (Npcs)
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("MarketMen"))
        {
            StartCoroutine("SpeedDown");
            m_healthPlayer.TakeDamage(20);

        }
    }

    private void StartBlinking(float time)
    {
        time_to_blink = time;
    }

    private void HandleBlinking()
    {
        if (time_to_blink > 0)
        {
            timer += Time.deltaTime;
            time_to_blink -= Time.deltaTime;
            if (timer > 0.2)
            {
                timer = 0f;
                m_sprite.enabled = !m_sprite.enabled;
            }
        }
        else if (!m_sprite.enabled)
        {
            m_sprite.enabled = true;
        }
    }

    IEnumerator SpeedUp()
    {
        float time = Time.time;
        GetComponent<CyclistMovement>().forwardForceMultiplier += 50;

        while (Time.time - time < bonusTime)
        {
            yield return null;
        }

        GetComponent<CyclistMovement>().forwardForceMultiplier -= 50;

    }

    IEnumerator SpeedDown()
    {
        StartBlinking(malusTime);

        float fm = GetComponent<CyclistMovement>().forwardForceMultiplier;

        float time = Time.time;
        float reduction = (fm > 50) ? -50 : -fm;

        GetComponent<CyclistMovement>().forwardForceMultiplier += reduction;

        m_rigidBody.velocity = m_rigidBody.velocity*0.5f;
        float time2 = time;

        while (time2 - time < malusTime)
        {
            //progressively recover initial speed
            GetComponent<CyclistMovement>().forwardForceMultiplier -= (Time.time - time2)*(reduction/malusTime);
            time2 = Time.time;
            yield return null;
        }

    }
}
