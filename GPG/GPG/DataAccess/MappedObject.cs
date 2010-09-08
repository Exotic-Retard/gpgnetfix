namespace GPG.DataAccess
{
    using GPG.Logging;
    using System;
    using System.Reflection;
    using System.Text;

    [Serializable]
    public abstract class MappedObject
    {
        protected MappedObject()
        {
        }

        protected MappedObject(DataRecord record)
        {
            this.Construct(record);
        }

        protected virtual void Construct(DataRecord record)
        {
            Type baseType = base.GetType();
            do
            {
                foreach (FieldInfo info in baseType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    try
                    {
                        string name;
                        int num;
                        float num3;
                        float num4;
                        int num5;
                        Type fieldType = info.FieldType;
                        object[] customAttributes = info.GetCustomAttributes(typeof(FieldMapAttribute), false);
                        if (customAttributes.Length > 0)
                        {
                            FieldMapAttribute attribute = customAttributes[0] as FieldMapAttribute;
                            name = attribute.Name;
                            if (attribute.DataType != null)
                            {
                                fieldType = attribute.DataType;
                            }
                        }
                        else
                        {
                            name = info.Name;
                        }
                        string str2 = record[name];
                        object obj2 = null;
                        if (((str2 == null) || !(str2 != "(null)")) || !(str2 != "None"))
                        {
                            goto Label_0310;
                        }
                        switch (fieldType.Name.ToLower())
                        {
                            case "nullable`1":
                                string str6;
                                if (((str6 = fieldType.FullName.Replace("System.Nullable`1[[System.", "").Replace(", mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]", "").ToLower()) != null) && (str6 == "boolean"))
                                {
                                    num = Convert.ToInt32(str2);
                                    if (num >= 0)
                                    {
                                        break;
                                    }
                                    obj2 = null;
                                }
                                goto Label_02F5;

                            case "string":
                                obj2 = str2;
                                goto Label_02F5;

                            case "guid":
                                obj2 = new Guid(str2);
                                goto Label_02F5;

                            case "boolean":
                                if (Convert.ToInt32(str2) > 0)
                                {
                                    goto Label_021E;
                                }
                                obj2 = false;
                                goto Label_02F5;

                            case "int32":
                                obj2 = Convert.ToInt32(str2);
                                goto Label_02F5;

                            case "datetime":
                                obj2 = Convert.ToDateTime(str2);
                                goto Label_02F5;

                            case "double":
                                double num2;
                                if (double.TryParse(str2, out num2))
                                {
                                    obj2 = num2;
                                }
                                goto Label_02F5;

                            case "single":
                                if (!float.TryParse(str2, out num3))
                                {
                                    goto Label_02F5;
                                }
                                if (str2.IndexOf(".") <= 0)
                                {
                                    goto Label_02D1;
                                }
                                num4 = num3;
                                num5 = 0;
                                goto Label_02A0;

                            default:
                                ErrorLog.WriteLine("ORM unhandled data type: {0}", new object[] { fieldType });
                                goto Label_02F5;
                        }
                        if (num > 0)
                        {
                            obj2 = true;
                        }
                        else
                        {
                            obj2 = false;
                        }
                        goto Label_02F5;
                    Label_021E:
                        obj2 = true;
                        goto Label_02F5;
                    Label_0290:
                        num4 /= 10f;
                        num5++;
                    Label_02A0:
                        if (num4 >= 1f)
                        {
                            goto Label_0290;
                        }
                        if (num5 > str2.IndexOf("."))
                        {
                            num3 /= (float) ((num5 - str2.IndexOf(".")) * 10);
                        }
                    Label_02D1:
                        obj2 = num3;
                    Label_02F5:
                        if (obj2 != null)
                        {
                            info.SetValue(this, obj2);
                        }
                    }
                    catch (Exception exception)
                    {
                        ErrorLog.WriteLine(exception);
                    }
                Label_0310:;
                }
                baseType = baseType.BaseType;
            }
            while (baseType != typeof(MappedObject));
        }

        public static type FromData<type>(DataRecord record) where type: MappedObject
        {
            return (Activator.CreateInstance(typeof(type), new object[] { record }) as type);
        }

        public static MappedObject FromData(Type type, DataRecord record)
        {
            return (Activator.CreateInstance(type, new object[] { record }) as MappedObject);
        }

        public static type FromDataString<type>(string dataString) where type: MappedObject
        {
            return FromDataTransfer<type>(DataRecord.FromDataString(dataString));
        }

        public static MappedObject FromDataString(Type type, string dataString)
        {
            return FromDataTransfer(type, DataRecord.FromDataString(dataString));
        }

        protected virtual void FromDataTransfer(DataRecord record)
        {
            this.Construct(record);
        }

        public static type FromDataTransfer<type>(DataRecord record) where type: MappedObject
        {
            type local = Activator.CreateInstance(typeof(type)) as type;
            local.FromDataTransfer(record);
            return local;
        }

        public static MappedObject FromDataTransfer(Type type, DataRecord record)
        {
            MappedObject obj2 = Activator.CreateInstance(type) as MappedObject;
            obj2.FromDataTransfer(record);
            return obj2;
        }

        public DataRecord ToDataRecord()
        {
            try
            {
                return DataRecord.FromDataString(this.ToDataString());
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                return null;
            }
        }

        public virtual string ToDataString()
        {
            try
            {
                FieldInfo[] fields = base.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
                StringBuilder builder = new StringBuilder(300);
                StringBuilder builder2 = new StringBuilder(300);
                for (int i = 0; i < fields.Length; i++)
                {
                    string name = "";
                    Type fieldType = fields[i].FieldType;
                    object[] customAttributes = fields[i].GetCustomAttributes(typeof(GPGDataAccessAttribute), true);
                    if (customAttributes.Length > 0)
                    {
                        foreach (GPGDataAccessAttribute attribute in customAttributes)
                        {
                            if (!(attribute is NotTransferredAttribute) && (attribute is FieldMapAttribute))
                            {
                                FieldMapAttribute attribute2 = attribute as FieldMapAttribute;
                                name = attribute2.Name;
                                if (attribute2.DataType != null)
                                {
                                    fieldType = attribute2.DataType;
                                }
                            }
                        }
                        builder.Append(name);
                        object obj2 = fields[i].GetValue(this);
                        string str2 = "";
                        if (obj2 == null)
                        {
                            str2 = "(null)";
                        }
                        else
                        {
                            string str4;
                            if (((str4 = fieldType.Name.ToLower()) != null) && (str4 == "boolean"))
                            {
                                if ((bool) obj2)
                                {
                                    str2 = "1";
                                }
                                else
                                {
                                    str2 = "0";
                                }
                            }
                            else
                            {
                                str2 = obj2.ToString();
                            }
                            builder2.Append(str2);
                        }
                        builder.Append('|');
                        builder2.Append('|');
                    }
                }
                builder.Remove(builder.Length - 1, 1);
                builder2.Remove(builder2.Length - 1, 1);
                builder.Append('\x0003');
                return (builder.ToString() + builder2.ToString());
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                return null;
            }
        }
    }
}

