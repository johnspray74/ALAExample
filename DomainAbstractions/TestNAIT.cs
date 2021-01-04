using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using ProgrammingParadigms;
using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace DomainAbstractions
{
    /// <summary>
    /// [DEPRECATED]
    /// </summary>
    public class TestNAIT : IEvent
    {
        public enum IntegrationTypes { NAIT = 2, NLIS = 3 } /* An excerpt from the backend list which is bigger. We do not add other unimplemented types to not introduce any confusion. */
        public enum AnimalMovementTypes
        {
            Unknown = 0,
            NaitRegistration = 201,
            NaitDeRegistration = 202,
            NaitReceive = 203,
            NaitSend = 204,
            NlisProducer = 301,
            NlisAgent = 302,
            NlisSaleYardIn = 303,
            NlisSaleYardOut = 304,
            NlisThirdParty = 305
        }

        public class AnimalTransferModel
        {
            public IntegrationTypes IntegrationType { get; set; }

            public AnimalMovementTypes AnimalMovementType { get; set; }

            public string ClientKey { get; set; }

            public DateTime? MovementDate { get; set; }

            public string OtherClientKey { get; set; }

            public List<string> ExternalAnimalIds { get; set; }
            public string JsonPayload { get; set; }

            public string Username { get; set; }

            public string Password { get; set; }

            public string DataLinkDeviceId { get; set; } = null;

            public int? FarmId { get; set; }

            public bool? IsStaging { get; set; } = null; // if not null, will force to point to NAIT/NLIS staging or production


        }

        public class TransferRequest
        {
            public bool Equals(TransferRequest other)
            {
                return false;
            }

            public override bool Equals(object other)
            {
                return other != null && other is TransferRequest && Equals(other as TransferRequest);
            }
            public override int GetHashCode()
            {
                return 1;
            }

        }

        public class NaitRequest : TransferRequest
        {
            public string NaitNumberClient { get; set; }

            public string NaitNumberOther { get; set; }

            public DateTime? DateOfBirth { get; set; }

            public DateTime DateSendOrReceive { get; set; }

            public string AnimalType { get; set; }

            public string SpeciesType { get; set; }

            public string ProductionType { get; set; }

        }

        public TestNAIT()
        {

        }

        

        private void ConnectToNait()
        {

            
        }

        public async Task NaitUploadAsync()
        {
            NaitRequest naitRequest = new NaitRequest
            {
                AnimalType = "C",
                ProductionType = "D",
                NaitNumberClient = "6606",
                NaitNumberOther = "6590"
            };

            string jsonPayload = JsonConvert.SerializeObject(naitRequest);

            AnimalTransferModel model = new AnimalTransferModel
            {
                JsonPayload = jsonPayload,
                IntegrationType = IntegrationTypes.NAIT,
                AnimalMovementType = AnimalMovementTypes.NaitSend,
                ClientKey = "6606",
                DataLinkDeviceId = "dads",//CrossDeviceInfo.Current.Id,
                MovementDate = DateTime.UtcNow,
                OtherClientKey = "7030",
                ExternalAnimalIds = new List<string> { "982 100000000129" },
                Username = "trutest",
                Password = "trutest123456",
                IsStaging = true
            };

            string url = "https://staging.livestock.mihub.tru-test.com/api/AnimalMovement";
            var request = new HttpRequestMessage(HttpMethod.Post, url);

            string json = JsonConvert.SerializeObject(model);
            request.Content = new System.Net.Http.StringContent(json, Encoding.UTF8, "application/json");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.UserAgent.ParseAdd($"User-Agent UserAgent");

            HttpResponseMessage response = await client.SendAsync(request);

            var responseJson = await response.Content.ReadAsStringAsync();

        }

        // IEvent implementation
        void IEvent.Execute()
        {
            var fireAndForget = NaitUploadAsync();
        }
    }
}
