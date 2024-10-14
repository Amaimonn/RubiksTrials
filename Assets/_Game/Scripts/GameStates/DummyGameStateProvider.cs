using Assets._Game.Scripts.MVVM.Models;
using R3;

public class DummyGameStateProvider : IGameStateProvider
{
    public SettingsModel Settings {get; private set;}

    public Observable<SettingsModel> LoadSettings()
    {
        var settings = new SettingsModel();
        Settings = settings;
        return Observable.Return(settings);
    }

    public Observable<bool> SaveSettingsState()
    {
        // save
        return Observable.Return(true);
    }
}