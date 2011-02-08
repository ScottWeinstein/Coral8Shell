using System;
using System.ComponentModel;
using System.Management.Automation;

namespace C8cx
{
    [RunInstaller(true)]
    public class C8cxSnapIn : PSSnapIn
    {
        public override string Name
        {
            get { return "C8cx"; }
        }

        public override string Vendor
        {
            get { return "ScottWeinstein"; }
        }

        public override string VendorResource
        {
            get { return string.Format("{0},{1}", Name, Vendor); }
        }

        public override string Description
        {
            get { return "Navigation provider & cmdlets for Coral8"; }
        }
    }

}
