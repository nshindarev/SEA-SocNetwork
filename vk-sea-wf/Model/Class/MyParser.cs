﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using vk_sea_wf.Model.Interfaces;
using xNet;

namespace vk_sea_wf.Model.Class
{
      class MyParser : IParse {
        public static string api_url = "https://api.vk.com/";
        public static int app_id = 5677623;
        public string version = "5.60";


        public string order = "hints";
        public IList<VkUser> user_friends { get; set; }


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
            HttpRequest myreq = new HttpRequest();

            //TODO: убрать!!!
            String s = AuthorizatedInfo.access_token;
            int n = AuthorizatedInfo.userId;
            //Авторизация
            myreq.AddUrlParam("access_token", AuthorizatedInfo.access_token);
            myreq.AddUrlParam("v", version);
            myreq.AddUrlParam("user_id", AuthorizatedInfo.userId);
            myreq.AddUrlParam("order", order);
            myreq.AddUrlParam("fields", "name");
            //Добавить необходимые поля
            try {
                string uri = api_url + "friends.get";
                string rez = myreq.Get(api_url + "friends.get/").ToString();
               
                // (JSON) string -> VkUser
                ResponseWrap rw = JsonConvert.DeserializeObject<ResponseWrap>(rez);
                this.user_friends = rw.response.items;
            }
            catch (HttpException ex) {
                MessageBox.Show(ex.ToString());
            }
         
           /* MessageBox.Show("Response: " + rez +
                            Environment.NewLine + "First friend's name: " + this.parser.response[0].first_name); */
        }
    }
}
