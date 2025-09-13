using Modding_Assistant.Core;

namespace Modding_Assistant.MVVM.Model
{
    internal class ModModel : ObservableObject
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public string Name { get; set; } = "Unknown";
        public string Version { get; set; } = "0.0.0";
        public string InstallInstructions { get; set; } = "No installation instructions available.";
        public string Url { get; set; } = "";
        public string[] Dependencies { get; set; } = [];
        public string ModRawName { get; set; } = "";
        public DateOnly LastUpdated { get; } = DateOnly.FromDateTime(DateTime.Today);
        public string Description { get; set; } = "No description available.";
        public string PotentialIssues { get; set; } = "No known issues.";
    }
}
