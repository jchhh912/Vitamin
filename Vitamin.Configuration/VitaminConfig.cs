using Microsoft.Extensions.Logging;
using System.Data;
using Vitamin.Configuration.Infrastructure;

namespace Vitamin.Configuration
{
    public class VitaminConfig : IVitaminConfig
    {
        private readonly ILogger<VitaminConfig> _logger;

        private readonly IDbConnection _dbConnection;

        public GeneralSettings GeneralSettings { get; set; }



        private bool _hasInitialized;

        public VitaminConfig(
            ILogger<VitaminConfig> logger,
            IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
            _logger = logger;

            GeneralSettings = new();
          
        }



        public void RequireRefresh()
        {
            _hasInitialized = false;
        }
    }
}
