using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kaliido.QuickBlox;
using RestSharp;
using Kaliido.QuickBloxDotNet.Api.Enums;
using System.Threading.Tasks;
using Kaliido.QuickBlox.Parameters;
using Kaliido.QuickBlox.Models;
using System.Net;
using Newtonsoft.Json.Linq;

namespace BLL.Masters
{
    public class QuickbloxRestApi : IQuickBloxClient
    {
        public string BaseUrl = "https://api.quickblox.com";
        public ApiCredential Credentials { get; set; }
        public UserCredential UserCredentials { get; set; }
        public IRestClient Client { get; set; }
        public string QBToken { get; set; }


        public QuickbloxRestApi(ApiCredential credentials, UserCredential adminUserCredential, string baseUrl = "https://api.quickblox.com")
        {
            Credentials = credentials;
            UserCredentials = adminUserCredential;
            Client = new RestSharp.RestClient(baseUrl);
        }



        public QuickbloxRestApi(ApiCredential credentials, string baseUrl = "https://api.quickblox.com")
        {
            Credentials = credentials;
            Client = new RestSharp.RestClient(baseUrl);
        }

        public QuickbloxRestApi()
        {

            Client = new RestSharp.RestClient(BaseUrl);
        }



        public void ValidateBeforeRequest(RequestValidationType tokenType)
        {

            if (this.Credentials == null)
            {
                throw new ArgumentNullException("You must set the Quickblox API Credentials Object to the QuickBloxClient.");
            }
            else
            {


                if (String.IsNullOrEmpty(this.Credentials.ApplicationID) || String.IsNullOrEmpty(this.Credentials.AuthKey) || String.IsNullOrEmpty(this.Credentials.AuthSecret))
                {
                    throw new ArgumentNullException("The Application Credentials must be set in the Quick Blox Client, you require ApplicationID, AuthKey & AuthSecret");
                }

            }

            if (tokenType == RequestValidationType.QuickBloxUserToken)
            {

                if (this.UserCredentials == null || String.IsNullOrEmpty(UserCredentials.UserLogin) || String.IsNullOrEmpty(UserCredentials.Password))
                {
                    throw new ArgumentNullException("The request is an quickblox user orientated request.\n This kind of request requires you to enter login credentials as either the Admin user, or a user of the quickblox app you are trying to use.");
                }
            }
        }

        public string GetQBToken()
        {
            var nonce = GlobalHelper.getNonce();
            var timeStamp = GlobalHelper.getTimestamp();

            var request = new RestRequest(Method.POST);
            request.Resource = "session.json";
            request.AddParameter("application_id", Credentials.ApplicationID);
            request.AddParameter("auth_key", Credentials.AuthKey);
            request.AddParameter("nonce", nonce);
            request.AddParameter("timestamp", timeStamp);



            var postData = new StringBuilder();
            postData.AppendFormat("application_id={0}", Credentials.ApplicationID);
            postData.AppendFormat("&auth_key={0}", Credentials.AuthKey);
            postData.AppendFormat("&nonce={0}", nonce);
            postData.AppendFormat("&timestamp={0}", timeStamp);
            //postData.AppendFormat("&user[login]={0}", UserCredentials.UserLogin);
            //postData.AppendFormat("&user[password]={0}", UserCredentials.Password);


            var signature = GlobalHelper.getHash(postData.ToString(), Credentials.AuthSecret).ByteToString();


            request.AddParameter("signature", signature);

            //request.AddParameter("user[login]", UserCredentials.UserLogin);
            //request.AddParameter("user[password]", UserCredentials.Password);
            request.AddHeader("QuickBlox-REST-API-Version", "0.1.0");
            request.RequestFormat = RestSharp.DataFormat.Json;

            //var a = await Client.ExecuteTaskAsync<Token>(request);
            IRestResponse a = Client.Execute(request);


            if (a.ResponseStatus == ResponseStatus.Completed && a.ErrorMessage == null)
            {
                JObject Jobj = JObject.Parse(a.Content);
                string Token = Jobj["session"]["token"].ToString();
                return Token;

            }
            else
            {
                throw a.ErrorException;
            }
        }

        public string RegisterAsync(UserParameters userParameters)
        {
            var request = new RestRequest("users.json", Method.POST)
                          {
                              RequestFormat = DataFormat.Json,
                              JsonSerializer = new NewtonsoftSerializer()
                          };
            request.AddBody(new { user = userParameters });

            PrepareRequestForQuickblox(request);

            //var result = await Client.ExecutePostTaskAsync<UserResponse>(request);

            IRestResponse result = Client.Execute(request);

            if (result.ResponseStatus != ResponseStatus.Completed)
            {
                return null;
            }
            if (result.StatusCode == HttpStatusCode.Created)
            {
                JObject Jobj = JObject.Parse(result.Content);
                string Blob_Id = Jobj["user"]["id"].ToString();
                return Blob_Id;
            }
            return null;
        }



        private Task PrepareRequestForQuickblox(RestRequest request)
        {
            ValidateBeforeRequest(RequestValidationType.QuickBloxUserToken);
            request.RequestFormat = DataFormat.Json;
            request.JsonSerializer = new NewtonsoftSerializer();
            request.AddHeader("QuickBlox-REST-API-Version", "0.1.0");
            string token = GetQBToken().ToString();
            request.AddHeader("QB-Token", token);

            return null;
        }


    }
}
