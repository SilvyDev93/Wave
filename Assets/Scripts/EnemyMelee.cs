using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMelee : EnemyBehavior
{
    public override IEnumerator InitiateAttack()
    {
        attacking = true;

        characterNavigation.FollowPlayer(false);
        characterNavigation.SetAgentDestination(transform.position);

        yield return new WaitForSeconds(time);

        hitbox.SetHitboxActive((int) GetComponent<CharacterNPC>().currentDamage);

        yield return new WaitForSeconds(duration);

        hitbox.gameObject.SetActive(false);
        characterNavigation.FollowPlayer(true);

        yield return new WaitForSeconds(cooldown);

        attacking = false;
    }
}
