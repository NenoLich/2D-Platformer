using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour {

    public float activationTime = 1.5f;
    public float activationRadius = 0.5f;

    private Animator anim;

    void Start ()
    {
        Invoke("Activate", activationTime);
        anim = GetComponent<Animator>();
    }

    private void Activate()
    {
        StartCoroutine(CheckEnemy());
    }

    IEnumerator CheckEnemy()
    {
        Collider2D enemyCollider=null;
        while (enemyCollider == null)
        {
            enemyCollider = Physics2D.Linecast(transform.position - transform.right * activationRadius, 
                transform.position + transform.right * activationRadius, LayerMask.GetMask("Enemies")).collider;

            yield return new WaitForSeconds(0.1f);
        }

        Catch(enemyCollider);
    }

    private void Catch(Collider2D collider)
    {
        collider.GetComponent<Enemy>().Hurt(2);
        anim.SetTrigger("Catch");
        Destroy(gameObject,0.4f);
    }
}
