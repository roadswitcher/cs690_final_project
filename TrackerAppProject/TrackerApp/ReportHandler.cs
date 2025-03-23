namespace TrackerApp
{
    public class ReportHandler(IDataStore dataStore)
    {
        // Give me a link to the DataStore singleton, or an exception if you cannot
        private readonly IDataStore _dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));
        
        // TODO
        // - Daily report ( aggregate, time of day )
        // - Weekly Report ( aggregate, time of day )
        // Stretch goal- Trends Identification?

    }
}