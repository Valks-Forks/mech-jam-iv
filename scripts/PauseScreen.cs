public partial class PauseScreen : CanvasLayer
{
    public override void _Ready()
    {
        //TODO this class shouldnt be the base; refactor this
        if (HasNode("Menu/ContinueButton"))
        {
            Button continueButton = GetNode<Button>("Menu/ContinueButton");
            continueButton.Pressed += () => UnpauseGame();
        }

        Button restartButton = GetNode<Button>("Menu/RestartButton");
        restartButton.Pressed += () => RestartScene();

        Button quitButton = GetNode<Button>("Menu/QuitButton");
        quitButton.Pressed += () => QuitGame();
    }

    public void PauseGame()
    {
        GetTree().Paused = true;

        Visible = true;
    }

    public void UnpauseGame()
    {
        Visible = false;

        GetTree().Paused = false;
    }

    public void QuitGame() => GetTree().Quit();

    public virtual void RestartScene()
    {
        GetTree().ReloadCurrentScene();

        UnpauseGame();
    }
}