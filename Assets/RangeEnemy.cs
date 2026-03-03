using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class RangeEnemy : EnemyBehavior
{
    [SerializeField] GameObject proyectile;

    public override IEnumerator InitiateAttack()
    {
        attacking = true;

        characterNavigation.FollowPlayer(false);
        characterNavigation.SetAgentDestination(transform.position);
        transform.LookAt(GameManager.Instance.playerCharacter.transform);

        yield return new WaitForSeconds(time);

        GameObject newProyectile = Instantiate(proyectile, transform.position, transform.rotation);
        newProyectile.transform.LookAt(GameManager.Instance.playerCharacter.transform);
        DamageParameters dmgPars = ScriptableObject.CreateInstance<DamageParameters>();
        dmgPars.SetEqualDamage((int) GetComponent<CharacterNPC>().currentDamage);
        newProyectile.GetComponent<Proyectile>().SetDamageParameters(dmgPars);

        yield return new WaitForSeconds(duration);

        characterNavigation.FollowPlayer(true);

        yield return new WaitForSeconds(cooldown);

        attacking = false;
    }
}
