//-----------------------------------------------------------------------
// <copyright file="ConductingEquipment.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.Services.NetworkModelService.DataModel.Core
{
    using System;
    using System.Collections.Generic;
    using EMS.Common;

    /// <summary>
    /// ConductingEquipment class
    /// </summary>
    public class ConductingEquipment : Equipment
    {
        public ConductingEquipment(long globalId) : base(globalId)
        {

        }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #region IAccess implementation

        public override bool HasProperty(ModelCode property)
        {
            return base.HasProperty(property);
        }

        public override void GetProperty(Property property)
        {
            base.GetProperty(property);
        }

        public override void SetProperty(Property property)
        {
            base.SetProperty(property);
        }

        #endregion IAccess implementation

        #region IReference implementation


        #endregion IReference implementation
    }
}