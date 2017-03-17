using UnityEngine;
using UnityEngine.VR.WSA.Input;

namespace Template_Hololens
{
    public class GestureManager : Singleton<GestureManager>
    {
        // Tap and Navigation gesture recognizer.
        public bool m_hasNavigationRecognizer;        
        public GestureSettings[] m_navigationGestures;
        //[HideInInspector]
        public GestureRecognizer m_navigationRecognizer { get; private set; }

        // Manipulation gesture recognizer.
        public bool m_hasManipulationRecognizer;
        public GestureSettings[] m_manipulationGestures;
        [HideInInspector]
        public GestureRecognizer m_manipulationRecognizer { get; private set; }

        // Currently active gesture recognizer.
        public GestureRecognizer m_activeRecognizer { get; private set; }

        public bool m_isNavigating{ get; private set; }

        public Vector3 m_NavigationPosition { get; private set; }

        public bool m_isManipulating { get; private set; }

        public Vector3 m_manipulationPosition { get; private set; }

        void Awake()
        {

            Debug.Log("NavigationManager : " + m_hasNavigationRecognizer);
            Debug.Log("ManipulationManager : " + m_hasManipulationRecognizer);

            if (m_hasNavigationRecognizer)
            {
                setNavigationRecognizer();
            }

            if (m_hasManipulationRecognizer)
            {
                setManipulationRecognizer();
            }

            ResetGestureRecognizers();
        }

        void OnDestroy()
        {
            m_navigationRecognizer.TappedEvent -= m_tapRecognizer_TappedEvent;
            m_navigationRecognizer.NavigationStartedEvent -= m_navigationRecognizer_NavigationStartedEvent;
            m_navigationRecognizer.NavigationUpdatedEvent -= m_navigationRecognizer_NavigationUpdatedEvent;
            m_navigationRecognizer.NavigationCompletedEvent -= m_navigationRecognizer_NavigationCompletedEvent;
            m_navigationRecognizer.NavigationCanceledEvent -= m_navigationRecognizer_NavigationCanceledEvent;

            m_manipulationRecognizer.ManipulationStartedEvent -= m_manipulationRecognizer_ManipulationStartedEvent;
            m_manipulationRecognizer.ManipulationUpdatedEvent -= m_manipulationRecognizer_ManipulationUpdatedEvent;
            m_manipulationRecognizer.ManipulationCompletedEvent -= m_manipulationRecognizer_ManipulationCompletedEvent;
            m_manipulationRecognizer.ManipulationCanceledEvent -= m_manipulationRecognizer_ManipulationCanceledEvent;
        }

        GestureSettings setGestureSettings(GestureSettings[] p_gestures)
        {
            GestureSettings gestureMask = p_gestures[0];

            for (int index = 1; index < p_gestures.Length; ++index)
            {
                gestureMask = gestureMask | p_gestures[index];
            }

            return gestureMask;
        }
        
        /// <summary>
        /// Set the gesture and the triggers for the navigation recognizer
        /// </summary>
        void setNavigationRecognizer()
        {
            m_navigationRecognizer = new GestureRecognizer();

            if (m_navigationGestures.Length < 1)
            {
                Debug.Log("/!\\ No gesture for the navigation recognizer", this.gameObject);
                return;
            }

            GestureSettings gestureSettings = setGestureSettings(m_navigationGestures);            

            m_navigationRecognizer.SetRecognizableGestures(
                gestureSettings
                );

            //Events to configure !
            m_navigationRecognizer.TappedEvent += m_tapRecognizer_TappedEvent;

            m_navigationRecognizer.NavigationStartedEvent += m_navigationRecognizer_NavigationStartedEvent;
            m_navigationRecognizer.NavigationUpdatedEvent += m_navigationRecognizer_NavigationUpdatedEvent;
            m_navigationRecognizer.NavigationCompletedEvent += m_navigationRecognizer_NavigationCompletedEvent;
            m_navigationRecognizer.NavigationCanceledEvent += m_navigationRecognizer_NavigationCanceledEvent;
        }

