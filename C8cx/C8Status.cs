﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3053
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=2.0.50727.3038.
// 
namespace C8cx.DTO
{
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.coral8.com/cpx/2004/03/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.coral8.com/cpx/2004/03/", IsNullable=false)]
    public partial class Status {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Object", IsNullable=false)]
        public StatusObject[] CclWorkspaceInfo;
        
        /// <remarks/>
        public StatusContainerInfo ContainerInfo;
        
        /// <remarks/>
        public StatusManagerInfo ManagerInfo;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Object", IsNullable=false)]
        public StatusObject1[] CclApplicationInfo;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Object", IsNullable=false)]
        public StatusObject2[] CclCompilerInfo;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Object", IsNullable=false)]
        public StatusObject3[] CclQueryInfo;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Object", IsNullable=false)]
        public StatusObject4[] CcxModuleInfo;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ulong SourceTimestamp;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.coral8.com/cpx/2004/03/")]
    public partial class StatusObject {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Value")]
        public StatusObjectValue[] Value;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.coral8.com/cpx/2004/03/")]
    public partial class StatusObjectValue {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.coral8.com/cpx/2004/03/")]
    public partial class StatusContainerInfo {
        
        /// <remarks/>
        public StatusContainerInfoObject Object;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.coral8.com/cpx/2004/03/")]
    public partial class StatusContainerInfoObject {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Value")]
        public StatusContainerInfoObjectValue[] Value;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.coral8.com/cpx/2004/03/")]
    public partial class StatusContainerInfoObjectValue {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ulong SourceTimestamp;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.coral8.com/cpx/2004/03/")]
    public partial class StatusManagerInfo {
        
        /// <remarks/>
        public StatusManagerInfoObject Object;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.coral8.com/cpx/2004/03/")]
    public partial class StatusManagerInfoObject {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Value")]
        public StatusManagerInfoObjectValue[] Value;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.coral8.com/cpx/2004/03/")]
    public partial class StatusManagerInfoObjectValue {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.coral8.com/cpx/2004/03/")]
    public partial class StatusObject1 {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Value")]
        public StatusObjectValue1[] Value;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.coral8.com/cpx/2004/03/")]
    public partial class StatusObjectValue1 {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ulong SourceTimestamp;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool SourceTimestampSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.coral8.com/cpx/2004/03/")]
    public partial class StatusObject2 {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Value")]
        public StatusObjectValue2[] Value;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.coral8.com/cpx/2004/03/")]
    public partial class StatusObjectValue2 {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ulong SourceTimestamp;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool SourceTimestampSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.coral8.com/cpx/2004/03/")]
    public partial class StatusObject3 {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Value")]
        public StatusObjectValue3[] Value;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.coral8.com/cpx/2004/03/")]
    public partial class StatusObjectValue3 {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ulong SourceTimestamp;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public uint Value;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.coral8.com/cpx/2004/03/")]
    public partial class StatusObject4 {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Value")]
        public StatusObjectValue4[] Value;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name2;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.coral8.com/cpx/2004/03/")]
    public partial class StatusObjectValue4 {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ulong SourceTimestamp;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool SourceTimestampSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value;
    }
}
