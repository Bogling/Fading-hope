using UnityEngine;

public class FearHitbox : MonoBehaviour, IDamageable
{
    [SerializeField] private FearAI fear;

    public void DealDamage(float damage)
    {
        fear.DealDamage(damage);
    }
}
