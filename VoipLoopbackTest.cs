using System.Collections.Generic;
using UnityEngine;

namespace UnityVOIP
{
    /// <summary>
    /// This is a test class to diagnose issues.
    /// </summary>
    [RequireComponent(typeof(VoipListener))]
    [RequireComponent(typeof(VoipSpeaker))]
    public class VoipLoopbackTest : MonoBehaviour
    {
        public VoipListener listener { get { return GetComponent<VoipListener>();  } }
        public VoipSpeaker speaker { get { return GetComponent<VoipSpeaker>(); } }

        private List<NetworkContainer> queue = new List<NetworkContainer>();
        
        /// <summary>
        /// Simulated packet loss.
        /// </summary>
        [Range(0f, 1f)]
        public float packetLoss = 0f;

        /// <summary>
        /// Delay the recieve call.
        /// </summary>
        [Range(0f, 1f)]
        public float networkDelaySec = 0f;

        /// <summary>
        /// This is used to test fragments being out of order.
        /// </summary>
        [Range(0f, 0.1f)]
        public float networkStutterSec = 0f;

        void OnEnable()
        {
            listener.OnAudioGenerated += OnAudioGenerated;
        }

        void OnDisable()
        {
            listener.OnAudioGenerated -= OnAudioGenerated;
        }

        void Update()
        {
            if (queue.Count == 0) return;
            var data = queue.Find(o => o.releaseAt <= Time.time);
            if (data == null) return;
            queue.Remove(data);
            speaker.Recieve(data.fragment);
        }

        private void OnAudioGenerated( VoipFragment audio )
        {
            if ( Random.value >= packetLoss )
            {
                queue.Add(new NetworkContainer()
                {
                    fragment = audio,
                    releaseAt = Time.time + networkDelaySec + Random.Range(networkStutterSec, networkStutterSec)
                });
            }
        }

        public class NetworkContainer
        {
            public float releaseAt;
            public VoipFragment fragment;
        }
    }
}