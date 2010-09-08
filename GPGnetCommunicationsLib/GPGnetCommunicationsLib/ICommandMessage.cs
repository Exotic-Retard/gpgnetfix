namespace GPGnetCommunicationsLib
{
    using System;

    public interface ICommandMessage
    {
        byte[] FormatMessage();
        void UnformatMessage(byte[] data);

        Commands CommandName { get; set; }
    }
}

