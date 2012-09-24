using System;

namespace Indiefreaks.Xna.Sessions
{
    /// <summary>
    /// Defines options for network packet transmission.
    /// </summary>
    [Flags]
    public enum DataTransferOptions
    {
        /// <summary>
        /// Sends the data with no guarantees. Packets of this type may be delivered in any order, with occasional packet loss. This is the most efficient option in terms of network bandwidth and machine resource usage. However, it is recommended only in situations where your game can recover from occasional packet loss.
        /// </summary>
        None,

        /// <summary>
        /// Sends the data with reliable delivery, but no special ordering. Packets of this type are resent until arrival at the destination. They may arrive out of order.
        /// </summary>
        Reliable,

        /// <summary>
        /// Sends the data with guaranteed ordering, but without reliable delivery. Occasionally, packets of this type are not delivered. However, any delivered packets always arrive in the order in which they are sent. Use this option in situations where the transmitted value changes constantly. Old versions never arrive after a more recent version.
        /// </summary>
        InOrder,

        /// <summary>
        /// Sends the data with reliability and arrival in the order originally sent. Packets of this type are resent until arrival and ordered internally. This means they arrive in the same order in which they were sent. In terms of network bandwidth usage, this is the strongest and most expensive option. Use this only when arrival and ordering are essential. Commonly, a game uses this option for a small percentage of packets. The majority of gameplay data is sent using None or Reliable.
        /// </summary>
        ReliableInOrder,

        /// <summary>
        /// Mark that this packet contains chat data, such as a player-to-player message string entered using the keyboard. To comply with international regulations, you must send such data without packet encryption. Therefore, you must use this flag to mark it. To maintain security, other game data should not use this flag. It is acceptable and efficient to mix encrypted and unencrypted data. If you send packets both with and without this flag within a single frame, both the encrypyted and unencrypted data streams will be merged into a single wire packet. This option can be combined with either or both of the Reliable and InOrder flags. When you request in-order delivery for chat packets, they will be ordered relative to other chat packets, but they may arrive out of order with respect to other non-chat data.
        /// </summary>
        Chat
    }
}