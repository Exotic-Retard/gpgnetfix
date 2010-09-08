namespace GPG.Multiplayer.Quazal.com.gaspowered.gpgnet.vault
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml.Serialization;

    [Serializable, XmlType(Namespace="http://tempuri.org/"), GeneratedCode("System.Xml", "2.0.50727.1433"), DebuggerStepThrough, DesignerCategory("code")]
    public class ChunkDownloadResult
    {
        private int chunkSizeField;
        private byte[] dataField;
        private int dataSizeField;
        private string fileNameField;
        private bool isEOFField;
        private long positionField;

        public int ChunkSize
        {
            get
            {
                return this.chunkSizeField;
            }
            set
            {
                this.chunkSizeField = value;
            }
        }

        [XmlElement(DataType="base64Binary")]
        public byte[] Data
        {
            get
            {
                return this.dataField;
            }
            set
            {
                this.dataField = value;
            }
        }

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

        public string FileName
        {
            get
            {
                return this.fileNameField;
            }
            set
            {
                this.fileNameField = value;
            }
        }

        public bool IsEOF
        {
            get
            {
                return this.isEOFField;
            }
            set
            {
                this.isEOFField = value;
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

