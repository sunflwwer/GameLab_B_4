using UnityEngine;

public enum EffectType
{
    Jump,
    Dash,
    Flash,
    Explosion,
    Implosion
}

public class PlayerEffect : MonoBehaviour
{
    [SerializeField] ParticleSystem jumpParticle;
    [SerializeField] ParticleSystem dashParticle;
    [SerializeField] ParticleSystem flashParticle;
    [SerializeField] ParticleSystem explosionParticle;
    [SerializeField] ParticleSystem implosionParticle;

    private void Start()
    {
        if (explosionParticle != null)
        {
            // use unscaled time for explosion particle
            ParticleSystem.MainModule main = explosionParticle.main;
            main.useUnscaledTime = true;
        }
    }


    public void TriggerParticle(EffectType type)
    {
        switch (type)
        {
            case EffectType.Jump:
                // 더블 점프 파티클 효과 실행
                jumpParticle.Play();
                break;
            case EffectType.Dash:
                // 대시 파티클 효과 실행
                dashParticle.Play();
                break;
            case EffectType.Flash:
                // 플래시 파티클 효과 실행
                flashParticle.Play();
                break;
            case EffectType.Explosion:
                explosionParticle.Play();
                break;
            case EffectType.Implosion:
                implosionParticle.Play();
                break;
            default:
                break;
        }
    }
}
