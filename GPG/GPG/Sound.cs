namespace GPG
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    public static class Sound
    {
        [DllImport("winmm.dll")]
        private static extern long mciSendString(string strCommand, StringBuilder strReturn, int iReturnLength, IntPtr hwndCallback);
        public static void Play(byte[] data)
        {
            PlaySound(data, IntPtr.Zero, SoundFlags.SND_MEMORY | SoundFlags.SND_ASYNC);
        }

        public static void Play(string file)
        {
            PlaySound(file, UIntPtr.Zero, 0x20001);
        }

        private static void PlayMp3(string file)
        {
        }

        [DllImport("winmm.dll", SetLastError=true)]
        private static extern bool PlaySound(byte[] soundData, IntPtr hmod, SoundFlags dwFlag);
        [DllImport("winmm.dll", SetLastError=true)]
        private static extern bool PlaySound(string soundFile, UIntPtr hmod, uint fdwSound);
        public static void PlaySynchronous(byte[] data)
        {
            PlaySound(data, IntPtr.Zero, SoundFlags.SND_MEMORY);
        }

        public static void PlaySynchronous(string file)
        {
            PlaySound(file, UIntPtr.Zero, 0x20000);
        }
    }
}

