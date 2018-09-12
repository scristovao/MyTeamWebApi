using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace MyTeamWebApi.Model
{
    public class Team
    {
        public Team()
        {
            Matches = new List<MatchResultType>();
        }

        public string CoachName { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
        public bool IsActive { get; set; } = true;

        [IgnoreDataMember]
        public List<MatchResultType> Matches { get; set; }

        [IgnoreDataMember]
        public bool IsValid { get { return !String.IsNullOrWhiteSpace(Name) && Id > 0; } }

        public int GetTotals(MatchResultType result)
        {
            return (result.Equals(MatchResultType.All))
                                    ? Matches.Count()
                                    : Matches.Count(x => x == result);
        }

        public void AddMatch(MatchResultType result)
        {
            Matches.Add(result);
        }
    }
}