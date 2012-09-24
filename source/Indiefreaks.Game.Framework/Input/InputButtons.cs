namespace Indiefreaks.Xna.Input
{
    /// <summary>
    /// Structure storing the current state of a gamepad's buttons
    /// </summary>
    public struct InputButtons
    {
        internal Button AButton;
        internal Button BButton;
        internal Button BackButton;
        internal Button DpadDButton;
        internal Button DpadLButton;
        internal Button DpadRButton;
        internal Button DpadUButton;
        internal Button LeftStickClickButton;
        internal Button RightStickClickButton;
        internal Button ShoulderLButton;
        internal Button ShoulderRButton;
        internal Button StartButton;
        internal Button XButton;
        internal Button YButton;

        /// <summary></summary>
        public Button A
        {
            get { return AButton; }
        }

        /// <summary></summary>
        public Button B
        {
            get { return BButton; }
        }

        /// <summary></summary>
        public Button X
        {
            get { return XButton; }
        }

        /// <summary></summary>
        public Button Y
        {
            get { return YButton; }
        }

        /// <summary></summary>
        public Button Back
        {
            get { return BackButton; }
        }

        /// <summary></summary>
        public Button Start
        {
            get { return StartButton; }
        }

        /// <summary></summary>
        public Button LeftStickClick
        {
            get { return LeftStickClickButton; }
        }

        /// <summary></summary>
        public Button RightStickClick
        {
            get { return RightStickClickButton; }
        }

        /// <summary></summary>
        public Button DpadLeft
        {
            get { return DpadLButton; }
        }

        /// <summary></summary>
        public Button DpadRight
        {
            get { return DpadRButton; }
        }

        /// <summary></summary>
        public Button DpadUp
        {
            get { return DpadUButton; }
        }

        /// <summary></summary>
        public Button DpadDown
        {
            get { return DpadDButton; }
        }

        /// <summary></summary>
        public Button LeftShoulder
        {
            get { return ShoulderLButton; }
        }

        /// <summary></summary>
        public Button RightShoulder
        {
            get { return ShoulderRButton; }
        }
    }
}