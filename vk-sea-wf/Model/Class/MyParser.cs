﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using vk_sea_wf.Model.Interfaces;
using VkNet.Enums.Filters;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Utils;
using xNet;

namespace vk_sea_wf.Model.Class
{
      class MyParser : IParse {
        public static string api_url = "https://api.vk.com/";
        public static int app_id = 5677623;
        public string version = "5.60";

        public FriendsOrder order = new FriendsOrder();
        public List<User> users { get; set; }
        public List<String> userFriends { get; set; }
        public List<List<String>> userSubFriends { get; set; }
        

        public IList<string> user_friends {
            get
            {
                return this.userFriends;
            }
        }
        public IList<List<string>> user_sub_friends {
            get
            {
                return this.userSubFriends;
            }
        }
        public enum VkontakteScopeList {
            notify = 1,
            friends = 2,
            photos = 4,
            audio = 8,
            video = 16,
            offers = 32,
            questions = 64,
            pages = 128,
            link = 256,
            notes = 2048,
            messages = 4096,
            wall = 8192,
            docs = 131072
        }
        public static int scope = (int)(VkontakteScopeList.audio |
            VkontakteScopeList.docs |
            VkontakteScopeList.friends |
            VkontakteScopeList.link |
            VkontakteScopeList.messages |
            VkontakteScopeList.notes |
            VkontakteScopeList.notify |
            VkontakteScopeList.offers |
            VkontakteScopeList.pages |
            VkontakteScopeList.photos |
            VkontakteScopeList.questions |
            VkontakteScopeList.video |
            VkontakteScopeList.wall);

        public int get_app_id {
            get {
                return app_id;
            }
        }
        public int get_scope {
            get {
                return scope;
            }
        }


        public void parseInformation() {

            this.users = new List<User>();
            this.userFriends = new List<string>();
            this.userSubFriends = new List<List<string>>();


            this.users = VkApiHolder.Api.Friends.Get(new FriendsGetParams {
                UserId = AuthorizatedInfo.userId,
                 Order = FriendsOrder.Hints,
                Fields = (ProfileFields) (ProfileFields.All)
               /* Fields = (ProfileFields) (ProfileFields.FirstName| 
                                            ProfileFields.LastName)*/
         
            }).ToList<User>();


            this.userSubFriends = new List<List<string>>();
            for (int i = 0; i < this.users.Count; i++)
            {
                this.userSubFriends.Add(new List<string>());
            }
            foreach (User user in users) {
                this.userSubFriends[users.IndexOf(user)] = new List<string>();
                List<User> cur_user = new List<User>();
                try
                {
                    cur_user = VkApiHolder.Api.Friends.Get(new FriendsGetParams
                    {
                        UserId = user.Id,
                        Order = FriendsOrder.Hints,
                        Fields = (ProfileFields)(ProfileFields.FirstName |
                                                  ProfileFields.LastName)

                    }).ToList<User>();
                }
                catch (Exception ex)
                {
                 
                }
                this.userFriends.Add(user.FirstName + " " +  
                                     user.LastName );
                foreach (User _user in cur_user)
                {
                    this.userSubFriends[this.userFriends.IndexOf(user.FirstName + " " +
                                                                 user.LastName)].Add(_user.FirstName + " " +
                                                                                     _user.LastName);
                }
            }
        }
    }
}
