using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Indiefreaks.Xna.Core;

namespace Indiefreaks.Xna.Logic
{
    public class ReadonlyBehaviorCollection : ReadOnlyCollection<Behavior>
    {
        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1"/> qui est un wrapper en lecture seule autour de la liste spécifiée.
        /// </summary>
        /// <param name="list">Liste à encapsuler.</param><exception cref="T:System.ArgumentNullException"><paramref name="list"/> a la valeur null.</exception>
        public ReadonlyBehaviorCollection(IList<Behavior> list)
            : base(list)
        {
        }

        public Behavior GetBehavior(Guid id)
        {
            foreach (var behavior in Items)
            {
                if (behavior.GetType().GUID == id)
                    return behavior;
            }

            throw new CoreException();
        }

        public T GetBehavior<T>() where T : Behavior
        {
            foreach (var behavior in Items)
            {
                if (behavior is T)
                    return (T)behavior;
            }

            return default(T);
        }
    }
}