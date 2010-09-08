namespace GPG.Multiplayer.Client.SolutionsLib
{
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.Quazal.SolutionsLib;
    using System;
    using System.Collections;
    using System.Threading;

    [Serializable]
    public class Solution
    {
        private bool AsyncInProgress;
        private static Hashtable AsyncOps = Hashtable.Synchronized(new Hashtable());
        private string mAuthor;
        private string mDescription;
        private int mID;
        private string[] mKeywords;
        private int mPopularity;
        private string[] mSearchWords;
        private string mTitle;
        private static GPG.Multiplayer.Quazal.SolutionsLib.Service Service = null;

        static Solution()
        {
            Service = new GPG.Multiplayer.Quazal.SolutionsLib.Service();
            Service.Url = ConfigSettings.GetString("SolutionsLibService", "http://gpgnet.gaspowered.com/quazal/Service.asmx?WSDL");
        }

        private Solution(int id)
        {
            this.mID = -1;
            this.AsyncInProgress = false;
            this.mID = id;
        }

        public Solution(int id, string title)
        {
            this.mID = -1;
            this.AsyncInProgress = false;
            this.mID = id;
            this.mTitle = title;
        }

        public Solution(int id, string title, string desc)
        {
            this.mID = -1;
            this.AsyncInProgress = false;
            this.mID = id;
            this.mTitle = title;
            this.mDescription = desc;
        }

        public Solution(int id, string title, string desc, int popular)
        {
            this.mID = -1;
            this.AsyncInProgress = false;
            this.mID = id;
            this.mTitle = title;
            this.mDescription = desc;
            this.mPopularity = popular;
        }

        private static void DoStraightLookup(object sender, GetSolutionDetailsCompletedEventArgs e)
        {
            Service.GetSolutionDetailsCompleted -= new GetSolutionDetailsCompletedEventHandler(Solution.DoStraightLookup);
            Solution solution = new Solution((int) e.UserState, e.title);
            solution.Author = e.author;
            solution.Description = e.Result;
            solution.Keywords = e.keywords;
            AsyncOps.Add(solution.ID, solution);
        }

        public override bool Equals(object obj)
        {
            return ((obj is Solution) && (this.GetHashCode() == obj.GetHashCode()));
        }

        public override int GetHashCode()
        {
            return this.ID;
        }

        public static Solution Lookup(int id)
        {
            Service.GetSolutionDetailsCompleted += new GetSolutionDetailsCompletedEventHandler(Solution.DoStraightLookup);
            Service.GetSolutionDetailsAsync(id, id);
            while (!AsyncOps.ContainsKey(id))
            {
                Thread.Sleep(10);
            }
            Solution solution = AsyncOps[id] as Solution;
            AsyncOps.Remove(id);
            return solution;
        }

        public override string ToString()
        {
            return this.Title;
        }

        public string Author
        {
            get
            {
                return this.mAuthor;
            }
            set
            {
                this.mAuthor = value;
            }
        }

        public string Description
        {
            get
            {
                return this.mDescription;
            }
            set
            {
                this.mDescription = value;
            }
        }

        public int ID
        {
            get
            {
                return this.mID;
            }
            set
            {
                this.mID = value;
            }
        }

        public string[] Keywords
        {
            get
            {
                return this.mKeywords;
            }
            set
            {
                this.mKeywords = value;
            }
        }

        public int Popularity
        {
            get
            {
                return this.mPopularity;
            }
            set
            {
                this.mPopularity = value;
            }
        }

        public string[] SearchWords
        {
            get
            {
                return this.mSearchWords;
            }
            set
            {
                this.mSearchWords = value;
            }
        }

        public string Title
        {
            get
            {
                return this.mTitle;
            }
            set
            {
                this.mTitle = value;
            }
        }
    }
}

