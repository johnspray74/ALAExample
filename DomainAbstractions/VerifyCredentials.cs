using System;
using System.Collections.Generic;
using System.Linq;
using ProgrammingParadigms;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;

namespace DomainAbstractions
{
    public class VerifyCredentials : IEvent
    {
        /// <summary>
        /// [DEPRECATED]
        /// Verifies whether locally stored access and refresh tokens (if there are any) are valid, or if a login is required.
        /// Mode local: read tokens from a local json file
        /// Mode form: read user and pass from login form
        /// </summary>

        // Outputs:
        private IEvent CredentialsHaveBeenChecked;
        private IDataFlow<bool> CredentialsAreValid;
        private IDataFlowB<string> usernameInput;
        private IDataFlowB<string> passwordInput;

        // Private fields:
        private string filePath = @"C:\ProgramData\Tru-Test\DataLink_ALA\" + "userinfo.json";
        private HttpClient client = new HttpClient();
        private string loginURI = "https://livestock.mihub.tru-test.com/jwt/login";

        // Public fields:
        public string InstanceName = "Default";
        public string Mode = "local";

        public VerifyCredentials() { }

        private async void ValidateRefreshTokenWithMiHubAsync(string refreshToken)
        {
            /// <summary>
            /// Use the refreshToken to request a new access token from MiHub.
            /// </summary>

            var content = new Dictionary<string, string>()
            {
                { "grant_type" , "refresh_token" },
                { "refresh_token" , refreshToken }
            };

            var response = await client.PostAsync(loginURI, new FormUrlEncodedContent(content));
            string responseString = await response.Content.ReadAsStringAsync();
            var jsonResponseDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseString);
            jsonResponseDict["refresh_token"] = refreshToken;


            if (jsonResponseDict.ContainsKey("error"))
            {
                CredentialsAreValid.Data = false;
            }
            else
            {
                WriteUserInfoToFile(jsonResponseDict);
                CredentialsAreValid.Data = true;
                Debug.WriteLine("Local user credentials have been validated.");
                CredentialsHaveBeenChecked.Execute();
            }
        }

        private async void ValidateUserAndPassWithMiHubAsync(string username, string password)
        {
            /// <summary>
            /// Uses the username and password to request a new access and refresh token from MiHub and saves the retrieved tokens locally.
            /// </summary>

            var content = new Dictionary<string, string>()
                {
                    { "grant_type" , "password" },
                    { "username" , username },
                    { "password" , password }
                };

            var response = await client.PostAsync(loginURI, new FormUrlEncodedContent(content));
            string responseString = await response.Content.ReadAsStringAsync();
            var jsonResponseDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseString);

            if (jsonResponseDict.ContainsKey("error"))
            {
                CredentialsAreValid.Data = false;
            }
            else
            {
                WriteUserInfoToFile(jsonResponseDict);
                Debug.WriteLine("Login form user credentials have been validated.");
                CredentialsAreValid.Data = true;
                CredentialsHaveBeenChecked.Execute();
            }
        }

        private void WriteUserInfoToFile(Dictionary<string, string> jsonResponseDict)
        {
            using (StreamWriter file = File.CreateText(filePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, jsonResponseDict);
            }
        }

        // IEvent implementation
        void IEvent.Execute()
        {
            if (Mode == "local")
            {
                if (!File.Exists(filePath))
                {
                    CredentialsAreValid.Data = false;
                    CredentialsHaveBeenChecked.Execute();
                    return;
                }

                // Read stored data to check that it contains valid user info
                var jsonBuffer = File.ReadAllText(filePath);
                if (jsonBuffer == "")
                {
                    CredentialsAreValid.Data = false;
                    CredentialsHaveBeenChecked.Execute();
                    return;
                }
                Dictionary<string, string> jsonResponseDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonBuffer);

                if (jsonResponseDict.ContainsKey("error"))
                {
                    CredentialsAreValid.Data = false;
                    CredentialsHaveBeenChecked.Execute();
                    return;
                }

                // Finally, if the credentials are still not invalidated, validate them asynchronously with MiHub.
                ValidateRefreshTokenWithMiHubAsync(jsonResponseDict["refresh_token"]);
            }
            else if (Mode == "form")
            {
                ValidateUserAndPassWithMiHubAsync(usernameInput.Data, passwordInput.Data);
            }
        }
    }
}
