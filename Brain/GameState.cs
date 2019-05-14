using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Brain
{
    public static class GameState
    {
        public static int Score;
        public static int Fails;

        public static bool GameOver => Fails >= 3;

        public static float HostTime = 30;
        public static float BrainProcessing = 0.07f;
        public static float BrainDrain = 0.12f;
        public static float BrainDrainDepthMultiplier = 0.89f;
        public static float BrainRecharge = 0.1f;

        public static int ScorePerContract = 100;
        public static int MaxScoreTimeRemaining = 100;

        public static float ContractTime = 25;

        public static int LevelIndex = -1;
        public static List<DifficultyLevel> Levels = new List<DifficultyLevel>()
        {
            new DifficultyLevel(20, 14, 14),
            new DifficultyLevel(40,12, 12),
            new DifficultyLevel(70, 12, 10),
            new DifficultyLevel(100, 11, 9.5f),
            new DifficultyLevel(140, 9, 9),
            new DifficultyLevel(180, 8, 8)
        };

        public static void Reset()
        {
            Score = 0;
            Fails = 0;
            LevelIndex = 0;
        }
    }

    public struct DifficultyLevel
    {
        public readonly float levelTime;
        public readonly float contractInterval;
        public readonly float hostInterval;

        public DifficultyLevel(float levelTime, float contractInterval, float hostInterval)
        {
            this.levelTime = levelTime;
            this.contractInterval = contractInterval;
            this.hostInterval = hostInterval;
        }
    }

    public static class GameColors
    {
        public static Color BadColor = new Color(216, 71, 42);
        public static Color GoodColor = new Color(216, 71, 42);
        public static Color DataColor = new Color(80, 111, 207);
    }

    internal static class Texts
    {
        private static int nameIndex;
        private static readonly List<string> Names = new List<string>()
        {
            "John",
            "Joe",
            "Adam",
            "Eric",
            "Steve",
            "Paul",
            "David",
            "Jim",
            "Creed",
            "Drew",
            "Bob"
        };

        private static int contractIndex;
        private static readonly List<string> Contracts = new List<string>()
        {
            "Missile\ntrajectory",
            "Cancer\ncoefficient",
            "Spam",
            "Invasion\nsimulation",
            "Summer\nweather",
            "Election\nprediction",
            "Virus\nspread",
            "Brute force\npasswords",
            "Stream\nvideo",
            "Encrypt\ndata",
            "Encode\nvideo",
            "Image\nanalysis"
        };

        static Texts()
        {
            var random = new Random();
            Names = Names.OrderBy(x => random.NextDouble()).ToList();
            Contracts = Contracts.OrderBy(x => random.NextDouble()).ToList();
        }

        public static string NextContract()
        {
            contractIndex = (contractIndex + 1) % Contracts.Count;
            return Contracts[contractIndex];
        }

        public static string NextName()
        {
            nameIndex = (nameIndex + 1) % Names.Count;
            return Names[nameIndex];
        }
    }
}