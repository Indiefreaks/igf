using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace Indiefreaks.Xna.Input
{
#if WINDOWS_PHONE
    public sealed class VirtualGamePadSkin
    {
        public VirtualGamePadSkin()
        {
            LeftStick = new VirtualGamePadStickSkin();
            RightStick = new VirtualGamePadStickSkin();
            A = new VirtualGamePadButtonSkin();
            B = new VirtualGamePadButtonSkin();
            X = new VirtualGamePadButtonSkin();
            Y = new VirtualGamePadButtonSkin();
            DpadLeft = new VirtualGamePadButtonSkin();
            DpadRight = new VirtualGamePadButtonSkin();
            DpadUp = new VirtualGamePadButtonSkin();
            DpadDown = new VirtualGamePadButtonSkin();
            LeftShoulder = new VirtualGamePadButtonSkin();
            RightShoulder = new VirtualGamePadButtonSkin();
            Start = new VirtualGamePadButtonSkin();
            LeftTrigger = new VirtualGamePadButtonSkin();
            RightTrigger = new VirtualGamePadButtonSkin();
        }

        public VirtualGamePadStickSkin LeftStick { get; private set; }
        public VirtualGamePadStickSkin RightStick { get; private set; }
        public VirtualGamePadButtonSkin A { get; private set; }
        public VirtualGamePadButtonSkin B { get; private set; }
        public VirtualGamePadButtonSkin X { get; private set; }
        public VirtualGamePadButtonSkin Y { get; private set; }
        public VirtualGamePadButtonSkin DpadLeft { get; private set; }
        public VirtualGamePadButtonSkin DpadRight { get; private set; }
        public VirtualGamePadButtonSkin DpadUp { get; private set; }
        public VirtualGamePadButtonSkin DpadDown { get; private set; }
        public VirtualGamePadButtonSkin LeftShoulder { get; private set; }
        public VirtualGamePadButtonSkin RightShoulder { get; private set; }
        public VirtualGamePadButtonSkin Start { get; private set; }
        public VirtualGamePadButtonSkin LeftTrigger { get; private set; }
        public VirtualGamePadButtonSkin RightTrigger { get; private set; }
    }

    public abstract class VirtualGamePadControlSkin
    {
        protected VirtualGamePadControlSkin()
        {
            SensibleVisibility = true;
            _opacity = 0f;
            _minOpacity = 0f;
            _maxOpacity = 1f;
        }

        private float _opacity;
        internal float Opacity
        {
            get { return _opacity; }
            set { _opacity = MathHelper.Clamp(value, MinOpacity, MaxOpacity); }
        }

        private float _minOpacity;
        public float MinOpacity
        {
            get { return _minOpacity; }
            set
            {
                if(value > MaxOpacity)
                    throw new ArgumentException("Cannot set a Minimal opacity bigger than MaxOpacity", "MinOpacity");

                _minOpacity = MathHelper.Clamp(value, 0f, 1f);
            }
        }

        private float _maxOpacity;
        public float MaxOpacity
        {
            get { return _maxOpacity; }
            set
            {
                if(value < MinOpacity)
                    throw new ArgumentException("Cannot set a Maximal opacity smaller than MinOpacity", "MaxOpacity");

                _maxOpacity = MathHelper.Clamp(value, 0f, 1f);
            }
        }

        public bool SensibleVisibility { get; set; }
    }

    public sealed class VirtualGamePadButtonSkin : VirtualGamePadControlSkin
    {
        public Texture2D NormalTexture { get; set; }
        public Texture2D PressedTexture { get; set; }
    }

    public sealed class VirtualGamePadStickSkin : VirtualGamePadControlSkin
    {
        public VirtualGamePadStickSkin()
        {
            MaxThumbStickDistance = 60f;
        }

        private float _maxThumbStickDistance;
        public float MaxThumbStickDistance
        {
            get { return _maxThumbStickDistance; }
            set { _maxThumbStickDistance = MathHelper.Clamp(value, 10f, MathHelper.Max(TouchPanel.DisplayWidth, TouchPanel.DisplayHeight)); }
        }

        public Texture2D AreaTexture { get; set; }
        public Texture2D ThumbStickTexture { get; set; }
    }
#endif
}