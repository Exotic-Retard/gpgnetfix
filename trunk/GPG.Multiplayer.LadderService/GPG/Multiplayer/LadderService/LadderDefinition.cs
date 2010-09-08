namespace GPG.Multiplayer.LadderService
{
    using GPG.DataAccess;
    using GPG.Multiplayer.Quazal;
    using System;
    using System.Collections.Generic;

    public class LadderDefinition : MappedObject
    {
        private static Dictionary<int, LadderDefinition> mAllDefinitions;
        [FieldMap("criteria_solution")]
        private int mCriteriaSolution;
        private static Type mDefaultParamsProvider;
        private LadderDegradation[] mDegradation;
        [FieldMap("description")]
        private string mDescription;
        [FieldMap("entity_name_query")]
        private string mEntityNameQuery;
        [FieldMap("entry_criteria_query")]
        private string mEntryCriteriaQuery;
        [FieldMap("ladder_definition_id")]
        private int mID;
        [FieldMap("is_clan")]
        private bool mIsClan;
        [FieldMap("is_individual")]
        private bool mIsIndiviual;
        [FieldMap("is_team")]
        private bool mIsTeam;
        [FieldMap("name")]
        private string mName;
        [FieldMap("params_provider_args")]
        private string mParamsProviderArgs;
        [FieldMap("params_provider_type")]
        private string mParamsProviderType;
        private ReportResolver mResolver;
        [FieldMap("resolver_type")]
        private string mResolverType;
        [FieldMap("rules_description")]
        private string mRulesDescription;

        public LadderDefinition(DataRecord record) : base(record)
        {
        }

        public GameParamsProvider CreateParamsProvider()
        {
            GameParamsProvider provider;
            if ((this.ParamsProviderType != null) && (this.ParamsProviderType.Length > 0))
            {
                provider = Activator.CreateInstance(null, this.ParamsProviderType).Unwrap() as GameParamsProvider;
            }
            else if (DefaultParamsProvider != null)
            {
                provider = Activator.CreateInstance(DefaultParamsProvider) as GameParamsProvider;
            }
            else
            {
                return null;
            }
            if ((this.ParamsProviderArgs != null) && (this.ParamsProviderArgs.Length > 0))
            {
                provider.ProcessArgs(this.ParamsProviderArgs);
            }
            return provider;
        }

        public static Dictionary<int, LadderDefinition> AllDefinitions
        {
            get
            {
                if (mAllDefinitions == null)
                {
                    mAllDefinitions = new Dictionary<int, LadderDefinition>();
                    foreach (LadderDefinition definition in new QuazalQuery("GetAllLadderDefinitions", new object[0]).GetObjects<LadderDefinition>())
                    {
                        mAllDefinitions[definition.ID] = definition;
                    }
                }
                return mAllDefinitions;
            }
        }

        public int CriteriaSolution
        {
            get
            {
                return this.mCriteriaSolution;
            }
        }

        public static Type DefaultParamsProvider
        {
            get
            {
                return mDefaultParamsProvider;
            }
            set
            {
                mDefaultParamsProvider = value;
            }
        }

        public LadderDegradation[] Degradation
        {
            get
            {
                if (this.mDegradation == null)
                {
                    this.mDegradation = new QuazalQuery("GetLadderDegradation", new object[] { this.ID }).GetObjects<LadderDegradation>().ToArray();
                    SortedList<int, LadderDegradation> list = new SortedList<int, LadderDegradation>(this.mDegradation.Length);
                    foreach (LadderDegradation degradation in this.mDegradation)
                    {
                        list.Add(degradation.RankFrom, degradation);
                    }
                    list.Values.CopyTo(this.mDegradation, 0);
                }
                return this.mDegradation;
            }
        }

        public string Description
        {
            get
            {
                return this.mDescription;
            }
        }

        public string EntityNameQuery
        {
            get
            {
                return this.mEntityNameQuery;
            }
        }

        public string EntryCriteriaQuery
        {
            get
            {
                return this.mEntryCriteriaQuery;
            }
        }

        public int ID
        {
            get
            {
                return this.mID;
            }
        }

        public bool IsClan
        {
            get
            {
                return this.mIsClan;
            }
        }

        public bool IsIndiviual
        {
            get
            {
                return this.mIsIndiviual;
            }
        }

        public bool IsTeam
        {
            get
            {
                return this.mIsTeam;
            }
        }

        public string Name
        {
            get
            {
                return this.mName;
            }
        }

        public string ParamsProviderArgs
        {
            get
            {
                return this.mParamsProviderArgs;
            }
        }

        public string ParamsProviderType
        {
            get
            {
                return this.mParamsProviderType;
            }
        }

        public ReportResolver Resolver
        {
            get
            {
                if (this.mResolver == null)
                {
                    if ((this.ResolverType != null) && (this.ResolverType.Length > 0))
                    {
                        this.mResolver = Activator.CreateInstance(null, this.ResolverType).Unwrap() as ReportResolver;
                    }
                    else
                    {
                        this.mResolver = new ReportResolver();
                    }
                }
                return this.mResolver;
            }
        }

        public string ResolverType
        {
            get
            {
                return this.mResolverType;
            }
        }

        public string RulesDescription
        {
            get
            {
                return this.mRulesDescription;
            }
        }
    }
}

