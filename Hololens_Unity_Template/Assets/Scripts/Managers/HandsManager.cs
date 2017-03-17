using UnityEngine.VR.WSA.Input;
using UnityEngine;

namespace Template_Hololens
{
    /// <summary>
    /// HandsManager keeps track of when a hand is detected.
    /// </summary>
    public class HandsManager : Singleton<HandsManager>
    {
        [Tooltip("Audio clip to play when Finger Pressed.")]
        public AudioClip m_FingerPressedSound;
        private AudioSource m_audioSource;

        /// <summary>
        /// Tracks the hand detected state.
        /// </summary>
        public bool m_handIsDetected
        {
            get;
            private set;
        }

        // Keeps track of the GameObject that the hand is interacting with.
        public GameObject FocusedGameObject { get; private set; }

        void Awake()
        {
            EnableAudioHapticFeedback();

            InteractionManager.SourceDetected += InteractionManager_SourceDetected;
            InteractionManager.SourceLost += InteractionManager_SourceLost;

            //InteractionManager.SourcePressed += InteractionManager_SourcePressed;
            InteractionManager.SourcePressed += InteractionManager_SourcePressed;
            InteractionManager.SourceReleased += InteractionManager_SourceReleased;

            FocusedGameObject = null;
        }

        private void EnableAudioHapticFeedback()
        {
            // If this hologram has an audio clip, add an m_audioSource with this clip.
            if (m_FingerPressedSound != null)
            {
                m_audioSource = GetComponent<AudioSource>();
                if (m_audioSource == null)
                {
                    m_audioSource = gameObject.AddComponent<AudioSource>();
                }

                m_audioSource.clip = m_FingerPressedSound;
                m_audioSource.playOnAwake = false;
                m_audioSource.spatialBlend = 1;
                m_audioSource.dopplerLevel = 0;
            }
        }

        private void InteractionManager_SourceDetected(InteractionSourceState hand)
        {
            m_handIsDetected = true;
        }

        private void InteractionManager_SourceLost(InteractionSourceState hand)
        {
            m_handIsDetected = false;

            ResetFocusedGameObject();
        }

        private void InteractionManager_SourcePressed(InteractionSourceState hand)
        {
            if (InteractibleManager.Instance.m_focusedGameObject != null)
            {
                // Play a select sound if we have an audio source and are not targeting an asset with a select sound.
                if (m_audioSource != null && !m_audioSource.isPlaying &&
                    (InteractibleManager.Instance.m_focusedGameObject.GetComponent<Interactible>() != null &&
                    InteractibleManager.Instance.m_focusedGameObject.GetComponent<Interactible>().m_targetFeedbackSound == null))
                {
                    m_audioSource.Play();
                }

                FocusedGameObject = InteractibleManager.Instance.m_focusedGameObject;
            }
        }

        private void InteractionManager_SourceReleased(InteractionSourceState hand)
        {
            ResetFocusedGameObject();
        }

        private void ResetFocusedGameObject()
        {
            FocusedGameObject = null;

            // On GestureManager call ResetGestureRecognizers to complete any currently active gestures.
            GestureManager.Instance.ResetGestureRecognizers();
        }

        void OnDestroy()
        {
            InteractionManager.SourceDetected -= InteractionManager_SourceDetected;
            InteractionManager.SourceLost -= InteractionManager_SourceLost;

            InteractionManager.SourceReleased -= InteractionManager_SourceReleased;
            InteractionManager.SourcePressed -= InteractionManager_SourcePressed;
        }
    }
}