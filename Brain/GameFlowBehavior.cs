using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Brain
{
    internal class GameFlowBehavior : DrawableEntity
    {

        private DifficultyLevel currentLevel;

        private readonly List<ContractEntity> activeContracts = new List<ContractEntity>();
        private readonly List<HostEntity> activeHosts = new List<HostEntity>();

        bool hasSetupStart;

        public GameFlowBehavior()
        {
            NextDifficulty();

            Delay(1, AddHost);
            Delay(6, AddHost);
            Delay(3, AddContract);

            contractTimer = new ActionTimer(currentLevel.contractInterval, AddContract);
            hostVisitTimer = new ActionTimer(currentLevel.hostInterval, AddHost);
            Add(contractTimer);
            Add(hostVisitTimer);
        }

        private readonly Timer hostVisitTimer;
        private readonly Timer contractTimer;

        public override void Update(GameTime gameTime)
        {
            if (GameState.GameOver)
                return;

            base.Update(gameTime);

            if(ImprovedKeyboard.DidJustPress(Keys.A))
                FinishAllActions();
            if(ImprovedKeyboard.DidJustPress(Keys.N))
                NextDifficulty();

            //if (!hasSetupStart)
            //    return;

            //activeContracts.RemoveAll(x => !x.IsInteractable);
            //activeHosts.RemoveAll(x => !x.IsInteractable);

            var playtime = (float)gameTime.TotalGameTime.TotalSeconds;
            if (playtime > currentLevel.levelTime)
            {
                NextDifficulty();
            }

            //if (activeContracts.Count < currentLevel.NumberOfContracts && contractTimer.Finished)
            //{
            //    AddContract();
            //}

            //var contractProcessingNeeded = activeContracts.Sum(x => x.ProcessingLeft * GameState.BrainProcessing);
            //var hostProcessingAvailable = activeHosts.Sum(x => x.VisitingTimeLeft * GameState.BrainProcessing);

            //if (contractProcessingNeeded > hostProcessingAvailable && hostVisitTimer.Finished)
            //{
            //    AddHost();
            //}
        }

        private void NextDifficulty()
        {
            if (GameState.LevelIndex >= GameState.Levels.Count - 1)
                return;

            GameState.LevelIndex += 1;
            currentLevel = GameState.Levels[GameState.LevelIndex];
        }

        private void AddHost()
        {
            var hostEntity = new HostEntity() { LocalPosition = new Vector2(200, 0) };
            hostEntity.Add(new AnimateMoveBehavior(new Vector2(80, RandomVertical()), 1));
            activeHosts.Add(hostEntity);
            Add(hostEntity);

            hostVisitTimer.Start(currentLevel.hostInterval);
        }

        private void AddContract()
        {
            var contractEntity = new ContractEntity { LocalPosition = new Vector2(-200, 0) };
            contractEntity.Add(new AnimateMoveBehavior(new Vector2(-80, RandomVertical()), 1));
            activeContracts.Add(contractEntity);
            Add(contractEntity);

            contractTimer.Start(currentLevel.contractInterval);
        }

        private static float RandomVertical()
        {
            return BrainGame.Random.Next(-50, 50);
        }
    }
}