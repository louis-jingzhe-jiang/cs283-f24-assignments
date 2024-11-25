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
    public float attackRange;
    public float observeRange;
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

    IEnumerator<BTState> Attack()
    {
        // To Be Implemented
        yield return BTState.Success;
    }

    IEnumerator<BTState> Look()
    {
        // To Be Implemented
        yield return BTState.Success;
    }

    IEnumerator<BTState> Idle()
    {
        // To Be Implemented
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