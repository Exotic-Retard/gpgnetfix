namespace GPG.Multiplayer.Quazal.com.gaspowered.gpgnet.vault
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml.Serialization;

    [Serializable, DesignerCategory("code"), DebuggerStepThrough, GeneratedCode("System.Xml", "2.0.50727.1433"), XmlType(Namespace="http://tempuri.org/")]
    public class ContentUploadResult
    {
        private string checksumField;
        private int sizeInKBField;

        public string Checksum
        {
            get
            {
                return this.checksumField;
            }
            set
            {
                this.checksumField = value;
            }
        }

        public int SizeInKB
        {
            get
            {
                return this.sizeInKBField;
            }
            set
            {
                this.sizeInKBField = value;
            }
        }
    }
}

