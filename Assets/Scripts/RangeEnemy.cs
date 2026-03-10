using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class RangeEnemy : EnemyBehavior
{
    [SerializeField] GameObject proyectile;
    [SerializeField] Transform shootOrigin;

    Transform proyectileSpawnPoint;

    public override IEnumerator InitiateAttack()
    {
        attacking = true;

        characterNavigation.FollowPlayer(false);
        characterNavigation.SetAgentDestination(transform.position);
        transform.LookAt(GameManager.Instance.playerCharacter.transform);

        yield return new WaitForSeconds(time);

        GameObject newProyectile = Instantiate(proyectile, proyectileSpawnPoint.position, proyectileSpawnPoint.rotation);
        newProyectile.transform.LookAt(GameManager.Instance.playerCharacter.transform);
        DamageParameters dmgPars = ScriptableObject.CreateInstance<DamageParameters>();
        dmgPars.SetEqualDamage((int) GetComponent<CharacterNPC>().currentDamage);
        newProyectile.GetComponent<Proyectile>().SetDamageParameters(dmgPars);

        yield return new WaitForSeconds(duration);

        characterNavigation.FollowPlayer(true);

        yield return new WaitForSeconds(cooldown);

        attacking = false;
    }

    public override void OnAwake()
    {
        base.OnAwake();

        if (shootOrigin == null)
        {
            proyectileSpawnPoint = transform;
        }
        else
        {
            proyectileSpawnPoint = shootOrigin;
        }
    }
}
