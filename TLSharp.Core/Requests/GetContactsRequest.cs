using System;
using System.Collections.Generic;
using System.IO;
using TLSharp.Core.MTProto;

namespace TLSharp.Core.Requests
{
    class GetContactsRequest : MTProtoRequest
    {
        public List<Contact> contacts;
        public List<User> users;
        private string _hash;
        

        public GetContactsRequest(string hash = "")
        {
            _hash = hash;
        }

        public override void OnSend(BinaryWriter writer)
        {
            writer.Write(0x22c6aa08);
            Serializers.String.write(writer, _hash);
        }

        public override void OnResponse(BinaryReader reader)
        {

            bool notModified = reader.ReadUInt32() == 0xb74ba9d2; // else contacts.contacts#6f8b8cb2
            
            var result = reader.ReadInt32(); // vector code
            int imported_len = reader.ReadInt32();
            this.contacts = new List<Contact>(imported_len);
            for (int imported_index = 0; imported_index < imported_len; imported_index++)
            {
                Contact contacts_element;
                contacts_element = TL.Parse<Contact>(reader);
                this.contacts.Add(contacts_element);
            }
            reader.ReadInt32(); // vector code
            int users_len = reader.ReadInt32();
            this.users = new List<User>(users_len);
            for (int users_index = 0; users_index < users_len; users_index++)
            {
                User users_element;
                users_element = TL.Parse<User>(reader);
                this.users.Add(users_element);
            }
        }

        public override void OnException(Exception exception)
        {
            throw new NotImplementedException();
        }

        public override bool Confirmed => true;
        public override bool Responded { get; }
    }
}
