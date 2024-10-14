using Assets._Game.Scripts.MVVM.Models;
using R3;

public interface IGameStateProvider: IService
{
    public SettingsModel Settings {get;}

    public Observable<SettingsModel> LoadSettings();
    public Observable<bool> SaveSettingsState();
}