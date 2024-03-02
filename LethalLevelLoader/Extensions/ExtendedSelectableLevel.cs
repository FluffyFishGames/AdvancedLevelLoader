using GameNetcodeStuff;
using LethalLevelLoader;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ContentType { Vanilla, LethalLevelLoader, LethalExpansion, Any } //Any & All included for built in checks.

namespace AdvancedLevelLoader
{
    [CreateAssetMenu(menuName = "LethalLevelLoader/ExtendedLevel")]
    public class ExtendedSelectableLevel : ScriptableObject
    {
        public SelectableLevel SelectableLevel;
        private ExtendedLevel _LethalLevelLoaderLevel;
        public List<StoryLogData> StoryLogs = new List<StoryLogData>();
        /// <summary>
        /// Will provide a lethal level loader compatible ExtendedLevel object for this level.
        /// </summary>
        public ExtendedLevel LethalLevelLoaderLevel
        {
            get
            {
                if (_LethalLevelLoaderLevel == null)
                {
                    _LethalLevelLoaderLevel = new ExtendedLevel();
                    _LethalLevelLoaderLevel.selectableLevel = SelectableLevel;
                    _LethalLevelLoaderLevel.levelEvents = LevelEvents;
                    _LethalLevelLoaderLevel.isLocked = false;
                    _LethalLevelLoaderLevel.isHidden = false;
                    _LethalLevelLoaderLevel.routeNode = RouteNode;
                    _LethalLevelLoaderLevel.routeConfirmNode = RouteConfirmNode;
                    _LethalLevelLoaderLevel.infoNode = InfoNode;
                    _LethalLevelLoaderLevel.levelTags = LevelTags;
                    _LethalLevelLoaderLevel.levelType = ContentType.Custom;
                    _LethalLevelLoaderLevel.storyLogs = StoryLogs;
                    _LethalLevelLoaderLevel.infoNodeDescripton = InfoNodeDescription;
                    _LethalLevelLoaderLevel.allowedDungeonContentTypes = ContentType.Any;
                }
                return _LethalLevelLoaderLevel;
            }
            set
            {
                _LethalLevelLoaderLevel = value;
            }
        }
        public List<string> LevelTags = new List<string>();

        public ContentType LevelType;
        public string NumberlessPlanetName => GetNumberlessPlanetName(SelectableLevel);
        public TerminalNode RouteNode;
        public TerminalNode RouteConfirmNode;
        public TerminalNode InfoNode;
        public string InfoNodeDescription;
        public LevelEvents LevelEvents = new LevelEvents();

        public bool IsLoaded => SceneManager.GetSceneByName(SelectableLevel.sceneName).isLoaded;

        internal static Dictionary<SelectableLevel, ExtendedSelectableLevel> AllLevels = new Dictionary<SelectableLevel, ExtendedSelectableLevel>();

        internal static ExtendedSelectableLevel GetOrCreate(ExtendedLevel level)
        {
            if (!AllLevels.ContainsKey(level.selectableLevel))
            {
                var extendedLevel = ScriptableObject.CreateInstance<ExtendedSelectableLevel>();
                extendedLevel.LevelType = ContentType.LethalLevelLoader;
                extendedLevel.SelectableLevel = level.selectableLevel;
                extendedLevel.InfoNodeDescription = level.infoNodeDescripton;
                extendedLevel.LevelEvents = level.levelEvents;
                extendedLevel.StoryLogs = level.storyLogs;
                extendedLevel.Initialize();
                AllLevels[level.selectableLevel] = extendedLevel;
            }
            return AllLevels[level.selectableLevel];
        }

        internal static ExtendedSelectableLevel GetOrCreate(SelectableLevel level, ContentType newContentType = ContentType.Vanilla)
        {
            if (!AllLevels.ContainsKey(level))
            {
                var extendedLevel = ScriptableObject.CreateInstance<ExtendedSelectableLevel>();
                extendedLevel.LevelType = newContentType;
                extendedLevel.SelectableLevel = level;
                extendedLevel.Initialize();
                AllLevels[level] = extendedLevel;
            }
            return AllLevels[level];
        }

        internal void Initialize()
        {
            if (LevelType == ContentType.Vanilla && SelectableLevel.levelID > 8)
            {
                DebugHelper.LogWarning("Moon of another mod found: " + NumberlessPlanetName + ". Setting To LevelType: Other.");
                LevelType = ContentType.LethalExpansion;
                LevelTags.Clear();
            }

            if (LevelType != ContentType.Vanilla)
                LevelTags.Add("Custom");

            TerminalManager.GatherOrCreateLevelTerminalData(this);

            if (LevelType != ContentType.Vanilla)
            {
                name = NumberlessPlanetName.StripSpecialCharacters() + "ExtendedLevel";
                SelectableLevel.name = NumberlessPlanetName.StripSpecialCharacters() + "Level";
            }
        }

        internal static Regex NonLettersRegex = new Regex("[^a-zA-Z]");
        /// <summary>
        /// Returns the name of the planet without the number
        /// </summary>
        /// <param name="selectableLevel">The level to return the name for</param>
        /// <returns>Name without number</returns>
        internal static string GetNumberlessPlanetName(SelectableLevel selectableLevel)
        {
            if (selectableLevel != null)
                return NonLettersRegex.Replace(selectableLevel.PlanetName, "");
            else
                return string.Empty;
        }

        /// <summary>
        /// Changes the ID of the level and reflect that change to terminal keywords as well
        /// </summary>
        /// <param name="id">New ID for the level</param>
        internal void SetLevelID(int id)
        {
            SelectableLevel.levelID = id;
            if (RouteNode != null)
                RouteNode.displayPlanetInfo = SelectableLevel.levelID;
            if (RouteConfirmNode != null)
                RouteConfirmNode.buyRerouteToMoon = SelectableLevel.levelID;
        }
    }
}
