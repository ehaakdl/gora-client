using System;

public class NetworkBuffer
{
    private long CreatedAt;
    private byte[] Buffer;
    public NetworkBuffer(byte[] buffer)
    {
        Buffer = buffer;

        var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
        CreatedAt = (long)timeSpan.TotalSeconds;
    }

    public byte[] GetBuffer() { return Buffer; }
    public void SetBuffer(byte[] buffer) {  Buffer = buffer; }
}
