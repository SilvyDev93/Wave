using UnityEngine;

[CreateAssetMenu(fileName = "DamageParameters", menuName = "Scriptable Objects/DamageParameters")]
public class DamageParameters : ScriptableObject
{
    [Header("Damage Range")]
    public int minDamage;
    public int maxDamage;


    public float criticalChance;
}
