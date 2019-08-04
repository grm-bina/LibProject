using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibProject.Models;
using LibProject.Exceptions;

namespace LibProject.BL
{
    public static class Filter
    {
        #region By Genre
        public static Respone<List<AbstractItem>> ByGenre (Enum genre, List<AbstractItem> list)
        {
            Respone<List<AbstractItem>> feedback = new Respone<List<AbstractItem>>();

            if(list == null || list.Count == 0)
            {
                feedback.IsWorking = false;
                feedback.Message = "Nothing found";
                return feedback;
            }

            List<AbstractItem> temp = new List<AbstractItem>();

            if (genre == null)
            {
                feedback.IsWorking = false;
                feedback.Message = "Genre Error: please try select genre again";
                return feedback;

            }

            // filter by maingenre
            if (genre.GetType() == typeof(Genre))
            {
                // if unknown genre - return all list as is
                if((Genre)genre == Genre.Unknown)
                {
                    feedback.IsWorking = true;
                    feedback.Data = list;
                }
                else
                {
                    // if not unknown - search
                    foreach (AbstractItem item in list)
                    {
                        if (item.Category == (Genre)genre)
                            temp.Add(item);
                    }
                    if (temp.Count == 0)
                    {
                        feedback.IsWorking = false;
                        feedback.Message = "Nothing found";
                    }
                    else
                    {
                        feedback.IsWorking = true;
                        feedback.Data = temp;
                    }
                }

                return feedback;
            }

            // filter by subgenre
            foreach (AbstractItem item in list)
            {
                if (item.GetSubgeneres()!=null && item.GetSubgeneres().Contains(genre))
                    temp.Add(item);
            }
            if (temp.Count == 0)
            {
                feedback.IsWorking = false;
                feedback.Message = "Nothing found";
            }
            else
            {
                feedback.IsWorking = true;
                feedback.Data = temp;
            }

            return feedback;
        }

        #endregion

        #region By Type
        public static Respone<List<AbstractItem>> ByType(ItemType type, List<AbstractItem> list)
        {
            Respone<List<AbstractItem>> feedback = new Respone<List<AbstractItem>>();

            if (list == null || list.Count == 0)
            {
                feedback.IsWorking = false;
                feedback.Message = "Nothing found";
                return feedback;
            }

            List<AbstractItem> temp = new List<AbstractItem>();
            switch (type)
            {
                case ItemType.NotSelected:
                    {
                        // if item-type not selected - return list as is
                        feedback.IsWorking = true;
                        feedback.Data = list;
                        return feedback;
                        break;
                    }
                case ItemType.Book:
                    {
                        foreach(AbstractItem item in list)
                        {
                            if (item is Book)
                                temp.Add(item);
                        }
                        break;
                    }
                case ItemType.Journal:
                    {
                        foreach (AbstractItem item in list)
                        {
                            if (item is Journal)
                                temp.Add(item);
                        }
                        break;
                    }
            }
            if (temp.Count == 0)
            {
                feedback.IsWorking = false;
                feedback.Message = "Nothing found";
            }
            else
            {
                feedback.IsWorking = true;
                feedback.Data = temp;
            }
            return feedback;
        }

        #endregion

        #region By Current User
        // Show to reader only Aviable Items
        public static Respone<List<AbstractItem>> ByCurrentUser(List<AbstractItem> list)
        {
            Respone<List<AbstractItem>> feedback = new Respone<List<AbstractItem>>();

            if (list==null || list.Count == 0)
            {
                feedback.IsWorking = false;
                feedback.Message = "Nothing found";
                return feedback;
            }

            if(UsersManager.GetInstanse.CurrentUser == null || UsersManager.GetInstanse.CurrentUser.Access != UserType.Reader)
            {
                feedback.IsWorking = true;
                feedback.Data = list;
                return feedback;
            }
            else
            {
                List<AbstractItem> temp = new List<AbstractItem>();
                
                foreach(AbstractItem item in list)
                {
                    if (item.IsAvailable)
                        temp.Add(item);
                }

                if(temp.Count == 0)
                {
                    feedback.IsWorking = false;
                    feedback.Message = "Aviable Items Not Found";
                }
                else
                {
                    feedback.IsWorking = true;
                    feedback.Data = temp;
                }

                return feedback;
            }


        }
        #endregion
    }
}
