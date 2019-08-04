using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibProject.Models;
using LibProject.Exceptions;

namespace LibProject.BL
{
    public static class ItemFactoryAndEditor
    {
        #region Factory

        // try create new instanse of item with basic data that must have
        public static Respone<AbstractItem> TryCreateItem(ItemType type, string title, int id, Genre mainGenre, string copies, string volume)
        {
            Respone<AbstractItem> feedback = new Respone<AbstractItem>();
            int Copies;
            int Volume;

            // check if the strings with input-data may be parsed
            if (!int.TryParse(copies, out Copies))
            {
                feedback.IsWorking = false;
                feedback.Message = "Error: enter digits for number of Copies";
                feedback.WrongDataType = WrongData.NumOfCopies;
                return feedback;
            }
            
            // if item-type not selected - can't be created instance
            if(type == ItemType.NotSelected)
            {
                feedback.IsWorking = false;
                feedback.Message = "Item-type must be selected";
                feedback.WrongDataType = WrongData.ItemType;
                return feedback;
            }


            // try create instanse by selected type
            try
            {

                switch (type)
                {
                    case ItemType.Book:
                        {
                            feedback.Data = new Book(title, id, mainGenre, Copies);
                            feedback.IsWorking = true;
                            break;
                        }
                    case ItemType.Journal:
                        {
                            if (!int.TryParse(volume, out Volume))
                            {
                                feedback.IsWorking = false;
                                feedback.Message = "Error: enter digits for number of Volume";
                                feedback.WrongDataType = WrongData.Volume;
                                return feedback;
                            }
                            else
                            {
                                feedback.Data = new Journal(Volume, title, id, mainGenre, Copies);
                                feedback.IsWorking = true;
                            }
                            break;
                        }
                }
            }
            catch (InvalidItemIdException e)
            {
                feedback.Data = null;
                feedback.Message = e.Message;
                feedback.WrongDataType = WrongData.ItemId;
                feedback.IsWorking = false;
            }
            catch (EmptyTitleException e)
            {
                feedback.Data = null;
                feedback.Message = e.Message;
                feedback.WrongDataType = WrongData.Title;
                feedback.IsWorking = false;
            }
            catch (InvalidNumCopiesException e)
            {
                feedback.Data = null;
                feedback.Message = e.Message;
                feedback.WrongDataType = WrongData.NumOfCopies;
                feedback.IsWorking = false;
            }
            catch (InvalidGenersException e)
            {
                feedback.Data = null;
                feedback.Message = e.Message;
                feedback.WrongDataType = WrongData.Genre;
                feedback.IsWorking = false;
            }
            catch (InvalidVolumException e)
            {
                feedback.Data = null;
                feedback.Message = e.Message;
                feedback.WrongDataType = WrongData.Volume;
                feedback.IsWorking = false;
            }
            return feedback;
        }
        #endregion

        #region Editor
        public static Respone<AbstractItem> TryEdit(AbstractItem item, string title, string publisher, string annotation, string placeInLib, string year, string editionOrIssue, List<Enum> subGeners, List<HumanName> authorsName, string volume)
        {
            Respone<AbstractItem> feedback = new Respone<AbstractItem>();

            // create backup of item (if edit will failed - restore the item to previous conditions
            AbstractItem backup;
            if(item is Book)
            {
                Book current = item as Book;
                backup = new Book("temp", current.ID, current.Category, 1);
            }
            else if(item is Journal)
            {
                Journal current = item as Journal;
                backup = new Journal(1, "temp", current.ID, current.Category, 1);
            }
            else
            {
                feedback.IsWorking = false;
                feedback.Message = "Unknown item-type";
                feedback.WrongDataType = WrongData.ItemType;
                return feedback;
            }

            // save backup
            if (!backup.RestoreFromBackup(item))
            {
                feedback.IsWorking = false;
                feedback.Message = "Unpredicted Error";
                return feedback;
            }

            // try edit item
            try
            {
                // abstract fields
                item.Title = title;
                item.Publisher = publisher;
                item.Annotation = annotation;
                item.PlaceInLib = placeInLib;
                int yearParsed, editionParsed;
                if (int.TryParse(year, out yearParsed))
                    item.Year = yearParsed;
                if (int.TryParse(editionOrIssue, out editionParsed))
                    item.EditionOrIssue = editionParsed;
                item.ReplaceSubGeneres(subGeners);

                // special fields by type
                if (item is Book)
                {
                    Book current = item as Book;
                    current.ReplaceAuthors(authorsName);
                }
                else if (item is Journal)
                {
                    int volumeParsed;
                    if (!int.TryParse(volume, out volumeParsed))
                        throw new InvalidVolumException();

                    Journal current = item as Journal;
                    current.Volume = volumeParsed;
                }

                feedback.IsWorking = true;
                feedback.Data = item;
            }
            catch(EmptyTitleException e)
            {
                feedback.IsWorking = false;
                feedback.WrongDataType = WrongData.Title;
                feedback.Message = e.Message;
                item.RestoreFromBackup(backup);
            }
            catch(InvalidPublishingDateException e)
            {
                feedback.IsWorking = false;
                feedback.WrongDataType = WrongData.PublishingDate;
                feedback.Message = e.Message;
                item.RestoreFromBackup(backup);
            }
            catch(InvalidEditionOrIssueException e)
            {
                feedback.IsWorking = false;
                feedback.WrongDataType = WrongData.EditionOrIssue;
                feedback.Message = e.Message;
                item.RestoreFromBackup(backup);
            }
            catch(InvalidGenersException e)
            {
                feedback.IsWorking = false;
                feedback.WrongDataType = WrongData.Genre;
                feedback.Message = e.Message;
                item.RestoreFromBackup(backup);
            }
            catch(EmptyLastNameException e)
            {
                feedback.IsWorking = false;
                feedback.WrongDataType = WrongData.LastName;
                feedback.Message = e.Message;
                item.RestoreFromBackup(backup);
            }
            catch(InvalidVolumException e)
            {
                feedback.IsWorking = false;
                feedback.WrongDataType = WrongData.Volume;
                feedback.Message = e.Message;
                item.RestoreFromBackup(backup);
            }
            catch(Exception e)
            {
                feedback.IsWorking = false;
                feedback.Message = e.Message;
                item.RestoreFromBackup(backup);
            }

            // if edit worked - resort library (because if item is already added in library and changed it's title- it may destroy sorted-by-title list and search in this)
            if(feedback.IsWorking)
                LibraryManager.GetInstance.ReSort();

            return feedback;
        }
        #endregion
    }
}
