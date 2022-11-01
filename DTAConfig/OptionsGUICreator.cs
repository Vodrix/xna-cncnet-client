using ClientGUI;
using DTAConfig.Settings;
<<<<<<< HEAD
=======
using DTAConfig.CustomSettings;
>>>>>>> e76474081c28fa7e61dbab5dff28b8aba5d63d1b

namespace DTAConfig
{
    /// <summary>
    /// A GUI creator that also includes DTAConfig's custom controls in addition
    /// to the controls of ClientGUI and Rampastring.XNAUI.
    /// </summary>
    internal class OptionsGUICreator : ClientGUICreator
    {
        public OptionsGUICreator()
        {
            AddControl(typeof(SettingCheckBox));
            AddControl(typeof(SettingDropDown));
            AddControl(typeof(FileSettingCheckBox));
            AddControl(typeof(FileSettingDropDown));
        }
    }
}
