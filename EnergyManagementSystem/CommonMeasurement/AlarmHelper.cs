//-----------------------------------------------------------------------
// <copyright file="AlarmHelper.cs" company="EMS-Team">
// Copyright (c) EMS-Team. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace EMS.CommonMeasurement
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization;

    /// <summary>
    /// Class for alarms
    /// </summary>
    [DataContract]
    public class AlarmHelper : INotifyPropertyChanged
    {
        /// <summary>
        /// stores gid of the entity
        /// </summary>
        private long gid;

        /// <summary>
        /// Id to identify alarme object in table on UI
        /// </summary>
        private int id;

        /// <summary>
        ///  represents alarm severity level - coloring
        /// </summary>
        private SeverityLevel severity;

        /// <summary>
        /// stores value of the entity
        /// </summary>
        private float value;

        /// <summary>
        /// value that triggers the alarm
        /// </summary>
        private float initiatingValue;

        /// <summary>
        /// stores minValue of the entity
        /// </summary>
        private float minValue;

        /// <summary>
        /// stores maxValue of the entity
        /// </summary>
        private float maxValue;

        /// <summary>
        /// stores timeStamp of the entity
        /// </summary>
        private DateTime timeStamp;

        private DateTime lastChange;

        private string currentState;

        private AckState ackState;

        private PublishingStatus pubStatus;

        /// <summary>
        /// stores type of the entity
        /// </summary>
        private AlarmType type;

        /// <summary>
        /// type of alarm - persistent or not
        /// </summary>
        private PersistentState persistent;

        /// <summary>
        /// type of alarm - inhibit or not
        /// </summary>
        private InhibitState inhibit;

        /// <summary>
        /// stores the message
        /// </summary>
        private string message;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AlarmHelper" /> class
        /// </summary>
        public AlarmHelper()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AlarmHelper" /> class
        /// </summary>
        public AlarmHelper(long gid, float value, float minValue, float maxValue, DateTime timeStamp)
        {
            this.gid = gid;
            this.value = value;
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.timeStamp = timeStamp;
            this.message = "";
            this.persistent = PersistentState.Persistent;
            this.inhibit = InhibitState.Noninhibit;
            this.lastChange = timeStamp;
        }

        /// <summary>
        /// Gets or sets Gid of the entity
        /// </summary>
        [DataMember]
        public long Gid
        {
            get
            {
                return this.gid;
            }

            set
            {
                this.gid = value;
            }
        }

        [DataMember]
        public int ID
        {
            get; set;
        }

        [DataMember]
        public SeverityLevel Severity
        {
            get
            {
                return severity;
            }
            set
            {
                severity = value;
                NotifyPropertyChanged();
            }
        }

        [DataMember]
        public float Value
        {
            get
            {
                return this.value;
            }

            set
            {
                this.value = value;
                NotifyPropertyChanged();
            }
        }

        [DataMember]
        public float MinValue
        {
            get
            {
                return this.minValue;
            }

            set
            {
                this.minValue = value;
            }
        }

        [DataMember]
        public float MaxValue
        {
            get
            {
                return this.maxValue;
            }

            set
            {
                this.maxValue = value;
            }
        }

        [DataMember]
        public DateTime TimeStamp
        {
            get
            {
                return this.timeStamp;
            }

            set
            {
                this.timeStamp = value;
                NotifyPropertyChanged();
            }
        }

        [DataMember]
        public DateTime LastChange
        {
            get
            {
                return this.lastChange;
            }

            set
            {
                this.lastChange = value;
                NotifyPropertyChanged();
            }
        }

        [DataMember]
        public AlarmType Type
        {
            get
            {
                return this.type;
            }

            set
            {
                this.type = value;
                NotifyPropertyChanged();
            }
        }

        [DataMember]
        public string Message
        {
            get
            {
                return this.message;
            }

            set
            {
                this.message = value;
                NotifyPropertyChanged();
            }
        }

        [DataMember]
        public string CurrentState
        {
            get
            {
                return this.currentState;
            }
            set
            {
                this.currentState = value;
                NotifyPropertyChanged();
            }
        }

        [DataMember]
        public AckState AckState
        {
            get
            {
                return ackState;
            }
            set
            {
                ackState = value;
            }
        }

        [DataMember]
        public PublishingStatus PubStatus
        {
            get
            {
                return pubStatus;
            }
            set
            {
                pubStatus = value;
                NotifyPropertyChanged();
            }
        }

        [DataMember]
        public float InitiatingValue
        {
            get
            {
                return initiatingValue;
            }
            set
            {
                initiatingValue = value;
                NotifyPropertyChanged();
            }
        }

        [DataMember]
        public PersistentState Persistent
        {
            get
            {
                return persistent;
            }
            set
            {
                persistent = value;
            }
        }

        [DataMember]
        public InhibitState Inhibit
        {
            get
            {
                return inhibit;
            }
            set
            {
                inhibit = value;
            }
        }
    }
}