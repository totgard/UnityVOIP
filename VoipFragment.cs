namespace UnityVOIP
{
    public struct VoipFragment
    {
        public readonly byte[] data;
        public readonly int index;
        public readonly BandMode mode;

        public VoipFragment(int index, byte[] data, BandMode mode)
        {
            this.index = index;
            this.data = data;
            this.mode = mode;
        }
    }
}