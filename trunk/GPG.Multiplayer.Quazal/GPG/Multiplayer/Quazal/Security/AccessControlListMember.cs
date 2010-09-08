namespace GPG.Multiplayer.Quazal.Security
{
    using GPG.DataAccess;
    using System;

    [Serializable]
    public class AccessControlListMember : MappedObject
    {
        [FieldMap("acl_id")]
        private int mAccessControlListID;
        [FieldMap("guid")]
        private System.Guid mGuid;
        [FieldMap("acl_member_id")]
        private int mID;
        [FieldMap("inclusion_type")]
        private int mInclusionType;
        [FieldMap("member_id")]
        private int mMemberID;
        [FieldMap("member_name")]
        private string mMemberName;
        [FieldMap("member_type")]
        private int mMemberType;

        public AccessControlListMember(DataRecord record) : base(record)
        {
        }

        public int AccessControlListID
        {
            get
            {
                return this.mAccessControlListID;
            }
        }

        public System.Guid Guid
        {
            get
            {
                return this.mGuid;
            }
        }

        public int ID
        {
            get
            {
                return this.mID;
            }
        }

        public InclusionTypes InclusionType
        {
            get
            {
                return (InclusionTypes) this.mInclusionType;
            }
        }

        public int MemberID
        {
            get
            {
                return this.mMemberID;
            }
        }

        public string MemberName
        {
            get
            {
                return this.mMemberName;
            }
        }

        public int MemberType
        {
            get
            {
                return this.mMemberType;
            }
        }
    }
}

