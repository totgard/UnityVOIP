namespace UnityVOIP
{
    public interface IVoipCodex
    {
        int dataSize { get; }
        BandMode mode { get; }

        float[] Decode(byte[] data);
        byte[] Encode(float[] data);
    }
}