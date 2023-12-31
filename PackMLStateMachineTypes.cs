/******************************************************************************
**
** <auto-generated>
**     This code was generated by a tool: UaModeler
**     Runtime Version: 1.6.9, using .NET Server 3.3.0 template (version 0)
**
**     Changes to this file may cause incorrect behavior and will be lost if
**     the code is regenerated.
** </auto-generated>
**
** Copyright (c) 2006-2023 Unified Automation GmbH All rights reserved.
**
** Software License Agreement ("SLA") Version 2.8
**
** Unless explicitly acquired and licensed from Licensor under another
** license, the contents of this file are subject to the Software License
** Agreement ("SLA") Version 2.8, or subsequent versions
** as allowed by the SLA, and You may not copy or use this file in either
** source code or executable form, except in compliance with the terms and
** conditions of the SLA.
**
** All software distributed under the SLA is provided strictly on an
** "AS IS" basis, WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESS OR IMPLIED,
** AND LICENSOR HEREBY DISCLAIMS ALL SUCH WARRANTIES, INCLUDING WITHOUT
** LIMITATION, ANY WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
** PURPOSE, QUIET ENJOYMENT, OR NON-INFRINGEMENT. See the SLA for specific
** language governing rights and limitations under the SLA.
**
** Project: .NET OPC UA SDK information model for namespace http://yourorganisation.org/PackMLModel/
**
** Description: OPC Unified Architecture Software Development Kit.
**
** The complete license agreement can be found here:
** http://unifiedautomation.com/License/SLA/2.8/
**
** Created: 13.07.2023
**
******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Xml;
using System.Linq;
using System.Runtime.Serialization;
using UnifiedAutomation.UaBase;
using System.Diagnostics;

namespace FIP.PackMLStateMachine
{
    #region AlarmType Class
    /// <summary>
    /// </summary>
    [DataContract(Namespace = FIP.PackMLStateMachine.Namespaces.PackMLStateMachineXsd)]
    public partial class AlarmType : IEncodeable
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public AlarmType()
        {
            Initialize();
        }

        [OnDeserializing]
        private void Initialize(StreamingContext context)
        {
            Initialize();
        }

        private void Initialize()
        {
            m_Id = (int)0;
            m_AlarmMessage = null;
            m_AlarmTransition = (int)0;
            m_On = false;
        }
        #endregion

        #region Public Properties

        /// <summary>
        /// </summary>
        [DataMember(Name = "Id", IsRequired = false, Order = 1)]
        public int Id
        {
            get
            {
                return m_Id;
            }
            set
            {
                m_Id = value;
            }
        }

        /// <summary>
        /// </summary>
        [DataMember(Name = "AlarmMessage", IsRequired = false, Order = 2)]
        public string AlarmMessage
        {
            get
            {
                return m_AlarmMessage;
            }
            set
            {
                m_AlarmMessage = value;
            }
        }

        /// <summary>
        /// </summary>
        [DataMember(Name = "AlarmTransition", IsRequired = false, Order = 3)]
        public int AlarmTransition
        {
            get
            {
                return m_AlarmTransition;
            }
            set
            {
                m_AlarmTransition = value;
            }
        }

        /// <summary>
        /// </summary>
        [DataMember(Name = "On", IsRequired = false, Order = 4)]
        public bool On
        {
            get
            {
                return m_On;
            }
            set
            {
                m_On = value;
            }
        }

        #endregion

        #region IEncodeable Members
        /// <summary cref="IEncodeable.TypeId" />
        public virtual ExpandedNodeId TypeId
        {
            get { return DataTypeIds.AlarmType; }
        }

        /// <summary cref="IEncodeable.BinaryEncodingId" />
        public virtual ExpandedNodeId BinaryEncodingId
        {
            get { return ObjectIds.AlarmType_Encoding_DefaultBinary; }
        }
        /// <summary cref="IEncodeable.XmlEncodingId" />
        public virtual ExpandedNodeId XmlEncodingId
        {
            get { return ObjectIds.AlarmType_Encoding_DefaultXml; }
        }

        /// <summary cref="IEncodeable.Encode(IEncoder)" />
        public virtual void Encode(IEncoder encoder)
        {
            encoder.PushNamespace(Namespaces.PackMLStateMachineXsd);

            encoder.WriteInt32("Id", Id);
            encoder.WriteString("AlarmMessage", AlarmMessage);
            encoder.WriteInt32("AlarmTransition", AlarmTransition);
            encoder.WriteBoolean("On", On);

            encoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.Decode(IDecoder)" />
        public virtual void Decode(IDecoder decoder)
        {
            decoder.PushNamespace(Namespaces.PackMLStateMachineXsd);
            Id = decoder.ReadInt32("Id");
            AlarmMessage = decoder.ReadString("AlarmMessage");
            AlarmTransition = decoder.ReadInt32("AlarmTransition");
            On = decoder.ReadBoolean("On");

            decoder.PopNamespace();
        }

        /// <summary cref="EncodeableObject.IsEqual(IEncodeable)" />
        public virtual bool IsEqual(IEncodeable encodeable)
        {
            if (Object.ReferenceEquals(this, encodeable))
            {
                return true;
            }

            AlarmType value = encodeable as AlarmType;

            if (value == null)
            {
                return false;
            }
            if (!TypeUtils.IsEqual(m_Id, value.m_Id)) return false;
            if (!TypeUtils.IsEqual(m_AlarmMessage, value.m_AlarmMessage)) return false;
            if (!TypeUtils.IsEqual(m_AlarmTransition, value.m_AlarmTransition)) return false;
            if (!TypeUtils.IsEqual(m_On, value.m_On)) return false;

            return true;
        }

        /// <summary cref="ICloneable.Clone" />
        public virtual object Clone()
        {
            AlarmType clone = (AlarmType)this.MemberwiseClone();

            clone.m_Id = (int)TypeUtils.Clone(this.m_Id);
            clone.m_AlarmMessage = (string)TypeUtils.Clone(this.m_AlarmMessage);
            clone.m_AlarmTransition = (int)TypeUtils.Clone(this.m_AlarmTransition);
            clone.m_On = (bool)TypeUtils.Clone(this.m_On);

            return clone;
        }
        #endregion

        #region Private Fields
        private int m_Id;
        private string m_AlarmMessage;
        private int m_AlarmTransition;
        private bool m_On;
        #endregion
    }

    #region AlarmTypeCollection class
    /// <summary>
    /// A collection of AlarmType objects.
    /// </summary>
    [CollectionDataContract(Name = "ListOfAlarmType", Namespace = FIP.PackMLStateMachine.Namespaces.PackMLStateMachine, ItemName = "AlarmType")]
    public partial class AlarmTypeCollection : List<AlarmType>, ICloneable
    {
        #region Constructors
        /// <summary>
        /// Initializes the collection with default values.
        /// </summary>
        public AlarmTypeCollection() { }

        /// <summary>
        /// Initializes the collection with an initial capacity.
        /// </summary>
        public AlarmTypeCollection(int capacity) : base(capacity) { }

        /// <summary>
        /// Initializes the collection with another collection.
        /// </summary>
        public AlarmTypeCollection(IEnumerable<AlarmType> collection) : base(collection) { }
        #endregion

        #region Static Operators
        /// <summary>
        /// Converts an array to a collection.
        /// </summary>
        public static implicit operator AlarmTypeCollection(AlarmType[] values)
        {
            if (values != null)
            {
                return new AlarmTypeCollection(values);
            }

            return new AlarmTypeCollection();
        }

        /// <summary>
        /// Converts a collection to an array.
        /// </summary>
        public static explicit operator AlarmType[](AlarmTypeCollection values)
        {
            if (values != null)
            {
                return values.ToArray();
            }

            return null;
        }
        #endregion

        #region ICloneable Methods
        /// <summary>
        /// Creates a deep copy of the collection.
        /// </summary>
        public object Clone()
        {
            AlarmTypeCollection clone = new AlarmTypeCollection(this.Count);

            for (int ii = 0; ii < this.Count; ii++)
            {
                clone.Add((AlarmType)TypeUtils.Clone(this[ii]));
            }

            return clone;
        }
        #endregion
    }
    #endregion
    #endregion

    #region ButtonType Class
    /// <summary>
    /// </summary>
    [DataContract(Namespace = FIP.PackMLStateMachine.Namespaces.PackMLStateMachineXsd)]
    public partial class ButtonType : IEncodeable
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public ButtonType()
        {
            Initialize();
        }

        [OnDeserializing]
        private void Initialize(StreamingContext context)
        {
            Initialize();
        }

        private void Initialize()
        {
            m_Id = (int)0;
            m_ButtonName = null;
            m_Commands = new Int32Collection();
        }
        #endregion

        #region Public Properties

        /// <summary>
        /// </summary>
        [DataMember(Name = "Id", IsRequired = false, Order = 1)]
        public int Id
        {
            get
            {
                return m_Id;
            }
            set
            {
                m_Id = value;
            }
        }

        /// <summary>
        /// </summary>
        [DataMember(Name = "ButtonName", IsRequired = false, Order = 2)]
        public string ButtonName
        {
            get
            {
                return m_ButtonName;
            }
            set
            {
                m_ButtonName = value;
            }
        }

        /// <summary>
        /// </summary>
        [DataMember(Name = "Commands", IsRequired = false, Order = 3)]
        public Int32Collection Commands
        {
            get
            {
                return m_Commands;
            }
            set
            {
                m_Commands = value;

                if (value == null)
                {
                    m_Commands = new Int32Collection();
                }
            }
        }

        #endregion

        #region IEncodeable Members
        /// <summary cref="IEncodeable.TypeId" />
        public virtual ExpandedNodeId TypeId
        {
            get { return DataTypeIds.ButtonType; }
        }

        /// <summary cref="IEncodeable.BinaryEncodingId" />
        public virtual ExpandedNodeId BinaryEncodingId
        {
            get { return ObjectIds.ButtonType_Encoding_DefaultBinary; }
        }
        /// <summary cref="IEncodeable.XmlEncodingId" />
        public virtual ExpandedNodeId XmlEncodingId
        {
            get { return ObjectIds.ButtonType_Encoding_DefaultXml; }
        }

        /// <summary cref="IEncodeable.Encode(IEncoder)" />
        public virtual void Encode(IEncoder encoder)
        {
            encoder.PushNamespace(Namespaces.PackMLStateMachineXsd);

            encoder.WriteInt32("Id", Id);
            encoder.WriteString("ButtonName", ButtonName);
            encoder.WriteInt32Array("Commands", Commands);

            encoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.Decode(IDecoder)" />
        public virtual void Decode(IDecoder decoder)
        {
            decoder.PushNamespace(Namespaces.PackMLStateMachineXsd);
            Id = decoder.ReadInt32("Id");
            ButtonName = decoder.ReadString("ButtonName");
            Commands = decoder.ReadInt32Array("Commands");

            decoder.PopNamespace();
        }

        /// <summary cref="EncodeableObject.IsEqual(IEncodeable)" />
        public virtual bool IsEqual(IEncodeable encodeable)
        {
            if (Object.ReferenceEquals(this, encodeable))
            {
                return true;
            }

            ButtonType value = encodeable as ButtonType;

            if (value == null)
            {
                return false;
            }
            if (!TypeUtils.IsEqual(m_Id, value.m_Id)) return false;
            if (!TypeUtils.IsEqual(m_ButtonName, value.m_ButtonName)) return false;
            if (!TypeUtils.IsEqual(m_Commands, value.m_Commands)) return false;

            return true;
        }

        /// <summary cref="ICloneable.Clone" />
        public virtual object Clone()
        {
            ButtonType clone = (ButtonType)this.MemberwiseClone();

            clone.m_Id = (int)TypeUtils.Clone(this.m_Id);
            clone.m_ButtonName = (string)TypeUtils.Clone(this.m_ButtonName);
            clone.m_Commands = (Int32Collection)TypeUtils.Clone(this.m_Commands);

            return clone;
        }
        #endregion

        #region Private Fields
        private int m_Id;
        private string m_ButtonName;
        private Int32Collection m_Commands;
        #endregion
    }

    #region ButtonTypeCollection class
    /// <summary>
    /// A collection of ButtonType objects.
    /// </summary>
    [CollectionDataContract(Name = "ListOfButtonType", Namespace = FIP.PackMLStateMachine.Namespaces.PackMLStateMachine, ItemName = "ButtonType")]
    public partial class ButtonTypeCollection : List<ButtonType>, ICloneable
    {
        #region Constructors
        /// <summary>
        /// Initializes the collection with default values.
        /// </summary>
        public ButtonTypeCollection() { }

        /// <summary>
        /// Initializes the collection with an initial capacity.
        /// </summary>
        public ButtonTypeCollection(int capacity) : base(capacity) { }

        /// <summary>
        /// Initializes the collection with another collection.
        /// </summary>
        public ButtonTypeCollection(IEnumerable<ButtonType> collection) : base(collection) { }
        #endregion

        #region Static Operators
        /// <summary>
        /// Converts an array to a collection.
        /// </summary>
        public static implicit operator ButtonTypeCollection(ButtonType[] values)
        {
            if (values != null)
            {
                return new ButtonTypeCollection(values);
            }

            return new ButtonTypeCollection();
        }

        /// <summary>
        /// Converts a collection to an array.
        /// </summary>
        public static explicit operator ButtonType[](ButtonTypeCollection values)
        {
            if (values != null)
            {
                return values.ToArray();
            }

            return null;
        }
        #endregion

        #region ICloneable Methods
        /// <summary>
        /// Creates a deep copy of the collection.
        /// </summary>
        public object Clone()
        {
            ButtonTypeCollection clone = new ButtonTypeCollection(this.Count);

            for (int ii = 0; ii < this.Count; ii++)
            {
                clone.Add((ButtonType)TypeUtils.Clone(this[ii]));
            }

            return clone;
        }
        #endregion
    }
    #endregion
    #endregion

    #region ProcessType Class
    /// <summary>
    /// </summary>
    [DataContract(Namespace = FIP.PackMLStateMachine.Namespaces.PackMLStateMachineXsd)]
    public partial class ProcessType : IEncodeable
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public ProcessType()
        {
            Initialize();
        }

        [OnDeserializing]
        private void Initialize(StreamingContext context)
        {
            Initialize();
        }

        private void Initialize()
        {
            m_id = (int)0;
            m_Commands = new Int32Collection();
            m_SCTime = 0.0;
            m_CommandTime = 0.0;
        }
        #endregion

        #region Public Properties

        /// <summary>
        /// </summary>
        [DataMember(Name = "id", IsRequired = false, Order = 1)]
        public int id
        {
            get
            {
                return m_id;
            }
            set
            {
                m_id = value;
            }
        }

        /// <summary>
        /// </summary>
        [DataMember(Name = "Commands", IsRequired = false, Order = 2)]
        public Int32Collection Commands
        {
            get
            {
                return m_Commands;
            }
            set
            {
                m_Commands = value;

                if (value == null)
                {
                    m_Commands = new Int32Collection();
                }
            }
        }

        /// <summary>
        /// </summary>
        [DataMember(Name = "SCTime", IsRequired = false, Order = 3)]
        public double SCTime
        {
            get
            {
                return m_SCTime;
            }
            set
            {
                m_SCTime = value;
            }
        }

        /// <summary>
        /// </summary>
        [DataMember(Name = "CommandTime", IsRequired = false, Order = 4)]
        public double CommandTime
        {
            get
            {
                return m_CommandTime;
            }
            set
            {
                m_CommandTime = value;
            }
        }

        #endregion

        #region IEncodeable Members
        /// <summary cref="IEncodeable.TypeId" />
        public virtual ExpandedNodeId TypeId
        {
            get { return DataTypeIds.ProcessType; }
        }

        /// <summary cref="IEncodeable.BinaryEncodingId" />
        public virtual ExpandedNodeId BinaryEncodingId
        {
            get { return ObjectIds.ProcessType_Encoding_DefaultBinary; }
        }
        /// <summary cref="IEncodeable.XmlEncodingId" />
        public virtual ExpandedNodeId XmlEncodingId
        {
            get { return ObjectIds.ProcessType_Encoding_DefaultXml; }
        }

        /// <summary cref="IEncodeable.Encode(IEncoder)" />
        public virtual void Encode(IEncoder encoder)
        {
            encoder.PushNamespace(Namespaces.PackMLStateMachineXsd);

            encoder.WriteInt32("id", id);
            encoder.WriteInt32Array("Commands", Commands);
            encoder.WriteDouble("SCTime", SCTime);
            encoder.WriteDouble("CommandTime", CommandTime);

            encoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.Decode(IDecoder)" />
        public virtual void Decode(IDecoder decoder)
        {
            decoder.PushNamespace(Namespaces.PackMLStateMachineXsd);
            id = decoder.ReadInt32("id");
            Commands = decoder.ReadInt32Array("Commands");
            SCTime = decoder.ReadDouble("SCTime");
            CommandTime = decoder.ReadDouble("CommandTime");

            decoder.PopNamespace();
        }

        /// <summary cref="EncodeableObject.IsEqual(IEncodeable)" />
        public virtual bool IsEqual(IEncodeable encodeable)
        {
            if (Object.ReferenceEquals(this, encodeable))
            {
                return true;
            }

            ProcessType value = encodeable as ProcessType;

            if (value == null)
            {
                return false;
            }
            if (!TypeUtils.IsEqual(m_id, value.m_id)) return false;
            if (!TypeUtils.IsEqual(m_Commands, value.m_Commands)) return false;
            if (!TypeUtils.IsEqual(m_SCTime, value.m_SCTime)) return false;
            if (!TypeUtils.IsEqual(m_CommandTime, value.m_CommandTime)) return false;

            return true;
        }

        /// <summary cref="ICloneable.Clone" />
        public virtual object Clone()
        {
            ProcessType clone = (ProcessType)this.MemberwiseClone();

            clone.m_id = (int)TypeUtils.Clone(this.m_id);
            clone.m_Commands = (Int32Collection)TypeUtils.Clone(this.m_Commands);
            clone.m_SCTime = (double)TypeUtils.Clone(this.m_SCTime);
            clone.m_CommandTime = (double)TypeUtils.Clone(this.m_CommandTime);

            return clone;
        }
        #endregion

        #region Private Fields
        private int m_id;
        private Int32Collection m_Commands;
        private double m_SCTime;
        private double m_CommandTime;
        #endregion
    }

    #region ProcessTypeCollection class
    /// <summary>
    /// A collection of ProcessType objects.
    /// </summary>
    [CollectionDataContract(Name = "ListOfProcessType", Namespace = FIP.PackMLStateMachine.Namespaces.PackMLStateMachine, ItemName = "ProcessType")]
    public partial class ProcessTypeCollection : List<ProcessType>, ICloneable
    {
        #region Constructors
        /// <summary>
        /// Initializes the collection with default values.
        /// </summary>
        public ProcessTypeCollection() { }

        /// <summary>
        /// Initializes the collection with an initial capacity.
        /// </summary>
        public ProcessTypeCollection(int capacity) : base(capacity) { }

        /// <summary>
        /// Initializes the collection with another collection.
        /// </summary>
        public ProcessTypeCollection(IEnumerable<ProcessType> collection) : base(collection) { }
        #endregion

        #region Static Operators
        /// <summary>
        /// Converts an array to a collection.
        /// </summary>
        public static implicit operator ProcessTypeCollection(ProcessType[] values)
        {
            if (values != null)
            {
                return new ProcessTypeCollection(values);
            }

            return new ProcessTypeCollection();
        }

        /// <summary>
        /// Converts a collection to an array.
        /// </summary>
        public static explicit operator ProcessType[](ProcessTypeCollection values)
        {
            if (values != null)
            {
                return values.ToArray();
            }

            return null;
        }
        #endregion

        #region ICloneable Methods
        /// <summary>
        /// Creates a deep copy of the collection.
        /// </summary>
        public object Clone()
        {
            ProcessTypeCollection clone = new ProcessTypeCollection(this.Count);

            for (int ii = 0; ii < this.Count; ii++)
            {
                clone.Add((ProcessType)TypeUtils.Clone(this[ii]));
            }

            return clone;
        }
        #endregion
    }
    #endregion
    #endregion

    #region StackLightType Class
    /// <summary>
    /// </summary>
    [DataContract(Namespace = FIP.PackMLStateMachine.Namespaces.PackMLStateMachineXsd)]
    public partial class StackLightType : IEncodeable
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public StackLightType()
        {
            Initialize();
        }

        [OnDeserializing]
        private void Initialize(StreamingContext context)
        {
            Initialize();
        }

        private void Initialize()
        {
            m_Id = (int)0;
            m_Description = null;
            m_On = false;
        }
        #endregion

        #region Public Properties

        /// <summary>
        /// </summary>
        [DataMember(Name = "Id", IsRequired = false, Order = 1)]
        public int Id
        {
            get
            {
                return m_Id;
            }
            set
            {
                m_Id = value;
            }
        }

        /// <summary>
        /// </summary>
        [DataMember(Name = "Description", IsRequired = false, Order = 2)]
        public string Description
        {
            get
            {
                return m_Description;
            }
            set
            {
                m_Description = value;
            }
        }

        /// <summary>
        /// </summary>
        [DataMember(Name = "On", IsRequired = false, Order = 3)]
        public bool On
        {
            get
            {
                return m_On;
            }
            set
            {
                m_On = value;
            }
        }

        #endregion

        #region IEncodeable Members
        /// <summary cref="IEncodeable.TypeId" />
        public virtual ExpandedNodeId TypeId
        {
            get { return DataTypeIds.StackLightType; }
        }

        /// <summary cref="IEncodeable.BinaryEncodingId" />
        public virtual ExpandedNodeId BinaryEncodingId
        {
            get { return ObjectIds.StackLightType_Encoding_DefaultBinary; }
        }
        /// <summary cref="IEncodeable.XmlEncodingId" />
        public virtual ExpandedNodeId XmlEncodingId
        {
            get { return ObjectIds.StackLightType_Encoding_DefaultXml; }
        }

        /// <summary cref="IEncodeable.Encode(IEncoder)" />
        public virtual void Encode(IEncoder encoder)
        {
            encoder.PushNamespace(Namespaces.PackMLStateMachineXsd);

            encoder.WriteInt32("Id", Id);
            encoder.WriteString("Description", Description);
            encoder.WriteBoolean("On", On);

            encoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.Decode(IDecoder)" />
        public virtual void Decode(IDecoder decoder)
        {
            decoder.PushNamespace(Namespaces.PackMLStateMachineXsd);
            Id = decoder.ReadInt32("Id");
            Description = decoder.ReadString("Description");
            On = decoder.ReadBoolean("On");

            decoder.PopNamespace();
        }

        /// <summary cref="EncodeableObject.IsEqual(IEncodeable)" />
        public virtual bool IsEqual(IEncodeable encodeable)
        {
            if (Object.ReferenceEquals(this, encodeable))
            {
                return true;
            }

            StackLightType value = encodeable as StackLightType;

            if (value == null)
            {
                return false;
            }
            if (!TypeUtils.IsEqual(m_Id, value.m_Id)) return false;
            if (!TypeUtils.IsEqual(m_Description, value.m_Description)) return false;
            if (!TypeUtils.IsEqual(m_On, value.m_On)) return false;

            return true;
        }

        /// <summary cref="ICloneable.Clone" />
        public virtual object Clone()
        {
            StackLightType clone = (StackLightType)this.MemberwiseClone();

            clone.m_Id = (int)TypeUtils.Clone(this.m_Id);
            clone.m_Description = (string)TypeUtils.Clone(this.m_Description);
            clone.m_On = (bool)TypeUtils.Clone(this.m_On);

            return clone;
        }
        #endregion

        #region Private Fields
        private int m_Id;
        private string m_Description;
        private bool m_On;
        #endregion
    }

    #region StackLightTypeCollection class
    /// <summary>
    /// A collection of StackLightType objects.
    /// </summary>
    [CollectionDataContract(Name = "ListOfStackLightType", Namespace = FIP.PackMLStateMachine.Namespaces.PackMLStateMachine, ItemName = "StackLightType")]
    public partial class StackLightTypeCollection : List<StackLightType>, ICloneable
    {
        #region Constructors
        /// <summary>
        /// Initializes the collection with default values.
        /// </summary>
        public StackLightTypeCollection() { }

        /// <summary>
        /// Initializes the collection with an initial capacity.
        /// </summary>
        public StackLightTypeCollection(int capacity) : base(capacity) { }

        /// <summary>
        /// Initializes the collection with another collection.
        /// </summary>
        public StackLightTypeCollection(IEnumerable<StackLightType> collection) : base(collection) { }
        #endregion

        #region Static Operators
        /// <summary>
        /// Converts an array to a collection.
        /// </summary>
        public static implicit operator StackLightTypeCollection(StackLightType[] values)
        {
            if (values != null)
            {
                return new StackLightTypeCollection(values);
            }

            return new StackLightTypeCollection();
        }

        /// <summary>
        /// Converts a collection to an array.
        /// </summary>
        public static explicit operator StackLightType[](StackLightTypeCollection values)
        {
            if (values != null)
            {
                return values.ToArray();
            }

            return null;
        }
        #endregion

        #region ICloneable Methods
        /// <summary>
        /// Creates a deep copy of the collection.
        /// </summary>
        public object Clone()
        {
            StackLightTypeCollection clone = new StackLightTypeCollection(this.Count);

            for (int ii = 0; ii < this.Count; ii++)
            {
                clone.Add((StackLightType)TypeUtils.Clone(this[ii]));
            }

            return clone;
        }
        #endregion
    }
    #endregion
    #endregion


    #region EncodeableTypes
    /// <summary>
    /// Contains a method for registering all encodeable types of the namespace.
    /// </summary>
    public class EncodeableTypes
    {
        /// <summary>
        /// Register all encodeable types of the namespace at the communication stack.
        /// The Decoder will decode the registered types.
        /// </summary>
        public static void RegisterEncodeableTypes(MessageContext context)
        {
            context.Factory.AddEncodeableType(typeof(FIP.PackMLStateMachine.AlarmType));
            context.Factory.AddEncodeableType(typeof(FIP.PackMLStateMachine.ButtonType));
            context.Factory.AddEncodeableType(typeof(FIP.PackMLStateMachine.ProcessType));
            context.Factory.AddEncodeableType(typeof(FIP.PackMLStateMachine.StackLightType));
        }
    }
    #endregion
}
