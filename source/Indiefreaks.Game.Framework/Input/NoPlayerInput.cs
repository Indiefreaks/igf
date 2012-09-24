using Indiefreaks.Xna.Core;
using Microsoft.Xna.Framework;

namespace Indiefreaks.Xna.Input
{
    /// <summary>
    ///   Stub Class used when no Input device is connected
    /// </summary>
    /// <remarks>
    ///   This avoids getting exceptions when trying to get a PlayerInput instance that isn't connected
    /// </remarks>
    public class NoPlayerInput : PlayerInput
    {
        public NoPlayerInput(PlayerIndex playerIndex) : base(playerIndex)
        {
        }

#if WINDOWS
        public override bool UseKeyboardMouseInput
        {
            get { return base.UseKeyboardMouseInput; }
            set
            {
                PlayerInput player;
                switch (PlayerIndex)
                {
                    default:
                    case PlayerIndex.One:
                        player = Application.Input.Player1;
                        break;
                    case PlayerIndex.Two:
                        player = Application.Input.Player2;
                        break;
                    case PlayerIndex.Three:
                        player = Application.Input.Player3;
                        break;
                    case PlayerIndex.Four:
                        player = Application.Input.Player4;
                        break;
                }

                player.UseKeyboardMouseInput = value;
                player.ControlInput = ControlInput.KeyboardMouse;
                player.CheckConnectionStatus();
            }
        }
#endif
    }
}