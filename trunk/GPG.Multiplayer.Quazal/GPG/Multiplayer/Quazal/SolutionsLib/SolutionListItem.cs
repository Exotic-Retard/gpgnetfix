namespace GPG.Multiplayer.Quazal.SolutionsLib
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml.Serialization;

    [Serializable, DesignerCategory("code"), GeneratedCode("System.Xml", "2.0.50727.1433"), XmlType(Namespace="http://gpgnet.gaspowered.com/"), DebuggerStepThrough]
    public class SolutionListItem
    {
        private int solutionIDField;
        private string titleField;

        public int SolutionID
        {
            get
            {
                return this.solutionIDField;
            }
            set
            {
                this.solutionIDField = value;
            }
        }

        public string Title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }
    }
}

