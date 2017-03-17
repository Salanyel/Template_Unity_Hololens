using Template_Hololens;
using UnityEngine;

/// <summary>
/// InteractibleManager keeps tracks of which GameObject
/// is currently in focus.
/// </summary>
public class InteractibleManager : Singleton<InteractibleManager>
{
    public GameObject m_focusedGameObject { get; private set; }

    private GameObject m_oldFocusedGameObject = null;

    void Start()
    {
        m_focusedGameObject = null;
    }

    void Update()
    {
        m_oldFocusedGameObject = m_focusedGameObject;

        if (GazeManager.Instance.m_hit)
        {
            RaycastHit hitInfo = GazeManager.Instance.m_hitInfo;
            if (hitInfo.collider != null)
            {
                m_focusedGameObject = hitInfo.collider.gameObject;
            }
            else
            {
                m_focusedGameObject = null;
            }
        }
        else
        {
            m_focusedGameObject = null;
        }

        if (m_focusedGameObject != m_oldFocusedGameObject)
        {
            ResetFocusedInteractible();

            if (m_focusedGameObject != null)
            {
                if (m_focusedGameObject.GetComponent<Interactible>() != null)
                {
                    m_focusedGameObject.SendMessage("GazeEntered");
                }
            }
        }
    }

    private void ResetFocusedInteractible()
    {
        if (m_oldFocusedGameObject != null)
        {
            if (m_oldFocusedGameObject.GetComponent<Interactible>() != null)
            {
                m_oldFocusedGameObject.SendMessage("GazeExited");
            }
        }
    }
}