using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Interop;
using Captura.Properties;

namespace Captura
{
    static class HotKeyManager
    {
        public static readonly List<Hotkey> Hotkeys = new List<Hotkey>();
        
        public static void RegisterAll()
        {
            Hotkeys.Add(new Hotkey("Start/Stop Recording", (Modifiers) Settings.Default.RecordHotkeyMod, Settings.Default.RecordHotkey, () =>
            {
                var command = MainViewModel.Instance.RecordCommand;

                if (command.CanExecute(null))
                    command.Execute(null);
            }));
            
            Hotkeys.Add(new Hotkey("Pause/Resume Recording", (Modifiers) Settings.Default.PauseHotkeyMod, Settings.Default.PauseHotkey, () =>
            {
                var command = MainViewModel.Instance.PauseCommand;

                if (command.CanExecute(null))
                    command.Execute(null);
            }));
            
            Hotkeys.Add(new Hotkey("ScreenShot", (Modifiers) Settings.Default.ScreenShotHotkeyMod, Settings.Default.ScreenShotHotkey, () =>
            {
                var command = MainViewModel.Instance.ScreenShotCommand;

                if (command.CanExecute(null))
                    command.Execute(null);
            }));

            ComponentDispatcher.ThreadPreprocessMessage += ProcessMessage;
        }

        const int WmHotkey = 786;

        static void ProcessMessage(ref MSG Message, ref bool Handled)
        {
            if (Message.message == WmHotkey)
            {
                var id = Message.wParam.ToInt32();

                foreach (var hotkey in Hotkeys)
                {
                    if (hotkey.ID == id)
                    {
                        hotkey.Work();
                        break;
                    }
                }
            }
        }
        
        public static void Dispose()
        {
            foreach (var hotkey in Hotkeys)
                hotkey.Unregister();

            Settings.Default.RecordHotkey = Hotkeys[0].Key;
            Settings.Default.RecordHotkeyMod = (int) Hotkeys[0].Modifiers;

            Settings.Default.PauseHotkey = Hotkeys[1].Key;
            Settings.Default.PauseHotkeyMod = (int) Hotkeys[1].Modifiers;

            Settings.Default.ScreenShotHotkey = Hotkeys[2].Key;
            Settings.Default.ScreenShotHotkeyMod = (int) Hotkeys[2].Modifiers;
        }
    }
}