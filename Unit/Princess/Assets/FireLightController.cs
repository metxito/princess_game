using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class FireLightController : MonoBehaviour
{
    private Light2D m_Light;


    [Range(0.01f, 1f)] [SerializeField] private float m_FrequenceHigh = .5f;
    [Range(0.01f, 1f)] [SerializeField] private float m_FrequenceLow = .2f;

    [Range(0f, 1f)] [SerializeField] private float m_RadioHigh = .5f;
    [Range(0f, 1f)] [SerializeField] private float m_RadioLow = .2f;


    private float m_NextChangeTime = 0f;
    private float m_OriginalIntensity;
    private float m_CurrentTargetRadio = 0f;


    void Awake()
    {
        m_Light = this.gameObject.GetComponent<Light2D>();
        m_OriginalIntensity = m_Light.intensity;
    }

    

    // Update is called once per frame
    void Update()
    {
        m_NextChangeTime -= Time.deltaTime;
        if (m_NextChangeTime <= 0f) {
            m_CurrentTargetRadio = m_OriginalIntensity * Random.Range(m_RadioLow, m_RadioHigh);
            m_NextChangeTime = Random.Range(m_FrequenceLow, m_FrequenceHigh);
        }
        m_Light.intensity = Mathf.Lerp(m_Light.volumeOpacity, m_CurrentTargetRadio, .3f);
        //m_Light.volumeOpacity = Mathf.Lerp(m_Light.volumeOpacity, m_CurrentTargetRadio, .3f);
    }
}
