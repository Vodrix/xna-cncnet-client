using ClientGUI;
<<<<<<< Updated upstream
using DTAConfig.Settings;
=======
using DTAConfig.CustomSettings;
>>>>>>> Stashed changes

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
