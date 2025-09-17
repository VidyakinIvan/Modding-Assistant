using Modding_Assistant.Core;

namespace Modding_Assistant.MVVM.Model
{
    public class ModModel : ObservableObject
    {
        private int _order = 0;
        private string _name = "Unknown";
        private string _version = "0.0.0";
        private string _installInstructions = "No installation instructions available.";
        private string _url = "";
        private string _dependencies = "";
        private string _modRawName = "";
        private DateOnly _lastUpdated = DateOnly.FromDateTime(DateTime.Today);
        private string _description = "No description available.";
        private string _potentialIssues = "";
        public int Id { get; set; }
        public int Order 
        {
            get => _order;
            set => SetProperty(ref _order, value);
        }
        public string Name 
        { 
            get => _name;
            set => SetProperty(ref _name, value);
        }
        public string Version
        {
            get => _version;
            set => SetProperty(ref _version, value);
        }
        public string InstallInstructions
        {
            get => _installInstructions;
            set => SetProperty(ref _installInstructions, value);
        }
        public string Url
        {
            get => _url;
            set => SetProperty(ref _url, value);
        }
        public string Dependencies
        {
            get => _dependencies;
            set => SetProperty(ref _dependencies, value);
        }
        public string ModRawName
        {
            get => _modRawName;
            set => SetProperty(ref _modRawName, value);
        }
        public DateOnly LastUpdated 
        { 
            get => _lastUpdated;
            set => SetProperty(ref _lastUpdated, value);
        }
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }
        public string PotentialIssues
        {
            get => _potentialIssues;
            set => SetProperty(ref _potentialIssues, value);
        }
    }
}
