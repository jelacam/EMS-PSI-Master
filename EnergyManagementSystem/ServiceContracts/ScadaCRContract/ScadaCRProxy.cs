//-----------------------------------------------------------------------
// <copyright file="ScadaCRProxy.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.ServiceContracts
{
    using System;
    using System.ServiceModel;

    /// <summary>
    /// Class for IScadaCRContract and IDisposable implementation
    /// </summary>
    public class ScadaCRProxy : IScadaCRContract, IDisposable
    {
        /// <summary>
        /// proxy object
        /// </summary>
        private static IScadaCRContract proxy;

        /// <summary>
        /// ChannelFactory object
        /// </summary>
        private static ChannelFactory<IScadaCRContract> factory;

        private static object lockObj = new object();

        /// <summary>
        /// Gets or sets instance of IScadaCRContract
        /// </summary>
        public static IScadaCRContract Instance
        {
            get
            {
                lock (lockObj)
                {
                    if (proxy == null)
                    {
                        factory = new ChannelFactory<IScadaCRContract>("*");
                        proxy = factory.CreateChannel();
                    }

                    return proxy;
                }
            }

            set
            {
                lock (lockObj)
                {
                    if (proxy == null)
                    {
                        proxy = value;
                    }
                }
            }
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            if (factory != null)
            {
                factory = null;
            }
        }

        /// <summary>
        /// SendValues method implementation
        /// </summary>
        /// <param name="value">values to send</param>
        /// <returns>returns true if success</returns>
        public bool SendValues(byte[] value)
        {
            lock (lockObj)
            {
                return proxy.SendValues(value);
            }
        }
    }
}