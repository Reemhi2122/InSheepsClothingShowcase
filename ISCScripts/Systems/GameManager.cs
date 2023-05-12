using UnityEngine;

namespace InSheepsClothing
{
    public enum GameState
    {
        GameState_None,
        GameState_Play,
        GameState_GameOver,
    }

    public class GameManager : UnitySingleton<GameManager>
    {
        public SheepSystem SheepSystem = null;
        public PresentationSystem PresentationSystem = null;
        public VotingSystem VotingSystem = null;
        public TargetSystem TargetSystem = null;
        public Player Player = null;
        public Transform PresentationSheepTransform = null;
        public Ipad Ipad;
    }
}