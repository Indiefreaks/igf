using System.Collections.Generic;
using System.Globalization;

namespace Indiefreaks.Xna.Storage
{
    public static class Strings
    {
        private enum Message
        {
            Ok,
            YesSelectNewDevice,
            NoContinueWithoutDevice,
            ReselectStorageDevice,
            StorageDeviceRequired,
            ForceDisconnectedReselectionMessage,
            PromptForDisconnectedMessage,
            ForceCanceledReselectionMessage,
            PromptForCancelledMessage,
            StorageDeviceIsNotValid,
            NeedGamerService
        };

        private static readonly Dictionary<CultureInfo, Dictionary<Message, string>> CulturedStrings;
 
        static Strings()
        {
            CulturedStrings = new Dictionary<CultureInfo, Dictionary<Message, string>>
                                  {
                                      {
                                          new CultureInfo("en"), new Dictionary<Message, string>
                                                                        {
                                                                            {Message.Ok, "Ok"},
                                                                            {Message.YesSelectNewDevice, "Yes. Select new device."},
                                                                            {Message.NoContinueWithoutDevice, "No. Continue without device."},
                                                                            {Message.ReselectStorageDevice, "Reselect Storage Device?"},
                                                                            {Message.StorageDeviceRequired, "Storage Device Required"},
                                                                            {Message.ForceDisconnectedReselectionMessage, "The storage device was disconnected. A storage device is required to continue."},
                                                                            {Message.PromptForDisconnectedMessage, "The storage device was disconnected. You can continue without a device, but you will not be able to save. Would you like to select a storage device?"},
                                                                            {Message.ForceCanceledReselectionMessage, "No storage device was selected. A storage device is required to continue."},
                                                                            {Message.PromptForCancelledMessage, "No storage device was selected. You can continue without a device, but you will not be able to save. Would you like to select a storage device?"},
                                                                            {Message.StorageDeviceIsNotValid, "StorageDevice is not valid."},
                                                                            {Message.NeedGamerService, "SaveDevice requries gamer services to operate. Add the GamerServicesComponent to your game."}
                                                                        }
                                          },
                                      {
                                          new CultureInfo("fr"), new Dictionary<Message, string>
                                                                        {
                                                                            {Message.Ok, "Ok"},
                                                                            {Message.YesSelectNewDevice, "Oui. Sélectionner un nouveau périphérique."},
                                                                            {Message.NoContinueWithoutDevice, "Non. Continuer sans le périphérique."},
                                                                            {Message.ReselectStorageDevice, "Sélectionner un nouveau périphérique de stockage?"},
                                                                            {Message.StorageDeviceRequired, "Périphérique de stockage requis."},
                                                                            {Message.ForceDisconnectedReselectionMessage, "Le périphérique de stockage a été déconnecté. Un périphérique de stockage est nécessaire pour continuer."},
                                                                            {Message.PromptForDisconnectedMessage, "Le périphérique de stockage a été déconnecté. Vous pouvez continuer sans périphérique, mais vous ne pourrez pas sauvegarder. Voulez vous sélectionner un périphérique de stockage?"},
                                                                            {Message.ForceCanceledReselectionMessage, "Aucun périphérique de stockage sélectionné. Un périphérique de stockage est nécessaire pour continuer."},
                                                                            {Message.PromptForCancelledMessage, "Aucun périphérique de stockage sélectionné. Vous pouvez continuer sans périphérique, mais vous ne pourrez pas sauvegarder. Voulez vous sélectionner un périphérique de stockage?"},
                                                                            {Message.StorageDeviceIsNotValid, "Le périphérique de stockage n'est pas valide."},
                                                                            {Message.NeedGamerService, "SSaveDevice nécessite le service Gamer pour fonctionner. Ajoutez GamerServicesComponent à votre jeu."},
                                                                        }
                                          },
                                      {
                                          new CultureInfo("es"), new Dictionary<Message, string>
                                                                        {
                                                                            {Message.Ok, "Ok"},
                                                                            {Message.YesSelectNewDevice, "Sí. Elegir nuevo dispositivo."},
                                                                            {Message.NoContinueWithoutDevice, "No. Continuar sin un dispositivo."},
                                                                            {Message.ReselectStorageDevice, "Volver a elegir Dispositivo de Almacenamiento?"},
                                                                            {Message.StorageDeviceRequired, "Dispositivo de Almacenamiento Requerido"},
                                                                            {Message.ForceDisconnectedReselectionMessage, "El dispositivo de almacenamiento se ha desconectado. Se requiere un dispositivo de almacenamiento para continuar."},
                                                                            {Message.PromptForDisconnectedMessage, "El dispositivo de almacenamiento se ha desconectado. Puedes continuar sin un dispositivo, pero no podrás grabar. ¿Deseas elegir un dispositivo de almacenamiento?"},
                                                                            {Message.ForceCanceledReselectionMessage, "No se ha seleccionado el dispositivo de almacenamiento. Se requiere un dispositivo de almacenamiento para continuar."},
                                                                            {Message.PromptForCancelledMessage, "No se ha seleccionado el dispositivo de almacenamiento. Puedes continuar sin un dispositivo, pero no podrás grabar. ¿Deseas elegir un dispositivo de almacenamiento?"},
                                                                            {Message.StorageDeviceIsNotValid, "El dispositivo de almacenamiento no es válido."},
                                                                            {Message.NeedGamerService, "El dispositivo de almacenamiento requiere los servicios de jugador para operar. Añade el componente GamerServicesComponent a tu juego."},
                                                                        }
                                          },
                                      {
                                          new CultureInfo("de"), new Dictionary<Message, string>
                                                                        {
                                                                            {Message.Ok, "Ok"},
                                                                            {Message.YesSelectNewDevice, "Ja, neues Speichergerät auswählen."},
                                                                            {Message.NoContinueWithoutDevice, "Nein, ohne Speichergerät fortsetzen."},
                                                                            {Message.ReselectStorageDevice, "Neues Speichergerät auswählen?"},
                                                                            {Message.StorageDeviceRequired, "Ein Speichergerät wird benötigt"},
                                                                            {Message.ForceDisconnectedReselectionMessage, "Das Speichergerät wurde entfernt. Ein Speichergerät wird benötigt um fortzusetzen."},
                                                                            {Message.PromptForDisconnectedMessage, "Das Speichergerät wurde entfernt. Du kannst ohne Speichergerät fortfahren, kannst dann aber nicht speichern. Willst du ein Speichergerät auswählen?"},
                                                                            {Message.ForceCanceledReselectionMessage, "Es wurde kein Speichergerät ausgewählt. Ein Speichergerät wird benötigt um fortzusetzen."},
                                                                            {Message.PromptForCancelledMessage, "Es wurde kein Speichergerät ausgewählt. Du kannst ohne Speichergerät fortfahren, kannst dann aber nicht speichern. Willst du ein Speichergerät auswählen?"},
                                                                            {Message.StorageDeviceIsNotValid, "Ungültiges Speichergerät."},
                                                                            {Message.NeedGamerService, "SaveDevice requries gamer services to operate. Add the GamerServicesComponent to your game."},
                                                                        }
                                          },
                                      {
                                          new CultureInfo("it"), new Dictionary<Message, string>
                                                                        {
                                                                            {Message.Ok, "Ok"},
                                                                            {Message.YesSelectNewDevice, "Sì, seleziona un nuovo supporto."},
                                                                            {Message.NoContinueWithoutDevice, "No, continua sensa supporto."},
                                                                            {Message.ReselectStorageDevice, "Riselezionare il supporto di memorizzazione?"},
                                                                            {Message.StorageDeviceRequired, "Il Supporto di Memorizzazione è necessario"},
                                                                            {Message.ForceDisconnectedReselectionMessage, "Il supporto di memorizzazione è stato scollegato. È necessario un supporto per continuare."},
                                                                            {Message.PromptForDisconnectedMessage, "Il supporto di memorizzazione è stato scollegato. Puoi anche continuare senza, ma non sarà possibile salvere i progressi. Vuoi selezionare un supporto di memorizzazione?"},
                                                                            {Message.ForceCanceledReselectionMessage, "Nessuna supporto di memorizzazione selezionato. È necessario selezionare un supporto per continuare."},
                                                                            {Message.PromptForCancelledMessage, "Non è stato scelto alcun supporto di memorizzazione. Puoi anche continuare senza, ma non sarà possibile salvere i progressi. Vuoi selezionare un supporto di memorizzazione?"},
                                                                            {Message.StorageDeviceIsNotValid, "StorageDevice non valido."},
                                                                            {Message.NeedGamerService, "SaveDevice ha bisogno del GamerService per funzionare. Aggiungi il GamerServicesComponent al tuo gioco."},
                                                                        }
                                          },
                                      {
                                          new CultureInfo("ja"), new Dictionary<Message, string>
                                                                        {
                                                                            {Message.Ok, "Ok"},
                                                                            {Message.YesSelectNewDevice, "はい。新しいデバイスを選択します。"},
                                                                            {Message.NoContinueWithoutDevice, "いいえ。デバイスなしで続けます。"},
                                                                            {Message.ReselectStorageDevice, "ストレージ機器を再選択しますか？"},
                                                                            {Message.StorageDeviceRequired, "ストレージ機器が必要です"},
                                                                            {Message.ForceDisconnectedReselectionMessage, "ストレージ機器が切断されました。続けるためにはストレージ機器が必要です。"},
                                                                            {Message.PromptForDisconnectedMessage, "ストレージ機器が切断されました。デバイスなしで続けられますが、保存することはできません。ストレージ機器を選択しますか？"},
                                                                            {Message.ForceCanceledReselectionMessage, "ストレージ機器が選択されませんでした。続けるためにはストレージ機器が必要です。"},
                                                                            {Message.PromptForCancelledMessage, "デバイスが選択されませんでした。デバイスなしで続けられますが、保存することはできません。ストレージ機器を選択しますか？"},
                                                                            {Message.StorageDeviceIsNotValid, "ストレージ機器が有効ではありません。"},
                                                                            {Message.NeedGamerService, "SaveDevice を行うにはゲーマー サービスが必要です。ゲームに GamerServicesComponent を追加してください。"},
                                                                        }
                                          }
                                  };
        }
        

