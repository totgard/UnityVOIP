using UnityEngine;

namespace UnityVOIP
{
    public class VoipSpeaker : VoipBehaviour
    {
        private const int Padding = 2;
        private const int BufferChunks = 10;

        private SpeexCodex decoder;
        private VoipBuffer buffer;

        private AudioSource source;
        private AudioClip recording;

        private int position;
        private int lastWrittenCursor = 0;
        private int lastReadCursor = 0;

        //The number of "frames" to delay the stream by.
        public int delay = 4;

        //Audio stuff.
        [Header("Volume Equalizer")]
        public float currentGain = 1f;
        public float targetGain = 1f;
        public bool Equalize = false;
        public float EqualizeSpeed = 1f;
        public float TargetEqualizeVolume = 1.2f;
        public float MaxEqualization = 5f;

        void Start()
        {

        }

        void Update()
        {
            if (source == null) return;

            delay = Mathf.Clamp(delay, Padding, Mathf.Max(Padding, buffer.fragmentCount - Padding));

            currentGain = Mathf.MoveTowards(currentGain, targetGain, Time.deltaTime * EqualizeSpeed);

            var now = Wrap(CurrentIndex() + buffer.fragmentSize * delay);
            if (now > position)
            {
                while (now - position > buffer.fragmentSize)
                {
                    //Play catch up if need be.
                    if (lastReadCursor < lastWrittenCursor - 30)
                    {
                        lastReadCursor = lastWrittenCursor - 30;
                    }

                    var data = buffer.Read(lastReadCursor);
                    recording.SetData(data, position);
                    if (lastReadCursor <= lastWrittenCursor)
                    {
                        lastReadCursor++;
                    }
                    position += buffer.fragmentSize;
                }
            }
            else
            {
                position = 0;
            }

            UpdateStats();
        }

        void ApplyGain(float[] data)
        {
            targetGain = TargetEqualizeVolume / AudioUtils.GetMaxAmplitude(data); ;
            if (targetGain > MaxEqualization)
            {
                targetGain = MaxEqualization;
            }
            if (targetGain < currentGain)
            {
                currentGain = targetGain;
            }
            AudioUtils.ApplyGain(data, currentGain);
        }

        int Wrap(int time)
        {
            return time >= recording.samples ? time - recording.samples : time;
        }

        int CurrentIndex()
        {
            return (source.timeSamples - source.timeSamples % buffer.fragmentSize);
        }

        public void Recieve(VoipFragment audio)
        {
            bytes += audio.data.Length + 11;
            chunkCount++;

            //If params have changed or decoder not set..
            if (decoder == null || decoder.mode != audio.mode)
            {
                //Create decoder.
                decoder = SpeexCodex.Create(audio.mode);
                //Create a buffer.
                buffer = new VoipBuffer(decoder.dataSize, BufferChunks);
                //Create clip.
                var frequency = AudioUtils.GetFrequency(audio.mode);
                recording = AudioClip.Create("Inbound", frequency * 10, 1, frequency, false);
                //Create source.
                source = gameObject.GetComponent<AudioSource>();
                if (source == null) source = gameObject.AddComponent<AudioSource>();
                source.clip = recording;
                source.Play();
                source.loop = true;
            }

            //Decode.
            var decoded = decoder.Decode(audio.data);
            //Apply effects.
            if (Equalize)
            {
                ApplyGain(decoded);
            }
            //Write to voip buffer.
            buffer.Write(audio.index, decoded);
            //Update write cursor.
            lastWrittenCursor = audio.index > lastWrittenCursor ? audio.index : lastWrittenCursor;
        }
    }
}