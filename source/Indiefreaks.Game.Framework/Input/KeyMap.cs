using Microsoft.Xna.Framework.Input;

namespace Indiefreaks.Xna.Input
{
#if WINDOWS
    //for efficiency reasons, extract the private members from the KeyboardState structure
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)]
    struct KeyMap
    {
        [System.Runtime.InteropServices.FieldOffset(0)]
        public KeyboardState state;

        [System.Runtime.InteropServices.FieldOffset(0)]
        public uint currentState0;
        [System.Runtime.InteropServices.FieldOffset(4)]
        public uint currentState1;
        [System.Runtime.InteropServices.FieldOffset(8)]
        public uint currentState2;
        [System.Runtime.InteropServices.FieldOffset(12)]
        public uint currentState3;
        [System.Runtime.InteropServices.FieldOffset(16)]
        public uint currentState4;
        [System.Runtime.InteropServices.FieldOffset(20)]
        public uint currentState5;
        [System.Runtime.InteropServices.FieldOffset(24)]
        public uint currentState6;
        [System.Runtime.InteropServices.FieldOffset(28)]
        public uint currentState7;
    }
#endif
}