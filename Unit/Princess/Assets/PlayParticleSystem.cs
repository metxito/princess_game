using UnityEngine;

public class PlayParticleSystem : MonoBehaviour
{
    [SerializeField] private ParticleSystem particlesA;

    public void PlayPartitcleSystemA(){
        if (particlesA != null)
            particlesA.Play();
    }
}