        /// <summary>
        /// Set the gesture and the triggers for the navigation recognizer
        /// </summary>
        void setManipulationRecognizer()
        {
            m_manipulationRecognizer = new GestureRecognizer();

            if (m_manipulationGestures.Length < 1)
            {
                Debug.Log("/!\\ No gesture for the manipulation recognizer", this.gameObject);
                return;
            }

            GestureSettings gestureSettings = setGestureSettings(m_manipulationGestures);

            //Configure the trigger
            m_manipulationRecognizer.SetRecognizableGestures(
                gestureSettings
                );            

            //Events to configure !
            m_manipulationRecognizer.ManipulationStartedEvent += m_manipulationRecognizer_ManipulationStartedEvent;
            m_manipulationRecognizer.ManipulationUpdatedEvent += m_manipulationRecognizer_ManipulationUpdatedEvent;
            m_manipulationRecognizer.ManipulationCompletedEvent += m_manipulationRecognizer_ManipulationCompletedEvent;
            m_manipulationRecognizer.ManipulationCanceledEvent += m_manipulationRecognizer_ManipulationCanceledEvent;
        }

        /// <summary>
        /// Revert back to the default GestureRecognizer.
        /// </summary>
        public void ResetGestureRecognizers()
        {
            // Default to the navigation gestures.
            Transition(m_navigationRecognizer);
        }

        /// <summary>
        /// Transition to a new GestureRecognizer.
        /// </summary>
        /// <param name="newRecognizer">The GestureRecognizer to transition to.</param>
        public void Transition(GestureRecognizer newRecognizer)
        {
            if (newRecognizer == null)
            {
                return;
            }

            if (m_activeRecognizer != null)
            {
                if (m_activeRecognizer == newRecognizer)
                {
                    return;
                }

                m_activeRecognizer.CancelGestures();
                m_activeRecognizer.StopCapturingGestures();
            }

            newRecognizer.StartCapturingGestures();
            m_activeRecognizer = newRecognizer;
        }

        #region Navigation_Event        

        private void m_navigationRecognizer_NavigationStartedEvent(InteractionSourceKind source, Vector3 relativePosition, Ray ray)
        {
            m_isNavigating= true;

            //Behavior for the navigation start
            //m_NavigationPosition = relativePosition;

        }

        private void m_navigationRecognizer_NavigationUpdatedEvent(InteractionSourceKind source, Vector3 relativePosition, Ray ray)
        {
            m_isNavigating= true;

            //Behavior for the navigation update
            //m_NavigationPosition = relativePosition;

        }

        private void m_navigationRecognizer_NavigationCompletedEvent(InteractionSourceKind source, Vector3 relativePosition, Ray ray)
        {
            m_isNavigating= false;
        }

        private void m_navigationRecognizer_NavigationCanceledEvent(InteractionSourceKind source, Vector3 relativePosition, Ray ray)
        {
            m_isNavigating= false;

        }

        #endregion

        #region Manipulation_Event

        private void m_manipulationRecognizer_ManipulationStartedEvent(InteractionSourceKind source, Vector3 position, Ray ray)
        {
            /*if (HandsManager.Instance.FocusedGameObject != null)
            {
                m_isManipulating = true;

                m_manipulationPosition = position;

                HandsManager.Instance.FocusedGameObject.SendMessageUpwards("PerformManipulationStart", position);
            }*/
        }

        private void m_manipulationRecognizer_ManipulationUpdatedEvent(InteractionSourceKind source, Vector3 position, Ray ray)
        {
            /*if (HandsManager.Instance.FocusedGameObject != null)
            {
                m_isManipulating = true;

                m_manipulationPosition = position;

                HandsManager.Instance.FocusedGameObject.SendMessageUpwards("PerformManipulationUpdate", position);
            }*/
        }

        private void m_manipulationRecognizer_ManipulationCompletedEvent(InteractionSourceKind source, Vector3 position, Ray ray)
        {
            m_isManipulating = false;
        }

        private void m_manipulationRecognizer_ManipulationCanceledEvent(InteractionSourceKind source, Vector3 position, Ray ray)
        {
            m_isManipulating = false;
        }

        #endregion

        #region Tap_Event

        private void m_tapRecognizer_TappedEvent(InteractionSourceKind source, int tapCount, Ray ray)
        {
            GameObject focusedObject = InteractibleManager.Instance.m_focusedGameObject;

            if (focusedObject != null)
            {
                focusedObject.SendMessageUpwards("OnSelect");
            }
        }

        #endregion
    }
}