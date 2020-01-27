using System;
[Serializable]
public class OurException : Exception
{
    private int special;
    public OurException() { }
    public OurException(string message) : base(message) { }
    public OurException(string message, Exception inner) : base(message, inner) { }

    //protected OurException(
    //  System.Runtime.Serialization.SerializationInfo info,
    //  System.Runtime.Serialization.StreamingContext context) : base("info", context) { }
    public OurException(string message, int spe) : base(message) { special = spe; }
}



