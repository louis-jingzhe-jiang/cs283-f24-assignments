using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BTAI;
using UnityEngine.AI;

/*
 * Basic idea of the behavior of the skeleton minion:
 * - Root
 *   - Main Selector (Selector)
 *     - Attack Sequence (Sequence)
 *       - Determine whether the enemy is in sight (Condition)
 *       - Do attack enemy (Action / RunCoroutine)
 *     - Watch Sequence (Sequence)
 *       - Determine whether the player is in observe range
 *       - Do look towards (Action / RunCoroutine)
 *     - Idle Sequence (Sequence)
 *       - Do idle (Action / RunCoroutine)
 */

public class BehaviorUnique : MonoBehaviour
{
    // public variables
    public GameObject enemy;
    public GameObject player;
    public GameObject bullet;
    public float attackRange;
    public float observeRange;
    public float bulletSpeed;
    // behavior tree structure
    private Root treeRoot;
    private Selector selector;
    private Sequence attackSeq;
    private BTNode enemyInRangeC;
    private BTNode attack;
    private Sequence watchSeq;
    private BTNode playerInSightC;
    private BTNode look;
    private Sequence idleSeq;
    private BTNode idle;
    // other private variables
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        // initialize nodes
        treeRoot = BT.Root();
        selector = BT.Selector(false);
        attackSeq = BT.Sequence();
        enemyInRangeC = BT.Condition(EnemyInRangeM);
        attack = BT.RunCoroutine(Attack);
        watchSeq = BT.Sequence();
        playerInSightC = BT.Condition(PlayerInSightM);
        look = BT.RunCoroutine(Look);
        idleSeq = BT.Sequence();
        idle = BT.RunCoroutine(Idle);
        // link nodes
        attackSeq.OpenBranch(enemyInRangeC, attack);
        watchSeq.OpenBranch(playerInSightC, look);
        idleSeq.OpenBranch(idle);
        selector.OpenBranch(attackSeq, watchSeq, idleSeq);
        treeRoot.OpenBranch(selector);
        // initialize other private variables
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        treeRoot.Tick();
    }

    /// <summary>
    /// The defender shoots a bullet towards the enemy. This function is done
    /// by moving the GameObject bullet towards the enemy at every frame
    /// </summary>
    /// <returns></returns>
    IEnumerator<BTState> Attack()
    {
        // move bullet to the defender's position
        bullet.transform.position = transform.position;
        // make the bullet visible
        bullet.SetActive(true);
        // bullet will be active for at most 5 seconds
        float time = 0f;
        while (time < 5f)
        {
            time += Time.deltaTime;
            Vector3 lookDirection = enemy.transform.position 
                - transform.position;
            transform.rotation = Quaternion.LookRotation(lookDirection);
            Vector3 direction = enemy.transform.position 
                - bullet.transform.position;
            if (Vector3.Magnitude(direction) <= 0.1f)
            {
                break;
            }
            Vector3 unitDirection = Vector3.Normalize(direction);
            bullet.transform.position += bulletSpeed * Time.deltaTime 
                * unitDirection;
            yield return BTState.Continue;
        }
        bullet.SetActive(false);
        yield return BTState.Success;
    }

    /// <summary>
    /// The defender look towards the player. This function is done through
    /// look towards.
    /// </summary>
    /// <returns></returns>
    IEnumerator<BTState> Look()
    {
        // To Be Implemented
        Vector3 direction = player.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(direction);
        yield return BTState.Success;
    }

    /// <summary>
    /// The defender look towards some random point. This function is done 
    /// through lerp and/or slerp.
    /// </summary>
    /// <returns></returns>
    IEnumerator<BTState> Idle()
    {
        // To Be Implemented
        float xComponent = Random.Range(-1f, 1f);
        float zComponent = Random.Range(-1f, 1f);
        Vector3 lookDirection = new Vector3(xComponent, 0, zComponent);
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
        Quaternion startRotation = transform.rotation;
        // each slerp should last 3 seconds
        float timeElapsed = 0;
        float time = 3f;
        while (timeElapsed <= time)
        {
            transform.rotation = Quaternion.Slerp(startRotation, 
                targetRotation, timeElapsed / time);
            timeElapsed += Time.deltaTime;
            yield return BTState.Continue;
        }
        // rest for 5 seconds
        time = 0f;
        while (time < 5f)
        {
            time += Time.deltaTime;
            yield return BTState.Continue;
        }
        yield return BTState.Success;
    }

    bool EnemyInRangeM()
    {
        float distance = Vector3.Distance(transform.position, 
            enemy.transform.position);
        if (distance < attackRange)
        {
            return true;
        }
        return false;
    }

    bool PlayerInSightM()
    {
        float distance = Vector3.Distance(transform.position,
            player.transform.position);
        if (distance < observeRange)
        {
            return true;
        }
        return false;
    }
}