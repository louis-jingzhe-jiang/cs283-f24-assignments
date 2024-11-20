using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BTAI;
/**
 * Basic idea of the behavior of the skeleton minion:
 * - Root
 *   - Main Selector (Selector)
 *     - Follow and Attack Sequence (Sequence)
 *       - Determine whether the player is in sight (Condition)
 *       - Second Selector (Selector)
 *         - Attack Sequence (Sequence)
 *           - Determine whether the player is in attack range (Condition)
 *           - Do attack (Action / RunCoroutine)
 *           - Do follow (Action / RunCoroutine)
 *         - Follow Sequence (Sequence)
 *           - Do follow (Action / RunCoroutine)
 *     - Retreat Sequence (Sequence)
 *       - Determine whether the player is in its home area (Condition)
 *       - Do retreat (Action / RunCoroutine)
 *     - Idle Sequence (Sequence)
 *       - Do idle (Action / RunCoroutine)
 */
public class BehaviorMinion : MonoBehaviour
{
    public GameObject playerHome; // home area of player
    public GameObject minionHome; // home are area of the minion
    
    // below are the private variables regarding the structure of the behavior
    // tree's structure
    private Root treeRoot; // root
    private Selector mainSelector; // Main Selector
    private Sequence followAttack; // Follow and Attack Sequence
    // Determine whether the player is in sight
    private bool isInSight;
    private BTNode isInSightC;
    private Selector secondSelector; // Second Selector
    private Sequence attackSequence; // Attack Sequence
    // Determine whether the player is in attack range
    private bool shouldAttack;
    private BTNode shouldAttackC;
    private BTNode attack; // Do Attack
    private BTNode follow; // Do Follow
    private Sequence followSequence; // Follow Sequence
    private Sequence retreatSequence; // Retreat Sequence
    // Determine whether the player is in its home area
    private bool shouldRetreat;
    private BTNode shouldRetreatC;
    private BTNode retreat; // Do Retreat
    private Sequence idleSequence; // Idle Sequence
    private BTNode idle; // Do Idle


    // Start is called before the first frame update
    void Start()
    {
        // initialize
        treeRoot = BT.Root();
        mainSelector = BT.Selector();
        followAttack = BT.Sequence();
        isInSightC = BT.Condition(IsInSightM);
        secondSelector = BT.Selector();
        attackSequence = BT.Sequence();
        shouldAttackC = BT.Condition(ShouldAttackM);
        attack = BT.RunCoroutine(Attack);
        follow = BT.RunCoroutine(Follow);
        followSequence = BT.Sequence();
        retreatSequence = BT.Sequence();
        shouldRetreatC = BT.Condition(ShouldRetreatM);
        retreat = BT.RunCoroutine(Retreat);
        idleSequence = BT.Sequence();
        idle = BT.RunCoroutine(Idle);
        // link
        idleSequence.OpenBranch(idle);
        followSequence.OpenBranch(follow);
        retreatSequence.OpenBranch(shouldRetreatC);
        retreatSequence.OpenBranch(retreat);
        attackSequence.OpenBranch(shouldAttackC);
        attackSequence.OpenBranch(attack);
        attackSequence.OpenBranch(follow);
        secondSelector.OpenBranch(attackSequence);
        secondSelector.OpenBranch(followSequence);
        followAttack.OpenBranch(isInSightC);
        followAttack.OpenBranch(secondSelector);
        mainSelector.OpenBranch(followAttack);
        mainSelector.OpenBranch(retreatSequence);
        mainSelector.OpenBranch(idleSequence);
        treeRoot.OpenBranch(mainSelector);
    }

    // Update is called once per frame
    void Update()
    {
        treeRoot.Tick();
    }

    /**
     * The skeleton minion randomly moves to a location in the range determined
     * by the GameObject minionHome, and then stays at the location for a few 
     * seconds. The minion should play moving animation when moving, and rest 
     * animation when it is at rest.
     */
    IEnumerator<BTState> Idle() 
    {
        // To Be Implemented
        yield return BTState.Success;
    }

    IEnumerator<BTState> Follow()
    {
        // To Be Implemented
        yield return BTState.Success;
    }

    /**
     * The minion's animator switches from moving animation to attack 
     * animation. After playing the attack animation, the minion's animator
     * should switch to rest animation.
     */
    IEnumerator<BTState> Attack()
    {
        // To Be Implemented
        yield return BTState.Success;
    }

    /** 
     * The minion returns to the center of its spawn area (minionHome). This
     * process cannot be interrupted by any action of the player. Moving 
     * animation should be played at all times.
     */
    IEnumerator<BTState> Retreat()
    {
        // To Be Implemented
        yield return BTState.Success;
    }

    bool IsInSightM()
    {
        // To Be Implemented
        return true;
    }

    bool ShouldAttackM()
    {
        // To Be Implemented
        return true;
    }

    bool ShouldRetreatM()
    {
        // To Be Implemented
        return true;
    }
}
