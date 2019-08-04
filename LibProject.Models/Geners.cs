using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibProject.Models
{
    // books & journals categoties & generes
    public enum Genre
    {
        Unknown,
        Fiction,
        NonFiction,
        Academic
    }

    #region Sub-Genres
    public enum Fiction // subgenre
    {
        Action,
        Adventure,
        Childrens,
        Comedy,
        Comics,
        Crime,
        Drama,
        Fantasy,
        Historical,
        Horror,
        Mystery,
        Philosophical,
        Political,
        Religion,
        Romance,
        Saga,
        Satire,
        Science_Fiction,
        Social,
        Thriller,
        Urban,
        Western
    }

    public enum NonFiction // subgenre
    {
        Arts,
        Biography,
        Computers,
        Cookbooks,
        Crafts,
        Education,
        Health,
        History,
        Law,
        Money,
        Parenting,
        Politics,
        Relationships,
        Popular_Science,
        SelfHelp,
        Sports,
        Travel
    }

    public enum Academic // subgenre
    {
        Arts,
        Anthropology,
        Biology,
        Chemistry,
        Computer_Science,
        Earth_Sciences,
        Economics,
        Engineering,
        Geography,
        History,
        Languages,
        Law,
        Literature,
        Mathematics,
        Medicine,
        Philosophy,
        Physics,
        Political_Science,
        Psychology,
        Sociology,
        Space_Sciences,
        Statistics
    }
    #endregion

}
