namespace Indiefreaks.Xna.Input
{
    public sealed class TouchMapping
    {
        private TouchMap _a, _b;
        private TouchMap _back;
        private TouchMap _dpadB;
        private TouchMap _dpadF;
        private TouchMap _dpadL, _dpadR, _leftClick;
        private TouchMap _leftTrigger;
        private TouchMap _leftStick;
        private TouchMap _rightClick;
        private TouchMap _rightTrigger;
        private TouchMap _rightStick;
        private TouchMap _shoulderL, _shoulderR;
        private TouchMap _start;
        private TouchMap _x, _y;

        public TouchMapping()
        {
            _leftStick = new TouchMap();
            _rightStick = new TouchMap();
            _leftTrigger = new TouchMap();
            _rightTrigger = new TouchMap();
            _a = new TouchMap();
            _b = new TouchMap();
            _x = new TouchMap();
            _y = new TouchMap();
            _back = new TouchMap();
            _start = new TouchMap();
            _dpadF = new TouchMap();
            _dpadB = new TouchMap();
            _dpadL = new TouchMap();
            _dpadR = new TouchMap();
            _leftClick = new TouchMap();
            _rightClick = new TouchMap();
            _shoulderL = new TouchMap();
            _shoulderR = new TouchMap();
        }

        public TouchMap Y
        {
            get { return _y; }
            set { _y = value; }
        }

        public TouchMap X
        {
            get { return _x; }
            set { _x = value; }
        }

        public TouchMap Start
        {
            get { return _start; }
            set { _start = value; }
        }

        public TouchMap RightShoulder
        {
            get { return _shoulderR; }
            set { _shoulderR = value; }
        }

        public TouchMap LeftShoulder
        {
            get { return _shoulderL; }
            set { _shoulderL = value; }
        }

        public TouchMap RightStick
        {
            get { return _rightStick; }
            set { _rightStick = value; }
        }

        public TouchMap RightTrigger
        {
            get { return _rightTrigger; }
            set { _rightTrigger = value; }
        }

        public TouchMap RightStickClick
        {
            get { return _rightClick; }
            set { _rightClick = value; }
        }

        public TouchMap LeftStick
        {
            get { return _leftStick; }
            set { _leftStick = value; }
        }

        public TouchMap LeftTrigger
        {
            get { return _leftTrigger; }
            set { _leftTrigger = value; }
        }

        public TouchMap LeftStickClick
        {
            get { return _leftClick; }
            set { _leftClick = value; }
        }

        public TouchMap DpadRight
        {
            get { return _dpadR; }
            set { _dpadR = value; }
        }

        public TouchMap DpadLeft
        {
            get { return _dpadL; }
            set { _dpadL = value; }
        }

        public TouchMap DpadUp
        {
            get { return _dpadF; }
            set { _dpadF = value; }
        }

        public TouchMap DpadDown
        {
            get { return _dpadB; }
            set { _dpadB = value; }
        }

        public TouchMap Back
        {
            get { return _back; }
            set { _back = value; }
        }

        public TouchMap B
        {
            get { return _b; }
            set { _b = value; }
        }

        public TouchMap A
        {
            get { return _a; }
            set { _a = value; }
        }
    }
}