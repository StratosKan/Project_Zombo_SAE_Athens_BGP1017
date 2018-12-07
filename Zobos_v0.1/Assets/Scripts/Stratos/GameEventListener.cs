public interface GameEventListener
{
    void OnNewGame();

    void OnLevelChange(); // Generic method to apply anything that changes when we change a floor/level. Use this to remove unnessesary features that are level depended.

    void OnLevelOne();

    void OnLevelTwo();

    //MIGHT BE IMPLEMENTED ON FUTURE UPDATE.
    //void OnLevelThree();
    //void OnLevelFour();
    //void OnLevelFive();

    void OnPlayerDeath();

    void OnGamePause();

    void OnGameUnpause();

    void OnGameExit();

    void OnGameStart();
}
