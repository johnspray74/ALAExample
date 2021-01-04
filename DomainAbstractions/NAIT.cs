using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using ProgrammingParadigms;
using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using DataLink_ALA.NaitService;
using System.IO;

namespace DomainAbstractions
{
    /// <summary>
    /// [DEPRECATED]
    /// </summary>

    // Create classes that are serializable to XML and in the appropriate format for NAIT (as per their XSD schema)
    #region RegisterAnimalsRequest
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://AnimalTraceCSI.nait.co.nz/Schema/2011/11/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://AnimalTraceCSI.nait.co.nz/Schema/2011/11/", IsNullable = false)]
    [DataContract(Namespace = "http://AnimalTraceCSI.nait.co.nz/Schema/2011/11/")]
    public partial class RegisterAnimalsRequest
    {

        private Animal[] animalsField;

        [DataMember]
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute(Namespace = "http://AnimalTraceCSI.nait.co.nz/Schema/2011/11/Common", IsNullable = false)]
        public Animal[] Animals
        {
            get
            {
                return this.animalsField;
            }
            set
            {
                this.animalsField = value;
            }
        }
    }
    #endregion

    #region MovementUploadRequest
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://AnimalTraceCSI.nait.co.nz/Schema/2011/11/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://AnimalTraceCSI.nait.co.nz/Schema/2011/11/", IsNullable = false)]
    [DataContract(Namespace = "http://AnimalTraceCSI.nait.co.nz/Schema/2011/11/")]
    public partial class MovementUploadRequest
    {

        private Movement[] movementsField;
        [DataMember]
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute(Namespace = "http://AnimalTraceCSI.nait.co.nz/Schema/2011/11/Common", IsNullable = false)]
        public Movement[] Movements
        {
            get
            {
                return this.movementsField;
            }
            set
            {
                this.movementsField = value;
            }
        }
    }
    #endregion

    #region Animal
    /// <summary>
    /// [DataMember(Order = 0)] and other similars specify the order of properties to be sent to NAIT, the sequence must follow NAIT example
    /// if use NAIT schema to regenerate this class, need to manually put these Orders back
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://AnimalTraceCSI.nait.co.nz/Schema/2011/11/Common")]
    [DataContract(Namespace = "http://AnimalTraceCSI.nait.co.nz/Schema/2011/11/Common")]
    public partial class Animal
    {

        private string externalRecordReferenceField;

        private string animalTypeField;

        private string speciesTypeField;

        private string productionTypeField;

        private string dateOfBirthField;

        private string nAITNumberAtBirthField;

        private System.Nullable<int> nAITNumberAtFirstRegistrationField;

        private bool nAITNumberAtFirstRegistrationFieldSpecified;

        private string nAITRFIDField;

        private string nAITVisualTagIDField;

        private string[] otherAnimalIdentifiersField;

        private string methodOfDisposalField;

        private bool genderFieldSpecified;

        private System.Nullable<System.DateTime> exportDateField;

        private bool exportDateFieldSpecified;

        // [DataMember]
        /// <remarks/>
        public string ExternalRecordReference
        {
            get
            {
                return this.externalRecordReferenceField;
            }
            set
            {
                this.externalRecordReferenceField = value;
            }
        }

        [DataMember(Order = 0)]
        public string AnimalType
        {
            get
            {
                return this.animalTypeField;
            }
            set
            {
                this.animalTypeField = value;
            }
        }

        [DataMember(Order = 1, IsRequired = false)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string SpeciesType
        {
            get
            {
                return this.speciesTypeField;
            }
            set
            {
                this.speciesTypeField = value;
            }
        }

        /// <remarks/>
        [DataMember(Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string ProductionType
        {
            get
            {
                return this.productionTypeField;
            }
            set
            {
                this.productionTypeField = value;
            }
        }

        [DataMember(Order = 3)]
        public string DateOfBirth
        {
            get
            {
                return this.dateOfBirthField;
            }
            set
            {
                this.dateOfBirthField = value;
            }
        }

        [DataMember(Order = 4)]
        public string NAITNumberAtBirth
        {
            get
            {
                return this.nAITNumberAtBirthField;
            }
            set
            {
                this.nAITNumberAtBirthField = value;
            }
        }

        [DataMember(Order = 5)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<int> NAITNumberAtFirstRegistration
        {
            get
            {
                return this.nAITNumberAtFirstRegistrationField;
            }
            set
            {
                this.nAITNumberAtFirstRegistrationField = value;
            }
        }

        // [DataMember]
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool NAITNumberAtFirstRegistrationSpecified
        {
            get
            {
                return this.nAITNumberAtFirstRegistrationFieldSpecified;
            }
            set
            {
                this.nAITNumberAtFirstRegistrationFieldSpecified = value;
            }
        }

        [DataMember(Order = 6)]
        public string NAITRFID
        {
            get
            {
                return this.nAITRFIDField;
            }
            set
            {
                this.nAITRFIDField = value;
            }
        }

        //[DataMember]
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string NAITVisualTagID
        {
            get
            {
                return this.nAITVisualTagIDField;
            }
            set
            {
                this.nAITVisualTagIDField = value;
            }
        }

        //[DataMember]
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("OtherAnimalIdentifier", IsNullable = false)]
        public string[] OtherAnimalIdentifiers
        {
            get
            {
                return this.otherAnimalIdentifiersField;
            }
            set
            {
                this.otherAnimalIdentifiersField = value;
            }
        }

        //[DataMember]
        /// <remarks/>
        public string MethodOfDisposal
        {
            get
            {
                return this.methodOfDisposalField;
            }
            set
            {
                this.methodOfDisposalField = value;
            }
        }


        // [DataMember]
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool GenderSpecified
        {
            get
            {
                return this.genderFieldSpecified;
            }
            set
            {
                this.genderFieldSpecified = value;
            }
        }

        //[DataMember]
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date", IsNullable = true)]
        public System.Nullable<System.DateTime> ExportDate
        {
            get
            {
                return this.exportDateField;
            }
            set
            {
                this.exportDateField = value;
            }
        }

        //[DataMember]
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ExportDateSpecified
        {
            get
            {
                return this.exportDateFieldSpecified;
            }
            set
            {
                this.exportDateFieldSpecified = value;
            }
        }
    }
    #endregion

    #region Movement
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://AnimalTraceCSI.nait.co.nz/Schema/2011/11/Common")]
    [DataContract(Namespace = "http://AnimalTraceCSI.nait.co.nz/Schema/2011/11/Common")]
    public partial class Movement
    {

        private string externalRecordReferenceField;

        private string nAITRFIDField;

        private string nAITVisualTagIDField;

        private string sendingNAITNumberField;

        private string receivingNAITNumberField;

        private string dateSentField;

        private bool dateSentFieldSpecified;

        private string dateReceivedField;

        private bool dateReceivedFieldSpecified;
        /// <remarks/>
        //[DataMember(Order = 0)]
        public string ExternalRecordReference
        {
            get
            {
                return this.externalRecordReferenceField;
            }
            set
            {
                this.externalRecordReferenceField = value;
            }
        }
        [DataMember(Order = 1)]
        /// <remarks/>
        public string NAITRFID
        {
            get
            {
                return this.nAITRFIDField;
            }
            set
            {
                this.nAITRFIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string NAITVisualTagID
        {
            get
            {
                return this.nAITVisualTagIDField;
            }
            set
            {
                this.nAITVisualTagIDField = value;
            }
        }
        [DataMember(Order = 2)]
        /// <remarks/>
        public string SendingNAITNumber
        {
            get
            {
                return this.sendingNAITNumberField;
            }
            set
            {
                this.sendingNAITNumberField = value;
            }
        }
        [DataMember(Order = 3)]
        /// <remarks/>
        public string ReceivingNAITNumber
        {
            get
            {
                return this.receivingNAITNumberField;
            }
            set
            {
                this.receivingNAITNumberField = value;
            }
        }
        [DataMember(Order = 4, EmitDefaultValue = false)]
        /// <remarks/>
        //[System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public string DateSent
        {
            get
            {
                return this.dateSentField;
            }
            set
            {
                this.dateSentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DateSentSpecified
        {
            get
            {
                return this.dateSentFieldSpecified;
            }
            set
            {
                this.dateSentFieldSpecified = value;
            }
        }
        [DataMember(Order = 5, EmitDefaultValue = false)]
        /// <remarks/>
        //[System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public string DateReceived
        {
            get
            {
                return this.dateReceivedField;
            }
            set
            {
                this.dateReceivedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DateReceivedSpecified
        {
            get
            {
                return this.dateReceivedFieldSpecified;
            }
            set
            {
                this.dateReceivedFieldSpecified = value;
            }
        }
    }
    #endregion
    public class NAIT : IEvent, IDataFlow<string>
    {
        // Properties
        public string InstanceName = "Default";

        // Private fields
        private ExternalClient client;
        private string requestType = "None";

        // Outputs
        private IDataFlowB<ExternalClient> clientInput;
        private IDataFlowB<string> naitUsernameInput;
        private IDataFlowB<string> naitPasswordInput;
        private IDataFlowB<string> naitNumberInput;
        private IDataFlowB<string> naitSendingNumberInput;
        private IDataFlowB<string> naitReceivingNumberInput;
        private IDataFlowB<string> dateOfBirthInput;
        private IDataFlowB<string> dateSentInput;
        private IDataFlowB<string> dateReceivedInput;
        private IDataFlowB<string> naitAnimalTypeInput;
        private IDataFlowB<string> naitProductionTypeInput;
        private IDataFlowB<string> naitCattleProductionTypeInput;
        private IDataFlowB<string> naitDeerProductionTypeInput;
        private IDataFlowB<string> naitIDFilePath;
        private IDataFlow<string> naitIDFilePathOutput;
        private IDataFlowB<List<string>> eidList;

        private IEvent naitRequestCompleted;

        public NAIT() { }

        private void WriteUserInfoToFile(Dictionary<string, string> userInfoDict, string filePath = @"C:\ProgramData\Tru-Test\DataLink_ALA\", string fileName = "naitUserInfo.json")
        {
            using (StreamWriter file = File.CreateText(filePath + fileName))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, userInfoDict);
            }
        }

        // IEvent implementation
        void IEvent.Execute()
        {
            client = clientInput.Data;

            #region Register
            if (requestType == "Animal Registration") // RegisterAnimalsRequest
            {
                //var requestMessage = File.ReadAllText(@"C:\Projects\AMDA Docs\NAIT\Test Files\SOAP_requestMessage_registerAnimal_1.xml");
                //var naitResponse = client.Operation(requestMessage);

                //var messageBodyContainer = new Animal()
                //{
                //    DateOfBirth = "unknown",
                //    NAITNumberAtBirth = "6606",
                //    NAITNumberAtFirstRegistration = 6606,
                //    NAITNumberAtFirstRegistrationSpecified = true,
                //    AnimalType = "C",
                //    SpeciesType = "Sheep", // Can't set value to null within an anonymous class
                //    ProductionType = "D",
                //    NAITRFID = "982123458445781",

                //};

                RegisterAnimalsRequest registerRequest = new RegisterAnimalsRequest();
                var eids = eidList.Data;
                if (eids == null) return; // temp solution, should rewire event flow to avoid this
                int count = eids.Count();
                registerRequest.Animals = new Animal[count];
                string animalType;
                string productionType = "";
                for (int i = 0; i < count; i++)
                {
                    // Handle NAIT schema, may need to transfer the config to a separate abstraction
                    productionType = naitProductionTypeInput.Data;
                    animalType = naitAnimalTypeInput.Data;
                    //animalType = naitAnimalTypeInput.Data[0].ToString();
                    //if (animalType == "C")
                    //{
                    //    productionType = naitCattleProductionTypeInput.Data[0].ToString(); // (D)airy, (B)eef, or (N)one
                    //}
                    //else if (animalType == "D")
                    //{
                    //    productionType = naitDeerProductionTypeInput.Data;
                    //    if (productionType == "Venison") productionType = "VE";
                    //    if (productionType == "Velvet") productionType = "VT";
                    //    if (productionType == "Trophy") productionType = "TR";
                    //    if (productionType == "None") productionType = "N";
                    //}

                    Animal animal = new Animal()
                    {
                        DateOfBirth = dateOfBirthInput.Data,
                        NAITNumberAtBirth = naitNumberInput.Data,
                        //NAITNumberAtFirstRegistration = 6606,
                        NAITNumberAtFirstRegistrationSpecified = false,
                        AnimalType = animalType,
                        //SpeciesType = "", // for deer only
                        ProductionType = productionType,
                        NAITRFID = eids[i],
                    };
                    registerRequest.Animals[i] = animal;
                }

                var messageBodyContainer = registerRequest;

                //var messageBody = JsonConvert.SerializeObject(messageBodyContainer);
                XmlSerializer x = new XmlSerializer(messageBodyContainer.GetType());
                var messageBody = new System.IO.StringWriter();

                x.Serialize(messageBody, messageBodyContainer);
                string msg = messageBody.ToString();
                Debug.WriteLine(msg);
                const string naitNameSpace = "http://AnimalTraceCSI.nait.co.nz/Schema/2011/11/Common";

                string naitUsername = naitUsernameInput.Data;
                string naitPassword = naitPasswordInput.Data;
                string naitNumber = naitNumberInput.Data;
                Dictionary<string, string> userInfoDict = new Dictionary<string, string>() { { "naitUsername", naitUsername }, { "naitPassword", naitPassword }, { "naitNumber", naitNumber } };
                WriteUserInfoToFile(userInfoDict);

                System.ServiceModel.Channels.Message requestMessage = System.ServiceModel.Channels.Message.CreateMessage(System.ServiceModel.Channels.MessageVersion.Soap11, "RegisterAnimals", messageBodyContainer);
                requestMessage.Headers.Add(System.ServiceModel.Channels.MessageHeader.CreateHeader("UserName", naitNameSpace, naitUsername));
                requestMessage.Headers.Add(System.ServiceModel.Channels.MessageHeader.CreateHeader("Password", naitNameSpace, naitPassword));

                var requestString = requestMessage.ToString();
                var naitResponse = client.Operation(requestMessage);
            }
            #endregion

            #region MovementUpload
            if (requestType == "Sending Movement" || requestType == "Receiving Movement") // MovementUpload, both sending and receiving
            {
                MovementUploadRequest moveUploadRequest = new MovementUploadRequest();
                var eids = eidList.Data;
                if (eids == null) return; // temp solution, should rewire event flow to avoid this
                int count = eids.Count();
                moveUploadRequest.Movements = new Movement[count];

                for (int i = 0; i < count; i++)
                {
                    Movement movement = new Movement
                    {
                        //ExternalRecordReference = string.Empty,
                        //NAITVisualTagID = string.Empty,
                        NAITRFID = eids[i],
                        //NAITRFID = "982123458445781",
                        SendingNAITNumber = naitSendingNumberInput.Data, //naitSendingNumberInput.Data,
                        ReceivingNAITNumber = naitReceivingNumberInput.Data,
                    };
                    if (requestType == "Sending Movement") movement.DateSent = dateSentInput.Data;
                    if (requestType == "Receiving Movement") movement.DateReceived = dateReceivedInput.Data;
                    moveUploadRequest.Movements[i] = movement;
                }
                
                var messageBodyContainer = moveUploadRequest;

                XmlSerializer x = new XmlSerializer(messageBodyContainer.GetType());
                var messageBody = new System.IO.StringWriter();

                x.Serialize(messageBody, messageBodyContainer);
                string msg = messageBody.ToString();
                Debug.WriteLine(msg);

                const string naitNameSpace = "http://AnimalTraceCSI.nait.co.nz/Schema/2011/11/Common";

                string naitUsername = naitUsernameInput.Data;
                string naitPassword = naitPasswordInput.Data;
                string naitNumber = naitNumberInput.Data;
                Dictionary<string, string> userInfoDict = new Dictionary<string, string>() { { "naitUsername", naitUsername }, { "naitPassword", naitPassword }, { "naitNumber", naitNumber } };
                WriteUserInfoToFile(userInfoDict);

                System.ServiceModel.Channels.Message requestMessage = System.ServiceModel.Channels.Message.CreateMessage(System.ServiceModel.Channels.MessageVersion.Soap11, "MovementUpload", messageBodyContainer);
                requestMessage.Headers.Add(System.ServiceModel.Channels.MessageHeader.CreateHeader("UserName", naitNameSpace, naitUsername));
                requestMessage.Headers.Add(System.ServiceModel.Channels.MessageHeader.CreateHeader("Password", naitNameSpace, naitPassword));

                var requestString = requestMessage.ToString();
                var naitResponse = client.Operation(requestMessage);
            }
            #endregion
            naitRequestCompleted?.Execute();
        }

        // IDataFlow<string> implementation
        string IDataFlow<string>.Data // Get the option selected by the user
        {
            set
            {
                requestType = value;
            }
        }
    }
}
