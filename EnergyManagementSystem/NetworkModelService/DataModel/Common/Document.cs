using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EMS.Common;
using EMS.Services.NetworkModelService.DataModel.Core;

namespace EMS.Services.NetworkModelService.DataModel.Common
{
    public class Document : IdentifiedObject
    {

        private DateTime createdDateTime;
        
        private DateTime lastModifiedDateTime;

        private string revisionNumber = string.Empty;

        private string subject = string.Empty;

        private string title = string.Empty;

        private string type = string.Empty;

        public Document(long globalId) : base(globalId)
        {

        }

        public DateTime CrecreatedDateTime
        {
            get { return createdDateTime; }
            set { createdDateTime = value; }
        }

        public DateTime LastModifiedDateTime
        {
            get { return lastModifiedDateTime; }
            set { lastModifiedDateTime = value; }
        }

        public string RevisionNumber
        {
            get { return revisionNumber; }
            set { revisionNumber = value; }
        }

        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                Document x = (Document)obj;
                return (x.createdDateTime == this.createdDateTime && x.lastModifiedDateTime == this.lastModifiedDateTime &&
                        x.revisionNumber == this.revisionNumber && x.subject == this.subject && x.title == this.title &&
                        x.type == this.type && x.createdDateTime == this.createdDateTime && x.lastModifiedDateTime == this.lastModifiedDateTime);
                        
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
            switch (property)
            {
                case ModelCode.DOCUMENT_REVNO:
                case ModelCode.DOCUMENT_SUBJECT:
                case ModelCode.DOCUMENT_TITLE:
                case ModelCode.DOCUMENT_TYPE:
                case ModelCode.DOCUMENT_CRDATETIME:
                case ModelCode.DOCUMENT_LASTMODIFIEDDATETIME:
                    return true;

                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property prop)
        {
            switch (prop.Id)
            {
                case ModelCode.DOCUMENT_REVNO:
                    prop.SetValue(revisionNumber);
                    break;

                case ModelCode.DOCUMENT_SUBJECT:
                    prop.SetValue(subject);
                    break;

                case ModelCode.DOCUMENT_TITLE:
                    prop.SetValue(title);
                    break;

                case ModelCode.DOCUMENT_TYPE:
                    prop.SetValue(type);
                    break;

                case ModelCode.DOCUMENT_CRDATETIME:
                    prop.SetValue(createdDateTime);
                    break;

                case ModelCode.DOCUMENT_LASTMODIFIEDDATETIME:
                    prop.SetValue(lastModifiedDateTime);
                    break;

                default:
                    base.GetProperty(prop);
                    break;
            }
        }

        public override void SetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.DOCUMENT_REVNO:
                    revisionNumber = property.AsString();
                    break;

                case ModelCode.DOCUMENT_SUBJECT:
                    subject = property.AsString();
                    break;

                case ModelCode.DOCUMENT_TITLE:
                    title = property.AsString();
                    break;

                case ModelCode.DOCUMENT_TYPE:
                    type = property.AsString();
                    break;

                case ModelCode.DOCUMENT_CRDATETIME:
                    createdDateTime = property.AsDateTime();
                    break;

                case ModelCode.DOCUMENT_LASTMODIFIEDDATETIME:
                    lastModifiedDateTime = property.AsDateTime();
                    break;

                default:
                    base.SetProperty(property);
                    break;
            }
        }

        #endregion IAccess implementation
    }
}
