using System;
using System.Collections.Generic;
using Indiefreaks.Xna.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Indiefreaks.Xna.Input
{
    public class TouchMap
    {
        private const float MaxThumbstickDistance = 60f;
        private const float DeadZoneSize = 0.27f;
        private Vector2 _centerPosition;
        private Vector2 _currentPosition;

        public TouchMap()
        {
            Id = -1;
            TouchArea = new Rectangle(0, 0, 0, 0);
            Static = true;
        }

        public int Id { get; private set; }

        public Rectangle TouchArea { get; set; }

        public bool Static { get; set; }
        
        public Vector2 CenterPosition
        {
            get { return _centerPosition; }
        }

        public Vector2 CurrentPosition
        {
            get { return _currentPosition; }
        }

        public bool GetBooleanValue(List<TouchLocation> touches)
        {
            TouchLocation? touch = null;
            Update(touches, ref touch);

            bool result = false;

            if (!touch.HasValue)
                result = false;
            else if (touch.Value.State == TouchLocationState.Pressed)
                result = true;
            else if (touch.Value.State == TouchLocationState.Moved)
            {
                result = TouchArea.Contains(touch.Value.Position.ToPoint());
            }
            
            return result;
        }

        public Vector2 GetVector2Value(List<TouchLocation> touches, GamePadDeadZone gamePadDeadZone)
        {
            TouchLocation? touch = null;
            Update(touches, ref touch);

            Vector2 result = Vector2.Zero;

            if (!touch.HasValue)
            {
                if (Static)
                {
                    _centerPosition = new Vector2(TouchArea.X + TouchArea.Width/2f, TouchArea.Y + TouchArea.Height/2f);
                    _currentPosition = _centerPosition;
                }

                result = Vector2.Zero;
            }
            else if (touch.Value.State == TouchLocationState.Pressed)
            {
                if (Static)
                    _centerPosition = new Vector2(TouchArea.X + TouchArea.Width / 2f, TouchArea.Y + TouchArea.Height / 2f);
                else
                    _centerPosition = touch.Value.Position;

                _currentPosition = touch.Value.Position;

                result = Vector2.Zero;
            }
            else if (touch.Value.State == TouchLocationState.Moved)
            {
                _currentPosition = touch.Value.Position;

                var vector = (_currentPosition - _centerPosition) / MaxThumbstickDistance;

                if (vector.LengthSquared() > 1f)
                    vector.Normalize();

                vector.Y = -vector.Y;

                switch (gamePadDeadZone)
                {
                    case GamePadDeadZone.None:
                        {
                            result = vector;
                            break;
                        }
                    case GamePadDeadZone.Circular:
                        {
                            if (vector.LengthSquared() < DeadZoneSize * DeadZoneSize)
                                vector = Vector2.Zero;
                            
                            break;
                        }
                    default:
                    case GamePadDeadZone.IndependentAxes:
                        {
                            if (Math.Abs(vector.X) < DeadZoneSize)
                                vector.X = 0f;
                            if (Math.Abs(vector.Y) < DeadZoneSize)
                                vector.Y = 0f;

                            break;
                        }   
                }

                result = vector;
            }

            return result;
        }

        public float GetFloatValue(List<TouchLocation> touches)
        {
            TouchLocation? touch = null;
            Update(touches, ref touch);

            float result = 0f;

            if (!touch.HasValue)
                result = 0f;
            else if (touch.Value.State == TouchLocationState.Pressed || touch.Value.State == TouchLocationState.Moved)
                result = 1f;

            return result;
        }

        private void Update(List<TouchLocation> touches, ref TouchLocation? touch)
        {
            foreach (var touchLocation in touches)
            {
                if (touchLocation.Id == Id)
                {
                    touch = touchLocation;
                    break;
                }

                TouchLocation earliestTouchLocation;
                if (!touchLocation.TryGetPreviousLocation(out earliestTouchLocation))
                    earliestTouchLocation = touchLocation;

                if (Id == -1)
                {
                    if (TouchArea.Contains(earliestTouchLocation.Position.ToPoint()))
                    {
                        touch = earliestTouchLocation;
                        break;
                    }
                }
            }

            if (touch.HasValue)
            {
                Id = touch.Value.Id;
            }
            else
            {
                Id = -1;
            }
        }
    }
}