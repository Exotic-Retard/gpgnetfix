namespace GPG.Multiplayer.Quazal.com.gaspowered.gpgnet.vault
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml.Serialization;

    [Serializable, DebuggerStepThrough, XmlType(Namespace="http://tempuri.org/"), GeneratedCode("System.Xml", "2.0.50727.1433"), DesignerCategory("code")]
    public class ChunkUploadResult
    {
        private int dataSizeField;
        private long fileSizeField;
        private long positionField;

        public int DataSize
        {
            get
            {
                return this.dataSizeField;
            }
            set
            {
                this.dataSizeField = value;
            }
        }

        public long FileSize
        {
            get
            {
                return this.fileSizeField;
            }
            set
            {
                this.fileSizeField = value;
            }
        }

        public long Position
        {
            get
            {
                return this.positionField;
            }
            set
            {
                this.positionField = value;
            }
        }
    }
}

