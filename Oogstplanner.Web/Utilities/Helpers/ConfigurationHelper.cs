namespace Oogstplanner.Utilities.Helpers
{
    public static class ConfigurationHelper
    {
        public static string ConnectionStringName
        {
            get
            {
                #if DEBUG
                return "TestOogstplannerDatabaseConnection";
                #else
                return "ProductionOogstplannerDatabaseConnection";
                #endif
            }
        }
    }
}
    