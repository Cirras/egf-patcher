using System;
using System.Runtime.InteropServices;

namespace EGF_Patcher
{
    internal static class NativeMethods
    {
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr BeginUpdateResource(
            string pFileName,
            [MarshalAs(UnmanagedType.Bool)]bool bDeleteExistingResources
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool UpdateResource(
            IntPtr hUpdate,
            IntPtr lpType,
            IntPtr lpName,
            ushort wLanguage,
            IntPtr lpData,
            uint cbData
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool EndUpdateResource(IntPtr hUpdate, bool fDiscard);
    }
}
