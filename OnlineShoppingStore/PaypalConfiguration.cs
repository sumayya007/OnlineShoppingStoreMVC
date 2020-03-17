using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShoppingStore
{
    public class PaypalConfiguration
    {
        //Variables for storing the clientID and clientSecret key
        public readonly static string ClientId;
        public readonly static string ClientSecret;
        //Constructor
        static PaypalConfiguration()
        {
            var config = GetConfig();
            ClientId = "AUjWDMV_a8hD_4M8zdCMmvVL-U-jk3lxEi7tyQWgwoQN7PDjpNYexr5U-swbHkjWIO_BxnY_ye97ZFBH";
            ClientSecret = "EFAl7vBFbJCOFL_ztJxkS3opRMnBFwyCReSX6g-kRjvU3s52FuHM-L4GBtfqDz5TEGQX_QvL1Bu4ZnYK";
        }
        // getting properties from the web.config
        public static Dictionary<string, string> GetConfig()
        {
            return PayPal.Api.ConfigManager.Instance.GetProperties();
        }
        private static string GetAccessToken()
        {
            // getting accesstocken from paypal
            string accessToken = new OAuthTokenCredential(ClientId, ClientSecret, GetConfig()).GetAccessToken();
            return accessToken;
        }
        public static APIContext GetAPIContext()
        {
            // return apicontext object by invoking it with the accesstoken
            APIContext apiContext = new APIContext(GetAccessToken());
            apiContext.Config = GetConfig();
            return apiContext;
        }
    }
}