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

    public void TriggerParticle(EffectType type)
    {
        switch (type)
        {
            case EffectType.Jump:
                // ���� ���� ��ƼŬ ȿ�� ����
                jumpParticle.Play();
                break;
            case EffectType.Dash:
                // ��� ��ƼŬ ȿ�� ����
                dashParticle.Play();
                break;
            case EffectType.Flash:
                // �÷��� ��ƼŬ ȿ�� ����
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
