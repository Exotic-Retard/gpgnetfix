namespace GPG.Multiplayer.Quazal.Security
{
    using GPG.DataAccess;
    using GPG.Logging;
    using GPG.Multiplayer.Quazal;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;

    [Serializable]
    public class AccessControlList : MappedObject
    {
        [FieldMap("access_level")]
        private int mAccessLevel;
        private static Dictionary<int, AccessControlList> mAll;
        [FieldMap("description")]
        private string mDescription;
        [FieldMap("guid")]
        private System.Guid mGuid;
        [FieldMap("access_control_list_id")]
        private int mID;
        [FieldMap("inclusion_type")]
        private int mInclusionType;
        [FieldMap("list_type")]
        private int mListType;
        private AccessControlListMember[] mMembers;
        [FieldMap("name")]
        private string mName;
        [FieldMap("tag")]
        private string mTag;

        public AccessControlList(DataRecord record) : base(record)
        {
        }

        public AccessControlListMember AddMember(int memberId)
        {
            return this.AddMember(memberId, InclusionTypes.Undefined);
        }

        public AccessControlListMember AddMember(int memberId, InclusionTypes inclusionType)
        {
            AccessControlListMember member2;
            if ((this.InclusionType == InclusionTypes.MemberDefined) && (inclusionType == InclusionTypes.Undefined))
            {
                throw new Exception("Cannot add a member whos inclusionType is Undefined to an ACL whos inclusion type is MemberDefined.");
            }
            if ((this.InclusionType != InclusionTypes.MemberDefined) && (inclusionType != InclusionTypes.Undefined))
            {
                ErrorLog.WriteLine("Ignoring member inclusion type because this ACL's inclusion type is not set to MemberDefined", new object[0]);
            }
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                System.Guid guid = System.Guid.NewGuid();
                if (new QuazalQuery("AddACLMember", new object[] { guid, this.ID, memberId, (int) inclusionType }).ExecuteNonQuery())
                {
                    AccessControlListMember member = new QuazalQuery("GetACLUserMemberByGuid", new object[] { guid }).GetObject<AccessControlListMember>();
                    DateTime now = DateTime.Now;
                    while (member == null)
                    {
                        Thread.Sleep(50);
                        member = new QuazalQuery("GetACLUserMemberByGuid", new object[] { guid }).GetObject<AccessControlListMember>();
                        if ((DateTime.Now - now) > TimeSpan.FromSeconds(5.0))
                        {
                            break;
                        }
                    }
                    if ((this.mMembers != null) && (member != null))
                    {
                        Array.Resize<AccessControlListMember>(ref this.mMembers, this.mMembers.Length + 1);
                        this.mMembers[this.mMembers.Length - 1] = member;
                    }
                    return member;
                }
                member2 = null;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
            return member2;
        }

        public static AccessControlList Create(string name, string description, AccessControlListTypes listType, InclusionTypes inclusionType)
        {
            return Create(name, description, listType, inclusionType, 0, null);
        }

        public static AccessControlList Create(string name, string description, AccessControlListTypes listType, InclusionTypes inclusionType, int access)
        {
            return Create(name, description, listType, inclusionType, access, null);
        }

        public static AccessControlList Create(string name, string description, AccessControlListTypes listType, InclusionTypes inclusionType, string tag)
        {
            return Create(name, description, listType, inclusionType, 0, tag);
        }

        public static AccessControlList Create(string name, string description, AccessControlListTypes listType, InclusionTypes inclusionType, int access, string tag)
        {
            AccessControlList list2;
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                System.Guid guid = System.Guid.NewGuid();
                if (new QuazalQuery("CreateACL", new object[] { guid, name, description, (int) listType, (int) inclusionType, access, tag }).ExecuteNonQuery())
                {
                    AccessControlList list = new QuazalQuery("GetACLByGuid", new object[] { guid }).GetObject<AccessControlList>();
                    DateTime now = DateTime.Now;
                    while (list == null)
                    {
                        Thread.Sleep(50);
                        list = new QuazalQuery("GetACLByGuid", new object[] { guid }).GetObject<AccessControlList>();
                        if ((DateTime.Now - now) > TimeSpan.FromSeconds(5.0))
                        {
                            break;
                        }
                    }
                    if (list != null)
                    {
                        All.Add(list.ID, list);
                    }
                    return list;
                }
                list2 = null;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
            return list2;
        }

        public bool Delete()
        {
            if (!new QuazalQuery("DeleteACL", new object[] { this.ID, this.ID }).ExecuteNonQuery())
            {
                return false;
            }
            if (All.ContainsKey(this.ID))
            {
                All.Remove(this.ID);
            }
            return true;
        }

        public AccessControlListMember FindMember(int memberId)
        {
            foreach (AccessControlListMember member in this.Members)
            {
                if (member.MemberID == memberId)
                {
                    return member;
                }
            }
            return null;
        }

        public bool FindMember(int memberId, out AccessControlListMember member)
        {
            member = this.FindMember(memberId);
            return (member != null);
        }

        public static AccessControlList GetByID(int id)
        {
            if (All.ContainsKey(id))
            {
                return All[id];
            }
            return null;
        }

        public static bool GetByID(int id, out AccessControlList acl)
        {
            acl = GetByID(id);
            return (acl != null);
        }

        public static AccessControlList GetByName(string name)
        {
            foreach (AccessControlList list in All.Values)
            {
                if (list.Name == name)
                {
                    return list;
                }
            }
            return null;
        }

        public static bool GetByName(string name, out AccessControlList acl)
        {
            acl = GetByName(name);
            return (acl != null);
        }

        public bool HasAccess()
        {
            return this.HasAccess(User.Current);
        }

        public bool HasAccess(User user)
        {
            if (user == null)
            {
                return false;
            }
            return (user.IsAdmin || this.HasAccess(user.ID));
        }

        public bool HasAccess(int memberId)
        {
            if (memberId > 0)
            {
                AccessControlListMember member;
                if (!this.FindMember(memberId, out member))
                {
                    return (this.InclusionType == InclusionTypes.Exclude);
                }
                switch (this.InclusionType)
                {
                    case InclusionTypes.Include:
                        return true;

                    case InclusionTypes.MemberDefined:
                        if (member.InclusionType != InclusionTypes.Include)
                        {
                            return false;
                        }
                        return true;
                }
            }
            return false;
        }

        public static bool HasAccessTo(int acl)
        {
            return HasAccessTo(acl, User.Current);
        }

        public static bool HasAccessTo(string aclName)
        {
            return HasAccessTo(aclName, User.Current);
        }

        public static bool HasAccessTo(int acl, User user)
        {
            if (user == null)
            {
                return false;
            }
            return ((acl == 0) || (user.IsAdmin || (All.ContainsKey(acl) && All[acl].HasAccess(user))));
        }

        public static bool HasAccessTo(int acl, int memberId)
        {
            return ((acl == 0) || (All.ContainsKey(acl) && All[acl].HasAccess(memberId)));
        }

        public static bool HasAccessTo(string aclName, User user)
        {
            return (((user != null) && user.IsAdmin) || HasAccessTo(aclName, user.ID));
        }

        public static bool HasAccessTo(string aclName, int memberId)
        {
            AccessControlList list;
            return (GetByName(aclName, out list) && list.HasAccess(memberId));
        }

        public bool RemoveMember(AccessControlListMember member)
        {
            return this.RemoveMember(member.ID);
        }

        public bool RemoveMember(int id)
        {
            if (new QuazalQuery("DeleteACLMember", new object[] { id, this.ID }).ExecuteNonQuery())
            {
                this.mMembers = null;
                return true;
            }
            return false;
        }

        public int AccessLevel
        {
            get
            {
                return this.mAccessLevel;
            }
        }

        public static Dictionary<int, AccessControlList> All
        {
            get
            {
                if (mAll == null)
                {
                    mAll = new Dictionary<int, AccessControlList>();
                    foreach (AccessControlList list in new QuazalQuery("GetAllACLs", new object[0]).GetObjects<AccessControlList>())
                    {
                        mAll.Add(list.ID, list);
                    }
                }
                return mAll;
            }
        }

        public string Description
        {
            get
            {
                return this.mDescription;
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

        public AccessControlListTypes ListType
        {
            get
            {
                return (AccessControlListTypes) this.mListType;
            }
        }

        public AccessControlListMember[] Members
        {
            get
            {
                if (this.mMembers == null)
                {
                    switch (this.ListType)
                    {
                        case AccessControlListTypes.MemberList:
                            this.mMembers = new QuazalQuery("GetACLUserMembers", new object[] { this.ID }).GetObjects<AccessControlListMember>().ToArray();
                            break;

                        case AccessControlListTypes.QueriedList:
                            this.mMembers = new QuazalQuery(this.Tag, new object[0]).GetObjects<AccessControlListMember>().ToArray();
                            break;
                    }
                }
                if (this.mMembers == null)
                {
                    this.mMembers = new AccessControlListMember[0];
                }
                return this.mMembers;
            }
        }

        public string Name
        {
            get
            {
                return this.mName;
            }
        }

        public string Tag
        {
            get
            {
                return this.mTag;
            }
        }
    }
}

