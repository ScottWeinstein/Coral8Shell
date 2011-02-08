using System;
using System.Runtime.Serialization;
using C8cx;
using System.Diagnostics;

[DataContract]
public partial class Out_2C8Tuple : C8Tuple
{
    public Out_2C8Tuple(C8Tuple tpl): base(tpl){}
    
    [DebuggerNonUserCode]
    [DataMember]
    public System.String @v1 
    { 
		get
		{
			return Field<System.String>(1);
		}
		set
		{
			this[1] = value;
		}
	}
    }
