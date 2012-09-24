using Microsoft.Xna.Framework.Input;

namespace Indiefreaks.Xna.Input
{
#if WINDOWS
    /// <summary>
    /// Stores a list of <see cref="Button"/> Key States
    /// </summary>
    public sealed class KeyState
    {
        private readonly Button[] _buttons;
        internal KeyState(Button[] buttons)
        {
            this._buttons = buttons;
        }

        /// <summary>
        /// Button Indexer (Keys)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Button this[Keys key]
        {
            get { return this._buttons[(int)key]; }
        }

        //don't worry, this was made with a regular expression - mostly :-)

        /// <summary>The 'A' button</summary>
        public Button A { get { return _buttons[0x41]; } }
        /// <summary>The 'Add' button</summary>
        public Button Add { get { return _buttons[0x6b]; } }
        /// <summary>The 'Apps' button</summary>
        public Button Apps { get { return _buttons[0x5d]; } }
        /// <summary>The 'Attn' button</summary>
        public Button Attn { get { return _buttons[0xf6]; } }
        /// <summary>The 'B' button</summary>
        public Button B { get { return _buttons[0x42]; } }
        /// <summary>The 'Back' button</summary>
        public Button Back { get { return _buttons[8]; } }
        /// <summary>The 'BrowserBack' button</summary>
        public Button BrowserBack { get { return _buttons[0xa6]; } }
        /// <summary>The 'BrowserFavorites' button</summary>
        public Button BrowserFavorites { get { return _buttons[0xab]; } }
        /// <summary>The 'BrowserForward' button</summary>
        public Button BrowserForward { get { return _buttons[0xa7]; } }
        /// <summary>The 'BrowserHome' button</summary>
        public Button BrowserHome { get { return _buttons[0xac]; } }
        /// <summary>The 'BrowserRefresh' button</summary>
        public Button BrowserRefresh { get { return _buttons[0xa8]; } }
        /// <summary>The 'BrowserSearch' button</summary>
        public Button BrowserSearch { get { return _buttons[170]; } }
        /// <summary>The 'BrowserStop' button</summary>
        public Button BrowserStop { get { return _buttons[0xa9]; } }
        /// <summary>The 'C' button</summary>
        public Button C { get { return _buttons[0x43]; } }
        /// <summary>The 'CapsLock' button</summary>
        public Button CapsLock { get { return _buttons[20]; } }
        /// <summary>The 'ChatPadGreen' button</summary>
        public Button ChatPadGreen { get { return _buttons[0xca]; } }
        /// <summary>The 'ChatPadOrange' button</summary>
        public Button ChatPadOrange { get { return _buttons[0xcb]; } }
        /// <summary>The 'Crsel' button</summary>
        public Button Crsel { get { return _buttons[0xf7]; } }
        /// <summary>The 'D' button</summary>
        public Button D { get { return _buttons[0x44]; } }
        /// <summary>The 'D0' button</summary>
        public Button D0 { get { return _buttons[0x30]; } }
        /// <summary>The 'D1' button</summary>
        public Button D1 { get { return _buttons[0x31]; } }
        /// <summary>The 'D2' button</summary>
        public Button D2 { get { return _buttons[50]; } }
        /// <summary>The 'D3' button</summary>
        public Button D3 { get { return _buttons[0x33]; } }
        /// <summary>The 'D4' button</summary>
        public Button D4 { get { return _buttons[0x34]; } }
        /// <summary>The 'D5' button</summary>
        public Button D5 { get { return _buttons[0x35]; } }
        /// <summary>The 'D6' button</summary>
        public Button D6 { get { return _buttons[0x36]; } }
        /// <summary>The 'D7' button</summary>
        public Button D7 { get { return _buttons[0x37]; } }
        /// <summary>The 'D8' button</summary>
        public Button D8 { get { return _buttons[0x38]; } }
        /// <summary>The 'D9' button</summary>
        public Button D9 { get { return _buttons[0x39]; } }
        /// <summary>The 'Decimal' button</summary>
        public Button Decimal { get { return _buttons[110]; } }
        /// <summary>The 'Delete' button</summary>
        public Button Delete { get { return _buttons[0x2e]; } }
        /// <summary>The 'Divide' button</summary>
        public Button Divide { get { return _buttons[0x6f]; } }
        /// <summary>The 'Down' button</summary>
        public Button Down { get { return _buttons[40]; } }
        /// <summary>The 'E' button</summary>
        public Button E { get { return _buttons[0x45]; } }
        /// <summary>The 'End' button</summary>
        public Button End { get { return _buttons[0x23]; } }
        /// <summary>The 'Enter' button</summary>
        public Button Enter { get { return _buttons[13]; } }
        /// <summary>The 'EraseEof' button</summary>
        public Button EraseEof { get { return _buttons[0xf9]; } }
        /// <summary>The 'Escape' button</summary>
        public Button Escape { get { return _buttons[0x1b]; } }
        /// <summary>The 'Execute' button</summary>
        public Button Execute { get { return _buttons[0x2b]; } }
        /// <summary>The 'Exsel' button</summary>
        public Button Exsel { get { return _buttons[0xf8]; } }
        /// <summary>The 'F' button</summary>
        public Button F { get { return _buttons[70]; } }
        /// <summary>The 'F1' button</summary>
        public Button F1 { get { return _buttons[0x70]; } }
        /// <summary>The 'F10' button</summary>
        public Button F10 { get { return _buttons[0x79]; } }
        /// <summary>The 'F11' button</summary>
        public Button F11 { get { return _buttons[0x7a]; } }
        /// <summary>The 'F12' button</summary>
        public Button F12 { get { return _buttons[0x7b]; } }
        /// <summary>The 'F13' button</summary>
        public Button F13 { get { return _buttons[0x7c]; } }
        /// <summary>The 'F14' button</summary>
        public Button F14 { get { return _buttons[0x7d]; } }
        /// <summary>The 'F15' button</summary>
        public Button F15 { get { return _buttons[0x7e]; } }
        /// <summary>The 'F16' button</summary>
        public Button F16 { get { return _buttons[0x7f]; } }
        /// <summary>The 'F17' button</summary>
        public Button F17 { get { return _buttons[0x80]; } }
        /// <summary>The 'F18' button</summary>
        public Button F18 { get { return _buttons[0x81]; } }
        /// <summary>The 'F19' button</summary>
        public Button F19 { get { return _buttons[130]; } }
        /// <summary>The 'F2' button</summary>
        public Button F2 { get { return _buttons[0x71]; } }
        /// <summary>The 'F20' button</summary>
        public Button F20 { get { return _buttons[0x83]; } }
        /// <summary>The 'F21' button</summary>
        public Button F21 { get { return _buttons[0x84]; } }
        /// <summary>The 'F22' button</summary>
        public Button F22 { get { return _buttons[0x85]; } }
        /// <summary>The 'F23' button</summary>
        public Button F23 { get { return _buttons[0x86]; } }
        /// <summary>The 'F24' button</summary>
        public Button F24 { get { return _buttons[0x87]; } }
        /// <summary>The 'F3' button</summary>
        public Button F3 { get { return _buttons[0x72]; } }
        /// <summary>The 'F4' button</summary>
        public Button F4 { get { return _buttons[0x73]; } }
        /// <summary>The 'F5' button</summary>
        public Button F5 { get { return _buttons[0x74]; } }
        /// <summary>The 'F6' button</summary>
        public Button F6 { get { return _buttons[0x75]; } }
        /// <summary>The 'F7' button</summary>
        public Button F7 { get { return _buttons[0x76]; } }
        /// <summary>The 'F8' button</summary>
        public Button F8 { get { return _buttons[0x77]; } }
        /// <summary>The 'F9' button</summary>
        public Button F9 { get { return _buttons[120]; } }
        /// <summary>The 'G' button</summary>
        public Button G { get { return _buttons[0x47]; } }
        /// <summary>The 'H' button</summary>
        public Button H { get { return _buttons[0x48]; } }
        /// <summary>The 'Help' button</summary>
        public Button Help { get { return _buttons[0x2f]; } }
        /// <summary>The 'Home' button</summary>
        public Button Home { get { return _buttons[0x24]; } }
        /// <summary>The 'I' button</summary>
        public Button I { get { return _buttons[0x49]; } }
        /// <summary>The 'Insert' button</summary>
        public Button Insert { get { return _buttons[0x2d]; } }
        /// <summary>The 'J' button</summary>
        public Button J { get { return _buttons[0x4a]; } }
        /// <summary>The 'K' button</summary>
        public Button K { get { return _buttons[0x4b]; } }
        /// <summary>The 'L' button</summary>
        public Button L { get { return _buttons[0x4c]; } }
        /// <summary>The 'LaunchApplication1' button</summary>
        public Button LaunchApplication1 { get { return _buttons[0xb6]; } }
        /// <summary>The 'LaunchApplication2' button</summary>
        public Button LaunchApplication2 { get { return _buttons[0xb7]; } }
        /// <summary>The 'LaunchMail' button</summary>
        public Button LaunchMail { get { return _buttons[180]; } }
        /// <summary>The 'Left' button</summary>
        public Button Left { get { return _buttons[0x25]; } }
        /// <summary>The 'LeftAlt' button</summary>
        public Button LeftAlt { get { return _buttons[0xa4]; } }
        /// <summary>The 'LeftControl' button</summary>
        public Button LeftControl { get { return _buttons[0xa2]; } }
        /// <summary>The 'LeftShift' button</summary>
        public Button LeftShift { get { return _buttons[160]; } }
        /// <summary>The 'LeftWindows' button</summary>
        public Button LeftWindows { get { return _buttons[0x5b]; } }
        /// <summary>The 'M' button</summary>
        public Button M { get { return _buttons[0x4d]; } }
        /// <summary>The 'MediaNextTrack' button</summary>
        public Button MediaNextTrack { get { return _buttons[0xb0]; } }
        /// <summary>The 'MediaPlayPause' button</summary>
        public Button MediaPlayPause { get { return _buttons[0xb3]; } }
        /// <summary>The 'MediaPreviousTrack' button</summary>
        public Button MediaPreviousTrack { get { return _buttons[0xb1]; } }
        /// <summary>The 'MediaStop' button</summary>
        public Button MediaStop { get { return _buttons[0xb2]; } }
        /// <summary>The 'Multiply' button</summary>
        public Button Multiply { get { return _buttons[0x6a]; } }
        /// <summary>The 'N' button</summary>
        public Button N { get { return _buttons[0x4e]; } }
        /// <summary>The 'NumLock' button</summary>
        public Button NumLock { get { return _buttons[0x90]; } }
        /// <summary>The 'NumPad0' button</summary>
        public Button NumPad0 { get { return _buttons[0x60]; } }
        /// <summary>The 'NumPad1' button</summary>
        public Button NumPad1 { get { return _buttons[0x61]; } }
        /// <summary>The 'NumPad2' button</summary>
        public Button NumPad2 { get { return _buttons[0x62]; } }
        /// <summary>The 'NumPad3' button</summary>
        public Button NumPad3 { get { return _buttons[0x63]; } }
        /// <summary>The 'NumPad4' button</summary>
        public Button NumPad4 { get { return _buttons[100]; } }
        /// <summary>The 'NumPad5' button</summary>
        public Button NumPad5 { get { return _buttons[0x65]; } }
        /// <summary>The 'NumPad6' button</summary>
        public Button NumPad6 { get { return _buttons[0x66]; } }
        /// <summary>The 'NumPad7' button</summary>
        public Button NumPad7 { get { return _buttons[0x67]; } }
        /// <summary>The 'NumPad8' button</summary>
        public Button NumPad8 { get { return _buttons[0x68]; } }
        /// <summary>The 'NumPad9' button</summary>
        public Button NumPad9 { get { return _buttons[0x69]; } }
        /// <summary>The 'O' button</summary>
        public Button O { get { return _buttons[0x4f]; } }
        /// <summary>The 'Oem8' button</summary>
        public Button Oem8 { get { return _buttons[0xdf]; } }
        /// <summary>The 'OemBackslash' button</summary>
        public Button OemBackslash { get { return _buttons[0xe2]; } }
        /// <summary>The 'OemClear' button</summary>
        public Button OemClear { get { return _buttons[0xfe]; } }
        /// <summary>The 'OemCloseBrackets' button</summary>
        public Button OemCloseBrackets { get { return _buttons[0xdd]; } }
        /// <summary>The 'OemComma' button</summary>
        public Button OemComma { get { return _buttons[0xbc]; } }
        /// <summary>The 'OemMinus' button</summary>
        public Button OemMinus { get { return _buttons[0xbd]; } }
        /// <summary>The 'OemOpenBrackets' button</summary>
        public Button OemOpenBrackets { get { return _buttons[0xdb]; } }
        /// <summary>The 'OemPeriod' button</summary>
        public Button OemPeriod { get { return _buttons[190]; } }
        /// <summary>The 'OemPipe' button</summary>
        public Button OemPipe { get { return _buttons[220]; } }
        /// <summary>The 'OemPlus' button</summary>
        public Button OemPlus { get { return _buttons[0xbb]; } }
        /// <summary>The 'OemQuestion' button</summary>
        public Button OemQuestion { get { return _buttons[0xbf]; } }
        /// <summary>The 'OemQuotes' button</summary>
        public Button OemQuotes { get { return _buttons[0xde]; } }
        /// <summary>The 'OemSemicolon' button</summary>
        public Button OemSemicolon { get { return _buttons[0xba]; } }
        /// <summary>The 'OemTilde' button</summary>
        public Button OemTilde { get { return _buttons[0xc0]; } }
        /// <summary>The 'P' button</summary>
        public Button P { get { return _buttons[80]; } }
        /// <summary>The 'Pa1' button</summary>
        public Button Pa1 { get { return _buttons[0xfd]; } }
        /// <summary>The 'PageDown' button</summary>
        public Button PageDown { get { return _buttons[0x22]; } }
        /// <summary>The 'PageUp' button</summary>
        public Button PageUp { get { return _buttons[0x21]; } }
        /// <summary>The 'Pause' button</summary>
        public Button Pause { get { return _buttons[0x13]; } }
        /// <summary>The 'Play' button</summary>
        public Button Play { get { return _buttons[250]; } }
        /// <summary>The 'Print' button</summary>
        public Button Print { get { return _buttons[0x2a]; } }
        /// <summary>The 'PrintScreen' button</summary>
        public Button PrintScreen { get { return _buttons[0x2c]; } }
        /// <summary>The 'ProcessKey' button</summary>
        public Button ProcessKey { get { return _buttons[0xe5]; } }
        /// <summary>The 'Q' button</summary>
        public Button Q { get { return _buttons[0x51]; } }
        /// <summary>The 'R' button</summary>
        public Button R { get { return _buttons[0x52]; } }
        /// <summary>The 'Right' button</summary>
        public Button Right { get { return _buttons[0x27]; } }
        /// <summary>The 'RightAlt' button</summary>
        public Button RightAlt { get { return _buttons[0xa5]; } }
        /// <summary>The 'RightControl' button</summary>
        public Button RightControl { get { return _buttons[0xa3]; } }
        /// <summary>The 'RightShift' button</summary>
        public Button RightShift { get { return _buttons[0xa1]; } }
        /// <summary>The 'RightWindows' button</summary>
        public Button RightWindows { get { return _buttons[0x5c]; } }
        /// <summary>The 'S' button</summary>
        public Button S { get { return _buttons[0x53]; } }
        /// <summary>The 'Scroll' button</summary>
        public Button Scroll { get { return _buttons[0x91]; } }
        /// <summary>The 'Select' button</summary>
        public Button Select { get { return _buttons[0x29]; } }
        /// <summary>The 'SelectMedia' button</summary>
        public Button SelectMedia { get { return _buttons[0xb5]; } }
        /// <summary>The 'Separator' button</summary>
        public Button Separator { get { return _buttons[0x6c]; } }
        /// <summary>The 'Sleep' button</summary>
        public Button Sleep { get { return _buttons[0x5f]; } }
        /// <summary>The 'Space' button</summary>
        public Button Space { get { return _buttons[0x20]; } }
        /// <summary>The 'Subtract' button</summary>
        public Button Subtract { get { return _buttons[0x6d]; } }
        /// <summary>The 'T' button</summary>
        public Button T { get { return _buttons[0x54]; } }
        /// <summary>The 'Tab' button</summary>
        public Button Tab { get { return _buttons[9]; } }
        /// <summary>The 'U' button</summary>
        public Button U { get { return _buttons[0x55]; } }
        /// <summary>The 'Up' button</summary>
        public Button Up { get { return _buttons[0x26]; } }
        /// <summary>The 'V' button</summary>
        public Button V { get { return _buttons[0x56]; } }
        /// <summary>The 'VolumeDown' button</summary>
        public Button VolumeDown { get { return _buttons[0xae]; } }
        /// <summary>The 'VolumeMute' button</summary>
        public Button VolumeMute { get { return _buttons[0xad]; } }
        /// <summary>The 'VolumeUp' button</summary>
        public Button VolumeUp { get { return _buttons[0xaf]; } }
        /// <summary>The 'W' button</summary>
        public Button W { get { return _buttons[0x57]; } }
        /// <summary>The 'X' button</summary>
        public Button X { get { return _buttons[0x58]; } }
        /// <summary>The 'Y' button</summary>
        public Button Y { get { return _buttons[0x59]; } }
        /// <summary>The 'Z' button</summary>
        public Button Z { get { return _buttons[90]; } }
        /// <summary>The 'Zoom' button</summary>
        public Button Zoom { get { return _buttons[0xfb]; } }
    }
#endif
}