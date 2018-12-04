using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {

    private bool alive = true;

    private float speed = 3f;

    private Animator zombieAnim;

    private Collider2D zombieCollider;

    [SerializeField]
    private Image pauseMenu;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject zombie;

    // Use this for initialization
    void Start () {
        zombieAnim = GetComponent<Animator>();
        zombieCollider = GetComponent<Collider2D>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (alive == true)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
    }

    IEnumerator HitByPlayer()
    {
        if (alive == true)
        {
            alive = false;
            Debug.Log("Zombie Killed");
            zombieAnim.SetBool("ZombieDead", true);
            yield return new WaitForSeconds(0.001f);
            zombieCollider.enabled = false;
            yield return new WaitForSeconds(2);
            Destroy(zombie.gameObject);
            
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collided");
        if (other.CompareTag("Player") && alive == true)
        {
            Time.timeScale = 0f;
            pauseMenu.gameObject.SetActive(true);
        }
    }
}
