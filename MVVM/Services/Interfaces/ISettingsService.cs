namespace Modding_Assistant.MVVM.Services.Interfaces
{
    public interface ISettingsService
    {
        double MainWindowLeft { get; set; }
        double MainWindowTop { get; set; }
        bool MainWindowFullScreen { get; set; }

        string ModsFolder { get; set; }
        void Save();
    }
}
