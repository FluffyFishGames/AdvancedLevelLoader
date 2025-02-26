﻿using GameNetcodeStuff;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ContentType { Vanilla, Custom, Any } //Any & All included for built in checks.

namespace LethalLevelLoader
{
    [CreateAssetMenu(menuName = "LethalLevelLoader/ExtendedLevel")]
    public class ExtendedLevel : ScriptableObject
    {
        [Header("Extended Level Settings")]
        [Space(5)] public string contentSourceName = string.Empty; //Levels from AssetBundles will have this as their Assembly Name.
        [Space(5)] public SelectableLevel selectableLevel;
        [Space(5)] [SerializeField] private int routePrice = 0;
        [Space(5)] public bool isHidden = false;
        [Space(5)] public bool isLocked = false;
        [Space(5)] public string lockedNodeText = string.Empty;
        [Space(10)] public List<StoryLogData> storyLogs = new List<StoryLogData>();
        [Header("Dynamic DungeonFlow Injections Settings")]
        [Space(5)] public ContentType allowedDungeonContentTypes = ContentType.Any;
        [Space(5)] public List<string> levelTags = new List<string>();
        [HideInInspector] public ContentType levelType;
        [SerializeField][TextArea] public string infoNodeDescripton = string.Empty;
        [HideInInspector] public TerminalNode routeNode;
        [HideInInspector] public TerminalNode routeConfirmNode;
        [HideInInspector] public TerminalNode infoNode;
        [Space(10)]
        [Header("Misc. Settings")]
        [Space(5)] public bool generateAutomaticConfigurationOptions = true;
        [HideInInspector] public LevelEvents levelEvents = new LevelEvents();
    }
        

    [System.Serializable]
    public class LevelEvents
    {
        public ExtendedEvent onLevelLoaded = new ExtendedEvent();
        public ExtendedEvent onNighttime = new ExtendedEvent();
        public ExtendedEvent<EnemyAI> onDaytimeEnemySpawn = new ExtendedEvent<EnemyAI>();
        public ExtendedEvent<EnemyAI> onNighttimeEnemySpawn = new ExtendedEvent<EnemyAI>();
        public ExtendedEvent<StoryLog> onStoryLogCollected = new ExtendedEvent<StoryLog>();
        public ExtendedEvent<LungProp> onApparatusTaken = new ExtendedEvent<LungProp>();
        public ExtendedEvent<(EntranceTeleport, PlayerControllerB)> onPlayerEnterDungeon = new ExtendedEvent<(EntranceTeleport, PlayerControllerB)>();
        public ExtendedEvent<(EntranceTeleport, PlayerControllerB)> onPlayerExitDungeon = new ExtendedEvent<(EntranceTeleport, PlayerControllerB)>();
        public ExtendedEvent<bool> onPowerSwitchToggle = new ExtendedEvent<bool>();

    }

    [System.Serializable]
    public class StoryLogData
    {
        public int storyLogID;
        public string terminalWord = string.Empty;
        public string storyLogTitle = string.Empty;
        [TextArea] public string storyLogDescription = string.Empty;

        [HideInInspector] internal int newStoryLogID;
    }
}
