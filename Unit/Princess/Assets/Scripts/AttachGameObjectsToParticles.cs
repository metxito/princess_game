using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(ParticleSystem))]
public class AttachGameObjectsToParticles : MonoBehaviour
{
    public GameObject m_Prefab;

    
    private ParticleSystem m_ParticleSystem;
    private List<GameObject> m_Instances = new List<GameObject>();
    private ParticleSystem.Particle[] m_Particles;
    private float m_InnerRadius = 1f;
    private float m_OuterRadius = 1f;
    private float intensity = 4f;
    // Start is called before the first frame update
    void Start()
    {
        m_ParticleSystem = GetComponent<ParticleSystem>();
        m_Particles = new ParticleSystem.Particle[m_ParticleSystem.main.maxParticles];
        m_InnerRadius = m_Prefab.GetComponent<Light2D>().pointLightInnerRadius;
        m_OuterRadius = m_Prefab.GetComponent<Light2D>().pointLightOuterRadius;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        int count = m_ParticleSystem.GetParticles(m_Particles);

        while (m_Instances.Count < count)
            m_Instances.Add(Instantiate(m_Prefab, m_ParticleSystem.transform));

        bool worldSpace = (m_ParticleSystem.main.simulationSpace == ParticleSystemSimulationSpace.World);
        for (int i = 0; i < m_Instances.Count; i++)
        {
            Light2D m_Light = m_Instances[i].GetComponent<Light2D>();
            if (i < count && m_Light != null)
            {
                if (worldSpace)
                    m_Instances[i].transform.position = m_Particles[i].position;
                else
                    m_Instances[i].transform.localPosition = m_Particles[i].position;
                
                float currentAlfa = ((float)m_Particles[i].GetCurrentColor(m_ParticleSystem).a / 255);

                m_Light.pointLightInnerRadius = m_InnerRadius * currentAlfa;
                m_Light.pointLightOuterRadius = m_OuterRadius * currentAlfa;

                //m_Light.pointLightOuterRadius = m_Particles[i].GetCurrentColor(m_ParticleSystem) * intensity * .6f;
                //m_Light.pointLightInnerRadius = m_Particles[i].GetCurrentSize(m_ParticleSystem) * intensity;
                m_Instances[i].SetActive(true);
            }
            else
            {
                m_Instances[i].SetActive(false);
            }
        }
    }
}

