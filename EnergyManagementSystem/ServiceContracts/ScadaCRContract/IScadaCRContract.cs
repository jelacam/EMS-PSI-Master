//-----------------------------------------------------------------------
// <copyright file="IScadaCRContract.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.ServiceContracts
{
    using System.ServiceModel;

    /// <summary>
    /// Contract for Scada Crunching
    /// </summary>
    [ServiceContract]
    public interface IScadaCRContract
    {
        /// <summary>
        /// Test method
        /// </summary>
        [OperationContract]
        void Test();

        /// <summary>
        /// SendValues method implementation
        /// </summary>
        /// <param name="value">values to send</param>
        /// <returns>returns true if success</returns>
        [OperationContract]
        bool SendValues(byte[] value);
    }
}