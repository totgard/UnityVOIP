namespace UnityVOIP
{
    public class VoipBuffer
    {
        public readonly int fragmentSize;
        public readonly int fragmentCount;

        private readonly Fragment[] data;
        private readonly float[] empty;

        public VoipBuffer(int fragmentSize, int fragmentCount)
        {
            this.fragmentSize = fragmentSize;
            this.fragmentCount = fragmentCount;

            data = new Fragment[fragmentCount];
            empty = new float[fragmentSize];

            for ( int i = 0; i < fragmentCount; i ++ )
            {
                data[i] = new Fragment();
            }
        }

        public void Write(int position, float[] fragment)
        {
            int stream = position % fragmentCount;
            data[stream].index = position;
            data[stream].data = fragment;
        }

        public float[] Read(int position)
        {
            int stream = position % fragmentCount;
            return data[stream].index != position ? empty : data[stream].data;
        }

        private class Fragment
        {
            public float[] data;
            public int index = -1;
        }
    }
}