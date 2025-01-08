using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWhenParticlesFinished : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private ParticleSystem targetParticleSystem;
    // Update is called once per frame
    void Update()
    {
        if (!this.targetParticleSystem.IsAlive())
            Destroy(this.targetParticleSystem.gameObject);
    }
}
