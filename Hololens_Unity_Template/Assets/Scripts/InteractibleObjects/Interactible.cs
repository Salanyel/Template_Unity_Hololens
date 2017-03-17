using UnityEngine;

/// <summary>
/// The Interactible class flags a Game Object as being "Interactible".
/// Determines what happens when an Interactible is being gazed at.
/// </summary>
public class Interactible : MonoBehaviour
{
    [Tooltip("Audio clip to play when interacting with this hologram.")]
    public AudioClip m_targetFeedbackSound;
    private AudioSource m_audioSource;

    void Start()
    {
        EnableAudioHapticFeedback();
    }

    /// <summary>
    /// Set up the audio feedback for the selection
    /// </summary>
    private void EnableAudioHapticFeedback()
    {
        // If this hologram has an audio clip, add an AudioSource with this clip.
        if (m_targetFeedbackSound != null)
        {
            m_audioSource = GetComponent<AudioSource>();
            if (m_audioSource == null)
            {
                m_audioSource = gameObject.AddComponent<AudioSource>();
            }

            m_audioSource.clip = m_targetFeedbackSound;
            m_audioSource.playOnAwake = false;
            m_audioSource.spatialBlend = 1;
            m_audioSource.dopplerLevel = 0;
        }
    }

    /// <summary>
    /// Function called when the gaze enter the gameObject
    /// </summary>
    virtual protected void GazeEntered()
    {

    }

    /// <summary>
    /// Function called when the gaze exited the gameObject
    /// </summary>
    virtual protected void GazeExited()
    {

    }

    /// <summary>
    /// Function called when an object is selected
    /// </summary>
    virtual protected void OnSelect()
    {
        // Play the audioSource feedback when we gaze and select a hologram.
        if (m_audioSource != null && !m_audioSource.isPlaying)
        {
            m_audioSource.Play();
        }

        this.SendMessage("PerformTagAlong");
    }
}