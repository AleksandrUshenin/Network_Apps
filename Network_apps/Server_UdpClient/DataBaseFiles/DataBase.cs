using Server_UdpClient.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Server_UdpClient.Controllers;

namespace Server_UdpClient.DataBaseFiles
{
    internal class DataBase : IDataBase
    {
        //private MessageContext messageContext;
        public DataBase() 
        {
        }
        public bool AddUser(User user)
        {
            using (var db = new MessageContext())
            {
                var DbUsers = db.Users.ToList();
                var listUsers = ConvertToUser(DbUsers);
                if (Contains(listUsers, user))
                    return false;
                db.Users.Add(ConvertToUserDataBase(user));
                db.SaveChanges();
            }
            return true;
        }
        public bool AddUser(string user)
        {
            //int id = user.GetHashCode();
            User newUser = new User() { Name = user };
            return AddUser(newUser);
        }
        public List<Message>? GetMessages(string user)
        {
            int id = user.GetHashCode();
            User newUser = new User() { Id = id, Name = user };
            return GetMessages(newUser);
        }
        public List<Message>? GetMessages(User user)
        {
            return ReverseMessage(_GetMessages(user));
        }
        public List<Message>? GetUpDate(string user)
        {
            var userDb = GetUser(user);
            if (userDb == null) return null;

            var res = _GetMessages(new User() { Id = userDb.Id, Name = userDb.Name }).Where(x => x.StatusReceipt == false).ToList();

            //res.ForEach(x => { x.StatusReceipt = true; });

            return ReverseMessage(res);
        }
        public bool RemoveUser(User user)
        { 
            return false;
        }
        public bool RemoveUser(string user)
        { 
            return false; 
        }
        public bool AddMessage(string user, IMessage message)
        { 
            UserDataBase userFrom = GetUser(message.UserNameFrom);
            UserDataBase userTo = GetUser(message.UserNameTo);

            using (var db = new MessageContext())
            {
                var DbUsers = db.Users.ToList();
                var listUsers = ConvertToUser(DbUsers);
                if (userFrom == null || userTo == null)
                    return false;

                var mes = new MessageDataBase()
                {
                    FromIdUser = userFrom.Id,
                    StatusReceipt = false,
                    MessageContent = message.MessageText
                };

                var u = db.Users.ToList().Where(x => x.Id == userTo.Id).First();
                //db.Users.ToList().Where(x => x.Id == userTo.Id).First().Messages.Add(mes);
                var mesUser = u.Messages;
                if (u.Messages == null)
                    u.Messages = new HashSet<MessageDataBase>();
                u.Messages.Add(mes);
                db.SaveChanges();
            }

            return true;
        }
        private bool Contains(List<User> users, User user)
        {
            return users.Contains(user, new ComparerUsers());
        }
        private List<User> ConvertToUser(List<UserDataBase> userDataBases)
        {
            List<User> users = new List<User>();
            foreach (var user in userDataBases) 
            {
                users.Add(new User() { Id = user.Id, Name = user.Name });
            }
            return users;
        }
        private UserDataBase ConvertToUserDataBase(User user)
        {
            UserDataBase res = new UserDataBase() { Id = user.Id, Name = user.Name };
            res.Messages = new HashSet<MessageDataBase>();
            return res;
        }
        private List<Message>? ReverseMessage(List<MessageDataBase> messages) 
        {
            List<Message> messageDataBases = new List<Message>();
            foreach (var item in messages)
            {
                messageDataBases.Add(new Message() { Id = item.Id, Command = Commands.GetUpDateResponse, MessageText = item.MessageContent });
            }
            return messageDataBases;
        }
        private List<MessageDataBase>? _GetMessages(User user)
        {
            using (var db = new MessageContext())
            {
                var DbUsers = db.Users.ToList();
                var listUsers = ConvertToUser(DbUsers);
                if (Contains(listUsers, user))
                {
                    if (db.Messages.ToList() == null) return null;
                    var userF = db.Users.ToList().Where(x => x.Id == user.Id).First();
                    //var userF2 = DbUsers.Where(x => x.Id == user.Id).First();


                    return userF.Messages.ToList();
                }
            }
            return null;
        }
        private UserDataBase GetUser(string name)
        {
            UserDataBase userDataBase = null;

            using (var db = new MessageContext())
            {
                var DbUsers = db.Users.ToList();
                foreach (var item in DbUsers) 
                {
                    if (item.Name.Equals(name))
                    {
                        userDataBase = new UserDataBase() { Id = item.Id, Name = item.Name };
                    }
                }
            }

            return userDataBase;
        }
    }
}
