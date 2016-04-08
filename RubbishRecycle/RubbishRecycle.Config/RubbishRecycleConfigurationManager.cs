using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Config
{
    public class RubbishRecycleConfigurationManager
    {
        #region Fields

        private static readonly Object SingletonLock = new Object();

        private static RubbishRecycleConfigurationManager Instance;

        private readonly Configuration _config;

        #endregion

        #region Constructors

        private RubbishRecycleConfigurationManager(String file)
        {
            if (String.IsNullOrEmpty(file))
            {
                throw new ArgumentNullException("file");
            }
            if (String.IsNullOrEmpty(Path.GetDirectoryName(file)))
            {
                file = Path.Combine(AppDomain.CurrentDomain.RelativeSearchPath, file);
            }
            if (File.Exists(file))
            {
                ExeConfigurationFileMap map = new ExeConfigurationFileMap();
                map.ExeConfigFilename = file;
                this._config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
            }
            else
            {
                throw new FileNotFoundException(file);
            }
        }

        private RubbishRecycleConfigurationManager()
            :this(ConfigurationManager.AppSettings["rubbishConfig"])
        {
        }

        #endregion

        #region Properties

        public SmsConfigSection SmsConfig
        {
            get
            {
                return this._config.GetSection("sms") as SmsConfigSection;
            }
        }

        #endregion

        #region Methods

        public static RubbishRecycleConfigurationManager GetInstance(String file = null)
        {
            lock(RubbishRecycleConfigurationManager.SingletonLock)
            {
                if (RubbishRecycleConfigurationManager.Instance == null)
                {
                    if (String.IsNullOrWhiteSpace(file))
                    {
                        RubbishRecycleConfigurationManager.Instance = new RubbishRecycleConfigurationManager();
                    }
                    else
                    {
                        RubbishRecycleConfigurationManager.Instance = new RubbishRecycleConfigurationManager(file);
                    }
                }
            }
            return RubbishRecycleConfigurationManager.Instance;
        }

        #endregion
    }
}
