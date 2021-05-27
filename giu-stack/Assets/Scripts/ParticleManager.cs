using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public ParticleSystem death;    
    public ParticleSystem confetti;
    public ParticleSystem fireWorks;

    private static ParticleManager instance;

    public static ParticleManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public IEnumerator StopParticle(float second)
    {
        yield return new WaitForSeconds(second);
        death.Pause();
    }

}
