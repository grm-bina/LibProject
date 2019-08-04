using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibProject.Models;
using LibProject.Exceptions;
using LibProject.Interfaces;

namespace LibProject.BL
{
    internal static class BorrowReturnOperations<T> where T : IBorrowingData
    {
        #region Borrow
        public static Respone<T> Borrow(T data, IBorrowable<T> item, IBorrower<T> client)
        {
            Respone<T> feedback = new Respone<T>();

            // check if item can be borrowed and client can borrow it
            if (!client.IsCanBorrow)
            {
                feedback.IsWorking = false;
                feedback.Message = "Client can't borrow any item";
                return feedback;
            }
            if (!item.IsAvailable)
            {
                feedback.IsWorking = false;
                feedback.Message = "The item is not available";
                return feedback;
            }
            if (!client.IsCanBorrowAt(data))
            {
                feedback.IsWorking = false;
                feedback.Message = "Client can't borrow this item";
                return feedback;
            }

            // update borrowing-data
            data.ItemDescription = item.ToString();
            data.BorrowedSinse = DateTime.Now;

            // try borrow
            try
            {
                if (!item.BorrowMe(data))
                {
                    feedback.IsWorking = false;
                    feedback.Message = "Failed";
                    return feedback;
                }

                if (!client.BorrowAt(data))
                {
                    feedback.IsWorking = false;
                    feedback.Message = "Failed";

                    item.ReturnMe(data);

                    return feedback;
                }

                feedback.Data = data;
                feedback.IsWorking = true;
                feedback.Message = $"{item.ToString()}\nborrowed succesfully by\n{client.ToString()}"; ;
            }
            catch(Exception e)
            {
                feedback.IsWorking = false;
                feedback.Message = e.Message;
            }

            return feedback;
        }

        #endregion

        #region Return
        public static Respone<T> Return(T data, IBorrowable<T> item, IBorrower<T> client)
        {
            Respone<T> feedback = new Respone<T>();

            try
            {
                if (!item.ReturnMe(data))
                {
                    feedback.IsWorking = false;
                    feedback.Message = "Failed: invalid data";
                    return feedback;
                }
                if (!client.ReturnAt(data))
                {
                    item.BorrowMe(data);

                    feedback.IsWorking = false;
                    feedback.Message = "Failed: invalid data";
                    return feedback;
                }

                feedback.IsWorking = true;
                feedback.Message = $"{item.ToString()}\nreturned succesfully by\n{client.ToString()}";
            }
            catch(Exception e)
            {
                feedback.IsWorking = false;
                feedback.Message = e.Message;
            }

            return feedback;
        }


        #endregion
    }
}
