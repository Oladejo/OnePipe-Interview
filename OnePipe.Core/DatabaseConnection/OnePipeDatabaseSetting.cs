namespace OnePipe.Core.DatabaseConnection
{
    public class OnePipeDatabaseSetting : IOnePipeDatabaseSetting
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IOnePipeDatabaseSetting
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}

