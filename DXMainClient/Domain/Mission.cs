using Rampastring.Tools;
using System;

namespace DTAClient.Domain
{
    /// <summary>
    /// A Tiberian Sun mission listed in Battle(E).ini.
    /// </summary>
    public class Mission
    {
        public Mission(IniFile iniFile, string sectionName)
        {
            Scenario = iniFile.GetStringValue(sectionName, nameof(Scenario), string.Empty);
            GUIName = iniFile.GetStringValue(sectionName, "Description", "Undefined mission");
            IconPath = iniFile.GetStringValue(sectionName, "SideName", string.Empty);
            GUIDescription = iniFile.GetStringValue(sectionName, "LongDescription", string.Empty);
            Enabled = iniFile.GetBooleanValue(sectionName, nameof(Enabled), true);
            BuildOffAlly = iniFile.GetBooleanValue(sectionName, nameof(BuildOffAlly), false);
            PlayerAlwaysOnNormalDifficulty = iniFile.GetBooleanValue(sectionName, nameof(PlayerAlwaysOnNormalDifficulty), false);
            GUIDescription = GUIDescription.Replace("@", Environment.NewLine);
        }

        public string Scenario { get; }
        public string GUIName { get; }
        public string IconPath { get; }
        public string GUIDescription { get; }
        public bool Enabled { get; }
        public bool BuildOffAlly { get; }
        public bool PlayerAlwaysOnNormalDifficulty { get; }
    }
}
