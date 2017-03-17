using UnityEngine;

namespace Template_Hololens
{
    /// <summary>
    /// GazeManager determines the location of the user's gaze, hit m_position and m_normals.
    /// </summary>
    public class GazeManager : Singleton<GazeManager>
    {
        [Tooltip("Maximum gaze distance for calculating a hit.")]
        public float m_maxGazeDistance = 5.0f;

        [Tooltip("Select the layers raycast should target.")]
        public LayerMask m_raycastLayerMask = Physics.DefaultRaycastLayers;

        /// <summary>
        /// Physics.Raycast result is true if it hits a Hologram.
        /// </summary>
        public bool m_hit { get; private set; }

        /// <summary>
        /// m_hitInfo property gives access
        /// to RaycastHit public members.
        /// </summary>
        public RaycastHit m_hitInfo { get; private set; }

        /// <summary>
        /// m_position of the user's gaze.
        /// </summary>
        public Vector3 m_position { get; private set; }

        /// <summary>
        /// RaycastHit m_normal direction.
        /// </summary>
        public Vector3 m_normal { get; private set; }

        private GazeStabilizer m_gazeStabilizer;
        private Vector3 m_gazeOrigin;
        private Vector3 m_gazeDirection;

        void Awake()
        {
            m_gazeStabilizer = GetComponent<GazeStabilizer>();
        }

        private void Update()
        {
            m_gazeOrigin = Camera.main.transform.position;

            m_gazeDirection = Camera.main.transform.forward;

            m_gazeStabilizer.UpdateHeadStability(m_gazeOrigin, Camera.main.transform.rotation);

            m_gazeOrigin = m_gazeStabilizer.StableHeadPosition;

            UpdateRaycast();
        }

        /// <summary>
        /// Calculates the Raycast hit m_position and m_normal.
        /// </summary>
        private void UpdateRaycast()
        {
            RaycastHit hitInfo;

            m_hit = Physics.Raycast(m_gazeOrigin,
                           m_gazeDirection,
                           out hitInfo,
                           m_maxGazeDistance,
                           m_raycastLayerMask);

            m_hitInfo = hitInfo;

            if (m_hit)
            {
                // If raycast hit a hologram...

                m_position = m_hitInfo.point;
                m_normal = m_hitInfo.normal;
            }
            else
            {
                // If raycast did not hit a hologram...
                // Save defaults ...
                m_position = m_gazeOrigin + (m_gazeDirection * m_maxGazeDistance);
                m_normal = m_gazeDirection;
            }
        }
    }
}