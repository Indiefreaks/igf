/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury.EffectEditor
{
    using System;
    using System.Collections.Generic;
    using ProjectMercury.PluginContracts;
    using ProjectMercury.Renderers;

    internal delegate void SerializeEventHandler              (object sender, SerializeEventArgs e);
    internal delegate void NewEmitterEventHandler             (object sender, NewEmitterEventArgs e);
    internal delegate void CloneEmitterEventHandler           (object sender, CloneEmitterEventArgs e);
    internal delegate void EmitterEventHandler                (object sender, EmitterEventArgs e);
    internal delegate void NewModifierEventHandler            (object sender, NewModifierEventArgs e);
    internal delegate void CloneModifierEventHandler          (object sender, CloneModifierEventArgs e);
    internal delegate void ModifierEventHandler               (object sender, ModifierEventArgs e);
    internal delegate void NewControllerEventHandler          (object sender, NewControllerEventArgs e);
    internal delegate void CloneControllerEventHandler        (object sender, CloneControllerEventArgs e);
    internal delegate void ControllerEventHandler             (object sender, ControllerEventArgs e);
    internal delegate void EmitterReinitialisedEventHandler   (object sender, EmitterReinitialisedEventArgs e);
    internal delegate void NewTextureReferenceEventHandler    (object sender, NewTextureReferenceEventArgs e);
    internal delegate void TextureReferenceChangedEventHandler(object sender, TextureReferenceChangedEventArgs e);

    internal interface IInterfaceProvider : IDisposable
    {
        event EventHandler                          Ready;
        event SerializeEventHandler                 Serialize;
        event SerializeEventHandler                 Deserialize;
        event NewEmitterEventHandler                EmitterAdded;
        event CloneEmitterEventHandler              EmitterCloned;
        event EmitterEventHandler                   EmitterRemoved;
        event NewModifierEventHandler               ModifierAdded;
        event CloneModifierEventHandler             ModifierCloned;
        event ModifierEventHandler                  ModifierRemoved;
        event NewControllerEventHandler             ControllerAdded;
        event CloneControllerEventHandler           ControllerCloned;
        event ControllerEventHandler                ControllerRemoved;
        event EmitterReinitialisedEventHandler      EmitterReinitialised;
        event NewTextureReferenceEventHandler       TextureReferenceAdded;
        event TextureReferenceChangedEventHandler   TextureReferenceChanged;
        event EventHandler                          NewParticleEffect;

        bool TriggerRequired(out float x, out float y);
        void Draw(ParticleEffect effect, AbstractRenderer renderer);

        void SetEmitterPlugins(IEnumerable<IEmitterPlugin> plugins);
        void SetModifierPlugins(IEnumerable<IModifierPlugin> plugins);
        void SetControllerPlugins(IEnumerable<IControllerPlugin> plugins);
        void SetSerializationPlugins(IEnumerable<ISerializerPlugin> plugins);
        void SetParticleEffect(ParticleEffect effect);
        void SetUpdateTime(float totalSeconds);

        IEnumerable<TextureReference> TextureReferences { get; set; }
    }
}