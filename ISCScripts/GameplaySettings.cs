public static class GameplaySettings
{
    // Min and max delay for voting sheep that randomly look at player.
    public static int VotingSheepPeekDelayTimeMax = 100;
    public static int VotingSheepPeekDelayTimeMin = 15;   
    
    // Delay for how long the voting sheep will look at the player.
    public static int VotingSheepPeekTimeMax = 7;
    public static int VotingSheepPeekTimeMin = 8;

    public static int SheepMaxSuspiciousness = 100;
    public static int SheepIncrementSuspiciousness = 10;

    public static float CameraPictureDelay = 0.5f;

    public static float TimeTillVotingDelayRoundStart = 10;
    public static float TimeTillVotingMultiplier = 0.97f;
    public static float TimeTillVotingMin = 15;
    public static float TimeTillVotingMax = 45;

    public static float TimeToVoteMultiplier = 0.97f;
    public static float TimeToVoteMin = 7;
    public static float TimeToVoteMax = 20;

    public static float PresentationTimeDelayRoundStart = 20;
    public static float PresentationMultiplier = 0.96f;
    public static float PresentationTimeMin = 10;
    public static float PresentationTimeMax = 30;

    public static float PresentationUrgentTime = 10;
    public static float PresentationUrgentMultiplier = 0.97f;
    public static float PresentationUrgentMin = 3;
    public static float PresentationUrgentMax = 10;

    public static float PeekSheepTimeDelayRoundStart = 20;
    public static float PeekSheepTimeMultiplier = 1;
    public static float PeekSheepTimeMin = 10;
    public static float PeekSheepTimeMax = 10;

    public static float PeekSheepPeekMultiplier = 1;
    public static float PeekSheepPeekMax = 10;
    public static float PeekSheepPeekMin = 10;

    public static float WindowSheepDelayRoundStart = 1;
    public static float WindowSheepTimeMultiplier = 0.99f;
    public static float WindowSheepTimeMin = 40.0f;
    public static float WindowSheepTimeMax = 80.0f;

    public static float WindowSheepPeekTimeMin = 15.0f;
    public static float WindowSheepPeekTimeMax = 15.0f;

    public static int AmountOfStartingSheep = 8;
    public static int ExtraSheepSpawnTimer = 60;

    public static float DefaultTutorialActionDelay = 0.0f;
    public static float TutorialReminderDelay = 15.0f;
    public static float TutorialStartDelay = 1.0f;
}