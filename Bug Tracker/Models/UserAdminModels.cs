﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bug_Tracker.Models
{
    public class UserAdminModel
    {
        public string ID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        public UserAdminModel(string ID, string userName, string email, string role)
        {
            this.ID = ID;
            UserName = userName ?? "";
            Email = email ?? "";
            Role = role ?? "";
        }

        public UserAdminModel()
        {
        }
    }

    public class ViewUser : UserAdminModel
    {
        public bool PermissionGranted { get; set; }
        public string AdditionalInfo { get; set; }

        public ViewUser()
        {
        }
    }
}