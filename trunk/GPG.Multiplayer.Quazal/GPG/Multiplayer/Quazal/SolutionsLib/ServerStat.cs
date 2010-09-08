namespace GPG.Multiplayer.Quazal.SolutionsLib
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml.Serialization;

    [Serializable, DebuggerStepThrough, XmlType(Namespace="http://gpgnet.gaspowered.com/"), GeneratedCode("System.Xml", "2.0.50727.1433"), DesignerCategory("code")]
    public class ServerStat
    {
        private string statNameField;
        private double statValueField;

        public string StatName
        {
            get
            {
                return this.statNameField;
            }
            set
            {
                this.statNameField = value;
            }
        }

        public double StatValue
        {
            get
            {
                return this.statValueField;
            }
            set
            {
                this.statValueField = value;
            }
        }
    }
}

