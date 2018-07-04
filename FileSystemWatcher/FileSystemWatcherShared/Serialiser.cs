using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace FSW
{
    class Serialiser
    {
        #region Singleton Initialisation

        private static Serialiser _instance = null;
        private static Object _initlock = typeof(Serialiser);

        /// <summary>
        /// Constructor. Initialize log4net
        /// </summary>
        private Serialiser() { }

        /// <summary>
        /// Gets access to the only instance of this class
        /// </summary>
        public static Serialiser Instance
        {
            get { return GetInstance(); }
        }

        private static Serialiser GetInstance()
        {
            try
            {
                lock (_initlock)
                {
                    if (_instance == null)
                    {
                        _instance = new Serialiser();
                    }
                }
            }
            catch { }

            return _instance;
        }

        #endregion
    }
}
