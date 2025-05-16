public class MovingPlatformManual : MovingPlatform, IDamageable
{
    public void DealDamage(float damage) {
        Activate();
    }    
}
