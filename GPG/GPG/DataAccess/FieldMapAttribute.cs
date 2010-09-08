namespace GPG.DataAccess
{
    using System;

    public class FieldMapAttribute : GPGDataAccessAttribute
    {
        private Type _DataType;
        private string _Name;

        public FieldMapAttribute(string name)
        {
            this._Name = name;
        }

        public FieldMapAttribute(string name, Type dataType)
        {
            this._Name = name;
            this._DataType = dataType;
        }

        public Type DataType
        {
            get
            {
                return this._DataType;
            }
        }

        public string Name
        {
            get
            {
                return this._Name;
            }
        }
    }
}

