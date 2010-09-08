namespace GPG.Multiplayer.LadderService
{
    using GPG.DataAccess;
    using GPG.UI;
    using System;
    using System.Drawing;

    public class LadderParticipant : MappedObject
    {
        [FieldMap("accepting_challenges")]
        private bool mAcceptingChallenges;
        [FieldMap("conflict_report_count")]
        private int mConflictReportCount;
        [FieldMap("current_streak")]
        private int mCurrentStreak;
        [FieldMap("disconnects")]
        private int mDisconnects;
        [FieldMap("draws")]
        private int mDraws;
        [FieldMap("entity_id")]
        private int mEntityID;
        [FieldMap("entity_name")]
        private string mEntityName;
        [FieldMap("online")]
        private bool mIsOnline;
        [FieldMap("join_date")]
        private DateTime mJoinDate;
        [FieldMap("ladder_instance_id")]
        private int mLadderInstanceID;
        [FieldMap("last_challenge_date")]
        private DateTime mLastChallengeDate;
        [FieldMap("last_degrade_date")]
        private DateTime mLastDegradeDate;
        [FieldMap("last_rank")]
        private int mLastRank;
        [FieldMap("losses")]
        private int mLosses;
        [FieldMap("non_report_count")]
        private int mNonReportCount;
        [FieldMap("rank")]
        private int mRank;
        [FieldMap("rater_count")]
        private int mRaterCount;
        private Image mRaterImageSmall;
        [FieldMap("rater_score")]
        private int mRaterScore;
        private Image mRatimgImageSmall;
        [FieldMap("rating_count")]
        private int mRatingCount;
        [FieldMap("rating_score")]
        private int mRatingScore;
        [FieldMap("record_streak")]
        private int mRecordStreak;
        [FieldMap("report_pending")]
        private bool mReportPending;
        [FieldMap("to_remove")]
        private bool mToRemove;
        [FieldMap("unresolved_games")]
        private int mUnresolvedGames;
        [FieldMap("wins")]
        private int mWins;

        public LadderParticipant(DataRecord record) : base(record)
        {
            this.mIsOnline = true;
        }

        public bool AcceptingChallenges
        {
            get
            {
                return (this.mAcceptingChallenges && this.IsOnline);
            }
        }

        public int ConflictReportCount
        {
            get
            {
                return this.mConflictReportCount;
            }
        }

        public int CurrentStreak
        {
            get
            {
                return this.mCurrentStreak;
            }
        }

        public double DaysSinceChallenge
        {
            get
            {
                TimeSpan span = (TimeSpan) (DateTime.UtcNow - this.LastChallengeDate);
                return Math.Round(span.TotalDays, 2);
            }
        }

        public double DaysSinceDegrade
        {
            get
            {
                TimeSpan span = (TimeSpan) (DateTime.UtcNow - this.LastDegradeDate);
                return Math.Round(span.TotalDays, 2);
            }
        }

        public double DaysUntilDegrade
        {
            get
            {
                foreach (LadderDegradation degradation in this.LadderInstance.LadderDefinition.Degradation)
                {
                    if (!degradation.RankTo.HasValue || (degradation.RankTo.Value >= this.Rank))
                    {
                        double num = Math.Round((double) (degradation.DegradeDayInterval - this.DaysSinceDegrade), 2);
                        if (num < 0.0)
                        {
                            return 0.0;
                        }
                        return num;
                    }
                }
                return 0.0;
            }
        }

        public float DisconnectPercentage
        {
            get
            {
                if (this.TotalGames == 0)
                {
                    return 0f;
                }
                return (float) Math.Round((double) ((((float) this.Disconnects) / ((float) this.TotalGames)) * 100f), 2);
            }
        }

        public int Disconnects
        {
            get
            {
                return this.mDisconnects;
            }
        }

        public int Draws
        {
            get
            {
                return this.mDraws;
            }
        }

        public int EntityID
        {
            get
            {
                return this.mEntityID;
            }
        }

        public string EntityName
        {
            get
            {
                return this.mEntityName;
            }
        }

        public bool IsOnline
        {
            get
            {
                return this.mIsOnline;
            }
        }

        public DateTime JoinDate
        {
            get
            {
                return this.mJoinDate;
            }
        }

        public GPG.Multiplayer.LadderService.LadderInstance LadderInstance
        {
            get
            {
                if (GPG.Multiplayer.LadderService.LadderInstance.AllInstances.ContainsKey(this.LadderInstanceID))
                {
                    return GPG.Multiplayer.LadderService.LadderInstance.AllInstances[this.LadderInstanceID];
                }
                return null;
            }
        }

        public int LadderInstanceID
        {
            get
            {
                return this.mLadderInstanceID;
            }
        }

        public DateTime LastChallengeDate
        {
            get
            {
                return DateTime.SpecifyKind(this.mLastChallengeDate, DateTimeKind.Utc);
            }
        }

        public DateTime LastDegradeDate
        {
            get
            {
                return DateTime.SpecifyKind(this.mLastDegradeDate, DateTimeKind.Utc);
            }
        }

        public int LastRank
        {
            get
            {
                return this.mLastRank;
            }
        }

        public int Losses
        {
            get
            {
                return this.mLosses;
            }
        }

        public int NonReportCount
        {
            get
            {
                return this.mNonReportCount;
            }
        }

        public int Rank
        {
            get
            {
                return this.mRank;
            }
        }

        public int RaterCount
        {
            get
            {
                return this.mRaterCount;
            }
        }

        public Image RaterImageSmall
        {
            get
            {
                if (this.mRaterImageSmall == null)
                {
                    if (float.IsNaN(this.ReputationRater) || (this.ReputationRater == 0f))
                    {
                        return LadderImages.stars_small_gray;
                    }
                    Image image = LadderImages.stars_small_blue;
                    int x = (int) (image.Width * (this.ReputationRater / 5f));
                    this.mRaterImageSmall = image;
                    DrawUtil.CopyImage(LadderImages.stars_small_gray, new Point(x, 0), this.mRaterImageSmall, new Point(x, 0), new Size(this.mRaterImageSmall.Width - x, this.mRaterImageSmall.Height));
                }
                return this.mRaterImageSmall;
            }
        }

        public int RaterScore
        {
            get
            {
                return this.mRaterScore;
            }
        }

        public int RatingCount
        {
            get
            {
                return this.mRatingCount;
            }
        }

        public Image RatingImageSmall
        {
            get
            {
                if (this.mRatimgImageSmall == null)
                {
                    if (float.IsNaN(this.ReputationRating) || (this.ReputationRating == 0f))
                    {
                        return LadderImages.stars_small_gray;
                    }
                    Image image = LadderImages.stars_small;
                    int x = (int) (image.Width * (this.ReputationRating / 5f));
                    this.mRatimgImageSmall = image;
                    DrawUtil.CopyImage(LadderImages.stars_small_gray, new Point(x, 0), this.mRatimgImageSmall, new Point(x, 0), new Size(this.mRatimgImageSmall.Width - x, this.mRatimgImageSmall.Height));
                }
                return this.mRatimgImageSmall;
            }
        }

        public int RatingScore
        {
            get
            {
                return this.mRatingScore;
            }
        }

        public int RecordStreak
        {
            get
            {
                return this.mRecordStreak;
            }
        }

        public bool ReportPending
        {
            get
            {
                return this.mReportPending;
            }
        }

        public float ReputationRater
        {
            get
            {
                if (this.RaterCount == 0)
                {
                    return 0f;
                }
                return (((float) this.RaterScore) / ((float) this.RaterCount));
            }
        }

        public float ReputationRating
        {
            get
            {
                if (this.RaterCount == 0)
                {
                    return 0f;
                }
                return (((float) this.RatingScore) / ((float) this.RatingCount));
            }
        }

        public bool ToRemove
        {
            get
            {
                return this.mToRemove;
            }
        }

        public int TotalGames
        {
            get
            {
                return ((this.Wins + this.Losses) + this.Draws);
            }
        }

        public int UnresolvedGames
        {
            get
            {
                return this.mUnresolvedGames;
            }
        }

        public float WinPercentage
        {
            get
            {
                if (this.TotalGames == 0)
                {
                    return 0f;
                }
                return (float) Math.Round((double) (((this.Wins + (((float) this.Draws) / 2f)) / ((float) this.TotalGames)) * 100f), 2);
            }
        }

        public int Wins
        {
            get
            {
                return this.mWins;
            }
        }
    }
}

