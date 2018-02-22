using System;
using System.Configuration;

namespace EMS.Services.NetworkModelService
{
    public class Config
    {
        private string connectionString = string.Empty;

        public string ConnectionString
        {
            get { return connectionString; }
        }

        private Config()
        {
            // connection string to nms sql server db
            //connectionString = ConfigurationManager.ConnectionStrings["SqlServer_NM_DB"].ConnectionString;

            // azure connection string
            //connectionString = ConfigurationManager.ConnectionStrings["Azure_NMS_DB"].ConnectionString;
            
			// connection string to nms db file
            // connectionString = ConfigurationManager.ConnectionStrings["networkModelDbConnectionString"].ConnectionString;

            // connection string to file
            //connectionString = ConfigurationManager.ConnectionStrings["networkModelconnectionString"].ConnectionString;
        }

        #region Static members

        private static Config instance = null;

        public static Config Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Config();
                }

                return instance;
            }
        }

        #endregion Static members

        public string GetCompositeId(long valueWithSystemId)
        {
            string systemId = (Math.Abs(valueWithSystemId) >> 48).ToString();
            string valueWithoutSystemId = (Math.Abs(valueWithSystemId) & 0x0000FFFFFFFFFFFF).ToString();

            return String.Format("{0}{1}.{2}", valueWithSystemId < 0 ? "-" : "", systemId, valueWithoutSystemId);
        }
    }
}