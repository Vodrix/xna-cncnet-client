namespace DTAClient.Domain
{
    /// <summary>
    /// A class for sending statistics about updates and CnCNet to Google Analytics.
    /// except it doesn't do anything
    /// </summary>
    public class StatisticsSender
    {
        private static StatisticsSender _instance;

        public static StatisticsSender Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new StatisticsSender();

                return _instance;
            }
        }

        public void SendUpdate()
        {
           
        }
    }
}
