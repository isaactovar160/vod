using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reticle : MonoBehaviour
{
    public Pointer m_Pointer;
    public SpriteRenderer m_CircleRenderer;

    public Sprite m_Opensprite;
    public Sprite m_CloseSprite;

    private Camera m_Camera = null;

    private void Awake()
    {
        m_Pointer.OnPointerUpdate += UpdateSprite;
        m_Camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(m_Camera.gameObject.transform);
    }

    private void OnDestroy() 
    {
        m_Pointer.OnPointerUpdate += UpdateSprite;   
    }

    private void UpdateSprite(Vector3 point, GameObject hitObject){
        transform.position = point;

        if(hitObject){
            m_CircleRenderer.sprite = m_CloseSprite;
        }else{
            m_CircleRenderer.sprite = m_Opensprite;
        }
    }
}