        public static CultureInfo Culture { get; set; }

        public static string Ok { get { return CulturedStrings[Culture][Message.Ok]; } }
        public static string YesSelectNewDevice { get { return CulturedStrings[Culture][Message.YesSelectNewDevice]; } }
        public static string NoContinueWithoutDevice { get { return CulturedStrings[Culture][Message.NoContinueWithoutDevice]; } }
        public static string ReselectStorageDevice { get { return CulturedStrings[Culture][Message.ReselectStorageDevice]; } }
        public static string StorageDeviceRequired { get { return CulturedStrings[Culture][Message.StorageDeviceRequired]; } }
        public static string ForceDisconnectedReselectionMessage { get { return CulturedStrings[Culture][Message.ForceDisconnectedReselectionMessage]; } }
        public static string PromptForDisconnectedMessage { get { return CulturedStrings[Culture][Message.PromptForDisconnectedMessage]; } }
        public static string ForceCanceledReselectionMessage { get { return CulturedStrings[Culture][Message.ForceCanceledReselectionMessage]; } }
        public static string PromptForCancelledMessage { get { return CulturedStrings[Culture][Message.PromptForCancelledMessage]; } }
        public static string StorageDeviceIsNotValid { get { return CulturedStrings[Culture][Message.StorageDeviceIsNotValid]; } }
        public static string NeedGamerService { get { return CulturedStrings[Culture][Message.NeedGamerService]; } }
    }
}