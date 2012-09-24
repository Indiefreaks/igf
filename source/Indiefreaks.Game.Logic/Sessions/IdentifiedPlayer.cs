using Indiefreaks.Xna.Core;
using Indiefreaks.Xna.Input;
using Indiefreaks.Xna.Logic;
using Indiefreaks.Xna.Storage;

namespace Indiefreaks.Xna.Sessions
{
    /// <summary>
    /// Represents a Local player that is identified in the game session
    /// </summary>
    public abstract class IdentifiedPlayer
    {
        /// <summary>
        /// Creates a new local player instance
        /// </summary>
        /// <param name="playerInput">The PlayerInput instance used by the Player</param>
        protected IdentifiedPlayer(PlayerInput playerInput)
        {
            Input = playerInput;
            LogicalPlayerIndex = LogicalPlayerIndex.None;
        }

        /// <summary>
        /// Creates a new remote player instance
        /// </summary>
        /// <param name="uniqueId">The id of the remote player</param>
        protected IdentifiedPlayer(string uniqueId)
        {
            UniqueId = uniqueId;
            LogicalPlayerIndex = LogicalPlayerIndex.None;
        }

        public IAsyncSaveDevice Storage { get; private set; }

        /// <summary>
        /// Returns the unique player id
        /// </summary>
        public string UniqueId { get; protected set; }

        /// <summary>
        /// Gets or sets the name display in the game
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        ///   Returns the LogicalPlayerIndex for this instance
        /// </summary>
        /// <remarks>
        ///   <para>PlayerIndex differs from LogicalPlayerIndex because it depends on when an input device is connected/disconnected.</para>
        /// </remarks>
        public LogicalPlayerIndex LogicalPlayerIndex { get; internal set; }

        /// <summary>
        /// Returns the current identified player input
        /// </summary>
        /// <remarks>Returns null if player is remote</remarks>
        public PlayerInput Input { get; private set; }

        /// <summary>
        /// Returns if the current player is local or remote
        /// </summary>
        public bool IsLocal
        {
            get { return Input != null; }
        }

        /// <summary>
        /// Returns if the current player is the Host of the session
        /// </summary>
        public bool IsHost { get; protected set; }

        public void PreparePlayerDevice()
        {
            if(!IsLocal)
                throw new CoreException("This player isn't local");

            Storage = Application.Storage.PreparePlayerDevice(Input.PlayerIndex);
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:Indiefreaks.Xna.Sessions.IdentifiedPlayer"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:Indiefreaks.Xna.Sessions.IdentifiedPlayer"/>; otherwise, false.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (obj != null && obj is IdentifiedPlayer)
                return UniqueId.Equals(((IdentifiedPlayer)obj).UniqueId);
            else
                return false;
        }

        public override int GetHashCode()
        {
            return UniqueId.GetHashCode();
        }

        public static bool operator == (IdentifiedPlayer player1, IdentifiedPlayer player2)
        {
            if ((object)player1 == null)
                return false;
            if ((object)player2 == null)
                return false;
            
            return player1.Equals(player2);
        }

        public static bool operator != (IdentifiedPlayer player1, IdentifiedPlayer player2)
        {
            if ((object)player1 == null)
                return false;
            else
                return !player1.Equals(player2);
        }
    }
}