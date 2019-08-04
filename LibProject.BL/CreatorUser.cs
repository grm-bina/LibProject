using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibProject.Exceptions;
using LibProject.Models;

namespace LibProject.BL
{
    public static class CreatorUser
    {
        public static Respone<User> Create(string id, string lastName, string firstName, string middleName, string password, UserType access)
        {
            Respone<User> feedback = new Respone<User>();

            // check if can parse id
            int tempId;
            if(!int.TryParse(id, out tempId))
            {
                feedback.IsWorking = false;
                feedback.Message = "For enter ID use digits only";
                feedback.WrongDataType = WrongData.UserId;
                return feedback;
            }

            try
            {
                feedback.Data = new User(tempId, new HumanName() { LastName = lastName, FirstName = firstName, MiddleNameOrPatronymic = middleName }, password, access);
                feedback.IsWorking = true;
            }
            catch(EmptyLastNameException e)
            {
                feedback.Data = null;
                feedback.IsWorking = false;
                feedback.Message = e.Message;
                feedback.WrongDataType = WrongData.LastName;
            }
            catch(EmptyPasswordException e)
            {
                feedback.Data = null;
                feedback.IsWorking = false;
                feedback.Message = e.Message;
                feedback.WrongDataType = WrongData.Password;
            }
            catch(Exception e)
            {
                feedback.Data = null;
                feedback.IsWorking = false;
                feedback.Message = e.Message;
            }
            return feedback;
        }
    }
}
