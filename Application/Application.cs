using System.Windows.Media;
using System.Windows;
using System.Data;
using System.Globalization;
using DomainAbstractions;
using ProgrammingParadigms;
using Libraries;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using DataLink_ALA.DomainAbstractions;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;

namespace Application
{
    /// <summary>
    /// Rules for writing Application Wiring referring to XMIND (Requires new rules - Note:11/12/2020)
    /// ------------------------------------------------------------------------------------------------------------------
    /// 1. Indentation structure should generally follow the blue line tree structure in XMind 
    /// 2. Blue lines are straight main tree lines going from left to right and are default WireTo.
    /// 3. When referring to diagram, ports should be in order from top to bottom
    /// 4. Red arrow lines should only reference that instance but not follow through their blue lines
    /// 5. Exception of a green arrow in XMind which means a .WireFrom is used at that point
    /// 6. Specifying what ports have already be wired on the Wiring e.g. // Port: listOfPorts & selectedComPort are already wired earlier
    /// 7. Specify portnames for implemented interfaces even if those names are never used as variables within the instance
    /// </summary>
    public class Application
    {
        private MainWindow mainWindow = new MainWindow("Datalink---ALA", logArchiveFilePath: CURRENT_LOG_ARCHIVE_FILEPATH);
        private string VERSION_NUMBER = Libraries.Constants.DataLinkPCVersionNumber;
        private static bool VERBOSE_LOG = true;

        [STAThread]
        public static void Main()
        {
            if (!System.IO.Directory.Exists(DEFAULT_DIRECTORY)) System.IO.Directory.CreateDirectory(DEFAULT_DIRECTORY);
            if (!System.IO.Directory.Exists(DEFAULT_DIRECTORY + "Logs\\")) System.IO.Directory.CreateDirectory(DEFAULT_DIRECTORY + "Logs\\");
            if (!System.IO.Directory.Exists(DEFAULT_DIRECTORY + "Updates\\")) System.IO.Directory.CreateDirectory(DEFAULT_DIRECTORY + "Updates\\");
            if (!File.Exists(@"C:\ProgramData\Tru-Test\DataLink_ALA\userinfo.json")) File.WriteAllText(@"C:\ProgramData\Tru-Test\DataLink_ALA\userinfo.json", "");
            if (!File.Exists(@"C:\ProgramData\Tru-Test\DataLink_ALA\usersettings.json")) File.WriteAllText(@"C:\ProgramData\Tru-Test\DataLink_ALA\usersettings.json", "{}");
            Logging.WriteText(path: Logging.wiringLogFilePath, content: "", createNewFile: true); // Create empty wiring log
            Logging.WriteText(path: Logging.logFilePath, content: "", createNewFile: true); // Create empty exception log
            AppDomain.CurrentDomain.FirstChanceException += (o, args) => CurrentDomain_FirstChanceException(o, args);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException; 
            var application = new Application();
            application.Initialize().mainWindow.Run();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;
            if (exception != null)
            {
                Logging.Log(exception);
            }
            else
            {
                Logging.Log(e.ExceptionObject);
            }

            File.Copy(Logging.logFilePath, CURRENT_LOG_ARCHIVE_FILEPATH); // Archive current log when app shuts down unexpectedly
        }

        [Conditional("DEBUG")]
        private static void CurrentDomain_FirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
        {
            if (VERBOSE_LOG)
            {
                Logging.Log(e.Exception);
            }

        }

        private Application Initialize()
        {
            Wiring.PostWiringInitialize();
            return this;
        }

        private static string DEFAULT_DIRECTORY = @"C:\ProgramData\Tru-Test\DataLink_ALA\";
        private static string CURRENT_LOG_ARCHIVE_FILEPATH = $"{DEFAULT_DIRECTORY}Logs\\{Logging.GetCurrentTime()}.log";
        private LiteralString defaultDirectoryDataFlow = new LiteralString(DEFAULT_DIRECTORY);

        private Application()
        {
            #region parser-generated instantiations

            #region WebServices INSTANTIATIONS
            // BEGIN AUTO-GENERATED INSTANTIATIONS FOR WebServices
            var id_050da19fe54e4c4ca477b8a6257db32e = new Apply<bool, int>() {InstanceName="Default",Lambda=s =>{    return 305;}}; /*  */
            var id_9dbe1dbf08f54712aae5645365775421 = new Apply<bool, int>() {InstanceName="Default",Lambda=s =>{    return 304;}}; /*  */
            var id_a05d136d60144a8e846b1a43202f2785 = new Apply<bool, int>() {InstanceName="Default",Lambda=s =>{    return 302;}}; /*  */
            var id_a4b61b22030841e7a46e64ad26da520a = new Apply<bool, int>() {InstanceName="Default",Lambda=s =>{    return 301;}}; /*  */
            var id_b245cda27f704651a8cd1babea5456e4 = new Apply<bool, int>() {InstanceName="Default",Lambda=s =>{    return 303;}}; /*  */
            var id_4a80df13ca8d44a299a61642bc942206 = new Apply<bool, JToken>() {InstanceName="Default",Lambda=s => (JToken)new JValue(s)}; /*  */
            var id_46f6ca1e4b0845f68dc62df2a610d679 = new Apply<Dictionary<string, JToken>, bool>() {InstanceName="Default",Lambda=d =>{    return !d.ContainsKey("refresh_token");}}; /*  */
            var id_6ab18e1d288a48009a389d4d520e538c = new Apply<int, JToken>() {InstanceName="Default",Lambda=s => (JToken)new JValue(s)}; /*  */
            var id_aca856078615468b9080a5dc93925a4e = new Apply<JObject, JToken>() {InstanceName="Default",Lambda=s => (JToken)new JValue(JsonConvert.SerializeObject(s))}; /*  */
            var id_8207d0fb24f94de09bd6fa52c6acf46b = new Apply<JObject, string>() {InstanceName="Default",Lambda=s => JsonConvert.SerializeObject(s)}; /*  */
            var id_b54c432fa4e842b9aa14e551595afcd2 = new Apply<JToken, List<JProperty>>() {InstanceName="Default",Lambda=t =>{    var jObject = JObject.FromObject(t);    return jObject.Properties().Select(s => s as JProperty).ToList();}}; /*  */
            var id_ca11332e468b41ceb56d820cc5734240 = new Apply<JToken, List<JProperty>>() {InstanceName="Default",Lambda=t =>{    var jObject = JObject.FromObject(t);    return jObject.Properties().Select(s => s as JProperty).ToList();}}; /*  */
            var id_e2ed6bc6009c463cbc0159cc24cf8d97 = new Apply<List<string>, JToken>() {InstanceName="Default",Lambda=s => (JToken)new JArray(s)}; /*  */
            var addSeparatorToEachEID = new Apply<List<string>, List<string>>() {InstanceName="addSeparatorToEachEID",Lambda=EIDList =>{    var newList = new List<string>();    foreach (var e in EIDList)    {        newList.Add(e.Substring(0, 3) + " " + e.Substring(3));    }    return newList;}}; /*  */
            var id_097c999a3d8a4da5b3ddc3095184ab38 = new Apply<string, bool>() {InstanceName="Default",Lambda=s =>{    return s.StartsWith("Error");}}; /*  */
            var id_18beba2589d94712997b45c2833e7e68 = new Apply<string, Dictionary<string, string>>() {InstanceName="Default",Lambda=s => new Dictionary<string, string>(){{"FarmName", s}}}; /*  */
            var naitIntegrationTypeCodes = new Apply<string, int>() {InstanceName="naitIntegrationTypeCodes",Lambda=s =>{    var d = new Dictionary<string, int>{{"Animal Registration", 201}, {"Sending Movement", 204}, {"Receiving Movement", 203}};    return d[s];}}; /*  */
            var id_c8a23b2380ff4f9bbefe5e5819f61b56 = new Apply<string, JToken>() {InstanceName="Default",Lambda=s => (JToken)new JValue(s)}; /*  */
            var convertMiHubAccessTokenToJson = new Apply<string, string>() {InstanceName="convertMiHubAccessTokenToJson",Lambda=s => Encoding.UTF8.GetString(Convert.FromBase64String(s.Split('.')[1]))}; /*  */
            var id_24bd840429f04f9cafff094b83842ab0 = new AsObject<bool>() {InstanceName="Default"}; /*  */
            var id_bd1f8ae26e3140bea6143d59150977f9 = new AsObject<bool>() {InstanceName="Default"}; /*  */
            var id_22a1f1c6e9b6456b80a529ca738db654 = new AsObject<DateTime>() {InstanceName="Default"}; /*  */
            var id_61de43b3043a49009a2b8a05e231e633 = new AsObject<DateTime>() {InstanceName="Default"}; /*  */
            var id_bae2e4eb948b418b833067670949a658 = new AsObject<DateTime>() {InstanceName="Default"}; /*  */
            var id_e3bd583f2dec45c5b1d37f6c22d9ded2 = new AsObject<DateTime>() {InstanceName="Default"}; /*  */
            var id_e4a000db68af49179abc652bd9980ce4 = new AsObject<DateTime>() {InstanceName="Default"}; /*  */
            var id_3c2467569f454be7b349c67f3a5c1b47 = new AsObject<int>() {InstanceName="Default"}; /*  */
            var id_a0b304fbaeba48888b79b1eb90322ae8 = new AsObject<int>() {InstanceName="Default"}; /*  */
            var id_f5cb5000f71b4506aa729790441d7761 = new AsObject<int>() {InstanceName="Default"}; /*  */
            var id_9917048599714a268c2e1de38c4776a9 = new AsObject<List<string>>() {InstanceName="Default"}; /*  */
            var mihubAutoSave = new AutoSave() {InstanceName="mihubAutoSave"}; /*  */
            var id_080c0f042e094b89a658e2490a8e7e4b = new Button("Skip") {Margin=new Thickness(2),InstanceName="Default"}; /*  */
            var id_19cac47015ae4f95aff82d367aaee21a = new Button("Continue") {InstanceName="Default"}; /*  */
            var id_254c64aeead1453ea551f8f9a514aca8 = new Button("Submit") {InstanceName="Default"}; /*  */
            var id_2a9c4ef4b1314910b09a613d6360fb67 = new Button("OK") {Width=60,Height=35,Margin=new Thickness(5),InstanceName="Default"}; /*  */
            var id_3757afd20b584b3da2b5647b3fb13bbf = new Button("Submit") {Margin=new Thickness(5),InstanceName="Default"}; /*  */
            var id_3b8bb260778f4d73b4d6d804aa07c7d8 = new Button("OK") {Width=100,Height=35,InstanceName="Default"}; /*  */
            var id_3dda425d1d44428888650217f8641170 = new Button("See the terms and conditions") {InstanceName="Default"}; /*  */
            var id_52fa92652fbe4256b8793c74567cb775 = new Button("Log in") {Margin=new Thickness(2),InstanceName="Default"}; /*  */
            var id_57lq4lvm4ject1qhjsafgev6b9 = new Button("OK") {Width=100,Height=35,Margin=new Thickness(5),InstanceName="Default"}; /*  */
            var id_5974aa971d5d493ca0ab489286721eeb = new Button("See the privacy policy") {Margin=new Thickness(5, 0, 0, 0),InstanceName="Default"}; /*  */
            var id_5sk780357aunm4tprle9ni2j8l = new Button("Forgot password?") {Margin=new Thickness(10, 0, 0, 0),InstanceName="Default"}; /*  */
            var id_7rf5s4d07ldvublva5s9cm65nm = new Button("Don't have an account? Sign up here!") {InstanceName="Default"}; /*  */
            var id_80e16a4e1d9b473c8814c0b2e92d279a = new Button("Submit") {InstanceName="Default"}; /*  */
            var id_85c2c7872bd1485a8d44858c08fe8cb9 = new Button("Yes") {Margin=new Thickness(5),InstanceName="Default"}; /*  */
            var id_86a1a47dd7364b68a3ac131c343a9736 = new Button("Sign up for free!") {Margin=new Thickness(2),InstanceName="Default"}; /*  */
            var id_978fb45f0da14e9085cd6d1f1e5deab7 = new Button("OK") {Width=100,Height=35,InstanceName="Default"}; /*  */
            var id_ba5c282a10f34d99b7f2fd8403969c84 = new Button("OK") {Width=60,Height=35,Margin=new Thickness(5),InstanceName="Default"}; /*  */
            var id_c4922d63c8b84896993f5d132e5722c5 = new Button("Send") {Margin=new Thickness(10, 0, 10, 0),InstanceName="Default"}; /*  */
            var id_fdd861eeb4364be792634a8195b2903a = new Button("No") {Margin=new Thickness(5),InstanceName="Default"}; /*  */
            var MiHubRegisterSubmitButton = new Button("Submit") {InstanceName="MiHubRegisterSubmitButton"}; /*  */
            var mihubSubmitButton = new Button("Submit") {InstanceName="mihubSubmitButton"}; /*  */
            var id_2e3877912e8445e08363641337958174 = new CheckBox() {InstanceName="Default"}; /*  */
            var id_4lhahk5tlaquvln6kiat712bm5 = new CheckBox("Remember me",checkedByDefault:true) {InstanceName="Default"}; /*  */
            var id_6c471bde36cb4b2399e6339c4b6c8e17 = new CheckBox("Show password") {InstanceName="Default"}; /*  */
            var id_9c5999cabf3a45f5b9b7dcf7328d2e16 = new CheckBox("Show password") {InstanceName="Default"}; /*  */
            var id_9fa6d12d37f44a2b86ed059d7c8229c9 = new CheckBox("I have read and I agree to the terms and conditions and privacy policy.") {InstanceName="Default"}; /*  */
            var id_ff230998ccef43cfb64db36b7fe946fa = new CheckBox("I would like to opt in to marketing communications.") {InstanceName="Default"}; /*  */
            var id_5544b1d1fc02448db83b43331a6f7569 = new Collection<JProperty>() {InstanceName="Default",OutputLength=-2,OutputOnEvent=true}; /*  */
            var naitAnimalTransferModelProperties = new Collection<JProperty>() {InstanceName="naitAnimalTransferModelProperties",OutputLength=-2,OutputOnEvent=true}; /*  */
            var id_5d5379ea03b74bad9a00188103e65258 = new Collection<string>() {InstanceName="Default",OutputLength=2,ClearOnOutput=true}; /*  */
            var id_6op7ddi9dvfpbhlobjc5cat8ol = new Collection<string>() {InstanceName="Default",OutputLength=2}; /*  */
            var MiHubFilePathsForUpload = new Collection<string>() {InstanceName="MiHubFilePathsForUpload",OutputLength=-2,OutputOnEvent=true}; /*  */
            var MiHubFileUploadResultRow = new Collection<string>() {InstanceName="MiHubFileUploadResultRow",OutputLength=2,ClearOnOutput=true}; /*  */
            var MiHubGetRegisterPostContent = new ConvertPairsToDictionary<string, string>() {InstanceName="MiHubGetRegisterPostContent"}; /*  */
            var id_a7d26ee500a14f4182bbd8064b73a7d3 = new ToEvent<JProperty>() {InstanceName="Default"}; /*  */
            var id_3e073009d92349cf9c0dcc0480874493 = new ToEvent<JToken>() {InstanceName="Default"}; /*  */
            var id_386b3e85562444f693c7fcade085a958 = new ToEvent<List<string>>() {InstanceName="Default"}; /*  */
            var id_30421d26a7924c0fb46aae1d73765332 = new ToEvent<string>() {InstanceName="Default"}; /*  */
            var id_40dq28vgo12k037keds1ng3dja = new ToEvent<string>() {InstanceName="Default"}; /*  */
            var id_6661ec50307749328dea851c9eea8a60 = new ToEvent<string>() {InstanceName="Default"}; /*  */
            var id_cb67b64a2ad1457ba5e5cfec25aa031c = new ToEvent<string>() {InstanceName="Default"}; /*  */
            var AnimalTransferModel = new ConvertToJObject() {InstanceName="AnimalTransferModel"}; /*  */
            var naitRequest = new ConvertToJObject() {InstanceName="naitRequest"}; /*  */
            var id_608f85c1386b4536bf8c8b1fe60322af = new ConvertToJProperty<DateTime>() {InstanceName="Default"}; /*  */
            var id_c8f605c53e0b477a8dddc51910a9bf81 = new ConvertToJProperty<DateTime>() {InstanceName="Default"}; /*  */
            var id_f12593382f6646eab4ee39013b380ec2 = new ConvertToJProperty<DateTime>() {InstanceName="Default"}; /*  */
            var id_916a416fd7524b4fa2df3a293ca01827 = new ConvertToJProperty<int>() {InstanceName="Default"}; /*  */
            var id_d30bb2b0d38f4719a360da3eb5fd1cc6 = new ConvertToJProperty<int>() {Key="IntegrationType",InstanceName="Default"}; /*  */
            var id_103cf07bd2f945728611c0e2b8b7b34c = new ConvertToJProperty<JToken>() {Key="ExternalAnimalIds",InstanceName="Default"}; /*  */
            var id_12d8420d774541ae9e19919bd1da77bf = new ConvertToJProperty<string>() {InstanceName="Default"}; /*  */
            var id_313fc9ff67f1439da6458d65bcc9af06 = new ConvertToJProperty<string>() {Key="DataLinkDeviceId",InstanceName="Default"}; /*  */
            var id_a84abf49555d47e39927398d67c37007 = new ConvertToJProperty<string>() {InstanceName="Default"}; /*  */
            var id_aae2ddd9f1dc4728a59446c980b7693c = new ConvertToJProperty<string>() {Key="JsonPayload",InstanceName="Default"}; /*  */
            var id_ad680c5d89a6414b83ad625a37b3cbab = new ConvertToJProperty<string>() {InstanceName="Default"}; /*  */
            var id_e3200202c206453eae7d97c9e1b301aa = new ConvertToJProperty<string>() {Key="IsStaging",InstanceName="Default"}; /*  */
            var id_16cbe6db330843bbb5c408737d50daff = new Count<DataTable>() {InstanceName="Default"}; /*  */
            var MiHubUploadErrorMessageCount = new Counter() {InstanceName="MiHubUploadErrorMessageCount"}; /*  */
            var MiHubUploadSuccessMessageCount = new Counter() {InstanceName="MiHubUploadSuccessMessageCount"}; /*  */
            var createAnimalTransferModel = new CreateJObject() {InstanceName="createAnimalTransferModel"}; /*  */
            var createNlisRequest = new CreateJObject() {InstanceName="createNlisRequest"}; /*  */
            var MiHubCreateUpdateFarmJObject = new CreateJObject() {InstanceName="MiHubCreateUpdateFarmJObject"}; /*  */
            var MiHubCreateUpdateUserJObject = new CreateJObject() {InstanceName="MiHubCreateUpdateUserJObject"}; /*  */
            var id_02b0997d1f9c43c0bde76049ae19c4fd = new Data<bool>() {InstanceName="Default"}; /*  */
            var id_096658137fc64c6aaa12ee21db062be4 = new Data<bool>() {InstanceName="Default",storedData=false}; /*  */
            var id_0cfa00c048684f4f8ee5eabc5ce84563 = new Data<bool>() {InstanceName="Default"}; /*  */
            var id_0cfc810d6b5b428caa7a75c6e1db9ba6 = new Data<bool>() {InstanceName="Default"}; /*  */
            var id_1b27e18ead644e4c8c084dcdf410acac = new Data<bool>() {InstanceName="Default",storedData=true}; /*  */
            var id_30ae8f8365824f7a97d2a1a521d5c5ba = new Data<bool>() {InstanceName="Default",storedData=true}; /*  */
            var id_314156256a124f6aae9a60c3ce57242d = new Data<bool>() {InstanceName="Default"}; /*  */
            var id_3f8e5b431e8446578e978b098d75231c = new Data<bool>() {InstanceName="Default",storedData=false}; /*  */
            var id_53c52960fe3241d89009c7d78fd506ef = new Data<bool>() {InstanceName="Default",storedData=true}; /*  */
            var id_54b337bf303c4da0b9699c9e9ec75fe0 = new Data<bool>() {InstanceName="Default"}; /*  */
            var id_5fb6256e873441bc875865b02d5e15d0 = new Data<bool>() {InstanceName="Default"}; /*  */
            var id_635dbd7ab6434dc3a53db2d263f03316 = new Data<bool>() {InstanceName="Default"}; /*  */
            var id_63a4d5f8ac9b47479d15685db94ca43e = new Data<bool>() {InstanceName="Default",storedData=true}; /*  */
            var id_73fefa5cddcf4c768d3aaecd2eb5411e = new Data<bool>() {InstanceName="Default",storedData=false}; /*  */
            var id_b82a41836e8b45c89a7efcc29acdb694 = new Data<bool>() {InstanceName="Default",storedData=false}; /*  */
            var id_b89e3416368e4d068ac246117f59752d = new Data<bool>() {InstanceName="Default",storedData=false}; /*  */
            var id_bd08431f0ad84f0cb71f566017085cdb = new Data<bool>() {InstanceName="Default"}; /*  */
            var id_bf96088659a0499d938bafec835fc3d7 = new Data<bool>() {InstanceName="Default"}; /*  */
            var id_c1cdac9b45714a94bd252b1e652478a9 = new Data<bool>() {InstanceName="Default",storedData=true}; /*  */
            var id_c6339ec9b4ca49819be8d7234fce48f4 = new Data<bool>() {InstanceName="Default",storedData=false}; /*  */
            var id_ca634d28d3834718a520df93c2ea74a1 = new Data<bool>() {InstanceName="Default",storedData=true}; /*  */
            var id_d09188984a2447e697aca79195b2c001 = new Data<bool>() {InstanceName="Default",storedData=false}; /*  */
            var id_f8eb03e5807c4f4caa8a87531936fa0d = new Data<bool>() {InstanceName="Default",storedData=true}; /*  */
            var MiHubResultsHeadersHaveBeenAdded = new Data<bool>() {InstanceName="MiHubResultsHeadersHaveBeenAdded",storedData=true}; /*  */
            var naitIsStaging = new Data<bool>() {InstanceName="naitIsStaging",storedData=true}; /*  */
            var naitSendTransactionFalse = new Data<bool>() {InstanceName="naitSendTransactionFalse",storedData=false}; /*  */
            var naitSendTransactionTrue = new Data<bool>() {InstanceName="naitSendTransactionTrue",storedData=true}; /*  */
            var nlisSendTransactionFalse = new Data<bool>() {InstanceName="nlisSendTransactionFalse",storedData=false}; /*  */
            var nlisSendTransactionTrue = new Data<bool>() {InstanceName="nlisSendTransactionTrue",storedData=true}; /*  */
            var id_0a1afcc9940543f0bd6380896e808b85 = new Data<int>() {InstanceName="Default",storedData=3}; /*  */
            var id_0fukarbv7uk94ec3lgmviqgc7e = new Data<int>() {InstanceName="Default",storedData=0}; /*  */
            var id_4rifkejvr2umhs9qgt2jbdn5oq = new Data<int>() {InstanceName="Default",storedData=0}; /*  */
            var naitIntegrationType = new Data<int>() {InstanceName="naitIntegrationType",storedData=2}; /*  */
            var id_ac3a5d8867ba488bb955ca380f6acc8d = new Data<List<string>>() {InstanceName="Default",storedData=new List<string>(){"982 000000000001"}}; /*  */
            var DataLinkDeviceID = new Data<string>() {InstanceName="DataLinkDeviceID",storedData="placeholderID"}; /*  */
            var id_04bac1d1ef804607a8a4c5e990e19184 = new Data<string>() {InstanceName="Default",storedData="NaitNumberClient"}; /*  */
            var id_086fb4d7643f48d08aeffc39af4ea44f = new Data<string>() {InstanceName="Default"}; /*  */
            var id_11e8237467ec49e6a6a7438511acc42e = new Data<string>() {InstanceName="Default",storedData=@"C:\ProgramData\Tru-Test\DataLink_ALA\userinfo.json"}; /*  */
            var id_14a577073c4d496397830c92a1aaf1c6 = new Data<string>() {InstanceName="Default"}; /*  */
            var id_19148524293a42fb9919a298cf1689aa = new Data<string>() {InstanceName="Default",storedData="Result message:"}; /*  */
            var id_1udlotvvg1j2v2spsjqui1ih71 = new Data<string>() {InstanceName="Default",Perishable=true,storedData="Message"}; /*  */
            var id_2bc9ebfeddb54821b9d4a0a80351bbea = new Data<string>() {InstanceName="Default",storedData="New Zealand"}; /*  */
            var id_36c0be0f8c3e4b99af4956137c978a80 = new Data<string>() {InstanceName="Default",storedData="DateOfBirth"}; /*  */
            var id_43e118675f474965956fac19cad8e1c8 = new Data<string>() {InstanceName="Default",storedData="AnimalMovementType"}; /*  */
            var id_5018714ff6b14b1dbd173a97124da854 = new Data<string>() {InstanceName="Default"}; /*  */
            var id_525fac181b18457da951b590c7c385d3 = new Data<string>() {InstanceName="Default"}; /*  */
            var id_56a7b5ad9cad45f38ecc45a37a686043 = new Data<string>() {InstanceName="Default",storedData=""}; /*  */
            var id_5b2f388fa717485a9311b2cccba6b8c4 = new Data<string>() {InstanceName="Default",storedData=@"C:\ProgramData\Tru-Test\DataLink_ALA\"}; /*  */
            var id_62c77e7aa1cc4b5187d58fabf10b99d0 = new Data<string>() {InstanceName="Default",storedData="Password"}; /*  */
            var id_635b56f1854b45d88592a25fb7f662d4 = new Data<string>() {InstanceName="Default",storedData="ProductionType"}; /*  */
            var id_727pk5631j8mibmggs23tpeso6 = new Data<string>() {InstanceName="Default",Perishable=true,storedData="File"}; /*  */
            var id_739acd6848914fdf913f1d84f191d65a = new Data<string>() {InstanceName="Default",storedData="MovementDate"}; /*  */
            var id_75c86eb2da5a4dcc89bdfd6cbaaaf552 = new Data<string>() {InstanceName="Default",storedData="DateSendOrReceive"}; /*  */
            var id_7746044f122f401c9627646aa1b9e938 = new Data<string>() {InstanceName="Default",storedData="OtherClientKey"}; /*  */
            var id_7aa01a29dd9548ceb7e4fd7145d70f92 = new Data<string>() {InstanceName="Default",storedData="PicTo"}; /*  */
            var id_80b50ee50c0f42148a32f273835a8d75 = new Data<string>() {InstanceName="Default",storedData="AnimalType"}; /*  */
            var id_8f768d6ed7c140949a3138a729589ae3 = new Data<string>() {InstanceName="Default",storedData=@"C:\ProgramData\Tru-Test\DataLink_ALA\userinfo.json"}; /*  */
            var id_a904b669b255441ba48d7e6faa581653 = new Data<string>() {InstanceName="Default",storedData="Username"}; /*  */
            var id_bfc9fd8f7a2e46d6ac05e0295cd37111 = new Data<string>() {InstanceName="Default"}; /*  */
            var id_d15c7648fe23487c9ae1ee732ca55295 = new Data<string>() {InstanceName="Default",storedData="NaitNumberOther"}; /*  */
            var id_e72b3cba2b8e4de6b410c5490cfb9225 = new Data<string>() {InstanceName="Default",storedData="userinfo.json"}; /*  */
            var id_ea5215389c924a94a0f0f750db1e8d72 = new Data<string>() {InstanceName="Default",storedData="ClientKey"}; /*  */
            var id_f5d9bc0340b44f7f95bd6ac688c5d6cd = new Data<string>() {InstanceName="Default",storedData="PicFrom"}; /*  */
            var MiHubFileUploadErrorMessageGate = new Data<string>() {InstanceName="MiHubFileUploadErrorMessageGate"}; /*  */
            var MiHubFileUploadSuccessMessageGate = new Data<string>() {InstanceName="MiHubFileUploadSuccessMessageGate"}; /*  */
            var MiHubRegisterAddressLine1Key = new Data<string>() {InstanceName="MiHubRegisterAddressLine1Key",storedData="AddressLine1"}; /*  */
            var MiHubRegisterAddressLine2Key = new Data<string>() {InstanceName="MiHubRegisterAddressLine2Key",storedData="AddressLine2"}; /*  */
            var MiHubRegisterCityKey = new Data<string>() {InstanceName="MiHubRegisterCityKey",storedData="City"}; /*  */
            var MiHubRegisterConfirmPasswordKey = new Data<string>() {InstanceName="MiHubRegisterConfirmPasswordKey",storedData="ConfirmPassword"}; /*  */
            var MiHubRegisterCountryKey = new Data<string>() {InstanceName="MiHubRegisterCountryKey",storedData="Country"}; /*  */
            var MiHubRegisterEmailKey = new Data<string>() {InstanceName="MiHubRegisterEmailKey",storedData="Email"}; /*  */
            var MiHubRegisterFirstNameKey = new Data<string>() {InstanceName="MiHubRegisterFirstNameKey",storedData="FirstName"}; /*  */
            var MiHubRegisterLastNameKey = new Data<string>() {InstanceName="MiHubRegisterLastNameKey",storedData="LastName"}; /*  */
            var MiHubRegisterPasswordKey = new Data<string>() {InstanceName="MiHubRegisterPasswordKey",storedData="Password"}; /*  */
            var MiHubRegisterPhoneNumberKey = new Data<string>() {InstanceName="MiHubRegisterPhoneNumberKey",storedData="PhoneNumber"}; /*  */
            var MiHubRegisterPostcodeKey = new Data<string>() {InstanceName="MiHubRegisterPostcodeKey",storedData="Postcode"}; /*  */
            var MiHubRegisterRegionKey = new Data<string>() {InstanceName="MiHubRegisterRegionKey",storedData="Region"}; /*  */
            var MiHubRegisterSuburbKey = new Data<string>() {InstanceName="MiHubRegisterSuburbKey",storedData="Suburb"}; /*  */
            var id_dd832e75f1d4430d9a68720945aef2f4 = new Data<Tuple<string, string>>() {InstanceName="Default",storedData=new Tuple<string, string>("X-HTTP-Method-Override", "PATCH")}; /*  */
            var id_f350e52641bf4046b178b0fa1646c4cf = new Data<Tuple<string, string>>() {InstanceName="Default",storedData=new Tuple<string, string>("X-HTTP-Method-Override", "PATCH")}; /*  */
            var id_0292522dc1b7445eb877bf981f255168 = new DataFlowConnector<bool>() {InstanceName="Default"}; /*  */
            var id_16a4e083c9e44516b12b97334e764e5f = new DataFlowConnector<bool>() {InstanceName="Default"}; /*  */
            var id_3fdfa60a9377405699a15fd2f93c91ef = new DataFlowConnector<bool>() {InstanceName="Default"}; /*  */
            var id_41029b6f4f394eb38cf253283438ec6a = new DataFlowConnector<bool>() {InstanceName="Default"}; /*  */
            var id_65f3c7ebb0b54cfbb3bf45b6059693e3 = new DataFlowConnector<bool>() {InstanceName="Default"}; /*  */
            var id_7423dde774de4cc891a9b13290dad2f4 = new DataFlowConnector<bool>() {InstanceName="Default"}; /*  */
            var id_81d9b9d1bf5f4352ac224751c8984754 = new DataFlowConnector<bool>() {InstanceName="Default"}; /*  */
            var id_82c68ffb6440414bb22a9f88ff668e4e = new DataFlowConnector<bool>() {InstanceName="Default"}; /*  */
            var id_d21ae554d46744d0a710090c4674582e = new DataFlowConnector<bool>() {InstanceName="Default"}; /*  */
            var id_dbcfc233fc4943e39e2fa6ae9776fec3 = new DataFlowConnector<bool>() {InstanceName="Default"}; /*  */
            var id_e3d3285bfff9411ab2b444bbae255d27 = new DataFlowConnector<bool>() {InstanceName="Default"}; /*  */
            var id_e6372bee98b6470bb3fc834a30a5e850 = new DataFlowConnector<bool>() {InstanceName="Default"}; /*  */
            var internetConnectionDetectedConnector = new DataFlowConnector<bool>() {InstanceName="internetConnectionDetectedConnector"}; /*  */
            var isNaitRequest = new DataFlowConnector<bool>() {InstanceName="isNaitRequest"}; /*  */
            var isNlisRequest = new DataFlowConnector<bool>() {InstanceName="isNlisRequest"}; /*  */
            var MiHubLoginCheckBoxNotCheckedConnector = new DataFlowConnector<bool>() {InstanceName="MiHubLoginCheckBoxNotCheckedConnector"}; /*  */
            var MiHubRequireLogin = new DataFlowConnector<bool>() {InstanceName="MiHubRequireLogin"}; /*  */
            var nlisAgentTransactionTypeConnector = new DataFlowConnector<bool>() {InstanceName="nlisAgentTransactionTypeConnector"}; /*  */
            var nlisProducerTransactionTypeConnector = new DataFlowConnector<bool>() {InstanceName="nlisProducerTransactionTypeConnector"}; /*  */
            var nlisSaleyardTransactionTypeConnector = new DataFlowConnector<bool>() {InstanceName="nlisSaleyardTransactionTypeConnector"}; /*  */
            var id_4cb4fef33e2441f399841c04c3c51e55 = new DataFlowConnector<DataTable>() {InstanceName="Default"}; /*  */
            var dateSelectedConnector = new DataFlowConnector<DateTime>() {InstanceName="dateSelectedConnector"}; /*  */
            var id_da358f4975324d07a4550c506c35976d = new DataFlowConnector<DateTime>() {InstanceName="Default"}; /*  */
            var id_40fd5f5747454fc79d44994b4e70932f = new DataFlowConnector<Dictionary<string, JToken>>() {InstanceName="Default"}; /*  */
            var id_783756683f1e475bbced2c42ea44f2f8 = new DataFlowConnector<Dictionary<string, JToken>>() {InstanceName="Default"}; /*  */
            var id_96e099d81d7542b8b88a9e659f1a4a61 = new DataFlowConnector<Dictionary<string, JToken>>() {InstanceName="Default"}; /*  */
            var id_594420eb1db24ba4b2332d379da3fb8b = new DataFlowConnector<Dictionary<string, string>>() {InstanceName="Default"}; /*  */
            var MiHubGetRegisterPostContentConnector = new DataFlowConnector<Dictionary<string, string>>() {InstanceName="MiHubGetRegisterPostContentConnector"}; /*  */
            var MiHubTokenDictConnector = new DataFlowConnector<Dictionary<string, string>>() {InstanceName="MiHubTokenDictConnector"}; /*  */
            var id_70573776acca4e54b264a2110d2057ef = new DataFlowConnector<int>() {InstanceName="Default"}; /*  */
            var id_d44743eceb264b98a0f83b41c854ff66 = new DataFlowConnector<int>() {InstanceName="Default"}; /*  */
            var nlisMovementType = new DataFlowConnector<int>() {InstanceName="nlisMovementType"}; /*  */
            var id_8992fcfc401344849f2b2e50f761c0b9 = new DataFlowConnector<JProperty>() {InstanceName="Default"}; /*  */
            var id_a1e0baa5d0cb4c208e6d711f1f3ee319 = new DataFlowConnector<JProperty>() {InstanceName="Default"}; /*  */
            var signalStartNaitRequest = new DataFlowConnector<JProperty>() {InstanceName="signalStartNaitRequest"}; /*  */
            var id_09692c5f6cef45b3a2f19016ed3d6da1 = new DataFlowConnector<JToken>() {InstanceName="Default"}; /*  */
            var id_1411bc583a074c2e9901c84a80dc235d = new DataFlowConnector<JToken>() {InstanceName="Default"}; /*  */
            var id_18798cc54be840a1985dd06683a7c6c9 = new DataFlowConnector<JToken>() {InstanceName="Default"}; /*  */
            var id_4231fb26b0384f19939340faedd53766 = new DataFlowConnector<JToken>() {InstanceName="Default"}; /*  */
            var id_49eda0f67ef5458d84350fcd6b932649 = new DataFlowConnector<JToken>() {InstanceName="Default"}; /*  */
            var id_6ec24c3656714fdeb9566c55140e5578 = new DataFlowConnector<JToken>() {InstanceName="Default"}; /*  */
            var id_b65a57f1fff140c1bafe7e9f2dfa3b7e = new DataFlowConnector<JToken>() {InstanceName="Default"}; /*  */
            var id_cdcf6555d3e54c8084afe5f2d6b30b16 = new DataFlowConnector<JToken>() {InstanceName="Default"}; /*  */
            var id_d6e883a6607748c38cb9b50a6e6eef71 = new DataFlowConnector<JToken>() {InstanceName="Default"}; /*  */
            var id_d7b4bee64cae4f8dbe9116e48bcf6643 = new DataFlowConnector<JToken>() {InstanceName="Default"}; /*  */
            var id_fdfe07fac9944b2fa0032b31ab38ccb1 = new DataFlowConnector<JToken>() {InstanceName="Default"}; /*  */
            var id_2b788b8e0252466f9c03fcbb2052b63d = new DataFlowConnector<List<string>>() {InstanceName="Default"}; /*  */
            var tickedSessionEIDConnector = new DataFlowConnector<List<string>>() {InstanceName="tickedSessionEIDConnector"}; /*  */
            var csvAutoSaveFilePathsConnector = new DataFlowConnector<string>() {InstanceName="csvAutoSaveFilePathsConnector"}; /*  */
            var id_0e93cf0de4784e1a9d587d8776484791 = new DataFlowConnector<string>() {InstanceName="Default"}; /*  */
            var id_150954a7cc4a4da599521fbcaaec1e8d = new DataFlowConnector<string>() {InstanceName="Default"}; /*  */
            var id_1b22e6d1faef4da29dca2492e66e4fdb = new DataFlowConnector<string>() {InstanceName="Default"}; /*  */
            var id_1c12611bf82d4513b4a2435dc30e939b = new DataFlowConnector<string>() {InstanceName="Default"}; /*  */
            var id_20f55ade17524bdc9ec9f20c6393c08b = new DataFlowConnector<string>() {InstanceName="Default"}; /*  */
            var id_24bd0becfb10498197af0bfb664282e6 = new DataFlowConnector<string>() {InstanceName="Default"}; /*  */
            var id_257f730a78944f218fcb41099f882921 = new DataFlowConnector<string>() {InstanceName="Default"}; /*  */
            var id_348262b7e41345779351390fc8578212 = new DataFlowConnector<string>() {InstanceName="Default"}; /*  */
            var id_38e8b9d6ecda42619e088c892acf96dd = new DataFlowConnector<string>() {InstanceName="Default"}; /*  */
            var id_44f53a25c8f24a00a34eea6bc688098e = new DataFlowConnector<string>() {InstanceName="Default"}; /*  */
            var id_53a4c589151c47c89047f71b5c6538cb = new DataFlowConnector<string>() {InstanceName="Default"}; /*  */
            var id_548730d776d34674b4975d55d2c64f07 = new DataFlowConnector<string>() {InstanceName="Default"}; /*  */
            var id_6690c83ad6de4da7b4363ccbe1e071b6 = new DataFlowConnector<string>() {InstanceName="Default"}; /*  */
            var id_6e5f96cd453b4895a693d449fc73ee97 = new DataFlowConnector<string>() {InstanceName="Default"}; /*  */
            var id_715693f09f1a461fa16c655ed8f759e1 = new DataFlowConnector<string>() {InstanceName="Default"}; /*  */
            var id_7c4df9f0eb4545ea9eb636fa7b2569af = new DataFlowConnector<string>() {InstanceName="Default"}; /*  */
            var id_7d9d7ce095f94b3592c00f45c2dded9a = new DataFlowConnector<string>() {InstanceName="Default"}; /*  */
            var id_8c07cd9ed4744a5089cd3ece10a84595 = new DataFlowConnector<string>() {InstanceName="Default"}; /*  */
            var id_8fab8fe267d941049387109f8dcd0dba = new DataFlowConnector<string>() {InstanceName="Default"}; /*  */
            var id_991cdbdd601a4fef8bc49a210527243d = new DataFlowConnector<string>() {InstanceName="Default"}; /*  */
            var id_b8b9cad76d86405e9df3cbae35578fef = new DataFlowConnector<string>() {InstanceName="Default"}; /*  */
            var id_bc04d19f4ce045298c36eeec2858f1bb = new DataFlowConnector<string>() {InstanceName="Default"}; /*  */
            var id_bcf1a241b140420c8446eaf4ef431de0 = new DataFlowConnector<string>() {InstanceName="Default"}; /*  */
            var id_c595fee688c54867b940737e17fb98fa = new DataFlowConnector<string>() {InstanceName="Default"}; /*  */
            var id_ce23f156695e40fc99d74a7ecde1f1e7 = new DataFlowConnector<string>() {InstanceName="Default"}; /*  */
            var id_cf7f3646d49d428680fa04612896bc86 = new DataFlowConnector<string>() {InstanceName="Default"}; /*  */
            var id_d19d6642f58b4abaac2bdeb8cba5463a = new DataFlowConnector<string>() {InstanceName="Default"}; /*  */
            var id_dd2bb2b9325a489483cfaf66baa14c36 = new DataFlowConnector<string>() {InstanceName="Default"}; /*  */
            var id_decf467d558d4b4eac506991ed477790 = new DataFlowConnector<string>() {InstanceName="Default"}; /*  */
            var id_e71fb3b8526b49c7b7be4da7699b4857 = new DataFlowConnector<string>() {InstanceName="Default"}; /*  */
            var id_e7b57c8a6e674b2eae8f718690c1d6d0 = new DataFlowConnector<string>() {InstanceName="Default"}; /*  */
            var id_e7e61b98fbd048b9aabd8d672bc34c78 = new DataFlowConnector<string>() {InstanceName="Default"}; /*  */
            var id_ea722864f23b4564943d776eb3f4dea2 = new DataFlowConnector<string>() {InstanceName="Default"}; /*  */
            var id_ffb2290fc0d34e5587097f4d26a4902b = new DataFlowConnector<string>() {InstanceName="Default"}; /*  */
            var MiHubAccessTokenConnector = new DataFlowConnector<string>() {InstanceName="MiHubAccessTokenConnector"}; /*  */
            var MiHubNewUserAccessTokenConnector = new DataFlowConnector<string>() {InstanceName="MiHubNewUserAccessTokenConnector"}; /*  */
            var MiHubRefreshTokenConnector = new DataFlowConnector<string>() {InstanceName="MiHubRefreshTokenConnector"}; /*  */
            var MiHubSessionFilePathFromLoop = new DataFlowConnector<string>() {InstanceName="MiHubSessionFilePathFromLoop"}; /*  */
            var MiHubSessionUploadResultMessageConnector = new DataFlowConnector<string>() {InstanceName="MiHubSessionUploadResultMessageConnector"}; /*  */
            var naitAnimalTypeConnector = new DataFlowConnector<string>() {InstanceName="naitAnimalTypeConnector"}; /*  */
            var naitProductionTypeConnector = new DataFlowConnector<string>() {InstanceName="naitProductionTypeConnector"}; /*  */
            var naitTransactionTypeConnector = new DataFlowConnector<string>() {InstanceName="naitTransactionTypeConnector"}; /*  */
            var nlisAccountTypeConnector = new DataFlowConnector<string>() {InstanceName="nlisAccountTypeConnector"}; /*  */
            var nlisTransactionTypeConnector = new DataFlowConnector<string>() {InstanceName="nlisTransactionTypeConnector"}; /*  */
            var id_10fc8ad6907140aa8ded720d980ece47 = new DataFlowConnector<Tuple<string, string>>() {InstanceName="Default"}; /*  */
            var id_1f01e51edbfa42468753b023959726fb = new DataFlowGate<bool>() {InstanceName="Default"}; /*  */
            var id_3744db3fbed141de831911d1860b8f23 = new DataFlowGate<bool>() {InstanceName="Default"}; /*  */
            var id_66d04bd1dd3d448a9858e515f4652663 = new DataFlowGate<bool>() {InstanceName="Default"}; /*  */
            var id_7010644277974849b00f068bed0dec09 = new DataFlowGate<bool>() {InstanceName="Default"}; /*  */
            var id_db1379daf87c4cab92c23fcf2804e022 = new DataFlowGate<bool>() {InstanceName="Default"}; /*  */
            var id_a7cb9588be2f41e6b556905c531ee324 = new DataFlowGate<DataTable>() {InstanceName="Default"}; /*  */
            var id_03aa7d025d164199a10d71221068655c = new DataFlowGate<List<string>>() {InstanceName="Default"}; /*  */
            var id_30499ea22cd84486b86eaca1fc1de4d0 = new DataFlowGate<string>() {InstanceName="Default"}; /*  */
            var id_90563c01d56a4a598debfa095e607cf3 = new DataFlowGate<string>() {InstanceName="Default"}; /*  */
            var id_96960a26ed8245ce806a2c8437e25a82 = new DataFlowGate<string>() {InstanceName="Default"}; /*  */
            var id_f3ec0073daa24f7f953b58c30559776c = new DataFlowGate<string>() {InstanceName="Default"}; /*  */
            var MiHubSession = new DataFromPath() {InstanceName="MiHubSession"}; /*  */
            var MiHubStoredUserinfoData = new DataFromPath() {InstanceName="MiHubStoredUserinfoData"}; /*  */
            var id_2930a1df8aec4aaa9972fde68ea5a20b = new DatePicker() {InstanceName="Default"}; /*  */
            var id_392f879b8a2c4ed3af984832834db5ae = new DatePicker() {InstanceName="Default"}; /*  */
            var id_7f94e8ce322b493a863fc4a236dec07b = new DatePicker() {InstanceName="Default"}; /*  */
            var id_a0783ff3db5648a3b344e0a0fa91db9e = new DatePicker() {InstanceName="Default"}; /*  */
            var id_b79e7486f476446b9994ceff04e551f0 = new DatePicker() {InstanceName="Default"}; /*  */
            var id_baa490506588456ab06001a06eb13ad4 = new DatePicker() {InstanceName="Default"}; /*  */
            var id_c7b2f89821134edcb8cd0bc25cbf80a7 = new DatePicker() {InstanceName="Default"}; /*  */
            var id_f5c2b9fd10284292964aebf2478e1acf = new DatePicker() {InstanceName="Default"}; /*  */
            var id_1a9a7707a5c3486981ac4b464aed203d = new Equals<string>("Producer") {InstanceName="Default"}; /*  */
            var id_22af63e4247f4c5ab67354542c72a081 = new Equals<string>("Third party") {InstanceName="Default"}; /*  */
            var id_2f76cca8bc6d47b1862934669812a67d = new Equals<string>("0") {InstanceName="Default"}; /*  */
            var id_34f649e132444d7c950b42d5079234c0 = new Equals<string>("") {InstanceName="Default"}; /*  */
            var id_385bc289fdf34b31a705055fbca249df = new Equals<string>("Cattle") {InstanceName="Default"}; /*  */
            var id_38eeb0ea8d0643bdba116ad7ad308e2e = new Equals<string>("") {InstanceName="Default"}; /*  */
            var id_3f58b33fcfd646d882ed6935d7237489 = new Equals<string>("Deer") {InstanceName="Default"}; /*  */
            var id_546f5fa264784a188af409f1ad8f1a92 = new Equals<string>("") {InstanceName="Default"}; /*  */
            var id_5786e950445d499999a6822969cee806 = new Equals<string>("Receiving Movement") {InstanceName="Default"}; /*  */
            var id_784d11c852b446339537ea8baef1fff6 = new Equals<string>("") {InstanceName="Default"}; /*  */
            var id_7bc3718dafb1425aa3782a71820c5fb8 = new Equals<string>("From Saleyard") {InstanceName="Default"}; /*  */
            var id_934599e288894f098d384e455a13a960 = new Equals<string>("Agent") {InstanceName="Default"}; /*  */
            var id_9c3af4b947234c80932ae681c50b879b = new Equals<string>("Saleyard") {InstanceName="Default"}; /*  */
            var id_a3e922c083d94162af14a93edf8b9cbc = new Equals<string>("") {InstanceName="Default"}; /*  */
            var id_a5ed03bac16644fd9dc8d5fa66bac7df = new Equals<string>("OK") {InstanceName="Default"}; /*  */
            var id_b675bf1260d44a949d2d220d0165ce79 = new Equals<string>("Sending Movement") {InstanceName="Default"}; /*  */
            var id_badc02266e0149ffabbfddba818c486d = new Equals<string>("") {InstanceName="Default"}; /*  */
            var id_dc03f72dd88c43f09a1ee3104a1273ad = new Equals<string>("OK") {InstanceName="Default"}; /*  */
            var id_e4c7565291744b6fa329498e79114986 = new Equals<string>("To Saleyard") {InstanceName="Default"}; /*  */
            var id_ea831641b8764e0dbbd0e1f883e998bb = new Equals<string>("Animal Registration") {InstanceName="Default"}; /*  */
            var id_fc22728ea3374db997b3118a4c1b29e7 = new Equals<string>("Sending") {InstanceName="Default"}; /*  */
            var id_fddb3c9c8df241d5acc803a92bfd52b4 = new Equals<string>("Receiving") {InstanceName="Default"}; /*  */
            var MiHubFileUploadResponseHasNoError = new Equals<string>("") {InstanceName="MiHubFileUploadResponseHasNoError"}; /*  */
            var autoSaveIteratorComplete = new EventConnector() {InstanceName="autoSaveIteratorComplete"}; /*  */
            var getTickedSessionEIDs = new EventConnector() {InstanceName="getTickedSessionEIDs"}; /*  */
            var getTickedSessionsForEIDTransactComplete = new EventConnector() {InstanceName="getTickedSessionsForEIDTransactComplete"}; /*  */
            var id_0838b6d25d5b43f4a53336fbf30d08e1 = new EventConnector() {InstanceName="Default"}; /*  */
            var id_0aa41e47f2f5472f9a081525b167345e = new EventConnector() {InstanceName="Default"}; /*  */
            var id_0ec4f4b44fc24466beeab9d39fb2127c = new EventConnector() {InstanceName="Default"}; /*  */
            var id_0sdmuk24l36e6nbriv3m9t60vb = new EventConnector() {InstanceName="Default"}; /*  */
            var id_10750c4b15304e9dbcade8918b743048 = new EventConnector() {InstanceName="Default"}; /*  */
            var id_14f3d515b9904a689f088337a0b96ca1 = new EventConnector() {InstanceName="Default"}; /*  */
            var id_1784b154f7374f5cbb911ee7a168f20c = new EventConnector() {InstanceName="Default"}; /*  */
            var id_1790e20601e24b7084bd5047009f2b65 = new EventConnector() {InstanceName="Default"}; /*  */
            var id_1e364cc584874660b7c47ec55e3a9f08 = new EventConnector() {InstanceName="Default"}; /*  */
            var id_2acca303266a4f2795a93ad706f29ba5 = new EventConnector() {InstanceName="Default"}; /*  */
            var id_2f1511c34cbd492a9ee80249134130b7 = new EventConnector() {InstanceName="Default"}; /*  */
            var id_365b5944a5384370a15012d139c8fcb7 = new EventConnector() {InstanceName="Default"}; /*  */
            var id_3c8d235f430a41cb8037e1eb36bdc5b6 = new EventConnector() {InstanceName="Default"}; /*  */
            var id_410d0216993b4902bf0ccfe0cbc4ba38 = new EventConnector() {InstanceName="Default"}; /*  */
            var id_46bf30f48d0f43cb9b5752595152f9f8 = new EventConnector() {InstanceName="Default"}; /*  */
            var id_491bb339b418484c94b0a0caa6b95204 = new EventConnector() {InstanceName="Default"}; /*  */
            var id_4a3a78d6ad2c4d11843585518606023b = new EventConnector() {InstanceName="Default"}; /*  */
            var id_4d0026212c224ea6a03b08a97f463779 = new EventConnector() {InstanceName="Default"}; /*  */
            var id_4fa85b51f3da42e29ae1b593501db9cb = new EventConnector() {InstanceName="Default"}; /*  */
            var id_50t2i8olkuc2veg4cianbkpvsp = new EventConnector() {InstanceName="Default"}; /*  */
            var id_5aaeib7gs6vg67h1ae0hi7hn29 = new EventConnector() {InstanceName="Default"}; /*  */
            var id_5cd4589272024031b2fd55cde26fa7b8 = new EventConnector() {InstanceName="Default"}; /*  */
            var id_5sk8qgkc7ksfoeg6voa8cgoc7v = new EventConnector() {InstanceName="Default"}; /*  */
            var id_60f4c18dce3b4cb28433117430e58a84 = new EventConnector() {InstanceName="Default"}; /*  */
            var id_66bbdd38737645e4898eae7fa8e3efbf = new EventConnector() {InstanceName="Default"}; /*  */
            var id_67cf1514e8084b228835fbbb7b15e055 = new EventConnector() {InstanceName="Default"}; /*  */
            var id_6a91653313cb4bf5a67728e7c8630789 = new EventConnector() {InstanceName="Default"}; /*  */
            var id_6cd3b5e1af494ddf991b79da7486bac7 = new EventConnector() {InstanceName="Default"}; /*  */
            var id_6dc8057461014705abd0a766af08ccbd = new EventConnector() {InstanceName="Default"}; /*  */
            var id_6e133aa0417447d0be52e3484150e9f6 = new EventConnector() {InstanceName="Default"}; /*  */
            var id_707783bbfea74260b899228a2d2df55d = new EventConnector() {InstanceName="Default"}; /*  */
            var id_70b2e242030f4e0b9d9210b5c18cece7 = new EventConnector() {InstanceName="Default"}; /*  */
            var id_7292bc98301444fab5b2aae11e31a8fd = new EventConnector() {InstanceName="Default"}; /*  */
            var id_729f1a9c040d4bb18d7a42b60690c1d2 = new EventConnector() {InstanceName="Default"}; /*  */
            var id_76039e3bdc1b4f7ba90e51383ea70209 = new EventConnector() {InstanceName="Default"}; /*  */
            var id_7a09fad3caa649e8b67685650452a36c = new EventConnector() {InstanceName="Default"}; /*  */
            var id_7a2ba58d2b5044e18940a99dea0db00b = new EventConnector() {InstanceName="Default"}; /*  */
            var id_8559aa3a6cb749c69d9446902037bd89 = new EventConnector() {InstanceName="Default"}; /*  */
            var id_897d889c69524975a7b3f8e228d5e76f = new EventConnector() {InstanceName="Default"}; /*  */
            var id_8a15c8a96f614f2295f410243c1aec04 = new EventConnector() {InstanceName="Default"}; /*  */
            var id_8ef93747cd14440badbb642424b8a579 = new EventConnector() {InstanceName="Default"}; /*  */
            var id_98a7500f377947f88d285e0c67c16199 = new EventConnector() {InstanceName="Default"}; /*  */
            var id_9c5321d556bb44fe8ce5ea16b9887432 = new EventConnector() {InstanceName="Default"}; /*  */
            var id_a4e8114b07f5455aafd6cd7945bbc4b4 = new EventConnector() {InstanceName="Default"}; /*  */
            var id_a6fcb0583533433a94686c80c1fca1c3 = new EventConnector() {InstanceName="Default"}; /*  */
            var id_b108cb8668894806bdf6286cc065202f = new EventConnector() {InstanceName="Default"}; /*  */
            var id_b258791c1cec4f3c8d400a6ea4c5713c = new EventConnector() {InstanceName="Default"}; /*  */
            var id_b72b6ec85d21411db0e57578d9f3bb04 = new EventConnector() {InstanceName="Default"}; /*  */
            var id_b9bb90fc21b445d2a543a0ddb0152a5e = new EventConnector() {InstanceName="Default"}; /*  */
            var id_c2be2a36ac0b4f869d794dfa2117022e = new EventConnector() {InstanceName="Default"}; /*  */
            var id_d29584803b8447809c77105bd76a3b8b = new EventConnector() {InstanceName="Default"}; /*  */
            var id_d5a14ef0a1454d139027164fb2bce628 = new EventConnector() {InstanceName="Default"}; /*  */
            var id_d5f0bea66f2f4945bbf8106d1662a816 = new EventConnector() {InstanceName="Default"}; /*  */
            var id_df9b625052db4aae8c7635214cb29f35 = new EventConnector() {InstanceName="Default"}; /*  */
            var id_e45679f11c3547afa5d59774a62460a8 = new EventConnector() {InstanceName="Default"}; /*  */
            var id_e806d38f344f40e49a438207c0c15dc8 = new EventConnector() {InstanceName="Default"}; /*  */
            var id_eff4bf50999b4992985b91d784414c43 = new EventConnector() {InstanceName="Default"}; /*  */
            var id_f44fb6b6e9ce43a6b712c9677ceccc8d = new EventConnector() {InstanceName="Default"}; /*  */
            var id_fc31b76184c54389bc050d721f3dda66 = new EventConnector() {InstanceName="Default"}; /*  */
            var id_fcb3a7bfc5ea4632964430a99ad31662 = new EventConnector() {InstanceName="Default"}; /*  */
            var id_fd1c0559ccd644d88310eeb2316b1d74 = new EventConnector() {InstanceName="Default"}; /*  */
            var id_fde652396df84a70b6936a22a7d5db0d = new EventConnector() {InstanceName="Default"}; /*  */
            var MiHubAddResultsTableHeadersConnector = new EventConnector() {InstanceName="MiHubAddResultsTableHeadersConnector"}; /*  */
            var MiHubCheckIfRequireLogin = new EventConnector() {InstanceName="MiHubCheckIfRequireLogin"}; /*  */
            var MiHubDeleteAllSessionsAndAnimalsConnector = new EventConnector() {InstanceName="MiHubDeleteAllSessionsAndAnimalsConnector"}; /*  */
            var mihubEventConnector = new EventConnector() {InstanceName="mihubEventConnector"}; /*  */
            var MiHubEventsToExecuteForShowingResults = new EventConnector() {InstanceName="MiHubEventsToExecuteForShowingResults"}; /*  */
            var MiHubGetRegisterFieldPairs = new EventConnector() {InstanceName="MiHubGetRegisterFieldPairs"}; /*  */
            var MiHubLoginEventsBeforeRequest = new EventConnector() {InstanceName="MiHubLoginEventsBeforeRequest"}; /*  */
            var MiHubLogOut = new EventConnector() {InstanceName="MiHubLogOut"}; /*  */
            var MiHubRegisterEventConnector = new EventConnector() {InstanceName="MiHubRegisterEventConnector"}; /*  */
            var MiHubResultsTableRowAddedConnector = new EventConnector() {InstanceName="MiHubResultsTableRowAddedConnector"}; /*  */
            var MiHubResultsTransactCompleteConnector = new EventConnector() {InstanceName="MiHubResultsTransactCompleteConnector"}; /*  */
            var MiHubStartUploadConnector = new EventConnector() {InstanceName="MiHubStartUploadConnector"}; /*  */
            var naitGetJsonAnimalTransferModel = new EventConnector() {InstanceName="naitGetJsonAnimalTransferModel"}; /*  */
            var naitGetJsonNaitRequest = new EventConnector() {InstanceName="naitGetJsonNaitRequest"}; /*  */
            var naitGetJsonNaitRequestProperties = new EventConnector() {InstanceName="naitGetJsonNaitRequestProperties"}; /*  */
            var naitSetPlaceholderValues = new EventConnector() {InstanceName="naitSetPlaceholderValues"}; /*  */
            var setPlaceholderValues = new EventConnector() {InstanceName="setPlaceholderValues"}; /*  */
            var showIntegrationRequestResultWindow = new EventConnector() {InstanceName="showIntegrationRequestResultWindow"}; /*  */
            var startNait = new EventConnector() {InstanceName="startNait"}; /*  */
            var startNlis = new EventConnector() {InstanceName="startNlis"}; /*  */
            var SUBROUTINE_checkInternetConnectivity = new EventConnector() {InstanceName="SUBROUTINE_checkInternetConnectivity"}; /*  */
            var SUBROUTINE_MiHubLogin = new EventConnector() {InstanceName="SUBROUTINE_MiHubLogin"}; /*  */
            var tickedSessionEIDsHaveBeenReceived = new EventConnector() {InstanceName="tickedSessionEIDsHaveBeenReceived"}; /*  */
            var webServicesSubroutines = new EventConnector() {InstanceName="webServicesSubroutines"}; /*  */
            var id_1544e37f661d4c9995b43caa3ee4b707 = new EventGate() {InstanceName="Default"}; /*  */
            var id_27df171a08874ecfa5ca05db59b118bb = new EventGate() {InstanceName="Default"}; /*  */
            var id_59ina9kj9f67rdrphpt4q9q3sr = new EventGate() {InstanceName="Default"}; /*  */
            var id_7407374bc4d24db9b37cce4a3b620b15 = new EventGate() {InstanceName="Default"}; /*  */
            var MiHubAddResultsTableHeaders = new EventGate() {LatchInput=true,InstanceName="MiHubAddResultsTableHeaders"}; /*  */
            var naitSendTransactionEventGate = new EventGate() {InstanceName="naitSendTransactionEventGate"}; /*  */
            var openNaitForm = new EventGate() {InstanceName="openNaitForm"}; /*  */
            var openNlisForm = new EventGate() {InstanceName="openNlisForm"}; /*  */
            var id_487d74e91f93403abd6e99919ff77ca1 = new ExportDataTableColumn() {InstanceName="Default",ColumnName="index"}; /*  */
            var id_82f7c31961ab4df09447c7a0293bd55f = new ExportDataTableColumn() {InstanceName="Default",ColumnName="EID"}; /*  */
            var id_a1082346f50547bf81ef66f895201a36 = new ExportDataTableColumn() {InstanceName="Default",ColumnName="name"}; /*  */
            var id_6335eed2d5f64767bdc9b52ce6765bdc = new FileWriter() {InstanceName="Default"}; /*  */
            var getTickedSessionsForEIDFilter = new Filter() {InstanceName="getTickedSessionsForEIDFilter",FilterDelegate=(DataRow r) =>{    return bool.Parse(r["checkbox"].ToString());}}; /*  */
            var id_623f503fb93a4f0598ef5fd8c8ae8eca = new Filter() {InstanceName="Default",FilterDelegate=(DataRow r) =>{    return string.IsNullOrEmpty(r["EID"].ToString());}}; /*  */
            var id_d5367f6cab024f04961df458931896db = new Filter() {InstanceName="Default",FilterDelegate=(DataRow r) =>{    return !string.IsNullOrEmpty(r["EID"].ToString());}}; /*  */
            var id_4glcvtjtrebtrihhqfo7sjj044 = new GetJSONDict() {InstanceName="Default",FilePath=@"C:\ProgramData\Tru-Test\DataLink_ALA\userinfo.json"}; /*  */
            var id_742658c1120e438fb80394c6126908e7 = new GetJSONDict() {InstanceName="Default"}; /*  */
            var id_48793bf0c1ab4eccb962da3a207f5294 = new Grid() {InstanceName="Default"}; /*  */
            var MiHubUploadResultsGrid = new Grid() {InstanceName="MiHubUploadResultsGrid",RowHeight=50,Margin=new Thickness(5, 0, 5, 0),PrimaryKey="index"}; /*  */
            var id_07a7e799676f44769e81fab90502a0f1 = new Horizontal() {Margin=new Thickness(10),InstanceName="Default"}; /*  */
            var id_09f936ef6d1e41e98cf206357c6cee6c = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /*  */
            var id_1a094b4fe9164d5cbe21e8473f0b88f1 = new Horizontal() {Margin=new Thickness(10),InstanceName="Default"}; /*  */
            var id_1d90a04b3bca409b9713fe2c72eb868e = new Horizontal() {Margin=new Thickness(10),InstanceName="Default"}; /*  */
            var id_2195c20f51d6485993d73294b9906051 = new Horizontal() {Margin=new Thickness(10),InstanceName="Default"}; /*  */
            var id_21a11a4e15914063973ebd1cc1ec799e = new Horizontal() {Ratios=new int[]{10, 70, 20},Margin=new Thickness(10),InstanceName="Default"}; /*  */
            var id_283c0c61ab87494bbf910cffd9bd3564 = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /*  */
            var id_2a2fec1f9a2845039336a435a0fd0468 = new Horizontal() {Margin=new Thickness(10),InstanceName="Default"}; /*  */
            var id_2f3615c4bb614684896a967547a3aa1e = new Horizontal() {InstanceName="Default"}; /*  */
            var id_332b34352fe14890b48709e66b3e4fca = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /*  */
            var id_36a8f157c984404893e73064f71acab6 = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /*  */
            var id_473ea818fe7a4629a7dffa02f6f20a27 = new Horizontal() {Margin=new Thickness(10),InstanceName="Default"}; /*  */
            var id_4905798c2ffb4741b72d398dada38552 = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /*  */
            var id_4c733314cf2d4879ae5596d2dd37377b = new Horizontal() {Margin=new Thickness(10),InstanceName="Default"}; /*  */
            var id_55d50254d0514a7c9f85b77e362825ac = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /*  */
            var id_5ae03381ed914c5dafde453e4446e88e = new Horizontal() {Margin=new Thickness(10),InstanceName="Default"}; /*  */
            var id_5dba992efe104d1aab612d3536f0e5be = new Horizontal() {Margin=new Thickness(10),InstanceName="Default"}; /*  */
            var id_5ef32b7b278b4b8386790ced9d754e42 = new Horizontal() {Margin=new Thickness(10),InstanceName="Default"}; /*  */
            var id_5k0f4ovn7rhs42h991j6cfkmo1 = new Horizontal() {Ratios=new int[]{25, 75},InstanceName="Default"}; /*  */
            var id_601e2c887a9748cdb7512e09f3968459 = new Horizontal() {Margin=new Thickness(10),InstanceName="Default"}; /*  */
            var id_614ea4fb0f834dc883f207ed7624d563 = new Horizontal() {Margin=new Thickness(10),InstanceName="Default"}; /*  */
            var id_6204c104c66c491a95de9f27e63c7b03 = new Horizontal() {Margin=new Thickness(10),InstanceName="Default"}; /*  */
            var id_639560130886407fb5467cb4377f094d = new Horizontal() {Margin=new Thickness(10),InstanceName="Default"}; /*  */
            var id_64df3c3c48964e39a2d168b5926bfbb5 = new Horizontal() {Margin=new Thickness(10),InstanceName="Default"}; /*  */
            var id_656c2b1a985d4984811244b5214736a8 = new Horizontal() {Margin=new Thickness(10),InstanceName="Default"}; /*  */
            var id_66d7b3d9edd04f538230869f755f8be3 = new Horizontal() {Margin=new Thickness(10),InstanceName="Default"}; /*  */
            var id_6b002e2f5d584cb787a9bbf74c0dd5a4 = new Horizontal() {Margin=new Thickness(10),InstanceName="Default"}; /*  */
            var id_6bc75430a8b54e0f9992f1376902374a = new Horizontal() {Margin=new Thickness(10),InstanceName="Default"}; /*  */
            var id_6bfa857fac2a402597a9c277da666064 = new Horizontal() {Margin=new Thickness(10),InstanceName="Default"}; /*  */
            var id_72771c1a98ee406e92cce7907e940a97 = new Horizontal() {Margin=new Thickness(10),InstanceName="Default"}; /*  */
            var id_7adc18c3e8bd45a3a7c066b04f8bd76b = new Horizontal() {Margin=new Thickness(10),InstanceName="Default"}; /*  */
            var id_7cc96389fc1c4e6b826163b0d8e3ebee = new Horizontal() {Margin=new Thickness(10),InstanceName="Default"}; /*  */
            var id_83e6183188e14047be5921ca96a7e670 = new Horizontal() {Margin=new Thickness(10),InstanceName="Default"}; /*  */
            var id_866886f9868b4d71ae7704ad2e302746 = new Horizontal() {Margin=new Thickness(10),InstanceName="Default"}; /*  */
            var id_8a9162efa4ce4d86860d5f72e04aabe8 = new Horizontal() {Margin=new Thickness(10),InstanceName="Default"}; /*  */
            var id_90efe4ac8e3a4c9fbf01aa3768ef31b5 = new Horizontal() {Margin=new Thickness(10),InstanceName="Default"}; /*  */
            var id_9315cf1f2761454f8f91154d01c7c5c0 = new Horizontal() {Margin=new Thickness(10),InstanceName="Default"}; /*  */
            var id_94253900dd1940ea8dfe2756ef97e88d = new Horizontal() {Margin=new Thickness(10),InstanceName="Default"}; /*  */
            var id_98dab0214b0d4d3c8794321adb485b85 = new Horizontal() {Margin=new Thickness(10),InstanceName="Default"}; /*  */
            var id_998bc95c9d35482db1cf8dc3c042f084 = new Horizontal() {Margin=new Thickness(10),InstanceName="Default"}; /*  */
            var id_99bcacf4a6ff49ab931d1425c9f4ff26 = new Horizontal() {Margin=new Thickness(10),InstanceName="Default"}; /*  */
            var id_9f1b76f87b864673987734704cd8903e = new Horizontal() {Margin=new Thickness(10),InstanceName="Default"}; /*  */
            var id_a3039a6c2ac4488692ad366290f95c64 = new Horizontal() {Margin=new Thickness(10),InstanceName="Default"}; /*  */
            var id_a36652086c4446f9bc05e9a00c8c3177 = new Horizontal() {Margin=new Thickness(10),Visibility=Visibility.Collapsed,InstanceName="Default"}; /*  */
            var id_a5211e05f4074ce693e8f681c6b2f704 = new Horizontal() {Ratios=new int[]{20, 80},Margin=new Thickness(10),Visibility=Visibility.Collapsed,InstanceName="Default"}; /*  */
            var id_a9ed7d035c2541da9dbfda67c46bd92e = new Horizontal() {Margin=new Thickness(10),InstanceName="Default"}; /*  */
            var id_ab4f6f6a905a4afe83ef36aabf903fa3 = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /*  */
            var id_b042b3662b594f5e99318f98dbf0822b = new Horizontal() {Margin=new Thickness(10),InstanceName="Default"}; /*  */
            var id_b0bc2f9bac5746259ce9d003a2e9a33e = new Horizontal() {Margin=new Thickness(10),InstanceName="Default"}; /*  */
            var id_b7b9e353a8c54692b3983629f1648c3e = new Horizontal() {Margin=new Thickness(10),InstanceName="Default"}; /*  */
            var id_b8f160c9b5f84d45bf34c4b9adcaf9fe = new Horizontal() {Margin=new Thickness(10),InstanceName="Default"}; /*  */
            var id_ba6ba3a68eae48cabc72ae95b9f58add = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /*  */
            var id_bb3a7a570457420aa5b9c06d5ab99ff5 = new Horizontal() {Margin=new Thickness(10),InstanceName="Default"}; /*  */
            var id_bb678b85e2eb485182b99f30adc6d76f = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /*  */
            var id_bc307cec648343a099ef75f02e3d4cc9 = new Horizontal() {Margin=new Thickness(10),InstanceName="Default"}; /*  */
            var id_c5ed553d124b4027a98352a58c52a81f = new Horizontal() {Ratios=new int[]{40, 40, 20},Margin=new Thickness(5),InstanceName="Default"}; /*  */
            var id_cbf60ba3c5ab4a5a847e79019f216a01 = new Horizontal() {Margin=new Thickness(10),InstanceName="Default"}; /*  */
            var id_cda9bb60c76e4f23aaa0e371a23ff46c = new Horizontal() {InstanceName="Default"}; /*  */
            var id_d6b8c97519914ba5a636e8b3c906d498 = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /*  */
            var id_d87e3d86a09c4e54b3a296d146f0d9ed = new Horizontal() {Margin=new Thickness(10),InstanceName="Default"}; /*  */
            var id_d8cae10f88e04444b90020f8346f0124 = new Horizontal() {Margin=new Thickness(10),InstanceName="Default"}; /*  */
            var id_da427f3354c0453fa309652bd27a6598 = new Horizontal() {InstanceName="Default"}; /*  */
            var id_dba459e9eea04a61a8d0c1a29765190b = new Horizontal() {Margin=new Thickness(10),InstanceName="Default"}; /*  */
            var id_e01ac7d1e3ed4d4aa9882c62d047477d = new Horizontal() {Margin=new Thickness(10),InstanceName="Default"}; /*  */
            var id_e0983e688ae94e53926652d8cef35e56 = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /*  */
            var id_e1d0d849589446a69d3b3542f54406c2 = new Horizontal() {Margin=new Thickness(10),InstanceName="Default"}; /*  */
            var id_e6dc9510f02e4307ac5dbfa69ddf79eb = new Horizontal() {Ratios=new int[]{5, 95},Margin=new Thickness(20, 0, 0, 0),InstanceName="Default"}; /*  */
            var id_eac79c6c75ec47c0a952e68fb6a9e3e4 = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /*  */
            var id_ebad3d064a304a5b8353a88e016d151b = new Horizontal() {Margin=new Thickness(10),InstanceName="Default"}; /*  */
            var id_f2966e850a5e4508a0e06c3fea49c18b = new Horizontal() {InstanceName="Default"}; /*  */
            var id_f66039bac47943a1865c046b3330c801 = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /*  */
            var id_f68fc132f97b436abf905b5a8b76e2f9 = new Horizontal() {Margin=new Thickness(10),InstanceName="Default"}; /*  */
            var id_f77cfa63e3d64602b39e0f4d9a22b90e = new Horizontal() {Margin=new Thickness(10),InstanceName="Default"}; /*  */
            var id_fe6a9da6596448099b556ef2b0baf606 = new Horizontal() {Margin=new Thickness(10),InstanceName="Default"}; /*  */
            var mihubCheckboxHoriz = new Horizontal() {Margin=new Thickness(0, 10, 0, 0),InstanceName="mihubCheckboxHoriz"}; /*  */
            var mihubLoginSubmitAndForgotPassHoriz = new Horizontal() {Margin=new Thickness(0, 10, 0, 0),InstanceName="mihubLoginSubmitAndForgotPassHoriz"}; /*  */
            var mihubPasswordForm = new Horizontal() {Margin=new Thickness(0, 10, 0, 0),InstanceName="mihubPasswordForm"}; /*  */
            var MiHubRegisterAddressLine1Horiz = new Horizontal() {Margin=new Thickness(10),Visibility=Visibility.Collapsed,InstanceName="MiHubRegisterAddressLine1Horiz"}; /*  */
            var MiHubRegisterAddressLine2Horiz = new Horizontal() {Margin=new Thickness(10),Visibility=Visibility.Collapsed,InstanceName="MiHubRegisterAddressLine2Horiz"}; /*  */
            var mihubRegisterButtonHoriz = new Horizontal() {Margin=new Thickness(0, 10, 0, 0),InstanceName="mihubRegisterButtonHoriz"}; /*  */
            var MiHubRegisterCityHoriz = new Horizontal() {Margin=new Thickness(10),Visibility=Visibility.Collapsed,InstanceName="MiHubRegisterCityHoriz"}; /*  */
            var MiHubRegisterConfirmPasswordHoriz = new Horizontal() {Margin=new Thickness(10),Visibility=Visibility.Collapsed,InstanceName="MiHubRegisterConfirmPasswordHoriz"}; /*  */
            var MiHubRegisterCountryHoriz = new Horizontal() {Margin=new Thickness(10),Visibility=Visibility.Collapsed,InstanceName="MiHubRegisterCountryHoriz"}; /*  */
            var MiHubRegisterEmailHoriz = new Horizontal() {Margin=new Thickness(10),InstanceName="MiHubRegisterEmailHoriz"}; /*  */
            var MiHubRegisterFirstNameHoriz = new Horizontal() {Margin=new Thickness(10),InstanceName="MiHubRegisterFirstNameHoriz"}; /*  */
            var MiHubRegisterLastNameHoriz = new Horizontal() {Margin=new Thickness(10),InstanceName="MiHubRegisterLastNameHoriz"}; /*  */
            var MiHubRegisterPasswordHoriz = new Horizontal() {Margin=new Thickness(10),InstanceName="MiHubRegisterPasswordHoriz"}; /*  */
            var MiHubRegisterPhoneNumberHoriz = new Horizontal() {Margin=new Thickness(10),Visibility=Visibility.Collapsed,InstanceName="MiHubRegisterPhoneNumberHoriz"}; /*  */
            var MiHubRegisterPostcodeHoriz = new Horizontal() {Margin=new Thickness(10),Visibility=Visibility.Collapsed,InstanceName="MiHubRegisterPostcodeHoriz"}; /*  */
            var MiHubRegisterRegionHoriz = new Horizontal() {Margin=new Thickness(10),Visibility=Visibility.Collapsed,InstanceName="MiHubRegisterRegionHoriz"}; /*  */
            var MiHubRegisterRevealedPasswordForm = new Horizontal() {Margin=new Thickness(10),Visibility=Visibility.Collapsed,InstanceName="MiHubRegisterRevealedPasswordForm"}; /*  */
            var MiHubRegisterSubmitButtonHoriz = new Horizontal() {Margin=new Thickness(10),InstanceName="MiHubRegisterSubmitButtonHoriz"}; /*  */
            var MiHubRegisterSuburbHoriz = new Horizontal() {Margin=new Thickness(10),Visibility=Visibility.Collapsed,InstanceName="MiHubRegisterSuburbHoriz"}; /*  */
            var mihubRevealedPasswordForm = new Horizontal() {Margin=new Thickness(0, 10, 0, 0),Visibility=Visibility.Collapsed,InstanceName="mihubRevealedPasswordForm"}; /*  */
            var mihubStatusMessageHoriz = new Horizontal() {Margin=new Thickness(0, 10, 0, 0),Visibility=Visibility.Collapsed,InstanceName="mihubStatusMessageHoriz"}; /*  */
            var mihubUsernameForm = new Horizontal() {Margin=new Thickness(0, 10, 0, 0),InstanceName="mihubUsernameForm"}; /*  */
            var nlisAgentTransactionOptionbox = new Horizontal(visible:false) {Margin=new Thickness(10),InstanceName="nlisAgentTransactionOptionbox"}; /*  */
            var nlisProducerTransactionOptionBox = new Horizontal(visible:false) {Margin=new Thickness(10),InstanceName="nlisProducerTransactionOptionBox"}; /*  */
            var nlisSaleyardTransactionOptionBox = new Horizontal(visible:false) {Margin=new Thickness(10),InstanceName="nlisSaleyardTransactionOptionBox"}; /*  */
            var checkInternetConnection = new HttpRequest("https://www.google.com") {InstanceName="checkInternetConnection",JustPing=true}; /*  */
            var id_e8d36ce5ca2d44b290d6146451af41e2 = new HttpRequest("https://livestock.mihub.tru-test.com/jwt/login") {InstanceName="Default",PostContent=new Dictionary<string, string>(){{"grant_type", "refresh_token"}}}; /*  */
            var MiHubDeleteAllSessionsAndAnimals = new HttpRequest("https://livestock.mihub.tru-test.com/odata/Farms/Services.DeleteAll") {InstanceName="MiHubDeleteAllSessionsAndAnimals"}; /*  */
            var MiHubEmailLinkToResetPassword = new HttpRequest("https://livestock.mihub.tru-test.com/ForgotPassword/SendLinkToResetPassword") {InstanceName="MiHubEmailLinkToResetPassword"}; /*  */
            var MiHubGetAccessTokenFromRefresh = new HttpRequest("https://livestock.mihub.tru-test.com/jwt/login") {InstanceName="MiHubGetAccessTokenFromRefresh",PostContent=new Dictionary<string, string>(){{"grant_type", "refresh_token"}}}; /*  */
            var MiHubGetUserFarmsHttpRequest = new HttpRequest("https://livestock.mihub.tru-test.com/odata/Users?$expand=userFarms&$select=userFarms,currentFarm_FarmId") {InstanceName="MiHubGetUserFarmsHttpRequest",requestMethod=HttpMethod.Get}; /*  */
            var MiHubLoginAfterRegistration = new HttpRequest("https://livestock.mihub.tru-test.com/jwt/login") {InstanceName="MiHubLoginAfterRegistration",PostContent=new Dictionary<string, string>(){{"grant_type", "password"}}}; /*  */
            var MiHubLoginWithUserPass = new HttpRequest("https://livestock.mihub.tru-test.com/jwt/login") {InstanceName="MiHubLoginWithUserPass",PostContent=new Dictionary<string, string>(){{"grant_type", "password"}}}; /*  */
            var MiHubRegisterHttpRequest = new HttpRequest("https://livestock.mihub.tru-test.com/jwt/register") {InstanceName="MiHubRegisterHttpRequest"}; /*  */
            var MiHubUpdateFarmHttpRequest = new HttpRequest() {InstanceName="MiHubUpdateFarmHttpRequest"}; /*  */
            var MiHubUpdateUserHttpRequest = new HttpRequest() {InstanceName="MiHubUpdateUserHttpRequest"}; /*  */
            var MiHubUploadSession = new HttpRequest("https://livestock.mihub.tru-test.com/Csv/Session") {InstanceName="MiHubUploadSession"}; /*  */
            var naitHttpRequest = new HttpRequest("https://staging.livestock.mihub.tru-test.com/api/AnimalMovement") {InstanceName="naitHttpRequest",SendRequestOnFlag=true}; /*  */
            var nlisHttpRequest = new HttpRequest("https://staging.livestock.mihub.tru-test.com/api/AnimalMovement") {InstanceName="nlisHttpRequest",SendRequestOnFlag=true}; /*  */
            var id_18409fc50fb44553b72b059afcb49cc9 = new IfElse() {InstanceName="Default"}; /*  */
            var id_2775b93b0a3343ddb9bde9f816634675 = new IfElse() {InstanceName="Default"}; /*  */
            var id_2ef417ff26cb4accb54bb2202fd282d8 = new IfElse() {InstanceName="Default"}; /*  */
            var id_3kshshqtmev9pnslp39m84p2dd = new IfElse() {InstanceName="Default"}; /*  */
            var id_4b38fa419af0426eac26e945bb307148 = new IfElse() {InstanceName="Default"}; /*  */
            var id_53e5e708283d4996bfcbd804c6669efa = new IfElse() {InstanceName="Default"}; /*  */
            var id_55abc935e20b4b0cb4ce2f9df82cf80a = new IfElse() {InstanceName="Default"}; /*  */
            var id_5667e7340a524c50951dbd19b51116f1 = new IfElse() {InstanceName="Default"}; /*  */
            var id_683fc02136f7430082056fa06e14c37f = new IfElse() {InstanceName="Default"}; /*  */
            var id_78d33d633d454dc6a862cb1638e6a785 = new IfElse() {InstanceName="Default"}; /*  */
            var id_7a3ac9990cf44bf494fd9b52cb07caec = new IfElse() {InstanceName="Default"}; /*  */
            var id_87c830dc159140ef8a2569de4ae4ca7d = new IfElse() {InstanceName="Default"}; /*  */
            var id_9a3dce085ff64d0c96197c0a6e578b93 = new IfElse() {InstanceName="Default"}; /*  */
            var id_aefa2b5a4a72443e8b0e7150502b066d = new IfElse() {InstanceName="Default"}; /*  */
            var id_bb43c755bbb94b408af6f22becf3741e = new IfElse() {InstanceName="Default"}; /*  */
            var id_c380f6df3dd84f5b928e1340b6ab937b = new IfElse() {InstanceName="Default"}; /*  */
            var id_c65580cbfe03402e9592e9f8170c4b03 = new IfElse() {InstanceName="Default"}; /*  */
            var id_f5e3b298728247d7a8a3635566934336 = new IfElse() {InstanceName="Default"}; /*  */
            var id_ffbbd9e4ba6e4d8cb9cff53774318346 = new IfElse() {InstanceName="Default"}; /*  */
            var id_1adc13ee7db64437bf2737342729407b = new JSONParser() {InstanceName="Default",JSONPath="$..access_token"}; /*  */
            var id_2548234f7f7a4d3291dc13c766c51540 = new JSONParser() {InstanceName="Default",JSONPath="$..value[0].currentFarm_FarmId"}; /*  */
            var id_4d17241aa5204137bad3cfa1a01ed732 = new JSONParser() {InstanceName="Default",JSONPath="$..error.message"}; /*  */
            var id_ae924356f83343afb2873a8c72b6e128 = new JSONParser() {InstanceName="Default",Configuration=new Dictionary<string, string>(){{"access_token", "$..access_token"}, {"refresh_token", "$..refresh_token"}}}; /*  */
            var id_c9d7f71775a3497d8fcdfb78f118d8bc = new JSONParser() {InstanceName="Default",Configuration=new Dictionary<string, string>(){{"error", "$..error"}, {"errorMessage", "$..error.innererror[0].description"}, {"rowsProcessed", "$..value[0].rowsProcessed"}}}; /*  */
            var id_f6ac589e2e064743aeee634b5f5ee397 = new JSONParser() {InstanceName="Default",JSONPath="$..nameid"}; /*  */
            var integrationRequestResponseParser = new JSONParser() {InstanceName="integrationRequestResponseParser",Configuration=new Dictionary<string, string>(){{"main message", "$..message"}, {"animalSuccesses", "$..['animalSuccesses']"}, {"animalErrors", "$..['animalErrors']"}, {"messages", "$..modelState.*[0]"}, {"animalTransferResult", "$..animalTransferResult"}}}; /*  */
            var storeMiHubTokensLocally = new JSONWriter<Dictionary<string, string>>(@"C:\ProgramData\Tru-Test\DataLink_ALA\userinfo.json") {InstanceName="storeMiHubTokensLocally"}; /*  */
            var id_2559a20025694a6c8f139120e2402147 = new KeyValue<int>() {InstanceName="Default",Dict=new Dictionary<string, int>(){{"Not authorised", 0}, {"Source PIC or vendor", 1}, {"Destination PIC or buyer", 2}, {"One or both source PIC", 3}}}; /*  */
            var id_2540b0e36b1a465689546bde6896b821 = new KeyValue<JToken>("animalSuccesses") {InstanceName="Default"}; /*  */
            var id_33b397262af848838805e10e29697a09 = new KeyValue<JToken>("errorMessage") {InstanceName="Default"}; /*  */
            var id_46ce5c4a4183496f8e7e5a7ab3fbaf4a = new KeyValue<JToken>("animalTransferResult") {InstanceName="Default"}; /*  */
            var id_8f72fc59cdc1482a80b7762e34770c05 = new KeyValue<JToken>("messages") {InstanceName="Default"}; /*  */
            var id_c88ce58c41064245beac0489a11a4dbf = new KeyValue<JToken>("animalErrors") {InstanceName="Default"}; /*  */
            var id_d4453910e9d5421aad5118628de55f5e = new KeyValue<JToken>("rowsProcessed") {InstanceName="Default"}; /*  */
            var id_d7cc872e4322498c9fba9d1d6a6ff738 = new KeyValue<JToken>("error") {InstanceName="Default"}; /*  */
            var id_e530b6c894324de68ee59b13b15c1c10 = new KeyValue<JToken>("main message") {InstanceName="Default"}; /*  */
            var getMiHubTokensFromDict = new KeyValue<string>() {InstanceName="getMiHubTokensFromDict",Keys=new List<string>(){"access_token", "refresh_token"}}; /*  */
            var id_28rn2rsmjrmspcm7c08lad73h0 = new KeyValue<string>() {InstanceName="Default",Key="access_token"}; /*  */
            var id_ab7ae585f8be44ccb53f113f138ff89f = new KeyValue<string>() {InstanceName="Default",Key="refresh_token"}; /*  */
            var naitAnimalTypeCodes = new KeyValue<string>() {InstanceName="naitAnimalTypeCodes",Dict=new Dictionary<string, string>(){{"Cattle", "C"}, {"Deer", "D"}}}; /*  */
            var naitProductionTypeCodes = new KeyValue<string>() {InstanceName="naitProductionTypeCodes",Dict=new Dictionary<string, string>(){{"Dairy", "D"}, {"Beef", "B"}, {"None", "N"}, {"Venison", "VE"}, {"Velvet", "VT"}, {"Trophy", "TR"}}}; /*  */
            var mihubLoginForm = new LoginForm() {Margin=new Thickness(20),InstanceName="mihubLoginForm"}; /*  */
            var eidSummaryLoop = new Loop<string>() {InstanceName="eidSummaryLoop"}; /*  */
            var id_ae5f2fc9d6294014baea6ef1adb5b6f8 = new Loop<string>() {InstanceName="Default"}; /*  */
            var MiHubLoopThroughFilePaths = new Loop<string>() {InstanceName="MiHubLoopThroughFilePaths"}; /*  */
            var datamarsLivestockMenu = new Menu("Datamars Livestock") {InstanceName="datamarsLivestockMenu"}; /*  */
            var MiHubDeleteAllSessionsAndAnimalsMenuItem = new MenuItem("Delete all sessions and animals") {InstanceName="MiHubDeleteAllSessionsAndAnimalsMenuItem"}; /*  */
            var MiHubForgotPasswordMenuItem = new MenuItem("Send link to reset password") {InstanceName="MiHubForgotPasswordMenuItem"}; /*  */
            var MiHubLoginMenuItem = new MenuItem("Log in") {InstanceName="MiHubLoginMenuItem"}; /*  */
            var MiHubLogoutMenuItem = new MenuItem("Log out") {InstanceName="MiHubLogoutMenuItem"}; /*  */
            var MiHubRegisterUser = new MenuItem("Register") {InstanceName="MiHubRegisterUser"}; /*  */
            var MiHubUploadTickedSessionsMenuItem = new MenuItem("Upload selected sessions") {InstanceName="MiHubUploadTickedSessionsMenuItem"}; /*  */
            var naitMovementMenu = new MenuItem("Start a NAIT integration request") {InstanceName="naitMovementMenu"}; /*  */
            var nlisMovementMenu = new MenuItem("Start an NLIS integration request") {InstanceName="nlisMovementMenu"}; /*  */
            var id_045ab76fe63042a0aa615641c4804568 = new Not() {InstanceName="Default"}; /*  */
            var id_0nvjhkf3agvr54tq3ho2fd8loo = new Not() {InstanceName="Default"}; /*  */
            var id_2df1fb7b1f5745b99e438c3d3ca301bc = new Not() {InstanceName="Default"}; /*  */
            var id_448b8a96f88f43d8b9737389f7ab4c38 = new Not() {InstanceName="Default"}; /*  */
            var id_7a8ttb8o1del67sriht4a3e52t = new Not() {InstanceName="Default"}; /*  */
            var id_8a773709e88a4658a6c0169bcc93b23a = new Not() {InstanceName="Default"}; /*  */
            var id_a84323d6b300407fb8c072138e780253 = new Not() {InstanceName="Default"}; /*  */
            var id_de3a77bdfe514e4ca90c8190c75f5648 = new Not() {InstanceName="Default"}; /*  */
            var id_ec977563cf2e4bf8b01beca7e6596239 = new Not() {InstanceName="Default"}; /*  */
            var id_74e03da34db240ce9b57802d3d8118dc = new OpenWebBrowser("https://app.livestock.datamars.com/app/#/terms-and-conditions") {InstanceName="Default"}; /*  */
            var id_b114566d7609404caf617d4afa325c3e = new OpenWebBrowser("https://app.livestock.datamars.com/app/#/privacy-policy") {InstanceName="Default"}; /*  */
            var id_2189bf232d1d41ef913d50ff7071c291 = new OptionBox() {InstanceName="Default",DefaultTitle="Select"}; /*  */
            var id_336063780bde46e7a066dff96fd6ac31 = new OptionBox() {InstanceName="Default",DefaultTitle="Select"}; /*  */
            var id_69b0483ed2cc453b9e9db1b4a6ae3626 = new OptionBox(width:140) {InstanceName="Default",DefaultTitle="None"}; /*  */
            var id_73ba94ac97a541a496540b545466204a = new OptionBox() {InstanceName="Default",DefaultTitle="Select"}; /*  */
            var id_83da74d940d0447aa61f86214507aae7 = new OptionBox(width:140) {InstanceName="Default",DefaultTitle="None"}; /*  */
            var id_85cbd2e4532b43bf8967b1bfb03dec7c = new OptionBox() {InstanceName="Default",DefaultTitle="Select"}; /*  */
            var id_9bf2f32a1b8a4e3f893e7ef14748936f = new OptionBox(width:140) {InstanceName="Default",DefaultTitle="Cattle"}; /*  */
            var id_b11aee306b4b4e1fb256c20d49386e93 = new OptionBox(width:140) {InstanceName="Default",DefaultTitle="Animal Registration"}; /*  */
            var id_b237b2d63a884f20970975afb770f663 = new OptionBox() {InstanceName="Default",DefaultTitle="Select"}; /*  */
            var id_06b3003a6e3e4d4a976e0e945e3fefa9 = new OptionBoxItem("Select") {InstanceName="Default"}; /*  */
            var id_0892202059054c0e86392d6e68776ea3 = new OptionBoxItem("Producer") {InstanceName="Default"}; /*  */
            var id_1ba6f3ab67634930b124978916995aff = new OptionBoxItem("Dairy") {InstanceName="Default"}; /*  */
            var id_2583994af4dd4c2e9817b8ac19d004ea = new OptionBoxItem("Cattle") {InstanceName="Default"}; /*  */
            var id_266167db72bd40749fd6c53fbcee7be2 = new OptionBoxItem("Sending") {InstanceName="Default"}; /*  */
            var id_2fa1bee29a884d9d9897ab5a5374d32a = new OptionBoxItem("None") {InstanceName="Default"}; /*  */
            var id_301bc1388210455a93af5233e89f5830 = new OptionBoxItem("Velvet") {InstanceName="Default"}; /*  */
            var id_4dc57149d5824468aca5028198df7e57 = new OptionBoxItem("Agent") {InstanceName="Default"}; /*  */
            var id_5567334a4d364ab79ac6eea5eb9cf426 = new OptionBoxItem("Trophy") {InstanceName="Default"}; /*  */
            var id_5f25a2b6ad334037b8dca7e5fa66edaa = new OptionBoxItem("Venison") {InstanceName="Default"}; /*  */
            var id_61ac04e7fadb4a4a80f7abbe1f3c561f = new OptionBoxItem("To Saleyard") {InstanceName="Default"}; /*  */
            var id_6df343b06c064f1b9dd681b5cd97bbce = new OptionBoxItem("Select") {InstanceName="Default"}; /*  */
            var id_74a8ecbb39604244a520002d02325975 = new OptionBoxItem("Sending Movement") {InstanceName="Default"}; /*  */
            var id_74e348ddb9ad450ebbb1742f6b46a6e7 = new OptionBoxItem("Destination PIC or buyer") {InstanceName="Default"}; /*  */
            var id_77634f1da61649ac92923ddedcdba8e0 = new OptionBoxItem("Sending") {InstanceName="Default"}; /*  */
            var id_7b86c57b469743769fc34d0df8c139c8 = new OptionBoxItem("Animal Registration") {InstanceName="Default"}; /*  */
            var id_7fa1036605c74d778c7f2e49cce92ddf = new OptionBoxItem("One or both source PIC") {InstanceName="Default"}; /*  */
            var id_80c109ef616b4d9291ea1aeae263dc67 = new OptionBoxItem("Receiving") {InstanceName="Default"}; /*  */
            var id_8213f6658cd44689babf6b49569ef67c = new OptionBoxItem("Beef") {InstanceName="Default"}; /*  */
            var id_8eb72df43dae4ad9888959b08b3edde2 = new OptionBoxItem("Receiving Movement") {InstanceName="Default"}; /*  */
            var id_95c078ea9e4e444dbf31a7356853d223 = new OptionBoxItem("Select") {InstanceName="Default"}; /*  */
            var id_ad47c5b2c4fd4013890e8437b13a0a10 = new OptionBoxItem("Deer") {InstanceName="Default"}; /*  */
            var id_b19b64cd4422486a8f2dd309d10646e8 = new OptionBoxItem("From Saleyard") {InstanceName="Default"}; /*  */
            var id_b487e314581745b0addf24d9ea714d67 = new OptionBoxItem("Not authorised") {InstanceName="Default"}; /*  */
            var id_c863ad350ac94c63bf14678db5605cc6 = new OptionBoxItem("Third party") {InstanceName="Default"}; /*  */
            var id_d30558d6137344ad9b3b8169a1097151 = new OptionBoxItem("Receiving") {InstanceName="Default"}; /*  */
            var id_d497b23195524132ba89ef21422901e8 = new OptionBoxItem("Source PIC or vendor") {InstanceName="Default"}; /*  */
            var id_d794a1c7dc694b4290af49bc2ba88c95 = new OptionBoxItem("None") {InstanceName="Default"}; /*  */
            var id_f7253718f0d04c758f029f5530cb55f4 = new OptionBoxItem("Saleyard") {InstanceName="Default"}; /*  */
            var id_0072b66a9562483db25ee7675fee3a18 = new Pair<string, DateTime>() {InstanceName="Default"}; /*  */
            var id_62e7cc7be118417e80b690f81202d8b6 = new Pair<string, DateTime>() {InstanceName="Default"}; /*  */
            var id_ee103defd55941458ec83a507ce6f696 = new Pair<string, DateTime>() {InstanceName="Default"}; /*  */
            var id_6c2bdbb762054efa9d5ba7f034cca638 = new Pair<string, int>() {InstanceName="Default"}; /*  */
            var id_014b61a91c364ef0a5b208c15cefb5de = new Pair<string, JToken>() {Item1="MovementDate",InstanceName="Default"}; /*  */
            var id_41c7a8a6f50e42c8a0daa93a6904c0cc = new Pair<string, JToken>() {Item1="MovementDate",InstanceName="Default"}; /*  */
            var id_52935ea9b8e748808816155a77eb29ef = new Pair<string, JToken>() {Item1="IntegrationType",InstanceName="Default"}; /*  */
            var id_697de7541ee74c1790a53ab5c654da54 = new Pair<string, JToken>() {Item1="MovementDate",InstanceName="Default"}; /*  */
            var id_76964d73ab16452faf22efb04ef4ad3e = new Pair<string, JToken>() {Item1="Date",InstanceName="Default"}; /*  */
            var id_7df06b94af2a4440b7914856430a2bec = new Pair<string, JToken>() {Item1="AnimalMovementType",InstanceName="Default"}; /*  */
            var id_7ff3876c4c714cd291b4f09ed396e468 = new Pair<string, JToken>() {Item1="Date",InstanceName="Default"}; /*  */
            var id_9a233d8e1b6f4752804859ad25aca2a1 = new Pair<string, JToken>() {Item1="Date",InstanceName="Default"}; /*  */
            var id_c0e910de5fa346ae94734210f24755d6 = new Pair<string, JToken>() {Item1="DisclaimerAccepted",InstanceName="Default"}; /*  */
            var id_c91ea6ab84f142168fc744d83055b19e = new Pair<string, JToken>() {Item1="Date",InstanceName="Default"}; /*  */
            var id_ddc6d323e71d4c248e03b4dfae20bf89 = new Pair<string, JToken>() {Item1="AuthorisationLevel",InstanceName="Default"}; /*  */
            var id_df4e7e9e6a5443af942e39cfd40603f6 = new Pair<string, JToken>() {Item1="IsStaging",InstanceName="Default"}; /*  */
            var id_e0a7320958984ff2a6bc26c6722a1040 = new Pair<string, JToken>() {Item1="MovementDate",InstanceName="Default"}; /*  */
            var id_e2137239798a4109a582d627c8820df0 = new Pair<string, JToken>() {Item1="MovementDate",InstanceName="Default"}; /*  */
            var id_f399f412e087413c91a8e3cfd1b674c1 = new Pair<string, JToken>() {Item1="Date",InstanceName="Default"}; /*  */
            var id_fc6be78b45874bfbbc212bcc1f40c1c4 = new Pair<string, JToken>() {Item1="ExternalAnimalIds",InstanceName="Default"}; /*  */
            var id_03672039bece413e9e5980def681fe84 = new Pair<string, string>() {Item1="PicFrom",InstanceName="Default"}; /*  */
            var id_0485ca43fd6147c499812456d61c1eb1 = new Pair<string, string>() {InstanceName="Default"}; /*  */
            var id_0b0f29eaabca462cacbd15d2fa64d11e = new Pair<string, string>() {Item1="PicFrom",InstanceName="Default"}; /*  */
            var id_0b31a771a39247b687f3126a6f5e7548 = new Pair<string, string>() {Item1="SerialNumber",InstanceName="Default"}; /*  */
            var id_14d0c11ef4fc48f9af63b3b2b8168346 = new Pair<string, string>() {Item1="OtherClientKey",InstanceName="Default"}; /*  */
            var id_15139c45fde04f8d80975026955d61c7 = new Pair<string, string>() {Item1="AuthoriserLastName",InstanceName="Default"}; /*  */
            var id_255e0cc68a4944e9947177c05e024d79 = new Pair<string, string>() {InstanceName="Default"}; /*  */
            var id_294392c31a284f38abb8780073cc8dd7 = new Pair<string, string>() {InstanceName="Default"}; /*  */
            var id_2fe9bf09ed0742958f031075a4c4c08e = new Pair<string, string>() {Item1="FreeText",InstanceName="Default"}; /*  */
            var id_3b5f6a8060de4091914af93eb36e5d88 = new Pair<string, string>() {Item1="OtherClientKey",InstanceName="Default"}; /*  */
            var id_4de4af0dd8854b7385bd8f1c0e20aa3b = new Pair<string, string>() {Item1="PicTo",InstanceName="Default"}; /*  */
            var id_5aead95e76514f8991de08769de4760a = new Pair<string, string>() {Item1="SerialNumber",InstanceName="Default"}; /*  */
            var id_6443b9839bb4423188f60bf081febcba = new Pair<string, string>() {Item1="AuthoriserFirstName",InstanceName="Default"}; /*  */
            var id_65eb47575ca948b4869480e51e1f7526 = new Pair<string, string>() {InstanceName="Default"}; /*  */
            var id_6774c022a717401d803bd9afb1ea894d = new Pair<string, string>() {Item1="SerialNumber",InstanceName="Default"}; /*  */
            var id_67ce72bd41ff40edb712a22231e55bca = new Pair<string, string>() {Item1="OtherClientKey",InstanceName="Default"}; /*  */
            var id_71bca7d0003f4225b1ba2342a172dba7 = new Pair<string, string>() {Item1="FreeText",InstanceName="Default"}; /*  */
            var id_7af94faf1ebe411d84913a53599b70f6 = new Pair<string, string>() {Item1="SerialNumber",InstanceName="Default"}; /*  */
            var id_8113f9d89f5345ffb6c9abcb89949097 = new Pair<string, string>() {Item1="FreeText",InstanceName="Default"}; /*  */
            var id_8292fb4e95874a65a80709eadaab9cf7 = new Pair<string, string>() {Item1="OtherClientKey",InstanceName="Default"}; /*  */
            var id_833c5ff0b4ca4f96a6de4a74dd3d51ed = new Pair<string, string>() {Item1="region",InstanceName="Default"}; /*  */
            var id_8e0de3cb26684ebb99c0862618bb750a = new Pair<string, string>() {Item1="SerialNumber",InstanceName="Default"}; /*  */
            var id_91bf51e9edcd43fa810e514f3b0d3bbe = new Pair<string, string>() {Item1="JsonPayload",InstanceName="Default"}; /*  */
            var id_9fe5a772d2c147829fa1abc8c4519c38 = new Pair<string, string>() {Item1="PicTo",InstanceName="Default"}; /*  */
            var id_a1ffbb81d9b044d191ffad1e8c160a63 = new Pair<string, string>() {Item1="ClientKey",InstanceName="Default"}; /*  */
            var id_a2f81ad95411456eb816c998fdc3369e = new Pair<string, string>() {InstanceName="Default"}; /*  */
            var id_a436f243d37d472795bb765052517fb7 = new Pair<string, string>() {Item1="PicTo",InstanceName="Default"}; /*  */
            var id_ab7220e544ba4431aaac9e8ccb3c8e2a = new Pair<string, string>() {InstanceName="Default"}; /*  */
            var id_b7061a02ae5342beb54d8b0e3df30470 = new Pair<string, string>() {Item1="FreeText",InstanceName="Default"}; /*  */
            var id_b744f58d2f4144938194e1e7dfaff50d = new Pair<string, string>() {InstanceName="Default"}; /*  */
            var id_b83ceebfdd854be295f2513d0af97887 = new Pair<string, string>() {Item1="Password",InstanceName="Default"}; /*  */
            var id_b94a5b5ba544412b99e1021e7382e4fc = new Pair<string, string>() {Item1="FreeText",InstanceName="Default"}; /*  */
            var id_bdd4d8c336b34fbc83a6af9249625d12 = new Pair<string, string>() {Item1="PicFrom",InstanceName="Default"}; /*  */
            var id_d4979dfb8e6c4cbfb0f0be6883c7f600 = new Pair<string, string>() {Item1="country",InstanceName="Default"}; /*  */
            var id_d8dcf82c24564f71ae752e1f19ba5bc2 = new Pair<string, string>() {Item1="farmName",InstanceName="Default"}; /*  */
            var id_ddbdc05bbf2142c2a7c9f9621591a8e8 = new Pair<string, string>() {InstanceName="Default"}; /*  */
            var id_ecf1b76ebf444821a6ac4f298f7d84fb = new Pair<string, string>() {InstanceName="Default"}; /*  */
            var id_f0fe76cb8c654e2d995a6d46768f21fd = new Pair<string, string>() {Item1="Email",InstanceName="Default"}; /*  */
            var id_f7a12fdb5ffc407898985df3b703dcd7 = new Pair<string, string>() {Item1="OtherClientKey",InstanceName="Default"}; /*  */
            var id_fccdd18f44be4215a75bc7695f01cae4 = new Pair<string, string>() {Item1="Username",InstanceName="Default"}; /*  */
            var MiHubRegisterAddressLine1Pair = new Pair<string, string>() {InstanceName="MiHubRegisterAddressLine1Pair"}; /*  */
            var MiHubRegisterAddressLine2Pair = new Pair<string, string>() {InstanceName="MiHubRegisterAddressLine2Pair"}; /*  */
            var MiHubRegisterCityPair = new Pair<string, string>() {InstanceName="MiHubRegisterCityPair"}; /*  */
            var MiHubRegisterConfirmPasswordPair = new Pair<string, string>() {InstanceName="MiHubRegisterConfirmPasswordPair"}; /*  */
            var MiHubRegisterCountryPair = new Pair<string, string>() {InstanceName="MiHubRegisterCountryPair"}; /*  */
            var MiHubRegisterEmailPair = new Pair<string, string>() {InstanceName="MiHubRegisterEmailPair"}; /*  */
            var MiHubRegisterFirstNamePair = new Pair<string, string>() {InstanceName="MiHubRegisterFirstNamePair"}; /*  */
            var MiHubRegisterLastNamePair = new Pair<string, string>() {InstanceName="MiHubRegisterLastNamePair"}; /*  */
            var MiHubRegisterPasswordPair = new Pair<string, string>() {InstanceName="MiHubRegisterPasswordPair"}; /*  */
            var MiHubRegisterPhoneNumberPair = new Pair<string, string>() {InstanceName="MiHubRegisterPhoneNumberPair"}; /*  */
            var MiHubRegisterPostcodePair = new Pair<string, string>() {InstanceName="MiHubRegisterPostcodePair"}; /*  */
            var MiHubRegisterRegionPair = new Pair<string, string>() {InstanceName="MiHubRegisterRegionPair"}; /*  */
            var MiHubRegisterSuburbPair = new Pair<string, string>() {InstanceName="MiHubRegisterSuburbPair"}; /*  */
            var id_213st23bu9dl8pnm821tldos15 = new Panel() {Height=400,InstanceName="Default"}; /*  */
            var WaitingForMiHubPanel = new Panel() {Height=300,Background=new SolidColorBrush(Color.FromRgb(255, 255, 255)),InstanceName="WaitingForMiHubPanel"}; /*  */
            var id_5dd95fa680e64e4eb7b89e4b606d0b70 = new PasswordBox() {InstanceName="Default"}; /*  */
            var id_d6a12177c39f4ee5a72a91066485c49a = new PasswordBox() {InstanceName="Default"}; /*  */
            var id_e480193bcbd04e1a88873f26877f90d6 = new PasswordBox() {InstanceName="Default"}; /*  */
            var mihubPasswordInput = new PasswordBox() {InstanceName="mihubPasswordInput"}; /*  */
            var MiHubRegisterConfirmPasswordPasswordBox = new PasswordBox() {InstanceName="MiHubRegisterConfirmPasswordPasswordBox"}; /*  */
            var id_0a1am9i9vecfi73fa1m32nnbs9 = new Picture("spinner_30x30.gif") {Width=30,Height=30,InstanceName="Default"}; /*  */
            var id_0d484d19bc0e432dbdd91f0f1cc14af4 = new Picture("button_ok.png") {Width=32,InstanceName="Default"}; /*  */
            var id_119917d9a29a42f0a55ccf6143f7b076 = new PopupWindow("Select Farm") {InstanceName="Default",Width=400,Height=100}; /*  */
            var id_201d0f0cf6f048c792c5990652a2de84 = new PopupWindow("") {Resize=SizeToContent.WidthAndHeight,InstanceName="Default"}; /*  */
            var id_2b524b4fb9de456695b8b8d53e190673 = new PopupWindow("Registration Successful") {Resize=SizeToContent.WidthAndHeight,InstanceName="Default",Width=500,Height=200}; /*  */
            var id_407212f08905495e96b29736f737539b = new PopupWindow("Datamars Livestock Start Screen") {InstanceName="Default",Width=500,Height=500}; /*  */
            var id_48b062128c014f31915c1fff53bce098 = new PopupWindow("Result") {Resize=SizeToContent.WidthAndHeight,InstanceName="Default",Width=500,Height=200}; /*  */
            var id_6b72224ba43042ecbb66a99659902cae = new PopupWindow("Not Connected") {InstanceName="Default",Width=306,Height=500}; /*  */
            var id_6fcc7feb31034e50be2eca33d2cfdcd8 = new PopupWindow("Summary of Selected Session Files") {InstanceName="Default",Width=500,Height=500}; /*  */
            var id_a1fae4aad2fd4eb588782da37eb678d5 = new PopupWindow("NAIT Transaction Window") {InstanceName="Default",Width=600,Height=350}; /*  */
            var id_f03ed8965e104025aa6cd23f16161782 = new PopupWindow("Not Connected") {InstanceName="Default",Width=306,Height=500}; /*  */
            var mihubLoginWindow = new PopupWindow("MiHub Login Window",true) {InstanceName="mihubLoginWindow",Width=600,Height=300}; /*  */
            var MiHubRegisterPopupWindow = new PopupWindow("Register") {InstanceName="MiHubRegisterPopupWindow",Width=500,Height=620}; /*  */
            var MiHubResetPasswordPopupWindow = new PopupWindow("Reset Password") {InstanceName="MiHubResetPasswordPopupWindow",Width=720,Height=150}; /*  */
            var MiHubResultsWindow = new PopupWindow("Datamars Livestock Session Upload Summary") {InstanceName="MiHubResultsWindow",Width=720,Height=500}; /*  */
            var MiHubWaitingWindow = new PopupWindow("Waiting for Response") {InstanceName="MiHubWaitingWindow",Width=720,Height=380}; /*  */
            var naitLoginWindow = new PopupWindow("NAIT Login Window") {InstanceName="naitLoginWindow",Width=600,Height=250}; /*  */
            var nlisLoginWindow = new PopupWindow("NLIS Form") {InstanceName="nlisLoginWindow",Width=500,Height=500}; /*  */
            var id_3c413e7bc825433c90fcf27cff93bb4f = new RightJustify() {InstanceName="Default"}; /*  */
            var id_412aa537acce4f12ad72b66d9c1a9316 = new RightJustify() {InstanceName="Default"}; /*  */
            var id_4e9a1ae87c044ef7800d9cd43f5867f0 = new RightJustify() {InstanceName="Default"}; /*  */
            var id_5bau91qrkn9t98ut9mhgii4k7j = new RightJustify() {InstanceName="Default"}; /*  */
            var id_99cd0ec5a9234a8fbf598ec941e23df1 = new RightJustify() {InstanceName="Default"}; /*  */
            var id_303511ce02bd49b1ac0908ce8cd615d1 = new Select() {InstanceName="Default",Columns=new string[]{"EID"}}; /*  */
            var MiHubUploadResultsFormatted = new StringFormat("{0} file(s) uploaded with error\n{1} file(s) uploaded successfully") {InstanceName="MiHubUploadResultsFormatted"}; /*  */
            var getFileNameFromPath = new StringModifier() {InstanceName="getFileNameFromPath",Lambda=(s) => Regex.Match(s, @"([^\\]+)(?=(\.[^\\]*$))").Value}; /*  */
            var id_0a87536e796c46749dfd9e2199fad0d0 = new StringModifier() {InstanceName="Default",Lambda=s => $"https://livestock.mihub.tru-test.com/odata/Users('{s}')"}; /*  */
            var id_672eb568cf9741838cdacc4ef5429b42 = new StringModifier() {InstanceName="Default",Lambda=s => $"{s} Farm"}; /*  */
            var id_a8fe59c13b8b46aab423955865e104b5 = new StringModifier() {InstanceName="Default",Lambda=s =>{    return s + " row(s) processed successfully.";}}; /*  */
            var id_defeab784c324bf08ba7c7465456885a = new StringModifier() {InstanceName="Default",Lambda=s => $"https://livestock.mihub.tru-test.com/odata/Farms({s})"}; /*  */
            var id_f32ab268257d4db2a9bcf455e1626864 = new StringModifier() {InstanceName="Default",Lambda=s =>{    return "Error: " + s;}}; /*  */
            var id_ae91d1aec20f4fefa0ed594e75f0cb57 = new Table() {InstanceName="Default"}; /*  */
            var MiHubResultsTable = new Table() {InstanceName="MiHubResultsTable"}; /*  */
            var noEIDTable = new Table("Session Name","Number of rows missing an EID") {InstanceName="noEIDTable"}; /*  */
            var eidTableDataFlowConnector = new TableDataFlowConnector() {InstanceName="eidTableDataFlowConnector"}; /*  */
            var id_02c4efb1335b41b09a6be5d88da16801 = new Text("Login failed. Please try again.") {HorizAlignment=HorizontalAlignment.Center,FontSize=14,InstanceName="Default",Color=Brushes.Red}; /*  */
            var id_08bbd1ff990f4a3dab79c98eabcd6fbb = new Text("Continue?") {FontSize=20,InstanceName="Default",Margin=new Thickness(5)}; /*  */
            var id_0b15ca3952674b5c84b15dc80dce134f = new Text("Your NLIS account:") {FontWeight=FontWeights.Bold,FontSize=14,InstanceName="Default",Margin=new Thickness(10)}; /*  */
            var id_1130a8893290457297e177e787eb8076 = new Text("Account type:") {InstanceName="Default"}; /*  */
            var id_15kmql52nb5r59127gughn33np = new Text("Username: ") {FontSize=15,InstanceName="Default"}; /*  */
            var id_190ecb29ce0e496f9d6f08413ed9e875 = new Text("Description (optional):") {InstanceName="Default"}; /*  */
            var id_1eaf59def17d4fbc9f2dd626b5db0e23 = new Text("Authorisation Level:") {InstanceName="Default"}; /*  */
            var id_1og2ov1qbcn1g4th22c9pm7tv4 = new Text("Transfer results summary: ") {FontSize=14,InstanceName="Default",Margin=new Thickness(5)}; /*  */
            var id_20a58a62d5c64a3da4631dcd0436f743 = new Text("NVD/Waybill (optional):") {InstanceName="Default"}; /*  */
            var id_24e47c79792840ff9243dba8ac427396 = new Text("Thanks for registering! We've created a farm for you. Please check and confirm the details below.") {InstanceName="Default"}; /*  */
            var id_2a2df9ed461d4a8ea41cd774ae2f6701 = new Text("Animal Type:") {FontSize=15,InstanceName="Default"}; /*  */
            var id_2b41308ef31546d880b27ec1ce41f01c = new Text("Farm Country:") {InstanceName="Default"}; /*  */
            var id_2knrb6kk3ifds72cl3d8h85noh = new Text("Waiting for Datamars Livestock to respond...") {HorizAlignment=HorizontalAlignment.Center,FontSize=20,InstanceName="Default",Margin=new Thickness(10)}; /*  */
            var id_2o033hb453l58815b7isadd9kp = new Text("Please enter your Datamars Livestock details below:\n") {FontSize=20,InstanceName="Default"}; /*  */
            var id_314f1211db554b31a998ebbacb9e04f9 = new Text() {FontSize=14,InstanceName="Default",Margin=new Thickness(5, 5, 0, 5)}; /*  */
            var id_31c41ea7451a42ff9bb479b6f5981030 = new Text("Transaction Details:") {FontWeight=FontWeights.Bold,FontSize=14,InstanceName="Default",Margin=new Thickness(10)}; /*  */
            var id_321bf0892f2e404b87e7a76ef35ca2b1 = new Text("Authorisation Details:") {FontWeight=FontWeights.Bold,FontSize=14,InstanceName="Default",Margin=new Thickness(10)}; /*  */
            var id_3385d6c5aa0d4ac6ae4a93c192f930da = new Text("") {Visibility=Visibility.Collapsed,FontSize=12,InstanceName="Default"}; /*  */
            var id_363297dfa7914702b8cfadabb9bb0d2c = new Text("Password: ") {FontSize=15,InstanceName="Default"}; /*  */
            var id_36e9888e3ea84322a8e8ab4c6ddbaceb = new Text("Date:") {InstanceName="Default"}; /*  */
            var id_39bda7lmkss6ee12kvbmdj0tcs = new Text("Password: ") {FontSize=15,InstanceName="Default"}; /*  */
            var id_39dfdbd7d9814b008c99aa0bd087940c = new Text("NVD/Waybill (optional):") {InstanceName="Default"}; /*  */
            var id_3b88d94e65a3469bb68f700ba76f993b = new Text(text:"Email:") {InstanceName="Default"}; /*  */
            var id_3ba80dc932cc4f938269acbca05d5495 = new Text("Receiving Movement Details:") {FontWeight=FontWeights.Bold,FontSize=16,InstanceName="Default",Margin=new Thickness(10)}; /*  */
            var id_3c7e55d2a54148e59d67a5a9afc2ab43 = new Text("NAIT Number:") {FontSize=15,InstanceName="Default"}; /*  */
            var id_3e6ff7a7277e4a2f8b84c3ccb5fa7519 = new Text("Date:") {InstanceName="Default"}; /*  */
            var id_417a85af281b473b8aea1f1e9f50c220 = new Text("Seller's PIC:") {InstanceName="Default"}; /*  */
            var id_42b9b766cdb34a2c8c13d6a17b020cff = new Text("Description (optional):") {InstanceName="Default"}; /*  */
            var id_44e9b2a1f7af4e5e9ee86cea71eac16f = new Text("Destination PIC:") {InstanceName="Default"}; /*  */
            var id_47760a5c942b4ca28a4b2c42d6e22cf6 = new Text("Transaction type:") {InstanceName="Default"}; /*  */
            var id_479aa2f483d5465c987e1f4f84947a38 = new Text("Sending Movement Details:") {FontWeight=FontWeights.Bold,FontSize=16,InstanceName="Default",Margin=new Thickness(10)}; /*  */
            var id_57057f9a45d3495f9d3fff51026faa84 = new Text("Sending NAIT Number:") {FontSize=15,InstanceName="Default"}; /*  */
            var id_5808ff8cf2c243448f186b40a50a830d = new Text("Animal Registration Details:") {FontWeight=FontWeights.Bold,FontSize=16,InstanceName="Default",Margin=new Thickness(10)}; /*  */
            var id_5a9efc74583f4d288f319f3fde7f1514 = new Text("") {Visibility=Visibility.Collapsed,FontSize=12,InstanceName="Default"}; /*  */
            var id_5ae8c8b17cc440eab5a4b7d39a8ad492 = new Text("NVD/Waybill (optional):") {InstanceName="Default"}; /*  */
            var id_5be179b9d1c6499c82fd3374fde6fd3a = new Text("Password:") {InstanceName="Default"}; /*  */
            var id_5e081e4d9d034e77bfa883b99d51fe9e = new Text("Authoriser Last Name:") {InstanceName="Default"}; /*  */
            var id_5e944b71c60246489b53162cf905c84c = new Text("Transaction type:") {InstanceName="Default"}; /*  */
            var id_5e99338c5c124d92a9275697299babac = new Text("am liable for all losses and damages arising out of this warranty") {FontSize=14,InstanceName="Default"}; /*  */
            var id_6783d645206643be81efa7d1b55380b4 = new Text("") {FontWeight=FontWeights.Bold,FontSize=16,InstanceName="Default"}; /*  */
            var id_6b606f533fd44d91a320d3412cdcb543 = new Text("NAIT Transaction Details:") {FontWeight=FontWeights.Bold,FontSize=16,InstanceName="Default",Margin=new Thickness(10)}; /*  */
            var id_6c4f74f3cea34eb78458b1fa29bca35d = new Text("Email:") {InstanceName="Default"}; /*  */
            var id_7a33f71704a34878bba6cde487f52945 = new Text("Production Type:") {FontSize=15,InstanceName="Default"}; /*  */
            var id_7b435b74036c4a64b9c0099e3455caf7 = new Text("Username:") {InstanceName="Default"}; /*  */
            var id_822acb4bc2684a5d9ad2a0966c707a9f = new Text("Date Received:") {FontSize=15,InstanceName="Default"}; /*  */
            var id_829bda95b61242e5b0da39592b161fb6 = new Text("Date:") {InstanceName="Default"}; /*  */
            var id_86e636dce3fc49cf8084b309b4ee42d6 = new Text("Password:") {InstanceName="Default"}; /*  */
            var id_88e085d3641e4988b87f886216282a91 = new Text("Password:") {InstanceName="Default"}; /*  */
            var id_8b9fe493bca443cdb43ffbfab28c368b = new Text("Farm Setup") {FontWeight=FontWeights.Bold,FontSize=14,InstanceName="Default"}; /*  */
            var id_8cd89ef3210b4737aaebcf5f67e33f8b = new Text("Receiver's PIC:") {InstanceName="Default"}; /*  */
            var id_8cfd55992e0a4aceb4d73687066440c1 = new Text("Date of Birth:") {FontSize=15,InstanceName="Default"}; /*  */
            var id_9315eaef4e0f4a37bf2e823dcae53def = new Text("Transaction type:") {InstanceName="Default"}; /*  */
            var id_9361214861474d599977127d9081abea = new Text("NAIT User Details:") {FontWeight=FontWeights.Bold,FontSize=16,InstanceName="Default"}; /*  */
            var id_95755a8171f049c1ba33611cdc02cae9 = new Text("NVD/Waybill (optional):") {InstanceName="Default"}; /*  */
            var id_998f19f207b04942813c43f68ed7511f = new Text("Password:") {FontSize=15,InstanceName="Default"}; /*  */
            var id_9a7a57298d13465f8c6cb78490fafb45 = new Text("Farm Name:") {InstanceName="Default"}; /*  */
            var id_9af41d1bfa784ef5b987030af690f77d = new Text("Success!") {FontWeight=FontWeights.Bold,FontSize=18,InstanceName="Default",Margin=new Thickness(5, 0, 5, 0)}; /*  */
            var id_9fd795adcaba47d8964f24ffe8e0474c = new Text("Date Sent:") {FontSize=15,InstanceName="Default"}; /*  */
            var id_a881f85f8e5447af9307c6ea24534035 = new Text("Date:") {InstanceName="Default"}; /*  */
            var id_ab592c073f574b64b445d2122b568ee8 = new Text("notify MLA of this transfer on their behalf. I acknowledge that I") {FontSize=14,InstanceName="Default"}; /*  */
            var id_c464b32436d3472b94a4f46724353bc8 = new Text("I warrant that I am authorised by the authoriser named above to") {FontSize=14,InstanceName="Default"}; /*  */
            var id_c84a91cf1346471698cffb077b5f7cdc = new Text("Sender's PIC:") {InstanceName="Default"}; /*  */
            var id_cc92467bca7e4115b2c9cb49a94413e5 = new Text("NVD/Waybill (optional):") {InstanceName="Default"}; /*  */
            var id_cd304edc595341d2bb09c89947e252cf = new Text("Description (optional):") {InstanceName="Default"}; /*  */
            var id_cd979cfb045047a09bd9887bc625f104 = new Text("Select your NAIT request type:") {FontSize=15,InstanceName="Default"}; /*  */
            var id_d8342442bbce40d69feb2faecddf3c7b = new Text("Send a password reset link to your email") {HorizAlignment=HorizontalAlignment.Center,FontSize=20,InstanceName="Default",Margin=new Thickness(10)}; /*  */
            var id_dc60bbe392684960b26dc2e59b7e565f = new Text("Internet connection detected. Would you like to sign up or log in to Datamars Livestock?") {InstanceName="Default"}; /*  */
            var id_ddcde050ae314943bf9104b14bdff335 = new Text("Description (optional):") {InstanceName="Default"}; /*  */
            var id_def2364a24374e1c88827983a708ac54 = new Text("Enter details for your NLIS transaction") {FontWeight=FontWeights.Bold,FontSize=16,InstanceName="Default",Margin=new Thickness(10)}; /*  */
            var id_df5b8eb15ae447cbba99838c68238360 = new Text("Receiving NAIT Number:") {FontSize=15,InstanceName="Default"}; /*  */
            var id_e0ba0132e37541b8abdbde614694ea88 = new Text("Farm Region:") {InstanceName="Default"}; /*  */
            var id_e101f98e6141432f8838ef41d48eafcf = new Text("Date:") {InstanceName="Default"}; /*  */
            var id_e2f794c2693048c7b24840310cf07fd8 = new Text("Upload to farm:") {InstanceName="Default",Margin=new Thickness(5)}; /*  */
            var id_e3850f2254344069b17a5539b5875948 = new Text("Description (optional):") {InstanceName="Default"}; /*  */
            var id_e74f8a7d6aaf4092863eb8fc58e33cd4 = new Text("Sending NAIT Number:") {FontSize=15,InstanceName="Default"}; /*  */
            var id_e8d17d5366a54e299ff416b362199e09 = new Text("being incorrect and indemnify MLA") {FontSize=14,InstanceName="Default"}; /*  */
            var id_ec118657a3fe4cb0a10d1159f219d95a = new Text("Source PIC:") {InstanceName="Default"}; /*  */
            var id_eceacd110a97449492a266eeb15bf3be = new Text("") {Visibility=Visibility.Collapsed,FontSize=12,InstanceName="Default"}; /*  */
            var id_ed2af88cf5184701920ccddf94186db2 = new Text("Receiving NAIT Number:") {FontSize=15,InstanceName="Default"}; /*  */
            var id_ee1c0b13a02a4811919f6548a8cf5701 = new Text("Warning: no internet connection detected.\nAny information you enter will not be sent.\nPlease try again later.") {FontSize=15,InstanceName="Default"}; /*  */
            var id_f1475d65b1b64c4f812c4a5c3cf4b957 = new Text("Buyer's PIC:") {InstanceName="Default"}; /*  */
            var id_f1d1b4ade6344ef4a701679e6af22de8 = new Text("Your PIC:") {InstanceName="Default"}; /*  */
            var id_f3a8376c0e8349e8bc7bf47f2d99d8fb = new Text("Warning: no internet connection detected.\nAny information you enter will not be sent.\nPlease try again later.") {FontSize=15,InstanceName="Default"}; /*  */
            var id_f747a8e3e41f493caf23ba92cf64e34f = new Text("Username:") {FontSize=15,InstanceName="Default"}; /*  */
            var id_fb3f38bfc561419ca502c83d0c1e3cfa = new Text("Authoriser First Name:") {InstanceName="Default"}; /*  */
            var id_fe9db59201c341c9946d7da494be3375 = new Text("") {Visibility=Visibility.Collapsed,FontStyle=FontStyles.Italic,FontSize=16,InstanceName="Default",Margin=new Thickness(5)}; /*  */
            var MiHubRegisterAddressLine1Text = new Text("Address:") {InstanceName="MiHubRegisterAddressLine1Text"}; /*  */
            var MiHubRegisterAddressLine2Text = new Text("") {InstanceName="MiHubRegisterAddressLine2Text"}; /*  */
            var MiHubRegisterCityText = new Text("City:") {InstanceName="MiHubRegisterCityText"}; /*  */
            var MiHubRegisterConfirmPasswordText = new Text("Confirm password: ") {InstanceName="MiHubRegisterConfirmPasswordText"}; /*  */
            var MiHubRegisterCountryText = new Text("Country:") {InstanceName="MiHubRegisterCountryText"}; /*  */
            var MiHubRegisterEmailText = new Text("Email:") {InstanceName="MiHubRegisterEmailText"}; /*  */
            var MiHubRegisterFirstNameText = new Text("First name:") {InstanceName="MiHubRegisterFirstNameText"}; /*  */
            var MiHubRegisterLastNameText = new Text("Last name:") {InstanceName="MiHubRegisterLastNameText"}; /*  */
            var MiHubRegisterPhoneNumberText = new Text("Phone number:") {InstanceName="MiHubRegisterPhoneNumberText"}; /*  */
            var MiHubRegisterPostcodeText = new Text("Postcode:") {InstanceName="MiHubRegisterPostcodeText"}; /*  */
            var MiHubRegisterRegionText = new Text("Region:") {InstanceName="MiHubRegisterRegionText"}; /*  */
            var MiHubRegisterSuburbText = new Text("Suburb:") {InstanceName="MiHubRegisterSuburbText"}; /*  */
            var id_01ce06f1ee2e43fa92bee221e2e4ffd6 = new TextBox() {InstanceName="Default"}; /*  */
            var id_036af24010e24da7ae2ebf1aef0f8181 = new TextBox() {InstanceName="Default"}; /*  */
            var id_0876c45a502f4edcbae1a297b7674987 = new TextBox() {InstanceName="Default"}; /*  */
            var id_110c4920ce014c1f80a2c03684cd6423 = new TextBox() {InstanceName="Default"}; /*  */
            var id_1dae1c9cba6542e093d34cf2350916b4 = new TextBox() {InstanceName="Default"}; /*  */
            var id_2603734bc36540b8a73ce5d18cdc749f = new TextBox() {Margin=new Thickness(5),InstanceName="Default"}; /*  */
            var id_2dbc0ce08d794b2e8a7f7ef076ec98db = new TextBox() {InstanceName="Default"}; /*  */
            var id_3420a6a3eb734749aa5ad598ec64a259 = new TextBox() {InstanceName="Default"}; /*  */
            var id_3a6518c31f9f41558a21bbc19c080881 = new TextBox() {InstanceName="Default"}; /*  */
            var id_40832017140c4188b03057a7400549ed = new TextBox() {InstanceName="Default"}; /*  */
            var id_417c72s0ir8kes1hjoolugkmjm = new TextBox() {Margin=new Thickness(5),Height=50,FontSize=16,InstanceName="Default"}; /*  */
            var id_4450fe81dfa2476886acc75395ee4b55 = new TextBox() {InstanceName="Default"}; /*  */
            var id_636eeea6e82f45819b9384063587b8ed = new TextBox() {InstanceName="Default"}; /*  */
            var id_657d14273506444f82de4d12b5851059 = new TextBox() {InstanceName="Default"}; /*  */
            var id_686904aaf1da4c979e70ef750bff9284 = new TextBox() {InstanceName="Default"}; /*  */
            var id_688bec5c64f043c0bf584a00b34001ab = new TextBox() {InstanceName="Default"}; /*  */
            var id_6bb3fb9231b04d07a8ec91506f8a8570 = new TextBox() {InstanceName="Default"}; /*  */
            var id_71140963662240209c3f6c4670bd075e = new TextBox() {InstanceName="Default"}; /*  */
            var id_7779793f380549deb7e33196c6827bf8 = new TextBox() {InstanceName="Default"}; /*  */
            var id_7d9b0dc90fc5428b93770ffa0d75936f = new TextBox() {InstanceName="Default"}; /*  */
            var id_893587f778e749ee99db0aa4872c199f = new TextBox() {InstanceName="Default"}; /*  */
            var id_9fdcb5b5ae214ebcbf1d66898eb33caa = new TextBox() {InstanceName="Default"}; /*  */
            var id_a60303b5a64c4911a8b9fd34f113aafa = new TextBox() {InstanceName="Default"}; /*  */
            var id_b2937648b6eb4ab8821fff7536397cf1 = new TextBox() {Text="New Zealand",InstanceName="Default"}; /*  */
            var id_ba1a634fe34e4a30aebc2e2d14f2caca = new TextBox() {Text="Auckland",InstanceName="Default"}; /*  */
            var id_bd895bfb66b3426c8cdb129dc677d153 = new TextBox() {InstanceName="Default"}; /*  */
            var id_d16421a67a454618b7e9fca13205cecd = new TextBox() {InstanceName="Default"}; /*  */
            var id_d971accd393f49c7a7165714e3739ce6 = new TextBox() {InstanceName="Default"}; /*  */
            var id_ded801bcc5ba44ea8f01c0c1c2e0bb99 = new TextBox() {InstanceName="Default"}; /*  */
            var id_eb8fa1f57a224fadbc9380d351737a41 = new TextBox() {InstanceName="Default"}; /*  */
            var id_ee1dbb0d58f046cdb08b363717032ff0 = new TextBox() {InstanceName="Default"}; /*  */
            var id_f2b795308c71450fbdf3e5f4d23a99bb = new TextBox() {InstanceName="Default"}; /*  */
            var id_faa1c9b7ad214735807f6e17f9e63ee2 = new TextBox() {InstanceName="Default"}; /*  */
            var id_fc9c19b650e842c880d61d14fa13e7d0 = new TextBox() {InstanceName="Default"}; /*  */
            var MiHubPasswordResetEmailTextBox = new TextBox() {InstanceName="MiHubPasswordResetEmailTextBox"}; /*  */
            var MiHubRegisterAddressLine1TextBox = new TextBox() {InstanceName="MiHubRegisterAddressLine1TextBox"}; /*  */
            var MiHubRegisterAddressLine2TextBox = new TextBox() {InstanceName="MiHubRegisterAddressLine2TextBox"}; /*  */
            var MiHubRegisterCityTextBox = new TextBox() {InstanceName="MiHubRegisterCityTextBox"}; /*  */
            var MiHubRegisterCountryTextBox = new TextBox() {InstanceName="MiHubRegisterCountryTextBox"}; /*  */
            var MiHubRegisterEmailTextBox = new TextBox() {InstanceName="MiHubRegisterEmailTextBox"}; /*  */
            var MiHubRegisterFirstNameTextBox = new TextBox() {InstanceName="MiHubRegisterFirstNameTextBox"}; /*  */
            var MiHubRegisterLastNameTextBox = new TextBox() {InstanceName="MiHubRegisterLastNameTextBox"}; /*  */
            var MiHubRegisterPhoneNumberTextBox = new TextBox() {InstanceName="MiHubRegisterPhoneNumberTextBox"}; /*  */
            var MiHubRegisterPostcodeTextBox = new TextBox() {InstanceName="MiHubRegisterPostcodeTextBox"}; /*  */
            var MiHubRegisterRegionTextBox = new TextBox() {InstanceName="MiHubRegisterRegionTextBox"}; /*  */
            var MiHubRegisterSuburbTextBox = new TextBox() {InstanceName="MiHubRegisterSuburbTextBox"}; /*  */
            var mihubUsernameInput = new TextBox() {InstanceName="mihubUsernameInput"}; /*  */
            var id_5cda8733fd4b4c2bbb95956c76f63a69 = new Timer() {InstanceName="Default",Delay=2000,LoopTimes=1}; /*  */
            var id_f8c78d4d0821449fb7caba4b68072bd1 = new Timer() {InstanceName="Default",Delay=2000,LoopTimes=1}; /*  */
            var id_053df641941e4a2187c70657336c68b6 = new ToString<JToken>() {InstanceName="Default"}; /*  */
            var id_597c3bcc3a6140e495f1aaf1b250c315 = new ToString<JToken>() {InstanceName="Default"}; /*  */
            var id_6f0bec1fe272464faa6c5a4fe6070f0d = new ToString<JToken>() {InstanceName="Default"}; /*  */
            var id_7584f5dc48014a659a2cdf812a4e1f05 = new ToString<JToken>() {InstanceName="Default"}; /*  */
            var id_98cb940aa8ba43fa81fc4638904f019f = new ToString<JToken>() {InstanceName="Default"}; /*  */
            var id_a1cffe13141047feab83daac141f4c65 = new ToString<JToken>() {InstanceName="Default"}; /*  */
            var id_a2ea6fd02b404e4bb5104a1ada607380 = new ToString<JToken>() {InstanceName="Default"}; /*  */
            var id_ab01b0f99fca4ba3aa5091a73134869a = new ToString<JToken>() {InstanceName="Default"}; /*  */
            var id_af25bcde89414573b8aa029152d262a6 = new ToString<JToken>() {InstanceName="Default"}; /*  */
            var id_d30a47070f7f426b8e286696e0e3bdc5 = new ToString<JToken>() {InstanceName="Default"}; /*  */
            var id_6b1962873675446f8df04ed75a4b0561 = new ToString<List<JProperty>>() {InstanceName="Default"}; /*  */
            var id_ea6914aa996d437ab102906292d9b093 = new ToString<List<JProperty>>() {InstanceName="Default"}; /*  */
            var eidSummaryTransact = new Transact() {InstanceName="eidSummaryTransact",ClearDestination=true,AutoLoadNextBatch=true}; /*  */
            var getEIDsFromSessionDataSCPTransact = new Transact() {InstanceName="getEIDsFromSessionDataSCPTransact",ClearDestination=true,AutoLoadNextBatch=true}; /*  */
            var getTickedSessionsForEIDTransact = new Transact() {InstanceName="getTickedSessionsForEIDTransact",ClearDestination=true,AutoLoadNextBatch=true}; /*  */
            var MiHubUploadResultsTransact = new Transact() {InstanceName="MiHubUploadResultsTransact",ClearDestination=true,AutoLoadNextBatch=true}; /*  */
            var id_0a2ae499595c42dcb031e3fb5880d75e = new UniqueCollection<string>() {InstanceName="Default"}; /*  */
            var id_37v5ijh1r6kt9to3j318fbg81s = new Vertical() {Layouts=new int[]{2, 2},InstanceName="Default"}; /*  */
            var id_3ba27c164bc641808ce69f70855ac852 = new Vertical() {Layouts=new int[]{2, 2, 2, 2},InstanceName="Default"}; /*  */
            var id_b6cdd489f7d7473993937a57c2084fef = new Vertical() {Layouts=new int[]{0, 0, 0, 0, 0},Margin=new Thickness(5),InstanceName="Default",HorizAlignment=HorizontalAlignment.Center}; /*  */
            var id_c7cd0ba17acd4028b25be62579dff6de = new Vertical() {Layouts=new int[]{0, 0},InstanceName="Default"}; /*  */
            var mihubLoginFormVertical = new Vertical() {Layouts=new int[]{2, 2, 2, 0, 2, 2, 2},InstanceName="mihubLoginFormVertical"}; /*  */
            var naitAnimalRegistrationForm = new Vertical(visible:false) {Layouts=new int[]{2, 2, 2, 2, 2},InstanceName="naitAnimalRegistrationForm"}; /*  */
            var naitReceivingMovementForm = new Vertical(visible:false) {Layouts=new int[]{2, 2, 2, 2, 2},InstanceName="naitReceivingMovementForm"}; /*  */
            var naitSendingMovementForm = new Vertical(visible:false) {Layouts=new int[]{2, 2, 2, 2, 2},InstanceName="naitSendingMovementForm"}; /*  */
            var nlisAgentOrProducerReceivingVertical = new Vertical() {Layouts=new int[]{2, 2, 2, 2, 2},InstanceName="nlisAgentOrProducerReceivingVertical"}; /*  */
            var nlisAgentOrProducerSendingVertical = new Vertical() {Layouts=new int[]{2, 2, 2, 2, 2},InstanceName="nlisAgentOrProducerSendingVertical"}; /*  */
            var nlisAgentThirdPartyTransferVertical = new Vertical() {Layouts=new int[]{2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2},InstanceName="nlisAgentThirdPartyTransferVertical"}; /*  */
            var nlisFromSaleyardVertical = new Vertical() {Layouts=new int[]{2, 2, 2, 2, 2},InstanceName="nlisFromSaleyardVertical"}; /*  */
            var nlisToSaleyardVertical = new Vertical() {Layouts=new int[]{2, 2, 2, 2, 2},InstanceName="nlisToSaleyardVertical"}; /*  */
            // END AUTO-GENERATED INSTANTIATIONS FOR WebServices
            #endregion

            #region UI INSTANTIATIONS
            // BEGIN AUTO-GENERATED INSTANTIATIONS FOR UI
            var id_2f8b78e826a94b678b6dcbe2745515fc = new Button("Close") {Width=100,Height=35,Margin=new Thickness(5, 0, 15, 15),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_513bc764885b46d5a9edc7eb3eaf0149 = new Button("Cancel") {Width=100,Height=35,Margin=new Thickness(5, 0, 15, 15),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_588121ad26d84fa2b586b8eb456fbfc0 = new Button("OK") {Width=100,Height=35,Margin=new Thickness(5, 0, 5, 15),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_6492530e8df04d70a523f4eae5b3e654 = new Button("Recover") {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_666e42c7abd745b19ce110e58fff7fe3 = new Button("Open File") {Margin=new Thickness(5, 0, 0, 0),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_828628611a114f9dac32cf4bcc4b62cd = new Button("Log out") {Margin=new Thickness(5, 0, 0, 0),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_933e18cb574e403a8293e7ad51ae87ff = new Button("Register a new account") {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_aadc4264ce44404cbc8dd793de3bebef = new Button("Refresh COM Ports") {Width=150,InstanceName="Default"}; /* {"IsRoot":false} */
            var id_bb23b069340d49c4ba9c005aa0cac650 = new Button("Check for updates") {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_e7221ba0d6b646dfad26b552248bc114 = new Button("Log in to Datamars Livestock") {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_e82f05fcee5c4e9c840638ff01f73910 = new Button("Log out") {Margin=new Thickness(5, 0, 0, 0),InstanceName="Default"}; /* {"IsRoot":false} */
            var RecoverDeviceButton = new Button("Recover XRS2/SRS2") {Margin=new Thickness(5),InstanceName="RecoverDeviceButton"}; /* {"IsRoot":false} */
            var UpdateFromFileButton = new Button("Update device firmware from a file",false) {Margin=new Thickness(5),InstanceName="UpdateFromFileButton"}; /* {"IsRoot":false} */
            var id_145c3e2d3f634089a36c7b35d96bbb16 = new CheckBox("Automatically check for updates") {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_aa1f726c2c234538b05c21929b7cfc31 = new DataFlowConnector<string>() {InstanceName="Default"}; /* {"IsRoot":false} */
            var RecoverDeviceType = new DataFlowConnector<string>() {InstanceName="RecoverDeviceType"}; /* {"IsRoot":false} */
            var id_6726c941558141fc9da87b08fe62d06c = new EventConnector() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_407dd69cf0a34642971073a089c2bc4b = new GroupingBox("Datamars Livestock") {FontSize=15,InstanceName="Default"}; /* {"IsRoot":false} */
            var id_487d1f7d030d410b8a1dd7202fdf3f28 = new GroupingBox("COM Port") {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_5e05cc197a2340618fe5c4c05e88eec9 = new GroupingBox("NAIT") {FontSize=15,InstanceName="Default"}; /* {"IsRoot":false} */
            var id_65b7643634014f2a99119d5dd062ae3e = new GroupingBox("File Path") {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_87fb3d397be445f1a6acbe67908b2efb = new GroupingBox("NLIS") {FontSize=15,InstanceName="Default"}; /* {"IsRoot":false} */
            var id_92151aec98a842b09e23e6fc8416a9ff = new GroupingBox("Device Type") {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_0c8b24013cc94463af0684e5450edf69 = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_0ef690880728491b8ddc1a9f9183cf9a = new Horizontal() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_243d5b85c8fb455b98721ef4f956855e = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_2b4234a9a01143a096a8c9764941b899 = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_42cbbc7338ff42ef9543d0d5d0d20263 = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_58970d93c39c40d098e4346bc5bef9af = new Horizontal() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_6331fe92301f457daeabc13eab4a22a7 = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_95fee20097d745a9911714cc3b989240 = new Horizontal() {Ratios=new int[]{390, 90},Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_9e78f81ba5a24adfa156927061608791 = new Horizontal() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_a3e8d677b58945af8670be93f7c5143f = new Horizontal() {Visibility=Visibility.Collapsed,InstanceName="Default"}; /* {"IsRoot":false} */
            var id_a88d32c6365f4075a7e1615ca9687eaf = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_aaa9e90cbe1e4c05a1fd7c15f2acaf10 = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_ae8c0ebdcc3744bebe1cb5d59fd63846 = new Horizontal() {Ratios=new int[]{10, 60, 30},Margin=new Thickness(5),Visibility=Visibility.Collapsed,InstanceName="Default"}; /* {"IsRoot":false} */
            var id_afc226e5502d48829a02e212e718b037 = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_cfb1198e4ab24bfcbd1464f2ab7dd6e0 = new Horizontal() {Visibility=Visibility.Collapsed,InstanceName="Default"}; /* {"IsRoot":false} */
            var EIDRecoverFirmwareBrowser = new OpenFirmwareBrowser("XRS2/SRS2 Firmware Browser",OpenFirmwareBrowser.EidReaderFilter) {InstanceName="EIDRecoverFirmwareBrowser"}; /* {"IsRoot":false} */
            var DeviceTypeOption = new OptionBox("Device type") {InstanceName="DeviceTypeOption"}; /* {"IsRoot":false} */
            var id_1b41e4246228498d992960dc7590c05d = new OptionBoxItem("XRS2/SRS2") {InstanceName="Default"}; /* {"IsRoot":false} */
            var EIDRecoverPopup = new PopupWindow("Recover Device Firmware") {InstanceName="EIDRecoverPopup",Width=500,Height=500}; /* {"IsRoot":false} */
            var id_0ab77e3097574fbdb6690e5e3399bba2 = new RadioButton("CSV (comma separated values)") {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_11da591c6fb440fb93edf0604163b9c1 = new RadioButton("CSV Minda format(comma separated values)") {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_1a353d8d6e4a4b26941d3938df0d0eec = new RadioButton("CSV Stock Agent Private Sale") {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_251a32494684419f9824a4035b839e61 = new RadioButton("Rest of the world") {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_2f9a085d266147dca9fc52889e310f52 = new RadioButton("Spanish") {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_3d190d4084cb47efb1b693a1330aab87 = new RadioButton("XML (Extensible Markup Language format)") {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_4262cd8bbf3244878794b42e440fe9ba = new RadioButton("XLS/XLSX (Microsoft Excel format)") {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_52ce31a20e944273bcfdddbd5e0add92 = new RadioButton("English") {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_5a8156ece91b4b7c9ef90338ec95a188 = new RadioButton("New Zealand") {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_78be548112d44cec84ccc50814adf868 = new RadioButton("NAIT (NZ National Animal Identification and Tracing format)") {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_7a84a73c626b4823b8e9d0427e3d8d6a = new RadioButton("Auto connect") {InstanceName="Default",ToolTip="If you don't know which COM port the device is connected to,use this option. DataLink will scan all COM ports and automatically connect when it finds the device."}; /* {"IsRoot":false} */
            var id_884392e88d504425b108598ee5e27e64 = new RadioButton("Uruguay") {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_a0160fe0401040909421dcda5cf7274e = new RadioButton("CSV 3000 format (comma separated values)") {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_a4ef455d46414565bd6c22c85fdeb28b = new RadioButton("German") {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_a6f8d557dbf84037be12cb39e1967f7e = new RadioButton("Australia") {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_a7a0ea0cb06243d5ab62b2dd206aaf32 = new RadioButton("CSV Stock Agent Sale Yards") {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_b56c26a718764c6aa28882a2bc7832ad = new RadioButton("Portuguese") {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_bf2cfd7432e44871a43f29afcc67786e = new RadioButton("French") {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_c8809d1e4a0e457c99ad7af2204e7336 = new RadioButton("CSV EID only (comma separated values)") {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_ca6c8fb992084e388491208c62949464 = new RadioButton("Choose COM port") {InstanceName="Default",ToolTip="If you know which COM port the device is connected to,use this option. DataLink will connect to the COM port you select,rather than searching through all of the ports."}; /* {"IsRoot":false} */
            var id_da2acc1bd3834eb2ab1671c52adf9867 = new RadioButton("CSV no header (comma separated values)") {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_0f9bf9de23c1469c8d22575658aaa098 = new RightJustify() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_241387242ba644a5b8d657ce5aa195bb = new RightJustify() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_941595b6dcca4780bb9e3c613ca56dd8 = new RightJustify() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_d4ce9b3e60ee4662a2996c639a53d424 = new RightJustify() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_2e22bc4f40d64ad3b6ff3861f6c5519f = new Tab("Updates") {FontSize=13,Height=400,InstanceName="Default"}; /* {"IsRoot":false} */
            var id_44cf5379dada4ce7bdbcfdfd4431331e = new Tab("File format") {FontSize=13,Height=400,InstanceName="Default"}; /* {"IsRoot":false} */
            var id_4c070cf2f6994838a4c2b6f7d482767a = new Tab("Country") {FontSize=13,Height=400,InstanceName="Default"}; /* {"IsRoot":false} */
            var id_4db3afe91a264933a0a8a4fd8495ac02 = new Tab("Language & Units") {FontSize=13,Height=400,InstanceName="Default"}; /* {"IsRoot":false} */
            var id_5391439747c4412eacd8d1787134553e = new Tab("Language & Units") {FontSize=13,Height=400,InstanceName="Default"}; /* {"IsRoot":false} */
            var id_98c6ac2fc16f440c87c0b6c78a624b2d = new Tab("Account Info") {FontSize=13,Height=400,InstanceName="Default"}; /* {"IsRoot":false} */
            var id_b6eda4011f5849cb9d2da5009b098c22 = new TabContainer() {Margin=new Thickness(15),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_dd4a9aa1cc8346509a61fc7f7485c5ff = new TabContainer() {Margin=new Thickness(15),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_24967b00300a464cb744ce9ca4e009b5 = new Text("Email:") {InstanceName="Default",Margin=new Thickness(1)}; /* {"IsRoot":false} */
            var id_3b2ab3e1bb104e32b7349b0213d70337 = new Text("Note: You need to restart the application to see the new language.") {FontSize=15,InstanceName="Default"}; /* {"IsRoot":false} */
            var EIDRecoverFirmwareTextBox = new TextBox() {Margin=new Thickness(0, 0, 5, 0),InstanceName="EIDRecoverFirmwareTextBox"}; /* {"IsRoot":false} */
            var id_7ee4e3a11a924e179073eb833fd7b618 = new TextBox(readOnly:true) {Margin=new Thickness(0, 0, 5, 0),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_28d3acf86e534fdc8014d3e49509b651 = new Vertical() {Layouts=new int[]{0, 0, 0, 0},InstanceName="Default"}; /* {"IsRoot":false} */
            var id_553e3ca462004f569bb0559d4e19da8a = new Vertical() {Layouts=new int[]{0, 0},InstanceName="Default"}; /* {"IsRoot":true} */
            var id_b3c578995cc14195a74a2cf73df44c94 = new Vertical() {Layouts=new int[]{0, 0},InstanceName="Default"}; /* {"IsRoot":true} */
            var ComPortList = new ComPortOptionBox() {InstanceName="ComPortList"}; /* {"IsRoot":false} */
            var id_82fc0f5dcc5742ee83dc4b92787ac58c = new EventConnector() {InstanceName="id_82fc0f5dcc5742ee83dc4b92787ac58c"}; /* {"IsRoot":false} */
            var FileFormatPersist = new PersistList(key:"Settings_FileFormat") {InstanceName="FileFormatPersist"}; /* {"IsRoot":false} */
            var COMPortPersist = new PersistList(key:"Settings_COMPort") {InstanceName="COMPortPersist"}; /* {"IsRoot":false} */
            var LanguagePersist = new PersistList(key:"Settings_Language") {InstanceName="LanguagePersist"}; /* {"IsRoot":false} */
            var CountryPersist = new PersistList(key:"Settings_Country") {InstanceName="CountryPersist"}; /* {"IsRoot":false} */
            var SettingsReaderWriter = new JsonReaderWriter(filePath:@"C:\ProgramData\Tru-Test\DataLink_ALA\usersettings.json") {InstanceName="SettingsReaderWriter"}; /* {"IsRoot":false} */
            var AutomaticUpdatesPersist = new Persist(key:"Settings_AutomaticUpdates") {InstanceName="AutomaticUpdatesPersist"}; /* {"IsRoot":false} */
            var id_25f85600badc40f2a6d9eb0f707fba3f = new EventConnector() {}; /* {"IsRoot":false} */
            var ReinstallUSBDriverButton = new Button(title:"Reinstall USB Driver (XRS2/SRS2)") {Margin=new Thickness(5),InstanceName="ReinstallUSBDriverButton"}; /* {"IsRoot":false} */
            // END AUTO-GENERATED INSTANTIATIONS FOR UI
            #endregion

            #region MainDiagram INSTANTIATIONS
            // BEGIN AUTO-GENERATED INSTANTIATIONS FOR MainDiagram
            var id_20228209bd8b404dbba386585f979588 = new Apply<string, string>() {InstanceName="Default",Lambda=s =>{    return string.IsNullOrWhiteSpace(s) ? null : DateTime.Parse(s).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);}}; /* {"IsRoot":false} */
            var id_85d8b4eb3a0241a3b6277746621420f0 = new DataFlowConnector<bool>() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_fa2109c563e24b87ab4c3e64f311145e = new DataFlowConnector<bool>() {InstanceName="Default"}; /* {"IsRoot":false} */
            var uploadSessionRecordsTotal = new DataFlowConnector<string>() {InstanceName="uploadSessionRecordsTotal"}; /* {"IsRoot":false} */
            var dataPanels = new Horizontal() {Ratios=new int[2]{1, 2},InstanceName="dataPanels"}; /* {"IsRoot":false} */
            var statusLine = new Horizontal() {InstanceName="statusLine",Background=new SolidColorBrush(Color.FromRgb(241, 237, 237))}; /* {"IsRoot":false} */
            var toolbar = new Horizontal() {InstanceName="toolbar"}; /* {"IsRoot":false} */
            var fileMenu = new Menu("File") {InstanceName="fileMenu"}; /* {"IsRoot":false} */
            var helpMenu = new Menu("Help") {InstanceName="helpMenu"}; /* {"IsRoot":false} */
            var menubar = new Horizontal() {InstanceName="menubar"}; /* {"IsRoot":false} */
            var exitApp = new MenuItem("Exit") {IconName="exit.png",InstanceName="exitApp"}; /* {"IsRoot":false} */
            var ImportMenuItem = new MenuItem("Get information off device") {IconName="Import.png",InstanceName="ImportMenuItem"}; /* {"IsRoot":false} */
            var notNeedAlertButtonWhenUSB = new Not() {InstanceName="Default"}; /* {"IsRoot":false} */
            var textDeviceConnected = new Text(null,false) {FontWeight=FontWeights.Bold,FontSize=14,InstanceName="textDeviceConnected",Color=Brushes.Green,Margin=new Thickness(0, 0, 0, 1)}; /* {"IsRoot":false} */
            var textSearchingPorts = new Text("Searching on all ports...") {FontWeight=FontWeights.Bold,FontSize=14,InstanceName="textSearchingPorts",Color=Brushes.Red,Margin=new Thickness(3, 0, 0, 1)}; /* {"IsRoot":false} */
            var ImportToCSV = new Tool("import.png") {InstanceName="ImportToCSV",ToolTip="Get information off device and save to a SCV file"}; /* {"IsRoot":false} */
            var id_0ks2hj4mlls0q8icpq3ubeqd5q = new Vertical() {Layouts=new int[]{0, 0, 2, 0},InstanceName="Default"}; /* {"IsRoot":false} */
            var destination = new Wizard("Get information off device") {SecondTitle="What do you want to do with the session file?",InstanceName="destination"}; /* {"IsRoot":false} */
            var saveSelectedSessionToCSV = new WizardItem("Save selected session to CSV file") {ImageName="Icon_Session.png",Checked=true,InstanceName="saveSelectedSessionToCSV"}; /* {"IsRoot":false} */
            var sendToCloud = new WizardItem("Send sessions to Cloud") {ImageName="cloud.png",InstanceName="sendToCloud"}; /* {"IsRoot":false} */
            var filterEmptyDates = new Filter() {InstanceName="filterEmptyDates",FilterDelegate=(DataRow r) =>{    if (!String.IsNullOrEmpty(r["date"].ToString()))        return true;    return false;}}; /* {"IsRoot":false} */
            var sortByDate = new Sort() {InstanceName="sortByDate",Column="date",IsDescending=true}; /* {"IsRoot":false} */
            var sessionListSelectionMap = new Map() {InstanceName="sessionListSelectionMap",Column="checkbox",MapDelegate=(DataRow r, string s) =>{    if (s.Equals("Today") && DateTime.Now.ToString("dd/MM/yyyy").Equals(r["date"]) || s.Equals("All"))        return true;    return false;}}; /* {"IsRoot":false} */
            var comPortAdapter = new COMPortAdapter() {InstanceName="comPortAdapter"}; /* {"IsRoot":true} */
            var scpDeviceSense = new SCPDeviceSense() {InstanceName="scpDeviceSense"}; /* {"IsRoot":false} */
            var scpProtocol = new SCPProtocol() {InstanceName="scpProtocol"}; /* {"IsRoot":false} */
            var scpArbitrator = new Arbitrator() {InstanceName="scpArbitrator"}; /* {"IsRoot":false} */
            var scpSessionFilesWriter = new CSVFileReaderWriter() {FileType=3,InstanceName="scpSessionFilesWriter"}; /* {"IsRoot":false} */
            var saveScpSessionsDataToFileBrowser = new SaveFileBrowser("Get information onto device") {InstanceName="saveScpSessionsDataToFileBrowser"}; /* {"IsRoot":true} */
            var sessionListScp = new SessionListSCP() {InstanceName="sessionListScp"}; /* {"IsRoot":true} */
            var sessionListGrid = new Grid() {InstanceName="sessionListGrid"}; /* {"IsRoot":false} */
            var sessionDataGrid = new Grid() {InstanceName="sessionDataGrid"}; /* {"IsRoot":false} */
            var sessionDataForGrid = new SessionDataSCP() {InstanceName="sessionDataForGrid"}; /* {"IsRoot":false} */
            var pollSerial = new Timer() {InstanceName="pollSerial"}; /* {"IsRoot":false} */
            var sessionDataTransact = new Transact() {InstanceName="sessionDataTransact"}; /* {"IsRoot":true} */
            var id_b450cfaa54744d0a978d3aa700de0f9a = new ToEvent<bool>() {}; /* {"IsRoot":false} */
            var id_91dac706bbe643529fca59fdc379a223 = new Not() {}; /* {"IsRoot":false} */
            var id_f9a3c2f1b4eb46d386e9b76a0d264683 = new ToEvent<bool>() {}; /* {"IsRoot":false} */
            var sessionDataForImport = new SessionDataSCP() {InstanceName="sessionDataForImport"}; /* {"IsRoot":false} */
            var id_ced2fdc009d34ef79c279aa1828c7744 = new ToEvent<bool>() {}; /* {"IsRoot":false} */
            // END AUTO-GENERATED INSTANTIATIONS FOR MainDiagram
            #endregion

            #region Ethernet_DeviceDetection INSTANTIATIONS
            // BEGIN AUTO-GENERATED INSTANTIATIONS FOR Ethernet_DeviceDetection
            var timerForEthernetDetection = new Timer() { InstanceName = "timerForEthernetDetection", Delay = 3000 }; /* {"IsRoot":true} */
            var id_9ce85ddd4a3a493d8844407bc3b6198c = new EventConnector() { InstanceName = "Default" }; /* {"IsRoot":false} */
            var ethernetDeviceDetectionGate = new EventGate() { LatchInput = true, InstanceName = "ethernetDeviceDetectionGate" }; /* {"IsRoot":false} */
            var deviceDrive = new DeviceDrive() { InstanceName = "deviceDrive" }; /* {"IsRoot":false} */
            var ethernetSwitchToUSB = new EthernetSwitchToUSB() { InstanceName = "ethernetSwitchToUSB" }; /* {"IsRoot":false} */
            var usbDeviceNameGate = new DataFlowGate<string>() { InstanceName = "usbDeviceNameGate", LatchInput = true }; /* {"IsRoot":false} */
            var usbDeviceDbPath = new DataFlowConnector<string>() { InstanceName = "usbDeviceDbPath" }; /* {"IsRoot":false} */
            var usbDeviceNameConnector = new DataFlowConnector<string>() { InstanceName = "usbDeviceNameConnector" }; /* {"IsRoot":false} */
            var deviceDrivePath = new DataFlowConnector<string>() { InstanceName = "deviceDrivePath" }; /* {"IsRoot":false} */
            var id_c6f16c53ee3f45c0959fb9a9eb77f428 = new Apply<string, string>() { InstanceName = "Default", Lambda = path => Path.Combine(path, "favourites") }; /* {"IsRoot":false} */
            var id_972cac857c904b1f9c1a1aff369a6fdd = new Apply<string, string>() { InstanceName = "Default", Lambda = s => { return Path.Combine(s, "polaris.xml"); } }; /* {"IsRoot":false} */
            var polarisFilePath = new DataFlowConnector<string>() { InstanceName = "polarisFilePath" }; /* {"IsRoot":false} */
            var favSettingFolderPath = new DataFlowConnector<string>() { InstanceName = "favSettingFolderPath" }; /* {"IsRoot":false} */
            var id_619d9e36e96a40e7bff6f8b7c33bed03 = new Equals<string>("ID5000") { InstanceName = "Default" }; /* {"IsRoot":false} */
            var id_bb2b4c21eff14e5cb2515d0d3d766c5d = new Equals<string>("XR5000") { InstanceName = "Default" }; /* {"IsRoot":false} */
            var XR5000ConnectedConnector = new DataFlowConnector<bool>() { InstanceName = "XR5000ConnectedConnector" }; /* {"IsRoot":false} */
            var ID5000ConnectedConnector = new DataFlowConnector<bool>() { InstanceName = "ID5000ConnectedConnector" }; /* {"IsRoot":false} */
            var usbConnector = new DataFlowConnector<bool>() { InstanceName = "usbConnector" }; /* {"IsRoot":false} */
            var usbDeviceConnectedOr = new Or() { InstanceName = "usbDeviceConnectedOr" }; /* {"IsRoot":false} */
            var usbConnectedEventConnector = new EventConnector() { InstanceName = "usbConnectedEventConnector" }; /* {"IsRoot":false} */
            var usbDisconnectedEventConnector = new EventConnector() { InstanceName = "usbDisconnectedEventConnector" }; /* {"IsRoot":false} */
            var id_5c6b2afd88984eefbf3cb5d5c0545d05 = new Not() { InstanceName = "Default" }; /* {"IsRoot":false} */
            var reverseUSBConnectedValue = new DataFlowConnector<bool>() { InstanceName = "Default" }; /* {"IsRoot":false} */
            var usbEventConnectorGate = new EventGate() { InstanceName = "usbEventConnectorGate" }; /* {"IsRoot":false} */
            var id_6b148185d4af41f78522ee6c40b370f2 = new ToEvent<bool>() { InstanceName = "Default" }; /* {"IsRoot":false} */
            var id_9807433033264d54b0616ca69a49ab6f = new EventConnector() { InstanceName = "Default" }; /* {"IsRoot":false} */
            var id_a3b8de1dd4f8451cac3e96b59f3acfe7 = new EventGate() { InstanceName = "Default" }; /* {"IsRoot":false} */
            var id_3d906cf5fd6747b1974a4d0cd697efad = new Equals<bool>(false) { InstanceName = "Default" }; /* {"IsRoot":false} */
            var notNeedCrossRefButtonWhenUSB = new Not() { InstanceName = "Default" }; /* {"IsRoot":false} */
            var usbEqualsTrueToAutoUpdate = new Equals<bool>(compareData: true) { InstanceName = "id_94662e0f099c4048abdfe91cb5f08269" }; /* {"IsRoot":false} */
            // END AUTO-GENERATED INSTANTIATIONS FOR Ethernet_DeviceDetection
            #endregion



            #region XR5000_SaveLifeDataToPC INSTANTIATIONS
            // BEGIN AUTO-GENERATED INSTANTIATIONS FOR XR5000_SaveLifeDataToPC
            var id_59f58417545646729ede73333dd6413e = new Button("Open containing folder") {Width=160,Height=35,InstanceName="Default"}; /*  */
            var id_ada2df96bba94b7a9c6fe6b04c082b84 = new Button("Stop") {Width=100,Height=35,InstanceName="Default"}; /*  */
            var id_d73da13c3df14fe6a4b81835cb38cd16 = new Button("OK") {Width=100,Height=35,Margin=new Thickness(5, 0, 5, 0),InstanceName="Default"}; /*  */
            var id_29efd96d028e45c4bfab619922d9db66 = new ToEvent<string>() {InstanceName="Default"}; /*  */
            var exportLifeDataCSVFileReaderWriter = new CSVFileReaderWriter() {InstanceName="exportLifeDataCSVFileReaderWriter"}; /*  */
            var connectorFileFullPath = new DataFlowConnector<string>() {InstanceName="connectorFileFullPath"}; /*  */
            var connectorLifeDataToFileProgress = new DataFlowConnector<string>() {InstanceName="connectorLifeDataToFileProgress"}; /*  */
            var connectorLifeDataToFileTotalCount = new DataFlowConnector<string>() {InstanceName="connectorLifeDataToFileTotalCount"}; /*  */
            var id_d0b224e6215d4095836828f399b07270 = new DataFlowConnector<string>() {InstanceName="Default"}; /*  */
            var id_dc9f0162d8974ff6ba36cd039cedfd56 = new DataFlowConnector<string>() {InstanceName="Default"}; /*  */
            var id_7df3d4e5d6d8454db8c295f09aa169cb = new EventConnector() {InstanceName="Default"}; /*  */
            var id_b881eeedf4274009810d98c79d4fd13b = new Horizontal() {Ratios=new int[]{4, 1},InstanceName="Default"}; /*  */
            var id_d080e7162acb481298961fea3b1961bf = new Horizontal() {InstanceName="Default"}; /*  */
            var id_dbff056362cc465fb29dde052e6908c0 = new Horizontal() {Ratios=new int[]{1, 10},Margin=new Thickness(10),InstanceName="Default"}; /*  */
            var id_760cea92c8644796aa3dc1540b7e5fae = new OpenWindowsExplorer() {InstanceName="Default"}; /*  */
            var id_29c3028618f64252a920bc95ba4ae708 = new Picture("button_ok.png") {Width=35,InstanceName="Default"}; /*  */
            var csvFileSaveSuccessWindow = new PopupWindow("Datamars Data Link",showCloseMiniMaxButton:false) {InstanceName="csvFileSaveSuccessWindow",Width=640,Height=300}; /*  */
            var id_2d6c1d577a8343378c102a42e3090434 = new PopupWindow("Datamars Data link",false) {InstanceName="Default",Width=600,Height=160}; /*  */
            var id_1865fd6f2066404fbddc5f6293b008ad = new ProgressBar() {InstanceName="Default"}; /*  */
            var id_2e9c0ae6f69a40c38ecbe3beeb36f07a = new RightJustify() {InstanceName="Default"}; /*  */
            var saveUsbLifeDataToFileBrowser = new SaveFileBrowser("Get information off device") {InstanceName="saveUsbLifeDataToFileBrowser"}; /* {"IsRoot":true} */
            var id_832224a290c8495ea7cf37ec2e07fe14 = new StringFormat("{0} LifeData") {InstanceName="Default"}; /*  */
            var id_a95b6ba5cf004396afbd1a6cd05cb9c9 = new StringFormat("{0} saved to \"{1}\"") {InstanceName="Default"}; /*  */
            var id_b5b783c369bc429b89780f56090b7a69 = new StringFormat("Loading life data from device. {0} of {1}") {InstanceName="Default"}; /*  */
            var id_318c926a79954f32b8cf3dc131b18c2f = new Text("Success") {FontWeight=FontWeights.Bold,FontSize=20,InstanceName="Default"}; /*  */
            var id_589bcf0180be4675a50baeb1bb52ceb6 = new Text() {FontSize=14,InstanceName="Default",Margin=new Thickness(65, 15, 65, 130)}; /*  */
            var id_7db53da6ab424f9a83159d7981d5be0d = new Text() {FontSize=14,InstanceName="Default",Margin=new Thickness(5, 10, 0, 5),Padding=new Thickness(5),ShowBorder=true}; /*  */
            var id_ecf1f1a325f54a6abeb527f479428538 = new Text("Do not pull out cable while transferring data!") {FontWeight=FontWeights.Bold,FontSize=12,InstanceName="Default",Color=Brushes.Red,Margin=new Thickness(5, 10, 5, 5)}; /*  */
            var lifeDataToCSVFileTransact = new Transact() {InstanceName="lifeDataToCSVFileTransact",ClearDestination=true}; /*  */
            // END AUTO-GENERATED INSTANTIATIONS FOR XR5000_SaveLifeDataToPC
            #endregion

            #region XR5000_UploadSessionsToDevice INSTANTIATIONS
            // BEGIN AUTO-GENERATED INSTANTIATIONS FOR XR5000_UploadSessionsToDevice
            var uploadSessionFromPCToXr5000 = new OpenFileBrowser("Put information onto device") { InstanceName = "uploadSessionFromPCToXr5000" }; /* {"IsRoot":true} */
            var uploadSessionFilesTotal = new DataFlowConnector<string>() { InstanceName = "uploadSessionFilesTotal" }; /* {"IsRoot":false} */
            var id_70786492fd1c4df89f024ac7bb5f1fc8 = new StringFormat("Transferred file {0} of {1}") { InstanceName = "Default" }; /* {"IsRoot":false} */
            var id_3b2899654a4945839a950f0e7e9b6e1c = new Text() { FontSize = 14, InstanceName = "Default", Margin = new Thickness(5, 10, 0, 5), Padding = new Thickness(5), ShowBorder = true }; /* {"IsRoot":false} */
            var id_ddafb5d7f2164179901d320dda93364a = new ProgressBar() { InstanceName = "Default" }; /* {"IsRoot":false} */
            var uploadSessionFilesProgress = new DataFlowConnector<string>() { InstanceName = "uploadSessionFilesProgress" }; /* {"IsRoot":false} */
            var id_ca5ff8c23a5540d5801857f94587497c = new EventConnector() { InstanceName = "id_ca5ff8c23a5540d5801857f94587497c" }; /*  */
            var transactFilesInfoToBeIterated = new Transact() { InstanceName = "transactFilesInfoToBeIterated", ClearDestination = true }; /* {"IsRoot":false} */
            var uploadXR5000SessionsToDevicePopupWindow = new PopupWindow("Datamars Data Link", showCloseMiniMaxButton: false) { InstanceName = "uploadXR5000SessionsToDevicePopupWindow", Width = 640, Height = 250 }; /* {"IsRoot":false} */
            var id_fa40cfa552a04e52ac8b95832cd1a294 = new ProgressBar() { InstanceName = "Default" }; /* {"IsRoot":false} */
            var id_2e2be7441f4340b8a9a953823103325b = new Text() { FontSize = 14, InstanceName = "Default", Margin = new Thickness(5, 10, 0, 5), Padding = new Thickness(5), ShowBorder = true }; /* {"IsRoot":false} */
            var id_19a7df63ae6240478a415f736094c0f4 = new Horizontal() { Ratios = new int[] { 4, 1 }, InstanceName = "Default" }; /* {"IsRoot":false} */
            var id_5f624f1f40884a67bbccc223fe7280e8 = new Text("Do not pull out cable while transferring data!") { FontWeight = FontWeights.Bold, FontSize = 12, InstanceName = "Default", Color = Brushes.Red, Margin = new Thickness(5, 10, 5, 5) }; /* {"IsRoot":false} */
            var id_eebdc2c8e9124163a245a605f60b414f = new Button("Stop") { Width = 100, Height = 35, InstanceName = "Default" }; /* {"IsRoot":false} */
            var uploadSessionFileIterator = new Iterator() { InstanceName = "uploadSessionFileIterator" }; /* {"IsRoot":false} */
            var extractTableForPathName = new ConvertTableToDataFlow() { InstanceName = "extractTableForPathName", Column = "pathname", Row = 0 }; /* {"IsRoot":false} */
            var id_a41c590961f844b9aea31f08bcf9e474 = new DataFlowConnector<string>() { InstanceName = "Default" }; /* {"IsRoot":false} */
            var id_73ec4ef08f8d4014ab401b32434f245e = new FileReader() { InstanceName = "Default" }; /* {"IsRoot":false} */
            var getFileNumberFromFileContent = new ConvertTableToDataFlow() { InstanceName = "getFileNumberFromFileContent", KeyWord = "FileNo:" }; /* {"IsRoot":false} */
            var getFileNameFromFileContent = new ConvertTableToDataFlow() { InstanceName = "getFileNameFromFileContent", KeyWord = "Name:" }; /* {"IsRoot":false} */
            var getFileDateFromFileContent = new ConvertTableToDataFlow() { InstanceName = "getFileDateFromFileContent", KeyWord = "Date:" }; /* {"IsRoot":false} */
            var sessionContentTable = new Table() { InstanceName = "sessionContentTable" }; /* {"IsRoot":false} */
            var fileName = new DataFlowConnector<string>() { InstanceName = "fileName" }; /* {"IsRoot":false} */
            var fileDate = new DataFlowConnector<string>() { InstanceName = "fileDate" }; /* {"IsRoot":false} */
            var id_1e5cc31a6a604f03bb8ede181e834788 = new DataFlowConnector<string>() { InstanceName = "" }; /* {"IsRoot":false} */
            var extractFileNumber = new Apply<string, string>() {InstanceName="extractFileNumber",Lambda=s =>{    return s.Substring(s.IndexOf(":") + 1).Trim();}}; /* {"IsRoot":false} */
            var extractFileName = new Apply<string, string>() {InstanceName="extractFileName",Lambda=s =>{    return string.IsNullOrWhiteSpace(s) ? null : s.Substring(s.IndexOf(":") + 1).Trim();}}; /* {"IsRoot":false} */
            var extractFileDate = new Apply<string, string>() { InstanceName = "extractFileDate", Lambda = s => { return s.Substring(s.IndexOf(":") + 1).Trim(); } }; /* {"IsRoot":false} */
            var containsFileName = new Apply<string, bool>() { InstanceName = "containsFileName", Lambda = s => { return s.ToUpper().Contains("NAME:"); } }; /* {"IsRoot":false} */
            var containsFileNumber = new Apply<string, bool>() { InstanceName = "containsFileNumber", Lambda = s => { return s.ToUpper().Contains("FILENO:"); } }; /* {"IsRoot":false} */
            var containsFileDate = new Apply<string, bool>() { InstanceName = "containsFileDate", Lambda = s => { return s.ToUpper().Contains("DATE:"); } }; /* {"IsRoot":false} */
            var fileNumber = new DataFlowConnector<string>() { InstanceName = "fileNumber" }; /* {"IsRoot":false} */
            var id_4552540ce40e4051ab5682b1eb25286e = new DataFlowConnector<bool>() { InstanceName = "Default" }; /* {"IsRoot":false} */
            var is3000Format = new Or() { InstanceName = "is3000Format" }; /* {"IsRoot":false} */
            var FiveRowsNeededToBeRemovedIf3000 = new Apply<bool, string>() { InstanceName = "FiveRowsNeededToBeRemovedIf3000", Lambda = b => { return b ? "5" : "0"; } }; /* {"IsRoot":false} */
            var id_cc7c4c0b6b5a45678130d86f85ac0bd7 = new DataFlowConnector<string>() { InstanceName = "Default" }; /* {"IsRoot":false} */
            var subtract3000HeadersRowsFromCount = new Subtract() { InstanceName = "subtract3000HeadersRowsFromCount" }; /* {"IsRoot":false} */
            var filterSessionFileForOnlyContentAndNo3000FormatHeader = new Filter() { InstanceName = "filterSessionFileForOnlyContentAndNo3000FormatHeader", FilterLambdaParamDelegate = (DataRow r, string param) => { return r.Table.Rows.IndexOf(r) >= Int32.Parse(param); } }; /* {"IsRoot":false} */
            var id_18ebb5f217624de1a5343d94c8b6368c = new DataFlowConnector<string>() { InstanceName = "Default" }; /* {"IsRoot":false} */
            var id_f0274770209a4c40ab5ab86202d1872c = new DataFlowConnector<bool>() { InstanceName = "Default" }; /* {"IsRoot":false} */
            var id_3d27570497bb42dbb1ae2233cb93ec5a = new DataFlowConnector<string>() { InstanceName = "Default" }; /* {"IsRoot":false} */
            var sessionFileTotalRowCount = new Apply<DataTable, string>() { InstanceName = "sessionFileTotalRowCount", Lambda = dt => { return dt.Rows.Count.ToString(); } }; /* {"IsRoot":false} */
            var id_c78ba23a885f4379914995fdcf87c904 = new DataFlowConnector<string>() { InstanceName = "Default" }; /* {"IsRoot":false} */
            var id_7190cb3c6a4f420db0a1076e6ef54b2c = new Apply<string, string>() { InstanceName = "Default", Lambda = s => { return (DateTime.Parse(s) - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds.ToString(CultureInfo.InvariantCulture); } }; /* {"IsRoot":false} */
            var dateStamp = new DataFlowConnector<string>() { InstanceName = "dateStamp" }; /* {"IsRoot":false} */
            var id_223e0b52ddfe43c69339f9de2caf8959 = new ToEvent<string>() { InstanceName = "Default" }; /* {"IsRoot":false} */
            var id_739f7aaea4714f33bba71fd90ad9e75d = new EventConnector() { InstanceName = "Default" }; /* {"IsRoot":false} */
            var upload5000SessionFileToDeviceTransact = new Transact() { InstanceName = "upload5000SessionFileToDeviceTransact", ClearDestination = true, AutoLoadNextBatch = true }; /* {"IsRoot":false} */
            var id_ab7204b70dd14d1bb9cd3e6d9e831954 = new EventConnector() { }; /*  */
            // END AUTO-GENERATED INSTANTIATIONS FOR XR5000_UploadSessionsToDevice
            #endregion

            #region FirmwareUpdate INSTANTIATIONS
            // BEGIN AUTO-GENERATED INSTANTIATIONS FOR FirmwareUpdate
            var EIDRecoverCheckDeviceType = new AndEvent() {InstanceName="EIDRecoverCheckDeviceType"}; /* {"IsRoot":false} */
            var NoUpdatesAvailableGate = new AndEvent() {InstanceName="NoUpdatesAvailableGate"}; /* {"IsRoot":false} */
            var DatalinkUpdateFileSizeString = new Apply<double, string>() {InstanceName="DatalinkUpdateFileSizeString",Lambda=s => String.Format("File size: {0} MB", s)}; /* {"IsRoot":false} */
            var EIDRecoveryFlashPercentage = new Apply<double, string>() {InstanceName="EIDRecoveryFlashPercentage",Lambda=s => s.ToString()}; /* {"IsRoot":false} */
            var Scale5000FileSizeStringFormat = new Apply<double, string>() {InstanceName="Scale5000FileSizeStringFormat",Lambda=s => String.Format("File size: {0} MB", s)}; /* {"IsRoot":false} */
            var EIDFileFirmwareFlashPercentage = new Apply<double, string>() {InstanceName="EIDFileFirmwareFlashPercentage",Lambda=s => s.ToString()}; /* {"IsRoot":false} */
            var EIDFileSizeStringFormat = new Apply<double, string>() {InstanceName="EIDFileSizeStringFormat",Lambda=s => String.Format("File size: {0} MB", s)}; /* {"IsRoot":false} */
            var EIDFirmwareFlashPercentage = new Apply<double, string>() {InstanceName="EIDFirmwareFlashPercentage",Lambda=s => s.ToString()}; /* {"IsRoot":false} */
            var DatalinkUpdateProgressBar = new Apply<int, string>() {InstanceName="DatalinkUpdateProgressBar",Lambda=s => s.ToString()}; /* {"IsRoot":false} */
            var DatalinkUpdateProgressPercentage = new Apply<int, string>() {InstanceName="DatalinkUpdateProgressPercentage",Lambda=s => String.Format("Progress: {0}%", s)}; /* {"IsRoot":false} */
            var Scale5000UpdateProgressBar = new Apply<int, string>() {InstanceName="Scale5000UpdateProgressBar",Lambda=s => s.ToString()}; /* {"IsRoot":false} */
            var Scale5000UpdateProgressPercentage = new Apply<int, string>() {InstanceName="Scale5000UpdateProgressPercentage",Lambda=s => String.Format("Progress: {0}%", s)}; /* {"IsRoot":false} */
            var EIDUpdateProgressBar = new Apply<int, string>() {InstanceName="EIDUpdateProgressBar",Lambda=s => s.ToString()}; /* {"IsRoot":false} */
            var EIDUpdateProgressPercentage = new Apply<int, string>() {InstanceName="EIDUpdateProgressPercentage",Lambda=s => String.Format("Progress: {0}%", s)}; /* {"IsRoot":false} */
            var DatalinkFirmwarePathFormatter = new Apply<SoftwareVersionsSoftwaresSoftware, string>() {InstanceName="DatalinkFirmwarePathFormatter",Lambda=s => Application.DEFAULT_DIRECTORY + "Updates\\" + s.FileName}; /* {"IsRoot":false} */
            var DatalinkGetFirmwareHash = new Apply<SoftwareVersionsSoftwaresSoftware, string>() {InstanceName="DatalinkGetFirmwareHash",Lambda=s => s.MD5}; /* {"IsRoot":false} */
            var DatalinkRetrieveURL = new Apply<SoftwareVersionsSoftwaresSoftware, string>() {InstanceName="DatalinkRetrieveURL",Lambda=s => s.URL}; /* {"IsRoot":false} */
            var DatalinkVersionCompareString = new Apply<SoftwareVersionsSoftwaresSoftware, string>() {InstanceName="DatalinkVersionCompareString",Lambda=s => String.Format("Current Version: {0}, Latest Version: {1}", Libraries.Constants.DataLinkPCVersionNumber, s.Version)}; /* {"IsRoot":false} */
            var Scale5000FirmwarePathFormatter = new Apply<SoftwareVersionsSoftwaresSoftware, string>() {InstanceName="Scale5000FirmwarePathFormatter",Lambda=s => Application.DEFAULT_DIRECTORY + "Updates\\" + s.FileName}; /* {"IsRoot":false} */
            var Scale5000GetFirmwareHash = new Apply<SoftwareVersionsSoftwaresSoftware, string>() {InstanceName="Scale5000GetFirmwareHash",Lambda=s => s.MD5}; /* {"IsRoot":false} */
            var Scale5000RetrieveURL = new Apply<SoftwareVersionsSoftwaresSoftware, string>() {InstanceName="Scale5000RetrieveURL",Lambda=s => s.URL}; /* {"IsRoot":false} */
            var EIDFirmwarePathFormatter = new Apply<SoftwareVersionsSoftwaresSoftware, string>() {InstanceName="EIDFirmwarePathFormatter",Lambda=s => Application.DEFAULT_DIRECTORY + "Updates\\" + s.FileName}; /* {"IsRoot":false} */
            var EIDGetFirmwareHash = new Apply<SoftwareVersionsSoftwaresSoftware, string>() {InstanceName="EIDGetFirmwareHash",Lambda=s => s.MD5}; /* {"IsRoot":false} */
            var EIDRetrieveURL = new Apply<SoftwareVersionsSoftwaresSoftware, string>() {InstanceName="EIDRetrieveURL",Lambda=s => s.URL}; /* {"IsRoot":false} */
            var id_37710d843c3942d79be066e62d568572 = new Apply<string, string>() {InstanceName="Default",Lambda=s => $"{s} Invalid Firmware"}; /* {"IsRoot":false} */
            var id_4461d0d6ca6b4a3faec5729558fbfe5a = new Apply<string, string>() {InstanceName="Default",Lambda=s => $"{s} Firmware Update Available"}; /* {"IsRoot":false} */
            var id_4bc11a7d1f2a47d0a80052e5fb159913 = new Apply<string, string>() {InstanceName="Default",Lambda=s => $"There was an error installing the latest firmware on your {s}. Please try again or contact Tru-Test customer support."}; /* {"IsRoot":false} */
            var id_5b58bbd15c904233ae78d2182ae5a737 = new Apply<string, string>() {InstanceName="Default",Lambda=s => $"Your {s} has been loaded with the latest firmware. Please unplug the USB cable from your device to complete the update."}; /* {"IsRoot":false} */
            var id_7dcf611eba444dd18cc33581b981a984 = new Apply<string, string>() {InstanceName="Default",Lambda=s => $"{s} Firmware Update Failed"}; /* {"IsRoot":false} */
            var id_8d274070abc149e1926c65a755948ae0 = new Apply<string, string>() {InstanceName="Default",Lambda=s => $"{s} Update Download"}; /* {"IsRoot":false} */
            var id_948178edc71647d48450b57fe572500f = new Apply<string, string>() {InstanceName="Default",Lambda=s => $"There is an update available for your {s}. Would you like to update now?"}; /* {"IsRoot":false} */
            var id_99e480e2f8b144178e320c4e11eb56f5 = new Apply<string, string>() {InstanceName="Default",Lambda=s => $"{s} Firmware Updated"}; /* {"IsRoot":false} */
            var Scale5000NoUpdatesCurrentVersion = new Apply<string, string>() {InstanceName="Scale5000NoUpdatesCurrentVersion",Lambda=s => $"Current Version: {s}"}; /* {"IsRoot":true} */
            var EIDNoUpdatesCurrentVersion = new Apply<string, string>() {InstanceName="EIDNoUpdatesCurrentVersion",Lambda=s => $"Current Version: {s}"}; /* {"IsRoot":false} */
            var id_02e3d49f3852467292d89eb98dc84f62 = new Apply<Tuple<string, SoftwareVersionsSoftwaresSoftware>, string>() {InstanceName="Default",Lambda=s => String.Format("Current Version: {0}, Latest Version: {1}", s.Item1, s.Item2.Version)}; /* {"IsRoot":false} */
            var id_41bfaf91ff954e2385db473b1a534ec2 = new Apply<Tuple<string, SoftwareVersionsSoftwaresSoftware>, string>() {InstanceName="Default",Lambda=s => String.Format("Current Version: {0}, Latest Version: {1}", s.Item1, s.Item2.Version)}; /* {"IsRoot":false} */
            var Scale5000FirmwareDestFormatter = new Apply<Tuple<string, SoftwareVersionsSoftwaresSoftware>, string>() {InstanceName="Scale5000FirmwareDestFormatter",Lambda=s => String.Format(@$"{s.Item1}\{s.Item2.FileName}")}; /* {"IsRoot":false} */
            var Scale5000FileFirmwareDestFormatter = new Apply<Tuple<string, string>, string>() {InstanceName="Scale5000FileFirmwareDestFormatter",Lambda=s => String.Format(@$"{s.Item1}\{s.Item2.Split('\\').Last()}")}; /* {"IsRoot":false} */
            var UpdateArbitrator = new Arbitrator(timeout:-1) {InstanceName="UpdateArbitrator"}; /* {"IsRoot":false} */
            var id_1cb06d8ce9c0436f8003dd6fc7455f70 = new Button("Close") {Margin=new Thickness(2),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_2a27debfb4144c7580ac91f977a12f07 = new Button("Not now") {Margin=new Thickness(2),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_2a88078b3e2b489ca4ba511f2b5041f4 = new Button("Close") {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_2ab00c184c5f4bc89076586ffa3b970f = new Button("Not now") {Margin=new Thickness(2),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_42a24cb00d7d479fbec70bfb0d9dc139 = new Button("Close") {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_44526f0dbf6144d4a87bbcab1c653e65 = new Button("Close") {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_518143103a83410e8573a87feb546b74 = new Button("Close") {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_601f0de91ed94f5abca456b96671fd4c = new Button("Close") {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_609ad71d48df4c9989f7f53869f4fb1f = new Button("Close") {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_61f23642c2db4b3eae7e16cd8a478553 = new Button("Download and Update") {Margin=new Thickness(2),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_63a0b6b073814ee986854fa5590a67bc = new Button("Close") {Margin=new Thickness(2),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_649c1274f7084e778a7df33251f88431 = new Button("Close") {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_661b7884addc43d0a0e96e8f00a2f65b = new Button("Close") {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_6d2bfdd347564c3880086706e17cc37e = new Button("Close") {Margin=new Thickness(2),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_707cbdad6563439c884361ee3269d51d = new Button("Download and Update") {Margin=new Thickness(2),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_71346fe5ba6f411dab1343b15ec71cd8 = new Button("Close") {Margin=new Thickness(2),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_9022a3ce353b4e59b7dd9c338ccd2c3c = new Button("Close") {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_a68043651f7b40e2bbb748c61d86fe5e = new Button("Close") {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_abfb9b82eac14935aaf3c363f437cd33 = new Button("Close") {Margin=new Thickness(2),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_b48db50b45b440c2bc98481eb2f27f02 = new Button("Download and Update") {Margin=new Thickness(2),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_b8b0b50cfc3d4def8c31cff0a7c16ca7 = new Button("Close") {Margin=new Thickness(2),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_c1ccaf01062d4a478275495a4b878924 = new Button("Not now") {Margin=new Thickness(2),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_cb0aa71ff7a4464bbacb87cba523aba8 = new Button("Close") {Margin=new Thickness(2),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_f612db2371e044769a2450f43ca8cf52 = new Button("Close") {Margin=new Thickness(2),InstanceName="Default"}; /* {"IsRoot":false} */
            var DatalinkFirmwareCheckHash = new CheckFileMD5Hash() {InstanceName="DatalinkFirmwareCheckHash"}; /* {"IsRoot":false} */
            var Scale5000FirmwareCheckHash = new CheckFileMD5Hash() {InstanceName="Scale5000FirmwareCheckHash"}; /* {"IsRoot":false} */
            var EIDFirmwareCheckHash = new CheckFileMD5Hash() {InstanceName="EIDFirmwareCheckHash"}; /* {"IsRoot":false} */
            var DatalinkVersionCompare = new CompareFirmwareVersions(Libraries.Constants.DataLinkPCVersionNumber) {InstanceName="DatalinkVersionCompare"}; /* {"IsRoot":false} */
            var Scale5000CompareVersions = new CompareFirmwareVersions() {InstanceName="Scale5000CompareVersions"}; /* {"IsRoot":false} */
            var EIDVersionCompare = new CompareFirmwareVersions() {InstanceName="EIDVersionCompare"}; /* {"IsRoot":false} */
            var Scale5000CopyFirmware = new CopyFile() {InstanceName="Scale5000CopyFirmware"}; /* {"IsRoot":false} */
            var Scale5000FileCopyFirmware = new CopyFile() {InstanceName="Scale5000FileCopyFirmware"}; /* {"IsRoot":false} */
            var Scale5000NewVersionAdapter = new DataFlowConnector<SoftwareVersionsSoftwaresSoftware>() {InstanceName="Default"}; /* {"IsRoot":false} */
            var EIDNewVersionAdapter = new DataFlowConnector<SoftwareVersionsSoftwaresSoftware>() {InstanceName="Default"}; /* {"IsRoot":false} */
            var Scale5000CurrentVersionAdapter = new DataFlowConnector<string>() {InstanceName="Default"}; /* {"IsRoot":true} */
            var EIDCurrentVersionAdapter = new DataFlowConnector<string>() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_86d8895b0b0f4f9aaa68a972fe89d413 = new DataFlowConnector<Dictionary<string, SoftwareVersionsSoftwaresSoftware>>() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_2f24b74093424fd1bc5aea1069d41f66 = new DataFlowConnector<int>() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_4c3dceb87c314a8a8b678e5fc2d0c682 = new DataFlowConnector<int>() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_9048ef7559b4412abed65f84cbac1c0f = new DataFlowConnector<int>() {InstanceName="Default"}; /* {"IsRoot":false} */
            var DatalinkUpdateInformation = new DataFlowConnector<SoftwareVersionsSoftwaresSoftware>() {InstanceName="DatalinkUpdateInformation"}; /* {"IsRoot":false} */
            var Scale5000UpdateInformation = new DataFlowConnector<SoftwareVersionsSoftwaresSoftware>() {InstanceName="Scale5000UpdateInformation"}; /* {"IsRoot":false} */
            var SRS2UpdateInformation = new DataFlowConnector<SoftwareVersionsSoftwaresSoftware>() {InstanceName="SRS2UpdateInformation"}; /* {"IsRoot":false} */
            var XRS2UpdateInformation = new DataFlowConnector<SoftwareVersionsSoftwaresSoftware>() {InstanceName="XRS2UpdateInformation"}; /* {"IsRoot":false} */
            var DatalinkURLConnector = new DataFlowConnector<string>() {InstanceName="DatalinkURLConnector"}; /* {"IsRoot":false} */
            var id_16bae58b2a0d4ffd8aa13b5c879dec91 = new DataFlowConnector<string>() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_17497b366d624db98c3ebf44dc382946 = new DataFlowConnector<string>() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_6815142b26ae41f89cc79c24445323dc = new DataFlowConnector<string>() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_728a5ac1f8b641cbbe31217db60c2a58 = new DataFlowConnector<string>() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_7bda0d719e224e67a6360188cbb5d4e0 = new DataFlowConnector<string>() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_b06cf2822c7f4f4b835a086bd0f1e395 = new DataFlowConnector<string>() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_da8b0e31b3bf4ba3bd08d8242020fe3b = new DataFlowConnector<string>() {InstanceName="Default"}; /* {"IsRoot":false} */
            var Scale5000URLConnector = new DataFlowConnector<string>() {InstanceName="Scale5000URLConnector"}; /* {"IsRoot":false} */
            var EIDFileFirmwarePathConnector = new DataFlowConnector<string>() {InstanceName="EIDFileFirmwarePathConnector"}; /* {"IsRoot":false} */
            var EIDURLConnector = new DataFlowConnector<string>() {InstanceName="EIDURLConnector"}; /* {"IsRoot":false} */
            var DatalinkUpdateDownloader = new DownloadFile() {InstanceName="DatalinkUpdateDownloader"}; /* {"IsRoot":false} */
            var Scale5000UpdateDownloader = new DownloadFile() {InstanceName="Scale5000UpdateDownloader"}; /* {"IsRoot":false} */
            var EIDUpdateDownloader = new DownloadFile() {InstanceName="EIDUpdateDownloader"}; /* {"IsRoot":false} */
            var EIDGetConnectionType = new EidCheckConnectionTypeSCP() {InstanceName="EIDGetConnectionType"}; /* {"IsRoot":false} */
            var EIDRecoveryUpdater = new EidFirmwareUpdater() {InstanceName="EIDRecoveryUpdater"}; /* {"IsRoot":false} */
            var EIDFileFirmwareUpdater = new EidFirmwareUpdater(Libraries.Constants.XRS2SRS2DeviceID) {InstanceName="EIDFileFirmwareUpdater"}; /* {"IsRoot":false} */
            var EIDFirmwareUpdater = new EidFirmwareUpdater(Libraries.Constants.XRS2SRS2DeviceID) {InstanceName="EIDFirmwareUpdater"}; /* {"IsRoot":false} */
            var EIDGetFirmware = new EidGetFirmwareVersion() {InstanceName="EIDGetFirmware"}; /* {"IsRoot":false} */
            var EIDFileValidateFirmware = new EidValidateFirmware() {InstanceName="EIDFileValidateFirmware"}; /* {"IsRoot":false} */
            var EIDValidateFirmware = new EidValidateFirmware() {InstanceName="EIDValidateFirmware"}; /* {"IsRoot":false} */
            var id_f20d313fd06e403da33ef1776bb30744 = new Equals<EidCheckConnectionTypeSCP.ConnectionType>(EidCheckConnectionTypeSCP.ConnectionType.USB) {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_6a0ed33d364e4b8ca83c1e823d5469b6 = new Equals<string>("XRS2/SRS2") {InstanceName="Default"}; /* {"IsRoot":false} */
            var AutomaticUpdateConnector = new EventConnector() {InstanceName="AutomaticUpdateConnector"}; /* {"IsRoot":true} */
            var CheckForUpdatesConnector = new EventConnector() {InstanceName="CheckForUpdatesConnector"}; /* {"IsRoot":false} */
            var EIDStartRecovery = new EventConnector() {InstanceName="EIDStartRecovery"}; /* {"IsRoot":true} */
            var id_01f8e70c73ff43a380f1481b09c84ece = new EventConnector() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_022a3938ab7a4c1fb65677843f804e7f = new EventConnector() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_060a112a46b144b28c264ea8d3a5d607 = new EventConnector() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_0b5e823df94f47439d69417edf240291 = new EventConnector() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_0e9d65d79ef34ae9a15711db0b3878ce = new EventConnector() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_1d99f6f0c0814a9b937d443c50b046b8 = new EventConnector() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_25430dabff34418fbd71756647733b18 = new EventConnector() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_348aafca8fbb47b1881c353249fa99aa = new EventConnector() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_38723030ab614b11959de9240504320f = new EventConnector() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_3a404cc30c51441a9469701aaa2ecc1a = new EventConnector() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_3be17c33504c4468ac27b36f4c8725f7 = new EventConnector() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_410e7e9335c14aa197f26f9c74736be4 = new EventConnector() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_492f805cbb794a078536ef45d0dc4d26 = new EventConnector() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_6da9089012774361a11eb8656c3f1c27 = new EventConnector() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_6de2b3b274e444e1bb4b4003c34bdc72 = new EventConnector() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_6e6dbb37f1e5421aaf39df6b0e58b139 = new EventConnector() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_710916bb939648649fd8666b19018ee4 = new EventConnector() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_73ad9f3f0c7e4d588bcea2f02eda960e = new EventConnector() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_8227ecbc757948e28fe240384ed383d9 = new EventConnector() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_8526b505a12141288572333a46989519 = new EventConnector() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_9a7ee90714684ea48f5c1d782d9b9ae0 = new EventConnector() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_ac0b74b3f4d04ee2a0ea1ab59ab7468e = new EventConnector() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_b126d73a74a44b02a5709dbb36b0dca4 = new EventConnector() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_ba8385e1110c4dbcba3e40e2e87c3574 = new EventConnector() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_bacb8c051fd145e8a2f91257c228a448 = new EventConnector() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_c7dff5ad4f394555ad8903ad537b145d = new EventConnector() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_cee9f49f4d84482c8b87d55656caf671 = new EventConnector() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_d1ebefdb03bb4176a194422521115fb9 = new EventConnector() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_d5fa0ff4fc5c42f8b60ccc7cc0d2e2ef = new EventConnector() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_d794423a7ddc4745a2201ba685a3e208 = new EventConnector() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_de2e963b5e9b4d989df90e302c1f19bf = new EventConnector() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_e40c2e783fff47f19fcde91f7bc96521 = new EventConnector() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_e49b7b7ddf2a4c718e6c60aee1471868 = new EventConnector() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_ef17163624f54b978edf4ab11ead0913 = new EventConnector() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_f5960dcbfb3b41c7bf10bc9e7e3e5ed5 = new EventConnector() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_f8b8342e8fee41c3b250e674d96550b6 = new EventConnector() {InstanceName="Default"}; /* {"IsRoot":false} */
            var Scale5000UpdateEvent = new EventConnector() {InstanceName="Scale5000UpdateEvent"}; /* {"IsRoot":false} */
            var EIDUpdateEvent = new EventConnector() {InstanceName="EIDUpdateEvent"}; /* {"IsRoot":false} */
            var UpdateFromFileConnector = new EventConnector() {InstanceName="UpdateFromFileConnector"}; /* {"IsRoot":true} */
            var VersionCollector = new FirmwareVersionCollector() {InstanceName="VersionCollector"}; /* {"IsRoot":false} */
            var DatalinkGetFirmwareSize = new GetDownloadableFileSize() {InstanceName="DatalinkGetFirmwareSize"}; /* {"IsRoot":false} */
            var Scale5000GetFirmwareSize = new GetDownloadableFileSize() {InstanceName="Scale5000GetFirmwareSize"}; /* {"IsRoot":false} */
            var EIDGetFirmwareSize = new GetDownloadableFileSize() {InstanceName="EIDGetFirmwareSize"}; /* {"IsRoot":false} */
            var id_edb5bdfb3da44b508e932fa110579a84 = new GroupingBox("DataLink") {InstanceName="Default"}; /* {"IsRoot":false} */
            var Scale5000NoUpdatesBox = new GroupingBox("XR5000/ID5000",visible:false) {InstanceName="Scale5000NoUpdatesBox"}; /* {"IsRoot":false} */
            var EIDNoUpdatesBox = new GroupingBox("XRS2/SRS2",visible:false) {InstanceName="EIDNoUpdatesBox"}; /* {"IsRoot":false} */
            var id_0367e590a2114498a93a7cbf950cdf92 = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_09a79cabb73e4dfdb329ad8c6f50a213 = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_09aab1c2a8ad4fff8fa6f07eb77499ec = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_11ba57133396478784ea4c89cb5357cf = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_12617eca30694e9fb934cde197439b8d = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_15189e8e68de42968a06b4b3f539cdc1 = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_212660b7fc4a4e03bcf9d48649e4c73f = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_24534e6649aa4ead9e7d6abdccbc466b = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_25c17a5cfac54e5f906153121e11d4bc = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_2d66cfd15d0e4e9b8571a9785c16a14d = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_2df0085865544e46b878964faddb7842 = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_30873862c31c4840b0e4ae2dd48bc7d6 = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_36b06547f2cd41069906e176f9bc8cb4 = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_42a7bbfd9fe24bc399c44a8d3707a9b7 = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_4347c2e07bc64372a83cd0c321664df0 = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_45719901486a4793942a9bf418a9d4ae = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_541370db1876442e8d9fa1fb0d69a21e = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_568a75ed7c0a4fbba9d8f7a240f49636 = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_59acd577b468484d9b9c42361331dccd = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_5b34c4c6aa534a7ab9a3013f562c6df9 = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_5beebf12b0034ec69bf0592483474722 = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_5c49b8393e05476f91f0998e6d41e2e6 = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_5deac2c57ece4c92af0bc9317531ec05 = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_619153a27edf4debb8b7b01741434859 = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_623eebd9457d4dbf894946f0ccc126d7 = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_65846a09cb6244559a126d66559b0bd5 = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_662c4d4b6f1347a09d6bc0913e3d14eb = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_68bac0c5ac754b1ba80fe21ba94aeac0 = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_6b26d9633f9e442d87b56d3f1cc281be = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_6d467b84693a4fdb9346982803b9dbbf = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_7a9e491c06b04eec83f8414563e9e7c2 = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_7ad2183782e24ff6b7da84a8609c5904 = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_7d717e2d9dc94ff28eb2410fffe9fa41 = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_808c9eae18334a629f698d2becdbb33a = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_8214740b79ac49c98df6d3c856ae4e01 = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_89637835ab4644338c0162a190ba353b = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_90a26be3185742d9bb758c0ed5e0b357 = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_90d9318c29174d91b523fbc335d47f73 = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_937bd3431e1243d7ada60d13524ebb50 = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_965fe5fe6f76430c9c9e2b5fe88c710e = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_9dfbf057cf2f427cb562c3894b4f24d6 = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_a18861426efa4c1683bcf9fd60afe8d7 = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_a4a3998e1d744fa28700de9a125618fb = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_a4da70dce6dc442082e738e6c0821bec = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_aaf414a2cfad4c6caaaaeb7aefade72e = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_b25392aa946545988955fe34d7d96cb0 = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_b32786f02be940858fd42179559bc3bf = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_b489c9479d5d4b7dbf0dfc4adfd2ebd8 = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_b9643f4d28534a95befe24a680d6023b = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_bc198912e53e4c55a0589f0cd00d69a4 = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_bf91b3e369574a1faab09dcdbd201e6d = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_c2ee2bbf64704c44b1e43dfaedb2515b = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_c39758d3899143da9c1396ddcce91df4 = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_ce232056f5e347579ca341b60bd9d80d = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_d490253dfdbc4d738e4097faaaf67448 = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_d534a7935e6f4d24a9ac6fba1719c80b = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_dcd25d1e9de44e9d9e70a3875053d611 = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_e18b3e8bd1aa47288c25885d6e5a3fe3 = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_e23ce2bded7a4d8e957cb7c077d8af7e = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_e9378bc3e36346ceb21f237198e63e6d = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_eabd8257465542639396603ec92c3389 = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_eac9b8e35a5c4fe79fa606b84c64b416 = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_eb391e0bf4b346d49e6b07b2aa01be31 = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_ebfef36ca2384811aafa25de122e69fa = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_ec2440a28e094437bf77cd753bf6e8e8 = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_ee858585712c48efa17845004713c1fc = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_f04a8a0948b24741955910bcfa0b43e0 = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_f3f4d20fd4cd4f11919cfeda07484b6f = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_f69ed031238a42458ff7b2c304ba23f2 = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_f6b84866d4f44d7c8061b8e0f9f4ec9a = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var id_fd7f3fd803bf4f08b331edf1dd4eebc0 = new Horizontal() {Margin=new Thickness(5),InstanceName="Default"}; /* {"IsRoot":false} */
            var datalinkUpdateRequest = new HttpRequest("http://sw.tru-test.com/weighing/ADS_Software_Versions_DataLink.xml") {InstanceName="datalinkUpdateRequest"}; /* {"IsRoot":false} */
            var srsUpdateRequest = new HttpRequest("http://sw.tru-test.com/weighing/ads_software_versions_datalinkpolaris.xml") {InstanceName="srsUpdateRequest"}; /* {"IsRoot":false} */
            var id_4e5556ad854c4592938af74c56a727d9 = new IfElse(defaultCondition:false,executeOnDataflow:false) {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_841dcdfbf340481aad3dbda0d921427b = new IfElse(defaultCondition:false,executeOnDataflow:false) {InstanceName="Default"}; /* {"IsRoot":false} */
            var InternetCheckFirmware = new IfElse(defaultCondition:false,executeOnDataflow:false) {InstanceName="InternetCheckFirmware"}; /* {"IsRoot":false} */
            var Scale5000UpdateCheck = new IfElse(defaultCondition:false,executeOnDataflow:false) {InstanceName="Scale5000UpdateCheck"}; /* {"IsRoot":false} */
            var Scale5000UpdateFromFileCheck = new IfElse(defaultCondition:false,executeOnDataflow:false) {InstanceName="Scale5000UpdateFromFileCheck"}; /* {"IsRoot":false} */
            var EIDUpdateCheck = new IfElse(defaultCondition:false,executeOnDataflow:false) {InstanceName="EIDUpdateCheck"}; /* {"IsRoot":false} */
            var EIDUpdateFromFileCheck = new IfElse(defaultCondition:false,executeOnDataflow:false) {InstanceName="EIDUpdateFromFileCheck"}; /* {"IsRoot":false} */
            var EIDFileHexValidate = new IntelHexCheckValidity() {InstanceName="EIDFileHexValidate"}; /* {"IsRoot":false} */
            var EIDHexValidate = new IntelHexCheckValidity() {InstanceName="EIDHexValidate"}; /* {"IsRoot":false} */
            var Scale5000SelectFirmware = new OpenFirmwareBrowser(filter:OpenFirmwareBrowser.Scale5000Filter,title:"5000 Series Update From File") {InstanceName="Scale5000SelectFirmware"}; /* {"IsRoot":false} */
            var EIDSelectFirmware = new OpenFirmwareBrowser(filter:OpenFirmwareBrowser.EidReaderFilter,title:"Update From File") {InstanceName="EIDSelectFirmware"}; /* {"IsRoot":false} */
            var Scale5000UpdateOrGate = new OrEvent() {InstanceName="Scale5000UpdateOrGate"}; /* {"IsRoot":false} */
            var EIDUpdateOrGate = new OrEvent() {InstanceName="EIDUpdateOrGate"}; /* {"IsRoot":false} */
            var DatalinkFailedToStartUpdaterPopup = new PopupWindow("Datalink Failed to Start Update") {InstanceName="DatalinkFailedToStartUpdaterPopup",Width=550,Height=500}; /* {"IsRoot":false} */
            var DatalinkInvalidFirmwarePopup = new PopupWindow("Datalink Invalid Update") {InstanceName="DatalinkInvalidFirmwarePopup",Width=550,Height=500}; /* {"IsRoot":false} */
            var DatalinkUpdateAvailable = new PopupWindow("DataLink Update Available") {InstanceName="DatalinkUpdateAvailable",Width=500,Height=500}; /* {"IsRoot":false} */
            var DatalinkUpdateDownloadProgress = new PopupWindow("Datalink Update Download") {InstanceName="DatalinkUpdateDownloadProgress",Width=500,Height=500}; /* {"IsRoot":false} */
            var EIDRecoveryFlashFailed = new PopupWindow("EID Recovery Failed") {InstanceName="EIDRecoveryFlashFailed",Width=700,Height=500}; /* {"IsRoot":false} */
            var EIDRecoveryFlashProgress = new PopupWindow("EID Recovery Progress") {InstanceName="EIDRecoveryFlashProgress",Width=500,Height=700}; /* {"IsRoot":false} */
            var EIDRecoveryFlashSuccess = new PopupWindow("EID Recovery Success") {InstanceName="EIDRecoveryFlashSuccess",Width=500,Height=500}; /* {"IsRoot":false} */
            var EIDRecoveryNoDeviceType = new PopupWindow("EID Recovery Failure") {InstanceName="EIDRecoveryNoDeviceType",Width=300,Height=300}; /* {"IsRoot":false} */
            var EIDRecoveryNoFilepath = new PopupWindow("EID Recovery Failure") {InstanceName="EIDRecoveryNoFilepath",Width=300,Height=300}; /* {"IsRoot":false} */
            var NoUpdatesAvailable = new PopupWindow("No updates available") {InstanceName="NoUpdatesAvailable",Width=500,Height=500}; /* {"IsRoot":false} */
            var Scale5000CopyFirmwareFailed = new PopupWindow("5000 Series Firmware Update Failed") {InstanceName="Scale5000CopyFirmwareFailed",Width=500,Height=500}; /* {"IsRoot":false} */
            var Scale5000CopyFirmwareSuccess = new PopupWindow("5000 Series Firmware Updated") {InstanceName="Scale5000CopyFirmwareSuccess",Width=500,Height=500}; /* {"IsRoot":false} */
            var Scale5000FileCopyFirmwareFailed = new PopupWindow("5000 Series Firmware Update Failed") {InstanceName="Scale5000FileCopyFirmwareFailed",Width=500,Height=500}; /* {"IsRoot":false} */
            var Scale5000FileCopyFirmwareSuccess = new PopupWindow("5000 Series Firmware Updated") {InstanceName="Scale5000FileCopyFirmwareSuccess",Width=500,Height=500}; /* {"IsRoot":false} */
            var Scale5000InvalidFirmwarePopup = new PopupWindow("5000 Series Invalid Firmware") {InstanceName="Scale5000InvalidFirmwarePopup",Width=550,Height=500}; /* {"IsRoot":false} */
            var Scale5000Update = new PopupWindow("5000 Series Firmware Update Available") {InstanceName="Scale5000Update",Width=500,Height=500}; /* {"IsRoot":false} */
            var Scale5000UpdateDownloadProgress = new PopupWindow("5000 Series Update Download") {InstanceName="Scale5000UpdateDownloadProgress",Width=500,Height=500}; /* {"IsRoot":false} */
            var EIDFileFirmwareFlashFailed = new PopupWindow() {InstanceName="EIDFileFirmwareFlashFailed",Width=700,Height=500}; /* {"IsRoot":false} */
            var EIDFileFirmwareFlashProgress = new PopupWindow() {InstanceName="EIDFileFirmwareFlashProgress",Width=500,Height=700}; /* {"IsRoot":false} */
            var EIDFileFirmwareFlashSuccess = new PopupWindow() {InstanceName="EIDFileFirmwareFlashSuccess",Width=500,Height=500}; /* {"IsRoot":false} */
            var EIDFirmwareFlashFailed = new PopupWindow() {InstanceName="EIDFirmwareFlashFailed",Width=700,Height=500}; /* {"IsRoot":false} */
            var EIDFirmwareFlashProgress = new PopupWindow() {InstanceName="EIDFirmwareFlashProgress",Width=500,Height=700}; /* {"IsRoot":false} */
            var SRS2FirmwareFlashSuccess = new PopupWindow() {InstanceName="SRS2FirmwareFlashSuccess",Width=500,Height=500}; /* {"IsRoot":false} */
            var EIDInvalidFirmwarePopup = new PopupWindow() {InstanceName="EIDInvalidFirmwarePopup",Width=550,Height=500}; /* {"IsRoot":false} */
            var EIDUpdate = new PopupWindow() {InstanceName="EIDUpdate",Width=500,Height=500}; /* {"IsRoot":false} */
            var EIDUpdateDownloadProgress = new PopupWindow() {InstanceName="EIDUpdateDownloadProgress",Width=500,Height=500}; /* {"IsRoot":false} */
            var UpdateNoInternetConnection = new PopupWindow("No internet connection detected") {InstanceName="UpdateNoInternetConnection",Width=500,Height=500}; /* {"IsRoot":false} */
            var id_045a7a953f514ccba9009d428160e0ff = new ProgressBar(200) {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_06111b2f3a07446ebf7090f6329117f7 = new ProgressBar(100) {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_398046e40f6545f892e7a394660ea06f = new ProgressBar(100) {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_8155771c5fd34b158166269259680aec = new ProgressBar(200) {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_ae15b3aadf7a460f93d014f3a57ccc51 = new ProgressBar(200) {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_bf142284640d4b17b8c170a5b2f4ec20 = new ProgressBar(100) {InstanceName="Default"}; /* {"IsRoot":false} */
            var DatalinkUpdateArbitrator = new RequestArbitrator() {InstanceName="DatalinkUpdateArbitrator"}; /* {"IsRoot":false} */
            var Scale5000UpdateArbitrator = new RequestArbitrator() {InstanceName="Scale5000UpdateArbitrator"}; /* {"IsRoot":false} */
            var EIDUpdateArbitrator = new RequestArbitrator() {InstanceName="EIDUpdateArbitrator"}; /* {"IsRoot":false} */
            var id_31bfaf1ab46b4e27a451490a9d8a438d = new RetrieveValueFromDict<string, SoftwareVersionsSoftwaresSoftware>("EziLink") {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_6cde3c2e162d4f34bdb85b3602cc46fc = new RetrieveValueFromDict<string, SoftwareVersionsSoftwaresSoftware>("Scale5000") {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_72fe7772e6c94325b3094fbd20f76e6d = new RetrieveValueFromDict<string, SoftwareVersionsSoftwaresSoftware>("XRS2") {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_b95f831e77cf42e9a33d1a89a81757c2 = new RetrieveValueFromDict<string, SoftwareVersionsSoftwaresSoftware>("SRS2") {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_068c82c6fb6d4c3d9cf242269ee31e75 = new RightJustify() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_12d88afdbcce4f64a252f127220f46b5 = new RightJustify() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_4e45a0f491a8417fae2e36a1d501111e = new RightJustify() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_57f95ab8d62c43daaba2d5c1dd6dc70c = new RightJustify() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_77bbfea542e343d79500adf6bf08e0b3 = new RightJustify() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_9160374cd164400abe71497fe223819e = new RightJustify() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_cf4509434ef943fd8b69f2a0323c2c71 = new RightJustify() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_f921d34001b443b2a7c598c76291f96d = new RightJustify() {InstanceName="Default"}; /* {"IsRoot":false} */
            var EIDRecoveryFailedStatusString = new Text(wrap:true) {InstanceName="EIDRecoveryFailedStatusString"}; /* {"IsRoot":false} */
            var EIDRecoveryStatusString = new Text() {InstanceName="EIDRecoveryStatusString"}; /* {"IsRoot":false} */
            var id_0286fe46de7b4e8c93f6a55476a14b9d = new Text("Your 5000 series indicator has been loaded with the latest firmware. Please unplug the USB cable from your device to complete the update.",wrap:true) {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_02f027db3a334a1183fb9a0dd846c3d3 = new Text("DO NOT DISCONNECT YOUR DEVICE DURING PROGRAMMING") {InstanceName="Default",Color=Brushes.Red}; /* {"IsRoot":false} */
            var id_031bf4a14d094dd4868d736e15828ec4 = new Text(wrap:true) {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_08bc031738da429b9a7b49d83a1d8c59 = new Text() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_0b85e8bd4cb549e3835e1ecff7b24e05 = new Text("There is an update available for your 5000 series indicator. Would you like to update now?") {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_12296006bbde48388c5ea71742c7aba3 = new Text("The firmware downloaded is not valid. Please try again or contact Tru-Test customer support.") {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_1639feb335854759bdc24bc212373f51 = new Text(wrap:true) {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_18b51fb517794f14aaae9bafcf1cd505 = new Text() {InstanceName="Default",Margin=new Thickness(5)}; /* {"IsRoot":false} */
            var id_18c44ec4315041b59ee9a69bc9194871 = new Text() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_3cdbdb36ab0a46ff8bc0e6f56f50fca8 = new Text() {InstanceName="Default",Margin=new Thickness(5)}; /* {"IsRoot":false} */
            var id_3fb20eb1cb8640d5ba33131a0f584800 = new Text("There was an error starting the updater. Please try again or contact Tru-Test customer support.",wrap:true) {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_4c67692140cd40a58cf529c3bad90286 = new Text() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_4d471c7dc54449309d14ef6e6aa7ea27 = new Text("The update downloaded is not valid. Please try again or contact Tru-Test customer support.") {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_51beab2afeb045ab811e5965253eb9f5 = new Text("The firmware downloaded is not valid. Please try again or contact Tru-Test customer support.") {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_532d7df13ad94fa1b2dc302ff6fecb99 = new Text("No internet connection was detected. To check for updates, please reconnect to the internet and restart Datalink. Automatic updates can be disabled from the updates menu.",wrap:true) {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_5565ca8ec66943e99a211f30ea5b9df7 = new Text($"Current Version: {Libraries.Constants.DataLinkPCVersionNumber}") {InstanceName="Default",Margin=new Thickness(5)}; /* {"IsRoot":false} */
            var id_557831077d564bdaac282bde9f758d38 = new Text("There was an error installing the given firmware on your 5000 series device. Please try again or contact Tru-Test customer support.",wrap:true) {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_61aa5f189b634472b5025356317f099f = new Text("DO NOT DISCONNECT YOUR DEVICE DURING PROGRAMMING") {InstanceName="Default",Color=Brushes.Red}; /* {"IsRoot":false} */
            var id_670bc7265cfe4101a8070f384a2147e8 = new Text() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_677572cfb2954441b3a5c942b4a31f3f = new Text("DO NOT DISCONNECT YOUR DEVICE DURING PROGRAMMING") {InstanceName="Default",Color=Brushes.Red}; /* {"IsRoot":false} */
            var id_7173a158f8dd41a4b3d0548b899b7cee = new Text("The firmware on your EID reader was successfully flashed. Please unplug the device and attempt to turn it on.",wrap:true) {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_7cb2264c069e4fed8116008adb783994 = new Text() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_87414f2af84e4779a9592b30ac7b71d1 = new Text("The firmware update for your SRS2 failed. Please try again or contact Tru-Test customer support with the given error message.",wrap:true) {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_9564a3d23b4c4f05b3ac75c3ad5f77bd = new Text() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_970d31d5cb904cbd8f87cce03a765832 = new Text() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_982e30e597734113a1791bde6a4783c5 = new Text(wrap:true) {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_a2830b2b842a49a7bd39efb9903e4835 = new Text() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_ac60bdd9d2c942e4a2a3e78514f0e225 = new Text() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_af2ddc149f4341eb8f52fa0cbd976683 = new Text("There is an update available for DataLink. Would you like to update now?") {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_b01f7670dbd142a489fc36748da3c932 = new Text("The given file path does not exist.",wrap:true) {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_b0e1629606704fb19e35864cd6d343ca = new Text(wrap:true) {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_bf88445f893d4a59b373aca6f5a96ea7 = new Text("Your devices are up to date.") {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_c1d2e735c5204b90b768b3b8f45750fa = new Text("Please select a device type.",wrap:true) {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_c836df4337004834953b8b5de3600ce6 = new Text() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_cf0ea7d7df94461198efeeb176676f43 = new Text() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_d129d3c94b974c699a9a52af6260a195 = new Text() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_edae258e300f4b9b837ebdc5384e803b = new Text("There was an error installing the latest firmware on your 5000 series device. Please try again or contact Tru-Test customer support.",wrap:true) {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_f5c8da14ee334ae492b7d633077e8c5a = new Text("Your 5000 series indicator has been loaded with the given firmware. Please unplug the USB cable from your device to complete the update.",wrap:true) {InstanceName="Default"}; /* {"IsRoot":false} */
            var EIDFileUpdateFailedStatusString = new Text(wrap:true) {InstanceName="EIDFileUpdateFailedStatusString"}; /* {"IsRoot":false} */
            var EIDFileUpdateStatusString = new Text() {InstanceName="EIDFileUpdateStatusString"}; /* {"IsRoot":false} */
            var EIDUpdateFailedStatusString = new Text(wrap:true) {InstanceName="EIDUpdateFailedStatusString"}; /* {"IsRoot":false} */
            var EIDUpdateStatusString = new Text() {InstanceName="EIDUpdateStatusString"}; /* {"IsRoot":false} */
            var id_577c3d09dac64fec886d19d9003fa585 = new TupleAbstraction<string, SoftwareVersionsSoftwaresSoftware>() {InstanceName="Default"}; /* {"IsRoot":false} */
            var id_c5b916ac91e8438db526d41a516fc91a = new TupleAbstraction<string, SoftwareVersionsSoftwaresSoftware>() {InstanceName="Default"}; /* {"IsRoot":false} */
            var Scale5000FirmwareTuple = new TupleAbstraction<string, SoftwareVersionsSoftwaresSoftware>() {InstanceName="Scale5000FirmwareTuple"}; /* {"IsRoot":false} */
            var Scale5000FileFirmwareTuple = new TupleAbstraction<string, string>() {InstanceName="Scale5000FileFirmwareTuple"}; /* {"IsRoot":false} */
            var DatalinkRunUpdater = new RunExecutable() {InstanceName="DatalinkRunUpdater"}; /* {"IsRoot":false} */
            var DatalinkUpdateParser = new XMLParser<SoftwareVersions>() {InstanceName="DatalinkUpdateParser"}; /* {"IsRoot":false} */
            var XRSUpdateParser = new XMLParser<SoftwareVersions>() {InstanceName="XRSUpdateParser"}; /* {"IsRoot":false} */
            var id_ed5d83930c4d4fe7b27d0b9016939366 = new Apply<string, bool>() {InstanceName="Default",Lambda=s => File.Exists(s)}; /* {"IsRoot":false} */
            var EIDUpdateInformation = new Switch<SoftwareVersionsSoftwaresSoftware, string>() {InstanceName="EIDUpdateInformation",Lambda=s => s.Name}; /* {"IsRoot":false} */
            var EIDUpdateConnector = new DataFlowConnector<SoftwareVersionsSoftwaresSoftware>() {InstanceName="EIDUpdateConnector"}; /* {"IsRoot":false} */
            var EIDSwitchFirmwareCode = new Switch<string, string>() {InstanceName="EIDSwitchFirmwareCode",compareValues=new string[]{"SRS2", "XRS2"}}; /* {"IsRoot":false} */
            var id_59680a5a0b2d494bb31764eade2b405f = new Data<string>() {InstanceName="id_59680a5a0b2d494bb31764eade2b405f",storedData=Libraries.Constants.SRS2FirmwareCode}; /* {"IsRoot":false} */
            var id_e32da1bd4dde4026a042ce4290e8d7aa = new Data<string>() {InstanceName="id_e32da1bd4dde4026a042ce4290e8d7aa",storedData=Libraries.Constants.XRS2FirmwareCode}; /* {"IsRoot":false} */
            var id_70cf715179874505ab5f042e1682d56e = new Apply<string, string>() {InstanceName="id_70cf715179874505ab5f042e1682d56e",Lambda=s => $"There is an update available for your {s}. Would you like to update now?",LatchData=true}; /* {"IsRoot":false} */
            var id_c92ed5ca8c454e569abff95c19ced160 = new Apply<string, string>() {InstanceName="id_c92ed5ca8c454e569abff95c19ced160",Lambda=s => $"{s} Firmware Update Available",LatchData=true}; /* {"IsRoot":false} */
            var id_a78d88158171491097fa7e9a7ec4c434 = new Apply<string, string>() {InstanceName="id_a78d88158171491097fa7e9a7ec4c434",Lambda=s => $"{s} Firmware Update Download",LatchData=true}; /* {"IsRoot":false} */
            var id_23072e7e52b54341bac02d2f6136f905 = new Apply<string, string>() {InstanceName=default,Lambda=s => $"{s} Firmware Update",LatchData=true}; /* {"IsRoot":false} */
            var id_1ccde769729441a882f2e57353493c51 = new Apply<string, string>() {InstanceName="id_1ccde769729441a882f2e57353493c51",Lambda=s => $"{s} Firmware Update Success",LatchData=true}; /* {"IsRoot":false} */
            var id_ebe2e08f45aa4a2b9301b07d7141bcb7 = new Apply<string, string>() {InstanceName="id_ebe2e08f45aa4a2b9301b07d7141bcb7",Lambda=s => $"The firmware on your {s} was successfully updated.",LatchData=true}; /* {"IsRoot":false} */
            var id_09c1ae0c0c0f43339537ab6df35528a8 = new Apply<string, string>() {InstanceName="id_09c1ae0c0c0f43339537ab6df35528a8",Lambda=s => $"{s} Firmware Update Failed",LatchData=true}; /* {"IsRoot":false} */
            var id_6c288085eb414204895f754161c31577 = new Apply<string, string>() {InstanceName="id_6c288085eb414204895f754161c31577",Lambda=s => $"The firmware update for your {s} failed. Please try again or contact Tru-Test customer support with the given error message.",LatchData=true}; /* {"IsRoot":false} */
            var id_114514ea2f1b4eb7b4f2587f3ed018cf = new DataFlowConnector<string>() {InstanceName="id_114514ea2f1b4eb7b4f2587f3ed018cf"}; /* {"IsRoot":false} */
            var id_ca510aa8b8364bfc95e7b5805a67061a = new DataFlowConnector<string>() {InstanceName="id_ca510aa8b8364bfc95e7b5805a67061a"}; /* {"IsRoot":false} */
            var id_0e7cf56442ea43598a50e21cb5e7e2d1 = new DataFlowConnector<string>() {InstanceName="id_0e7cf56442ea43598a50e21cb5e7e2d1"}; /* {"IsRoot":false} */
            var id_1778a5a3ac204908b97abacafd6d4938 = new DataFlowConnector<string>() {InstanceName="id_1778a5a3ac204908b97abacafd6d4938"}; /* {"IsRoot":false} */
            var id_7e5736dde59b4d05bfc9f5c32f7b6c7b = new DataFlowConnector<string>() {InstanceName="id_7e5736dde59b4d05bfc9f5c32f7b6c7b"}; /* {"IsRoot":false} */
            var id_4330f2fdc8fb473a8b15452a0aeebd9b = new Apply<string, string>() {InstanceName="id_4330f2fdc8fb473a8b15452a0aeebd9b",Lambda=s => $"{s} Invalid Firmware"}; /* {"IsRoot":false} */
            var id_d925cfbf11de4fc1a0d4c1d0ea69c86a = new DataFlowConnector<string>() {InstanceName="id_d925cfbf11de4fc1a0d4c1d0ea69c86a"}; /* {"IsRoot":false} */
            var ReinstallUSBDriverConnector = new EventConnector() {InstanceName="ReinstallUSBDriverConnector"}; /* {"IsRoot":true} */
            var StartUSBDriverInstallation = new RunExecutable(executableLocation:Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + @"\CypressDriverInstaller_1.exe") {InstanceName="StartUSBDriverInstallation"}; /* {"IsRoot":false} */
            var USBDriverReinstallFailed = new PopupWindow(title:"USB Driver Reinstallation Failed") {InstanceName="USBDriverReinstallFailed",Width=500,Height=500}; /* {"IsRoot":false} */
            var id_4a271f7bf5b04314af7834622530ced5 = new Text(text:"There was an error starting the USB driver reinstallation. Please try again or contact Tru-Test customer support.",wrap:true) {InstanceName="id_4a271f7bf5b04314af7834622530ced5",Margin=new Thickness(5)}; /* {"IsRoot":false} */
            var id_47d3fe737d5741ae8692d628049cf688 = new Button(title:"Close") {Margin=new Thickness(5),InstanceName="id_47d3fe737d5741ae8692d628049cf688"}; /* {"IsRoot":false} */
            var DatalinkUnzipUpdater = new UnzipFile(pattern:"^.*\\.exe$",unzipLocation:@"C:\ProgramData\Tru-Test\Datalink_ALA\Updates") {InstanceName="DatalinkUnzipUpdater"}; /* {"IsRoot":false} */
            var id_4770c103caf14c028fbc7e1bf7d0591d = new EventGate() {LatchEvent=false,InstanceName="id_4770c103caf14c028fbc7e1bf7d0591d"}; /* {"IsRoot":false} */
            // END AUTO-GENERATED INSTANTIATIONS FOR FirmwareUpdate
            #endregion

            #region XR5000_Functionalities INSTANTIATIONS
            // BEGIN AUTO-GENERATED INSTANTIATIONS FOR XR5000_Functionalities
            var stringXR5000Connected = new LiteralString("Connected to XR5000") { InstanceName = "stringXR5000Connected" }; /* {"IsRoot":false} */
            var XR5000DeleteMenu = new MenuItem("Delete information off device", false) { IconName = "5000Delete.png", InstanceName = "XR5000DeleteMenu" }; /* {"IsRoot":false} */
            var XR5000ExportMenu = new MenuItem("Put information onto device", false) { IconName = "5000Export.png", InstanceName = "XR5000ExportMenu" }; /* {"IsRoot":false} */
            var XR5000ImportMenu = new MenuItem("Get information off device", false) { IconName = "5000Import.png", InstanceName = "XR5000ImportMenu" }; /* {"IsRoot":false} */
            var XR5000getInfoWizard = new Wizard("Get information off device") {SecondTitle="What information do you want to get off the device?",InstanceName="XR5000getInfoWizard"}; /* {"IsRoot":false} */
            var id_39cc8679a22b46b4aebeef125cd5f0ae = new WizardItem("Get selected session files") {ImageName="Icon_Session.png",Checked=true,InstanceName="Default"}; /* {"IsRoot":false} */
            var id_5ffa0209425b4dcea9e1a3f02ff216ea = new WizardItem("Save all animal lifetime information as a file on the PC") { ImageName = "Icon_Animal.png", InstanceName = "Default" }; /* {"IsRoot":false} */
            var id_8ea685db14ec42f4bd8bb4a297d16289 = new WizardItem("Save favourite setups as files on the PC") { ImageName = "Favourite_48x48px.png", InstanceName = "Default" }; /* {"IsRoot":false} */
            var id_f62a188e32b04fd494e53c0486fd417d = new WizardItem("Back up the device database onto the PC") { ImageName = "Icon_Database.png", InstanceName = "Default" }; /* {"IsRoot":false} */
            var id_673d82ee3c3b49e39694cd342c73d4ad = new WizardItem("Generate a report") { ImageName = "reporticon.png", InstanceName = "Default" }; /* {"IsRoot":false} */
            var XR5000getSessionWizard = new Wizard("Get information off device") {SecondTitle="What do you want to do with the session files?",ShowBackButton=true,InstanceName="XR5000getSessionWizard"}; /* {"IsRoot":false} */
            var id_210bf17d3a114bc5a7a19a6ab678dc48 = new WizardItem("Save selected session files as files on the PC") {ImageName="Icon_Session.png",Checked=true,InstanceName="Default"}; /* {"IsRoot":false} */
            var id_ae94e2df21a4408e8b0bc9a55d2d481e = new WizardItem("Send records to NAIT") { ImageName = "NAIT.png", InstanceName = "Default" }; /* {"IsRoot":false} */
            var id_09bf8feb99e14e8e82def502ce66a0bf = new WizardItem("Send records to NLIS") { ImageName = "NLIS_logo.jpg", InstanceName = "Default" }; /* {"IsRoot":false} */
            var id_db985bc62bfd412d8db7559b1ffa2ad0 = new WizardItem("Send sessions to MiHub Livestock") { ImageName = "MiHub40x40_ltblue_cloud.png", InstanceName = "Default" }; /* {"IsRoot":false} */
            var XR5000putInfoWizard = new Wizard("Put information onto device") {SecondTitle="What information do you want to put onto the device?",InstanceName="XR5000putInfoWizard"}; /* {"IsRoot":false} */
            var id_748496bee52a4661b3699f367afb7904 = new WizardItem("Session files") {ImageName="Icon_Session.png",Checked=true,InstanceName="Default"}; /* {"IsRoot":false} */
            var id_48aaec02f3c245329aa25660a915faf7 = new WizardItem() { ContentText = "Animal lifetime information", ImageName = "Icon_Animal.png", InstanceName = "Default" }; /* {"IsRoot":false} */
            var id_a2cf0f9732584f008b372e537dd1cefb = new WizardItem("Favourite setups from my PC") { ImageName = "Favourite_48x48px.png", InstanceName = "Default" }; /* {"IsRoot":false} */
            var id_835f7e3a147544e2997e5e7edeb864a7 = new WizardItem("Favourite setups from the Tru-Test website") { ImageName = "Icon_Favourite_Web.png", InstanceName = "Default" }; /* {"IsRoot":false} */
            var XR5000Tools = new Horizontal() { InstanceName = "XR5000Tools" }; /* {"IsRoot":false} */
            var XR5000DeleteTool = new Tool("5000Delete.png", false) { InstanceName = "XR5000DeleteTool", ToolTip = "Delete information off device" }; /* {"IsRoot":false} */
            var XR5000ExportTool = new Tool("5000Export.png", false) { InstanceName = "XR5000ExportTool", ToolTip = "Put information onto device" }; /* {"IsRoot":false} */
            var XR5000ImportTool = new Tool("5000Import.png", false) { InstanceName = "XR5000ImportTool", ToolTip = "Get information off device" }; /* {"IsRoot":false} */
            // END AUTO-GENERATED INSTANTIATIONS FOR XR5000_Functionalities
            #endregion

            #region ID5000_Functionalities INSTANTIATIONS
            // BEGIN AUTO-GENERATED INSTANTIATIONS FOR ID5000_Functionalities.xmind
            var stringID5000Connected = new LiteralString(liter:"Connected to ID5000") {InstanceName="stringID5000Connected"}; /* {"IsRoot":false} */
            var ID5000ImportMenu = new MenuItem(title:"Get information off device",visible:false) {IconName="5000Import.png",InstanceName="ID5000ImportMenu"}; /* {"IsRoot":false} */
            var ID5000ExportMenu = new MenuItem(title:"Put information onto device",visible:false) {IconName="5000Export.png",InstanceName="ID5000ExportMenu"}; /* {"IsRoot":false} */
            var ID5000DeleteMenu = new MenuItem(title:"Delete information off device",visible:false) {IconName="5000Delete.png",InstanceName="ID5000DeleteMenu"}; /* {"IsRoot":false} */
            var ID5000ImportTool = new Tool(ImageName:"5000Import.png",visible:false) {InstanceName="ID5000ImportTool",ToolTip="Get information off device"}; /* {"IsRoot":false} */
            var ID5000ExportTool = new Tool(ImageName:"5000Export.png",visible:false) {InstanceName="ID5000ExportTool",ToolTip="Put information onto device"}; /* {"IsRoot":false} */
            var ID5000DeleteTool = new Tool(ImageName:"5000Delete.png",visible:false) {InstanceName="ID5000DeleteTool",ToolTip="Delete information off device"}; /* {"IsRoot":false} */
            var ID5000Tools = new Horizontal() {InstanceName="ID5000Tools"}; /* {"IsRoot":false} */
            // END AUTO-GENERATED INSTANTIATIONS FOR ID5000_Functionalities.xmind
            #endregion
            
            #region XRS2_Functionalities INSTANTIATIONS
            // BEGIN AUTO-GENERATED INSTANTIATIONS FOR XRS2_Functionalities
            var stringXRS2Connected = new LiteralString("Connected to XRS2") {InstanceName="stringXRS2Connected"}; /* {"IsRoot":false} */
            var XRS2DeleteMenu = new MenuItem("Delete information off device",false) {IconName="XRSDelete.png",InstanceName="XRS2DeleteMenu"}; /* {"IsRoot":false} */
            var XRS2ExportMenu = new MenuItem("Put information onto device",false) {IconName="XRSExport.png",InstanceName="XRS2ExportMenu"}; /* {"IsRoot":false} */
            var XRS2ImportMenu = new MenuItem("Get information off device",false) {IconName="XRSImport.png",InstanceName="XRS2ImportMenu"}; /* {"IsRoot":false} */
            var XRS2getInfoWizard = new Wizard("Get information off device") {SecondTitle="What information do you want to get off the device?",InstanceName="XRS2getInfoWizard"}; /* {"IsRoot":false} */
            var id_4216ce5152f947a3a426ae090a8ad744 = new WizardItem("Get selected session files") {ImageName="Icon_Session.png",Checked=true,InstanceName="Default"}; /* {"IsRoot":false} */
            var id_df57bd52cb3a4d23ba1c48843960fb51 = new WizardItem("Save all animal lifetime information as a file on the PC") {ImageName="Icon_Animal.png",InstanceName="Default"}; /* {"IsRoot":false} */
            var id_de93ebf8c103419db2e78ce17ae77d25 = new WizardItem("Save alerts as a file on the PC") {ImageName="Icon_Alert.png",InstanceName="Default"}; /* {"IsRoot":false} */
            var id_cc1a52c5e51a41a393bb650f6d028f82 = new WizardItem("Save favourite setups as files on the PC") {ImageName="Icon_Alert.png",InstanceName="Default"}; /* {"IsRoot":false} */
            var XRS2putInfoWizard = new Wizard("Put information onto device") {SecondTitle="What information do you want to put onto the device?",InstanceName="XRS2putInfoWizard"}; /* {"IsRoot":false} */
            var id_d460f06dbd154e6384414880181c02c6 = new WizardItem("Session files") {ImageName="Icon_Session.png",Checked=true,InstanceName="Default"}; /* {"IsRoot":false} */
            var id_597725dd93f142ef8fe04aac94e4ef93 = new WizardItem("Animal lifetime information") {ImageName="Icon_Animal.png",InstanceName="Default"}; /* {"IsRoot":false} */
            var id_dece0c065b8c4e6ea2760f8fc59ae5e6 = new WizardItem("Favourite setups from my PC") {ImageName="Favourite_48x48px.png",InstanceName="Default"}; /* {"IsRoot":false} */
            var id_aafe87cd67214b5a96d5e01feb73055d = new WizardItem("Alerts file") {ImageName="Icon_Alert.png",InstanceName="Default"}; /* {"IsRoot":false} */
            var XRS2Tools = new Horizontal() {InstanceName="XRS2Tools"}; /* {"IsRoot":false} */
            var XRS2DeleteTool = new Tool("XRSDelete.png",false) {InstanceName="XRS2DeleteTool",ToolTip="Delete information off device"}; /* {"IsRoot":false} */
            var XRS2ExportTool = new Tool("XRSExport.png",false) {InstanceName="XRS2ExportTool",ToolTip="Put information onto device"}; /* {"IsRoot":false} */
            var XRS2ImportTool = new Tool("XRSImport.png",false) {InstanceName="XRS2ImportTool",ToolTip="Get information off device"}; /* {"IsRoot":false} */
            // END AUTO-GENERATED INSTANTIATIONS FOR XRS2_Functionalities
            #endregion

            #region DeleteSessions_XRS2_SRS2 INSTANTIATIONS
            // BEGIN AUTO-GENERATED INSTANTIATIONS FOR DeleteSessions_XRS2_SRS2
            var EIDDeleteSessions = new Wizard(title:"Delete information off device") {SecondTitle="What information do you want to delete off the device?",Width=600,InstanceName="EIDDeleteSessions"}; /* {"IsRoot":true} */
            var id_48d017247e2747059f60cfb040f4c3bf = new PopupWindow(title:"Delete all sessions") {InstanceName="id_48d017247e2747059f60cfb040f4c3bf",Width=500,Height=500}; /* {"IsRoot":false} */
            var id_797742532e0e4b0498843a54daf1793f = new Horizontal() {Margin=new Thickness(5),InstanceName="id_797742532e0e4b0498843a54daf1793f"}; /* {"IsRoot":false} */
            var id_9b6b728aa2484103a955e1e1aa84c0ee = new Horizontal() {InstanceName="id_9b6b728aa2484103a955e1e1aa84c0ee"}; /* {"IsRoot":false} */
            var id_1336f6c1f70645bb8a177050ceefd398 = new Text(text:"Are you sure you want to delete all information off your device?") {InstanceName="id_1336f6c1f70645bb8a177050ceefd398"}; /* {"IsRoot":false} */
            var id_4e7c07d78a2b4ca9affeb437e8c74df4 = new RightJustify() {InstanceName="id_4e7c07d78a2b4ca9affeb437e8c74df4"}; /* {"IsRoot":false} */
            var id_c713f89a6cb944679cda8e87e4285753 = new Button(title:"Yes") {Width=100,Height=35,Margin=new Thickness(5),InstanceName="id_c713f89a6cb944679cda8e87e4285753"}; /* {"IsRoot":false} */
            var id_13173868102b466d84cf780f38376486 = new Button(title:"Cancel") {Width=100,Height=35,Margin=new Thickness(5),InstanceName="id_13173868102b466d84cf780f38376486"}; /* {"IsRoot":false} */
            var EIDDeleteAllSessions = new EventConnector() {InstanceName="EIDDeleteAllSessions"}; /* {"IsRoot":true} */
            var id_99513a6d5a09462a9cf09c2814d4582e = new SessionDeleteSCP(deleteAll:true) {InstanceName="id_99513a6d5a09462a9cf09c2814d4582e"}; /* {"IsRoot":false} */
            var id_f70b9a4eec2d49b3a7291d4ef5d0a0c3 = new PopupWindow(title:"Successfully deleted sessions") {InstanceName="id_f70b9a4eec2d49b3a7291d4ef5d0a0c3",Width=500,Height=500}; /* {"IsRoot":false} */
            var id_4a156b60efdb4abeb774e5a60eca9942 = new Horizontal() {Margin=new Thickness(5),InstanceName="id_4a156b60efdb4abeb774e5a60eca9942"}; /* {"IsRoot":false} */
            var id_8b9bd9c431684d319c570258beaf8156 = new Text(text:"Successfully deleted the information off your device.",wrap:true) {InstanceName="id_8b9bd9c431684d319c570258beaf8156"}; /* {"IsRoot":false} */
            var id_b5081ffa25064584a05fc3986466d713 = new Horizontal() {Margin=new Thickness(5),InstanceName="id_b5081ffa25064584a05fc3986466d713"}; /* {"IsRoot":false} */
            var id_bac2620e78614dde9dfe2bc2f160de15 = new Button(title:"Close") {InstanceName="id_bac2620e78614dde9dfe2bc2f160de15"}; /* {"IsRoot":false} */
            var id_1cb57a3c0e6d4d1c92125280481be643 = new PopupWindow(title:"Error deleting sessions") {InstanceName="id_1cb57a3c0e6d4d1c92125280481be643",Width=500,Height=500}; /* {"IsRoot":false} */
            var id_1d3d376dbec34ba3b48754fe3fda25e0 = new Horizontal() {Margin=new Thickness(5),InstanceName="id_1d3d376dbec34ba3b48754fe3fda25e0"}; /* {"IsRoot":false} */
            var id_6b2396c36888442baaac6c495e616bca = new Text(text:"There was an error deleting the information off your device. Please try again or contact Tru-Test customer support.",wrap:true) {InstanceName="id_6b2396c36888442baaac6c495e616bca"}; /* {"IsRoot":false} */
            var id_ec1017d8cac4449bb91ee3573a2c7093 = new Horizontal() {Margin=new Thickness(5),InstanceName="id_ec1017d8cac4449bb91ee3573a2c7093"}; /* {"IsRoot":false} */
            var id_4f418d359b864eeb884f40bd88adfee7 = new Button(title:"Close") {InstanceName="id_4f418d359b864eeb884f40bd88adfee7"}; /* {"IsRoot":false} */
            var EIDDeleteSelectedSessions = new EventConnector() {InstanceName="EIDDeleteSelectedSessions"}; /* {"IsRoot":true} */
            var id_679dbd92de28437f86537199e22e801f = new Transact() {InstanceName="id_679dbd92de28437f86537199e22e801f",ClearDestination=true,AutoLoadNextBatch=true}; /* {"IsRoot":false} */
            var id_03660d1db16d4e1a9bbba3d2d9b7caac = new Filter() {InstanceName="id_03660d1db16d4e1a9bbba3d2d9b7caac",FilterDelegate=(DataRow r) =>{    return bool.Parse(r["checkbox"].ToString());}}; /* {"IsRoot":false} */
            var SessionDeleteSelectedSessions = new SessionDeleteSCP(deleteAll:false) {InstanceName="SessionDeleteSelectedSessions"}; /* {"IsRoot":false} */
            var id_30c6e35bc85c4a91b6b8fa0a92e434f2 = new DataFlowConnector<DataTable>() {InstanceName="id_30c6e35bc85c4a91b6b8fa0a92e434f2"}; /* {"IsRoot":false} */
            var id_39c3d1547b2d49c8b826ec82382f88c2 = new Apply<DataTable, int>(latchData:default) {InstanceName="id_39c3d1547b2d49c8b826ec82382f88c2",Lambda=(DataTable table) =>{    return table.Rows.Count;}}; /* {"IsRoot":false} */
            var id_6a671f14aaa74810ad35f3cb58338dbe = new PopupWindow(title:"Delete selected sessions") {InstanceName="id_6a671f14aaa74810ad35f3cb58338dbe",Width=500,Height=500}; /* {"IsRoot":false} */
            var id_ce81189a00ff48d2a477b9591cc68c4a = new Horizontal() {Margin=new Thickness(5),InstanceName="id_ce81189a00ff48d2a477b9591cc68c4a"}; /* {"IsRoot":false} */
            var id_868bdb726abd400dad035fdaa4ae6fa5 = new Text() {InstanceName="id_868bdb726abd400dad035fdaa4ae6fa5"}; /* {"IsRoot":false} */
            var id_c7c7a1bb53af4646bbd253c377ca4133 = new Apply<int, string>(latchData:default) {InstanceName="id_c7c7a1bb53af4646bbd253c377ca4133",Lambda=(i) => $"Are you sure you want to delete {i} session(s)?"}; /* {"IsRoot":false} */
            var id_3ebca753f2b74540a303abf516ccaf6b = new Horizontal() {InstanceName="id_3ebca753f2b74540a303abf516ccaf6b"}; /* {"IsRoot":false} */
            var id_e5c3854d95c1490c952b5dc0afd9ca0a = new RightJustify() {InstanceName="id_e5c3854d95c1490c952b5dc0afd9ca0a"}; /* {"IsRoot":false} */
            var id_33893ffce90944808e3f8536badf39eb = new Button(title:"Yes") {Width=100,Height=35,Margin=new Thickness(5),InstanceName="id_33893ffce90944808e3f8536badf39eb"}; /* {"IsRoot":false} */
            var id_56bd217ea1e643f8b2d3d774212c7f44 = new Button(title:"Cancel") {Width=100,Height=35,Margin=new Thickness(5),InstanceName="id_56bd217ea1e643f8b2d3d774212c7f44"}; /* {"IsRoot":false} */
            var id_d136475f9a66438a87c0d8c1977b1dfd = new EventConnector() {InstanceName="id_d136475f9a66438a87c0d8c1977b1dfd"}; /* {"IsRoot":false} */
            var id_6aca070ee6bf4492a0bd6a646966c528 = new GreaterThan<int>() {InputOne=default,InputTwo=0,InstanceName="id_6aca070ee6bf4492a0bd6a646966c528"}; /* {"IsRoot":false} */
            var id_81bbaeec7b5b4bc8a8ea23774035d3e0 = new IfElse(defaultCondition:false,executeOnDataflow:false) {InstanceName="id_81bbaeec7b5b4bc8a8ea23774035d3e0"}; /* {"IsRoot":false} */
            var id_2a5458b36d7b42a0910a5206f380a60f = new PopupWindow(title:"No selected sessions") {InstanceName="id_2a5458b36d7b42a0910a5206f380a60f",Width=500,Height=500}; /* {"IsRoot":false} */
            var id_319bda78453643948b2d3a33d288c58f = new Horizontal() {Margin=new Thickness(5),InstanceName="id_319bda78453643948b2d3a33d288c58f"}; /* {"IsRoot":false} */
            var id_813cd9ceb3fb4a12a532feed01390979 = new Horizontal() {Margin=new Thickness(5),InstanceName="id_813cd9ceb3fb4a12a532feed01390979"}; /* {"IsRoot":false} */
            var id_dcf6089271224392bfec8815468c5bae = new Text(text:"Please select at least one session to delete.") {InstanceName="id_dcf6089271224392bfec8815468c5bae"}; /* {"IsRoot":false} */
            var id_4e620f498e474e7890d95aadc1c7edb8 = new Button(title:"Close") {InstanceName="id_4e620f498e474e7890d95aadc1c7edb8"}; /* {"IsRoot":false} */
            var id_7169f3ed24b64038882656ad626a8971 = new EventConnector() {InstanceName="id_7169f3ed24b64038882656ad626a8971"}; /* {"IsRoot":false} */
            var id_05a43661c70248fab459d30af3547158 = new EventConnector() {InstanceName="id_05a43661c70248fab459d30af3547158"}; /* {"IsRoot":false} */
            var selectedSessionsRemoveTransact = new Transact() {InstanceName="selectedSessionsRemoveTransact",ClearDestination=true,AutoLoadNextBatch=true}; /* {"IsRoot":true} */
            var id_2e55d4664efb49118dcdd70afef93f07 = new TableDataFlowConnector() {InstanceName="id_2e55d4664efb49118dcdd70afef93f07"}; /* {"IsRoot":false} */
            var id_a63396a48ea04b9a9bdffef9361693bc = new Data<int>() {InstanceName="id_a63396a48ea04b9a9bdffef9361693bc",storedData=-1}; /* {"IsRoot":false} */
            var id_112e5d0dc095444fbf7f0d68c905f902 = new Transact() {InstanceName="id_112e5d0dc095444fbf7f0d68c905f902",ClearDestination=true,AutoLoadNextBatch=true}; /* {"IsRoot":false} */
            var id_355497fa88664d6db83547db4aa3ef70 = new Transact() {InstanceName="id_355497fa88664d6db83547db4aa3ef70",ClearDestination=true,AutoLoadNextBatch=true}; /* {"IsRoot":false} */
            var id_c56ede8d820d4242b1542cf810196011 = new EventConnector() {InstanceName="id_c56ede8d820d4242b1542cf810196011"}; /* {"IsRoot":false} */
            var XRS2DeleteAlerts = new EventConnector() {InstanceName="XRS2DeleteAlerts"}; /* {"IsRoot":true} */
            var id_6e66797b51924dd3a23647be2b2e0ab6 = new PopupWindow(title:"Clear all alerts") {InstanceName="id_6e66797b51924dd3a23647be2b2e0ab6",Width=500,Height=500}; /* {"IsRoot":false} */
            var id_bc12a1de672147d58ae2d2aea63dc36d = new Horizontal() {Margin=new Thickness(5),InstanceName="id_bc12a1de672147d58ae2d2aea63dc36d"}; /* {"IsRoot":false} */
            var id_26c93d2744d7409cade0e0cf42f110dd = new Text(text:"Are you sure you want to delete all alerts off your device?") {InstanceName="id_26c93d2744d7409cade0e0cf42f110dd"}; /* {"IsRoot":false} */
            var id_e0165e2b44834b50b5c5c4dc3006f4f9 = new Horizontal() {InstanceName="id_e0165e2b44834b50b5c5c4dc3006f4f9"}; /* {"IsRoot":false} */
            var id_f4c1df4913d1474e80ee4278caade871 = new RightJustify() {InstanceName="id_f4c1df4913d1474e80ee4278caade871"}; /* {"IsRoot":false} */
            var id_f8c34be2883a4ae9b4441e4e7d46a7a1 = new Button(title:"Yes") {Width=100,Height=35,Margin=new Thickness(5),InstanceName="id_f8c34be2883a4ae9b4441e4e7d46a7a1"}; /* {"IsRoot":false} */
            var id_0d409290252f4a3fb52f61b96509809e = new Button(title:"Cancel") {Width=100,Height=35,Margin=new Thickness(5),InstanceName="id_0d409290252f4a3fb52f61b96509809e"}; /* {"IsRoot":false} */
            var id_f920e30d2814465db92841666335f9f1 = new EventConnector() {InstanceName="id_f920e30d2814465db92841666335f9f1"}; /* {"IsRoot":false} */
            var id_c0607234d86748b8b9ec7acff4b86bf2 = new AlertDeleteSCP() {InstanceName="id_c0607234d86748b8b9ec7acff4b86bf2"}; /* {"IsRoot":false} */
            var id_13a400afca4b4d3f96c04028d8879293 = new PopupWindow(title:"Successfully cleared all alerts") {InstanceName="id_13a400afca4b4d3f96c04028d8879293",Width=500,Height=500}; /* {"IsRoot":false} */
            var id_3d3805dc630e4f99b55ad351a7a6f6bd = new Horizontal() {Margin=new Thickness(5),InstanceName="id_3d3805dc630e4f99b55ad351a7a6f6bd"}; /* {"IsRoot":false} */
            var id_a5406d0cdeff4ab18b71efe7de537a46 = new Text(text:"Successfully cleared all alerts off your device.") {InstanceName="id_a5406d0cdeff4ab18b71efe7de537a46"}; /* {"IsRoot":false} */
            var id_e5ff6b01c6754064a7acc3cdd747c5e6 = new Horizontal() {Margin=new Thickness(5),InstanceName="id_e5ff6b01c6754064a7acc3cdd747c5e6"}; /* {"IsRoot":false} */
            var id_177d155675b84070b7077ed353b9e0a8 = new Button(title:"Close") {InstanceName="id_177d155675b84070b7077ed353b9e0a8"}; /* {"IsRoot":false} */
            var id_0b304d131bf6460fa2577ed5ceb0268e = new PopupWindow(title:"Failed clearing all alerts") {InstanceName="id_0b304d131bf6460fa2577ed5ceb0268e",Width=500,Height=500}; /* {"IsRoot":false} */
            var id_64c8aeb3ae3642a48cd2d2dac478b836 = new Horizontal() {Margin=new Thickness(5),InstanceName="id_64c8aeb3ae3642a48cd2d2dac478b836"}; /* {"IsRoot":false} */
            var id_83e76fec24b248c0999f8eee149b3898 = new Horizontal() {Margin=new Thickness(5),InstanceName="id_83e76fec24b248c0999f8eee149b3898"}; /* {"IsRoot":false} */
            var id_14a068d0ee1247c6bde4fd313cbe664c = new Text(text:"There was an error clearing all alerts off your device. Please try again or contact Tru-Test customer support.",wrap:true) {InstanceName="id_14a068d0ee1247c6bde4fd313cbe664c"}; /* {"IsRoot":false} */
            var id_18b11b35e9944402a8da0979c60b7ac5 = new Button(title:"Close") {InstanceName="id_18b11b35e9944402a8da0979c60b7ac5"}; /* {"IsRoot":false} */
            var id_ad34acf12b57488b9793866f20c52269 = new WizardItem(contentText:default) {ContentText="Selected sessions",ImageName="Icon_Session_Delete.png",Checked=true,InstanceName="id_ad34acf12b57488b9793866f20c52269"}; /* {"IsRoot":false} */
            var id_acc9a2d3912749fd813d3736040d86d6 = new WizardItem(contentText:default) {ContentText="All information on device (sessions and cross references)",ImageName="Icon_ClearStickReader.png",InstanceName="id_acc9a2d3912749fd813d3736040d86d6"}; /* {"IsRoot":false} */
            var id_199db356282348de832495a04d6ca839 = new WizardItem() {ContentText="All alerts on device",ImageName="Icon_Alert_Delete.png",Visible=false,InstanceName="id_199db356282348de832495a04d6ca839"}; /* {"IsRoot":false} */
            var id_8a24f36fe4a340d391a4205207293642 = new Data<string>() {InstanceName="id_8a24f36fe4a340d391a4205207293642",storedData="0"}; /* {"IsRoot":false} */
            var id_72a41eed63e8409a9053392441c2792f = new EventConnector() {InstanceName="id_72a41eed63e8409a9053392441c2792f"}; /* {"IsRoot":false} */
            var id_c95839c10b874b05acc09e28bab9eadf = new Data<string>() {InstanceName="id_c95839c10b874b05acc09e28bab9eadf",storedData="0"}; /* {"IsRoot":false} */
            var id_e8e6abe9b4674f25bca071440c0be6a8 = new Transact() {InstanceName="id_e8e6abe9b4674f25bca071440c0be6a8",ClearDestination=true,AutoLoadNextBatch=true}; /* {"IsRoot":false} */
            var id_1208c8e407e543d78662f15070f2bd62 = new Transact() {InstanceName="id_1208c8e407e543d78662f15070f2bd62",ClearDestination=true,AutoLoadNextBatch=true}; /* {"IsRoot":false} */
            var id_ba6d5278cb09412ab8aae27671ae94ae = new Transact() {InstanceName="id_ba6d5278cb09412ab8aae27671ae94ae",ClearDestination=true,AutoLoadNextBatch=true}; /* {"IsRoot":false} */
            var id_2895bf95b5b043d695b7c3d287e6d381 = new LifeDataDeleteSCP() {InstanceName="id_2895bf95b5b043d695b7c3d287e6d381"}; /* {"IsRoot":false} */
            var id_5f3eab9aeec94326bd95c9d0a42a992e = new Transact() {InstanceName="id_5f3eab9aeec94326bd95c9d0a42a992e",ClearDestination=true,AutoLoadNextBatch=true}; /* {"IsRoot":false} */
            var id_b3f079ac06c940709312f2fbf7235ace = new Switch<string, string>() {InstanceName="id_b3f079ac06c940709312f2fbf7235ace",compareValues=new string[]{"SRS2", "XRS2"}}; /* {"IsRoot":false} */
            var id_5e42eed1a5e446e299df0731a1020c71 = new Data<string>() {InstanceName="id_5e42eed1a5e446e299df0731a1020c71",storedData="All information on device (sessions and cross references)"}; /* {"IsRoot":true} */
            var id_45ceacd53ddd44ceb79ae8d5b8f2c817 = new Data<string>() {InstanceName="id_45ceacd53ddd44ceb79ae8d5b8f2c817",storedData="All information on device (sessions, alerts and cross references)"}; /* {"IsRoot":true} */
            var id_7349f43f57ae4f14a0041a3ffc64995e = new ClearTable() {}; /* {"IsRoot":false} */
            // END AUTO-GENERATED INSTANTIATIONS FOR DeleteSessions_XRS2_SRS2
            #endregion

            #region DeleteSessions_Scale5000 INSTANTIATIONS
            // BEGIN AUTO-GENERATED INSTANTIATIONS FOR DeleteSessions_Scale5000
            var Scale5000DeleteSessions = new Wizard(title:"Delete information off device") {SecondTitle="What information do you want to delete off the device?",InstanceName="Scale5000DeleteSessions"}; /* {"IsRoot":true} */
            var id_452cd410b68249898c9a38d8090d75bb = new WizardItem() {ContentText="Selected sessions",ImageName="Icon_Session_Delete.png",Checked=true,InstanceName="id_452cd410b68249898c9a38d8090d75bb"}; /* {"IsRoot":false} */
            var id_cdabf62c83304b42831f31db68d86cdb = new WizardItem() {ContentText="All information on device\n(all session files and animal lifetime information)",ImageName="Icon_Clear3000.png",Checked=default,InstanceName="id_cdabf62c83304b42831f31db68d86cdb"}; /* {"IsRoot":false} */
            var id_f9502c5e92d24728870748a784b8f8d7 = new WizardItem() {ContentText="All animals in selected sessions",ImageName="Icon_DeleteSessionAndAnimal.png",InstanceName="id_f9502c5e92d24728870748a784b8f8d7"}; /* {"IsRoot":false} */
            var Scale5000DeleteSelectedSessions = new EventConnector() {InstanceName="Scale5000DeleteSelectedSessions"}; /* {"IsRoot":true} */
            var Scale5000DeleteAllSessions = new EventConnector() {InstanceName="Scale5000DeleteAllSessions"}; /* {"IsRoot":true} */
            var Scale5000DeleteAnimalsInSesion = new EventConnector() {InstanceName="Scale5000DeleteAnimalsInSesion"}; /* {"IsRoot":true} */
            var id_7e593a1e387f48ec9eef520448218fe6 = new Transact() {InstanceName="id_7e593a1e387f48ec9eef520448218fe6",ClearDestination=true,AutoLoadNextBatch=true}; /* {"IsRoot":false} */
            var id_245f98c7072442b7aa71b681f5d00062 = new TableDataFlowConnector() {InstanceName="id_245f98c7072442b7aa71b681f5d00062"}; /* {"IsRoot":false} */
            var id_8877f6deea8b404db28ac84f1c82cf84 = new Filter() {InstanceName="id_8877f6deea8b404db28ac84f1c82cf84",FilterDelegate=(DataRow r) =>{    string check = r["checkbox"].ToString();    if (check == "0")        return false;    if (check == "1")        return true;    return bool.Parse(check);}}; /* {"IsRoot":false} */
            var id_6ef1a7ff90204dd2a06dcd7eb1f7fceb = new DataFlowConnector<DataTable>() {InstanceName="id_6ef1a7ff90204dd2a06dcd7eb1f7fceb"}; /* {"IsRoot":false} */
            var id_59d757678fe8437ca54b51662a37be5a = new Apply<DataTable, int>(latchData:default) {InstanceName="id_59d757678fe8437ca54b51662a37be5a",Lambda=(DataTable table) =>{    return table.Rows.Count;}}; /* {"IsRoot":false} */
            var id_da940a49c596462fa0b172a83d433227 = new IfElse(defaultCondition:false,executeOnDataflow:false) {InstanceName="id_da940a49c596462fa0b172a83d433227"}; /* {"IsRoot":false} */
            var id_4f0605cede024da783149f93cee20caa = new GreaterThan<int>() {InputTwo=0,InstanceName="id_4f0605cede024da783149f93cee20caa"}; /* {"IsRoot":false} */
            var id_df92778da4a047b3813805808354533e = new Apply<int, string>() {InstanceName="id_df92778da4a047b3813805808354533e",Lambda=(i) => $"Are you sure you want to delete {i} session(s)?"}; /* {"IsRoot":false} */
            var id_63d67d8001274b9c90329e0f7841a8d6 = new PopupWindow(title:"No selected sessions") {InstanceName="id_63d67d8001274b9c90329e0f7841a8d6",Width=500,Height=500}; /* {"IsRoot":false} */
            var id_a257e73b8a3c47e9925d6521be00a55d = new Text(text:"Please select at least one session to delete.") {InstanceName="id_a257e73b8a3c47e9925d6521be00a55d",Margin=new Thickness(5)}; /* {"IsRoot":false} */
            var id_9ed1b051b79841909cc93a4b0523260f = new Button(title:"Close") {Margin=new Thickness(5),InstanceName="id_9ed1b051b79841909cc93a4b0523260f"}; /* {"IsRoot":false} */
            var id_b755a40bf8cf48b8b712391e4f0da6bc = new PopupWindow(title:"Delete selected sessions") {InstanceName="id_b755a40bf8cf48b8b712391e4f0da6bc",Width=500,Height=500}; /* {"IsRoot":false} */
            var id_45888abd3d0946fca32ebdacc087bccd = new Text() {InstanceName="id_45888abd3d0946fca32ebdacc087bccd",Margin=new Thickness(5)}; /* {"IsRoot":false} */
            var id_f7971d205f764b0f9cf7afc6d5a231be = new RightJustify() {InstanceName="id_f7971d205f764b0f9cf7afc6d5a231be"}; /* {"IsRoot":false} */
            var id_887f3b39d6dc4347a8155ec7ff07383e = new Button(title:"Yes") {Width=100,Height=35,Margin=new Thickness(5),InstanceName="id_887f3b39d6dc4347a8155ec7ff07383e"}; /* {"IsRoot":false} */
            var id_ec69ddaab61a46c6b7e63dbbb2cac700 = new Button(title:"Cancel") {Width=100,Height=35,Margin=new Thickness(5),InstanceName="id_ec69ddaab61a46c6b7e63dbbb2cac700"}; /* {"IsRoot":false} */
            var id_d2ada736889c41d5a1d7e26e23018a61 = new SessionDeleteQuery(deleteAll:false) {InstanceName="id_d2ada736889c41d5a1d7e26e23018a61"}; /* {"IsRoot":false} */
            var id_e49d83414cf64a579de25bd2885778ef = new EventConnector() {InstanceName="id_e49d83414cf64a579de25bd2885778ef"}; /* {"IsRoot":false} */
            var id_9fce0aa410dc42bb8d7f69e5343c539e = new EventConnector() {InstanceName="id_9fce0aa410dc42bb8d7f69e5343c539e"}; /* {"IsRoot":false} */
            var id_482e9408b3fd4a6e9bc4c228ca2fb1d9 = new Transact() {InstanceName="id_482e9408b3fd4a6e9bc4c228ca2fb1d9",ClearDestination=true,AutoLoadNextBatch=true}; /* {"IsRoot":false} */
            var id_a0babac309174795ae32cdf5beaeee75 = new PopupWindow(title:"Successfully deleted sessions") {InstanceName="id_a0babac309174795ae32cdf5beaeee75",Width=500,Height=500}; /* {"IsRoot":false} */
            var id_893461027de245aea1b73581680a8c27 = new Text(text:"Successfully deleted the sessions and animal data off your device.",wrap:true) {InstanceName="id_893461027de245aea1b73581680a8c27",Margin=new Thickness(5)}; /* {"IsRoot":false} */
            var id_dff2dc973a9a4bee8c4bde410f8167c2 = new Button(title:"Close") {Margin=new Thickness(5),InstanceName="id_dff2dc973a9a4bee8c4bde410f8167c2"}; /* {"IsRoot":false} */
            var id_7da7a71793e44a63807bd8f2028e0718 = new PopupWindow(title:"Error deleting sessions") {InstanceName="id_7da7a71793e44a63807bd8f2028e0718",Width=500,Height=500}; /* {"IsRoot":false} */
            var id_31dc680a9e884d2696bf4b97f953fbd5 = new Text(text:"There was an error deleting sessions and animal data. Please try again or contact Tru-Test customer support.",wrap:true) {InstanceName="id_31dc680a9e884d2696bf4b97f953fbd5",Margin=new Thickness(5)}; /* {"IsRoot":false} */
            var id_5ce7b528b8cc4586924b17606cb9a449 = new Button(title:"Close") {Margin=new Thickness(5),InstanceName="id_5ce7b528b8cc4586924b17606cb9a449"}; /* {"IsRoot":false} */
            var id_7bdd96ea8be0487281bae9f7fa04e945 = new Transact() {InstanceName="id_7bdd96ea8be0487281bae9f7fa04e945",ClearDestination=true,AutoLoadNextBatch=true}; /* {"IsRoot":false} */
            var id_ba3e627f147f4f63bd4fa491ef4013ce = new PopupWindow(title:"Delete all sessions") {InstanceName="id_ba3e627f147f4f63bd4fa491ef4013ce",Width=500}; /* {"IsRoot":false} */
            var id_00b9806a4d1141beaf2b5250bfff2838 = new Text(text:"Are you sure you want to delete all sessions and animal lifetime information off your device?",wrap:true) {InstanceName="id_00b9806a4d1141beaf2b5250bfff2838",Margin=new Thickness(5)}; /* {"IsRoot":false} */
            var id_e2b7d29c2aa64d578f7544870a2963fc = new RightJustify() {InstanceName="id_e2b7d29c2aa64d578f7544870a2963fc"}; /* {"IsRoot":false} */
            var id_bcb9738028d944d38a8d786b6c741155 = new Button(title:"Yes") {Width=100,Height=35,Margin=new Thickness(5),InstanceName="id_bcb9738028d944d38a8d786b6c741155"}; /* {"IsRoot":false} */
            var id_c87d65ad50ac4ac6bfd72ac7b8a22464 = new Button(title:"Cancel") {Width=100,Height=35,Margin=new Thickness(5),InstanceName="id_c87d65ad50ac4ac6bfd72ac7b8a22464"}; /* {"IsRoot":false} */
            var id_61c7d695c4164f15ae887d50b04771ad = new EventConnector() {InstanceName="id_61c7d695c4164f15ae887d50b04771ad"}; /* {"IsRoot":false} */
            var id_bf2bf99800ae46c2a787ab888db762cf = new SessionDeleteQuery(deleteAll:true) {InstanceName="id_bf2bf99800ae46c2a787ab888db762cf"}; /* {"IsRoot":false} */
            var id_945a98d708ff4442bd44dada16462256 = new EventConnector() {InstanceName="id_945a98d708ff4442bd44dada16462256"}; /* {"IsRoot":false} */
            var id_f833a4234daa456ca221ca14cd9c477f = new Transact() {InstanceName="id_f833a4234daa456ca221ca14cd9c477f",ClearDestination=true,AutoLoadNextBatch=true}; /* {"IsRoot":false} */
            var id_5f4dceb4a7d549db8a369ae531ac0ede = new LifeDataDeleteQuery() {InstanceName="id_5f4dceb4a7d549db8a369ae531ac0ede"}; /* {"IsRoot":false} */
            var id_2c3aa0324aeb4bf497b8aebb30416cf7 = new EventConnector() {InstanceName="id_2c3aa0324aeb4bf497b8aebb30416cf7"}; /* {"IsRoot":false} */
            var id_8b01316204af48b794ccbae9d69dd91a = new Transact() {InstanceName="id_8b01316204af48b794ccbae9d69dd91a",ClearDestination=true,AutoLoadNextBatch=true}; /* {"IsRoot":false} */
            var id_c0dbb58f39c14bc1bb30313e9cfdf42a = new Transact() {InstanceName="id_c0dbb58f39c14bc1bb30313e9cfdf42a",ClearDestination=true,AutoLoadNextBatch=true}; /* {"IsRoot":false} */
            var id_07294915e06f40488cd68107d8a1d274 = new Transact() {InstanceName="id_07294915e06f40488cd68107d8a1d274",ClearDestination=true,AutoLoadNextBatch=true}; /* {"IsRoot":false} */
            var id_9b06445643aa4ca1974a1fb9a442c16c = new TableDataFlowConnector() {InstanceName="id_9b06445643aa4ca1974a1fb9a442c16c"}; /* {"IsRoot":false} */
            var id_18a02f8deb2345348b49e59b58c9136d = new Filter() {InstanceName="id_18a02f8deb2345348b49e59b58c9136d",FilterDelegate=(DataRow r) =>{    string check = r["checkbox"].ToString();    if (check == "0")        return false;    if (check == "1")        return true;    return bool.Parse(check);}}; /* {"IsRoot":false} */
            var id_c02f762bfa2645e1bb8636f3d8be0e90 = new DataFlowConnector<DataTable>() {InstanceName="id_c02f762bfa2645e1bb8636f3d8be0e90"}; /* {"IsRoot":false} */
            var id_7b5646861c0d42cc9e95ed4c23cd7b9e = new Apply<DataTable, int>() {InstanceName="id_7b5646861c0d42cc9e95ed4c23cd7b9e",Lambda=(DataTable table) =>{    return table.Rows.Count;}}; /* {"IsRoot":false} */
            var id_8e8408cd330243e78963f04ae4c59663 = new GreaterThan<int>() {InputTwo=0,InstanceName="id_8e8408cd330243e78963f04ae4c59663"}; /* {"IsRoot":false} */
            var id_adc7cb23cac64e26882a0fde31532fb0 = new IfElse(defaultCondition:false,executeOnDataflow:false) {InstanceName="id_adc7cb23cac64e26882a0fde31532fb0"}; /* {"IsRoot":false} */
            var id_b32d386e15804d5486513738ddc6d381 = new PopupWindow(title:"Delete animal data from session") {InstanceName="id_b32d386e15804d5486513738ddc6d381",Width=500,Height=default}; /* {"IsRoot":false} */
            var id_80f7974b4de04ff4b668e26b1399b75e = new Text() {InstanceName="id_80f7974b4de04ff4b668e26b1399b75e",Margin=new Thickness(5)}; /* {"IsRoot":false} */
            var id_37e080a10e0f4b208894b41eb507c5f5 = new RightJustify() {InstanceName="id_37e080a10e0f4b208894b41eb507c5f5"}; /* {"IsRoot":false} */
            var id_4674a092953845ab97e43d46909bca70 = new Button(title:"Yes") {Width=100,Height=35,Margin=new Thickness(5),InstanceName="id_4674a092953845ab97e43d46909bca70"}; /* {"IsRoot":false} */
            var id_63c90c7440d448f894d9b46fa6a695bc = new Button(title:"Cancel") {Width=100,Height=35,Margin=new Thickness(5),InstanceName="id_63c90c7440d448f894d9b46fa6a695bc"}; /* {"IsRoot":false} */
            var id_b8b58b54f72942deb892801d072c79cd = new Apply<int, string>(latchData:default) {InstanceName="id_b8b58b54f72942deb892801d072c79cd",Lambda=(i) => $"Are you sure you want to delete the animal data from {i} session(s)?"}; /* {"IsRoot":false} */
            var id_1b38d526b9434409b3bcc8e8353aa456 = new PopupWindow(title:"Delete animal data from session") {InstanceName="id_1b38d526b9434409b3bcc8e8353aa456",Width=500}; /* {"IsRoot":false} */
            var id_f9f31de8fd5f427796da7edb235551cf = new Text(text:"Please select at least one session to delete animal data from.") {InstanceName="id_f9f31de8fd5f427796da7edb235551cf",Margin=new Thickness(5)}; /* {"IsRoot":false} */
            var id_71a22611bcae461db6cc0e7a0fd6b47c = new Button(title:"Close") {Margin=new Thickness(5),InstanceName="id_71a22611bcae461db6cc0e7a0fd6b47c"}; /* {"IsRoot":false} */
            var id_a0c10e35eb904703ab5efa42de1ad66d = new EventConnector() {InstanceName="id_a0c10e35eb904703ab5efa42de1ad66d"}; /* {"IsRoot":false} */
            var id_4fee89bab42045aa88486ea9f65ee3aa = new SessionDataDeleteQuery() {}; /* {"IsRoot":false} */
            var id_5ec709fbd7dc48daa896c73d9c4dbab2 = new Transact() {ClearDestination=true,AutoLoadNextBatch=true}; /* {"IsRoot":false} */
            var id_c74e418370384cefb728c0128c54c885 = new EventConnector() {}; /* {"IsRoot":false} */
            var id_69a132bec2a540798f5192ab4bec6fb2 = new PopupWindow(title:"Successfully deleted animal data") {Width=500}; /* {"IsRoot":false} */
            var id_0ec5ba53a8d44676a6f745d8a4af84b5 = new Text(text:"Successfully deleted animal data from the selected sessions.") {Margin=new Thickness(5)}; /* {"IsRoot":false} */
            var id_f8f451bf51b242378f6eb46dd24d75d9 = new Button(title:"Close") {Margin=new Thickness(5)}; /* {"IsRoot":false} */
            var id_767277e46f72429c808ac2a746a8e328 = new PopupWindow(title:"Error deleting animal data") {Width=500}; /* {"IsRoot":false} */
            var id_5140e98ae93d4a06ba659d476e1d8f63 = new Text(text:"There was an error deleting animal data from the selected sessions. Please try again or contact Tru-Test customer support.",wrap:true) {Margin=new Thickness(5)}; /* {"IsRoot":false} */
            var id_7af80d5722b847688de777730b0781c5 = new Button(title:"Close") {Margin=new Thickness(5)}; /* {"IsRoot":false} */
            var id_e5e6fb2059f549d3a5dd80e2a70f6ef3 = new Data<string>() {storedData="0"}; /* {"IsRoot":false} */
            // END AUTO-GENERATED INSTANTIATIONS FOR DeleteSessions_Scale5000
            #endregion

            #endregion

            /// ------------------------------------------------------------------------------------------------------------------

            #region parser-generated wiring

            #region WebServices WIRING
            // BEGIN AUTO-GENERATED WIRING FOR WebServices
            webServicesSubroutines.WireTo(SUBROUTINE_checkInternetConnectivity, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            webServicesSubroutines.WireTo(SUBROUTINE_checkInternetConnectivity, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            webServicesSubroutines.WireTo(id_5fb6256e873441bc875865b02d5e15d0, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            webServicesSubroutines.WireTo(SUBROUTINE_MiHubLogin, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            SUBROUTINE_checkInternetConnectivity.WireTo(checkInternetConnection, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"HttpRequest","DestinationIsReference":false} */
            checkInternetConnection.WireTo(internetConnectionDetectedConnector, "hasConnection"); /* {"SourceType":"HttpRequest","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_5fb6256e873441bc875865b02d5e15d0.WireTo(internetConnectionDetectedConnector, "inputDataB"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_5fb6256e873441bc875865b02d5e15d0.WireTo(id_9a3dce085ff64d0c96197c0a6e578b93, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"IfElse","DestinationIsReference":false} */
            SUBROUTINE_MiHubLogin.WireTo(MiHubCheckIfRequireLogin, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            MiHubRequireLogin.WireTo(mihubLoginWindow, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            mihubEventConnector.WireTo(MiHubCheckIfRequireLogin, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            mihubEventConnector.WireTo(id_4glcvtjtrebtrihhqfo7sjj044, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"GetJSONDict","DestinationIsReference":false} */
            MiHubCheckIfRequireLogin.WireTo(id_11e8237467ec49e6a6a7438511acc42e, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_11e8237467ec49e6a6a7438511acc42e.WireTo(MiHubStoredUserinfoData, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"DataFromPath","DestinationIsReference":false} */
            MiHubStoredUserinfoData.WireTo(id_34f649e132444d7c950b42d5079234c0, "dataOutput"); /* {"SourceType":"DataFromPath","SourceIsReference":false,"DestinationType":"Equals","DestinationIsReference":false} */
            id_34f649e132444d7c950b42d5079234c0.WireTo(id_2ef417ff26cb4accb54bb2202fd282d8, "isEqual"); /* {"SourceType":"Equals","SourceIsReference":false,"DestinationType":"IfElse","DestinationIsReference":false} */
            id_2ef417ff26cb4accb54bb2202fd282d8.WireTo(id_f8eb03e5807c4f4caa8a87531936fa0d, "ifOutput"); /* {"SourceType":"IfElse","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_2ef417ff26cb4accb54bb2202fd282d8.WireTo(id_8f768d6ed7c140949a3138a729589ae3, "elseOutput"); /* {"SourceType":"IfElse","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_f8eb03e5807c4f4caa8a87531936fa0d.WireTo(MiHubRequireLogin, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            MiHubRequireLogin.WireTo(mihubLoginWindow, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            mihubLoginWindow.WireTo(mihubLoginForm, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"LoginForm","DestinationIsReference":false} */
            mihubLoginForm.WireTo(id_2o033hb453l58815b7isadd9kp, "children"); /* {"SourceType":"LoginForm","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            mihubLoginForm.WireTo(mihubLoginFormVertical, "children"); /* {"SourceType":"LoginForm","SourceIsReference":false,"DestinationType":"Vertical","DestinationIsReference":false} */
            mihubLoginFormVertical.WireTo(mihubUsernameForm, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            mihubLoginFormVertical.WireTo(mihubPasswordForm, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            mihubLoginFormVertical.WireTo(mihubRevealedPasswordForm, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            mihubLoginFormVertical.WireTo(mihubCheckboxHoriz, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            mihubLoginFormVertical.WireTo(mihubLoginSubmitAndForgotPassHoriz, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            mihubLoginFormVertical.WireTo(mihubRegisterButtonHoriz, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            mihubLoginFormVertical.WireTo(mihubStatusMessageHoriz, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            mihubUsernameForm.WireTo(id_15kmql52nb5r59127gughn33np, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            mihubUsernameForm.WireTo(mihubUsernameInput, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            mihubUsernameInput.WireTo(MiHubPasswordResetEmailTextBox, "textOutput"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            mihubPasswordForm.WireTo(id_39bda7lmkss6ee12kvbmdj0tcs, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            mihubPasswordForm.WireTo(id_f2966e850a5e4508a0e06c3fea49c18b, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_f2966e850a5e4508a0e06c3fea49c18b.WireTo(mihubPasswordInput, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"PasswordBox","DestinationIsReference":false} */
            id_14f3d515b9904a689f088337a0b96ca1.WireTo(mihubPasswordInput, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PasswordBox","DestinationIsReference":false} */
            mihubPasswordInput.WireTo(id_8fab8fe267d941049387109f8dcd0dba, "textOutput"); /* {"SourceType":"PasswordBox","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_14f3d515b9904a689f088337a0b96ca1.WireTo(id_7d9b0dc90fc5428b93770ffa0d75936f, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            id_8fab8fe267d941049387109f8dcd0dba.WireTo(id_90563c01d56a4a598debfa095e607cf3, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"DataFlowGate","DestinationIsReference":false} */
            id_90563c01d56a4a598debfa095e607cf3.WireTo(id_6e5f96cd453b4895a693d449fc73ee97, "dataOutput"); /* {"SourceType":"DataFlowGate","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_90563c01d56a4a598debfa095e607cf3.WireTo(id_82c68ffb6440414bb22a9f88ff668e4e, "triggerLatchInput"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"DataFlowGate","DestinationIsReference":false} */
            mihubRevealedPasswordForm.WireTo(id_363297dfa7914702b8cfadabb9bb0d2c, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            mihubRevealedPasswordForm.WireTo(id_7d9b0dc90fc5428b93770ffa0d75936f, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            id_7d9b0dc90fc5428b93770ffa0d75936f.WireTo(id_348262b7e41345779351390fc8578212, "textOutput"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_348262b7e41345779351390fc8578212.WireTo(id_f3ec0073daa24f7f953b58c30559776c, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"DataFlowGate","DestinationIsReference":false} */
            id_f3ec0073daa24f7f953b58c30559776c.WireTo(id_53a4c589151c47c89047f71b5c6538cb, "dataOutput"); /* {"SourceType":"DataFlowGate","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_f3ec0073daa24f7f953b58c30559776c.WireTo(id_0292522dc1b7445eb877bf981f255168, "triggerLatchInput"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"DataFlowGate","DestinationIsReference":false} */
            id_53a4c589151c47c89047f71b5c6538cb.WireTo(id_6e5f96cd453b4895a693d449fc73ee97, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            mihubCheckboxHoriz.WireTo(id_9c5999cabf3a45f5b9b7dcf7328d2e16, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"CheckBox","DestinationIsReference":false} */
            mihubCheckboxHoriz.WireTo(id_4lhahk5tlaquvln6kiat712bm5, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"CheckBox","DestinationIsReference":false} */
            id_9c5999cabf3a45f5b9b7dcf7328d2e16.WireTo(id_0292522dc1b7445eb877bf981f255168, "isChecked"); /* {"SourceType":"CheckBox","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_0292522dc1b7445eb877bf981f255168.WireTo(id_ec977563cf2e4bf8b01beca7e6596239, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Not","DestinationIsReference":false} */
            id_0292522dc1b7445eb877bf981f255168.WireTo(id_ffbbd9e4ba6e4d8cb9cff53774318346, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"IfElse","DestinationIsReference":false} */
            id_0292522dc1b7445eb877bf981f255168.WireTo(mihubRevealedPasswordForm, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_ec977563cf2e4bf8b01beca7e6596239.WireTo(id_82c68ffb6440414bb22a9f88ff668e4e, "reversedInput"); /* {"SourceType":"Not","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_82c68ffb6440414bb22a9f88ff668e4e.WireTo(mihubPasswordForm, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_ffbbd9e4ba6e4d8cb9cff53774318346.WireTo(id_5018714ff6b14b1dbd173a97124da854, "ifOutput"); /* {"SourceType":"IfElse","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_ffbbd9e4ba6e4d8cb9cff53774318346.WireTo(id_14a577073c4d496397830c92a1aaf1c6, "elseOutput"); /* {"SourceType":"IfElse","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_5018714ff6b14b1dbd173a97124da854.WireTo(id_8fab8fe267d941049387109f8dcd0dba, "inputDataB"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_5018714ff6b14b1dbd173a97124da854.WireTo(id_7d9b0dc90fc5428b93770ffa0d75936f, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            id_14a577073c4d496397830c92a1aaf1c6.WireTo(id_348262b7e41345779351390fc8578212, "inputDataB"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_14a577073c4d496397830c92a1aaf1c6.WireTo(mihubPasswordInput, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"PasswordBox","DestinationIsReference":false} */
            id_4lhahk5tlaquvln6kiat712bm5.WireTo(id_7a8ttb8o1del67sriht4a3e52t, "isChecked"); /* {"SourceType":"CheckBox","SourceIsReference":false,"DestinationType":"Not","DestinationIsReference":false} */
            id_7a8ttb8o1del67sriht4a3e52t.WireTo(MiHubLoginCheckBoxNotCheckedConnector, "reversedInput"); /* {"SourceType":"Not","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            MiHubLoginCheckBoxNotCheckedConnector.WireTo(id_59ina9kj9f67rdrphpt4q9q3sr, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"EventGate","DestinationIsReference":false} */
            mihubLoginSubmitAndForgotPassHoriz.WireTo(mihubSubmitButton, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            mihubLoginSubmitAndForgotPassHoriz.WireTo(id_5sk780357aunm4tprle9ni2j8l, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            mihubSubmitButton.WireTo(id_a6fcb0583533433a94686c80c1fca1c3, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_a6fcb0583533433a94686c80c1fca1c3.WireTo(MiHubLoginEventsBeforeRequest, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_a6fcb0583533433a94686c80c1fca1c3.WireTo(MiHubLoginWithUserPass, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"HttpRequest","DestinationIsReference":false} */
            MiHubLoginEventsBeforeRequest.WireTo(mihubLoginWindow, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            MiHubLoginEventsBeforeRequest.WireTo(MiHubWaitingWindow, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            MiHubLoginWithUserPass.WireTo(mihubUsernameInput, "username"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"HttpRequest","DestinationIsReference":false} */
            MiHubLoginWithUserPass.WireTo(id_6e5f96cd453b4895a693d449fc73ee97, "password"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"HttpRequest","DestinationIsReference":false} */
            MiHubLoginWithUserPass.WireTo(id_a5ed03bac16644fd9dc8d5fa66bac7df, "statusCodeOutput"); /* {"SourceType":"HttpRequest","SourceIsReference":false,"DestinationType":"Equals","DestinationIsReference":false} */
            MiHubLoginWithUserPass.WireTo(id_ae924356f83343afb2873a8c72b6e128, "responseJsonOutput"); /* {"SourceType":"HttpRequest","SourceIsReference":false,"DestinationType":"JSONParser","DestinationIsReference":false} */
            MiHubLoginWithUserPass.WireTo(storeMiHubTokensLocally, "contentOutput"); /* {"SourceType":"HttpRequest","SourceIsReference":false,"DestinationType":"JSONWriter","DestinationIsReference":false} */
            MiHubLoginWithUserPass.WireTo(id_0sdmuk24l36e6nbriv3m9t60vb, "taskComplete"); /* {"SourceType":"HttpRequest","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_a5ed03bac16644fd9dc8d5fa66bac7df.WireTo(id_2df1fb7b1f5745b99e438c3d3ca301bc, "isEqual"); /* {"SourceType":"Equals","SourceIsReference":false,"DestinationType":"Not","DestinationIsReference":false} */
            id_2df1fb7b1f5745b99e438c3d3ca301bc.WireTo(id_dbcfc233fc4943e39e2fa6ae9776fec3, "reversedInput"); /* {"SourceType":"Not","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_dbcfc233fc4943e39e2fa6ae9776fec3.WireTo(id_1544e37f661d4c9995b43caa3ee4b707, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"EventGate","DestinationIsReference":false} */
            id_dbcfc233fc4943e39e2fa6ae9776fec3.WireTo(mihubStatusMessageHoriz, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_ae924356f83343afb2873a8c72b6e128.WireTo(id_96e099d81d7542b8b88a9e659f1a4a61, "parsedOutput"); /* {"SourceType":"JSONParser","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_96e099d81d7542b8b88a9e659f1a4a61.WireTo(id_46f6ca1e4b0845f68dc62df2a610d679, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            id_46f6ca1e4b0845f68dc62df2a610d679.WireTo(id_dbcfc233fc4943e39e2fa6ae9776fec3, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_0sdmuk24l36e6nbriv3m9t60vb.WireTo(id_e45679f11c3547afa5d59774a62460a8, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_0sdmuk24l36e6nbriv3m9t60vb.WireTo(id_59ina9kj9f67rdrphpt4q9q3sr, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventGate","DestinationIsReference":false} */
            id_e45679f11c3547afa5d59774a62460a8.WireTo(id_5cd4589272024031b2fd55cde26fa7b8, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_e45679f11c3547afa5d59774a62460a8.WireTo(id_1544e37f661d4c9995b43caa3ee4b707, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventGate","DestinationIsReference":false} */
            id_5cd4589272024031b2fd55cde26fa7b8.WireTo(MiHubWaitingWindow, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_b82a41836e8b45c89a7efcc29acdb694.WireTo(id_1544e37f661d4c9995b43caa3ee4b707, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"EventGate","DestinationIsReference":false} */
            id_1544e37f661d4c9995b43caa3ee4b707.WireTo(id_6cd3b5e1af494ddf991b79da7486bac7, "eventOutput"); /* {"SourceType":"EventGate","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_6cd3b5e1af494ddf991b79da7486bac7.WireTo(id_d29584803b8447809c77105bd76a3b8b, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_6cd3b5e1af494ddf991b79da7486bac7.WireTo(MiHubLogOut, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_6cd3b5e1af494ddf991b79da7486bac7.WireTo(mihubLoginWindow, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_d29584803b8447809c77105bd76a3b8b.WireTo(id_b82a41836e8b45c89a7efcc29acdb694, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_59ina9kj9f67rdrphpt4q9q3sr.WireTo(MiHubLogOut, "eventOutput"); /* {"SourceType":"EventGate","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            MiHubLogOut.WireTo(mihubUsernameInput, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            MiHubLogOut.WireTo(id_14f3d515b9904a689f088337a0b96ca1, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_5sk780357aunm4tprle9ni2j8l.WireTo(MiHubResetPasswordPopupWindow, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            mihubRegisterButtonHoriz.WireTo(id_7rf5s4d07ldvublva5s9cm65nm, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_7rf5s4d07ldvublva5s9cm65nm.WireTo(MiHubRegisterPopupWindow, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_b89e3416368e4d068ac246117f59752d.WireTo(mihubStatusMessageHoriz, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            mihubStatusMessageHoriz.WireTo(id_02c4efb1335b41b09a6be5d88da16801, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            MiHubCheckIfRequireLogin.WireTo(id_b89e3416368e4d068ac246117f59752d, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_8f768d6ed7c140949a3138a729589ae3.WireTo(id_742658c1120e438fb80394c6126908e7, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"GetJSONDict","DestinationIsReference":false} */
            id_742658c1120e438fb80394c6126908e7.WireTo(id_ab7ae585f8be44ccb53f113f138ff89f, "jsonDictOutput"); /* {"SourceType":"GetJSONDict","SourceIsReference":false,"DestinationType":"KeyValue","DestinationIsReference":false} */
            id_ab7ae585f8be44ccb53f113f138ff89f.WireTo(id_78d33d633d454dc6a862cb1638e6a785, "keyNotFoundOutput"); /* {"SourceType":"KeyValue","SourceIsReference":false,"DestinationType":"IfElse","DestinationIsReference":false} */
            id_78d33d633d454dc6a862cb1638e6a785.WireTo(id_f8eb03e5807c4f4caa8a87531936fa0d, "ifOutput"); /* {"SourceType":"IfElse","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_78d33d633d454dc6a862cb1638e6a785.WireTo(id_e8d36ce5ca2d44b290d6146451af41e2, "elseOutput"); /* {"SourceType":"IfElse","SourceIsReference":false,"DestinationType":"HttpRequest","DestinationIsReference":false} */
            id_e8d36ce5ca2d44b290d6146451af41e2.WireTo(id_dc03f72dd88c43f09a1ee3104a1273ad, "statusCodeOutput"); /* {"SourceType":"HttpRequest","SourceIsReference":false,"DestinationType":"Equals","DestinationIsReference":false} */
            id_dc03f72dd88c43f09a1ee3104a1273ad.WireTo(id_5667e7340a524c50951dbd19b51116f1, "isEqual"); /* {"SourceType":"Equals","SourceIsReference":false,"DestinationType":"IfElse","DestinationIsReference":false} */
            id_5667e7340a524c50951dbd19b51116f1.WireTo(id_f8eb03e5807c4f4caa8a87531936fa0d, "elseOutput"); /* {"SourceType":"IfElse","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_4glcvtjtrebtrihhqfo7sjj044.WireTo(getMiHubTokensFromDict, "jsonDictOutput"); /* {"SourceType":"GetJSONDict","SourceIsReference":false,"DestinationType":"KeyValue","DestinationIsReference":false} */
            getMiHubTokensFromDict.WireTo(MiHubAccessTokenConnector, "valueOutputs"); /* {"SourceType":"KeyValue","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            getMiHubTokensFromDict.WireTo(MiHubRefreshTokenConnector, "valueOutputs"); /* {"SourceType":"KeyValue","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            MiHubRefreshTokenConnector.WireTo(id_40dq28vgo12k037keds1ng3dja, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"ConvertToEvent","DestinationIsReference":false} */
            id_40dq28vgo12k037keds1ng3dja.WireTo(MiHubGetAccessTokenFromRefresh, "eventOutput"); /* {"SourceType":"ConvertToEvent","SourceIsReference":false,"DestinationType":"HttpRequest","DestinationIsReference":false} */
            MiHubGetAccessTokenFromRefresh.WireTo(MiHubRefreshTokenConnector, "refreshToken"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"HttpRequest","DestinationIsReference":false} */
            MiHubGetAccessTokenFromRefresh.WireTo(MiHubTokenDictConnector, "contentOutput"); /* {"SourceType":"HttpRequest","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            MiHubGetAccessTokenFromRefresh.WireTo(MiHubStartUploadConnector, "taskComplete"); /* {"SourceType":"HttpRequest","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            MiHubTokenDictConnector.WireTo(id_28rn2rsmjrmspcm7c08lad73h0, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"KeyValue","DestinationIsReference":false} */
            MiHubTokenDictConnector.WireTo(getMiHubTokensFromDict, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"KeyValue","DestinationIsReference":false} */
            id_28rn2rsmjrmspcm7c08lad73h0.WireTo(MiHubAccessTokenConnector, "valueOutputs"); /* {"SourceType":"KeyValue","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            MiHubStartUploadConnector.WireTo(id_5sk8qgkc7ksfoeg6voa8cgoc7v, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            MiHubStartUploadConnector.WireTo(id_50t2i8olkuc2veg4cianbkpvsp, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            MiHubStartUploadConnector.WireTo(id_119917d9a29a42f0a55ccf6143f7b076, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_5sk8qgkc7ksfoeg6voa8cgoc7v.WireTo(MiHubResultsTable, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Table","DestinationIsReference":false} */
            id_5sk8qgkc7ksfoeg6voa8cgoc7v.WireTo(id_4rifkejvr2umhs9qgt2jbdn5oq, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_5sk8qgkc7ksfoeg6voa8cgoc7v.WireTo(id_0fukarbv7uk94ec3lgmviqgc7e, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_50t2i8olkuc2veg4cianbkpvsp.WireTo(MiHubAddResultsTableHeaders, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventGate","DestinationIsReference":false} */
            id_0nvjhkf3agvr54tq3ho2fd8loo.WireTo(MiHubAddResultsTableHeaders, "reversedInput"); /* {"SourceType":"Not","SourceIsReference":false,"DestinationType":"EventGate","DestinationIsReference":false} */
            MiHubAddResultsTableHeaders.WireTo(MiHubAddResultsTableHeadersConnector, "eventOutput"); /* {"SourceType":"EventGate","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            MiHubResultsHeadersHaveBeenAdded.WireTo(id_0nvjhkf3agvr54tq3ho2fd8loo, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"Not","DestinationIsReference":false} */
            MiHubAddResultsTableHeadersConnector.WireTo(id_727pk5631j8mibmggs23tpeso6, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            MiHubAddResultsTableHeadersConnector.WireTo(id_1udlotvvg1j2v2spsjqui1ih71, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_727pk5631j8mibmggs23tpeso6.WireTo(id_6op7ddi9dvfpbhlobjc5cat8ol, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"Collection","DestinationIsReference":false} */
            id_6op7ddi9dvfpbhlobjc5cat8ol.WireTo(MiHubResultsTable, "listOutput"); /* {"SourceType":"Collection","SourceIsReference":false,"DestinationType":"Table","DestinationIsReference":false} */
            MiHubResultsTable.WireTo(MiHubResultsTableRowAddedConnector, "rowAdded"); /* {"SourceType":"Table","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            MiHubResultsTableRowAddedConnector.WireTo(id_6op7ddi9dvfpbhlobjc5cat8ol, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Collection","DestinationIsReference":false} */
            MiHubResultsTableRowAddedConnector.WireTo(MiHubResultsHeadersHaveBeenAdded, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            MiHubResultsTableRowAddedConnector.WireTo(MiHubLoopThroughFilePaths, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Loop","DestinationIsReference":false} */
            id_1udlotvvg1j2v2spsjqui1ih71.WireTo(id_6op7ddi9dvfpbhlobjc5cat8ol, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"Collection","DestinationIsReference":false} */
            id_119917d9a29a42f0a55ccf6143f7b076.WireTo(id_a9ed7d035c2541da9dbfda67c46bd92e, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_a9ed7d035c2541da9dbfda67c46bd92e.WireTo(id_e2f794c2693048c7b24840310cf07fd8, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_a9ed7d035c2541da9dbfda67c46bd92e.WireTo(id_2603734bc36540b8a73ce5d18cdc749f, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            id_a9ed7d035c2541da9dbfda67c46bd92e.WireTo(id_3757afd20b584b3da2b5647b3fb13bbf, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_2603734bc36540b8a73ce5d18cdc749f.WireTo(id_18beba2589d94712997b45c2833e7e68, "textOutput"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            id_18beba2589d94712997b45c2833e7e68.WireTo(id_594420eb1db24ba4b2332d379da3fb8b, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            MiHubUploadSession.WireTo(id_594420eb1db24ba4b2332d379da3fb8b, "postContent"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"HttpRequest","DestinationIsReference":false} */
            id_3757afd20b584b3da2b5647b3fb13bbf.WireTo(id_3c8d235f430a41cb8037e1eb36bdc5b6, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_3c8d235f430a41cb8037e1eb36bdc5b6.WireTo(id_b258791c1cec4f3c8d400a6ea4c5713c, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_3c8d235f430a41cb8037e1eb36bdc5b6.WireTo(mihubAutoSave, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"AutoSave","DestinationIsReference":false} */
            id_b258791c1cec4f3c8d400a6ea4c5713c.WireTo(id_119917d9a29a42f0a55ccf6143f7b076, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            mihubAutoSave.WireTo(csvAutoSaveFilePathsConnector, "dataFlowOutputFilePath"); /* {"SourceType":"AutoSave","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            mihubAutoSave.WireTo(autoSaveIteratorComplete, "autoSaveHappened"); /* {"SourceType":"AutoSave","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            csvAutoSaveFilePathsConnector.WireTo(MiHubFilePathsForUpload, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Collection","DestinationIsReference":false} */
            MiHubFilePathsForUpload.WireTo(MiHubLoopThroughFilePaths, "listOutput"); /* {"SourceType":"Collection","SourceIsReference":false,"DestinationType":"Loop","DestinationIsReference":false} */
            MiHubLoopThroughFilePaths.WireTo(MiHubSessionFilePathFromLoop, "nextValue"); /* {"SourceType":"Loop","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            MiHubLoopThroughFilePaths.WireTo(MiHubEventsToExecuteForShowingResults, "loopComplete"); /* {"SourceType":"Loop","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            MiHubSessionFilePathFromLoop.WireTo(getFileNameFromPath, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"StringModifier","DestinationIsReference":false} */
            MiHubSessionFilePathFromLoop.WireTo(MiHubSession, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"DataFromPath","DestinationIsReference":false} */
            getFileNameFromPath.WireTo(MiHubFileUploadResultRow, "stringOutput"); /* {"SourceType":"StringModifier","SourceIsReference":false,"DestinationType":"Collection","DestinationIsReference":false} */
            MiHubFileUploadResultRow.WireTo(MiHubResultsTable, "listOutput"); /* {"SourceType":"Collection","SourceIsReference":false,"DestinationType":"Table","DestinationIsReference":false} */
            MiHubSession.WireTo(MiHubUploadSession, "dataOutput"); /* {"SourceType":"DataFromPath","SourceIsReference":false,"DestinationType":"HttpRequest","DestinationIsReference":false} */
            MiHubUploadSession.WireTo(MiHubAccessTokenConnector, "accessToken"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"HttpRequest","DestinationIsReference":false} */
            MiHubUploadSession.WireTo(id_c9d7f71775a3497d8fcdfb78f118d8bc, "responseJsonOutput"); /* {"SourceType":"HttpRequest","SourceIsReference":false,"DestinationType":"JSONParser","DestinationIsReference":false} */
            id_c9d7f71775a3497d8fcdfb78f118d8bc.WireTo(id_40fd5f5747454fc79d44994b4e70932f, "parsedOutput"); /* {"SourceType":"JSONParser","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_40fd5f5747454fc79d44994b4e70932f.WireTo(id_33b397262af848838805e10e29697a09, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"KeyValue","DestinationIsReference":false} */
            id_40fd5f5747454fc79d44994b4e70932f.WireTo(id_d4453910e9d5421aad5118628de55f5e, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"KeyValue","DestinationIsReference":false} */
            id_40fd5f5747454fc79d44994b4e70932f.WireTo(id_d7cc872e4322498c9fba9d1d6a6ff738, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"KeyValue","DestinationIsReference":false} */
            id_33b397262af848838805e10e29697a09.WireTo(id_98cb940aa8ba43fa81fc4638904f019f, "valueOutputs"); /* {"SourceType":"KeyValue","SourceIsReference":false,"DestinationType":"ToString","DestinationIsReference":false} */
            id_98cb940aa8ba43fa81fc4638904f019f.WireTo(MiHubFileUploadErrorMessageGate, "stringOutput"); /* {"SourceType":"ToString","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            MiHubFileUploadErrorMessageGate.WireTo(id_f32ab268257d4db2a9bcf455e1626864, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"StringModifier","DestinationIsReference":false} */
            id_f32ab268257d4db2a9bcf455e1626864.WireTo(MiHubSessionUploadResultMessageConnector, "stringOutput"); /* {"SourceType":"StringModifier","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            MiHubSessionUploadResultMessageConnector.WireTo(id_097c999a3d8a4da5b3ddc3095184ab38, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            MiHubSessionUploadResultMessageConnector.WireTo(MiHubFileUploadResultRow, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Collection","DestinationIsReference":false} */
            id_097c999a3d8a4da5b3ddc3095184ab38.WireTo(id_3kshshqtmev9pnslp39m84p2dd, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"IfElse","DestinationIsReference":false} */
            id_3kshshqtmev9pnslp39m84p2dd.WireTo(MiHubUploadErrorMessageCount, "ifOutput"); /* {"SourceType":"IfElse","SourceIsReference":false,"DestinationType":"Counter","DestinationIsReference":false} */
            id_3kshshqtmev9pnslp39m84p2dd.WireTo(MiHubUploadSuccessMessageCount, "elseOutput"); /* {"SourceType":"IfElse","SourceIsReference":false,"DestinationType":"Counter","DestinationIsReference":false} */
            id_0fukarbv7uk94ec3lgmviqgc7e.WireTo(MiHubUploadErrorMessageCount, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"Counter","DestinationIsReference":false} */
            MiHubUploadResultsFormatted.WireTo(MiHubUploadErrorMessageCount, "dataFlowBsList"); /* {"SourceType":"StringFormat","SourceIsReference":false,"DestinationType":"Counter","DestinationIsReference":false} */
            MiHubUploadResultsFormatted.WireTo(id_417c72s0ir8kes1hjoolugkmjm, "dataFlowOutput"); /* {"SourceType":"StringFormat","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            MiHubUploadResultsFormatted.WireTo(MiHubUploadSuccessMessageCount, "dataFlowBsList"); /* {"SourceType":"StringFormat","SourceIsReference":false,"DestinationType":"Counter","DestinationIsReference":false} */
            id_4rifkejvr2umhs9qgt2jbdn5oq.WireTo(MiHubUploadSuccessMessageCount, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"Counter","DestinationIsReference":false} */
            id_d4453910e9d5421aad5118628de55f5e.WireTo(id_a1cffe13141047feab83daac141f4c65, "valueOutputs"); /* {"SourceType":"KeyValue","SourceIsReference":false,"DestinationType":"ToString","DestinationIsReference":false} */
            id_a1cffe13141047feab83daac141f4c65.WireTo(MiHubFileUploadSuccessMessageGate, "stringOutput"); /* {"SourceType":"ToString","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            MiHubFileUploadSuccessMessageGate.WireTo(id_a8fe59c13b8b46aab423955865e104b5, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"StringModifier","DestinationIsReference":false} */
            id_a8fe59c13b8b46aab423955865e104b5.WireTo(MiHubSessionUploadResultMessageConnector, "stringOutput"); /* {"SourceType":"StringModifier","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_d7cc872e4322498c9fba9d1d6a6ff738.WireTo(id_7584f5dc48014a659a2cdf812a4e1f05, "valueOutputs"); /* {"SourceType":"KeyValue","SourceIsReference":false,"DestinationType":"ToString","DestinationIsReference":false} */
            id_7584f5dc48014a659a2cdf812a4e1f05.WireTo(MiHubFileUploadResponseHasNoError, "stringOutput"); /* {"SourceType":"ToString","SourceIsReference":false,"DestinationType":"Equals","DestinationIsReference":false} */
            MiHubFileUploadResponseHasNoError.WireTo(id_41029b6f4f394eb38cf253283438ec6a, "isEqual"); /* {"SourceType":"Equals","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_41029b6f4f394eb38cf253283438ec6a.WireTo(id_18409fc50fb44553b72b059afcb49cc9, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"IfElse","DestinationIsReference":false} */
            id_18409fc50fb44553b72b059afcb49cc9.WireTo(MiHubFileUploadSuccessMessageGate, "ifOutput"); /* {"SourceType":"IfElse","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_18409fc50fb44553b72b059afcb49cc9.WireTo(MiHubFileUploadErrorMessageGate, "elseOutput"); /* {"SourceType":"IfElse","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            MiHubEventsToExecuteForShowingResults.WireTo(MiHubUploadResultsTransact, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Transact","DestinationIsReference":false} */
            MiHubUploadResultsTransact.WireTo(MiHubResultsTable, "tableDataFlowSource"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"Table","DestinationIsReference":false} */
            MiHubUploadResultsTransact.WireTo(MiHubUploadResultsGrid, "tableDataFlowDestination"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"Grid","DestinationIsReference":false} */
            MiHubUploadResultsTransact.WireTo(MiHubResultsTransactCompleteConnector, "eventCompleteNoErrors"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            MiHubResultsTransactCompleteConnector.WireTo(id_5aaeib7gs6vg67h1ae0hi7hn29, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            MiHubResultsTransactCompleteConnector.WireTo(MiHubResultsWindow, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_5aaeib7gs6vg67h1ae0hi7hn29.WireTo(MiHubWaitingWindow, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            MiHubResultsWindow.WireTo(id_37v5ijh1r6kt9to3j318fbg81s, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Vertical","DestinationIsReference":false} */
            id_37v5ijh1r6kt9to3j318fbg81s.WireTo(id_213st23bu9dl8pnm821tldos15, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Panel","DestinationIsReference":false} */
            id_37v5ijh1r6kt9to3j318fbg81s.WireTo(id_5bau91qrkn9t98ut9mhgii4k7j, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"RightJustify","DestinationIsReference":false} */
            id_213st23bu9dl8pnm821tldos15.WireTo(id_5k0f4ovn7rhs42h991j6cfkmo1, "children"); /* {"SourceType":"Panel","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_213st23bu9dl8pnm821tldos15.WireTo(MiHubUploadResultsGrid, "children"); /* {"SourceType":"Panel","SourceIsReference":false,"DestinationType":"Grid","DestinationIsReference":false} */
            id_5k0f4ovn7rhs42h991j6cfkmo1.WireTo(id_1og2ov1qbcn1g4th22c9pm7tv4, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_5k0f4ovn7rhs42h991j6cfkmo1.WireTo(id_417c72s0ir8kes1hjoolugkmjm, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            id_5bau91qrkn9t98ut9mhgii4k7j.WireTo(id_57lq4lvm4ject1qhjsafgev6b9, "children"); /* {"SourceType":"RightJustify","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_57lq4lvm4ject1qhjsafgev6b9.WireTo(MiHubResultsWindow, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            autoSaveIteratorComplete.WireTo(MiHubWaitingWindow, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            autoSaveIteratorComplete.WireTo(MiHubFilePathsForUpload, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Collection","DestinationIsReference":false} */
            MiHubWaitingWindow.WireTo(id_2knrb6kk3ifds72cl3d8h85noh, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            MiHubWaitingWindow.WireTo(WaitingForMiHubPanel, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Panel","DestinationIsReference":false} */
            WaitingForMiHubPanel.WireTo(id_0a1am9i9vecfi73fa1m32nnbs9, "children"); /* {"SourceType":"Panel","SourceIsReference":false,"DestinationType":"Picture","DestinationIsReference":false} */
            mainMenuBar.WireTo(datamarsLivestockMenu, "children"); /* {"SourceType":"Menubar","SourceIsReference":true,"DestinationType":"Menu","DestinationIsReference":false} */
            datamarsLivestockMenu.WireTo(MiHubLoginMenuItem, "children"); /* {"SourceType":"Menu","SourceIsReference":false,"DestinationType":"MenuItem","DestinationIsReference":false} */
            datamarsLivestockMenu.WireTo(MiHubLogoutMenuItem, "children"); /* {"SourceType":"Menu","SourceIsReference":false,"DestinationType":"MenuItem","DestinationIsReference":false} */
            datamarsLivestockMenu.WireTo(MiHubUploadTickedSessionsMenuItem, "children"); /* {"SourceType":"Menu","SourceIsReference":false,"DestinationType":"MenuItem","DestinationIsReference":false} */
            datamarsLivestockMenu.WireTo(MiHubDeleteAllSessionsAndAnimalsMenuItem, "children"); /* {"SourceType":"Menu","SourceIsReference":false,"DestinationType":"MenuItem","DestinationIsReference":false} */
            datamarsLivestockMenu.WireTo(MiHubForgotPasswordMenuItem, "children"); /* {"SourceType":"Menu","SourceIsReference":false,"DestinationType":"MenuItem","DestinationIsReference":false} */
            datamarsLivestockMenu.WireTo(MiHubRegisterUser, "children"); /* {"SourceType":"Menu","SourceIsReference":false,"DestinationType":"MenuItem","DestinationIsReference":false} */
            datamarsLivestockMenu.WireTo(naitMovementMenu, "children"); /* {"SourceType":"Menu","SourceIsReference":false,"DestinationType":"MenuItem","DestinationIsReference":false} */
            datamarsLivestockMenu.WireTo(nlisMovementMenu, "children"); /* {"SourceType":"Menu","SourceIsReference":false,"DestinationType":"MenuItem","DestinationIsReference":false} */
            MiHubLoginMenuItem.WireTo(SUBROUTINE_MiHubLogin, "eventOutput"); /* {"SourceType":"MenuItem","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            MiHubLogoutMenuItem.WireTo(MiHubLogOut, "eventOutput"); /* {"SourceType":"MenuItem","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            MiHubLogOut.WireTo(id_6335eed2d5f64767bdc9b52ce6765bdc, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"FileWriter","DestinationIsReference":false} */
            id_6335eed2d5f64767bdc9b52ce6765bdc.WireTo(id_5b2f388fa717485a9311b2cccba6b8c4, "fileLocation"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"FileWriter","DestinationIsReference":false} */
            id_6335eed2d5f64767bdc9b52ce6765bdc.WireTo(id_e72b3cba2b8e4de6b410c5490cfb9225, "fileName"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"FileWriter","DestinationIsReference":false} */
            id_6335eed2d5f64767bdc9b52ce6765bdc.WireTo(id_56a7b5ad9cad45f38ecc45a37a686043, "fileData"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"FileWriter","DestinationIsReference":false} */
            MiHubUploadTickedSessionsMenuItem.WireTo(mihubEventConnector, "eventOutput"); /* {"SourceType":"MenuItem","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            MiHubDeleteAllSessionsAndAnimalsMenuItem.WireTo(MiHubDeleteAllSessionsAndAnimalsConnector, "eventOutput"); /* {"SourceType":"MenuItem","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            MiHubDeleteAllSessionsAndAnimalsConnector.WireTo(MiHubWaitingWindow, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            MiHubDeleteAllSessionsAndAnimalsConnector.WireTo(MiHubDeleteAllSessionsAndAnimals, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"HttpRequest","DestinationIsReference":false} */
            MiHubDeleteAllSessionsAndAnimals.WireTo(MiHubAccessTokenConnector, "accessToken"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"HttpRequest","DestinationIsReference":false} */
            MiHubDeleteAllSessionsAndAnimals.WireTo(MiHubWaitingWindow, "taskComplete"); /* {"SourceType":"HttpRequest","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            MiHubForgotPasswordMenuItem.WireTo(MiHubResetPasswordPopupWindow, "eventOutput"); /* {"SourceType":"MenuItem","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            MiHubResetPasswordPopupWindow.WireTo(id_c7cd0ba17acd4028b25be62579dff6de, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Vertical","DestinationIsReference":false} */
            id_c7cd0ba17acd4028b25be62579dff6de.WireTo(id_da427f3354c0453fa309652bd27a6598, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_c7cd0ba17acd4028b25be62579dff6de.WireTo(id_21a11a4e15914063973ebd1cc1ec799e, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_da427f3354c0453fa309652bd27a6598.WireTo(id_d8342442bbce40d69feb2faecddf3c7b, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_21a11a4e15914063973ebd1cc1ec799e.WireTo(id_3b88d94e65a3469bb68f700ba76f993b, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_21a11a4e15914063973ebd1cc1ec799e.WireTo(MiHubPasswordResetEmailTextBox, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            id_21a11a4e15914063973ebd1cc1ec799e.WireTo(id_c4922d63c8b84896993f5d132e5722c5, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_c4922d63c8b84896993f5d132e5722c5.WireTo(MiHubEmailLinkToResetPassword, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"HttpRequest","DestinationIsReference":false} */
            MiHubRegisterUser.WireTo(MiHubRegisterPopupWindow, "eventOutput"); /* {"SourceType":"MenuItem","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            MiHubRegisterPopupWindow.WireTo(MiHubRegisterFirstNameHoriz, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            MiHubRegisterPopupWindow.WireTo(MiHubRegisterLastNameHoriz, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            MiHubRegisterPopupWindow.WireTo(MiHubRegisterPhoneNumberHoriz, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            MiHubRegisterPopupWindow.WireTo(MiHubRegisterEmailHoriz, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            MiHubRegisterPopupWindow.WireTo(MiHubRegisterPasswordHoriz, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            MiHubRegisterPopupWindow.WireTo(MiHubRegisterRevealedPasswordForm, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            MiHubRegisterPopupWindow.WireTo(MiHubRegisterConfirmPasswordHoriz, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            MiHubRegisterPopupWindow.WireTo(id_7cc96389fc1c4e6b826163b0d8e3ebee, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            MiHubRegisterPopupWindow.WireTo(MiHubRegisterAddressLine1Horiz, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            MiHubRegisterPopupWindow.WireTo(MiHubRegisterAddressLine2Horiz, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            MiHubRegisterPopupWindow.WireTo(MiHubRegisterSuburbHoriz, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            MiHubRegisterPopupWindow.WireTo(MiHubRegisterCityHoriz, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            MiHubRegisterPopupWindow.WireTo(MiHubRegisterRegionHoriz, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            MiHubRegisterPopupWindow.WireTo(MiHubRegisterPostcodeHoriz, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            MiHubRegisterPopupWindow.WireTo(MiHubRegisterCountryHoriz, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            MiHubRegisterPopupWindow.WireTo(id_866886f9868b4d71ae7704ad2e302746, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            MiHubRegisterPopupWindow.WireTo(id_dba459e9eea04a61a8d0c1a29765190b, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            MiHubRegisterPopupWindow.WireTo(id_72771c1a98ee406e92cce7907e940a97, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            MiHubRegisterPopupWindow.WireTo(MiHubRegisterSubmitButtonHoriz, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            MiHubRegisterFirstNameHoriz.WireTo(MiHubRegisterFirstNameText, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            MiHubRegisterFirstNameHoriz.WireTo(MiHubRegisterFirstNameTextBox, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            MiHubRegisterFirstNamePair.WireTo(MiHubRegisterFirstNameTextBox, "Item2"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            MiHubRegisterFirstNameKey.WireTo(MiHubRegisterFirstNamePair, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            MiHubRegisterLastNameHoriz.WireTo(MiHubRegisterLastNameText, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            MiHubRegisterLastNameHoriz.WireTo(MiHubRegisterLastNameTextBox, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            MiHubRegisterLastNamePair.WireTo(MiHubRegisterLastNameTextBox, "Item2"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            MiHubRegisterLastNameTextBox.WireTo(id_672eb568cf9741838cdacc4ef5429b42, "textOutput"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"StringModifier","DestinationIsReference":false} */
            MiHubRegisterLastNameKey.WireTo(MiHubRegisterLastNamePair, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            MiHubRegisterPhoneNumberHoriz.WireTo(MiHubRegisterPhoneNumberText, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            MiHubRegisterPhoneNumberHoriz.WireTo(MiHubRegisterPhoneNumberTextBox, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            MiHubRegisterPhoneNumberPair.WireTo(MiHubRegisterPhoneNumberTextBox, "Item2"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            MiHubRegisterPhoneNumberKey.WireTo(MiHubRegisterPhoneNumberPair, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            MiHubRegisterEmailHoriz.WireTo(MiHubRegisterEmailText, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            MiHubRegisterEmailHoriz.WireTo(MiHubRegisterEmailTextBox, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            MiHubRegisterEmailPair.WireTo(MiHubRegisterEmailTextBox, "Item2"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            MiHubRegisterEmailTextBox.WireTo(id_24bd0becfb10498197af0bfb664282e6, "textOutput"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            MiHubRegisterEmailKey.WireTo(MiHubRegisterEmailPair, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            MiHubLoginAfterRegistration.WireTo(id_24bd0becfb10498197af0bfb664282e6, "username"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"HttpRequest","DestinationIsReference":false} */
            MiHubRegisterPasswordHoriz.WireTo(id_5be179b9d1c6499c82fd3374fde6fd3a, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            MiHubRegisterPasswordHoriz.WireTo(id_cda9bb60c76e4f23aaa0e371a23ff46c, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_cda9bb60c76e4f23aaa0e371a23ff46c.WireTo(id_e480193bcbd04e1a88873f26877f90d6, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"PasswordBox","DestinationIsReference":false} */
            MiHubRegisterPasswordPair.WireTo(id_e480193bcbd04e1a88873f26877f90d6, "Item2"); /* {"SourceType":"PasswordBox","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_729f1a9c040d4bb18d7a42b60690c1d2.WireTo(id_e480193bcbd04e1a88873f26877f90d6, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PasswordBox","DestinationIsReference":false} */
            id_e480193bcbd04e1a88873f26877f90d6.WireTo(id_e71fb3b8526b49c7b7be4da7699b4857, "textOutput"); /* {"SourceType":"PasswordBox","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            MiHubRegisterPasswordKey.WireTo(MiHubRegisterPasswordPair, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_729f1a9c040d4bb18d7a42b60690c1d2.WireTo(id_3420a6a3eb734749aa5ad598ec64a259, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            id_e71fb3b8526b49c7b7be4da7699b4857.WireTo(id_30499ea22cd84486b86eaca1fc1de4d0, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"DataFlowGate","DestinationIsReference":false} */
            id_e71fb3b8526b49c7b7be4da7699b4857.WireTo(id_991cdbdd601a4fef8bc49a210527243d, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_e71fb3b8526b49c7b7be4da7699b4857.WireTo(MiHubRegisterConfirmPasswordPasswordBox, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"PasswordBox","DestinationIsReference":false} */
            id_30499ea22cd84486b86eaca1fc1de4d0.WireTo(id_c595fee688c54867b940737e17fb98fa, "dataOutput"); /* {"SourceType":"DataFlowGate","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_30499ea22cd84486b86eaca1fc1de4d0.WireTo(id_e3d3285bfff9411ab2b444bbae255d27, "triggerLatchInput"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"DataFlowGate","DestinationIsReference":false} */
            MiHubLoginAfterRegistration.WireTo(id_991cdbdd601a4fef8bc49a210527243d, "password"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"HttpRequest","DestinationIsReference":false} */
            MiHubRegisterRevealedPasswordForm.WireTo(id_86e636dce3fc49cf8084b309b4ee42d6, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            MiHubRegisterRevealedPasswordForm.WireTo(id_3420a6a3eb734749aa5ad598ec64a259, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            id_3420a6a3eb734749aa5ad598ec64a259.WireTo(id_decf467d558d4b4eac506991ed477790, "textOutput"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_decf467d558d4b4eac506991ed477790.WireTo(id_96960a26ed8245ce806a2c8437e25a82, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"DataFlowGate","DestinationIsReference":false} */
            id_decf467d558d4b4eac506991ed477790.WireTo(MiHubRegisterConfirmPasswordPasswordBox, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"PasswordBox","DestinationIsReference":false} */
            id_96960a26ed8245ce806a2c8437e25a82.WireTo(id_7c4df9f0eb4545ea9eb636fa7b2569af, "dataOutput"); /* {"SourceType":"DataFlowGate","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_96960a26ed8245ce806a2c8437e25a82.WireTo(id_3fdfa60a9377405699a15fd2f93c91ef, "triggerLatchInput"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"DataFlowGate","DestinationIsReference":false} */
            id_7c4df9f0eb4545ea9eb636fa7b2569af.WireTo(id_c595fee688c54867b940737e17fb98fa, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            MiHubRegisterConfirmPasswordHoriz.WireTo(MiHubRegisterConfirmPasswordText, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            MiHubRegisterConfirmPasswordHoriz.WireTo(MiHubRegisterConfirmPasswordPasswordBox, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"PasswordBox","DestinationIsReference":false} */
            MiHubRegisterConfirmPasswordPair.WireTo(MiHubRegisterConfirmPasswordPasswordBox, "Item2"); /* {"SourceType":"PasswordBox","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            MiHubRegisterConfirmPasswordKey.WireTo(MiHubRegisterConfirmPasswordPair, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_7cc96389fc1c4e6b826163b0d8e3ebee.WireTo(id_6c471bde36cb4b2399e6339c4b6c8e17, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"CheckBox","DestinationIsReference":false} */
            id_6c471bde36cb4b2399e6339c4b6c8e17.WireTo(id_3fdfa60a9377405699a15fd2f93c91ef, "isChecked"); /* {"SourceType":"CheckBox","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_3fdfa60a9377405699a15fd2f93c91ef.WireTo(id_045ab76fe63042a0aa615641c4804568, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Not","DestinationIsReference":false} */
            id_3fdfa60a9377405699a15fd2f93c91ef.WireTo(id_c380f6df3dd84f5b928e1340b6ab937b, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"IfElse","DestinationIsReference":false} */
            id_3fdfa60a9377405699a15fd2f93c91ef.WireTo(MiHubRegisterRevealedPasswordForm, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_045ab76fe63042a0aa615641c4804568.WireTo(id_e3d3285bfff9411ab2b444bbae255d27, "reversedInput"); /* {"SourceType":"Not","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_e3d3285bfff9411ab2b444bbae255d27.WireTo(MiHubRegisterPasswordHoriz, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_c380f6df3dd84f5b928e1340b6ab937b.WireTo(id_525fac181b18457da951b590c7c385d3, "ifOutput"); /* {"SourceType":"IfElse","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_c380f6df3dd84f5b928e1340b6ab937b.WireTo(id_086fb4d7643f48d08aeffc39af4ea44f, "elseOutput"); /* {"SourceType":"IfElse","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_525fac181b18457da951b590c7c385d3.WireTo(id_e71fb3b8526b49c7b7be4da7699b4857, "inputDataB"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_525fac181b18457da951b590c7c385d3.WireTo(id_3420a6a3eb734749aa5ad598ec64a259, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            id_086fb4d7643f48d08aeffc39af4ea44f.WireTo(id_decf467d558d4b4eac506991ed477790, "inputDataB"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_086fb4d7643f48d08aeffc39af4ea44f.WireTo(id_e480193bcbd04e1a88873f26877f90d6, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"PasswordBox","DestinationIsReference":false} */
            MiHubRegisterAddressLine1Horiz.WireTo(MiHubRegisterAddressLine1Text, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            MiHubRegisterAddressLine1Horiz.WireTo(MiHubRegisterAddressLine1TextBox, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            MiHubRegisterAddressLine1Pair.WireTo(MiHubRegisterAddressLine1TextBox, "Item2"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            MiHubRegisterAddressLine1Key.WireTo(MiHubRegisterAddressLine1Pair, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            MiHubRegisterAddressLine2Horiz.WireTo(MiHubRegisterAddressLine2Text, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            MiHubRegisterAddressLine2Horiz.WireTo(MiHubRegisterAddressLine2TextBox, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            MiHubRegisterAddressLine2Pair.WireTo(MiHubRegisterAddressLine2TextBox, "Item2"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            MiHubRegisterAddressLine2Key.WireTo(MiHubRegisterAddressLine2Pair, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            MiHubRegisterSuburbHoriz.WireTo(MiHubRegisterSuburbText, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            MiHubRegisterSuburbHoriz.WireTo(MiHubRegisterSuburbTextBox, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            MiHubRegisterSuburbPair.WireTo(MiHubRegisterSuburbTextBox, "Item2"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            MiHubRegisterSuburbKey.WireTo(MiHubRegisterSuburbPair, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            MiHubRegisterCityHoriz.WireTo(MiHubRegisterCityText, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            MiHubRegisterCityHoriz.WireTo(MiHubRegisterCityTextBox, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            MiHubRegisterCityPair.WireTo(MiHubRegisterCityTextBox, "Item2"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            MiHubRegisterCityKey.WireTo(MiHubRegisterCityPair, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            MiHubRegisterRegionHoriz.WireTo(MiHubRegisterRegionText, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            MiHubRegisterRegionHoriz.WireTo(MiHubRegisterRegionTextBox, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            MiHubRegisterRegionPair.WireTo(MiHubRegisterRegionTextBox, "Item2"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            MiHubRegisterRegionKey.WireTo(MiHubRegisterRegionPair, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            MiHubRegisterPostcodeHoriz.WireTo(MiHubRegisterPostcodeText, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            MiHubRegisterPostcodeHoriz.WireTo(MiHubRegisterPostcodeTextBox, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            MiHubRegisterPostcodePair.WireTo(MiHubRegisterPostcodeTextBox, "Item2"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            MiHubRegisterPostcodeKey.WireTo(MiHubRegisterPostcodePair, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            MiHubRegisterCountryHoriz.WireTo(MiHubRegisterCountryText, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            MiHubRegisterCountryHoriz.WireTo(MiHubRegisterCountryTextBox, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            id_2bc9ebfeddb54821b9d4a0a80351bbea.WireTo(MiHubRegisterCountryTextBox, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            MiHubRegisterCountryPair.WireTo(MiHubRegisterCountryTextBox, "Item2"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            AppStartEventConnector.WireTo(id_2bc9ebfeddb54821b9d4a0a80351bbea, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":true,"DestinationType":"Data","DestinationIsReference":false} */
            MiHubRegisterCountryKey.WireTo(MiHubRegisterCountryPair, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_866886f9868b4d71ae7704ad2e302746.WireTo(id_3dda425d1d44428888650217f8641170, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_866886f9868b4d71ae7704ad2e302746.WireTo(id_5974aa971d5d493ca0ab489286721eeb, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_3dda425d1d44428888650217f8641170.WireTo(id_74e03da34db240ce9b57802d3d8118dc, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"OpenWebBrowser","DestinationIsReference":false} */
            id_5974aa971d5d493ca0ab489286721eeb.WireTo(id_b114566d7609404caf617d4afa325c3e, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"OpenWebBrowser","DestinationIsReference":false} */
            id_dba459e9eea04a61a8d0c1a29765190b.WireTo(id_9fa6d12d37f44a2b86ed059d7c8229c9, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"CheckBox","DestinationIsReference":false} */
            id_9fa6d12d37f44a2b86ed059d7c8229c9.WireTo(id_65f3c7ebb0b54cfbb3bf45b6059693e3, "isChecked"); /* {"SourceType":"CheckBox","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_72771c1a98ee406e92cce7907e940a97.WireTo(id_ff230998ccef43cfb64db36b7fe946fa, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"CheckBox","DestinationIsReference":false} */
            MiHubRegisterSubmitButtonHoriz.WireTo(MiHubRegisterSubmitButton, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            MiHubRegisterSubmitButton.WireTo(id_0cfc810d6b5b428caa7a75c6e1db9ba6, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_0cfc810d6b5b428caa7a75c6e1db9ba6.WireTo(id_65f3c7ebb0b54cfbb3bf45b6059693e3, "inputDataB"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_0cfc810d6b5b428caa7a75c6e1db9ba6.WireTo(id_c65580cbfe03402e9592e9f8170c4b03, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"IfElse","DestinationIsReference":false} */
            id_c65580cbfe03402e9592e9f8170c4b03.WireTo(MiHubRegisterEventConnector, "ifOutput"); /* {"SourceType":"IfElse","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            MiHubRegisterEventConnector.WireTo(MiHubGetRegisterFieldPairs, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            MiHubRegisterEventConnector.WireTo(MiHubGetRegisterPostContent, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"ConvertPairsToDictionary","DestinationIsReference":false} */
            MiHubRegisterEventConnector.WireTo(MiHubRegisterHttpRequest, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"HttpRequest","DestinationIsReference":false} */
            MiHubGetRegisterFieldPairs.WireTo(MiHubRegisterFirstNameKey, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            MiHubGetRegisterFieldPairs.WireTo(MiHubRegisterLastNameKey, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            MiHubGetRegisterFieldPairs.WireTo(MiHubRegisterPhoneNumberKey, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            MiHubGetRegisterFieldPairs.WireTo(MiHubRegisterEmailKey, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            MiHubGetRegisterFieldPairs.WireTo(MiHubRegisterConfirmPasswordKey, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            MiHubGetRegisterFieldPairs.WireTo(MiHubRegisterAddressLine1Key, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            MiHubGetRegisterFieldPairs.WireTo(MiHubRegisterAddressLine2Key, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            MiHubGetRegisterFieldPairs.WireTo(MiHubRegisterSuburbKey, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            MiHubGetRegisterFieldPairs.WireTo(MiHubRegisterCityKey, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            MiHubGetRegisterFieldPairs.WireTo(MiHubRegisterRegionKey, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            MiHubGetRegisterFieldPairs.WireTo(MiHubRegisterPostcodeKey, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            MiHubGetRegisterFieldPairs.WireTo(MiHubRegisterCountryKey, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            MiHubGetRegisterFieldPairs.WireTo(MiHubRegisterPasswordKey, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            MiHubGetRegisterPostContent.WireTo(MiHubRegisterFirstNamePair, "pairs"); /* {"SourceType":"ConvertPairsToDictionary","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            MiHubGetRegisterPostContent.WireTo(MiHubRegisterLastNamePair, "pairs"); /* {"SourceType":"ConvertPairsToDictionary","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            MiHubGetRegisterPostContent.WireTo(MiHubRegisterPhoneNumberPair, "pairs"); /* {"SourceType":"ConvertPairsToDictionary","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            MiHubGetRegisterPostContent.WireTo(MiHubRegisterEmailPair, "pairs"); /* {"SourceType":"ConvertPairsToDictionary","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            MiHubGetRegisterPostContent.WireTo(MiHubRegisterConfirmPasswordPair, "pairs"); /* {"SourceType":"ConvertPairsToDictionary","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            MiHubGetRegisterPostContent.WireTo(MiHubRegisterAddressLine1Pair, "pairs"); /* {"SourceType":"ConvertPairsToDictionary","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            MiHubGetRegisterPostContent.WireTo(MiHubRegisterAddressLine2Pair, "pairs"); /* {"SourceType":"ConvertPairsToDictionary","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            MiHubGetRegisterPostContent.WireTo(MiHubRegisterSuburbPair, "pairs"); /* {"SourceType":"ConvertPairsToDictionary","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            MiHubGetRegisterPostContent.WireTo(MiHubRegisterCityPair, "pairs"); /* {"SourceType":"ConvertPairsToDictionary","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            MiHubGetRegisterPostContent.WireTo(MiHubRegisterRegionPair, "pairs"); /* {"SourceType":"ConvertPairsToDictionary","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            MiHubGetRegisterPostContent.WireTo(MiHubRegisterPostcodePair, "pairs"); /* {"SourceType":"ConvertPairsToDictionary","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            MiHubGetRegisterPostContent.WireTo(MiHubRegisterCountryPair, "pairs"); /* {"SourceType":"ConvertPairsToDictionary","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            MiHubGetRegisterPostContent.WireTo(MiHubRegisterPasswordPair, "pairs"); /* {"SourceType":"ConvertPairsToDictionary","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            MiHubGetRegisterPostContent.WireTo(MiHubGetRegisterPostContentConnector, "dictOutput"); /* {"SourceType":"ConvertPairsToDictionary","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            MiHubRegisterHttpRequest.WireTo(MiHubGetRegisterPostContentConnector, "postContent"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"HttpRequest","DestinationIsReference":false} */
            MiHubRegisterHttpRequest.WireTo(id_1b22e6d1faef4da29dca2492e66e4fdb, "responseJsonOutput"); /* {"SourceType":"HttpRequest","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_1b22e6d1faef4da29dca2492e66e4fdb.WireTo(id_546f5fa264784a188af409f1ad8f1a92, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Equals","DestinationIsReference":false} */
            id_546f5fa264784a188af409f1ad8f1a92.WireTo(id_aefa2b5a4a72443e8b0e7150502b066d, "isEqual"); /* {"SourceType":"Equals","SourceIsReference":false,"DestinationType":"IfElse","DestinationIsReference":false} */
            id_aefa2b5a4a72443e8b0e7150502b066d.WireTo(id_6e133aa0417447d0be52e3484150e9f6, "ifOutput"); /* {"SourceType":"IfElse","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_aefa2b5a4a72443e8b0e7150502b066d.WireTo(id_1784b154f7374f5cbb911ee7a168f20c, "elseOutput"); /* {"SourceType":"IfElse","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_6e133aa0417447d0be52e3484150e9f6.WireTo(MiHubRegisterPopupWindow, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_6e133aa0417447d0be52e3484150e9f6.WireTo(id_2b524b4fb9de456695b8b8d53e190673, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_2b524b4fb9de456695b8b8d53e190673.WireTo(id_36a8f157c984404893e73064f71acab6, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_2b524b4fb9de456695b8b8d53e190673.WireTo(id_e0983e688ae94e53926652d8cef35e56, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_2b524b4fb9de456695b8b8d53e190673.WireTo(id_09f936ef6d1e41e98cf206357c6cee6c, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_2b524b4fb9de456695b8b8d53e190673.WireTo(id_d6b8c97519914ba5a636e8b3c906d498, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_2b524b4fb9de456695b8b8d53e190673.WireTo(id_55d50254d0514a7c9f85b77e362825ac, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_2b524b4fb9de456695b8b8d53e190673.WireTo(id_332b34352fe14890b48709e66b3e4fca, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_36a8f157c984404893e73064f71acab6.WireTo(id_8b9fe493bca443cdb43ffbfab28c368b, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_e0983e688ae94e53926652d8cef35e56.WireTo(id_24e47c79792840ff9243dba8ac427396, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_09f936ef6d1e41e98cf206357c6cee6c.WireTo(id_9a7a57298d13465f8c6cb78490fafb45, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_09f936ef6d1e41e98cf206357c6cee6c.WireTo(id_faa1c9b7ad214735807f6e17f9e63ee2, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            id_672eb568cf9741838cdacc4ef5429b42.WireTo(id_faa1c9b7ad214735807f6e17f9e63ee2, "stringOutput"); /* {"SourceType":"StringModifier","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            id_faa1c9b7ad214735807f6e17f9e63ee2.WireTo(id_8c07cd9ed4744a5089cd3ece10a84595, "textOutput"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_d8dcf82c24564f71ae752e1f19ba5bc2.WireTo(id_8c07cd9ed4744a5089cd3ece10a84595, "Item2"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            MiHubCreateUpdateFarmJObject.WireTo(id_d8dcf82c24564f71ae752e1f19ba5bc2, "stringPairs"); /* {"SourceType":"CreateJObject","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_d6b8c97519914ba5a636e8b3c906d498.WireTo(id_2b41308ef31546d880b27ec1ce41f01c, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_d6b8c97519914ba5a636e8b3c906d498.WireTo(id_b2937648b6eb4ab8821fff7536397cf1, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            id_d4979dfb8e6c4cbfb0f0be6883c7f600.WireTo(id_b2937648b6eb4ab8821fff7536397cf1, "Item2"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_b2937648b6eb4ab8821fff7536397cf1.WireTo(id_150954a7cc4a4da599521fbcaaec1e8d, "textOutput"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            MiHubCreateUpdateFarmJObject.WireTo(id_d4979dfb8e6c4cbfb0f0be6883c7f600, "stringPairs"); /* {"SourceType":"CreateJObject","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            MiHubCreateUpdateUserJObject.WireTo(id_d4979dfb8e6c4cbfb0f0be6883c7f600, "stringPairs"); /* {"SourceType":"CreateJObject","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_55d50254d0514a7c9f85b77e362825ac.WireTo(id_e0ba0132e37541b8abdbde614694ea88, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_55d50254d0514a7c9f85b77e362825ac.WireTo(id_ba1a634fe34e4a30aebc2e2d14f2caca, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            id_833c5ff0b4ca4f96a6de4a74dd3d51ed.WireTo(id_ba1a634fe34e4a30aebc2e2d14f2caca, "Item2"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_ba1a634fe34e4a30aebc2e2d14f2caca.WireTo(id_20f55ade17524bdc9ec9f20c6393c08b, "textOutput"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            MiHubCreateUpdateFarmJObject.WireTo(id_833c5ff0b4ca4f96a6de4a74dd3d51ed, "stringPairs"); /* {"SourceType":"CreateJObject","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            MiHubCreateUpdateUserJObject.WireTo(id_833c5ff0b4ca4f96a6de4a74dd3d51ed, "stringPairs"); /* {"SourceType":"CreateJObject","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_332b34352fe14890b48709e66b3e4fca.WireTo(id_4e9a1ae87c044ef7800d9cd43f5867f0, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"RightJustify","DestinationIsReference":false} */
            id_4e9a1ae87c044ef7800d9cd43f5867f0.WireTo(id_2a9c4ef4b1314910b09a613d6360fb67, "children"); /* {"SourceType":"RightJustify","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_2a9c4ef4b1314910b09a613d6360fb67.WireTo(id_4a3a78d6ad2c4d11843585518606023b, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_4a3a78d6ad2c4d11843585518606023b.WireTo(id_8559aa3a6cb749c69d9446902037bd89, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_4a3a78d6ad2c4d11843585518606023b.WireTo(MiHubLoginAfterRegistration, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"HttpRequest","DestinationIsReference":false} */
            id_8559aa3a6cb749c69d9446902037bd89.WireTo(id_2b524b4fb9de456695b8b8d53e190673, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            MiHubLoginAfterRegistration.WireTo(id_1adc13ee7db64437bf2737342729407b, "responseJsonOutput"); /* {"SourceType":"HttpRequest","SourceIsReference":false,"DestinationType":"JSONParser","DestinationIsReference":false} */
            MiHubLoginAfterRegistration.WireTo(id_0838b6d25d5b43f4a53336fbf30d08e1, "taskComplete"); /* {"SourceType":"HttpRequest","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_1adc13ee7db64437bf2737342729407b.WireTo(id_a2ea6fd02b404e4bb5104a1ada607380, "jTokenOutput"); /* {"SourceType":"JSONParser","SourceIsReference":false,"DestinationType":"ToString","DestinationIsReference":false} */
            id_a2ea6fd02b404e4bb5104a1ada607380.WireTo(MiHubNewUserAccessTokenConnector, "stringOutput"); /* {"SourceType":"ToString","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            MiHubNewUserAccessTokenConnector.WireTo(convertMiHubAccessTokenToJson, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            convertMiHubAccessTokenToJson.WireTo(id_f6ac589e2e064743aeee634b5f5ee397, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"JSONParser","DestinationIsReference":false} */
            id_f6ac589e2e064743aeee634b5f5ee397.WireTo(id_d30a47070f7f426b8e286696e0e3bdc5, "jTokenOutput"); /* {"SourceType":"JSONParser","SourceIsReference":false,"DestinationType":"ToString","DestinationIsReference":false} */
            id_d30a47070f7f426b8e286696e0e3bdc5.WireTo(id_ce23f156695e40fc99d74a7ecde1f1e7, "stringOutput"); /* {"SourceType":"ToString","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_ce23f156695e40fc99d74a7ecde1f1e7.WireTo(id_0a87536e796c46749dfd9e2199fad0d0, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"StringModifier","DestinationIsReference":false} */
            id_0a87536e796c46749dfd9e2199fad0d0.WireTo(id_bcf1a241b140420c8446eaf4ef431de0, "stringOutput"); /* {"SourceType":"StringModifier","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_0838b6d25d5b43f4a53336fbf30d08e1.WireTo(MiHubGetUserFarmsHttpRequest, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"HttpRequest","DestinationIsReference":false} */
            id_0838b6d25d5b43f4a53336fbf30d08e1.WireTo(MiHubCreateUpdateUserJObject, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"CreateJObject","DestinationIsReference":false} */
            MiHubGetUserFarmsHttpRequest.WireTo(MiHubNewUserAccessTokenConnector, "accessToken"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"HttpRequest","DestinationIsReference":false} */
            MiHubGetUserFarmsHttpRequest.WireTo(id_2548234f7f7a4d3291dc13c766c51540, "responseJsonOutput"); /* {"SourceType":"HttpRequest","SourceIsReference":false,"DestinationType":"JSONParser","DestinationIsReference":false} */
            MiHubGetUserFarmsHttpRequest.WireTo(MiHubCreateUpdateFarmJObject, "taskComplete"); /* {"SourceType":"HttpRequest","SourceIsReference":false,"DestinationType":"CreateJObject","DestinationIsReference":false} */
            id_2548234f7f7a4d3291dc13c766c51540.WireTo(id_6f0bec1fe272464faa6c5a4fe6070f0d, "jTokenOutput"); /* {"SourceType":"JSONParser","SourceIsReference":false,"DestinationType":"ToString","DestinationIsReference":false} */
            id_6f0bec1fe272464faa6c5a4fe6070f0d.WireTo(id_defeab784c324bf08ba7c7465456885a, "stringOutput"); /* {"SourceType":"ToString","SourceIsReference":false,"DestinationType":"StringModifier","DestinationIsReference":false} */
            id_defeab784c324bf08ba7c7465456885a.WireTo(id_7d9d7ce095f94b3592c00f45c2dded9a, "stringOutput"); /* {"SourceType":"StringModifier","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            MiHubCreateUpdateFarmJObject.WireTo(id_dd2bb2b9325a489483cfaf66baa14c36, "outputAsJSON"); /* {"SourceType":"CreateJObject","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_dd2bb2b9325a489483cfaf66baa14c36.WireTo(id_30421d26a7924c0fb46aae1d73765332, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"ConvertToEvent","DestinationIsReference":false} */
            id_30421d26a7924c0fb46aae1d73765332.WireTo(MiHubUpdateFarmHttpRequest, "eventOutput"); /* {"SourceType":"ConvertToEvent","SourceIsReference":false,"DestinationType":"HttpRequest","DestinationIsReference":false} */
            MiHubUpdateFarmHttpRequest.WireTo(id_7d9d7ce095f94b3592c00f45c2dded9a, "url"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"HttpRequest","DestinationIsReference":false} */
            MiHubUpdateFarmHttpRequest.WireTo(id_dd2bb2b9325a489483cfaf66baa14c36, "jsonData"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"HttpRequest","DestinationIsReference":false} */
            MiHubUpdateFarmHttpRequest.WireTo(id_f350e52641bf4046b178b0fa1646c4cf, "headers"); /* {"SourceType":"HttpRequest","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            MiHubUpdateFarmHttpRequest.WireTo(MiHubNewUserAccessTokenConnector, "accessToken"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"HttpRequest","DestinationIsReference":false} */
            MiHubCreateUpdateUserJObject.WireTo(id_b8b9cad76d86405e9df3cbae35578fef, "outputAsJSON"); /* {"SourceType":"CreateJObject","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_b8b9cad76d86405e9df3cbae35578fef.WireTo(id_cb67b64a2ad1457ba5e5cfec25aa031c, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"ConvertToEvent","DestinationIsReference":false} */
            id_cb67b64a2ad1457ba5e5cfec25aa031c.WireTo(MiHubUpdateUserHttpRequest, "eventOutput"); /* {"SourceType":"ConvertToEvent","SourceIsReference":false,"DestinationType":"HttpRequest","DestinationIsReference":false} */
            MiHubUpdateUserHttpRequest.WireTo(id_bcf1a241b140420c8446eaf4ef431de0, "url"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"HttpRequest","DestinationIsReference":false} */
            MiHubUpdateUserHttpRequest.WireTo(id_b8b9cad76d86405e9df3cbae35578fef, "jsonData"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"HttpRequest","DestinationIsReference":false} */
            MiHubUpdateUserHttpRequest.WireTo(id_dd832e75f1d4430d9a68720945aef2f4, "headers"); /* {"SourceType":"HttpRequest","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            MiHubUpdateUserHttpRequest.WireTo(MiHubNewUserAccessTokenConnector, "accessToken"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"HttpRequest","DestinationIsReference":false} */
            id_1784b154f7374f5cbb911ee7a168f20c.WireTo(id_bfc9fd8f7a2e46d6ac05e0295cd37111, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_1784b154f7374f5cbb911ee7a168f20c.WireTo(id_201d0f0cf6f048c792c5990652a2de84, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_bfc9fd8f7a2e46d6ac05e0295cd37111.WireTo(id_1b22e6d1faef4da29dca2492e66e4fdb, "inputDataB"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_bfc9fd8f7a2e46d6ac05e0295cd37111.WireTo(id_e7b57c8a6e674b2eae8f718690c1d6d0, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_e7b57c8a6e674b2eae8f718690c1d6d0.WireTo(id_4d17241aa5204137bad3cfa1a01ed732, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"JSONParser","DestinationIsReference":false} */
            id_4d17241aa5204137bad3cfa1a01ed732.WireTo(id_053df641941e4a2187c70657336c68b6, "jTokenOutput"); /* {"SourceType":"JSONParser","SourceIsReference":false,"DestinationType":"ToString","DestinationIsReference":false} */
            id_053df641941e4a2187c70657336c68b6.WireTo(id_314f1211db554b31a998ebbacb9e04f9, "stringOutput"); /* {"SourceType":"ToString","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_201d0f0cf6f048c792c5990652a2de84.WireTo(id_2f3615c4bb614684896a967547a3aa1e, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_2f3615c4bb614684896a967547a3aa1e.WireTo(id_314f1211db554b31a998ebbacb9e04f9, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_2f3615c4bb614684896a967547a3aa1e.WireTo(id_ba5c282a10f34d99b7f2fd8403969c84, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_ba5c282a10f34d99b7f2fd8403969c84.WireTo(id_201d0f0cf6f048c792c5990652a2de84, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            naitMovementMenu.WireTo(startNait, "eventOutput"); /* {"SourceType":"MenuItem","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            startNait.WireTo(id_53c52960fe3241d89009c7d78fd506ef, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            startNait.WireTo(id_c6339ec9b4ca49819be8d7234fce48f4, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            startNait.WireTo(checkInternetConnection, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"HttpRequest","DestinationIsReference":false} */
            startNait.WireTo(id_bd08431f0ad84f0cb71f566017085cdb, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_53c52960fe3241d89009c7d78fd506ef.WireTo(isNaitRequest, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_c6339ec9b4ca49819be8d7234fce48f4.WireTo(isNlisRequest, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_bd08431f0ad84f0cb71f566017085cdb.WireTo(internetConnectionDetectedConnector, "inputDataB"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_bd08431f0ad84f0cb71f566017085cdb.WireTo(id_55abc935e20b4b0cb4ce2f9df82cf80a, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"IfElse","DestinationIsReference":false} */
            id_55abc935e20b4b0cb4ce2f9df82cf80a.WireTo(id_7292bc98301444fab5b2aae11e31a8fd, "ifOutput"); /* {"SourceType":"IfElse","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_55abc935e20b4b0cb4ce2f9df82cf80a.WireTo(id_6b72224ba43042ecbb66a99659902cae, "elseOutput"); /* {"SourceType":"IfElse","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_7292bc98301444fab5b2aae11e31a8fd.WireTo(id_30ae8f8365824f7a97d2a1a521d5c5ba, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_7292bc98301444fab5b2aae11e31a8fd.WireTo(id_0aa41e47f2f5472f9a081525b167345e, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_7292bc98301444fab5b2aae11e31a8fd.WireTo(id_df9b625052db4aae8c7635214cb29f35, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_30ae8f8365824f7a97d2a1a521d5c5ba.WireTo(openNaitForm, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"EventGate","DestinationIsReference":false} */
            id_0aa41e47f2f5472f9a081525b167345e.WireTo(openNaitForm, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventGate","DestinationIsReference":false} */
            id_df9b625052db4aae8c7635214cb29f35.WireTo(id_096658137fc64c6aaa12ee21db062be4, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_096658137fc64c6aaa12ee21db062be4.WireTo(openNaitForm, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"EventGate","DestinationIsReference":false} */
            id_6b72224ba43042ecbb66a99659902cae.WireTo(id_eac79c6c75ec47c0a952e68fb6a9e3e4, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_6b72224ba43042ecbb66a99659902cae.WireTo(id_bb678b85e2eb485182b99f30adc6d76f, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_eac79c6c75ec47c0a952e68fb6a9e3e4.WireTo(id_f3a8376c0e8349e8bc7bf47f2d99d8fb, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_bb678b85e2eb485182b99f30adc6d76f.WireTo(id_978fb45f0da14e9085cd6d1f1e5deab7, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_978fb45f0da14e9085cd6d1f1e5deab7.WireTo(id_365b5944a5384370a15012d139c8fcb7, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_365b5944a5384370a15012d139c8fcb7.WireTo(id_410d0216993b4902bf0ccfe0cbc4ba38, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_410d0216993b4902bf0ccfe0cbc4ba38.WireTo(id_6b72224ba43042ecbb66a99659902cae, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_410d0216993b4902bf0ccfe0cbc4ba38.WireTo(id_7292bc98301444fab5b2aae11e31a8fd, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            nlisMovementMenu.WireTo(startNlis, "eventOutput"); /* {"SourceType":"MenuItem","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            startNlis.WireTo(id_1b27e18ead644e4c8c084dcdf410acac, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            startNlis.WireTo(id_3f8e5b431e8446578e978b098d75231c, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            startNlis.WireTo(checkInternetConnection, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"HttpRequest","DestinationIsReference":false} */
            startNlis.WireTo(id_bf96088659a0499d938bafec835fc3d7, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_1b27e18ead644e4c8c084dcdf410acac.WireTo(isNlisRequest, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_3f8e5b431e8446578e978b098d75231c.WireTo(isNaitRequest, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_bf96088659a0499d938bafec835fc3d7.WireTo(internetConnectionDetectedConnector, "inputDataB"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_bf96088659a0499d938bafec835fc3d7.WireTo(id_4b38fa419af0426eac26e945bb307148, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"IfElse","DestinationIsReference":false} */
            id_4b38fa419af0426eac26e945bb307148.WireTo(id_0ec4f4b44fc24466beeab9d39fb2127c, "ifOutput"); /* {"SourceType":"IfElse","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_4b38fa419af0426eac26e945bb307148.WireTo(id_f03ed8965e104025aa6cd23f16161782, "elseOutput"); /* {"SourceType":"IfElse","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_0ec4f4b44fc24466beeab9d39fb2127c.WireTo(id_c1cdac9b45714a94bd252b1e652478a9, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_0ec4f4b44fc24466beeab9d39fb2127c.WireTo(id_8a15c8a96f614f2295f410243c1aec04, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_0ec4f4b44fc24466beeab9d39fb2127c.WireTo(id_4fa85b51f3da42e29ae1b593501db9cb, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_c1cdac9b45714a94bd252b1e652478a9.WireTo(openNlisForm, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"EventGate","DestinationIsReference":false} */
            id_8a15c8a96f614f2295f410243c1aec04.WireTo(openNlisForm, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventGate","DestinationIsReference":false} */
            id_4fa85b51f3da42e29ae1b593501db9cb.WireTo(id_73fefa5cddcf4c768d3aaecd2eb5411e, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_73fefa5cddcf4c768d3aaecd2eb5411e.WireTo(openNlisForm, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"EventGate","DestinationIsReference":false} */
            id_f03ed8965e104025aa6cd23f16161782.WireTo(id_283c0c61ab87494bbf910cffd9bd3564, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_f03ed8965e104025aa6cd23f16161782.WireTo(id_4905798c2ffb4741b72d398dada38552, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_283c0c61ab87494bbf910cffd9bd3564.WireTo(id_ee1c0b13a02a4811919f6548a8cf5701, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_4905798c2ffb4741b72d398dada38552.WireTo(id_3b8bb260778f4d73b4d6d804aa07c7d8, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_3b8bb260778f4d73b4d6d804aa07c7d8.WireTo(id_7a09fad3caa649e8b67685650452a36c, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_7a09fad3caa649e8b67685650452a36c.WireTo(id_2f1511c34cbd492a9ee80249134130b7, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_2f1511c34cbd492a9ee80249134130b7.WireTo(id_f03ed8965e104025aa6cd23f16161782, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_2f1511c34cbd492a9ee80249134130b7.WireTo(id_0ec4f4b44fc24466beeab9d39fb2127c, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            openNaitForm.WireTo(naitLoginWindow, "eventOutput"); /* {"SourceType":"EventGate","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            naitLoginWindow.WireTo(id_6bc75430a8b54e0f9992f1376902374a, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            naitLoginWindow.WireTo(id_d8cae10f88e04444b90020f8346f0124, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            naitLoginWindow.WireTo(id_9315cf1f2761454f8f91154d01c7c5c0, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            naitLoginWindow.WireTo(id_90efe4ac8e3a4c9fbf01aa3768ef31b5, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            naitLoginWindow.WireTo(id_07a7e799676f44769e81fab90502a0f1, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_6bc75430a8b54e0f9992f1376902374a.WireTo(id_9361214861474d599977127d9081abea, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_d8cae10f88e04444b90020f8346f0124.WireTo(id_f747a8e3e41f493caf23ba92cf64e34f, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_d8cae10f88e04444b90020f8346f0124.WireTo(id_40832017140c4188b03057a7400549ed, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            id_65eb47575ca948b4869480e51e1f7526.WireTo(id_40832017140c4188b03057a7400549ed, "Item2"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_a904b669b255441ba48d7e6faa581653.WireTo(id_65eb47575ca948b4869480e51e1f7526, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_65eb47575ca948b4869480e51e1f7526.WireTo(id_ad680c5d89a6414b83ad625a37b3cbab, "pairOutput"); /* {"SourceType":"Pair","SourceIsReference":false,"DestinationType":"ConvertToJProperty","DestinationIsReference":false} */
            id_ad680c5d89a6414b83ad625a37b3cbab.WireTo(naitAnimalTransferModelProperties, "jPropertyOutput"); /* {"SourceType":"ConvertToJProperty","SourceIsReference":false,"DestinationType":"Collection","DestinationIsReference":false} */
            naitAnimalTransferModelProperties.WireTo(AnimalTransferModel, "listOutput"); /* {"SourceType":"Collection","SourceIsReference":false,"DestinationType":"ConvertToJObject","DestinationIsReference":false} */
            AnimalTransferModel.WireTo(id_8207d0fb24f94de09bd6fa52c6acf46b, "output"); /* {"SourceType":"ConvertToJObject","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            id_8207d0fb24f94de09bd6fa52c6acf46b.WireTo(id_bc04d19f4ce045298c36eeec2858f1bb, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            naitSetPlaceholderValues.WireTo(naitIntegrationType, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            naitSetPlaceholderValues.WireTo(DataLinkDeviceID, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            naitSetPlaceholderValues.WireTo(naitIsStaging, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            naitIntegrationType.WireTo(id_6ab18e1d288a48009a389d4d520e538c, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            id_6ab18e1d288a48009a389d4d520e538c.WireTo(id_d30bb2b0d38f4719a360da3eb5fd1cc6, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"ConvertToJProperty","DestinationIsReference":false} */
            id_d30bb2b0d38f4719a360da3eb5fd1cc6.WireTo(naitAnimalTransferModelProperties, "jPropertyOutput"); /* {"SourceType":"ConvertToJProperty","SourceIsReference":false,"DestinationType":"Collection","DestinationIsReference":false} */
            DataLinkDeviceID.WireTo(id_c8a23b2380ff4f9bbefe5e5819f61b56, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            id_c8a23b2380ff4f9bbefe5e5819f61b56.WireTo(id_313fc9ff67f1439da6458d65bcc9af06, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"ConvertToJProperty","DestinationIsReference":false} */
            id_313fc9ff67f1439da6458d65bcc9af06.WireTo(naitAnimalTransferModelProperties, "jPropertyOutput"); /* {"SourceType":"ConvertToJProperty","SourceIsReference":false,"DestinationType":"Collection","DestinationIsReference":false} */
            naitIsStaging.WireTo(id_4a80df13ca8d44a299a61642bc942206, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            id_4a80df13ca8d44a299a61642bc942206.WireTo(id_e3200202c206453eae7d97c9e1b301aa, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"ConvertToJProperty","DestinationIsReference":false} */
            id_e3200202c206453eae7d97c9e1b301aa.WireTo(naitAnimalTransferModelProperties, "jPropertyOutput"); /* {"SourceType":"ConvertToJProperty","SourceIsReference":false,"DestinationType":"Collection","DestinationIsReference":false} */
            id_9315cf1f2761454f8f91154d01c7c5c0.WireTo(id_998f19f207b04942813c43f68ed7511f, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_9315cf1f2761454f8f91154d01c7c5c0.WireTo(id_5dd95fa680e64e4eb7b89e4b606d0b70, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"PasswordBox","DestinationIsReference":false} */
            id_a2f81ad95411456eb816c998fdc3369e.WireTo(id_5dd95fa680e64e4eb7b89e4b606d0b70, "Item2"); /* {"SourceType":"PasswordBox","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_62c77e7aa1cc4b5187d58fabf10b99d0.WireTo(id_a2f81ad95411456eb816c998fdc3369e, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_a2f81ad95411456eb816c998fdc3369e.WireTo(id_ad680c5d89a6414b83ad625a37b3cbab, "pairOutput"); /* {"SourceType":"Pair","SourceIsReference":false,"DestinationType":"ConvertToJProperty","DestinationIsReference":false} */
            id_90efe4ac8e3a4c9fbf01aa3768ef31b5.WireTo(id_3c7e55d2a54148e59d67a5a9afc2ab43, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_90efe4ac8e3a4c9fbf01aa3768ef31b5.WireTo(id_9fdcb5b5ae214ebcbf1d66898eb33caa, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            id_ecf1b76ebf444821a6ac4f298f7d84fb.WireTo(id_9fdcb5b5ae214ebcbf1d66898eb33caa, "Item2"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_b744f58d2f4144938194e1e7dfaff50d.WireTo(id_9fdcb5b5ae214ebcbf1d66898eb33caa, "Item2"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_9fdcb5b5ae214ebcbf1d66898eb33caa.WireTo(id_257f730a78944f218fcb41099f882921, "textOutput"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_04bac1d1ef804607a8a4c5e990e19184.WireTo(id_ecf1b76ebf444821a6ac4f298f7d84fb, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_ecf1b76ebf444821a6ac4f298f7d84fb.WireTo(id_10fc8ad6907140aa8ded720d980ece47, "pairOutput"); /* {"SourceType":"Pair","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_10fc8ad6907140aa8ded720d980ece47.WireTo(id_a84abf49555d47e39927398d67c37007, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"ConvertToJProperty","DestinationIsReference":false} */
            id_a84abf49555d47e39927398d67c37007.WireTo(id_5544b1d1fc02448db83b43331a6f7569, "jPropertyOutput"); /* {"SourceType":"ConvertToJProperty","SourceIsReference":false,"DestinationType":"Collection","DestinationIsReference":false} */
            id_5544b1d1fc02448db83b43331a6f7569.WireTo(naitRequest, "listOutput"); /* {"SourceType":"Collection","SourceIsReference":false,"DestinationType":"ConvertToJObject","DestinationIsReference":false} */
            naitRequest.WireTo(id_aca856078615468b9080a5dc93925a4e, "output"); /* {"SourceType":"ConvertToJObject","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            id_aca856078615468b9080a5dc93925a4e.WireTo(id_aae2ddd9f1dc4728a59446c980b7693c, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"ConvertToJProperty","DestinationIsReference":false} */
            id_aae2ddd9f1dc4728a59446c980b7693c.WireTo(naitAnimalTransferModelProperties, "jPropertyOutput"); /* {"SourceType":"ConvertToJProperty","SourceIsReference":false,"DestinationType":"Collection","DestinationIsReference":false} */
            id_ea5215389c924a94a0f0f750db1e8d72.WireTo(id_b744f58d2f4144938194e1e7dfaff50d, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_b744f58d2f4144938194e1e7dfaff50d.WireTo(id_12d8420d774541ae9e19919bd1da77bf, "pairOutput"); /* {"SourceType":"Pair","SourceIsReference":false,"DestinationType":"ConvertToJProperty","DestinationIsReference":false} */
            id_12d8420d774541ae9e19919bd1da77bf.WireTo(naitAnimalTransferModelProperties, "jPropertyOutput"); /* {"SourceType":"ConvertToJProperty","SourceIsReference":false,"DestinationType":"Collection","DestinationIsReference":false} */
            id_257f730a78944f218fcb41099f882921.WireTo(id_ded801bcc5ba44ea8f01c0c1c2e0bb99, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            id_257f730a78944f218fcb41099f882921.WireTo(id_1dae1c9cba6542e093d34cf2350916b4, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            id_07a7e799676f44769e81fab90502a0f1.WireTo(id_19cac47015ae4f95aff82d367aaee21a, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_19cac47015ae4f95aff82d367aaee21a.WireTo(id_4d0026212c224ea6a03b08a97f463779, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_4d0026212c224ea6a03b08a97f463779.WireTo(id_60f4c18dce3b4cb28433117430e58a84, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_4d0026212c224ea6a03b08a97f463779.WireTo(id_897d889c69524975a7b3f8e228d5e76f, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_4d0026212c224ea6a03b08a97f463779.WireTo(id_a1fae4aad2fd4eb588782da37eb678d5, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_60f4c18dce3b4cb28433117430e58a84.WireTo(naitLoginWindow, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_897d889c69524975a7b3f8e228d5e76f.WireTo(id_04bac1d1ef804607a8a4c5e990e19184, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_897d889c69524975a7b3f8e228d5e76f.WireTo(id_a904b669b255441ba48d7e6faa581653, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_897d889c69524975a7b3f8e228d5e76f.WireTo(id_ea5215389c924a94a0f0f750db1e8d72, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_897d889c69524975a7b3f8e228d5e76f.WireTo(id_62c77e7aa1cc4b5187d58fabf10b99d0, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_a1fae4aad2fd4eb588782da37eb678d5.WireTo(id_6b606f533fd44d91a320d3412cdcb543, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_a1fae4aad2fd4eb588782da37eb678d5.WireTo(id_9f1b76f87b864673987734704cd8903e, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_a1fae4aad2fd4eb588782da37eb678d5.WireTo(naitAnimalRegistrationForm, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Vertical","DestinationIsReference":false} */
            id_a1fae4aad2fd4eb588782da37eb678d5.WireTo(naitSendingMovementForm, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Vertical","DestinationIsReference":false} */
            id_a1fae4aad2fd4eb588782da37eb678d5.WireTo(naitReceivingMovementForm, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Vertical","DestinationIsReference":false} */
            id_a1fae4aad2fd4eb588782da37eb678d5.WireTo(id_cbf60ba3c5ab4a5a847e79019f216a01, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_9f1b76f87b864673987734704cd8903e.WireTo(id_cd979cfb045047a09bd9887bc625f104, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_9f1b76f87b864673987734704cd8903e.WireTo(id_412aa537acce4f12ad72b66d9c1a9316, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"RightJustify","DestinationIsReference":false} */
            id_412aa537acce4f12ad72b66d9c1a9316.WireTo(id_b11aee306b4b4e1fb256c20d49386e93, "children"); /* {"SourceType":"RightJustify","SourceIsReference":false,"DestinationType":"OptionBox","DestinationIsReference":false} */
            id_b11aee306b4b4e1fb256c20d49386e93.WireTo(naitTransactionTypeConnector, "dataFlowSelectionOutput"); /* {"SourceType":"OptionBox","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_b11aee306b4b4e1fb256c20d49386e93.WireTo(id_7b86c57b469743769fc34d0df8c139c8, "children"); /* {"SourceType":"OptionBox","SourceIsReference":false,"DestinationType":"OptionBoxItem","DestinationIsReference":false} */
            id_b11aee306b4b4e1fb256c20d49386e93.WireTo(id_74a8ecbb39604244a520002d02325975, "children"); /* {"SourceType":"OptionBox","SourceIsReference":false,"DestinationType":"OptionBoxItem","DestinationIsReference":false} */
            id_b11aee306b4b4e1fb256c20d49386e93.WireTo(id_8eb72df43dae4ad9888959b08b3edde2, "children"); /* {"SourceType":"OptionBox","SourceIsReference":false,"DestinationType":"OptionBoxItem","DestinationIsReference":false} */
            naitTransactionTypeConnector.WireTo(id_ea831641b8764e0dbbd0e1f883e998bb, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Equals","DestinationIsReference":false} */
            naitTransactionTypeConnector.WireTo(id_b675bf1260d44a949d2d220d0165ce79, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Equals","DestinationIsReference":false} */
            naitTransactionTypeConnector.WireTo(id_5786e950445d499999a6822969cee806, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Equals","DestinationIsReference":false} */
            naitTransactionTypeConnector.WireTo(naitIntegrationTypeCodes, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            id_ea831641b8764e0dbbd0e1f883e998bb.WireTo(naitAnimalRegistrationForm, "isEqual"); /* {"SourceType":"Equals","SourceIsReference":false,"DestinationType":"Vertical","DestinationIsReference":false} */
            id_b675bf1260d44a949d2d220d0165ce79.WireTo(naitSendingMovementForm, "isEqual"); /* {"SourceType":"Equals","SourceIsReference":false,"DestinationType":"Vertical","DestinationIsReference":false} */
            id_5786e950445d499999a6822969cee806.WireTo(naitReceivingMovementForm, "isEqual"); /* {"SourceType":"Equals","SourceIsReference":false,"DestinationType":"Vertical","DestinationIsReference":false} */
            naitIntegrationTypeCodes.WireTo(id_70573776acca4e54b264a2110d2057ef, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_6c2bdbb762054efa9d5ba7f034cca638.WireTo(id_70573776acca4e54b264a2110d2057ef, "Item2"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_43e118675f474965956fac19cad8e1c8.WireTo(id_6c2bdbb762054efa9d5ba7f034cca638, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_6c2bdbb762054efa9d5ba7f034cca638.WireTo(id_916a416fd7524b4fa2df3a293ca01827, "pairOutput"); /* {"SourceType":"Pair","SourceIsReference":false,"DestinationType":"ConvertToJProperty","DestinationIsReference":false} */
            id_916a416fd7524b4fa2df3a293ca01827.WireTo(naitAnimalTransferModelProperties, "jPropertyOutput"); /* {"SourceType":"ConvertToJProperty","SourceIsReference":false,"DestinationType":"Collection","DestinationIsReference":false} */
            naitAnimalRegistrationForm.WireTo(id_5808ff8cf2c243448f186b40a50a830d, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            naitAnimalRegistrationForm.WireTo(id_a3039a6c2ac4488692ad366290f95c64, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            naitAnimalRegistrationForm.WireTo(id_601e2c887a9748cdb7512e09f3968459, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            naitAnimalRegistrationForm.WireTo(id_b042b3662b594f5e99318f98dbf0822b, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_a3039a6c2ac4488692ad366290f95c64.WireTo(id_8cfd55992e0a4aceb4d73687066440c1, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_a3039a6c2ac4488692ad366290f95c64.WireTo(id_baa490506588456ab06001a06eb13ad4, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"DatePicker","DestinationIsReference":false} */
            id_baa490506588456ab06001a06eb13ad4.WireTo(id_da358f4975324d07a4550c506c35976d, "selectedDate"); /* {"SourceType":"DatePicker","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_62e7cc7be118417e80b690f81202d8b6.WireTo(id_da358f4975324d07a4550c506c35976d, "Item2"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_36c0be0f8c3e4b99af4956137c978a80.WireTo(id_62e7cc7be118417e80b690f81202d8b6, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_62e7cc7be118417e80b690f81202d8b6.WireTo(id_c8f605c53e0b477a8dddc51910a9bf81, "pairOutput"); /* {"SourceType":"Pair","SourceIsReference":false,"DestinationType":"ConvertToJProperty","DestinationIsReference":false} */
            id_c8f605c53e0b477a8dddc51910a9bf81.WireTo(id_5544b1d1fc02448db83b43331a6f7569, "jPropertyOutput"); /* {"SourceType":"ConvertToJProperty","SourceIsReference":false,"DestinationType":"Collection","DestinationIsReference":false} */
            id_601e2c887a9748cdb7512e09f3968459.WireTo(id_2a2df9ed461d4a8ea41cd774ae2f6701, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_601e2c887a9748cdb7512e09f3968459.WireTo(id_99cd0ec5a9234a8fbf598ec941e23df1, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"RightJustify","DestinationIsReference":false} */
            id_99cd0ec5a9234a8fbf598ec941e23df1.WireTo(id_9bf2f32a1b8a4e3f893e7ef14748936f, "children"); /* {"SourceType":"RightJustify","SourceIsReference":false,"DestinationType":"OptionBox","DestinationIsReference":false} */
            id_9bf2f32a1b8a4e3f893e7ef14748936f.WireTo(naitAnimalTypeConnector, "dataFlowSelectionOutput"); /* {"SourceType":"OptionBox","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_9bf2f32a1b8a4e3f893e7ef14748936f.WireTo(id_2583994af4dd4c2e9817b8ac19d004ea, "children"); /* {"SourceType":"OptionBox","SourceIsReference":false,"DestinationType":"OptionBoxItem","DestinationIsReference":false} */
            id_9bf2f32a1b8a4e3f893e7ef14748936f.WireTo(id_ad47c5b2c4fd4013890e8437b13a0a10, "children"); /* {"SourceType":"OptionBox","SourceIsReference":false,"DestinationType":"OptionBoxItem","DestinationIsReference":false} */
            naitAnimalTypeConnector.WireTo(id_385bc289fdf34b31a705055fbca249df, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Equals","DestinationIsReference":false} */
            naitAnimalTypeConnector.WireTo(id_3f58b33fcfd646d882ed6935d7237489, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Equals","DestinationIsReference":false} */
            naitAnimalTypeConnector.WireTo(naitAnimalTypeCodes, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"KeyValue","DestinationIsReference":false} */
            id_385bc289fdf34b31a705055fbca249df.WireTo(id_69b0483ed2cc453b9e9db1b4a6ae3626, "isEqual"); /* {"SourceType":"Equals","SourceIsReference":false,"DestinationType":"OptionBox","DestinationIsReference":false} */
            id_3f58b33fcfd646d882ed6935d7237489.WireTo(id_83da74d940d0447aa61f86214507aae7, "isEqual"); /* {"SourceType":"Equals","SourceIsReference":false,"DestinationType":"OptionBox","DestinationIsReference":false} */
            naitAnimalTypeCodes.WireTo(id_ea722864f23b4564943d776eb3f4dea2, "valueOutputs"); /* {"SourceType":"KeyValue","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_255e0cc68a4944e9947177c05e024d79.WireTo(id_ea722864f23b4564943d776eb3f4dea2, "Item2"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_80b50ee50c0f42148a32f273835a8d75.WireTo(id_255e0cc68a4944e9947177c05e024d79, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_255e0cc68a4944e9947177c05e024d79.WireTo(id_a84abf49555d47e39927398d67c37007, "pairOutput"); /* {"SourceType":"Pair","SourceIsReference":false,"DestinationType":"ConvertToJProperty","DestinationIsReference":false} */
            id_b042b3662b594f5e99318f98dbf0822b.WireTo(id_7a33f71704a34878bba6cde487f52945, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_b042b3662b594f5e99318f98dbf0822b.WireTo(id_3c413e7bc825433c90fcf27cff93bb4f, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"RightJustify","DestinationIsReference":false} */
            id_3c413e7bc825433c90fcf27cff93bb4f.WireTo(id_69b0483ed2cc453b9e9db1b4a6ae3626, "children"); /* {"SourceType":"RightJustify","SourceIsReference":false,"DestinationType":"OptionBox","DestinationIsReference":false} */
            id_3c413e7bc825433c90fcf27cff93bb4f.WireTo(id_83da74d940d0447aa61f86214507aae7, "children"); /* {"SourceType":"RightJustify","SourceIsReference":false,"DestinationType":"OptionBox","DestinationIsReference":false} */
            id_69b0483ed2cc453b9e9db1b4a6ae3626.WireTo(naitProductionTypeConnector, "dataFlowSelectionOutput"); /* {"SourceType":"OptionBox","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_69b0483ed2cc453b9e9db1b4a6ae3626.WireTo(id_2fa1bee29a884d9d9897ab5a5374d32a, "children"); /* {"SourceType":"OptionBox","SourceIsReference":false,"DestinationType":"OptionBoxItem","DestinationIsReference":false} */
            id_69b0483ed2cc453b9e9db1b4a6ae3626.WireTo(id_1ba6f3ab67634930b124978916995aff, "children"); /* {"SourceType":"OptionBox","SourceIsReference":false,"DestinationType":"OptionBoxItem","DestinationIsReference":false} */
            id_69b0483ed2cc453b9e9db1b4a6ae3626.WireTo(id_8213f6658cd44689babf6b49569ef67c, "children"); /* {"SourceType":"OptionBox","SourceIsReference":false,"DestinationType":"OptionBoxItem","DestinationIsReference":false} */
            naitProductionTypeConnector.WireTo(naitProductionTypeCodes, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"KeyValue","DestinationIsReference":false} */
            naitProductionTypeCodes.WireTo(id_715693f09f1a461fa16c655ed8f759e1, "valueOutputs"); /* {"SourceType":"KeyValue","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_ab7220e544ba4431aaac9e8ccb3c8e2a.WireTo(id_715693f09f1a461fa16c655ed8f759e1, "Item2"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_635b56f1854b45d88592a25fb7f662d4.WireTo(id_ab7220e544ba4431aaac9e8ccb3c8e2a, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_ab7220e544ba4431aaac9e8ccb3c8e2a.WireTo(id_a84abf49555d47e39927398d67c37007, "pairOutput"); /* {"SourceType":"Pair","SourceIsReference":false,"DestinationType":"ConvertToJProperty","DestinationIsReference":false} */
            id_83da74d940d0447aa61f86214507aae7.WireTo(naitProductionTypeConnector, "dataFlowSelectionOutput"); /* {"SourceType":"OptionBox","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_83da74d940d0447aa61f86214507aae7.WireTo(id_d794a1c7dc694b4290af49bc2ba88c95, "children"); /* {"SourceType":"OptionBox","SourceIsReference":false,"DestinationType":"OptionBoxItem","DestinationIsReference":false} */
            id_83da74d940d0447aa61f86214507aae7.WireTo(id_5f25a2b6ad334037b8dca7e5fa66edaa, "children"); /* {"SourceType":"OptionBox","SourceIsReference":false,"DestinationType":"OptionBoxItem","DestinationIsReference":false} */
            id_83da74d940d0447aa61f86214507aae7.WireTo(id_301bc1388210455a93af5233e89f5830, "children"); /* {"SourceType":"OptionBox","SourceIsReference":false,"DestinationType":"OptionBoxItem","DestinationIsReference":false} */
            id_83da74d940d0447aa61f86214507aae7.WireTo(id_5567334a4d364ab79ac6eea5eb9cf426, "children"); /* {"SourceType":"OptionBox","SourceIsReference":false,"DestinationType":"OptionBoxItem","DestinationIsReference":false} */
            naitSendingMovementForm.WireTo(id_479aa2f483d5465c987e1f4f84947a38, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            naitSendingMovementForm.WireTo(id_656c2b1a985d4984811244b5214736a8, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            naitSendingMovementForm.WireTo(id_4c733314cf2d4879ae5596d2dd37377b, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            naitSendingMovementForm.WireTo(id_e1d0d849589446a69d3b3542f54406c2, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_656c2b1a985d4984811244b5214736a8.WireTo(id_e74f8a7d6aaf4092863eb8fc58e33cd4, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_656c2b1a985d4984811244b5214736a8.WireTo(id_ded801bcc5ba44ea8f01c0c1c2e0bb99, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            id_ded801bcc5ba44ea8f01c0c1c2e0bb99.WireTo(id_9fdcb5b5ae214ebcbf1d66898eb33caa, "textOutput"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            id_4c733314cf2d4879ae5596d2dd37377b.WireTo(id_ed2af88cf5184701920ccddf94186db2, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_4c733314cf2d4879ae5596d2dd37377b.WireTo(id_3a6518c31f9f41558a21bbc19c080881, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            id_0485ca43fd6147c499812456d61c1eb1.WireTo(id_3a6518c31f9f41558a21bbc19c080881, "Item2"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_294392c31a284f38abb8780073cc8dd7.WireTo(id_3a6518c31f9f41558a21bbc19c080881, "Item2"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_3a6518c31f9f41558a21bbc19c080881.WireTo(id_657d14273506444f82de4d12b5851059, "textOutput"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            id_d15c7648fe23487c9ae1ee732ca55295.WireTo(id_0485ca43fd6147c499812456d61c1eb1, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_0485ca43fd6147c499812456d61c1eb1.WireTo(id_a84abf49555d47e39927398d67c37007, "pairOutput"); /* {"SourceType":"Pair","SourceIsReference":false,"DestinationType":"ConvertToJProperty","DestinationIsReference":false} */
            id_7746044f122f401c9627646aa1b9e938.WireTo(id_294392c31a284f38abb8780073cc8dd7, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_294392c31a284f38abb8780073cc8dd7.WireTo(id_ad680c5d89a6414b83ad625a37b3cbab, "pairOutput"); /* {"SourceType":"Pair","SourceIsReference":false,"DestinationType":"ConvertToJProperty","DestinationIsReference":false} */
            id_e1d0d849589446a69d3b3542f54406c2.WireTo(id_9fd795adcaba47d8964f24ffe8e0474c, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_e1d0d849589446a69d3b3542f54406c2.WireTo(id_c7b2f89821134edcb8cd0bc25cbf80a7, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"DatePicker","DestinationIsReference":false} */
            id_c7b2f89821134edcb8cd0bc25cbf80a7.WireTo(dateSelectedConnector, "selectedDate"); /* {"SourceType":"DatePicker","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_ee103defd55941458ec83a507ce6f696.WireTo(dateSelectedConnector, "Item2"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_0072b66a9562483db25ee7675fee3a18.WireTo(dateSelectedConnector, "Item2"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_75c86eb2da5a4dcc89bdfd6cbaaaf552.WireTo(id_ee103defd55941458ec83a507ce6f696, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_ee103defd55941458ec83a507ce6f696.WireTo(id_f12593382f6646eab4ee39013b380ec2, "pairOutput"); /* {"SourceType":"Pair","SourceIsReference":false,"DestinationType":"ConvertToJProperty","DestinationIsReference":false} */
            id_f12593382f6646eab4ee39013b380ec2.WireTo(id_5544b1d1fc02448db83b43331a6f7569, "jPropertyOutput"); /* {"SourceType":"ConvertToJProperty","SourceIsReference":false,"DestinationType":"Collection","DestinationIsReference":false} */
            id_739acd6848914fdf913f1d84f191d65a.WireTo(id_0072b66a9562483db25ee7675fee3a18, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_0072b66a9562483db25ee7675fee3a18.WireTo(id_608f85c1386b4536bf8c8b1fe60322af, "pairOutput"); /* {"SourceType":"Pair","SourceIsReference":false,"DestinationType":"ConvertToJProperty","DestinationIsReference":false} */
            id_608f85c1386b4536bf8c8b1fe60322af.WireTo(naitAnimalTransferModelProperties, "jPropertyOutput"); /* {"SourceType":"ConvertToJProperty","SourceIsReference":false,"DestinationType":"Collection","DestinationIsReference":false} */
            naitReceivingMovementForm.WireTo(id_3ba80dc932cc4f938269acbca05d5495, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            naitReceivingMovementForm.WireTo(id_bc307cec648343a099ef75f02e3d4cc9, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            naitReceivingMovementForm.WireTo(id_ebad3d064a304a5b8353a88e016d151b, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            naitReceivingMovementForm.WireTo(id_1d90a04b3bca409b9713fe2c72eb868e, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_bc307cec648343a099ef75f02e3d4cc9.WireTo(id_57057f9a45d3495f9d3fff51026faa84, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_bc307cec648343a099ef75f02e3d4cc9.WireTo(id_657d14273506444f82de4d12b5851059, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            id_657d14273506444f82de4d12b5851059.WireTo(id_3a6518c31f9f41558a21bbc19c080881, "textOutput"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            id_ebad3d064a304a5b8353a88e016d151b.WireTo(id_df5b8eb15ae447cbba99838c68238360, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_ebad3d064a304a5b8353a88e016d151b.WireTo(id_1dae1c9cba6542e093d34cf2350916b4, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            id_1dae1c9cba6542e093d34cf2350916b4.WireTo(id_9fdcb5b5ae214ebcbf1d66898eb33caa, "textOutput"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            id_1d90a04b3bca409b9713fe2c72eb868e.WireTo(id_822acb4bc2684a5d9ad2a0966c707a9f, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_1d90a04b3bca409b9713fe2c72eb868e.WireTo(id_f5c2b9fd10284292964aebf2478e1acf, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"DatePicker","DestinationIsReference":false} */
            id_f5c2b9fd10284292964aebf2478e1acf.WireTo(dateSelectedConnector, "selectedDate"); /* {"SourceType":"DatePicker","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_cbf60ba3c5ab4a5a847e79019f216a01.WireTo(id_80e16a4e1d9b473c8814c0b2e92d279a, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_80e16a4e1d9b473c8814c0b2e92d279a.WireTo(id_9c5321d556bb44fe8ce5ea16b9887432, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_9c5321d556bb44fe8ce5ea16b9887432.WireTo(id_fde652396df84a70b6936a22a7d5db0d, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_9c5321d556bb44fe8ce5ea16b9887432.WireTo(naitGetJsonNaitRequestProperties, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_fde652396df84a70b6936a22a7d5db0d.WireTo(id_a1fae4aad2fd4eb588782da37eb678d5, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            naitGetJsonNaitRequestProperties.WireTo(id_491bb339b418484c94b0a0caa6b95204, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            naitGetJsonNaitRequestProperties.WireTo(id_8ef93747cd14440badbb642424b8a579, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_491bb339b418484c94b0a0caa6b95204.WireTo(naitSetPlaceholderValues, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_491bb339b418484c94b0a0caa6b95204.WireTo(id_d15c7648fe23487c9ae1ee732ca55295, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_491bb339b418484c94b0a0caa6b95204.WireTo(id_7746044f122f401c9627646aa1b9e938, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_491bb339b418484c94b0a0caa6b95204.WireTo(id_43e118675f474965956fac19cad8e1c8, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_491bb339b418484c94b0a0caa6b95204.WireTo(id_36c0be0f8c3e4b99af4956137c978a80, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_491bb339b418484c94b0a0caa6b95204.WireTo(id_80b50ee50c0f42148a32f273835a8d75, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_491bb339b418484c94b0a0caa6b95204.WireTo(id_635b56f1854b45d88592a25fb7f662d4, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_491bb339b418484c94b0a0caa6b95204.WireTo(id_75c86eb2da5a4dcc89bdfd6cbaaaf552, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_491bb339b418484c94b0a0caa6b95204.WireTo(id_739acd6848914fdf913f1d84f191d65a, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_8ef93747cd14440badbb642424b8a579.WireTo(naitGetJsonNaitRequest, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            naitGetJsonNaitRequest.WireTo(id_d5a14ef0a1454d139027164fb2bce628, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            naitGetJsonNaitRequest.WireTo(id_b72b6ec85d21411db0e57578d9f3bb04, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_d5a14ef0a1454d139027164fb2bce628.WireTo(id_5544b1d1fc02448db83b43331a6f7569, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Collection","DestinationIsReference":false} */
            id_b72b6ec85d21411db0e57578d9f3bb04.WireTo(naitGetJsonAnimalTransferModel, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            naitGetJsonAnimalTransferModel.WireTo(getTickedSessionsForEIDTransact, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Transact","DestinationIsReference":false} */
            naitGetJsonAnimalTransferModel.WireTo(id_707783bbfea74260b899228a2d2df55d, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            getTickedSessionsForEIDTransact.WireTo(sessionListGrid, "tableDataFlowSource"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"Grid","DestinationIsReference":true} */
            getTickedSessionsForEIDTransact.WireTo(getTickedSessionsForEIDFilter, "tableDataFlowDestination"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"Filter","DestinationIsReference":false} */
            getTickedSessionsForEIDTransact.WireTo(getTickedSessionsForEIDTransactComplete, "eventCompleteNoErrors"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            getTickedSessionsForEIDFilter.WireTo(id_ae91d1aec20f4fefa0ed594e75f0cb57, "tableDataFlow"); /* {"SourceType":"Filter","SourceIsReference":false,"DestinationType":"Table","DestinationIsReference":false} */
            id_ae91d1aec20f4fefa0ed594e75f0cb57.WireTo(id_a7cb9588be2f41e6b556905c531ee324, "dataTableOutput"); /* {"SourceType":"Table","SourceIsReference":false,"DestinationType":"DataFlowGate","DestinationIsReference":false} */
            id_a7cb9588be2f41e6b556905c531ee324.WireTo(id_4cb4fef33e2441f399841c04c3c51e55, "dataOutput"); /* {"SourceType":"DataFlowGate","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_4cb4fef33e2441f399841c04c3c51e55.WireTo(id_a1082346f50547bf81ef66f895201a36, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"ExportDataTableColumn","DestinationIsReference":false} */
            id_4cb4fef33e2441f399841c04c3c51e55.WireTo(id_487d74e91f93403abd6e99919ff77ca1, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"ExportDataTableColumn","DestinationIsReference":false} */
            id_a1082346f50547bf81ef66f895201a36.WireTo(eidSummaryLoop, "exportedColumn"); /* {"SourceType":"ExportDataTableColumn","SourceIsReference":false,"DestinationType":"Loop","DestinationIsReference":false} */
            eidSummaryLoop.WireTo(id_5d5379ea03b74bad9a00188103e65258, "nextValue"); /* {"SourceType":"Loop","SourceIsReference":false,"DestinationType":"Collection","DestinationIsReference":false} */
            eidSummaryLoop.WireTo(eidSummaryTransact, "loopComplete"); /* {"SourceType":"Loop","SourceIsReference":false,"DestinationType":"Transact","DestinationIsReference":false} */
            id_5d5379ea03b74bad9a00188103e65258.WireTo(noEIDTable, "listOutput"); /* {"SourceType":"Collection","SourceIsReference":false,"DestinationType":"Table","DestinationIsReference":false} */
            eidSummaryTransact.WireTo(noEIDTable, "tableDataFlowSource"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"Table","DestinationIsReference":false} */
            eidSummaryTransact.WireTo(id_48793bf0c1ab4eccb962da3a207f5294, "tableDataFlowDestination"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"Grid","DestinationIsReference":false} */
            eidSummaryTransact.WireTo(id_6fcc7feb31034e50be2eca33d2cfdcd8, "eventCompleteNoErrors"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_6fcc7feb31034e50be2eca33d2cfdcd8.WireTo(id_ba6ba3a68eae48cabc72ae95b9f58add, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_6fcc7feb31034e50be2eca33d2cfdcd8.WireTo(id_f66039bac47943a1865c046b3330c801, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_ba6ba3a68eae48cabc72ae95b9f58add.WireTo(id_48793bf0c1ab4eccb962da3a207f5294, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Grid","DestinationIsReference":false} */
            id_f66039bac47943a1865c046b3330c801.WireTo(id_08bbd1ff990f4a3dab79c98eabcd6fbb, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_f66039bac47943a1865c046b3330c801.WireTo(id_85c2c7872bd1485a8d44858c08fe8cb9, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_f66039bac47943a1865c046b3330c801.WireTo(id_fdd861eeb4364be792634a8195b2903a, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_85c2c7872bd1485a8d44858c08fe8cb9.WireTo(id_46bf30f48d0f43cb9b5752595152f9f8, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_46bf30f48d0f43cb9b5752595152f9f8.WireTo(id_1e364cc584874660b7c47ec55e3a9f08, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_46bf30f48d0f43cb9b5752595152f9f8.WireTo(id_eff4bf50999b4992985b91d784414c43, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_1e364cc584874660b7c47ec55e3a9f08.WireTo(id_6fcc7feb31034e50be2eca33d2cfdcd8, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_1e364cc584874660b7c47ec55e3a9f08.WireTo(noEIDTable, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Table","DestinationIsReference":false} */
            id_eff4bf50999b4992985b91d784414c43.WireTo(id_0cfa00c048684f4f8ee5eabc5ce84563, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_0cfa00c048684f4f8ee5eabc5ce84563.WireTo(isNaitRequest, "inputDataB"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_0cfa00c048684f4f8ee5eabc5ce84563.WireTo(id_bb43c755bbb94b408af6f22becf3741e, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"IfElse","DestinationIsReference":false} */
            id_bb43c755bbb94b408af6f22becf3741e.WireTo(naitSendTransactionTrue, "ifOutput"); /* {"SourceType":"IfElse","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_bb43c755bbb94b408af6f22becf3741e.WireTo(id_54b337bf303c4da0b9699c9e9ec75fe0, "elseOutput"); /* {"SourceType":"IfElse","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_54b337bf303c4da0b9699c9e9ec75fe0.WireTo(isNlisRequest, "inputDataB"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_54b337bf303c4da0b9699c9e9ec75fe0.WireTo(id_f5e3b298728247d7a8a3635566934336, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"IfElse","DestinationIsReference":false} */
            id_f5e3b298728247d7a8a3635566934336.WireTo(nlisSendTransactionTrue, "ifOutput"); /* {"SourceType":"IfElse","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_fdd861eeb4364be792634a8195b2903a.WireTo(id_d5f0bea66f2f4945bbf8106d1662a816, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_d5f0bea66f2f4945bbf8106d1662a816.WireTo(id_6dc8057461014705abd0a766af08ccbd, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_d5f0bea66f2f4945bbf8106d1662a816.WireTo(id_fd1c0559ccd644d88310eeb2316b1d74, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_6dc8057461014705abd0a766af08ccbd.WireTo(id_1e364cc584874660b7c47ec55e3a9f08, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_fd1c0559ccd644d88310eeb2316b1d74.WireTo(naitSendTransactionFalse, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_fd1c0559ccd644d88310eeb2316b1d74.WireTo(nlisSendTransactionFalse, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_487d74e91f93403abd6e99919ff77ca1.WireTo(id_ae5f2fc9d6294014baea6ef1adb5b6f8, "exportedColumn"); /* {"SourceType":"ExportDataTableColumn","SourceIsReference":false,"DestinationType":"Loop","DestinationIsReference":false} */
            id_ae5f2fc9d6294014baea6ef1adb5b6f8.WireTo(id_548730d776d34674b4975d55d2c64f07, "nextValue"); /* {"SourceType":"Loop","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_ae5f2fc9d6294014baea6ef1adb5b6f8.WireTo(id_e806d38f344f40e49a438207c0c15dc8, "loopComplete"); /* {"SourceType":"Loop","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_548730d776d34674b4975d55d2c64f07.WireTo(sessionDataScp, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"SessionDataSCP","DestinationIsReference":true} */
            id_548730d776d34674b4975d55d2c64f07.WireTo(id_6661ec50307749328dea851c9eea8a60, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"ConvertToEvent","DestinationIsReference":false} */
            id_6661ec50307749328dea851c9eea8a60.WireTo(getEIDsFromSessionDataSCPTransact, "eventOutput"); /* {"SourceType":"ConvertToEvent","SourceIsReference":false,"DestinationType":"Transact","DestinationIsReference":false} */
            getEIDsFromSessionDataSCPTransact.WireTo(sessionDataScp, "tableDataFlowSource"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"SessionDataSCP","DestinationIsReference":true} */
            getEIDsFromSessionDataSCPTransact.WireTo(id_303511ce02bd49b1ac0908ce8cd615d1, "tableDataFlowDestination"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"Select","DestinationIsReference":false} */
            id_303511ce02bd49b1ac0908ce8cd615d1.WireTo(eidTableDataFlowConnector, "tableDataFlow"); /* {"SourceType":"Select","SourceIsReference":false,"DestinationType":"TableDataFlowConnector","DestinationIsReference":false} */
            eidTableDataFlowConnector.WireTo(id_623f503fb93a4f0598ef5fd8c8ae8eca, "tableDataFlowList"); /* {"SourceType":"TableDataFlowConnector","SourceIsReference":false,"DestinationType":"Filter","DestinationIsReference":false} */
            eidTableDataFlowConnector.WireTo(id_d5367f6cab024f04961df458931896db, "tableDataFlowList"); /* {"SourceType":"TableDataFlowConnector","SourceIsReference":false,"DestinationType":"Filter","DestinationIsReference":false} */
            id_623f503fb93a4f0598ef5fd8c8ae8eca.WireTo(id_16cbe6db330843bbb5c408737d50daff, "dataFlowDataTableOutput"); /* {"SourceType":"Filter","SourceIsReference":false,"DestinationType":"Count","DestinationIsReference":false} */
            id_16cbe6db330843bbb5c408737d50daff.WireTo(id_5d5379ea03b74bad9a00188103e65258, "countOutput"); /* {"SourceType":"Count","SourceIsReference":false,"DestinationType":"Collection","DestinationIsReference":false} */
            id_d5367f6cab024f04961df458931896db.WireTo(id_82f7c31961ab4df09447c7a0293bd55f, "dataFlowDataTableOutput"); /* {"SourceType":"Filter","SourceIsReference":false,"DestinationType":"ExportDataTableColumn","DestinationIsReference":false} */
            id_82f7c31961ab4df09447c7a0293bd55f.WireTo(id_0a2ae499595c42dcb031e3fb5880d75e, "exportedColumn"); /* {"SourceType":"ExportDataTableColumn","SourceIsReference":false,"DestinationType":"UniqueCollection","DestinationIsReference":false} */
            id_0a2ae499595c42dcb031e3fb5880d75e.WireTo(id_2b788b8e0252466f9c03fcbb2052b63d, "outputAsList"); /* {"SourceType":"UniqueCollection","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_2b788b8e0252466f9c03fcbb2052b63d.WireTo(id_03aa7d025d164199a10d71221068655c, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"DataFlowGate","DestinationIsReference":false} */
            id_2b788b8e0252466f9c03fcbb2052b63d.WireTo(id_386b3e85562444f693c7fcade085a958, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"ConvertToEvent","DestinationIsReference":false} */
            id_03aa7d025d164199a10d71221068655c.WireTo(addSeparatorToEachEID, "dataOutput"); /* {"SourceType":"DataFlowGate","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            addSeparatorToEachEID.WireTo(tickedSessionEIDConnector, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            tickedSessionEIDConnector.WireTo(id_e2ed6bc6009c463cbc0159cc24cf8d97, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            id_e2ed6bc6009c463cbc0159cc24cf8d97.WireTo(id_103cf07bd2f945728611c0e2b8b7b34c, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"ConvertToJProperty","DestinationIsReference":false} */
            id_103cf07bd2f945728611c0e2b8b7b34c.WireTo(id_a1e0baa5d0cb4c208e6d711f1f3ee319, "jPropertyOutput"); /* {"SourceType":"ConvertToJProperty","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_a1e0baa5d0cb4c208e6d711f1f3ee319.WireTo(id_8992fcfc401344849f2b2e50f761c0b9, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_a1e0baa5d0cb4c208e6d711f1f3ee319.WireTo(signalStartNaitRequest, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_8992fcfc401344849f2b2e50f761c0b9.WireTo(naitAnimalTransferModelProperties, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Collection","DestinationIsReference":false} */
            signalStartNaitRequest.WireTo(id_a7d26ee500a14f4182bbd8064b73a7d3, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"ConvertToEvent","DestinationIsReference":false} */
            id_a7d26ee500a14f4182bbd8064b73a7d3.WireTo(naitSendTransactionEventGate, "eventOutput"); /* {"SourceType":"ConvertToEvent","SourceIsReference":false,"DestinationType":"EventGate","DestinationIsReference":false} */
            naitSendTransactionTrue.WireTo(naitSendTransactionEventGate, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"EventGate","DestinationIsReference":false} */
            naitSendTransactionFalse.WireTo(naitSendTransactionEventGate, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"EventGate","DestinationIsReference":false} */
            naitSendTransactionEventGate.WireTo(id_b108cb8668894806bdf6286cc065202f, "eventOutput"); /* {"SourceType":"EventGate","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_b108cb8668894806bdf6286cc065202f.WireTo(id_76039e3bdc1b4f7ba90e51383ea70209, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_b108cb8668894806bdf6286cc065202f.WireTo(id_fcb3a7bfc5ea4632964430a99ad31662, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_76039e3bdc1b4f7ba90e51383ea70209.WireTo(naitSendTransactionFalse, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_fcb3a7bfc5ea4632964430a99ad31662.WireTo(id_ca634d28d3834718a520df93c2ea74a1, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_386b3e85562444f693c7fcade085a958.WireTo(id_70b2e242030f4e0b9d9210b5c18cece7, "eventOutput"); /* {"SourceType":"ConvertToEvent","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_70b2e242030f4e0b9d9210b5c18cece7.WireTo(id_66bbdd38737645e4898eae7fa8e3efbf, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_70b2e242030f4e0b9d9210b5c18cece7.WireTo(id_10750c4b15304e9dbcade8918b743048, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_66bbdd38737645e4898eae7fa8e3efbf.WireTo(eidSummaryLoop, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Loop","DestinationIsReference":false} */
            id_10750c4b15304e9dbcade8918b743048.WireTo(id_ae5f2fc9d6294014baea6ef1adb5b6f8, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Loop","DestinationIsReference":false} */
            id_e806d38f344f40e49a438207c0c15dc8.WireTo(id_03aa7d025d164199a10d71221068655c, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"DataFlowGate","DestinationIsReference":false} */
            id_e806d38f344f40e49a438207c0c15dc8.WireTo(id_0a2ae499595c42dcb031e3fb5880d75e, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"UniqueCollection","DestinationIsReference":false} */
            getTickedSessionsForEIDTransactComplete.WireTo(id_a7cb9588be2f41e6b556905c531ee324, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"DataFlowGate","DestinationIsReference":false} */
            id_707783bbfea74260b899228a2d2df55d.WireTo(id_27df171a08874ecfa5ca05db59b118bb, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventGate","DestinationIsReference":false} */
            id_ca634d28d3834718a520df93c2ea74a1.WireTo(id_27df171a08874ecfa5ca05db59b118bb, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"EventGate","DestinationIsReference":false} */
            id_d09188984a2447e697aca79195b2c001.WireTo(id_27df171a08874ecfa5ca05db59b118bb, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"EventGate","DestinationIsReference":false} */
            id_27df171a08874ecfa5ca05db59b118bb.WireTo(id_67cf1514e8084b228835fbbb7b15e055, "eventOutput"); /* {"SourceType":"EventGate","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_67cf1514e8084b228835fbbb7b15e055.WireTo(id_fc31b76184c54389bc050d721f3dda66, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_67cf1514e8084b228835fbbb7b15e055.WireTo(MiHubWaitingWindow, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_67cf1514e8084b228835fbbb7b15e055.WireTo(naitHttpRequest, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"HttpRequest","DestinationIsReference":false} */
            id_67cf1514e8084b228835fbbb7b15e055.WireTo(id_c2be2a36ac0b4f869d794dfa2117022e, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_fc31b76184c54389bc050d721f3dda66.WireTo(naitAnimalTransferModelProperties, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Collection","DestinationIsReference":false} */
            id_fc31b76184c54389bc050d721f3dda66.WireTo(id_d09188984a2447e697aca79195b2c001, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            naitHttpRequest.WireTo(id_bc04d19f4ce045298c36eeec2858f1bb, "jsonData"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"HttpRequest","DestinationIsReference":false} */
            naitHttpRequest.WireTo(internetConnectionDetectedConnector, "sendRequestFlag"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"HttpRequest","DestinationIsReference":false} */
            naitHttpRequest.WireTo(integrationRequestResponseParser, "responseJsonOutput"); /* {"SourceType":"HttpRequest","SourceIsReference":false,"DestinationType":"JSONParser","DestinationIsReference":false} */
            naitHttpRequest.WireTo(showIntegrationRequestResultWindow, "taskComplete"); /* {"SourceType":"HttpRequest","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            integrationRequestResponseParser.WireTo(id_783756683f1e475bbced2c42ea44f2f8, "parsedOutput"); /* {"SourceType":"JSONParser","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_783756683f1e475bbced2c42ea44f2f8.WireTo(id_46ce5c4a4183496f8e7e5a7ab3fbaf4a, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"KeyValue","DestinationIsReference":false} */
            id_783756683f1e475bbced2c42ea44f2f8.WireTo(id_e530b6c894324de68ee59b13b15c1c10, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"KeyValue","DestinationIsReference":false} */
            id_783756683f1e475bbced2c42ea44f2f8.WireTo(id_8f72fc59cdc1482a80b7762e34770c05, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"KeyValue","DestinationIsReference":false} */
            id_783756683f1e475bbced2c42ea44f2f8.WireTo(id_2540b0e36b1a465689546bde6896b821, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"KeyValue","DestinationIsReference":false} */
            id_783756683f1e475bbced2c42ea44f2f8.WireTo(id_c88ce58c41064245beac0489a11a4dbf, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"KeyValue","DestinationIsReference":false} */
            id_46ce5c4a4183496f8e7e5a7ab3fbaf4a.WireTo(id_ab01b0f99fca4ba3aa5091a73134869a, "valueOutputs"); /* {"SourceType":"KeyValue","SourceIsReference":false,"DestinationType":"ToString","DestinationIsReference":false} */
            id_ab01b0f99fca4ba3aa5091a73134869a.WireTo(id_ffb2290fc0d34e5587097f4d26a4902b, "stringOutput"); /* {"SourceType":"ToString","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_ffb2290fc0d34e5587097f4d26a4902b.WireTo(id_2f76cca8bc6d47b1862934669812a67d, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Equals","DestinationIsReference":false} */
            id_2f76cca8bc6d47b1862934669812a67d.WireTo(id_a36652086c4446f9bc05e9a00c8c3177, "isEqual"); /* {"SourceType":"Equals","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_e530b6c894324de68ee59b13b15c1c10.WireTo(id_597c3bcc3a6140e495f1aaf1b250c315, "valueOutputs"); /* {"SourceType":"KeyValue","SourceIsReference":false,"DestinationType":"ToString","DestinationIsReference":false} */
            id_597c3bcc3a6140e495f1aaf1b250c315.WireTo(id_44f53a25c8f24a00a34eea6bc688098e, "stringOutput"); /* {"SourceType":"ToString","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_44f53a25c8f24a00a34eea6bc688098e.WireTo(id_a3e922c083d94162af14a93edf8b9cbc, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Equals","DestinationIsReference":false} */
            id_44f53a25c8f24a00a34eea6bc688098e.WireTo(id_fe9db59201c341c9946d7da494be3375, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_a3e922c083d94162af14a93edf8b9cbc.WireTo(id_8a773709e88a4658a6c0169bcc93b23a, "isEqual"); /* {"SourceType":"Equals","SourceIsReference":false,"DestinationType":"Not","DestinationIsReference":false} */
            id_8a773709e88a4658a6c0169bcc93b23a.WireTo(id_fe9db59201c341c9946d7da494be3375, "reversedInput"); /* {"SourceType":"Not","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_8f72fc59cdc1482a80b7762e34770c05.WireTo(id_af25bcde89414573b8aa029152d262a6, "valueOutputs"); /* {"SourceType":"KeyValue","SourceIsReference":false,"DestinationType":"ToString","DestinationIsReference":false} */
            id_af25bcde89414573b8aa029152d262a6.WireTo(id_1c12611bf82d4513b4a2435dc30e939b, "stringOutput"); /* {"SourceType":"ToString","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_1c12611bf82d4513b4a2435dc30e939b.WireTo(id_38eeb0ea8d0643bdba116ad7ad308e2e, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Equals","DestinationIsReference":false} */
            id_1c12611bf82d4513b4a2435dc30e939b.WireTo(id_3385d6c5aa0d4ac6ae4a93c192f930da, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_38eeb0ea8d0643bdba116ad7ad308e2e.WireTo(id_a84323d6b300407fb8c072138e780253, "isEqual"); /* {"SourceType":"Equals","SourceIsReference":false,"DestinationType":"Not","DestinationIsReference":false} */
            id_a84323d6b300407fb8c072138e780253.WireTo(id_3385d6c5aa0d4ac6ae4a93c192f930da, "reversedInput"); /* {"SourceType":"Not","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_2540b0e36b1a465689546bde6896b821.WireTo(id_ca11332e468b41ceb56d820cc5734240, "valueOutputs"); /* {"SourceType":"KeyValue","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            id_ca11332e468b41ceb56d820cc5734240.WireTo(id_6b1962873675446f8df04ed75a4b0561, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"ToString","DestinationIsReference":false} */
            id_6b1962873675446f8df04ed75a4b0561.WireTo(id_0e93cf0de4784e1a9d587d8776484791, "stringOutput"); /* {"SourceType":"ToString","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_0e93cf0de4784e1a9d587d8776484791.WireTo(id_badc02266e0149ffabbfddba818c486d, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Equals","DestinationIsReference":false} */
            id_0e93cf0de4784e1a9d587d8776484791.WireTo(id_5a9efc74583f4d288f319f3fde7f1514, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_badc02266e0149ffabbfddba818c486d.WireTo(id_de3a77bdfe514e4ca90c8190c75f5648, "isEqual"); /* {"SourceType":"Equals","SourceIsReference":false,"DestinationType":"Not","DestinationIsReference":false} */
            id_de3a77bdfe514e4ca90c8190c75f5648.WireTo(id_5a9efc74583f4d288f319f3fde7f1514, "reversedInput"); /* {"SourceType":"Not","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_c88ce58c41064245beac0489a11a4dbf.WireTo(id_b54c432fa4e842b9aa14e551595afcd2, "valueOutputs"); /* {"SourceType":"KeyValue","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            id_b54c432fa4e842b9aa14e551595afcd2.WireTo(id_ea6914aa996d437ab102906292d9b093, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"ToString","DestinationIsReference":false} */
            id_ea6914aa996d437ab102906292d9b093.WireTo(id_e7e61b98fbd048b9aabd8d672bc34c78, "stringOutput"); /* {"SourceType":"ToString","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_e7e61b98fbd048b9aabd8d672bc34c78.WireTo(id_784d11c852b446339537ea8baef1fff6, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Equals","DestinationIsReference":false} */
            id_e7e61b98fbd048b9aabd8d672bc34c78.WireTo(id_eceacd110a97449492a266eeb15bf3be, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_784d11c852b446339537ea8baef1fff6.WireTo(id_448b8a96f88f43d8b9737389f7ab4c38, "isEqual"); /* {"SourceType":"Equals","SourceIsReference":false,"DestinationType":"Not","DestinationIsReference":false} */
            id_448b8a96f88f43d8b9737389f7ab4c38.WireTo(id_eceacd110a97449492a266eeb15bf3be, "reversedInput"); /* {"SourceType":"Not","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            showIntegrationRequestResultWindow.WireTo(id_6a91653313cb4bf5a67728e7c8630789, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            showIntegrationRequestResultWindow.WireTo(id_2acca303266a4f2795a93ad706f29ba5, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            showIntegrationRequestResultWindow.WireTo(id_48b062128c014f31915c1fff53bce098, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_6a91653313cb4bf5a67728e7c8630789.WireTo(MiHubWaitingWindow, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_2acca303266a4f2795a93ad706f29ba5.WireTo(id_19148524293a42fb9919a298cf1689aa, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_19148524293a42fb9919a298cf1689aa.WireTo(id_6783d645206643be81efa7d1b55380b4, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_48b062128c014f31915c1fff53bce098.WireTo(id_a5211e05f4074ce693e8f681c6b2f704, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_48b062128c014f31915c1fff53bce098.WireTo(id_b6cdd489f7d7473993937a57c2084fef, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Vertical","DestinationIsReference":false} */
            id_a5211e05f4074ce693e8f681c6b2f704.WireTo(id_6783d645206643be81efa7d1b55380b4, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_b6cdd489f7d7473993937a57c2084fef.WireTo(id_a36652086c4446f9bc05e9a00c8c3177, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_b6cdd489f7d7473993937a57c2084fef.WireTo(id_fe9db59201c341c9946d7da494be3375, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_b6cdd489f7d7473993937a57c2084fef.WireTo(id_3385d6c5aa0d4ac6ae4a93c192f930da, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_b6cdd489f7d7473993937a57c2084fef.WireTo(id_5a9efc74583f4d288f319f3fde7f1514, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_b6cdd489f7d7473993937a57c2084fef.WireTo(id_eceacd110a97449492a266eeb15bf3be, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_a36652086c4446f9bc05e9a00c8c3177.WireTo(id_9af41d1bfa784ef5b987030af690f77d, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_a36652086c4446f9bc05e9a00c8c3177.WireTo(id_0d484d19bc0e432dbdd91f0f1cc14af4, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Picture","DestinationIsReference":false} */
            id_c2be2a36ac0b4f869d794dfa2117022e.WireTo(checkInternetConnection, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"HttpRequest","DestinationIsReference":false} */
            id_c2be2a36ac0b4f869d794dfa2117022e.WireTo(id_02b0997d1f9c43c0bde76049ae19c4fd, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_02b0997d1f9c43c0bde76049ae19c4fd.WireTo(internetConnectionDetectedConnector, "inputDataB"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_02b0997d1f9c43c0bde76049ae19c4fd.WireTo(id_87c830dc159140ef8a2569de4ae4ca7d, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"IfElse","DestinationIsReference":false} */
            id_87c830dc159140ef8a2569de4ae4ca7d.WireTo(id_5cda8733fd4b4c2bbb95956c76f63a69, "elseOutput"); /* {"SourceType":"IfElse","SourceIsReference":false,"DestinationType":"Timer","DestinationIsReference":false} */
            id_5cda8733fd4b4c2bbb95956c76f63a69.WireTo(id_c2be2a36ac0b4f869d794dfa2117022e, "eventHappened"); /* {"SourceType":"Timer","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            openNlisForm.WireTo(nlisLoginWindow, "eventOutput"); /* {"SourceType":"EventGate","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            nlisLoginWindow.WireTo(id_def2364a24374e1c88827983a708ac54, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            nlisLoginWindow.WireTo(id_0b15ca3952674b5c84b15dc80dce134f, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            nlisLoginWindow.WireTo(id_d87e3d86a09c4e54b3a296d146f0d9ed, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            nlisLoginWindow.WireTo(id_b0bc2f9bac5746259ce9d003a2e9a33e, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            nlisLoginWindow.WireTo(id_473ea818fe7a4629a7dffa02f6f20a27, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            nlisLoginWindow.WireTo(id_99bcacf4a6ff49ab931d1425c9f4ff26, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            nlisLoginWindow.WireTo(id_6b002e2f5d584cb787a9bbf74c0dd5a4, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            nlisLoginWindow.WireTo(id_31c41ea7451a42ff9bb479b6f5981030, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            nlisLoginWindow.WireTo(nlisAgentTransactionOptionbox, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            nlisLoginWindow.WireTo(nlisProducerTransactionOptionBox, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            nlisLoginWindow.WireTo(nlisSaleyardTransactionOptionBox, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            nlisLoginWindow.WireTo(nlisAgentOrProducerSendingVertical, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Vertical","DestinationIsReference":false} */
            nlisLoginWindow.WireTo(nlisAgentOrProducerReceivingVertical, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Vertical","DestinationIsReference":false} */
            nlisLoginWindow.WireTo(nlisAgentThirdPartyTransferVertical, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Vertical","DestinationIsReference":false} */
            nlisLoginWindow.WireTo(nlisToSaleyardVertical, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Vertical","DestinationIsReference":false} */
            nlisLoginWindow.WireTo(nlisFromSaleyardVertical, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Vertical","DestinationIsReference":false} */
            nlisLoginWindow.WireTo(id_5dba992efe104d1aab612d3536f0e5be, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_d87e3d86a09c4e54b3a296d146f0d9ed.WireTo(id_7b435b74036c4a64b9c0099e3455caf7, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_d87e3d86a09c4e54b3a296d146f0d9ed.WireTo(id_036af24010e24da7ae2ebf1aef0f8181, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            id_fccdd18f44be4215a75bc7695f01cae4.WireTo(id_036af24010e24da7ae2ebf1aef0f8181, "Item2"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_b0bc2f9bac5746259ce9d003a2e9a33e.WireTo(id_88e085d3641e4988b87f886216282a91, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_b0bc2f9bac5746259ce9d003a2e9a33e.WireTo(id_d6a12177c39f4ee5a72a91066485c49a, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"PasswordBox","DestinationIsReference":false} */
            id_b83ceebfdd854be295f2513d0af97887.WireTo(id_d6a12177c39f4ee5a72a91066485c49a, "Item2"); /* {"SourceType":"PasswordBox","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_473ea818fe7a4629a7dffa02f6f20a27.WireTo(id_6c4f74f3cea34eb78458b1fa29bca35d, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_473ea818fe7a4629a7dffa02f6f20a27.WireTo(id_ee1dbb0d58f046cdb08b363717032ff0, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            id_f0fe76cb8c654e2d995a6d46768f21fd.WireTo(id_ee1dbb0d58f046cdb08b363717032ff0, "Item2"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_99bcacf4a6ff49ab931d1425c9f4ff26.WireTo(id_f1d1b4ade6344ef4a701679e6af22de8, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_99bcacf4a6ff49ab931d1425c9f4ff26.WireTo(id_eb8fa1f57a224fadbc9380d351737a41, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            id_a1ffbb81d9b044d191ffad1e8c160a63.WireTo(id_eb8fa1f57a224fadbc9380d351737a41, "Item2"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_eb8fa1f57a224fadbc9380d351737a41.WireTo(id_d19d6642f58b4abaac2bdeb8cba5463a, "textOutput"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_ddbdc05bbf2142c2a7c9f9621591a8e8.WireTo(id_d19d6642f58b4abaac2bdeb8cba5463a, "Item2"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_7aa01a29dd9548ceb7e4fd7145d70f92.WireTo(id_ddbdc05bbf2142c2a7c9f9621591a8e8, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_f5d9bc0340b44f7f95bd6ac688c5d6cd.WireTo(id_ddbdc05bbf2142c2a7c9f9621591a8e8, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_2775b93b0a3343ddb9bde9f816634675.WireTo(id_7aa01a29dd9548ceb7e4fd7145d70f92, "ifOutput"); /* {"SourceType":"IfElse","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_683fc02136f7430082056fa06e14c37f.WireTo(id_f5d9bc0340b44f7f95bd6ac688c5d6cd, "ifOutput"); /* {"SourceType":"IfElse","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_6b002e2f5d584cb787a9bbf74c0dd5a4.WireTo(id_1130a8893290457297e177e787eb8076, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_6b002e2f5d584cb787a9bbf74c0dd5a4.WireTo(id_2189bf232d1d41ef913d50ff7071c291, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"OptionBox","DestinationIsReference":false} */
            id_2189bf232d1d41ef913d50ff7071c291.WireTo(nlisAccountTypeConnector, "dataFlowSelectionOutput"); /* {"SourceType":"OptionBox","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_2189bf232d1d41ef913d50ff7071c291.WireTo(id_06b3003a6e3e4d4a976e0e945e3fefa9, "children"); /* {"SourceType":"OptionBox","SourceIsReference":false,"DestinationType":"OptionBoxItem","DestinationIsReference":false} */
            id_2189bf232d1d41ef913d50ff7071c291.WireTo(id_4dc57149d5824468aca5028198df7e57, "children"); /* {"SourceType":"OptionBox","SourceIsReference":false,"DestinationType":"OptionBoxItem","DestinationIsReference":false} */
            id_2189bf232d1d41ef913d50ff7071c291.WireTo(id_0892202059054c0e86392d6e68776ea3, "children"); /* {"SourceType":"OptionBox","SourceIsReference":false,"DestinationType":"OptionBoxItem","DestinationIsReference":false} */
            id_2189bf232d1d41ef913d50ff7071c291.WireTo(id_f7253718f0d04c758f029f5530cb55f4, "children"); /* {"SourceType":"OptionBox","SourceIsReference":false,"DestinationType":"OptionBoxItem","DestinationIsReference":false} */
            nlisAccountTypeConnector.WireTo(id_934599e288894f098d384e455a13a960, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Equals","DestinationIsReference":false} */
            nlisAccountTypeConnector.WireTo(id_1a9a7707a5c3486981ac4b464aed203d, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Equals","DestinationIsReference":false} */
            nlisAccountTypeConnector.WireTo(id_9c3af4b947234c80932ae681c50b879b, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Equals","DestinationIsReference":false} */
            nlisAccountTypeConnector.WireTo(nlisTransactionTypeConnector, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_934599e288894f098d384e455a13a960.WireTo(nlisAgentTransactionTypeConnector, "isEqual"); /* {"SourceType":"Equals","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            nlisAgentTransactionTypeConnector.WireTo(id_66d04bd1dd3d448a9858e515f4652663, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"DataFlowGate","DestinationIsReference":false} */
            nlisAgentTransactionTypeConnector.WireTo(nlisAgentTransactionOptionbox, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_66d04bd1dd3d448a9858e515f4652663.WireTo(id_a05d136d60144a8e846b1a43202f2785, "dataOutput"); /* {"SourceType":"DataFlowGate","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            id_66d04bd1dd3d448a9858e515f4652663.WireTo(nlisAgentTransactionTypeConnector, "triggerLatchInput"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"DataFlowGate","DestinationIsReference":false} */
            id_a05d136d60144a8e846b1a43202f2785.WireTo(nlisMovementType, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            nlisMovementType.WireTo(id_a0b304fbaeba48888b79b1eb90322ae8, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"AsObject","DestinationIsReference":false} */
            id_a0b304fbaeba48888b79b1eb90322ae8.WireTo(id_d7b4bee64cae4f8dbe9116e48bcf6643, "outputAsJToken"); /* {"SourceType":"AsObject","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_7df06b94af2a4440b7914856430a2bec.WireTo(id_d7b4bee64cae4f8dbe9116e48bcf6643, "Item2"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_1a9a7707a5c3486981ac4b464aed203d.WireTo(nlisProducerTransactionTypeConnector, "isEqual"); /* {"SourceType":"Equals","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            nlisProducerTransactionTypeConnector.WireTo(id_3744db3fbed141de831911d1860b8f23, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"DataFlowGate","DestinationIsReference":false} */
            nlisProducerTransactionTypeConnector.WireTo(nlisProducerTransactionOptionBox, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_3744db3fbed141de831911d1860b8f23.WireTo(id_a4b61b22030841e7a46e64ad26da520a, "dataOutput"); /* {"SourceType":"DataFlowGate","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            id_3744db3fbed141de831911d1860b8f23.WireTo(nlisProducerTransactionTypeConnector, "triggerLatchInput"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"DataFlowGate","DestinationIsReference":false} */
            id_a4b61b22030841e7a46e64ad26da520a.WireTo(nlisMovementType, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_9c3af4b947234c80932ae681c50b879b.WireTo(nlisSaleyardTransactionTypeConnector, "isEqual"); /* {"SourceType":"Equals","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            nlisSaleyardTransactionTypeConnector.WireTo(nlisSaleyardTransactionOptionBox, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            nlisAgentTransactionOptionbox.WireTo(id_47760a5c942b4ca28a4b2c42d6e22cf6, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            nlisAgentTransactionOptionbox.WireTo(id_85cbd2e4532b43bf8967b1bfb03dec7c, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"OptionBox","DestinationIsReference":false} */
            id_85cbd2e4532b43bf8967b1bfb03dec7c.WireTo(nlisTransactionTypeConnector, "dataFlowSelectionOutput"); /* {"SourceType":"OptionBox","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_85cbd2e4532b43bf8967b1bfb03dec7c.WireTo(id_6df343b06c064f1b9dd681b5cd97bbce, "children"); /* {"SourceType":"OptionBox","SourceIsReference":false,"DestinationType":"OptionBoxItem","DestinationIsReference":false} */
            id_85cbd2e4532b43bf8967b1bfb03dec7c.WireTo(id_266167db72bd40749fd6c53fbcee7be2, "children"); /* {"SourceType":"OptionBox","SourceIsReference":false,"DestinationType":"OptionBoxItem","DestinationIsReference":false} */
            id_85cbd2e4532b43bf8967b1bfb03dec7c.WireTo(id_d30558d6137344ad9b3b8169a1097151, "children"); /* {"SourceType":"OptionBox","SourceIsReference":false,"DestinationType":"OptionBoxItem","DestinationIsReference":false} */
            id_85cbd2e4532b43bf8967b1bfb03dec7c.WireTo(id_c863ad350ac94c63bf14678db5605cc6, "children"); /* {"SourceType":"OptionBox","SourceIsReference":false,"DestinationType":"OptionBoxItem","DestinationIsReference":false} */
            nlisTransactionTypeConnector.WireTo(id_fc22728ea3374db997b3118a4c1b29e7, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Equals","DestinationIsReference":false} */
            nlisTransactionTypeConnector.WireTo(id_fddb3c9c8df241d5acc803a92bfd52b4, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Equals","DestinationIsReference":false} */
            nlisTransactionTypeConnector.WireTo(id_22af63e4247f4c5ab67354542c72a081, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Equals","DestinationIsReference":false} */
            nlisTransactionTypeConnector.WireTo(id_e4c7565291744b6fa329498e79114986, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Equals","DestinationIsReference":false} */
            nlisTransactionTypeConnector.WireTo(id_7bc3718dafb1425aa3782a71820c5fb8, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Equals","DestinationIsReference":false} */
            id_fc22728ea3374db997b3118a4c1b29e7.WireTo(id_e6372bee98b6470bb3fc834a30a5e850, "isEqual"); /* {"SourceType":"Equals","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_e6372bee98b6470bb3fc834a30a5e850.WireTo(nlisAgentOrProducerSendingVertical, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Vertical","DestinationIsReference":false} */
            id_e6372bee98b6470bb3fc834a30a5e850.WireTo(id_683fc02136f7430082056fa06e14c37f, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"IfElse","DestinationIsReference":false} */
            id_fddb3c9c8df241d5acc803a92bfd52b4.WireTo(id_d21ae554d46744d0a710090c4674582e, "isEqual"); /* {"SourceType":"Equals","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_d21ae554d46744d0a710090c4674582e.WireTo(nlisAgentOrProducerReceivingVertical, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Vertical","DestinationIsReference":false} */
            id_d21ae554d46744d0a710090c4674582e.WireTo(id_2775b93b0a3343ddb9bde9f816634675, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"IfElse","DestinationIsReference":false} */
            id_22af63e4247f4c5ab67354542c72a081.WireTo(id_81d9b9d1bf5f4352ac224751c8984754, "isEqual"); /* {"SourceType":"Equals","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_81d9b9d1bf5f4352ac224751c8984754.WireTo(id_1f01e51edbfa42468753b023959726fb, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"DataFlowGate","DestinationIsReference":false} */
            id_81d9b9d1bf5f4352ac224751c8984754.WireTo(nlisAgentThirdPartyTransferVertical, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Vertical","DestinationIsReference":false} */
            id_1f01e51edbfa42468753b023959726fb.WireTo(id_050da19fe54e4c4ca477b8a6257db32e, "dataOutput"); /* {"SourceType":"DataFlowGate","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            id_1f01e51edbfa42468753b023959726fb.WireTo(id_81d9b9d1bf5f4352ac224751c8984754, "triggerLatchInput"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"DataFlowGate","DestinationIsReference":false} */
            id_050da19fe54e4c4ca477b8a6257db32e.WireTo(nlisMovementType, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_e4c7565291744b6fa329498e79114986.WireTo(id_7423dde774de4cc891a9b13290dad2f4, "isEqual"); /* {"SourceType":"Equals","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_7423dde774de4cc891a9b13290dad2f4.WireTo(id_db1379daf87c4cab92c23fcf2804e022, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"DataFlowGate","DestinationIsReference":false} */
            id_7423dde774de4cc891a9b13290dad2f4.WireTo(nlisToSaleyardVertical, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Vertical","DestinationIsReference":false} */
            id_db1379daf87c4cab92c23fcf2804e022.WireTo(id_b245cda27f704651a8cd1babea5456e4, "dataOutput"); /* {"SourceType":"DataFlowGate","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            id_db1379daf87c4cab92c23fcf2804e022.WireTo(id_7423dde774de4cc891a9b13290dad2f4, "triggerLatchInput"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"DataFlowGate","DestinationIsReference":false} */
            id_b245cda27f704651a8cd1babea5456e4.WireTo(nlisMovementType, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_7bc3718dafb1425aa3782a71820c5fb8.WireTo(id_16a4e083c9e44516b12b97334e764e5f, "isEqual"); /* {"SourceType":"Equals","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_16a4e083c9e44516b12b97334e764e5f.WireTo(id_7010644277974849b00f068bed0dec09, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"DataFlowGate","DestinationIsReference":false} */
            id_16a4e083c9e44516b12b97334e764e5f.WireTo(nlisFromSaleyardVertical, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Vertical","DestinationIsReference":false} */
            id_7010644277974849b00f068bed0dec09.WireTo(id_9dbe1dbf08f54712aae5645365775421, "dataOutput"); /* {"SourceType":"DataFlowGate","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            id_7010644277974849b00f068bed0dec09.WireTo(id_16a4e083c9e44516b12b97334e764e5f, "triggerLatchInput"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"DataFlowGate","DestinationIsReference":false} */
            id_9dbe1dbf08f54712aae5645365775421.WireTo(nlisMovementType, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            nlisProducerTransactionOptionBox.WireTo(id_9315eaef4e0f4a37bf2e823dcae53def, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            nlisProducerTransactionOptionBox.WireTo(id_73ba94ac97a541a496540b545466204a, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"OptionBox","DestinationIsReference":false} */
            id_73ba94ac97a541a496540b545466204a.WireTo(nlisTransactionTypeConnector, "dataFlowSelectionOutput"); /* {"SourceType":"OptionBox","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_73ba94ac97a541a496540b545466204a.WireTo(id_77634f1da61649ac92923ddedcdba8e0, "children"); /* {"SourceType":"OptionBox","SourceIsReference":false,"DestinationType":"OptionBoxItem","DestinationIsReference":false} */
            id_73ba94ac97a541a496540b545466204a.WireTo(id_80c109ef616b4d9291ea1aeae263dc67, "children"); /* {"SourceType":"OptionBox","SourceIsReference":false,"DestinationType":"OptionBoxItem","DestinationIsReference":false} */
            nlisSaleyardTransactionOptionBox.WireTo(id_5e944b71c60246489b53162cf905c84c, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            nlisSaleyardTransactionOptionBox.WireTo(id_b237b2d63a884f20970975afb770f663, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"OptionBox","DestinationIsReference":false} */
            id_b237b2d63a884f20970975afb770f663.WireTo(nlisTransactionTypeConnector, "dataFlowSelectionOutput"); /* {"SourceType":"OptionBox","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_b237b2d63a884f20970975afb770f663.WireTo(id_61ac04e7fadb4a4a80f7abbe1f3c561f, "children"); /* {"SourceType":"OptionBox","SourceIsReference":false,"DestinationType":"OptionBoxItem","DestinationIsReference":false} */
            id_b237b2d63a884f20970975afb770f663.WireTo(id_b19b64cd4422486a8f2dd309d10646e8, "children"); /* {"SourceType":"OptionBox","SourceIsReference":false,"DestinationType":"OptionBoxItem","DestinationIsReference":false} */
            nlisAgentOrProducerSendingVertical.WireTo(id_2a2fec1f9a2845039336a435a0fd0468, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            nlisAgentOrProducerSendingVertical.WireTo(id_7adc18c3e8bd45a3a7c066b04f8bd76b, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            nlisAgentOrProducerSendingVertical.WireTo(id_6204c104c66c491a95de9f27e63c7b03, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            nlisAgentOrProducerSendingVertical.WireTo(id_64df3c3c48964e39a2d168b5926bfbb5, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_2a2fec1f9a2845039336a435a0fd0468.WireTo(id_8cd89ef3210b4737aaebcf5f67e33f8b, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_2a2fec1f9a2845039336a435a0fd0468.WireTo(id_4450fe81dfa2476886acc75395ee4b55, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            id_f7a12fdb5ffc407898985df3b703dcd7.WireTo(id_4450fe81dfa2476886acc75395ee4b55, "Item2"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_4de4af0dd8854b7385bd8f1c0e20aa3b.WireTo(id_4450fe81dfa2476886acc75395ee4b55, "Item2"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_7adc18c3e8bd45a3a7c066b04f8bd76b.WireTo(id_95755a8171f049c1ba33611cdc02cae9, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_7adc18c3e8bd45a3a7c066b04f8bd76b.WireTo(id_f2b795308c71450fbdf3e5f4d23a99bb, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            id_7af94faf1ebe411d84913a53599b70f6.WireTo(id_f2b795308c71450fbdf3e5f4d23a99bb, "Item2"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_6204c104c66c491a95de9f27e63c7b03.WireTo(id_cd304edc595341d2bb09c89947e252cf, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_6204c104c66c491a95de9f27e63c7b03.WireTo(id_a60303b5a64c4911a8b9fd34f113aafa, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            id_b94a5b5ba544412b99e1021e7382e4fc.WireTo(id_a60303b5a64c4911a8b9fd34f113aafa, "Item2"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_a60303b5a64c4911a8b9fd34f113aafa.WireTo(id_686904aaf1da4c979e70ef750bff9284, "textOutput"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            id_64df3c3c48964e39a2d168b5926bfbb5.WireTo(id_a881f85f8e5447af9307c6ea24534035, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_64df3c3c48964e39a2d168b5926bfbb5.WireTo(id_7f94e8ce322b493a863fc4a236dec07b, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"DatePicker","DestinationIsReference":false} */
            id_7f94e8ce322b493a863fc4a236dec07b.WireTo(id_22a1f1c6e9b6456b80a529ca738db654, "selectedDate"); /* {"SourceType":"DatePicker","SourceIsReference":false,"DestinationType":"AsObject","DestinationIsReference":false} */
            id_22a1f1c6e9b6456b80a529ca738db654.WireTo(id_6ec24c3656714fdeb9566c55140e5578, "outputAsJToken"); /* {"SourceType":"AsObject","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_f399f412e087413c91a8e3cfd1b674c1.WireTo(id_6ec24c3656714fdeb9566c55140e5578, "Item2"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_014b61a91c364ef0a5b208c15cefb5de.WireTo(id_6ec24c3656714fdeb9566c55140e5578, "Item2"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            nlisAgentOrProducerReceivingVertical.WireTo(id_1a094b4fe9164d5cbe21e8473f0b88f1, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            nlisAgentOrProducerReceivingVertical.WireTo(id_f77cfa63e3d64602b39e0f4d9a22b90e, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            nlisAgentOrProducerReceivingVertical.WireTo(id_fe6a9da6596448099b556ef2b0baf606, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            nlisAgentOrProducerReceivingVertical.WireTo(id_bb3a7a570457420aa5b9c06d5ab99ff5, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_1a094b4fe9164d5cbe21e8473f0b88f1.WireTo(id_c84a91cf1346471698cffb077b5f7cdc, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_1a094b4fe9164d5cbe21e8473f0b88f1.WireTo(id_2dbc0ce08d794b2e8a7f7ef076ec98db, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            id_14d0c11ef4fc48f9af63b3b2b8168346.WireTo(id_2dbc0ce08d794b2e8a7f7ef076ec98db, "Item2"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_bdd4d8c336b34fbc83a6af9249625d12.WireTo(id_2dbc0ce08d794b2e8a7f7ef076ec98db, "Item2"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_f77cfa63e3d64602b39e0f4d9a22b90e.WireTo(id_39dfdbd7d9814b008c99aa0bd087940c, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_f77cfa63e3d64602b39e0f4d9a22b90e.WireTo(id_7779793f380549deb7e33196c6827bf8, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            id_8e0de3cb26684ebb99c0862618bb750a.WireTo(id_7779793f380549deb7e33196c6827bf8, "Item2"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_fe6a9da6596448099b556ef2b0baf606.WireTo(id_42b9b766cdb34a2c8c13d6a17b020cff, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_fe6a9da6596448099b556ef2b0baf606.WireTo(id_686904aaf1da4c979e70ef750bff9284, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            id_b7061a02ae5342beb54d8b0e3df30470.WireTo(id_686904aaf1da4c979e70ef750bff9284, "Item2"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_686904aaf1da4c979e70ef750bff9284.WireTo(id_a60303b5a64c4911a8b9fd34f113aafa, "textOutput"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            id_bb3a7a570457420aa5b9c06d5ab99ff5.WireTo(id_36e9888e3ea84322a8e8ab4c6ddbaceb, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_bb3a7a570457420aa5b9c06d5ab99ff5.WireTo(id_a0783ff3db5648a3b344e0a0fa91db9e, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"DatePicker","DestinationIsReference":false} */
            id_a0783ff3db5648a3b344e0a0fa91db9e.WireTo(id_61de43b3043a49009a2b8a05e231e633, "selectedDate"); /* {"SourceType":"DatePicker","SourceIsReference":false,"DestinationType":"AsObject","DestinationIsReference":false} */
            id_61de43b3043a49009a2b8a05e231e633.WireTo(id_fdfe07fac9944b2fa0032b31ab38ccb1, "outputAsJToken"); /* {"SourceType":"AsObject","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_9a233d8e1b6f4752804859ad25aca2a1.WireTo(id_fdfe07fac9944b2fa0032b31ab38ccb1, "Item2"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_e0a7320958984ff2a6bc26c6722a1040.WireTo(id_fdfe07fac9944b2fa0032b31ab38ccb1, "Item2"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            nlisAgentThirdPartyTransferVertical.WireTo(id_83e6183188e14047be5921ca96a7e670, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            nlisAgentThirdPartyTransferVertical.WireTo(id_5ae03381ed914c5dafde453e4446e88e, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            nlisAgentThirdPartyTransferVertical.WireTo(id_6bfa857fac2a402597a9c277da666064, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            nlisAgentThirdPartyTransferVertical.WireTo(id_2195c20f51d6485993d73294b9906051, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            nlisAgentThirdPartyTransferVertical.WireTo(id_94253900dd1940ea8dfe2756ef97e88d, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            nlisAgentThirdPartyTransferVertical.WireTo(id_321bf0892f2e404b87e7a76ef35ca2b1, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            nlisAgentThirdPartyTransferVertical.WireTo(id_8a9162efa4ce4d86860d5f72e04aabe8, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            nlisAgentThirdPartyTransferVertical.WireTo(id_639560130886407fb5467cb4377f094d, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            nlisAgentThirdPartyTransferVertical.WireTo(id_5ef32b7b278b4b8386790ced9d754e42, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            nlisAgentThirdPartyTransferVertical.WireTo(id_e6dc9510f02e4307ac5dbfa69ddf79eb, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_83e6183188e14047be5921ca96a7e670.WireTo(id_ec118657a3fe4cb0a10d1159f219d95a, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_83e6183188e14047be5921ca96a7e670.WireTo(id_688bec5c64f043c0bf584a00b34001ab, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            id_0b0f29eaabca462cacbd15d2fa64d11e.WireTo(id_688bec5c64f043c0bf584a00b34001ab, "Item2"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_5ae03381ed914c5dafde453e4446e88e.WireTo(id_44e9b2a1f7af4e5e9ee86cea71eac16f, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_5ae03381ed914c5dafde453e4446e88e.WireTo(id_0876c45a502f4edcbae1a297b7674987, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            id_67ce72bd41ff40edb712a22231e55bca.WireTo(id_0876c45a502f4edcbae1a297b7674987, "Item2"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_9fe5a772d2c147829fa1abc8c4519c38.WireTo(id_0876c45a502f4edcbae1a297b7674987, "Item2"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_6bfa857fac2a402597a9c277da666064.WireTo(id_20a58a62d5c64a3da4631dcd0436f743, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_6bfa857fac2a402597a9c277da666064.WireTo(id_fc9c19b650e842c880d61d14fa13e7d0, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            id_5aead95e76514f8991de08769de4760a.WireTo(id_fc9c19b650e842c880d61d14fa13e7d0, "Item2"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_2195c20f51d6485993d73294b9906051.WireTo(id_190ecb29ce0e496f9d6f08413ed9e875, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_2195c20f51d6485993d73294b9906051.WireTo(id_110c4920ce014c1f80a2c03684cd6423, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            id_2fe9bf09ed0742958f031075a4c4c08e.WireTo(id_110c4920ce014c1f80a2c03684cd6423, "Item2"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_94253900dd1940ea8dfe2756ef97e88d.WireTo(id_3e6ff7a7277e4a2f8b84c3ccb5fa7519, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_94253900dd1940ea8dfe2756ef97e88d.WireTo(id_b79e7486f476446b9994ceff04e551f0, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"DatePicker","DestinationIsReference":false} */
            id_b79e7486f476446b9994ceff04e551f0.WireTo(id_bae2e4eb948b418b833067670949a658, "selectedDate"); /* {"SourceType":"DatePicker","SourceIsReference":false,"DestinationType":"AsObject","DestinationIsReference":false} */
            id_bae2e4eb948b418b833067670949a658.WireTo(id_d6e883a6607748c38cb9b50a6e6eef71, "outputAsJToken"); /* {"SourceType":"AsObject","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_7ff3876c4c714cd291b4f09ed396e468.WireTo(id_d6e883a6607748c38cb9b50a6e6eef71, "Item2"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_41c7a8a6f50e42c8a0daa93a6904c0cc.WireTo(id_d6e883a6607748c38cb9b50a6e6eef71, "Item2"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_8a9162efa4ce4d86860d5f72e04aabe8.WireTo(id_1eaf59def17d4fbc9f2dd626b5db0e23, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_8a9162efa4ce4d86860d5f72e04aabe8.WireTo(id_336063780bde46e7a066dff96fd6ac31, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"OptionBox","DestinationIsReference":false} */
            id_336063780bde46e7a066dff96fd6ac31.WireTo(id_38e8b9d6ecda42619e088c892acf96dd, "dataFlowSelectionOutput"); /* {"SourceType":"OptionBox","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_336063780bde46e7a066dff96fd6ac31.WireTo(id_95c078ea9e4e444dbf31a7356853d223, "children"); /* {"SourceType":"OptionBox","SourceIsReference":false,"DestinationType":"OptionBoxItem","DestinationIsReference":false} */
            id_336063780bde46e7a066dff96fd6ac31.WireTo(id_b487e314581745b0addf24d9ea714d67, "children"); /* {"SourceType":"OptionBox","SourceIsReference":false,"DestinationType":"OptionBoxItem","DestinationIsReference":false} */
            id_336063780bde46e7a066dff96fd6ac31.WireTo(id_d497b23195524132ba89ef21422901e8, "children"); /* {"SourceType":"OptionBox","SourceIsReference":false,"DestinationType":"OptionBoxItem","DestinationIsReference":false} */
            id_336063780bde46e7a066dff96fd6ac31.WireTo(id_74e348ddb9ad450ebbb1742f6b46a6e7, "children"); /* {"SourceType":"OptionBox","SourceIsReference":false,"DestinationType":"OptionBoxItem","DestinationIsReference":false} */
            id_336063780bde46e7a066dff96fd6ac31.WireTo(id_7fa1036605c74d778c7f2e49cce92ddf, "children"); /* {"SourceType":"OptionBox","SourceIsReference":false,"DestinationType":"OptionBoxItem","DestinationIsReference":false} */
            id_38e8b9d6ecda42619e088c892acf96dd.WireTo(id_2559a20025694a6c8f139120e2402147, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"KeyValue","DestinationIsReference":false} */
            id_2559a20025694a6c8f139120e2402147.WireTo(id_d44743eceb264b98a0f83b41c854ff66, "valueOutputs"); /* {"SourceType":"KeyValue","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_d44743eceb264b98a0f83b41c854ff66.WireTo(id_3c2467569f454be7b349c67f3a5c1b47, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"AsObject","DestinationIsReference":false} */
            id_3c2467569f454be7b349c67f3a5c1b47.WireTo(id_09692c5f6cef45b3a2f19016ed3d6da1, "outputAsJToken"); /* {"SourceType":"AsObject","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_ddc6d323e71d4c248e03b4dfae20bf89.WireTo(id_09692c5f6cef45b3a2f19016ed3d6da1, "Item2"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_639560130886407fb5467cb4377f094d.WireTo(id_fb3f38bfc561419ca502c83d0c1e3cfa, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_639560130886407fb5467cb4377f094d.WireTo(id_bd895bfb66b3426c8cdb129dc677d153, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            id_6443b9839bb4423188f60bf081febcba.WireTo(id_bd895bfb66b3426c8cdb129dc677d153, "Item2"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_5ef32b7b278b4b8386790ced9d754e42.WireTo(id_5e081e4d9d034e77bfa883b99d51fe9e, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_5ef32b7b278b4b8386790ced9d754e42.WireTo(id_d971accd393f49c7a7165714e3739ce6, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            id_15139c45fde04f8d80975026955d61c7.WireTo(id_d971accd393f49c7a7165714e3739ce6, "Item2"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_e6dc9510f02e4307ac5dbfa69ddf79eb.WireTo(id_2e3877912e8445e08363641337958174, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"CheckBox","DestinationIsReference":false} */
            id_e6dc9510f02e4307ac5dbfa69ddf79eb.WireTo(id_3ba27c164bc641808ce69f70855ac852, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Vertical","DestinationIsReference":false} */
            id_2e3877912e8445e08363641337958174.WireTo(id_bd1f8ae26e3140bea6143d59150977f9, "isChecked"); /* {"SourceType":"CheckBox","SourceIsReference":false,"DestinationType":"AsObject","DestinationIsReference":false} */
            id_bd1f8ae26e3140bea6143d59150977f9.WireTo(id_1411bc583a074c2e9901c84a80dc235d, "outputAsJToken"); /* {"SourceType":"AsObject","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_c0e910de5fa346ae94734210f24755d6.WireTo(id_1411bc583a074c2e9901c84a80dc235d, "Item2"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_3ba27c164bc641808ce69f70855ac852.WireTo(id_c464b32436d3472b94a4f46724353bc8, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_3ba27c164bc641808ce69f70855ac852.WireTo(id_ab592c073f574b64b445d2122b568ee8, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_3ba27c164bc641808ce69f70855ac852.WireTo(id_5e99338c5c124d92a9275697299babac, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_3ba27c164bc641808ce69f70855ac852.WireTo(id_e8d17d5366a54e299ff416b362199e09, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            nlisToSaleyardVertical.WireTo(id_998bc95c9d35482db1cf8dc3c042f084, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            nlisToSaleyardVertical.WireTo(id_614ea4fb0f834dc883f207ed7624d563, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            nlisToSaleyardVertical.WireTo(id_66d7b3d9edd04f538230869f755f8be3, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            nlisToSaleyardVertical.WireTo(id_e01ac7d1e3ed4d4aa9882c62d047477d, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_998bc95c9d35482db1cf8dc3c042f084.WireTo(id_417a85af281b473b8aea1f1e9f50c220, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_998bc95c9d35482db1cf8dc3c042f084.WireTo(id_6bb3fb9231b04d07a8ec91506f8a8570, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            id_3b5f6a8060de4091914af93eb36e5d88.WireTo(id_6bb3fb9231b04d07a8ec91506f8a8570, "Item2"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_03672039bece413e9e5980def681fe84.WireTo(id_6bb3fb9231b04d07a8ec91506f8a8570, "Item2"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_614ea4fb0f834dc883f207ed7624d563.WireTo(id_cc92467bca7e4115b2c9cb49a94413e5, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_614ea4fb0f834dc883f207ed7624d563.WireTo(id_71140963662240209c3f6c4670bd075e, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            id_0b31a771a39247b687f3126a6f5e7548.WireTo(id_71140963662240209c3f6c4670bd075e, "Item2"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_66d7b3d9edd04f538230869f755f8be3.WireTo(id_e3850f2254344069b17a5539b5875948, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_66d7b3d9edd04f538230869f755f8be3.WireTo(id_01ce06f1ee2e43fa92bee221e2e4ffd6, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            id_71bca7d0003f4225b1ba2342a172dba7.WireTo(id_01ce06f1ee2e43fa92bee221e2e4ffd6, "Item2"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_e01ac7d1e3ed4d4aa9882c62d047477d.WireTo(id_829bda95b61242e5b0da39592b161fb6, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_e01ac7d1e3ed4d4aa9882c62d047477d.WireTo(id_392f879b8a2c4ed3af984832834db5ae, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"DatePicker","DestinationIsReference":false} */
            id_392f879b8a2c4ed3af984832834db5ae.WireTo(id_e3bd583f2dec45c5b1d37f6c22d9ded2, "selectedDate"); /* {"SourceType":"DatePicker","SourceIsReference":false,"DestinationType":"AsObject","DestinationIsReference":false} */
            id_e3bd583f2dec45c5b1d37f6c22d9ded2.WireTo(id_4231fb26b0384f19939340faedd53766, "outputAsJToken"); /* {"SourceType":"AsObject","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_76964d73ab16452faf22efb04ef4ad3e.WireTo(id_4231fb26b0384f19939340faedd53766, "Item2"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_697de7541ee74c1790a53ab5c654da54.WireTo(id_4231fb26b0384f19939340faedd53766, "Item2"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            nlisFromSaleyardVertical.WireTo(id_b7b9e353a8c54692b3983629f1648c3e, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            nlisFromSaleyardVertical.WireTo(id_b8f160c9b5f84d45bf34c4b9adcaf9fe, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            nlisFromSaleyardVertical.WireTo(id_f68fc132f97b436abf905b5a8b76e2f9, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            nlisFromSaleyardVertical.WireTo(id_98dab0214b0d4d3c8794321adb485b85, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_b7b9e353a8c54692b3983629f1648c3e.WireTo(id_f1475d65b1b64c4f812c4a5c3cf4b957, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_b7b9e353a8c54692b3983629f1648c3e.WireTo(id_636eeea6e82f45819b9384063587b8ed, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            id_8292fb4e95874a65a80709eadaab9cf7.WireTo(id_636eeea6e82f45819b9384063587b8ed, "Item2"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_a436f243d37d472795bb765052517fb7.WireTo(id_636eeea6e82f45819b9384063587b8ed, "Item2"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_b8f160c9b5f84d45bf34c4b9adcaf9fe.WireTo(id_5ae8c8b17cc440eab5a4b7d39a8ad492, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_b8f160c9b5f84d45bf34c4b9adcaf9fe.WireTo(id_893587f778e749ee99db0aa4872c199f, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            id_6774c022a717401d803bd9afb1ea894d.WireTo(id_893587f778e749ee99db0aa4872c199f, "Item2"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_f68fc132f97b436abf905b5a8b76e2f9.WireTo(id_ddcde050ae314943bf9104b14bdff335, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_f68fc132f97b436abf905b5a8b76e2f9.WireTo(id_d16421a67a454618b7e9fca13205cecd, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            id_8113f9d89f5345ffb6c9abcb89949097.WireTo(id_d16421a67a454618b7e9fca13205cecd, "Item2"); /* {"SourceType":"TextBox","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_98dab0214b0d4d3c8794321adb485b85.WireTo(id_e101f98e6141432f8838ef41d48eafcf, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_98dab0214b0d4d3c8794321adb485b85.WireTo(id_2930a1df8aec4aaa9972fde68ea5a20b, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"DatePicker","DestinationIsReference":false} */
            id_2930a1df8aec4aaa9972fde68ea5a20b.WireTo(id_e4a000db68af49179abc652bd9980ce4, "selectedDate"); /* {"SourceType":"DatePicker","SourceIsReference":false,"DestinationType":"AsObject","DestinationIsReference":false} */
            id_e4a000db68af49179abc652bd9980ce4.WireTo(id_cdcf6555d3e54c8084afe5f2d6b30b16, "outputAsJToken"); /* {"SourceType":"AsObject","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_c91ea6ab84f142168fc744d83055b19e.WireTo(id_cdcf6555d3e54c8084afe5f2d6b30b16, "Item2"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_e2137239798a4109a582d627c8820df0.WireTo(id_cdcf6555d3e54c8084afe5f2d6b30b16, "Item2"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_5dba992efe104d1aab612d3536f0e5be.WireTo(id_254c64aeead1453ea551f8f9a514aca8, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_254c64aeead1453ea551f8f9a514aca8.WireTo(id_a4e8114b07f5455aafd6cd7945bbc4b4, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_a4e8114b07f5455aafd6cd7945bbc4b4.WireTo(id_1790e20601e24b7084bd5047009f2b65, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_a4e8114b07f5455aafd6cd7945bbc4b4.WireTo(setPlaceholderValues, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_a4e8114b07f5455aafd6cd7945bbc4b4.WireTo(getTickedSessionEIDs, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_1790e20601e24b7084bd5047009f2b65.WireTo(nlisLoginWindow, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            setPlaceholderValues.WireTo(id_0a1afcc9940543f0bd6380896e808b85, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            setPlaceholderValues.WireTo(id_63a4d5f8ac9b47479d15685db94ca43e, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            setPlaceholderValues.WireTo(id_ac3a5d8867ba488bb955ca380f6acc8d, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_0a1afcc9940543f0bd6380896e808b85.WireTo(id_f5cb5000f71b4506aa729790441d7761, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"AsObject","DestinationIsReference":false} */
            id_f5cb5000f71b4506aa729790441d7761.WireTo(id_49eda0f67ef5458d84350fcd6b932649, "outputAsJToken"); /* {"SourceType":"AsObject","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_52935ea9b8e748808816155a77eb29ef.WireTo(id_49eda0f67ef5458d84350fcd6b932649, "Item2"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_63a4d5f8ac9b47479d15685db94ca43e.WireTo(id_24bd840429f04f9cafff094b83842ab0, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"AsObject","DestinationIsReference":false} */
            id_24bd840429f04f9cafff094b83842ab0.WireTo(id_b65a57f1fff140c1bafe7e9f2dfa3b7e, "outputAsJToken"); /* {"SourceType":"AsObject","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_df4e7e9e6a5443af942e39cfd40603f6.WireTo(id_b65a57f1fff140c1bafe7e9f2dfa3b7e, "Item2"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            getTickedSessionEIDs.WireTo(getTickedSessionsForEIDTransact, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Transact","DestinationIsReference":false} */
            tickedSessionEIDConnector.WireTo(id_9917048599714a268c2e1de38c4776a9, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"AsObject","DestinationIsReference":false} */
            id_9917048599714a268c2e1de38c4776a9.WireTo(id_18798cc54be840a1985dd06683a7c6c9, "outputAsJToken"); /* {"SourceType":"AsObject","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_fc6be78b45874bfbbc212bcc1f40c1c4.WireTo(id_18798cc54be840a1985dd06683a7c6c9, "Item2"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            id_18798cc54be840a1985dd06683a7c6c9.WireTo(id_3e073009d92349cf9c0dcc0480874493, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"ConvertToEvent","DestinationIsReference":false} */
            id_3e073009d92349cf9c0dcc0480874493.WireTo(tickedSessionEIDsHaveBeenReceived, "eventOutput"); /* {"SourceType":"ConvertToEvent","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            tickedSessionEIDsHaveBeenReceived.WireTo(id_7407374bc4d24db9b37cce4a3b620b15, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventGate","DestinationIsReference":false} */
            nlisSendTransactionTrue.WireTo(id_7407374bc4d24db9b37cce4a3b620b15, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"EventGate","DestinationIsReference":false} */
            nlisSendTransactionFalse.WireTo(id_7407374bc4d24db9b37cce4a3b620b15, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"EventGate","DestinationIsReference":false} */
            id_7407374bc4d24db9b37cce4a3b620b15.WireTo(id_7a2ba58d2b5044e18940a99dea0db00b, "eventOutput"); /* {"SourceType":"EventGate","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_7a2ba58d2b5044e18940a99dea0db00b.WireTo(createNlisRequest, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"CreateJObject","DestinationIsReference":false} */
            id_7a2ba58d2b5044e18940a99dea0db00b.WireTo(createAnimalTransferModel, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"CreateJObject","DestinationIsReference":false} */
            id_7a2ba58d2b5044e18940a99dea0db00b.WireTo(MiHubWaitingWindow, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_7a2ba58d2b5044e18940a99dea0db00b.WireTo(nlisHttpRequest, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"HttpRequest","DestinationIsReference":false} */
            id_7a2ba58d2b5044e18940a99dea0db00b.WireTo(id_f44fb6b6e9ce43a6b712c9677ceccc8d, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_7a2ba58d2b5044e18940a99dea0db00b.WireTo(nlisSendTransactionFalse, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            createNlisRequest.WireTo(id_03672039bece413e9e5980def681fe84, "stringPairs"); /* {"SourceType":"CreateJObject","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            createNlisRequest.WireTo(id_0b0f29eaabca462cacbd15d2fa64d11e, "stringPairs"); /* {"SourceType":"CreateJObject","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            createNlisRequest.WireTo(id_bdd4d8c336b34fbc83a6af9249625d12, "stringPairs"); /* {"SourceType":"CreateJObject","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            createNlisRequest.WireTo(id_a436f243d37d472795bb765052517fb7, "stringPairs"); /* {"SourceType":"CreateJObject","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            createNlisRequest.WireTo(id_9fe5a772d2c147829fa1abc8c4519c38, "stringPairs"); /* {"SourceType":"CreateJObject","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            createNlisRequest.WireTo(id_4de4af0dd8854b7385bd8f1c0e20aa3b, "stringPairs"); /* {"SourceType":"CreateJObject","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            createNlisRequest.WireTo(id_b94a5b5ba544412b99e1021e7382e4fc, "stringPairs"); /* {"SourceType":"CreateJObject","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            createNlisRequest.WireTo(id_b7061a02ae5342beb54d8b0e3df30470, "stringPairs"); /* {"SourceType":"CreateJObject","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            createNlisRequest.WireTo(id_2fe9bf09ed0742958f031075a4c4c08e, "stringPairs"); /* {"SourceType":"CreateJObject","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            createNlisRequest.WireTo(id_71bca7d0003f4225b1ba2342a172dba7, "stringPairs"); /* {"SourceType":"CreateJObject","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            createNlisRequest.WireTo(id_8113f9d89f5345ffb6c9abcb89949097, "stringPairs"); /* {"SourceType":"CreateJObject","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            createNlisRequest.WireTo(id_7af94faf1ebe411d84913a53599b70f6, "stringPairs"); /* {"SourceType":"CreateJObject","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            createNlisRequest.WireTo(id_8e0de3cb26684ebb99c0862618bb750a, "stringPairs"); /* {"SourceType":"CreateJObject","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            createNlisRequest.WireTo(id_5aead95e76514f8991de08769de4760a, "stringPairs"); /* {"SourceType":"CreateJObject","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            createNlisRequest.WireTo(id_0b31a771a39247b687f3126a6f5e7548, "stringPairs"); /* {"SourceType":"CreateJObject","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            createNlisRequest.WireTo(id_6774c022a717401d803bd9afb1ea894d, "stringPairs"); /* {"SourceType":"CreateJObject","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            createNlisRequest.WireTo(id_8292fb4e95874a65a80709eadaab9cf7, "stringPairs"); /* {"SourceType":"CreateJObject","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            createNlisRequest.WireTo(id_f0fe76cb8c654e2d995a6d46768f21fd, "stringPairs"); /* {"SourceType":"CreateJObject","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            createNlisRequest.WireTo(id_ddbdc05bbf2142c2a7c9f9621591a8e8, "stringPairs"); /* {"SourceType":"CreateJObject","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            createNlisRequest.WireTo(id_6443b9839bb4423188f60bf081febcba, "stringPairs"); /* {"SourceType":"CreateJObject","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            createNlisRequest.WireTo(id_15139c45fde04f8d80975026955d61c7, "stringPairs"); /* {"SourceType":"CreateJObject","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            createNlisRequest.WireTo(id_c91ea6ab84f142168fc744d83055b19e, "jTokenPairs"); /* {"SourceType":"CreateJObject","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            createNlisRequest.WireTo(id_76964d73ab16452faf22efb04ef4ad3e, "jTokenPairs"); /* {"SourceType":"CreateJObject","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            createNlisRequest.WireTo(id_7ff3876c4c714cd291b4f09ed396e468, "jTokenPairs"); /* {"SourceType":"CreateJObject","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            createNlisRequest.WireTo(id_9a233d8e1b6f4752804859ad25aca2a1, "jTokenPairs"); /* {"SourceType":"CreateJObject","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            createNlisRequest.WireTo(id_f399f412e087413c91a8e3cfd1b674c1, "jTokenPairs"); /* {"SourceType":"CreateJObject","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            createNlisRequest.WireTo(id_ddc6d323e71d4c248e03b4dfae20bf89, "jTokenPairs"); /* {"SourceType":"CreateJObject","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            createNlisRequest.WireTo(id_c0e910de5fa346ae94734210f24755d6, "jTokenPairs"); /* {"SourceType":"CreateJObject","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            createNlisRequest.WireTo(id_6690c83ad6de4da7b4363ccbe1e071b6, "outputAsJSON"); /* {"SourceType":"CreateJObject","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_91bf51e9edcd43fa810e514f3b0d3bbe.WireTo(id_6690c83ad6de4da7b4363ccbe1e071b6, "Item2"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            createAnimalTransferModel.WireTo(id_a1ffbb81d9b044d191ffad1e8c160a63, "stringPairs"); /* {"SourceType":"CreateJObject","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            createAnimalTransferModel.WireTo(id_f7a12fdb5ffc407898985df3b703dcd7, "stringPairs"); /* {"SourceType":"CreateJObject","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            createAnimalTransferModel.WireTo(id_14d0c11ef4fc48f9af63b3b2b8168346, "stringPairs"); /* {"SourceType":"CreateJObject","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            createAnimalTransferModel.WireTo(id_67ce72bd41ff40edb712a22231e55bca, "stringPairs"); /* {"SourceType":"CreateJObject","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            createAnimalTransferModel.WireTo(id_3b5f6a8060de4091914af93eb36e5d88, "stringPairs"); /* {"SourceType":"CreateJObject","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            createAnimalTransferModel.WireTo(id_fccdd18f44be4215a75bc7695f01cae4, "stringPairs"); /* {"SourceType":"CreateJObject","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            createAnimalTransferModel.WireTo(id_b83ceebfdd854be295f2513d0af97887, "stringPairs"); /* {"SourceType":"CreateJObject","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            createAnimalTransferModel.WireTo(id_91bf51e9edcd43fa810e514f3b0d3bbe, "stringPairs"); /* {"SourceType":"CreateJObject","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            createAnimalTransferModel.WireTo(id_fc6be78b45874bfbbc212bcc1f40c1c4, "jTokenPairs"); /* {"SourceType":"CreateJObject","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            createAnimalTransferModel.WireTo(id_014b61a91c364ef0a5b208c15cefb5de, "jTokenPairs"); /* {"SourceType":"CreateJObject","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            createAnimalTransferModel.WireTo(id_e0a7320958984ff2a6bc26c6722a1040, "jTokenPairs"); /* {"SourceType":"CreateJObject","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            createAnimalTransferModel.WireTo(id_41c7a8a6f50e42c8a0daa93a6904c0cc, "jTokenPairs"); /* {"SourceType":"CreateJObject","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            createAnimalTransferModel.WireTo(id_697de7541ee74c1790a53ab5c654da54, "jTokenPairs"); /* {"SourceType":"CreateJObject","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            createAnimalTransferModel.WireTo(id_e2137239798a4109a582d627c8820df0, "jTokenPairs"); /* {"SourceType":"CreateJObject","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            createAnimalTransferModel.WireTo(id_52935ea9b8e748808816155a77eb29ef, "jTokenPairs"); /* {"SourceType":"CreateJObject","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            createAnimalTransferModel.WireTo(id_df4e7e9e6a5443af942e39cfd40603f6, "jTokenPairs"); /* {"SourceType":"CreateJObject","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            createAnimalTransferModel.WireTo(id_7df06b94af2a4440b7914856430a2bec, "jTokenPairs"); /* {"SourceType":"CreateJObject","SourceIsReference":false,"DestinationType":"Pair","DestinationIsReference":false} */
            createAnimalTransferModel.WireTo(id_cf7f3646d49d428680fa04612896bc86, "outputAsJSON"); /* {"SourceType":"CreateJObject","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            nlisHttpRequest.WireTo(id_cf7f3646d49d428680fa04612896bc86, "jsonData"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"HttpRequest","DestinationIsReference":false} */
            nlisHttpRequest.WireTo(internetConnectionDetectedConnector, "sendRequestFlag"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"HttpRequest","DestinationIsReference":false} */
            nlisHttpRequest.WireTo(integrationRequestResponseParser, "responseJsonOutput"); /* {"SourceType":"HttpRequest","SourceIsReference":false,"DestinationType":"JSONParser","DestinationIsReference":false} */
            nlisHttpRequest.WireTo(showIntegrationRequestResultWindow, "taskComplete"); /* {"SourceType":"HttpRequest","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_f44fb6b6e9ce43a6b712c9677ceccc8d.WireTo(checkInternetConnection, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"HttpRequest","DestinationIsReference":false} */
            id_f44fb6b6e9ce43a6b712c9677ceccc8d.WireTo(id_635dbd7ab6434dc3a53db2d263f03316, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_635dbd7ab6434dc3a53db2d263f03316.WireTo(internetConnectionDetectedConnector, "inputDataB"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_635dbd7ab6434dc3a53db2d263f03316.WireTo(id_7a3ac9990cf44bf494fd9b52cb07caec, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"IfElse","DestinationIsReference":false} */
            id_7a3ac9990cf44bf494fd9b52cb07caec.WireTo(id_f8c78d4d0821449fb7caba4b68072bd1, "elseOutput"); /* {"SourceType":"IfElse","SourceIsReference":false,"DestinationType":"Timer","DestinationIsReference":false} */
            id_f8c78d4d0821449fb7caba4b68072bd1.WireTo(id_f44fb6b6e9ce43a6b712c9677ceccc8d, "eventHappened"); /* {"SourceType":"Timer","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            AppStartEventConnector.WireTo(SUBROUTINE_checkInternetConnectivity, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":true,"DestinationType":"EventConnector","DestinationIsReference":false} */
            AppStartEventConnector.WireTo(id_314156256a124f6aae9a60c3ce57242d, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":true,"DestinationType":"Data","DestinationIsReference":false} */
            id_314156256a124f6aae9a60c3ce57242d.WireTo(internetConnectionDetectedConnector, "inputDataB"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_314156256a124f6aae9a60c3ce57242d.WireTo(id_53e5e708283d4996bfcbd804c6669efa, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"IfElse","DestinationIsReference":false} */
            id_53e5e708283d4996bfcbd804c6669efa.WireTo(id_407212f08905495e96b29736f737539b, "ifOutput"); /* {"SourceType":"IfElse","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_407212f08905495e96b29736f737539b.WireTo(id_ab4f6f6a905a4afe83ef36aabf903fa3, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_407212f08905495e96b29736f737539b.WireTo(id_c5ed553d124b4027a98352a58c52a81f, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_ab4f6f6a905a4afe83ef36aabf903fa3.WireTo(id_dc60bbe392684960b26dc2e59b7e565f, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_c5ed553d124b4027a98352a58c52a81f.WireTo(id_86a1a47dd7364b68a3ac131c343a9736, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_c5ed553d124b4027a98352a58c52a81f.WireTo(id_52fa92652fbe4256b8793c74567cb775, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_c5ed553d124b4027a98352a58c52a81f.WireTo(id_080c0f042e094b89a658e2490a8e7e4b, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_86a1a47dd7364b68a3ac131c343a9736.WireTo(id_98a7500f377947f88d285e0c67c16199, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_98a7500f377947f88d285e0c67c16199.WireTo(MiHubRegisterPopupWindow, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_98a7500f377947f88d285e0c67c16199.WireTo(id_407212f08905495e96b29736f737539b, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_52fa92652fbe4256b8793c74567cb775.WireTo(id_b9bb90fc21b445d2a543a0ddb0152a5e, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_b9bb90fc21b445d2a543a0ddb0152a5e.WireTo(SUBROUTINE_MiHubLogin, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_b9bb90fc21b445d2a543a0ddb0152a5e.WireTo(id_407212f08905495e96b29736f737539b, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_080c0f042e094b89a658e2490a8e7e4b.WireTo(id_407212f08905495e96b29736f737539b, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            // END AUTO-GENERATED WIRING FOR WebServices
            #endregion

            #region UI WIRING
            // BEGIN AUTO-GENERATED WIRING FOR UI
            id_553e3ca462004f569bb0559d4e19da8a.WireTo(id_dd4a9aa1cc8346509a61fc7f7485c5ff, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"TabContainer","DestinationIsReference":false} */
            id_553e3ca462004f569bb0559d4e19da8a.WireTo(id_0ef690880728491b8ddc1a9f9183cf9a, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_dd4a9aa1cc8346509a61fc7f7485c5ff.WireTo(id_44cf5379dada4ce7bdbcfdfd4431331e, "childrenTabs"); /* {"SourceType":"TabContainer","SourceIsReference":false,"DestinationType":"Tab","DestinationIsReference":false} */
            id_dd4a9aa1cc8346509a61fc7f7485c5ff.WireTo(id_5391439747c4412eacd8d1787134553e, "childrenTabs"); /* {"SourceType":"TabContainer","SourceIsReference":false,"DestinationType":"Tab","DestinationIsReference":false} */
            id_dd4a9aa1cc8346509a61fc7f7485c5ff.WireTo(id_4db3afe91a264933a0a8a4fd8495ac02, "childrenTabs"); /* {"SourceType":"TabContainer","SourceIsReference":false,"DestinationType":"Tab","DestinationIsReference":false} */
            id_dd4a9aa1cc8346509a61fc7f7485c5ff.WireTo(id_4c070cf2f6994838a4c2b6f7d482767a, "childrenTabs"); /* {"SourceType":"TabContainer","SourceIsReference":false,"DestinationType":"Tab","DestinationIsReference":false} */
            id_dd4a9aa1cc8346509a61fc7f7485c5ff.WireTo(id_98c6ac2fc16f440c87c0b6c78a624b2d, "childrenTabs"); /* {"SourceType":"TabContainer","SourceIsReference":false,"DestinationType":"Tab","DestinationIsReference":false} */
            id_4db3afe91a264933a0a8a4fd8495ac02.WireTo(id_3b2ab3e1bb104e32b7349b0213d70337, "tabItemList"); /* {"SourceType":"Tab","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_98c6ac2fc16f440c87c0b6c78a624b2d.WireTo(id_9e78f81ba5a24adfa156927061608791, "tabItemList"); /* {"SourceType":"Tab","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_98c6ac2fc16f440c87c0b6c78a624b2d.WireTo(id_a3e8d677b58945af8670be93f7c5143f, "tabItemList"); /* {"SourceType":"Tab","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_98c6ac2fc16f440c87c0b6c78a624b2d.WireTo(id_cfb1198e4ab24bfcbd1464f2ab7dd6e0, "tabItemList"); /* {"SourceType":"Tab","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_9e78f81ba5a24adfa156927061608791.WireTo(id_407dd69cf0a34642971073a089c2bc4b, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"GroupingBox","DestinationIsReference":false} */
            id_407dd69cf0a34642971073a089c2bc4b.WireTo(id_ae8c0ebdcc3744bebe1cb5d59fd63846, "groupBoxContent"); /* {"SourceType":"GroupingBox","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_407dd69cf0a34642971073a089c2bc4b.WireTo(id_a88d32c6365f4075a7e1615ca9687eaf, "groupBoxContent"); /* {"SourceType":"GroupingBox","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_407dd69cf0a34642971073a089c2bc4b.WireTo(id_243d5b85c8fb455b98721ef4f956855e, "groupBoxContent"); /* {"SourceType":"GroupingBox","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_ae8c0ebdcc3744bebe1cb5d59fd63846.WireTo(id_24967b00300a464cb744ce9ca4e009b5, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_ae8c0ebdcc3744bebe1cb5d59fd63846.WireTo(id_7ee4e3a11a924e179073eb833fd7b618, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            id_ae8c0ebdcc3744bebe1cb5d59fd63846.WireTo(id_e82f05fcee5c4e9c840638ff01f73910, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_e82f05fcee5c4e9c840638ff01f73910.WireTo(MiHubLogOut, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":true} */
            id_a88d32c6365f4075a7e1615ca9687eaf.WireTo(id_e7221ba0d6b646dfad26b552248bc114, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_a88d32c6365f4075a7e1615ca9687eaf.WireTo(id_828628611a114f9dac32cf4bcc4b62cd, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_e7221ba0d6b646dfad26b552248bc114.WireTo(mihubLoginWindow, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":true} */
            id_828628611a114f9dac32cf4bcc4b62cd.WireTo(MiHubLogOut, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":true} */
            id_243d5b85c8fb455b98721ef4f956855e.WireTo(id_933e18cb574e403a8293e7ad51ae87ff, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_933e18cb574e403a8293e7ad51ae87ff.WireTo(MiHubRegisterPopupWindow, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":true} */
            id_a3e8d677b58945af8670be93f7c5143f.WireTo(id_5e05cc197a2340618fe5c4c05e88eec9, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"GroupingBox","DestinationIsReference":false} */
            id_cfb1198e4ab24bfcbd1464f2ab7dd6e0.WireTo(id_87fb3d397be445f1a6acbe67908b2efb, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"GroupingBox","DestinationIsReference":false} */
            id_0ef690880728491b8ddc1a9f9183cf9a.WireTo(id_241387242ba644a5b8d657ce5aa195bb, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"RightJustify","DestinationIsReference":false} */
            id_241387242ba644a5b8d657ce5aa195bb.WireTo(id_588121ad26d84fa2b586b8eb456fbfc0, "children"); /* {"SourceType":"RightJustify","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_241387242ba644a5b8d657ce5aa195bb.WireTo(id_513bc764885b46d5a9edc7eb3eaf0149, "children"); /* {"SourceType":"RightJustify","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_513bc764885b46d5a9edc7eb3eaf0149.WireTo(dataLinkOption, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":true} */
            dataLinkUpdates.WireTo(id_b3c578995cc14195a74a2cf73df44c94, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":true,"DestinationType":"Vertical","DestinationIsReference":false} */
            id_b3c578995cc14195a74a2cf73df44c94.WireTo(id_b6eda4011f5849cb9d2da5009b098c22, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"TabContainer","DestinationIsReference":false} */
            id_b3c578995cc14195a74a2cf73df44c94.WireTo(id_58970d93c39c40d098e4346bc5bef9af, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_b6eda4011f5849cb9d2da5009b098c22.WireTo(id_2e22bc4f40d64ad3b6ff3861f6c5519f, "childrenTabs"); /* {"SourceType":"TabContainer","SourceIsReference":false,"DestinationType":"Tab","DestinationIsReference":false} */
            id_2e22bc4f40d64ad3b6ff3861f6c5519f.WireTo(id_bb23b069340d49c4ba9c005aa0cac650, "tabItemList"); /* {"SourceType":"Tab","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_2e22bc4f40d64ad3b6ff3861f6c5519f.WireTo(UpdateFromFileButton, "tabItemList"); /* {"SourceType":"Tab","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_2e22bc4f40d64ad3b6ff3861f6c5519f.WireTo(RecoverDeviceButton, "tabItemList"); /* {"SourceType":"Tab","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_bb23b069340d49c4ba9c005aa0cac650.WireTo(CheckForUpdatesConnector, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":true} */
            UpdateFromFileButton.WireTo(UpdateFromFileConnector, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":true} */
            RecoverDeviceButton.WireTo(id_6726c941558141fc9da87b08fe62d06c, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_6726c941558141fc9da87b08fe62d06c.WireTo(EIDRecoverPopup, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_6726c941558141fc9da87b08fe62d06c.WireTo(ComPortList, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"ComPortOptionBox","DestinationIsReference":false} */
            EIDRecoverPopup.WireTo(id_28d3acf86e534fdc8014d3e49509b651, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Vertical","DestinationIsReference":false} */
            id_28d3acf86e534fdc8014d3e49509b651.WireTo(id_aaa9e90cbe1e4c05a1fd7c15f2acaf10, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_28d3acf86e534fdc8014d3e49509b651.WireTo(id_42cbbc7338ff42ef9543d0d5d0d20263, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_28d3acf86e534fdc8014d3e49509b651.WireTo(id_0c8b24013cc94463af0684e5450edf69, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_28d3acf86e534fdc8014d3e49509b651.WireTo(id_afc226e5502d48829a02e212e718b037, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_aaa9e90cbe1e4c05a1fd7c15f2acaf10.WireTo(id_92151aec98a842b09e23e6fc8416a9ff, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"GroupingBox","DestinationIsReference":false} */
            id_92151aec98a842b09e23e6fc8416a9ff.WireTo(id_6331fe92301f457daeabc13eab4a22a7, "groupBoxContent"); /* {"SourceType":"GroupingBox","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_6331fe92301f457daeabc13eab4a22a7.WireTo(DeviceTypeOption, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"OptionBox","DestinationIsReference":false} */
            DeviceTypeOption.WireTo(RecoverDeviceType, "dataFlowSelectionOutput"); /* {"SourceType":"OptionBox","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            DeviceTypeOption.WireTo(id_1b41e4246228498d992960dc7590c05d, "children"); /* {"SourceType":"OptionBox","SourceIsReference":false,"DestinationType":"OptionBoxItem","DestinationIsReference":false} */
            id_42cbbc7338ff42ef9543d0d5d0d20263.WireTo(id_487d1f7d030d410b8a1dd7202fdf3f28, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"GroupingBox","DestinationIsReference":false} */
            id_487d1f7d030d410b8a1dd7202fdf3f28.WireTo(id_2b4234a9a01143a096a8c9764941b899, "groupBoxContent"); /* {"SourceType":"GroupingBox","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_2b4234a9a01143a096a8c9764941b899.WireTo(ComPortList, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"ComPortOptionBox","DestinationIsReference":false} */
            id_2b4234a9a01143a096a8c9764941b899.WireTo(id_d4ce9b3e60ee4662a2996c639a53d424, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"RightJustify","DestinationIsReference":false} */
            id_d4ce9b3e60ee4662a2996c639a53d424.WireTo(id_aadc4264ce44404cbc8dd793de3bebef, "children"); /* {"SourceType":"RightJustify","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_aadc4264ce44404cbc8dd793de3bebef.WireTo(ComPortList, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"ComPortOptionBox","DestinationIsReference":false} */
            id_0c8b24013cc94463af0684e5450edf69.WireTo(id_65b7643634014f2a99119d5dd062ae3e, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"GroupingBox","DestinationIsReference":false} */
            id_65b7643634014f2a99119d5dd062ae3e.WireTo(id_95fee20097d745a9911714cc3b989240, "groupBoxContent"); /* {"SourceType":"GroupingBox","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_95fee20097d745a9911714cc3b989240.WireTo(EIDRecoverFirmwareTextBox, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            id_95fee20097d745a9911714cc3b989240.WireTo(id_666e42c7abd745b19ce110e58fff7fe3, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_666e42c7abd745b19ce110e58fff7fe3.WireTo(EIDRecoverFirmwareBrowser, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"OpenFirmwareBrowser","DestinationIsReference":false} */
            EIDRecoverFirmwareBrowser.WireTo(id_aa1f726c2c234538b05c21929b7cfc31, "selectedFile"); /* {"SourceType":"OpenFirmwareBrowser","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_aa1f726c2c234538b05c21929b7cfc31.WireTo(EIDRecoverFirmwareTextBox, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"TextBox","DestinationIsReference":false} */
            id_afc226e5502d48829a02e212e718b037.WireTo(id_0f9bf9de23c1469c8d22575658aaa098, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"RightJustify","DestinationIsReference":false} */
            id_0f9bf9de23c1469c8d22575658aaa098.WireTo(id_6492530e8df04d70a523f4eae5b3e654, "children"); /* {"SourceType":"RightJustify","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_6492530e8df04d70a523f4eae5b3e654.WireTo(EIDStartRecovery, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":true} */
            id_58970d93c39c40d098e4346bc5bef9af.WireTo(id_941595b6dcca4780bb9e3c613ca56dd8, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"RightJustify","DestinationIsReference":false} */
            id_941595b6dcca4780bb9e3c613ca56dd8.WireTo(id_2f8b78e826a94b678b6dcbe2745515fc, "children"); /* {"SourceType":"RightJustify","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            dataLinkOption.WireTo(id_553e3ca462004f569bb0559d4e19da8a, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":true,"DestinationType":"Vertical","DestinationIsReference":false} */
            id_588121ad26d84fa2b586b8eb456fbfc0.WireTo(id_82fc0f5dcc5742ee83dc4b92787ac58c, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_82fc0f5dcc5742ee83dc4b92787ac58c.WireTo(dataLinkOption, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":true} */
            id_44cf5379dada4ce7bdbcfdfd4431331e.WireTo(id_0ab77e3097574fbdb6690e5e3399bba2, "tabItemList"); /* {"SourceType":"Tab","SourceIsReference":false,"DestinationType":"RadioButton","DestinationIsReference":false} */
            id_44cf5379dada4ce7bdbcfdfd4431331e.WireTo(id_da2acc1bd3834eb2ab1671c52adf9867, "tabItemList"); /* {"SourceType":"Tab","SourceIsReference":false,"DestinationType":"RadioButton","DestinationIsReference":false} */
            id_44cf5379dada4ce7bdbcfdfd4431331e.WireTo(id_a0160fe0401040909421dcda5cf7274e, "tabItemList"); /* {"SourceType":"Tab","SourceIsReference":false,"DestinationType":"RadioButton","DestinationIsReference":false} */
            id_44cf5379dada4ce7bdbcfdfd4431331e.WireTo(id_11da591c6fb440fb93edf0604163b9c1, "tabItemList"); /* {"SourceType":"Tab","SourceIsReference":false,"DestinationType":"RadioButton","DestinationIsReference":false} */
            id_44cf5379dada4ce7bdbcfdfd4431331e.WireTo(id_c8809d1e4a0e457c99ad7af2204e7336, "tabItemList"); /* {"SourceType":"Tab","SourceIsReference":false,"DestinationType":"RadioButton","DestinationIsReference":false} */
            id_44cf5379dada4ce7bdbcfdfd4431331e.WireTo(id_1a353d8d6e4a4b26941d3938df0d0eec, "tabItemList"); /* {"SourceType":"Tab","SourceIsReference":false,"DestinationType":"RadioButton","DestinationIsReference":false} */
            id_44cf5379dada4ce7bdbcfdfd4431331e.WireTo(id_a7a0ea0cb06243d5ab62b2dd206aaf32, "tabItemList"); /* {"SourceType":"Tab","SourceIsReference":false,"DestinationType":"RadioButton","DestinationIsReference":false} */
            id_44cf5379dada4ce7bdbcfdfd4431331e.WireTo(id_4262cd8bbf3244878794b42e440fe9ba, "tabItemList"); /* {"SourceType":"Tab","SourceIsReference":false,"DestinationType":"RadioButton","DestinationIsReference":false} */
            id_44cf5379dada4ce7bdbcfdfd4431331e.WireTo(id_78be548112d44cec84ccc50814adf868, "tabItemList"); /* {"SourceType":"Tab","SourceIsReference":false,"DestinationType":"RadioButton","DestinationIsReference":false} */
            id_44cf5379dada4ce7bdbcfdfd4431331e.WireTo(id_3d190d4084cb47efb1b693a1330aab87, "tabItemList"); /* {"SourceType":"Tab","SourceIsReference":false,"DestinationType":"RadioButton","DestinationIsReference":false} */
            id_5391439747c4412eacd8d1787134553e.WireTo(id_7a84a73c626b4823b8e9d0427e3d8d6a, "tabItemList"); /* {"SourceType":"Tab","SourceIsReference":false,"DestinationType":"RadioButton","DestinationIsReference":false} */
            id_5391439747c4412eacd8d1787134553e.WireTo(id_ca6c8fb992084e388491208c62949464, "tabItemList"); /* {"SourceType":"Tab","SourceIsReference":false,"DestinationType":"RadioButton","DestinationIsReference":false} */
            id_4db3afe91a264933a0a8a4fd8495ac02.WireTo(id_52ce31a20e944273bcfdddbd5e0add92, "tabItemList"); /* {"SourceType":"Tab","SourceIsReference":false,"DestinationType":"RadioButton","DestinationIsReference":false} */
            id_4db3afe91a264933a0a8a4fd8495ac02.WireTo(id_b56c26a718764c6aa28882a2bc7832ad, "tabItemList"); /* {"SourceType":"Tab","SourceIsReference":false,"DestinationType":"RadioButton","DestinationIsReference":false} */
            id_4db3afe91a264933a0a8a4fd8495ac02.WireTo(id_2f9a085d266147dca9fc52889e310f52, "tabItemList"); /* {"SourceType":"Tab","SourceIsReference":false,"DestinationType":"RadioButton","DestinationIsReference":false} */
            id_4db3afe91a264933a0a8a4fd8495ac02.WireTo(id_a4ef455d46414565bd6c22c85fdeb28b, "tabItemList"); /* {"SourceType":"Tab","SourceIsReference":false,"DestinationType":"RadioButton","DestinationIsReference":false} */
            id_4db3afe91a264933a0a8a4fd8495ac02.WireTo(id_bf2cfd7432e44871a43f29afcc67786e, "tabItemList"); /* {"SourceType":"Tab","SourceIsReference":false,"DestinationType":"RadioButton","DestinationIsReference":false} */
            id_4c070cf2f6994838a4c2b6f7d482767a.WireTo(id_a6f8d557dbf84037be12cb39e1967f7e, "tabItemList"); /* {"SourceType":"Tab","SourceIsReference":false,"DestinationType":"RadioButton","DestinationIsReference":false} */
            id_4c070cf2f6994838a4c2b6f7d482767a.WireTo(id_884392e88d504425b108598ee5e27e64, "tabItemList"); /* {"SourceType":"Tab","SourceIsReference":false,"DestinationType":"RadioButton","DestinationIsReference":false} */
            id_4c070cf2f6994838a4c2b6f7d482767a.WireTo(id_251a32494684419f9824a4035b839e61, "tabItemList"); /* {"SourceType":"Tab","SourceIsReference":false,"DestinationType":"RadioButton","DestinationIsReference":false} */
            id_4c070cf2f6994838a4c2b6f7d482767a.WireTo(id_5a8156ece91b4b7c9ef90338ec95a188, "tabItemList"); /* {"SourceType":"Tab","SourceIsReference":false,"DestinationType":"RadioButton","DestinationIsReference":false} */
            id_2e22bc4f40d64ad3b6ff3861f6c5519f.WireTo(id_145c3e2d3f634089a36c7b35d96bbb16, "tabItemList"); /* {"SourceType":"Tab","SourceIsReference":false,"DestinationType":"CheckBox","DestinationIsReference":false} */
            FileFormatPersist.WireTo(id_0ab77e3097574fbdb6690e5e3399bba2, "children"); /* {"SourceType":"PersistList","SourceIsReference":false,"DestinationType":"RadioButton","DestinationIsReference":false} */
            FileFormatPersist.WireTo(id_da2acc1bd3834eb2ab1671c52adf9867, "children"); /* {"SourceType":"PersistList","SourceIsReference":false,"DestinationType":"RadioButton","DestinationIsReference":false} */
            FileFormatPersist.WireTo(id_a0160fe0401040909421dcda5cf7274e, "children"); /* {"SourceType":"PersistList","SourceIsReference":false,"DestinationType":"RadioButton","DestinationIsReference":false} */
            FileFormatPersist.WireTo(id_11da591c6fb440fb93edf0604163b9c1, "children"); /* {"SourceType":"PersistList","SourceIsReference":false,"DestinationType":"RadioButton","DestinationIsReference":false} */
            FileFormatPersist.WireTo(id_c8809d1e4a0e457c99ad7af2204e7336, "children"); /* {"SourceType":"PersistList","SourceIsReference":false,"DestinationType":"RadioButton","DestinationIsReference":false} */
            FileFormatPersist.WireTo(id_1a353d8d6e4a4b26941d3938df0d0eec, "children"); /* {"SourceType":"PersistList","SourceIsReference":false,"DestinationType":"RadioButton","DestinationIsReference":false} */
            FileFormatPersist.WireTo(id_a7a0ea0cb06243d5ab62b2dd206aaf32, "children"); /* {"SourceType":"PersistList","SourceIsReference":false,"DestinationType":"RadioButton","DestinationIsReference":false} */
            FileFormatPersist.WireTo(id_4262cd8bbf3244878794b42e440fe9ba, "children"); /* {"SourceType":"PersistList","SourceIsReference":false,"DestinationType":"RadioButton","DestinationIsReference":false} */
            FileFormatPersist.WireTo(id_78be548112d44cec84ccc50814adf868, "children"); /* {"SourceType":"PersistList","SourceIsReference":false,"DestinationType":"RadioButton","DestinationIsReference":false} */
            FileFormatPersist.WireTo(id_3d190d4084cb47efb1b693a1330aab87, "children"); /* {"SourceType":"PersistList","SourceIsReference":false,"DestinationType":"RadioButton","DestinationIsReference":false} */
            COMPortPersist.WireTo(id_7a84a73c626b4823b8e9d0427e3d8d6a, "children"); /* {"SourceType":"PersistList","SourceIsReference":false,"DestinationType":"RadioButton","DestinationIsReference":false} */
            COMPortPersist.WireTo(id_ca6c8fb992084e388491208c62949464, "children"); /* {"SourceType":"PersistList","SourceIsReference":false,"DestinationType":"RadioButton","DestinationIsReference":false} */
            LanguagePersist.WireTo(id_52ce31a20e944273bcfdddbd5e0add92, "children"); /* {"SourceType":"PersistList","SourceIsReference":false,"DestinationType":"RadioButton","DestinationIsReference":false} */
            LanguagePersist.WireTo(id_b56c26a718764c6aa28882a2bc7832ad, "children"); /* {"SourceType":"PersistList","SourceIsReference":false,"DestinationType":"RadioButton","DestinationIsReference":false} */
            LanguagePersist.WireTo(id_2f9a085d266147dca9fc52889e310f52, "children"); /* {"SourceType":"PersistList","SourceIsReference":false,"DestinationType":"RadioButton","DestinationIsReference":false} */
            LanguagePersist.WireTo(id_a4ef455d46414565bd6c22c85fdeb28b, "children"); /* {"SourceType":"PersistList","SourceIsReference":false,"DestinationType":"RadioButton","DestinationIsReference":false} */
            LanguagePersist.WireTo(id_bf2cfd7432e44871a43f29afcc67786e, "children"); /* {"SourceType":"PersistList","SourceIsReference":false,"DestinationType":"RadioButton","DestinationIsReference":false} */
            CountryPersist.WireTo(id_a6f8d557dbf84037be12cb39e1967f7e, "children"); /* {"SourceType":"PersistList","SourceIsReference":false,"DestinationType":"RadioButton","DestinationIsReference":false} */
            CountryPersist.WireTo(id_884392e88d504425b108598ee5e27e64, "children"); /* {"SourceType":"PersistList","SourceIsReference":false,"DestinationType":"RadioButton","DestinationIsReference":false} */
            CountryPersist.WireTo(id_251a32494684419f9824a4035b839e61, "children"); /* {"SourceType":"PersistList","SourceIsReference":false,"DestinationType":"RadioButton","DestinationIsReference":false} */
            CountryPersist.WireTo(id_5a8156ece91b4b7c9ef90338ec95a188, "children"); /* {"SourceType":"PersistList","SourceIsReference":false,"DestinationType":"RadioButton","DestinationIsReference":false} */
            FileFormatPersist.WireTo(SettingsReaderWriter, "storage"); /* {"SourceType":"PersistList","SourceIsReference":false,"DestinationType":"JsonReaderWriter","DestinationIsReference":false} */
            COMPortPersist.WireTo(SettingsReaderWriter, "storage"); /* {"SourceType":"PersistList","SourceIsReference":false,"DestinationType":"JsonReaderWriter","DestinationIsReference":false} */
            LanguagePersist.WireTo(SettingsReaderWriter, "storage"); /* {"SourceType":"PersistList","SourceIsReference":false,"DestinationType":"JsonReaderWriter","DestinationIsReference":false} */
            CountryPersist.WireTo(SettingsReaderWriter, "storage"); /* {"SourceType":"PersistList","SourceIsReference":false,"DestinationType":"JsonReaderWriter","DestinationIsReference":false} */
            id_82fc0f5dcc5742ee83dc4b92787ac58c.WireTo(SettingsReaderWriter, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"JsonReaderWriter","DestinationIsReference":false} */
            AutomaticUpdatesPersist.WireTo(SettingsReaderWriter, "storage"); /* {"SourceType":"Persist","SourceIsReference":false,"DestinationType":"JsonReaderWriter","DestinationIsReference":false} */
            AutomaticUpdatesPersist.WireTo(id_145c3e2d3f634089a36c7b35d96bbb16, "child"); /* {"SourceType":"Persist","SourceIsReference":false,"DestinationType":"CheckBox","DestinationIsReference":false} */
            id_2f8b78e826a94b678b6dcbe2745515fc.WireTo(id_25f85600badc40f2a6d9eb0f707fba3f, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_25f85600badc40f2a6d9eb0f707fba3f.WireTo(dataLinkUpdates, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":true} */
            id_25f85600badc40f2a6d9eb0f707fba3f.WireTo(SettingsReaderWriter, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"JsonReaderWriter","DestinationIsReference":false} */
            id_2e22bc4f40d64ad3b6ff3861f6c5519f.WireTo(ReinstallUSBDriverButton, "tabItemList"); /* {"SourceType":"Tab","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            ReinstallUSBDriverButton.WireTo(ReinstallUSBDriverConnector, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":true} */
            // END AUTO-GENERATED WIRING FOR UI
            #endregion

            #region XR5000_Functionalities WIRING
            // BEGIN AUTO-GENERATED WIRING FOR XR5000_Functionalities
            XR5000ConnectedConnector.WireTo(stringXR5000Connected, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"LiteralString","DestinationIsReference":false} */
            stringXR5000Connected.WireTo(textDeviceConnected, "dataFlowOutput"); /* {"SourceType":"LiteralString","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":true} */
            XR5000ConnectedConnector.WireTo(XR5000ImportMenu, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"MenuItem","DestinationIsReference":false} */
            XR5000ConnectedConnector.WireTo(XR5000ExportMenu, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"MenuItem","DestinationIsReference":false} */
            XR5000ConnectedConnector.WireTo(XR5000DeleteMenu, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"MenuItem","DestinationIsReference":false} */
            XR5000ConnectedConnector.WireTo(XR5000ImportTool, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Tool","DestinationIsReference":false} */
            XR5000ConnectedConnector.WireTo(XR5000ExportTool, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Tool","DestinationIsReference":false} */
            XR5000ConnectedConnector.WireTo(XR5000DeleteTool, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Tool","DestinationIsReference":false} */
            fileMenu.WireTo(XR5000ImportMenu, "children"); /* {"SourceType":"Menu","SourceIsReference":true,"DestinationType":"MenuItem","DestinationIsReference":false} */
            fileMenu.WireTo(XR5000ExportMenu, "children"); /* {"SourceType":"Menu","SourceIsReference":true,"DestinationType":"MenuItem","DestinationIsReference":false} */
            fileMenu.WireTo(XR5000DeleteMenu, "children"); /* {"SourceType":"Menu","SourceIsReference":true,"DestinationType":"MenuItem","DestinationIsReference":false} */
            XR5000getInfoWizard.WireTo(id_39cc8679a22b46b4aebeef125cd5f0ae, "children"); /* {"SourceType":"Wizard","SourceIsReference":false,"DestinationType":"WizardItem","DestinationIsReference":false} */
            XR5000getInfoWizard.WireTo(id_5ffa0209425b4dcea9e1a3f02ff216ea, "children"); /* {"SourceType":"Wizard","SourceIsReference":false,"DestinationType":"WizardItem","DestinationIsReference":false} */
            XR5000getInfoWizard.WireTo(id_8ea685db14ec42f4bd8bb4a297d16289, "children"); /* {"SourceType":"Wizard","SourceIsReference":false,"DestinationType":"WizardItem","DestinationIsReference":false} */
            XR5000getInfoWizard.WireTo(id_f62a188e32b04fd494e53c0486fd417d, "children"); /* {"SourceType":"Wizard","SourceIsReference":false,"DestinationType":"WizardItem","DestinationIsReference":false} */
            XR5000getInfoWizard.WireTo(id_673d82ee3c3b49e39694cd342c73d4ad, "children"); /* {"SourceType":"Wizard","SourceIsReference":false,"DestinationType":"WizardItem","DestinationIsReference":false} */
            id_39cc8679a22b46b4aebeef125cd5f0ae.WireTo(XR5000getSessionWizard, "eventOutput"); /* {"SourceType":"WizardItem","SourceIsReference":false,"DestinationType":"Wizard","DestinationIsReference":false} */
            XR5000getSessionWizard.WireTo(id_210bf17d3a114bc5a7a19a6ab678dc48, "children"); /* {"SourceType":"Wizard","SourceIsReference":false,"DestinationType":"WizardItem","DestinationIsReference":false} */
            XR5000getSessionWizard.WireTo(id_ae94e2df21a4408e8b0bc9a55d2d481e, "children"); /* {"SourceType":"Wizard","SourceIsReference":false,"DestinationType":"WizardItem","DestinationIsReference":false} */
            XR5000getSessionWizard.WireTo(id_09bf8feb99e14e8e82def502ce66a0bf, "children"); /* {"SourceType":"Wizard","SourceIsReference":false,"DestinationType":"WizardItem","DestinationIsReference":false} */
            XR5000getSessionWizard.WireTo(id_db985bc62bfd412d8db7559b1ffa2ad0, "children"); /* {"SourceType":"Wizard","SourceIsReference":false,"DestinationType":"WizardItem","DestinationIsReference":false} */
            XR5000getSessionWizard.WireTo(XR5000getInfoWizard, "backEventOutput"); /* {"SourceType":"Wizard","SourceIsReference":false,"DestinationType":"Wizard","DestinationIsReference":false} */
            XR5000ImportMenu.WireTo(XR5000getInfoWizard, "eventOutput"); /* {"SourceType":"MenuItem","SourceIsReference":false,"DestinationType":"Wizard","DestinationIsReference":false} */
            id_210bf17d3a114bc5a7a19a6ab678dc48.WireTo(saveUsbSessionsDataToFileBrowser, "eventOutput"); /* {"SourceType":"WizardItem","SourceIsReference":false,"DestinationType":"SaveFileBrowser","DestinationIsReference":true} */
            id_ae94e2df21a4408e8b0bc9a55d2d481e.WireTo(naitLoginWindow, "eventOutput"); /* {"SourceType":"WizardItem","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":true} */
            id_09bf8feb99e14e8e82def502ce66a0bf.WireTo(nlisLoginWindow, "eventOutput"); /* {"SourceType":"WizardItem","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":true} */
            id_db985bc62bfd412d8db7559b1ffa2ad0.WireTo(mihubEventConnector, "eventOutput"); /* {"SourceType":"WizardItem","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":true} */
            id_5ffa0209425b4dcea9e1a3f02ff216ea.WireTo(saveUsbLifeDataToFileBrowser, "eventOutput"); /* {"SourceType":"WizardItem","SourceIsReference":false,"DestinationType":"SaveFileBrowser","DestinationIsReference":true} */
            id_8ea685db14ec42f4bd8bb4a297d16289.WireTo(getInformationOffDeviceBrower, "eventOutput"); /* {"SourceType":"WizardItem","SourceIsReference":false,"DestinationType":"FolderBrowser","DestinationIsReference":true} */
            XR5000ExportMenu.WireTo(XR5000putInfoWizard, "eventOutput"); /* {"SourceType":"MenuItem","SourceIsReference":false,"DestinationType":"Wizard","DestinationIsReference":false} */
            XR5000putInfoWizard.WireTo(id_748496bee52a4661b3699f367afb7904, "children"); /* {"SourceType":"Wizard","SourceIsReference":false,"DestinationType":"WizardItem","DestinationIsReference":false} */
            XR5000putInfoWizard.WireTo(id_48aaec02f3c245329aa25660a915faf7, "children"); /* {"SourceType":"Wizard","SourceIsReference":false,"DestinationType":"WizardItem","DestinationIsReference":false} */
            XR5000putInfoWizard.WireTo(id_a2cf0f9732584f008b372e537dd1cefb, "children"); /* {"SourceType":"Wizard","SourceIsReference":false,"DestinationType":"WizardItem","DestinationIsReference":false} */
            XR5000putInfoWizard.WireTo(id_835f7e3a147544e2997e5e7edeb864a7, "children"); /* {"SourceType":"Wizard","SourceIsReference":false,"DestinationType":"WizardItem","DestinationIsReference":false} */
            id_748496bee52a4661b3699f367afb7904.WireTo(uploadSessionFromPCToXr5000, "eventOutput"); /* {"SourceType":"WizardItem","SourceIsReference":true,"DestinationType":"OpenFileBrowser","DestinationIsReference":true} */
            id_a2cf0f9732584f008b372e537dd1cefb.WireTo(uploadFavSettingsBrowser, "eventOutput"); /* {"SourceType":"WizardItem","SourceIsReference":false,"DestinationType":"OpenFileBrowser","DestinationIsReference":true} */
            ToolBar.WireTo(XR5000Tools, "children"); /* {"SourceType":"Toolbar","SourceIsReference":true,"DestinationType":"Horizontal","DestinationIsReference":false} */
            XR5000Tools.WireTo(XR5000ImportTool, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Tool","DestinationIsReference":false} */
            XR5000Tools.WireTo(XR5000ExportTool, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Tool","DestinationIsReference":false} */
            XR5000Tools.WireTo(XR5000DeleteTool, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Tool","DestinationIsReference":false} */
            XR5000ImportTool.WireTo(XR5000getInfoWizard, "eventOutput"); /* {"SourceType":"Tool","SourceIsReference":false,"DestinationType":"Wizard","DestinationIsReference":false} */
            XR5000ExportTool.WireTo(XR5000putInfoWizard, "eventOutput"); /* {"SourceType":"Tool","SourceIsReference":false,"DestinationType":"Wizard","DestinationIsReference":false} */
            XR5000DeleteTool.WireTo(Scale5000DeleteSessions, "eventOutput"); /* {"SourceType":"Tool","SourceIsReference":false,"DestinationType":"Wizard","DestinationIsReference":false} */
            XR5000DeleteMenu.WireTo(Scale5000DeleteSessions, "eventOutput"); /* {"SourceType":"MenuItem","SourceIsReference":false,"DestinationType":"Wizard","DestinationIsReference":true} */
            // END AUTO-GENERATED WIRING FOR XR5000_Functionalities
            #endregion

            #region ID5000_Functionalities WIRING 
            // BEGIN AUTO-GENERATED WIRING FOR ID5000_Functionalities.xmind
            ID5000ConnectedConnector.WireTo(stringID5000Connected, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":true,"DestinationType":"LiteralString","DestinationIsReference":false} */
            stringID5000Connected.WireTo(textDeviceConnected, "dataFlowOutput"); /* {"SourceType":"LiteralString","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":true} */
            ID5000ConnectedConnector.WireTo(ID5000ImportMenu, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":true,"DestinationType":"MenuItem","DestinationIsReference":false} */
            ID5000ConnectedConnector.WireTo(ID5000ExportMenu, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":true,"DestinationType":"MenuItem","DestinationIsReference":false} */
            ID5000ConnectedConnector.WireTo(ID5000DeleteMenu, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":true,"DestinationType":"MenuItem","DestinationIsReference":false} */
            fileMenu.WireTo(ID5000ImportMenu, "children"); /* {"SourceType":"Menu","SourceIsReference":true,"DestinationType":"MenuItem","DestinationIsReference":false} */
            fileMenu.WireTo(ID5000ExportMenu, "children"); /* {"SourceType":"Menu","SourceIsReference":true,"DestinationType":"MenuItem","DestinationIsReference":false} */
            fileMenu.WireTo(ID5000DeleteMenu, "children"); /* {"SourceType":"Menu","SourceIsReference":true,"DestinationType":"MenuItem","DestinationIsReference":false} */
            ID5000ConnectedConnector.WireTo(ID5000ImportTool, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":true,"DestinationType":"Tool","DestinationIsReference":false} */
            ID5000ConnectedConnector.WireTo(ID5000ExportTool, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":true,"DestinationType":"Tool","DestinationIsReference":false} */
            ID5000ConnectedConnector.WireTo(ID5000DeleteTool, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":true,"DestinationType":"Tool","DestinationIsReference":false} */
            ToolBar.WireTo(ID5000Tools, "children"); /* {"SourceType":"Toolbar","SourceIsReference":true,"DestinationType":"Horizontal","DestinationIsReference":false} */
            ID5000Tools.WireTo(ID5000ImportTool, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Tool","DestinationIsReference":false} */
            ID5000Tools.WireTo(ID5000ExportTool, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Tool","DestinationIsReference":false} */
            ID5000Tools.WireTo(ID5000DeleteTool, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Tool","DestinationIsReference":false} */
            ID5000DeleteMenu.WireTo(Scale5000DeleteSessions, "eventOutput"); /* {"SourceType":"MenuItem","SourceIsReference":false,"DestinationType":"Wizard","DestinationIsReference":true} */
            ID5000DeleteTool.WireTo(Scale5000DeleteSessions, "eventOutput"); /* {"SourceType":"Tool","SourceIsReference":false,"DestinationType":"Wizard","DestinationIsReference":true} */
            // END AUTO-GENERATED WIRING FOR ID5000_Functionalities.xmind
            #endregion

            #region XRS2_Functionalities WIRING
            // BEGIN AUTO-GENERATED WIRING FOR XRS2_Functionalities
            ToolBar.WireTo(XRS2Tools, "children"); /* {"SourceType":"Toolbar","SourceIsReference":true,"DestinationType":"Horizontal","DestinationIsReference":false} */
            fileMenu.WireTo(XRS2ImportMenu, "children"); /* {"SourceType":"Menu","SourceIsReference":true,"DestinationType":"MenuItem","DestinationIsReference":false} */
            fileMenu.WireTo(XRS2ExportMenu, "children"); /* {"SourceType":"Menu","SourceIsReference":true,"DestinationType":"MenuItem","DestinationIsReference":false} */
            fileMenu.WireTo(XRS2DeleteMenu, "children"); /* {"SourceType":"Menu","SourceIsReference":true,"DestinationType":"MenuItem","DestinationIsReference":false} */
            stringXRS2Connected.WireTo(textDeviceConnected, "dataFlowOutput"); /* {"SourceType":"LiteralString","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":true} */
            XRS2ConnectedConnector.WireTo(stringXRS2Connected, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":true,"DestinationType":"LiteralString","DestinationIsReference":false} */
            XRS2ConnectedConnector.WireTo(XRS2ImportMenu, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":true,"DestinationType":"MenuItem","DestinationIsReference":false} */
            XRS2ConnectedConnector.WireTo(XRS2ExportMenu, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":true,"DestinationType":"MenuItem","DestinationIsReference":false} */
            XRS2ConnectedConnector.WireTo(XRS2DeleteMenu, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":true,"DestinationType":"MenuItem","DestinationIsReference":false} */
            XRS2ConnectedConnector.WireTo(XRS2ImportTool, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":true,"DestinationType":"Tool","DestinationIsReference":false} */
            XRS2ConnectedConnector.WireTo(XRS2ExportTool, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":true,"DestinationType":"Tool","DestinationIsReference":false} */
            XRS2ConnectedConnector.WireTo(XRS2DeleteTool, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":true,"DestinationType":"Tool","DestinationIsReference":false} */
            XRS2ConnectedConnector.WireTo(favouriteSetupRowButton, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":true,"DestinationType":"RowButton","DestinationIsReference":false} */
            XRS2ConnectedConnector.WireTo(id_7f0dae1024b349cdbb1ed20a1b07aa1f, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":true,"DestinationType":"EventGate","DestinationIsReference":false} */
            XRS2ConnectedConnector.WireTo(alertRowButton, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":true,"DestinationType":"RowButton","DestinationIsReference":false} */
            XRS2ConnectedConnector.WireTo(favouriteSetupSCP, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":true,"DestinationType":"FavouriteSetupSCP","DestinationIsReference":false} */
            XRS2ConnectedConnector.WireTo(alertDataSCP, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":true,"DestinationType":"AlertDataSCP","DestinationIsReference":false} */
            XRS2ImportTool.WireTo(XRS2getInfoWizard, "eventOutput"); /* {"SourceType":"Tool","SourceIsReference":false,"DestinationType":"Wizard","DestinationIsReference":false} */
            XRS2ExportTool.WireTo(XRS2putInfoWizard, "eventOutput"); /* {"SourceType":"Tool","SourceIsReference":false,"DestinationType":"Wizard","DestinationIsReference":false} */
            XRS2ImportMenu.WireTo(XRS2getInfoWizard, "eventOutput"); /* {"SourceType":"MenuItem","SourceIsReference":false,"DestinationType":"Wizard","DestinationIsReference":false} */
            XRS2getInfoWizard.WireTo(id_4216ce5152f947a3a426ae090a8ad744, "children"); /* {"SourceType":"Wizard","SourceIsReference":false,"DestinationType":"WizardItem","DestinationIsReference":false} */
            XRS2getInfoWizard.WireTo(id_df57bd52cb3a4d23ba1c48843960fb51, "children"); /* {"SourceType":"Wizard","SourceIsReference":false,"DestinationType":"WizardItem","DestinationIsReference":false} */
            XRS2getInfoWizard.WireTo(id_de93ebf8c103419db2e78ce17ae77d25, "children"); /* {"SourceType":"Wizard","SourceIsReference":false,"DestinationType":"WizardItem","DestinationIsReference":false} */
            XRS2getInfoWizard.WireTo(id_cc1a52c5e51a41a393bb650f6d028f82, "children"); /* {"SourceType":"Wizard","SourceIsReference":false,"DestinationType":"WizardItem","DestinationIsReference":false} */
            id_4216ce5152f947a3a426ae090a8ad744.WireTo(saveScpSessionsDataToFileBrowser, "eventOutput"); /* {"SourceType":"WizardItem","SourceIsReference":false,"DestinationType":"SaveFileBrowser","DestinationIsReference":true} */
            id_df57bd52cb3a4d23ba1c48843960fb51.WireTo(saveScpLifeDataToFileBrowser, "eventOutput"); /* {"SourceType":"WizardItem","SourceIsReference":false,"DestinationType":"SaveFileBrowser","DestinationIsReference":true} */
            id_de93ebf8c103419db2e78ce17ae77d25.WireTo(saveAlertDataToFileBrowser, "eventOutput"); /* {"SourceType":"WizardItem","SourceIsReference":false,"DestinationType":"SaveFileBrowser","DestinationIsReference":true} */
            XRS2ExportMenu.WireTo(XRS2putInfoWizard, "eventOutput"); /* {"SourceType":"MenuItem","SourceIsReference":false,"DestinationType":"Wizard","DestinationIsReference":false} */
            XRS2putInfoWizard.WireTo(id_d460f06dbd154e6384414880181c02c6, "children"); /* {"SourceType":"Wizard","SourceIsReference":false,"DestinationType":"WizardItem","DestinationIsReference":false} */
            XRS2putInfoWizard.WireTo(id_597725dd93f142ef8fe04aac94e4ef93, "children"); /* {"SourceType":"Wizard","SourceIsReference":false,"DestinationType":"WizardItem","DestinationIsReference":false} */
            XRS2putInfoWizard.WireTo(id_dece0c065b8c4e6ea2760f8fc59ae5e6, "children"); /* {"SourceType":"Wizard","SourceIsReference":false,"DestinationType":"WizardItem","DestinationIsReference":false} */
            XRS2putInfoWizard.WireTo(id_aafe87cd67214b5a96d5e01feb73055d, "children"); /* {"SourceType":"Wizard","SourceIsReference":false,"DestinationType":"WizardItem","DestinationIsReference":false} */
            id_d460f06dbd154e6384414880181c02c6.WireTo(uploadSCPSessionsToDevice, "eventOutput"); /* {"SourceType":"WizardItem","SourceIsReference":false,"DestinationType":"OpenFileBrowser","DestinationIsReference":true} */
            XRS2Tools.WireTo(XRS2ImportTool, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Tool","DestinationIsReference":false} */
            XRS2Tools.WireTo(XRS2ExportTool, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Tool","DestinationIsReference":false} */
            XRS2Tools.WireTo(XRS2DeleteTool, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Tool","DestinationIsReference":false} */
            XRS2DeleteTool.WireTo(EIDDeleteSessions, "eventOutput"); /* {"SourceType":"Tool","SourceIsReference":false,"DestinationType":"Wizard","DestinationIsReference":true} */
            XRS2DeleteMenu.WireTo(EIDDeleteSessions, "eventOutput"); /* {"SourceType":"MenuItem","SourceIsReference":false,"DestinationType":"Wizard","DestinationIsReference":true} */
            // END AUTO-GENERATED WIRING FOR XRS2_Functionalities
            #endregion WIRING

            #region MainDiagram WIRING
            // BEGIN AUTO-GENERATED WIRING FOR MainDiagram
            mainWindow.WireTo(id_0ks2hj4mlls0q8icpq3ubeqd5q, "iuiStructure"); /* {"SourceType":"MainWindow","SourceIsReference":true,"DestinationType":"Vertical","DestinationIsReference":false} */
            id_0ks2hj4mlls0q8icpq3ubeqd5q.WireTo(menubar, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_0ks2hj4mlls0q8icpq3ubeqd5q.WireTo(toolbar, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_0ks2hj4mlls0q8icpq3ubeqd5q.WireTo(dataPanels, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_0ks2hj4mlls0q8icpq3ubeqd5q.WireTo(statusLine, "children"); /* {"SourceType":"Vertical","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            menubar.WireTo(fileMenu, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Menu","DestinationIsReference":false} */
            menubar.WireTo(helpMenu, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Menu","DestinationIsReference":false} */
            fileMenu.WireTo(ImportMenuItem, "children"); /* {"SourceType":"Menu","SourceIsReference":false,"DestinationType":"MenuItem","DestinationIsReference":false} */
            fileMenu.WireTo(exitApp, "children"); /* {"SourceType":"Menu","SourceIsReference":false,"DestinationType":"MenuItem","DestinationIsReference":false} */
            ImportMenuItem.WireTo(destination, "eventOutput"); /* {"SourceType":"MenuItem","SourceIsReference":false,"DestinationType":"Wizard","DestinationIsReference":false} */
            destination.WireTo(saveSelectedSessionToCSV, "children"); /* {"SourceType":"Wizard","SourceIsReference":false,"DestinationType":"WizardItem","DestinationIsReference":false} */
            destination.WireTo(sendToCloud, "children"); /* {"SourceType":"Wizard","SourceIsReference":false,"DestinationType":"WizardItem","DestinationIsReference":false} */
            saveSelectedSessionToCSV.WireTo(saveScpSessionsDataToFileBrowser, "eventOutput"); /* {"SourceType":"WizardItem","SourceIsReference":false,"DestinationType":"SaveFileBrowser","DestinationIsReference":false} */
            toolbar.WireTo(ImportToCSV, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Tool","DestinationIsReference":false} */
            dataPanels.WireTo(sessionListGrid, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Grid","DestinationIsReference":false} */
            dataPanels.WireTo(sessionDataGrid, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Grid","DestinationIsReference":false} */
            statusLine.WireTo(textSearchingPorts, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            statusLine.WireTo(textDeviceConnected, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            sessionDataGrid.WireTo(sessionDataForGrid, "?ITableDataFlow"); /* {"SourceType":"Grid","SourceIsReference":false,"DestinationType":"SessionDataSCP","DestinationIsReference":false} */
            sessionDataTransact.WireTo(scpSessionFilesWriter, "tableDataFlowDestination"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"CSVFileReaderWriter","DestinationIsReference":false} */
            filterEmptyDates.WireTo(sortByDate, "tableDataFlow"); /* {"SourceType":"Filter","SourceIsReference":false,"DestinationType":"Sort","DestinationIsReference":false} */
            sortByDate.WireTo(sessionListSelectionMap, "sourceDataFlow"); /* {"SourceType":"Sort","SourceIsReference":false,"DestinationType":"Map","DestinationIsReference":false} */
            sessionListGrid.WireTo(sessionDataForGrid, "dataFlowSelectedPrimaryKey"); /* {"SourceType":"Grid","SourceIsReference":false,"DestinationType":"SessionDataSCP","DestinationIsReference":false} */
            comPortAdapter.WireTo(scpProtocol, "charFromPort"); /* {"SourceType":"COMPortAdapter","SourceIsReference":false,"DestinationType":"SCPProtocol","DestinationIsReference":false} */
            scpDeviceSense.WireTo(scpProtocol, "SCPRequestResponse_B"); /* {"SourceType":"SCPDeviceSense","SourceIsReference":false,"DestinationType":"SCPProtocol","DestinationIsReference":false} */
            sessionListScp.WireTo(scpProtocol, "requestResponseDataFlow_B"); /* {"SourceType":"SessionListSCP","SourceIsReference":false,"DestinationType":"SCPProtocol","DestinationIsReference":false} */
            scpDeviceSense.WireTo(scpArbitrator, "arbitrator"); /* {"SourceType":"SCPDeviceSense","SourceIsReference":false,"DestinationType":"Arbitrator","DestinationIsReference":false} */
            sessionListScp.WireTo(scpArbitrator, "arbitrator"); /* {"SourceType":"SessionListSCP","SourceIsReference":false,"DestinationType":"Arbitrator","DestinationIsReference":false} */
            sessionDataForGrid.WireTo(scpArbitrator, "arbitrator"); /* {"SourceType":"SessionDataSCP","SourceIsReference":false,"DestinationType":"Arbitrator","DestinationIsReference":false} */
            saveScpSessionsDataToFileBrowser.WireTo(scpSessionFilesWriter, "dataFlowOutputFilePaths"); /* {"SourceType":"SaveFileBrowser","SourceIsReference":false,"DestinationType":"CSVFileReaderWriter","DestinationIsReference":false} */
            scpDeviceSense.WireTo(id_b450cfaa54744d0a978d3aa700de0f9a, "connected"); /* {"SourceType":"SCPDeviceSense","SourceIsReference":false,"DestinationType":"ToEvent","DestinationIsReference":false} */
            scpDeviceSense.WireTo(textDeviceConnected, "connected"); /* {"SourceType":"SCPDeviceSense","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            scpDeviceSense.WireTo(id_91dac706bbe643529fca59fdc379a223, "connected"); /* {"SourceType":"SCPDeviceSense","SourceIsReference":false,"DestinationType":"Not","DestinationIsReference":false} */
            id_91dac706bbe643529fca59fdc379a223.WireTo(textSearchingPorts, "reversedInput"); /* {"SourceType":"Not","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            sessionDataForGrid.WireTo(scpProtocol, "requestResponseDataFlow"); /* {"SourceType":"SessionDataSCP","SourceIsReference":false,"DestinationType":"SCPProtocol","DestinationIsReference":false} */
            pollSerial.WireTo(scpDeviceSense, "eventHappened"); /* {"SourceType":"Timer","SourceIsReference":false,"DestinationType":"SCPDeviceSense","DestinationIsReference":false} */
            id_b450cfaa54744d0a978d3aa700de0f9a.WireTo(sessionListGrid, "eventOutput"); /* {"SourceType":"ToEvent","SourceIsReference":false,"DestinationType":"Grid","DestinationIsReference":false} */
            sessionListGrid.WireTo(id_f9a3c2f1b4eb46d386e9b76a0d264683, "dataFlowSelectedPrimaryKey"); /* {"SourceType":"Grid","SourceIsReference":false,"DestinationType":"ToEvent","DestinationIsReference":false} */
            id_f9a3c2f1b4eb46d386e9b76a0d264683.WireTo(sessionDataGrid, "eventOutput"); /* {"SourceType":"ToEvent","SourceIsReference":false,"DestinationType":"Grid","DestinationIsReference":false} */
            sessionListGrid.WireTo(sessionDataForImport, "dataFlowSelectedPrimaryKey"); /* {"SourceType":"Grid","SourceIsReference":false,"DestinationType":"SessionDataSCP","DestinationIsReference":false} */
            sessionDataForImport.WireTo(scpArbitrator, "arbitrator"); /* {"SourceType":"SessionDataSCP","SourceIsReference":false,"DestinationType":"Arbitrator","DestinationIsReference":false} */
            sessionDataForImport.WireTo(scpProtocol, "requestResponseDataFlow"); /* {"SourceType":"SessionDataSCP","SourceIsReference":false,"DestinationType":"SCPProtocol","DestinationIsReference":false} */
            sessionDataTransact.WireTo(sessionDataForImport, "tableDataFlowSource"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"SessionDataSCP","DestinationIsReference":false} */
            mainWindow.WireTo(pollSerial, "appStartsRun"); /* {"SourceType":"MainWindow","SourceIsReference":true,"DestinationType":"Timer","DestinationIsReference":false} */
            saveScpSessionsDataToFileBrowser.WireTo(id_ced2fdc009d34ef79c279aa1828c7744, "dataFlowOutputFilePaths"); /* {"SourceType":"SaveFileBrowser","SourceIsReference":false,"DestinationType":"ToEvent","DestinationIsReference":false} */
            id_ced2fdc009d34ef79c279aa1828c7744.WireTo(sessionDataTransact, "eventOutput"); /* {"SourceType":"ToEvent","SourceIsReference":false,"DestinationType":"Transact","DestinationIsReference":false} */
            exitApp.WireTo(mainWindow, "eventOutput"); /* {"SourceType":"MenuItem","SourceIsReference":false,"DestinationType":"MainWindow","DestinationIsReference":true} */
            // END AUTO-GENERATED WIRING FOR MainDiagram
            #endregion


            #region Ethernet_DeviceDetection WIRING
            // BEGIN AUTO-GENERATED WIRING FOR Ethernet_DeviceDetection
            timerForEthernetDetection.WireTo(id_9ce85ddd4a3a493d8844407bc3b6198c, "eventHappened"); /* {"SourceType":"Timer","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_9ce85ddd4a3a493d8844407bc3b6198c.WireTo(ethernetDeviceDetectionGate, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventGate","DestinationIsReference":false} */
            id_9ce85ddd4a3a493d8844407bc3b6198c.WireTo(deviceDrive, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"DeviceDrive","DestinationIsReference":false} */
            noDeviceConnectedConnector.WireTo(ethernetDeviceDetectionGate, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":true,"DestinationType":"EventGate","DestinationIsReference":false} */
            ethernetDeviceDetectionGate.WireTo(ethernetSwitchToUSB, "eventOutput"); /* {"SourceType":"EventGate","SourceIsReference":false,"DestinationType":"EthernetSwitchToUSB","DestinationIsReference":false} */
            ethernetSwitchToUSB.WireTo(deviceDrive, "ethernetSwitchCompleted"); /* {"SourceType":"EthernetSwitchToUSB","SourceIsReference":false,"DestinationType":"DeviceDrive","DestinationIsReference":false} */
            deviceDrive.WireTo(usbDeviceNameGate, "usbDeviceName"); /* {"SourceType":"DeviceDrive","SourceIsReference":false,"DestinationType":"DataFlowGate","DestinationIsReference":false} */
            deviceDrive.WireTo(usbDeviceDbPath, "usbDeviceDbPath"); /* {"SourceType":"DeviceDrive","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            deviceDrive.WireTo(deviceDrivePath, "deviceDrivePath"); /* {"SourceType":"DeviceDrive","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            deviceDrive.WireTo(Scale5000CurrentVersionAdapter, "deviceFirmwareVersion"); /* {"SourceType":"DeviceDrive","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":true} */
            deviceDrivePath.WireTo(id_c6f16c53ee3f45c0959fb9a9eb77f428, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"StringModifier","DestinationIsReference":false} */
            id_c6f16c53ee3f45c0959fb9a9eb77f428.WireTo(favSettingFolderPath, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            deviceDrivePath.WireTo(id_972cac857c904b1f9c1a1aff369a6fdd, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            id_972cac857c904b1f9c1a1aff369a6fdd.WireTo(polarisFilePath, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            Scale5000FirmwareTuple.WireTo(deviceDrivePath, "portA"); /* {"SourceType":"TupleAbstraction","SourceIsReference":true,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            Scale5000FileFirmwareTuple.WireTo(deviceDrivePath, "portA"); /* {"SourceType":"TupleAbstraction","SourceIsReference":true,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            usbDeviceNameGate.WireTo(usbDeviceNameConnector, "dataOutput"); /* {"SourceType":"DataFlowGate","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            usbDeviceNameConnector.WireTo(id_bb2b4c21eff14e5cb2515d0d3d766c5d, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Equals","DestinationIsReference":false} */
            usbDeviceNameConnector.WireTo(id_619d9e36e96a40e7bff6f8b7c33bed03, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Equals","DestinationIsReference":false} */
            usbDeviceNameConnector.WireTo(Scale5000NoUpdatesBox, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"GroupingBox","DestinationIsReference":true} */
            id_bb2b4c21eff14e5cb2515d0d3d766c5d.WireTo(XR5000ConnectedConnector, "isEqual"); /* {"SourceType":"Equals","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            usbDeviceConnectedOr.WireTo(XR5000ConnectedConnector, "listOfInputs"); /* {"SourceType":"Or","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_619d9e36e96a40e7bff6f8b7c33bed03.WireTo(ID5000ConnectedConnector, "isEqual"); /* {"SourceType":"Equals","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            usbDeviceConnectedOr.WireTo(ID5000ConnectedConnector, "listOfInputs"); /* {"SourceType":"Or","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            usbDeviceConnectedOr.WireTo(usbConnector, "booleanResult"); /* {"SourceType":"Or","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            anyDeviceConnectedOr.WireTo(usbConnector, "listOfInputs"); /* {"SourceType":"Or","SourceIsReference":true,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            usbConnector.WireTo(id_5c6b2afd88984eefbf3cb5d5c0545d05, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Not","DestinationIsReference":false} */
            id_5c6b2afd88984eefbf3cb5d5c0545d05.WireTo(reverseUSBConnectedValue, "reversedInput"); /* {"SourceType":"Not","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            scpDeviceNameGate.WireTo(reverseUSBConnectedValue, "triggerLatchInput"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"DataFlowGate","DestinationIsReference":true} */
            usbConnector.WireTo(usbEventConnectorGate, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"EventGate","DestinationIsReference":false} */
            usbEventConnectorGate.WireTo(usbConnectedEventConnector, "eventOutput"); /* {"SourceType":"EventGate","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            usbConnectedEventConnector.WireTo(progressWindowForStartUpSessionDisplay, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":true} */
            usbConnectedEventConnector.WireTo(convertTableForSessionName, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"ConvertTableToDataFlow","DestinationIsReference":true} */
            usbConnectedEventConnector.WireTo(convertTableForSessionCount, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"ConvertTableToDataFlow","DestinationIsReference":true} */
            usbConnectedEventConnector.WireTo(sessionListQueryTransact, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Transact","DestinationIsReference":true} */
            usbConnector.WireTo(TransactQueryMapToGridGate, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"EventGate","DestinationIsReference":true} */
            usbConnector.WireTo(usbSDQueryGate, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"EventGate","DestinationIsReference":true} */
            usbConnector.WireTo(id_6b148185d4af41f78522ee6c40b370f2, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"ConvertToEvent","DestinationIsReference":false} */
            id_6b148185d4af41f78522ee6c40b370f2.WireTo(id_9807433033264d54b0616ca69a49ab6f, "eventOutput"); /* {"SourceType":"ConvertToEvent","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_9807433033264d54b0616ca69a49ab6f.WireTo(usbEventConnectorGate, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventGate","DestinationIsReference":false} */
            id_9807433033264d54b0616ca69a49ab6f.WireTo(id_a3b8de1dd4f8451cac3e96b59f3acfe7, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventGate","DestinationIsReference":false} */
            id_3d906cf5fd6747b1974a4d0cd697efad.WireTo(id_a3b8de1dd4f8451cac3e96b59f3acfe7, "isEqual"); /* {"SourceType":"Equals","SourceIsReference":false,"DestinationType":"EventGate","DestinationIsReference":false} */
            id_a3b8de1dd4f8451cac3e96b59f3acfe7.WireTo(usbDisconnectedEventConnector, "eventOutput"); /* {"SourceType":"EventGate","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            usbDisconnectedEventConnector.WireTo(convertTableForSessionName, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"ConvertTableToDataFlow","DestinationIsReference":true} */
            usbDisconnectedEventConnector.WireTo(convertTableForSessionCount, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"ConvertTableToDataFlow","DestinationIsReference":true} */
            usbConnector.WireTo(id_3d906cf5fd6747b1974a4d0cd697efad, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Equals","DestinationIsReference":false} */
            usbConnector.WireTo(favouriteSetupRowButton, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"RowButton","DestinationIsReference":true} */
            usbConnector.WireTo(display5000FavSettingGate, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"EventGate","DestinationIsReference":true} */
            usbConnector.WireTo(usbDisplayLifeDataGate, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"EventGate","DestinationIsReference":true} */
            usbConnector.WireTo(lifeDataRowButton, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"RowButton","DestinationIsReference":true} */
            usbConnector.WireTo(notNeedCrossRefButtonWhenUSB, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Not","DestinationIsReference":false} */
            notNeedCrossRefButtonWhenUSB.WireTo(crossRefRowButton, "reversedInput"); /* {"SourceType":"Not","SourceIsReference":false,"DestinationType":"RowButton","DestinationIsReference":true} */
            usbConnector.WireTo(notNeedAlertButtonWhenUSB, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Not","DestinationIsReference":false} */
            notNeedAlertButtonWhenUSB.WireTo(alertRowButton, "reversedInput"); /* {"SourceType":"Not","SourceIsReference":false,"DestinationType":"RowButton","DestinationIsReference":true} */
            usbConnector.WireTo(Scale5000UpdateCheck, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"IfElse","DestinationIsReference":true} */
            usbConnector.WireTo(Scale5000UpdateFromFileCheck, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"IfElse","DestinationIsReference":true} */
            usbConnector.WireTo(Scale5000NoUpdatesBox, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"GroupingBox","DestinationIsReference":true} */
            usbConnector.WireTo(usbEqualsTrueToAutoUpdate, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Equals","DestinationIsReference":false} */
            usbEqualsTrueToAutoUpdate.WireTo(AutomaticUpdateConnector, "equalEvent"); /* {"SourceType":"Equals","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":true} */
            // END AUTO-GENERATED WIRING FOR Ethernet_DeviceDetection
            #endregion



            #region XR5000_SaveLifeDataToPC WIRING
            // BEGIN AUTO-GENERATED WIRING FOR XR5000_SaveLifeDataToPC
            id_832224a290c8495ea7cf37ec2e07fe14.WireTo(saveUsbLifeDataToFileBrowser, "dataFlowOutput"); /* {"SourceType":"StringFormat","SourceIsReference":false,"DestinationType":"SaveFileBrowser","DestinationIsReference":false} */
            saveUsbLifeDataToFileBrowser.WireTo(id_dc9f0162d8974ff6ba36cd039cedfd56, "dataFlowOutputFileNames"); /* {"SourceType":"SaveFileBrowser","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            saveUsbLifeDataToFileBrowser.WireTo(id_d0b224e6215d4095836828f399b07270, "dataFlowOutputFilePaths"); /* {"SourceType":"SaveFileBrowser","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            saveUsbLifeDataToFileBrowser.WireTo(connectorFileFullPath, "dataFlowOutputFilePathNames"); /* {"SourceType":"SaveFileBrowser","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            saveUsbLifeDataToFileBrowser.WireTo(exportLifeDataCSVFileReaderWriter, "dataFlowFileFormatIndex"); /* {"SourceType":"SaveFileBrowser","SourceIsReference":false,"DestinationType":"CSVFileReaderWriter","DestinationIsReference":false} */
            id_a95b6ba5cf004396afbd1a6cd05cb9c9.WireTo(id_dc9f0162d8974ff6ba36cd039cedfd56, "dataFlowBsList"); /* {"SourceType":"StringFormat","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_a95b6ba5cf004396afbd1a6cd05cb9c9.WireTo(id_d0b224e6215d4095836828f399b07270, "dataFlowBsList"); /* {"SourceType":"StringFormat","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            connectorFileFullPath.WireTo(exportLifeDataCSVFileReaderWriter, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"CSVFileReaderWriter","DestinationIsReference":false} */
            connectorFileFullPath.WireTo(id_29efd96d028e45c4bfab619922d9db66, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"ConvertToEvent","DestinationIsReference":false} */
            exportLifeDataCSVFileReaderWriter.WireTo(id_2d6c1d577a8343378c102a42e3090434, "dataFlowOpenOrCloseProgressWindow"); /* {"SourceType":"CSVFileReaderWriter","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            exportLifeDataCSVFileReaderWriter.WireTo(id_760cea92c8644796aa3dc1540b7e5fae, "dataFlowFilePath"); /* {"SourceType":"CSVFileReaderWriter","SourceIsReference":false,"DestinationType":"OpenWindowsExplorer","DestinationIsReference":false} */
            exportLifeDataCSVFileReaderWriter.WireTo(csvFileSaveSuccessWindow, "eventOutputSuccessWindow"); /* {"SourceType":"CSVFileReaderWriter","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_2d6c1d577a8343378c102a42e3090434.WireTo(id_1865fd6f2066404fbddc5f6293b008ad, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"ProgressBar","DestinationIsReference":false} */
            id_2d6c1d577a8343378c102a42e3090434.WireTo(id_7db53da6ab424f9a83159d7981d5be0d, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_2d6c1d577a8343378c102a42e3090434.WireTo(id_b881eeedf4274009810d98c79d4fd13b, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_1865fd6f2066404fbddc5f6293b008ad.WireTo(connectorLifeDataToFileProgress, "progressValue"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"ProgressBar","DestinationIsReference":false} */
            id_1865fd6f2066404fbddc5f6293b008ad.WireTo(connectorLifeDataToFileTotalCount, "maximumValue"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"ProgressBar","DestinationIsReference":false} */
            animalInfoCount.WireTo(connectorLifeDataToFileTotalCount, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":true,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_b5b783c369bc429b89780f56090b7a69.WireTo(id_7db53da6ab424f9a83159d7981d5be0d, "dataFlowOutput"); /* {"SourceType":"StringFormat","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_b5b783c369bc429b89780f56090b7a69.WireTo(connectorLifeDataToFileProgress, "dataFlowBsList"); /* {"SourceType":"StringFormat","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_b5b783c369bc429b89780f56090b7a69.WireTo(connectorLifeDataToFileTotalCount, "dataFlowBsList"); /* {"SourceType":"StringFormat","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_b881eeedf4274009810d98c79d4fd13b.WireTo(id_ecf1f1a325f54a6abeb527f479428538, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_b881eeedf4274009810d98c79d4fd13b.WireTo(id_ada2df96bba94b7a9c6fe6b04c082b84, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            csvFileSaveSuccessWindow.WireTo(id_dbff056362cc465fb29dde052e6908c0, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            csvFileSaveSuccessWindow.WireTo(id_589bcf0180be4675a50baeb1bb52ceb6, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            csvFileSaveSuccessWindow.WireTo(id_d080e7162acb481298961fea3b1961bf, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_dbff056362cc465fb29dde052e6908c0.WireTo(id_29c3028618f64252a920bc95ba4ae708, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Picture","DestinationIsReference":false} */
            id_dbff056362cc465fb29dde052e6908c0.WireTo(id_318c926a79954f32b8cf3dc131b18c2f, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_a95b6ba5cf004396afbd1a6cd05cb9c9.WireTo(id_589bcf0180be4675a50baeb1bb52ceb6, "dataFlowOutput"); /* {"SourceType":"StringFormat","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_d080e7162acb481298961fea3b1961bf.WireTo(id_2e9c0ae6f69a40c38ecbe3beeb36f07a, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"RightJustify","DestinationIsReference":false} */
            id_2e9c0ae6f69a40c38ecbe3beeb36f07a.WireTo(id_59f58417545646729ede73333dd6413e, "children"); /* {"SourceType":"RightJustify","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_2e9c0ae6f69a40c38ecbe3beeb36f07a.WireTo(id_d73da13c3df14fe6a4b81835cb38cd16, "children"); /* {"SourceType":"RightJustify","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_59f58417545646729ede73333dd6413e.WireTo(id_7df3d4e5d6d8454db8c295f09aa169cb, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_7df3d4e5d6d8454db8c295f09aa169cb.WireTo(id_760cea92c8644796aa3dc1540b7e5fae, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"OpenWindowsExplorer","DestinationIsReference":false} */
            id_7df3d4e5d6d8454db8c295f09aa169cb.WireTo(csvFileSaveSuccessWindow, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_d73da13c3df14fe6a4b81835cb38cd16.WireTo(csvFileSaveSuccessWindow, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_29efd96d028e45c4bfab619922d9db66.WireTo(lifeDataToCSVFileTransact, "eventOutput"); /* {"SourceType":"ConvertToEvent","SourceIsReference":false,"DestinationType":"Transact","DestinationIsReference":false} */
            lifeDataToCSVFileTransact.WireTo(animalInfoQuerySelect, "tableDataFlowSource"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"SelectColumns","DestinationIsReference":true} */
            lifeDataToCSVFileTransact.WireTo(exportLifeDataCSVFileReaderWriter, "tableDataFlowDestination"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"CSVFileReaderWriter","DestinationIsReference":false} */
            id_832224a290c8495ea7cf37ec2e07fe14.WireTo(usbDeviceNameConnector, "dataFlowBsList"); /* {"SourceType":"StringFormat","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":true} */
            // END AUTO-GENERATED WIRING FOR XR5000_SaveLifeDataToPC
            #endregion

            #region XR5000_UploadSessionsToDevice WIRING
            // BEGIN AUTO-GENERATED WIRING FOR XR5000_UploadSessionsToDevice
            uploadSessionFromPCToXr5000.WireTo(uploadSessionFilesTotal, "dataFlowSelectedFileCount"); /* {"SourceType":"OpenFileBrowser","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            transactFilesInfoToBeIterated.WireTo(uploadSessionFromPCToXr5000, "tableDataFlowSource"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"OpenFileBrowser","DestinationIsReference":false} */
            uploadSessionFromPCToXr5000.WireTo(id_ca5ff8c23a5540d5801857f94587497c, "eventFileSelectedOutput"); /* {"SourceType":"OpenFileBrowser","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_ca5ff8c23a5540d5801857f94587497c.WireTo(transactFilesInfoToBeIterated, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Transact","DestinationIsReference":false} */
            id_ca5ff8c23a5540d5801857f94587497c.WireTo(uploadXR5000SessionsToDevicePopupWindow, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            uploadXR5000SessionsToDevicePopupWindow.WireTo(id_fa40cfa552a04e52ac8b95832cd1a294, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"ProgressBar","DestinationIsReference":false} */
            id_fa40cfa552a04e52ac8b95832cd1a294.WireTo(uploadSessionRecordsProgress, "progressValue"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"ProgressBar","DestinationIsReference":false} */
            id_fa40cfa552a04e52ac8b95832cd1a294.WireTo(uploadSessionRecordsTotal, "maximumValue"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"ProgressBar","DestinationIsReference":false} */
            uploadXR5000SessionsToDevicePopupWindow.WireTo(id_3b2899654a4945839a950f0e7e9b6e1c, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            uploadXR5000SessionsToDevicePopupWindow.WireTo(id_ddafb5d7f2164179901d320dda93364a, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"ProgressBar","DestinationIsReference":false} */
            uploadXR5000SessionsToDevicePopupWindow.WireTo(id_2e2be7441f4340b8a9a953823103325b, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            uploadXR5000SessionsToDevicePopupWindow.WireTo(id_19a7df63ae6240478a415f736094c0f4, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_19a7df63ae6240478a415f736094c0f4.WireTo(id_5f624f1f40884a67bbccc223fe7280e8, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_19a7df63ae6240478a415f736094c0f4.WireTo(id_eebdc2c8e9124163a245a605f60b414f, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_ddafb5d7f2164179901d320dda93364a.WireTo(uploadSessionFilesTotal, "maximumValue"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"ProgressBar","DestinationIsReference":false} */
            id_ddafb5d7f2164179901d320dda93364a.WireTo(uploadSessionFilesProgress, "progressValue"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"ProgressBar","DestinationIsReference":false} */
            id_70786492fd1c4df89f024ac7bb5f1fc8.WireTo(uploadSessionFilesProgress, "dataFlowBsList"); /* {"SourceType":"StringFormat","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_70786492fd1c4df89f024ac7bb5f1fc8.WireTo(uploadSessionFilesTotal, "dataFlowBsList"); /* {"SourceType":"StringFormat","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_70786492fd1c4df89f024ac7bb5f1fc8.WireTo(id_2e2be7441f4340b8a9a953823103325b, "dataFlowOutput"); /* {"SourceType":"StringFormat","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_4884b49de9644a119c9311d6d3ed0eeb.WireTo(id_3b2899654a4945839a950f0e7e9b6e1c, "dataFlowOutput"); /* {"SourceType":"StringFormat","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            transactFilesInfoToBeIterated.WireTo(uploadSessionFileIterator, "tableDataFlowDestination"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"Iterator","DestinationIsReference":false} */
            uploadSessionFileIterator.WireTo(extractTableForPathName, "currentRowOutput"); /* {"SourceType":"Iterator","SourceIsReference":false,"DestinationType":"ConvertTableToDataFlow","DestinationIsReference":false} */
            uploadSessionFileIterator.WireTo(id_38aa845407074d39813f7b4cc677bf5a, "iteratorRunningOutput"); /* {"SourceType":"Iterator","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            uploadSessionFileIterator.WireTo(uploadSessionFilesProgress, "indexOutput"); /* {"SourceType":"Iterator","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            uploadSessionFileIterator.WireTo(uploadXR5000SessionsToDevicePopupWindow, "eventCompleteOutput"); /* {"SourceType":"Iterator","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            extractTableForPathName.WireTo(id_a41c590961f844b9aea31f08bcf9e474, "cellOutput"); /* {"SourceType":"ConvertTableToDataFlow","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_a41c590961f844b9aea31f08bcf9e474.WireTo(id_73ec4ef08f8d4014ab401b32434f245e, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"FileReader","DestinationIsReference":false} */
            id_73ec4ef08f8d4014ab401b32434f245e.WireTo(getFileNumberFromFileContent, "fileContent"); /* {"SourceType":"FileReader","SourceIsReference":false,"DestinationType":"ConvertTableToDataFlow","DestinationIsReference":false} */
            id_73ec4ef08f8d4014ab401b32434f245e.WireTo(getFileNameFromFileContent, "fileContent"); /* {"SourceType":"FileReader","SourceIsReference":false,"DestinationType":"ConvertTableToDataFlow","DestinationIsReference":false} */
            id_73ec4ef08f8d4014ab401b32434f245e.WireTo(getFileDateFromFileContent, "fileContent"); /* {"SourceType":"FileReader","SourceIsReference":false,"DestinationType":"ConvertTableToDataFlow","DestinationIsReference":false} */
            id_73ec4ef08f8d4014ab401b32434f245e.WireTo(sessionContentTable, "fileContent"); /* {"SourceType":"FileReader","SourceIsReference":false,"DestinationType":"Table","DestinationIsReference":false} */
            id_73ec4ef08f8d4014ab401b32434f245e.WireTo(fileName, "fileName"); /* {"SourceType":"FileReader","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_73ec4ef08f8d4014ab401b32434f245e.WireTo(fileDate, "fileCreationDate"); /* {"SourceType":"FileReader","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            getFileNumberFromFileContent.WireTo(id_1e5cc31a6a604f03bb8ede181e834788, "cellOutput"); /* {"SourceType":"ConvertTableToDataFlow","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_1e5cc31a6a604f03bb8ede181e834788.WireTo(extractFileNumber, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            id_1e5cc31a6a604f03bb8ede181e834788.WireTo(containsFileNumber, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            extractFileNumber.WireTo(fileNumber, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            sessionDataQuery.WireTo(fileNumber, "sessionNumberDataFlow"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"QueryGeneratorSessionData","DestinationIsReference":true} */
            containsFileNumber.WireTo(id_4552540ce40e4051ab5682b1eb25286e, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            is3000Format.WireTo(id_4552540ce40e4051ab5682b1eb25286e, "listOfInputs"); /* {"SourceType":"Or","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            is3000Format.WireTo(FiveRowsNeededToBeRemovedIf3000, "booleanResult"); /* {"SourceType":"Or","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            FiveRowsNeededToBeRemovedIf3000.WireTo(id_cc7c4c0b6b5a45678130d86f85ac0bd7, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            subtract3000HeadersRowsFromCount.WireTo(id_cc7c4c0b6b5a45678130d86f85ac0bd7, "inputTwo"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Subtract","DestinationIsReference":false} */
            id_cc7c4c0b6b5a45678130d86f85ac0bd7.WireTo(filterSessionFileForOnlyContentAndNo3000FormatHeader, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Filter","DestinationIsReference":false} */
            filterSessionFileForOnlyContentAndNo3000FormatHeader.WireTo(sessionContentTable, "tableDataFlow"); /* {"SourceType":"Filter","SourceIsReference":false,"DestinationType":"Table","DestinationIsReference":false} */
            getFileNameFromFileContent.WireTo(id_18ebb5f217624de1a5343d94c8b6368c, "cellOutput"); /* {"SourceType":"ConvertTableToDataFlow","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_18ebb5f217624de1a5343d94c8b6368c.WireTo(extractFileName, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            id_18ebb5f217624de1a5343d94c8b6368c.WireTo(containsFileName, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            extractFileName.WireTo(fileName, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            containsFileName.WireTo(id_f0274770209a4c40ab5ab86202d1872c, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            is3000Format.WireTo(id_f0274770209a4c40ab5ab86202d1872c, "listOfInputs"); /* {"SourceType":"Or","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            getFileDateFromFileContent.WireTo(id_3d27570497bb42dbb1ae2233cb93ec5a, "cellOutput"); /* {"SourceType":"ConvertTableToDataFlow","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_3d27570497bb42dbb1ae2233cb93ec5a.WireTo(extractFileDate, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            id_3d27570497bb42dbb1ae2233cb93ec5a.WireTo(containsFileDate, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            extractFileDate.WireTo(id_20228209bd8b404dbba386585f979588, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            id_20228209bd8b404dbba386585f979588.WireTo(fileDate, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            containsFileDate.WireTo(id_85d8b4eb3a0241a3b6277746621420f0, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            is3000Format.WireTo(id_85d8b4eb3a0241a3b6277746621420f0, "listOfInputs"); /* {"SourceType":"Or","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            sessionContentTable.WireTo(sessionFileTotalRowCount, "dataTableOutput"); /* {"SourceType":"Table","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            sessionContentTable.WireTo(id_c78ba23a885f4379914995fdcf87c904, "tableRowCount"); /* {"SourceType":"Table","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            subtract3000HeadersRowsFromCount.WireTo(id_c78ba23a885f4379914995fdcf87c904, "inputOne"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Subtract","DestinationIsReference":false} */
            subtract3000HeadersRowsFromCount.WireTo(uploadSessionRecordsTotal, "outputResult"); /* {"SourceType":"Subtract","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_4884b49de9644a119c9311d6d3ed0eeb.WireTo(uploadSessionRecordsTotal, "dataFlowBsList"); /* {"SourceType":"StringFormat","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            sessionDataQuery.WireTo(fileName, "sessionNameDataFlow"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"QueryGeneratorSessionData","DestinationIsReference":true} */
            sessionDataQuery.WireTo(fileDate, "sessionDateDataFlow"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"QueryGeneratorSessionData","DestinationIsReference":true} */
            fileDate.WireTo(id_7190cb3c6a4f420db0a1076e6ef54b2c, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            id_7190cb3c6a4f420db0a1076e6ef54b2c.WireTo(dateStamp, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            sessionDataQuery.WireTo(dateStamp, "sessionDateStampDataFlow"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"QueryGeneratorSessionData","DestinationIsReference":true} */
            id_a41c590961f844b9aea31f08bcf9e474.WireTo(id_223e0b52ddfe43c69339f9de2caf8959, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"ConvertToEvent","DestinationIsReference":false} */
            id_223e0b52ddfe43c69339f9de2caf8959.WireTo(id_739f7aaea4714f33bba71fd90ad9e75d, "eventOutput"); /* {"SourceType":"ConvertToEvent","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_739f7aaea4714f33bba71fd90ad9e75d.WireTo(upload5000SessionFileToDeviceTransact, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Transact","DestinationIsReference":false} */
            upload5000SessionFileToDeviceTransact.WireTo(filterSessionFileForOnlyContentAndNo3000FormatHeader, "tableDataFlowSource"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"Filter","DestinationIsReference":false} */
            upload5000SessionFileToDeviceTransact.WireTo(sessionDataQuery, "tableDataFlowDestination"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"QueryGeneratorSessionData","DestinationIsReference":true} */
            upload5000SessionFileToDeviceTransact.WireTo(uploadSessionRecordsProgress, "dataFlowsIndex"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            upload5000SessionFileToDeviceTransact.WireTo(id_ab7204b70dd14d1bb9cd3e6d9e831954, "eventCompleteNoErrors"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_ab7204b70dd14d1bb9cd3e6d9e831954.WireTo(sessionListQueryTransact, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Transact","DestinationIsReference":true} */
            id_ab7204b70dd14d1bb9cd3e6d9e831954.WireTo(uploadSessionFileIterator, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Iterator","DestinationIsReference":false} */
            // END AUTO-GENERATED WIRING FOR XR5000_UploadSessionsToDevice
            #endregion

            #region FirmwareUpdate WIRING
            // BEGIN AUTO-GENERATED WIRING FOR FirmwareUpdate
            CheckForUpdatesConnector.WireTo(InternetCheckFirmware, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"IfElse","DestinationIsReference":false} */
            internetConnectionDetectedConnector.WireTo(InternetCheckFirmware, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":true,"DestinationType":"IfElse","DestinationIsReference":false} */
            InternetCheckFirmware.WireTo(id_022a3938ab7a4c1fb65677843f804e7f, "ifOutput"); /* {"SourceType":"IfElse","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            InternetCheckFirmware.WireTo(UpdateNoInternetConnection, "elseOutput"); /* {"SourceType":"IfElse","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_022a3938ab7a4c1fb65677843f804e7f.WireTo(srsUpdateRequest, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"HttpRequest","DestinationIsReference":false} */
            id_022a3938ab7a4c1fb65677843f804e7f.WireTo(datalinkUpdateRequest, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"HttpRequest","DestinationIsReference":false} */
            id_022a3938ab7a4c1fb65677843f804e7f.WireTo(VersionCollector, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"FirmwareVersionCollector","DestinationIsReference":false} */
            srsUpdateRequest.WireTo(XRSUpdateParser, "responseJsonOutput"); /* {"SourceType":"HttpRequest","SourceIsReference":false,"DestinationType":"XMLParser","DestinationIsReference":false} */
            VersionCollector.WireTo(XRSUpdateParser, "firmwareVersionInputs"); /* {"SourceType":"FirmwareVersionCollector","SourceIsReference":false,"DestinationType":"XMLParser","DestinationIsReference":false} */
            datalinkUpdateRequest.WireTo(DatalinkUpdateParser, "responseJsonOutput"); /* {"SourceType":"HttpRequest","SourceIsReference":false,"DestinationType":"XMLParser","DestinationIsReference":false} */
            VersionCollector.WireTo(DatalinkUpdateParser, "firmwareVersionInputs"); /* {"SourceType":"FirmwareVersionCollector","SourceIsReference":false,"DestinationType":"XMLParser","DestinationIsReference":false} */
            VersionCollector.WireTo(id_86d8895b0b0f4f9aaa68a972fe89d413, "firmwareVersionsOutput"); /* {"SourceType":"FirmwareVersionCollector","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            VersionCollector.WireTo(id_ef17163624f54b978edf4ab11ead0913, "dataCollected"); /* {"SourceType":"FirmwareVersionCollector","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_86d8895b0b0f4f9aaa68a972fe89d413.WireTo(id_31bfaf1ab46b4e27a451490a9d8a438d, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"RetrieveValueFromDict","DestinationIsReference":false} */
            id_86d8895b0b0f4f9aaa68a972fe89d413.WireTo(id_b95f831e77cf42e9a33d1a89a81757c2, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"RetrieveValueFromDict","DestinationIsReference":false} */
            id_86d8895b0b0f4f9aaa68a972fe89d413.WireTo(id_72fe7772e6c94325b3094fbd20f76e6d, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"RetrieveValueFromDict","DestinationIsReference":false} */
            id_86d8895b0b0f4f9aaa68a972fe89d413.WireTo(id_6cde3c2e162d4f34bdb85b3602cc46fc, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"RetrieveValueFromDict","DestinationIsReference":false} */
            id_31bfaf1ab46b4e27a451490a9d8a438d.WireTo(DatalinkUpdateInformation, "output"); /* {"SourceType":"RetrieveValueFromDict","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            DatalinkUpdateInformation.WireTo(DatalinkVersionCompare, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"CompareFirmwareVersions","DestinationIsReference":false} */
            DatalinkUpdateInformation.WireTo(DatalinkRetrieveURL, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            DatalinkUpdateInformation.WireTo(DatalinkVersionCompareString, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            DatalinkUpdateInformation.WireTo(DatalinkGetFirmwareHash, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            DatalinkUpdateInformation.WireTo(DatalinkFirmwarePathFormatter, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            DatalinkRetrieveURL.WireTo(DatalinkURLConnector, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            DatalinkURLConnector.WireTo(DatalinkGetFirmwareSize, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"GetDownloadableFileSize","DestinationIsReference":false} */
            DatalinkURLConnector.WireTo(DatalinkUpdateDownloader, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"DownloadFile","DestinationIsReference":false} */
            id_b95f831e77cf42e9a33d1a89a81757c2.WireTo(SRS2UpdateInformation, "output"); /* {"SourceType":"RetrieveValueFromDict","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_72fe7772e6c94325b3094fbd20f76e6d.WireTo(XRS2UpdateInformation, "output"); /* {"SourceType":"RetrieveValueFromDict","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_6cde3c2e162d4f34bdb85b3602cc46fc.WireTo(Scale5000UpdateInformation, "output"); /* {"SourceType":"RetrieveValueFromDict","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            Scale5000UpdateInformation.WireTo(Scale5000CompareVersions, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"CompareFirmwareVersions","DestinationIsReference":false} */
            Scale5000UpdateInformation.WireTo(Scale5000RetrieveURL, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            Scale5000UpdateInformation.WireTo(Scale5000NewVersionAdapter, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            Scale5000UpdateInformation.WireTo(Scale5000FirmwarePathFormatter, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            Scale5000UpdateInformation.WireTo(Scale5000GetFirmwareHash, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            Scale5000FirmwareTuple.WireTo(Scale5000UpdateInformation, "portB"); /* {"SourceType":"TupleAbstraction","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            Scale5000RetrieveURL.WireTo(Scale5000URLConnector, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            Scale5000URLConnector.WireTo(Scale5000GetFirmwareSize, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"GetDownloadableFileSize","DestinationIsReference":false} */
            Scale5000URLConnector.WireTo(Scale5000UpdateDownloader, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"DownloadFile","DestinationIsReference":false} */
            DatalinkUpdateArbitrator.WireTo(id_ef17163624f54b978edf4ab11ead0913, "request"); /* {"SourceType":"RequestArbitrator","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_ef17163624f54b978edf4ab11ead0913.WireTo(NoUpdatesAvailableGate, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"AndEvent","DestinationIsReference":false} */
            id_ef17163624f54b978edf4ab11ead0913.WireTo(EIDUpdateOrGate, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"OrEvent","DestinationIsReference":false} */
            id_ef17163624f54b978edf4ab11ead0913.WireTo(Scale5000UpdateOrGate, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"OrEvent","DestinationIsReference":false} */
            id_ef17163624f54b978edf4ab11ead0913.WireTo(Scale5000UpdateCheck, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"IfElse","DestinationIsReference":false} */
            id_ef17163624f54b978edf4ab11ead0913.WireTo(EIDUpdateCheck, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"IfElse","DestinationIsReference":false} */
            DatalinkUpdateArbitrator.WireTo(UpdateArbitrator, "arbitrator"); /* {"SourceType":"RequestArbitrator","SourceIsReference":false,"DestinationType":"Arbitrator","DestinationIsReference":false} */
            DatalinkUpdateArbitrator.WireTo(DatalinkVersionCompare, "ready"); /* {"SourceType":"RequestArbitrator","SourceIsReference":false,"DestinationType":"CompareFirmwareVersions","DestinationIsReference":false} */
            DatalinkVersionCompare.WireTo(DatalinkGetFirmwareSize, "isOldVersionEvent"); /* {"SourceType":"CompareFirmwareVersions","SourceIsReference":false,"DestinationType":"GetDownloadableFileSize","DestinationIsReference":false} */
            DatalinkVersionCompare.WireTo(id_060a112a46b144b28c264ea8d3a5d607, "versionMatchEvent"); /* {"SourceType":"CompareFirmwareVersions","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            DatalinkGetFirmwareSize.WireTo(DatalinkUpdateFileSizeString, "fileSize"); /* {"SourceType":"GetDownloadableFileSize","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            DatalinkGetFirmwareSize.WireTo(DatalinkUpdateAvailable, "taskComplete"); /* {"SourceType":"GetDownloadableFileSize","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            DatalinkUpdateAvailable.WireTo(id_36b06547f2cd41069906e176f9bc8cb4, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            DatalinkUpdateAvailable.WireTo(id_c39758d3899143da9c1396ddcce91df4, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            DatalinkUpdateAvailable.WireTo(id_25c17a5cfac54e5f906153121e11d4bc, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            DatalinkUpdateAvailable.WireTo(id_5c49b8393e05476f91f0998e6d41e2e6, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_36b06547f2cd41069906e176f9bc8cb4.WireTo(id_af2ddc149f4341eb8f52fa0cbd976683, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_c39758d3899143da9c1396ddcce91df4.WireTo(id_d129d3c94b974c699a9a52af6260a195, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            DatalinkVersionCompareString.WireTo(id_d129d3c94b974c699a9a52af6260a195, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_25c17a5cfac54e5f906153121e11d4bc.WireTo(id_7cb2264c069e4fed8116008adb783994, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            DatalinkUpdateFileSizeString.WireTo(id_7cb2264c069e4fed8116008adb783994, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_5c49b8393e05476f91f0998e6d41e2e6.WireTo(id_707cbdad6563439c884361ee3269d51d, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_5c49b8393e05476f91f0998e6d41e2e6.WireTo(id_2ab00c184c5f4bc89076586ffa3b970f, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_707cbdad6563439c884361ee3269d51d.WireTo(id_ac0b74b3f4d04ee2a0ea1ab59ab7468e, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_ac0b74b3f4d04ee2a0ea1ab59ab7468e.WireTo(DatalinkUpdateAvailable, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_ac0b74b3f4d04ee2a0ea1ab59ab7468e.WireTo(DatalinkUpdateDownloadProgress, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_ac0b74b3f4d04ee2a0ea1ab59ab7468e.WireTo(DatalinkUpdateDownloader, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"DownloadFile","DestinationIsReference":false} */
            DatalinkUpdateDownloadProgress.WireTo(id_15189e8e68de42968a06b4b3f539cdc1, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            DatalinkUpdateDownloadProgress.WireTo(id_09aab1c2a8ad4fff8fa6f07eb77499ec, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_15189e8e68de42968a06b4b3f539cdc1.WireTo(id_06111b2f3a07446ebf7090f6329117f7, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"ProgressBar","DestinationIsReference":false} */
            id_06111b2f3a07446ebf7090f6329117f7.WireTo(DatalinkUpdateProgressBar, "progressValue"); /* {"SourceType":"ProgressBar","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            id_09aab1c2a8ad4fff8fa6f07eb77499ec.WireTo(id_4c67692140cd40a58cf529c3bad90286, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            DatalinkUpdateProgressPercentage.WireTo(id_4c67692140cd40a58cf529c3bad90286, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            DatalinkUpdateDownloader.WireTo(DatalinkFirmwarePathFormatter, "outputPathPort"); /* {"SourceType":"DownloadFile","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            DatalinkUpdateDownloader.WireTo(id_6de2b3b274e444e1bb4b4003c34bdc72, "downloadFinished"); /* {"SourceType":"DownloadFile","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_2f24b74093424fd1bc5aea1069d41f66.WireTo(DatalinkUpdateProgressPercentage, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            id_2f24b74093424fd1bc5aea1069d41f66.WireTo(DatalinkUpdateProgressBar, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            DatalinkFirmwarePathFormatter.WireTo(id_6815142b26ae41f89cc79c24445323dc, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_6815142b26ae41f89cc79c24445323dc.WireTo(DatalinkUnzipUpdater, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"UnzipFile","DestinationIsReference":false} */
            id_6de2b3b274e444e1bb4b4003c34bdc72.WireTo(DatalinkUpdateDownloadProgress, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_6de2b3b274e444e1bb4b4003c34bdc72.WireTo(DatalinkFirmwareCheckHash, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"CheckFileMD5Hash","DestinationIsReference":false} */
            DatalinkFirmwareCheckHash.WireTo(DatalinkGetFirmwareHash, "compareHashPort"); /* {"SourceType":"CheckFileMD5Hash","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            DatalinkFirmwareCheckHash.WireTo(DatalinkFirmwarePathFormatter, "filePathPort"); /* {"SourceType":"CheckFileMD5Hash","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            DatalinkFirmwareCheckHash.WireTo(DatalinkInvalidFirmwarePopup, "hashNoMatch"); /* {"SourceType":"CheckFileMD5Hash","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            DatalinkRunUpdater.WireTo(mainWindow, "running"); /* {"SourceType":"RunExecutable","SourceIsReference":false,"DestinationType":"MainWindow","DestinationIsReference":true} */
            DatalinkRunUpdater.WireTo(DatalinkFailedToStartUpdaterPopup, "failedToStart"); /* {"SourceType":"RunExecutable","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            DatalinkFailedToStartUpdaterPopup.WireTo(id_aaf414a2cfad4c6caaaaeb7aefade72e, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            DatalinkFailedToStartUpdaterPopup.WireTo(id_89637835ab4644338c0162a190ba353b, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_aaf414a2cfad4c6caaaaeb7aefade72e.WireTo(id_3fb20eb1cb8640d5ba33131a0f584800, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_89637835ab4644338c0162a190ba353b.WireTo(id_4e45a0f491a8417fae2e36a1d501111e, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"RightJustify","DestinationIsReference":false} */
            id_4e45a0f491a8417fae2e36a1d501111e.WireTo(id_1cb06d8ce9c0436f8003dd6fc7455f70, "children"); /* {"SourceType":"RightJustify","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_1cb06d8ce9c0436f8003dd6fc7455f70.WireTo(id_492f805cbb794a078536ef45d0dc4d26, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_492f805cbb794a078536ef45d0dc4d26.WireTo(DatalinkUpdateArbitrator, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"RequestArbitrator","DestinationIsReference":false} */
            id_492f805cbb794a078536ef45d0dc4d26.WireTo(DatalinkFailedToStartUpdaterPopup, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            DatalinkInvalidFirmwarePopup.WireTo(id_5beebf12b0034ec69bf0592483474722, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            DatalinkInvalidFirmwarePopup.WireTo(id_937bd3431e1243d7ada60d13524ebb50, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_5beebf12b0034ec69bf0592483474722.WireTo(id_4d471c7dc54449309d14ef6e6aa7ea27, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_937bd3431e1243d7ada60d13524ebb50.WireTo(id_068c82c6fb6d4c3d9cf242269ee31e75, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"RightJustify","DestinationIsReference":false} */
            id_068c82c6fb6d4c3d9cf242269ee31e75.WireTo(id_6d2bfdd347564c3880086706e17cc37e, "children"); /* {"SourceType":"RightJustify","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_6d2bfdd347564c3880086706e17cc37e.WireTo(id_38723030ab614b11959de9240504320f, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_38723030ab614b11959de9240504320f.WireTo(DatalinkUpdateArbitrator, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"RequestArbitrator","DestinationIsReference":false} */
            id_38723030ab614b11959de9240504320f.WireTo(DatalinkInvalidFirmwarePopup, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_2ab00c184c5f4bc89076586ffa3b970f.WireTo(id_3be17c33504c4468ac27b36f4c8725f7, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_3be17c33504c4468ac27b36f4c8725f7.WireTo(DatalinkUpdateAvailable, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_3be17c33504c4468ac27b36f4c8725f7.WireTo(DatalinkUpdateArbitrator, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"RequestArbitrator","DestinationIsReference":false} */
            NoUpdatesAvailableGate.WireTo(id_060a112a46b144b28c264ea8d3a5d607, "logicalInputs"); /* {"SourceType":"AndEvent","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_060a112a46b144b28c264ea8d3a5d607.WireTo(DatalinkUpdateArbitrator, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"RequestArbitrator","DestinationIsReference":false} */
            NoUpdatesAvailableGate.WireTo(NoUpdatesAvailable, "logicalOutput"); /* {"SourceType":"AndEvent","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            NoUpdatesAvailable.WireTo(id_e9378bc3e36346ceb21f237198e63e6d, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            NoUpdatesAvailable.WireTo(id_5deac2c57ece4c92af0bc9317531ec05, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            NoUpdatesAvailable.WireTo(id_c2ee2bbf64704c44b1e43dfaedb2515b, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            NoUpdatesAvailable.WireTo(id_541370db1876442e8d9fa1fb0d69a21e, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            NoUpdatesAvailable.WireTo(id_90d9318c29174d91b523fbc335d47f73, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_e9378bc3e36346ceb21f237198e63e6d.WireTo(id_bf88445f893d4a59b373aca6f5a96ea7, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_5deac2c57ece4c92af0bc9317531ec05.WireTo(id_edb5bdfb3da44b508e932fa110579a84, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"GroupingBox","DestinationIsReference":false} */
            id_edb5bdfb3da44b508e932fa110579a84.WireTo(id_5565ca8ec66943e99a211f30ea5b9df7, "groupBoxContent"); /* {"SourceType":"GroupingBox","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_c2ee2bbf64704c44b1e43dfaedb2515b.WireTo(EIDNoUpdatesBox, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"GroupingBox","DestinationIsReference":false} */
            EIDNoUpdatesBox.WireTo(id_18b51fb517794f14aaae9bafcf1cd505, "groupBoxContent"); /* {"SourceType":"GroupingBox","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            EIDNoUpdatesCurrentVersion.WireTo(id_18b51fb517794f14aaae9bafcf1cd505, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_541370db1876442e8d9fa1fb0d69a21e.WireTo(Scale5000NoUpdatesBox, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"GroupingBox","DestinationIsReference":false} */
            Scale5000NoUpdatesBox.WireTo(id_3cdbdb36ab0a46ff8bc0e6f56f50fca8, "groupBoxContent"); /* {"SourceType":"GroupingBox","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            Scale5000NoUpdatesCurrentVersion.WireTo(id_3cdbdb36ab0a46ff8bc0e6f56f50fca8, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_90d9318c29174d91b523fbc335d47f73.WireTo(id_661b7884addc43d0a0e96e8f00a2f65b, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_661b7884addc43d0a0e96e8f00a2f65b.WireTo(id_01f8e70c73ff43a380f1481b09c84ece, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_01f8e70c73ff43a380f1481b09c84ece.WireTo(NoUpdatesAvailable, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            NoUpdatesAvailableGate.WireTo(EIDUpdateOrGate, "logicalInputs"); /* {"SourceType":"AndEvent","SourceIsReference":false,"DestinationType":"OrEvent","DestinationIsReference":false} */
            NoUpdatesAvailableGate.WireTo(Scale5000UpdateOrGate, "logicalInputs"); /* {"SourceType":"AndEvent","SourceIsReference":false,"DestinationType":"OrEvent","DestinationIsReference":false} */
            Scale5000UpdateCheck.WireTo(id_de2e963b5e9b4d989df90e302c1f19bf, "ifOutput"); /* {"SourceType":"IfElse","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            Scale5000UpdateCheck.WireTo(id_73ad9f3f0c7e4d588bcea2f02eda960e, "elseOutput"); /* {"SourceType":"IfElse","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            Scale5000UpdateArbitrator.WireTo(id_de2e963b5e9b4d989df90e302c1f19bf, "request"); /* {"SourceType":"RequestArbitrator","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            Scale5000UpdateArbitrator.WireTo(UpdateArbitrator, "arbitrator"); /* {"SourceType":"RequestArbitrator","SourceIsReference":false,"DestinationType":"Arbitrator","DestinationIsReference":false} */
            Scale5000UpdateArbitrator.WireTo(Scale5000CompareVersions, "ready"); /* {"SourceType":"RequestArbitrator","SourceIsReference":false,"DestinationType":"CompareFirmwareVersions","DestinationIsReference":false} */
            Scale5000CompareVersions.WireTo(Scale5000GetFirmwareSize, "isOldVersionEvent"); /* {"SourceType":"CompareFirmwareVersions","SourceIsReference":false,"DestinationType":"GetDownloadableFileSize","DestinationIsReference":false} */
            Scale5000CompareVersions.WireTo(id_25430dabff34418fbd71756647733b18, "versionMatchEvent"); /* {"SourceType":"CompareFirmwareVersions","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            Scale5000GetFirmwareSize.WireTo(Scale5000FileSizeStringFormat, "fileSize"); /* {"SourceType":"GetDownloadableFileSize","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            Scale5000GetFirmwareSize.WireTo(Scale5000Update, "taskComplete"); /* {"SourceType":"GetDownloadableFileSize","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            Scale5000Update.WireTo(id_bf91b3e369574a1faab09dcdbd201e6d, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            Scale5000Update.WireTo(id_59acd577b468484d9b9c42361331dccd, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            Scale5000Update.WireTo(id_568a75ed7c0a4fbba9d8f7a240f49636, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            Scale5000Update.WireTo(id_d534a7935e6f4d24a9ac6fba1719c80b, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_4461d0d6ca6b4a3faec5729558fbfe5a.WireTo(Scale5000Update, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_bf91b3e369574a1faab09dcdbd201e6d.WireTo(id_0b85e8bd4cb549e3835e1ecff7b24e05, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_948178edc71647d48450b57fe572500f.WireTo(id_0b85e8bd4cb549e3835e1ecff7b24e05, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            usbDeviceNameConnector.WireTo(id_948178edc71647d48450b57fe572500f, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":true,"DestinationType":"Apply","DestinationIsReference":false} */
            id_59acd577b468484d9b9c42361331dccd.WireTo(id_08bc031738da429b9a7b49d83a1d8c59, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_41bfaf91ff954e2385db473b1a534ec2.WireTo(id_08bc031738da429b9a7b49d83a1d8c59, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_577c3d09dac64fec886d19d9003fa585.WireTo(id_41bfaf91ff954e2385db473b1a534ec2, "portOut"); /* {"SourceType":"TupleAbstraction","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            id_577c3d09dac64fec886d19d9003fa585.WireTo(Scale5000CurrentVersionAdapter, "portA"); /* {"SourceType":"TupleAbstraction","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_577c3d09dac64fec886d19d9003fa585.WireTo(Scale5000NewVersionAdapter, "portB"); /* {"SourceType":"TupleAbstraction","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_568a75ed7c0a4fbba9d8f7a240f49636.WireTo(id_c836df4337004834953b8b5de3600ce6, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            Scale5000FileSizeStringFormat.WireTo(id_c836df4337004834953b8b5de3600ce6, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_d534a7935e6f4d24a9ac6fba1719c80b.WireTo(id_b48db50b45b440c2bc98481eb2f27f02, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_d534a7935e6f4d24a9ac6fba1719c80b.WireTo(id_2a27debfb4144c7580ac91f977a12f07, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_b48db50b45b440c2bc98481eb2f27f02.WireTo(Scale5000UpdateEvent, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            Scale5000UpdateEvent.WireTo(Scale5000UpdateDownloadProgress, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            Scale5000UpdateEvent.WireTo(Scale5000UpdateDownloader, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"DownloadFile","DestinationIsReference":false} */
            Scale5000UpdateEvent.WireTo(Scale5000Update, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            Scale5000UpdateDownloadProgress.WireTo(id_b9643f4d28534a95befe24a680d6023b, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            Scale5000UpdateDownloadProgress.WireTo(id_a4da70dce6dc442082e738e6c0821bec, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_8d274070abc149e1926c65a755948ae0.WireTo(Scale5000UpdateDownloadProgress, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_b9643f4d28534a95befe24a680d6023b.WireTo(id_bf142284640d4b17b8c170a5b2f4ec20, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"ProgressBar","DestinationIsReference":false} */
            id_bf142284640d4b17b8c170a5b2f4ec20.WireTo(Scale5000UpdateProgressBar, "progressValue"); /* {"SourceType":"ProgressBar","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            id_a4da70dce6dc442082e738e6c0821bec.WireTo(id_18c44ec4315041b59ee9a69bc9194871, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            Scale5000UpdateProgressPercentage.WireTo(id_18c44ec4315041b59ee9a69bc9194871, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            usbDeviceNameConnector.WireTo(id_8d274070abc149e1926c65a755948ae0, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":true,"DestinationType":"Apply","DestinationIsReference":false} */
            Scale5000UpdateDownloader.WireTo(Scale5000FirmwarePathFormatter, "outputPathPort"); /* {"SourceType":"DownloadFile","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            Scale5000UpdateDownloader.WireTo(id_348aafca8fbb47b1881c353249fa99aa, "downloadFinished"); /* {"SourceType":"DownloadFile","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_4c3dceb87c314a8a8b678e5fc2d0c682.WireTo(Scale5000UpdateProgressPercentage, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            id_4c3dceb87c314a8a8b678e5fc2d0c682.WireTo(Scale5000UpdateProgressBar, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            Scale5000FirmwarePathFormatter.WireTo(id_7bda0d719e224e67a6360188cbb5d4e0, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_348aafca8fbb47b1881c353249fa99aa.WireTo(Scale5000UpdateDownloadProgress, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_348aafca8fbb47b1881c353249fa99aa.WireTo(Scale5000FirmwareCheckHash, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"CheckFileMD5Hash","DestinationIsReference":false} */
            Scale5000FirmwareCheckHash.WireTo(Scale5000GetFirmwareHash, "compareHashPort"); /* {"SourceType":"CheckFileMD5Hash","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            Scale5000FirmwareCheckHash.WireTo(Scale5000FirmwarePathFormatter, "filePathPort"); /* {"SourceType":"CheckFileMD5Hash","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            Scale5000FirmwareCheckHash.WireTo(Scale5000CopyFirmware, "hashMatch"); /* {"SourceType":"CheckFileMD5Hash","SourceIsReference":false,"DestinationType":"CopyFile","DestinationIsReference":false} */
            Scale5000FirmwareCheckHash.WireTo(Scale5000InvalidFirmwarePopup, "hashNoMatch"); /* {"SourceType":"CheckFileMD5Hash","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            Scale5000CopyFirmware.WireTo(Scale5000FirmwarePathFormatter, "sourcePathPort"); /* {"SourceType":"CopyFile","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            Scale5000CopyFirmware.WireTo(Scale5000FirmwareDestFormatter, "destPathPort"); /* {"SourceType":"CopyFile","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            Scale5000CopyFirmware.WireTo(Scale5000CopyFirmwareSuccess, "copySuccess"); /* {"SourceType":"CopyFile","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            Scale5000CopyFirmware.WireTo(Scale5000CopyFirmwareFailed, "copyFailed"); /* {"SourceType":"CopyFile","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            Scale5000FirmwareTuple.WireTo(Scale5000FirmwareDestFormatter, "portOut"); /* {"SourceType":"TupleAbstraction","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            Scale5000CopyFirmwareSuccess.WireTo(id_b25392aa946545988955fe34d7d96cb0, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            Scale5000CopyFirmwareSuccess.WireTo(id_7a9e491c06b04eec83f8414563e9e7c2, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_99e480e2f8b144178e320c4e11eb56f5.WireTo(Scale5000CopyFirmwareSuccess, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_b25392aa946545988955fe34d7d96cb0.WireTo(id_0286fe46de7b4e8c93f6a55476a14b9d, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_5b58bbd15c904233ae78d2182ae5a737.WireTo(id_0286fe46de7b4e8c93f6a55476a14b9d, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            usbDeviceNameConnector.WireTo(id_5b58bbd15c904233ae78d2182ae5a737, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":true,"DestinationType":"Apply","DestinationIsReference":false} */
            id_7a9e491c06b04eec83f8414563e9e7c2.WireTo(id_9160374cd164400abe71497fe223819e, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"RightJustify","DestinationIsReference":false} */
            id_9160374cd164400abe71497fe223819e.WireTo(id_71346fe5ba6f411dab1343b15ec71cd8, "children"); /* {"SourceType":"RightJustify","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_71346fe5ba6f411dab1343b15ec71cd8.WireTo(id_8227ecbc757948e28fe240384ed383d9, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_8227ecbc757948e28fe240384ed383d9.WireTo(Scale5000UpdateArbitrator, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"RequestArbitrator","DestinationIsReference":false} */
            id_8227ecbc757948e28fe240384ed383d9.WireTo(Scale5000CopyFirmwareSuccess, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            usbDeviceNameConnector.WireTo(id_99e480e2f8b144178e320c4e11eb56f5, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":true,"DestinationType":"Apply","DestinationIsReference":false} */
            Scale5000CopyFirmwareFailed.WireTo(id_ee858585712c48efa17845004713c1fc, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            Scale5000CopyFirmwareFailed.WireTo(id_b489c9479d5d4b7dbf0dfc4adfd2ebd8, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            Scale5000CopyFirmwareFailed.WireTo(id_7ad2183782e24ff6b7da84a8609c5904, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_7dcf611eba444dd18cc33581b981a984.WireTo(Scale5000CopyFirmwareFailed, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_ee858585712c48efa17845004713c1fc.WireTo(id_edae258e300f4b9b837ebdc5384e803b, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_4bc11a7d1f2a47d0a80052e5fb159913.WireTo(id_edae258e300f4b9b837ebdc5384e803b, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            usbDeviceNameConnector.WireTo(id_4bc11a7d1f2a47d0a80052e5fb159913, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":true,"DestinationType":"Apply","DestinationIsReference":false} */
            id_b489c9479d5d4b7dbf0dfc4adfd2ebd8.WireTo(id_982e30e597734113a1791bde6a4783c5, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            Scale5000CopyFirmware.WireTo(id_982e30e597734113a1791bde6a4783c5, "failReason"); /* {"SourceType":"CopyFile","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_7ad2183782e24ff6b7da84a8609c5904.WireTo(id_f921d34001b443b2a7c598c76291f96d, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"RightJustify","DestinationIsReference":false} */
            id_f921d34001b443b2a7c598c76291f96d.WireTo(id_b8b0b50cfc3d4def8c31cff0a7c16ca7, "children"); /* {"SourceType":"RightJustify","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_b8b0b50cfc3d4def8c31cff0a7c16ca7.WireTo(id_d794423a7ddc4745a2201ba685a3e208, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_d794423a7ddc4745a2201ba685a3e208.WireTo(Scale5000UpdateArbitrator, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"RequestArbitrator","DestinationIsReference":false} */
            id_d794423a7ddc4745a2201ba685a3e208.WireTo(Scale5000CopyFirmwareFailed, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            usbDeviceNameConnector.WireTo(id_7dcf611eba444dd18cc33581b981a984, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":true,"DestinationType":"Apply","DestinationIsReference":false} */
            Scale5000InvalidFirmwarePopup.WireTo(id_eabd8257465542639396603ec92c3389, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            Scale5000InvalidFirmwarePopup.WireTo(id_12617eca30694e9fb934cde197439b8d, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_37710d843c3942d79be066e62d568572.WireTo(Scale5000InvalidFirmwarePopup, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_eabd8257465542639396603ec92c3389.WireTo(id_51beab2afeb045ab811e5965253eb9f5, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_12617eca30694e9fb934cde197439b8d.WireTo(id_57f95ab8d62c43daaba2d5c1dd6dc70c, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"RightJustify","DestinationIsReference":false} */
            id_57f95ab8d62c43daaba2d5c1dd6dc70c.WireTo(id_abfb9b82eac14935aaf3c363f437cd33, "children"); /* {"SourceType":"RightJustify","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_abfb9b82eac14935aaf3c363f437cd33.WireTo(id_f8b8342e8fee41c3b250e674d96550b6, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_f8b8342e8fee41c3b250e674d96550b6.WireTo(Scale5000UpdateArbitrator, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"RequestArbitrator","DestinationIsReference":false} */
            id_f8b8342e8fee41c3b250e674d96550b6.WireTo(Scale5000InvalidFirmwarePopup, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            usbDeviceNameConnector.WireTo(id_37710d843c3942d79be066e62d568572, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":true,"DestinationType":"Apply","DestinationIsReference":false} */
            id_2a27debfb4144c7580ac91f977a12f07.WireTo(id_9a7ee90714684ea48f5c1d782d9b9ae0, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_9a7ee90714684ea48f5c1d782d9b9ae0.WireTo(Scale5000UpdateArbitrator, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"RequestArbitrator","DestinationIsReference":false} */
            id_9a7ee90714684ea48f5c1d782d9b9ae0.WireTo(Scale5000Update, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            usbDeviceNameConnector.WireTo(id_4461d0d6ca6b4a3faec5729558fbfe5a, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":true,"DestinationType":"Apply","DestinationIsReference":false} */
            Scale5000UpdateOrGate.WireTo(id_25430dabff34418fbd71756647733b18, "logicalInputs"); /* {"SourceType":"OrEvent","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_25430dabff34418fbd71756647733b18.WireTo(Scale5000UpdateArbitrator, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"RequestArbitrator","DestinationIsReference":false} */
            Scale5000UpdateOrGate.WireTo(id_73ad9f3f0c7e4d588bcea2f02eda960e, "logicalInputs"); /* {"SourceType":"OrEvent","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            EIDUpdateCheck.WireTo(EIDGetConnectionType, "ifOutput"); /* {"SourceType":"IfElse","SourceIsReference":false,"DestinationType":"EidCheckConnectionTypeSCP","DestinationIsReference":false} */
            EIDUpdateCheck.WireTo(id_d5fa0ff4fc5c42f8b60ccc7cc0d2e2ef, "elseOutput"); /* {"SourceType":"IfElse","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            EIDGetConnectionType.WireTo(scpArbitrator, "arbitrator"); /* {"SourceType":"EidCheckConnectionTypeSCP","SourceIsReference":false,"DestinationType":"Arbitrator","DestinationIsReference":true} */
            EIDGetConnectionType.WireTo(scpProtocol, "requestResponseDataFlow"); /* {"SourceType":"EidCheckConnectionTypeSCP","SourceIsReference":false,"DestinationType":"SCPProtocol","DestinationIsReference":true} */
            EIDGetConnectionType.WireTo(id_f20d313fd06e403da33ef1776bb30744, "connectionType"); /* {"SourceType":"EidCheckConnectionTypeSCP","SourceIsReference":false,"DestinationType":"Equals","DestinationIsReference":false} */
            id_f20d313fd06e403da33ef1776bb30744.WireTo(id_0e9d65d79ef34ae9a15711db0b3878ce, "equalEvent"); /* {"SourceType":"Equals","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            EIDUpdateArbitrator.WireTo(id_0e9d65d79ef34ae9a15711db0b3878ce, "request"); /* {"SourceType":"RequestArbitrator","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            EIDUpdateArbitrator.WireTo(UpdateArbitrator, "arbitrator"); /* {"SourceType":"RequestArbitrator","SourceIsReference":false,"DestinationType":"Arbitrator","DestinationIsReference":false} */
            EIDUpdateArbitrator.WireTo(EIDGetFirmware, "ready"); /* {"SourceType":"RequestArbitrator","SourceIsReference":false,"DestinationType":"EidGetFirmwareVersion","DestinationIsReference":false} */
            EIDGetFirmware.WireTo(scpArbitrator, "arbitrator"); /* {"SourceType":"EidGetFirmwareVersion","SourceIsReference":false,"DestinationType":"Arbitrator","DestinationIsReference":true} */
            EIDGetFirmware.WireTo(scpProtocol, "requestResponseDataFlow"); /* {"SourceType":"EidGetFirmwareVersion","SourceIsReference":false,"DestinationType":"SCPProtocol","DestinationIsReference":true} */
            EIDGetFirmware.WireTo(id_b06cf2822c7f4f4b835a086bd0f1e395, "versionOutput"); /* {"SourceType":"EidGetFirmwareVersion","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            EIDGetFirmware.WireTo(EIDVersionCompare, "gotVersion"); /* {"SourceType":"EidGetFirmwareVersion","SourceIsReference":false,"DestinationType":"CompareFirmwareVersions","DestinationIsReference":false} */
            id_b06cf2822c7f4f4b835a086bd0f1e395.WireTo(EIDNoUpdatesCurrentVersion, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            id_b06cf2822c7f4f4b835a086bd0f1e395.WireTo(EIDCurrentVersionAdapter, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            EIDVersionCompare.WireTo(id_b06cf2822c7f4f4b835a086bd0f1e395, "currentVersion"); /* {"SourceType":"CompareFirmwareVersions","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            EIDVersionCompare.WireTo(EIDGetFirmwareSize, "isOldVersionEvent"); /* {"SourceType":"CompareFirmwareVersions","SourceIsReference":false,"DestinationType":"GetDownloadableFileSize","DestinationIsReference":false} */
            EIDVersionCompare.WireTo(id_410e7e9335c14aa197f26f9c74736be4, "versionMatchEvent"); /* {"SourceType":"CompareFirmwareVersions","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            EIDURLConnector.WireTo(EIDGetFirmwareSize, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"GetDownloadableFileSize","DestinationIsReference":false} */
            EIDGetFirmwareSize.WireTo(EIDFileSizeStringFormat, "fileSize"); /* {"SourceType":"GetDownloadableFileSize","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            EIDGetFirmwareSize.WireTo(EIDUpdate, "taskComplete"); /* {"SourceType":"GetDownloadableFileSize","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            EIDRetrieveURL.WireTo(EIDURLConnector, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            EIDUpdate.WireTo(id_f69ed031238a42458ff7b2c304ba23f2, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            EIDUpdate.WireTo(id_65846a09cb6244559a126d66559b0bd5, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            EIDUpdate.WireTo(id_4347c2e07bc64372a83cd0c321664df0, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            EIDUpdate.WireTo(id_30873862c31c4840b0e4ae2dd48bc7d6, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_f69ed031238a42458ff7b2c304ba23f2.WireTo(id_970d31d5cb904cbd8f87cce03a765832, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_65846a09cb6244559a126d66559b0bd5.WireTo(id_670bc7265cfe4101a8070f384a2147e8, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_02e3d49f3852467292d89eb98dc84f62.WireTo(id_670bc7265cfe4101a8070f384a2147e8, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_c5b916ac91e8438db526d41a516fc91a.WireTo(id_02e3d49f3852467292d89eb98dc84f62, "portOut"); /* {"SourceType":"TupleAbstraction","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            id_c5b916ac91e8438db526d41a516fc91a.WireTo(EIDCurrentVersionAdapter, "portA"); /* {"SourceType":"TupleAbstraction","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_c5b916ac91e8438db526d41a516fc91a.WireTo(EIDNewVersionAdapter, "portB"); /* {"SourceType":"TupleAbstraction","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_4347c2e07bc64372a83cd0c321664df0.WireTo(id_ac60bdd9d2c942e4a2a3e78514f0e225, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            EIDFileSizeStringFormat.WireTo(id_ac60bdd9d2c942e4a2a3e78514f0e225, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_30873862c31c4840b0e4ae2dd48bc7d6.WireTo(id_61f23642c2db4b3eae7e16cd8a478553, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_30873862c31c4840b0e4ae2dd48bc7d6.WireTo(id_c1ccaf01062d4a478275495a4b878924, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_61f23642c2db4b3eae7e16cd8a478553.WireTo(EIDUpdateEvent, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            EIDUpdateEvent.WireTo(EIDUpdateDownloadProgress, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            EIDUpdateEvent.WireTo(EIDUpdateDownloader, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"DownloadFile","DestinationIsReference":false} */
            EIDUpdateEvent.WireTo(EIDUpdate, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            EIDUpdateDownloadProgress.WireTo(id_8214740b79ac49c98df6d3c856ae4e01, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            EIDUpdateDownloadProgress.WireTo(id_f6b84866d4f44d7c8061b8e0f9f4ec9a, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_8214740b79ac49c98df6d3c856ae4e01.WireTo(id_398046e40f6545f892e7a394660ea06f, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"ProgressBar","DestinationIsReference":false} */
            id_398046e40f6545f892e7a394660ea06f.WireTo(EIDUpdateProgressBar, "progressValue"); /* {"SourceType":"ProgressBar","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            id_f6b84866d4f44d7c8061b8e0f9f4ec9a.WireTo(id_9564a3d23b4c4f05b3ac75c3ad5f77bd, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            EIDUpdateProgressPercentage.WireTo(id_9564a3d23b4c4f05b3ac75c3ad5f77bd, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            EIDURLConnector.WireTo(EIDUpdateDownloader, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"DownloadFile","DestinationIsReference":false} */
            EIDUpdateDownloader.WireTo(EIDFirmwarePathFormatter, "outputPathPort"); /* {"SourceType":"DownloadFile","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            EIDUpdateDownloader.WireTo(id_cee9f49f4d84482c8b87d55656caf671, "downloadFinished"); /* {"SourceType":"DownloadFile","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_9048ef7559b4412abed65f84cbac1c0f.WireTo(EIDUpdateProgressPercentage, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            id_9048ef7559b4412abed65f84cbac1c0f.WireTo(EIDUpdateProgressBar, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            EIDFirmwarePathFormatter.WireTo(id_17497b366d624db98c3ebf44dc382946, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_17497b366d624db98c3ebf44dc382946.WireTo(EIDHexValidate, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"IntelHexCheckValidity","DestinationIsReference":false} */
            id_17497b366d624db98c3ebf44dc382946.WireTo(EIDValidateFirmware, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"EidValidateFirmware","DestinationIsReference":false} */
            EIDFirmwareUpdater.WireTo(id_17497b366d624db98c3ebf44dc382946, "firmwarePathPort"); /* {"SourceType":"EidFirmwareUpdater","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_cee9f49f4d84482c8b87d55656caf671.WireTo(EIDUpdateDownloadProgress, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_cee9f49f4d84482c8b87d55656caf671.WireTo(EIDFirmwareCheckHash, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"CheckFileMD5Hash","DestinationIsReference":false} */
            EIDFirmwareCheckHash.WireTo(EIDGetFirmwareHash, "compareHashPort"); /* {"SourceType":"CheckFileMD5Hash","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            EIDFirmwareCheckHash.WireTo(EIDFirmwarePathFormatter, "filePathPort"); /* {"SourceType":"CheckFileMD5Hash","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            EIDFirmwareCheckHash.WireTo(EIDHexValidate, "hashMatch"); /* {"SourceType":"CheckFileMD5Hash","SourceIsReference":false,"DestinationType":"IntelHexCheckValidity","DestinationIsReference":false} */
            EIDFirmwareCheckHash.WireTo(EIDInvalidFirmwarePopup, "hashNoMatch"); /* {"SourceType":"CheckFileMD5Hash","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            EIDHexValidate.WireTo(EIDValidateFirmware, "isValid"); /* {"SourceType":"IntelHexCheckValidity","SourceIsReference":false,"DestinationType":"EidValidateFirmware","DestinationIsReference":false} */
            EIDHexValidate.WireTo(EIDInvalidFirmwarePopup, "notValid"); /* {"SourceType":"IntelHexCheckValidity","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            EIDValidateFirmware.WireTo(id_f5960dcbfb3b41c7bf10bc9e7e3e5ed5, "isValid"); /* {"SourceType":"EidValidateFirmware","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            EIDValidateFirmware.WireTo(EIDInvalidFirmwarePopup, "notValid"); /* {"SourceType":"EidValidateFirmware","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_f5960dcbfb3b41c7bf10bc9e7e3e5ed5.WireTo(EIDFirmwareUpdater, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EidFirmwareUpdater","DestinationIsReference":false} */
            id_f5960dcbfb3b41c7bf10bc9e7e3e5ed5.WireTo(EIDFirmwareFlashProgress, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            EIDFirmwareUpdater.WireTo(scpArbitrator, "arbitrator"); /* {"SourceType":"EidFirmwareUpdater","SourceIsReference":false,"DestinationType":"Arbitrator","DestinationIsReference":true} */
            EIDFirmwareUpdater.WireTo(scpProtocol, "requestResponseDataFlow"); /* {"SourceType":"EidFirmwareUpdater","SourceIsReference":false,"DestinationType":"SCPProtocol","DestinationIsReference":true} */
            EIDFirmwareUpdater.WireTo(id_da8b0e31b3bf4ba3bd08d8242020fe3b, "statusString"); /* {"SourceType":"EidFirmwareUpdater","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            EIDFirmwareUpdater.WireTo(id_6e6dbb37f1e5421aaf39df6b0e58b139, "updateSuccess"); /* {"SourceType":"EidFirmwareUpdater","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            EIDFirmwareUpdater.WireTo(id_710916bb939648649fd8666b19018ee4, "updateFailed"); /* {"SourceType":"EidFirmwareUpdater","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_da8b0e31b3bf4ba3bd08d8242020fe3b.WireTo(EIDUpdateStatusString, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_da8b0e31b3bf4ba3bd08d8242020fe3b.WireTo(EIDUpdateFailedStatusString, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_6e6dbb37f1e5421aaf39df6b0e58b139.WireTo(EIDFirmwareFlashProgress, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_6e6dbb37f1e5421aaf39df6b0e58b139.WireTo(SRS2FirmwareFlashSuccess, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            SRS2FirmwareFlashSuccess.WireTo(id_808c9eae18334a629f698d2becdbb33a, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            SRS2FirmwareFlashSuccess.WireTo(id_b32786f02be940858fd42179559bc3bf, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_808c9eae18334a629f698d2becdbb33a.WireTo(id_cf0ea7d7df94461198efeeb176676f43, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_b32786f02be940858fd42179559bc3bf.WireTo(id_601f0de91ed94f5abca456b96671fd4c, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_601f0de91ed94f5abca456b96671fd4c.WireTo(id_8526b505a12141288572333a46989519, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_8526b505a12141288572333a46989519.WireTo(EIDUpdateArbitrator, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"RequestArbitrator","DestinationIsReference":false} */
            id_8526b505a12141288572333a46989519.WireTo(SRS2FirmwareFlashSuccess, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_710916bb939648649fd8666b19018ee4.WireTo(EIDFirmwareFlashProgress, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_710916bb939648649fd8666b19018ee4.WireTo(EIDFirmwareFlashFailed, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            EIDFirmwareFlashFailed.WireTo(id_90a26be3185742d9bb758c0ed5e0b357, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            EIDFirmwareFlashFailed.WireTo(id_fd7f3fd803bf4f08b331edf1dd4eebc0, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            EIDFirmwareFlashFailed.WireTo(id_42a7bbfd9fe24bc399c44a8d3707a9b7, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_90a26be3185742d9bb758c0ed5e0b357.WireTo(id_b0e1629606704fb19e35864cd6d343ca, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_fd7f3fd803bf4f08b331edf1dd4eebc0.WireTo(EIDUpdateFailedStatusString, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_42a7bbfd9fe24bc399c44a8d3707a9b7.WireTo(id_44526f0dbf6144d4a87bbcab1c653e65, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_44526f0dbf6144d4a87bbcab1c653e65.WireTo(id_0b5e823df94f47439d69417edf240291, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_0b5e823df94f47439d69417edf240291.WireTo(EIDUpdateArbitrator, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"RequestArbitrator","DestinationIsReference":false} */
            id_0b5e823df94f47439d69417edf240291.WireTo(EIDFirmwareFlashFailed, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            EIDFirmwareFlashProgress.WireTo(id_6d467b84693a4fdb9346982803b9dbbf, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            EIDFirmwareFlashProgress.WireTo(id_24534e6649aa4ead9e7d6abdccbc466b, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            EIDFirmwareFlashProgress.WireTo(id_662c4d4b6f1347a09d6bc0913e3d14eb, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_6d467b84693a4fdb9346982803b9dbbf.WireTo(id_045a7a953f514ccba9009d428160e0ff, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"ProgressBar","DestinationIsReference":false} */
            id_045a7a953f514ccba9009d428160e0ff.WireTo(EIDFirmwareFlashPercentage, "progressValue"); /* {"SourceType":"ProgressBar","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            EIDFirmwareUpdater.WireTo(EIDFirmwareFlashPercentage, "progressPercentage"); /* {"SourceType":"EidFirmwareUpdater","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            id_24534e6649aa4ead9e7d6abdccbc466b.WireTo(EIDUpdateStatusString, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_662c4d4b6f1347a09d6bc0913e3d14eb.WireTo(id_02f027db3a334a1183fb9a0dd846c3d3, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            EIDInvalidFirmwarePopup.WireTo(id_f3f4d20fd4cd4f11919cfeda07484b6f, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            EIDInvalidFirmwarePopup.WireTo(id_6b26d9633f9e442d87b56d3f1cc281be, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_f3f4d20fd4cd4f11919cfeda07484b6f.WireTo(id_12296006bbde48388c5ea71742c7aba3, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_6b26d9633f9e442d87b56d3f1cc281be.WireTo(id_77bbfea542e343d79500adf6bf08e0b3, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"RightJustify","DestinationIsReference":false} */
            id_77bbfea542e343d79500adf6bf08e0b3.WireTo(id_f612db2371e044769a2450f43ca8cf52, "children"); /* {"SourceType":"RightJustify","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_f612db2371e044769a2450f43ca8cf52.WireTo(id_d1ebefdb03bb4176a194422521115fb9, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_d1ebefdb03bb4176a194422521115fb9.WireTo(EIDUpdateArbitrator, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"RequestArbitrator","DestinationIsReference":false} */
            id_d1ebefdb03bb4176a194422521115fb9.WireTo(EIDInvalidFirmwarePopup, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_c1ccaf01062d4a478275495a4b878924.WireTo(id_c7dff5ad4f394555ad8903ad537b145d, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_c7dff5ad4f394555ad8903ad537b145d.WireTo(EIDUpdateArbitrator, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"RequestArbitrator","DestinationIsReference":false} */
            id_c7dff5ad4f394555ad8903ad537b145d.WireTo(EIDUpdate, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            EIDUpdateOrGate.WireTo(id_410e7e9335c14aa197f26f9c74736be4, "logicalInputs"); /* {"SourceType":"OrEvent","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_410e7e9335c14aa197f26f9c74736be4.WireTo(EIDUpdateArbitrator, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"RequestArbitrator","DestinationIsReference":false} */
            EIDUpdateOrGate.WireTo(id_d5fa0ff4fc5c42f8b60ccc7cc0d2e2ef, "logicalInputs"); /* {"SourceType":"OrEvent","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            UpdateNoInternetConnection.WireTo(id_0367e590a2114498a93a7cbf950cdf92, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            UpdateNoInternetConnection.WireTo(id_a18861426efa4c1683bcf9fd60afe8d7, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_0367e590a2114498a93a7cbf950cdf92.WireTo(id_532d7df13ad94fa1b2dc302ff6fecb99, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_a18861426efa4c1683bcf9fd60afe8d7.WireTo(id_518143103a83410e8573a87feb546b74, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_518143103a83410e8573a87feb546b74.WireTo(UpdateNoInternetConnection, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            UpdateFromFileConnector.WireTo(Scale5000UpdateFromFileCheck, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"IfElse","DestinationIsReference":false} */
            UpdateFromFileConnector.WireTo(EIDUpdateFromFileCheck, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"IfElse","DestinationIsReference":false} */
            Scale5000UpdateFromFileCheck.WireTo(Scale5000SelectFirmware, "ifOutput"); /* {"SourceType":"IfElse","SourceIsReference":false,"DestinationType":"OpenFirmwareBrowser","DestinationIsReference":false} */
            Scale5000SelectFirmware.WireTo(Scale5000FileCopyFirmware, "fileSelected"); /* {"SourceType":"OpenFirmwareBrowser","SourceIsReference":false,"DestinationType":"CopyFile","DestinationIsReference":false} */
            Scale5000FileCopyFirmware.WireTo(Scale5000SelectFirmware, "sourcePathPort"); /* {"SourceType":"CopyFile","SourceIsReference":false,"DestinationType":"OpenFirmwareBrowser","DestinationIsReference":false} */
            Scale5000FileCopyFirmware.WireTo(Scale5000FileFirmwareDestFormatter, "destPathPort"); /* {"SourceType":"CopyFile","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            Scale5000FileCopyFirmware.WireTo(Scale5000FileCopyFirmwareSuccess, "copySuccess"); /* {"SourceType":"CopyFile","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            Scale5000FileCopyFirmware.WireTo(Scale5000FileCopyFirmwareFailed, "copyFailed"); /* {"SourceType":"CopyFile","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            Scale5000FileFirmwareTuple.WireTo(Scale5000FileFirmwareDestFormatter, "portOut"); /* {"SourceType":"TupleAbstraction","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            Scale5000FileFirmwareTuple.WireTo(Scale5000SelectFirmware, "portB"); /* {"SourceType":"TupleAbstraction","SourceIsReference":false,"DestinationType":"OpenFirmwareBrowser","DestinationIsReference":false} */
            Scale5000FileCopyFirmwareSuccess.WireTo(id_623eebd9457d4dbf894946f0ccc126d7, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            Scale5000FileCopyFirmwareSuccess.WireTo(id_eb391e0bf4b346d49e6b07b2aa01be31, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_623eebd9457d4dbf894946f0ccc126d7.WireTo(id_f5c8da14ee334ae492b7d633077e8c5a, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_eb391e0bf4b346d49e6b07b2aa01be31.WireTo(id_cf4509434ef943fd8b69f2a0323c2c71, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"RightJustify","DestinationIsReference":false} */
            id_cf4509434ef943fd8b69f2a0323c2c71.WireTo(id_cb0aa71ff7a4464bbacb87cba523aba8, "children"); /* {"SourceType":"RightJustify","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_cb0aa71ff7a4464bbacb87cba523aba8.WireTo(Scale5000FileCopyFirmwareSuccess, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            Scale5000FileCopyFirmwareFailed.WireTo(id_ebfef36ca2384811aafa25de122e69fa, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            Scale5000FileCopyFirmwareFailed.WireTo(id_619153a27edf4debb8b7b01741434859, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            Scale5000FileCopyFirmwareFailed.WireTo(id_bc198912e53e4c55a0589f0cd00d69a4, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_ebfef36ca2384811aafa25de122e69fa.WireTo(id_557831077d564bdaac282bde9f758d38, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_619153a27edf4debb8b7b01741434859.WireTo(id_031bf4a14d094dd4868d736e15828ec4, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            Scale5000FileCopyFirmware.WireTo(id_031bf4a14d094dd4868d736e15828ec4, "failReason"); /* {"SourceType":"CopyFile","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_bc198912e53e4c55a0589f0cd00d69a4.WireTo(id_12d88afdbcce4f64a252f127220f46b5, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"RightJustify","DestinationIsReference":false} */
            id_12d88afdbcce4f64a252f127220f46b5.WireTo(id_63a0b6b073814ee986854fa5590a67bc, "children"); /* {"SourceType":"RightJustify","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_63a0b6b073814ee986854fa5590a67bc.WireTo(Scale5000FileCopyFirmwareFailed, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            EIDUpdateFromFileCheck.WireTo(EIDSelectFirmware, "ifOutput"); /* {"SourceType":"IfElse","SourceIsReference":false,"DestinationType":"OpenFirmwareBrowser","DestinationIsReference":false} */
            EIDSelectFirmware.WireTo(EIDFileHexValidate, "fileSelected"); /* {"SourceType":"OpenFirmwareBrowser","SourceIsReference":false,"DestinationType":"IntelHexCheckValidity","DestinationIsReference":false} */
            EIDSelectFirmware.WireTo(EIDFileFirmwarePathConnector, "selectedFile"); /* {"SourceType":"OpenFirmwareBrowser","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            EIDFileFirmwareUpdater.WireTo(EIDSelectFirmware, "firmwarePathPort"); /* {"SourceType":"EidFirmwareUpdater","SourceIsReference":false,"DestinationType":"OpenFirmwareBrowser","DestinationIsReference":false} */
            EIDFileHexValidate.WireTo(EIDFileValidateFirmware, "isValid"); /* {"SourceType":"IntelHexCheckValidity","SourceIsReference":false,"DestinationType":"EidValidateFirmware","DestinationIsReference":false} */
            EIDFileHexValidate.WireTo(EIDInvalidFirmwarePopup, "notValid"); /* {"SourceType":"IntelHexCheckValidity","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            EIDFileValidateFirmware.WireTo(id_bacb8c051fd145e8a2f91257c228a448, "isValid"); /* {"SourceType":"EidValidateFirmware","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            EIDFileValidateFirmware.WireTo(EIDInvalidFirmwarePopup, "notValid"); /* {"SourceType":"EidValidateFirmware","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_bacb8c051fd145e8a2f91257c228a448.WireTo(EIDFileFirmwareUpdater, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EidFirmwareUpdater","DestinationIsReference":false} */
            id_bacb8c051fd145e8a2f91257c228a448.WireTo(EIDFileFirmwareFlashProgress, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            EIDFileFirmwareUpdater.WireTo(scpArbitrator, "arbitrator"); /* {"SourceType":"EidFirmwareUpdater","SourceIsReference":false,"DestinationType":"Arbitrator","DestinationIsReference":true} */
            EIDFileFirmwareUpdater.WireTo(scpProtocol, "requestResponseDataFlow"); /* {"SourceType":"EidFirmwareUpdater","SourceIsReference":false,"DestinationType":"SCPProtocol","DestinationIsReference":true} */
            EIDFileFirmwareUpdater.WireTo(id_728a5ac1f8b641cbbe31217db60c2a58, "statusString"); /* {"SourceType":"EidFirmwareUpdater","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            EIDFileFirmwareUpdater.WireTo(id_3a404cc30c51441a9469701aaa2ecc1a, "updateSuccess"); /* {"SourceType":"EidFirmwareUpdater","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            EIDFileFirmwareUpdater.WireTo(id_b126d73a74a44b02a5709dbb36b0dca4, "updateFailed"); /* {"SourceType":"EidFirmwareUpdater","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_728a5ac1f8b641cbbe31217db60c2a58.WireTo(EIDFileUpdateStatusString, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_728a5ac1f8b641cbbe31217db60c2a58.WireTo(EIDFileUpdateFailedStatusString, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_3a404cc30c51441a9469701aaa2ecc1a.WireTo(EIDFileFirmwareFlashProgress, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_3a404cc30c51441a9469701aaa2ecc1a.WireTo(EIDFileFirmwareFlashSuccess, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            EIDFileFirmwareFlashSuccess.WireTo(id_9dfbf057cf2f427cb562c3894b4f24d6, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            EIDFileFirmwareFlashSuccess.WireTo(id_ce232056f5e347579ca341b60bd9d80d, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_9dfbf057cf2f427cb562c3894b4f24d6.WireTo(id_a2830b2b842a49a7bd39efb9903e4835, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_ce232056f5e347579ca341b60bd9d80d.WireTo(id_649c1274f7084e778a7df33251f88431, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_649c1274f7084e778a7df33251f88431.WireTo(EIDFileFirmwareFlashSuccess, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_b126d73a74a44b02a5709dbb36b0dca4.WireTo(EIDFileFirmwareFlashProgress, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_b126d73a74a44b02a5709dbb36b0dca4.WireTo(EIDFileFirmwareFlashFailed, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            EIDFileFirmwareFlashFailed.WireTo(id_dcd25d1e9de44e9d9e70a3875053d611, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            EIDFileFirmwareFlashFailed.WireTo(id_2df0085865544e46b878964faddb7842, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            EIDFileFirmwareFlashFailed.WireTo(id_68bac0c5ac754b1ba80fe21ba94aeac0, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_dcd25d1e9de44e9d9e70a3875053d611.WireTo(id_1639feb335854759bdc24bc212373f51, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_2df0085865544e46b878964faddb7842.WireTo(EIDFileUpdateFailedStatusString, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_68bac0c5ac754b1ba80fe21ba94aeac0.WireTo(id_2a88078b3e2b489ca4ba511f2b5041f4, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_2a88078b3e2b489ca4ba511f2b5041f4.WireTo(EIDFileFirmwareFlashFailed, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            EIDFileFirmwareFlashProgress.WireTo(id_d490253dfdbc4d738e4097faaaf67448, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            EIDFileFirmwareFlashProgress.WireTo(id_eac9b8e35a5c4fe79fa606b84c64b416, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            EIDFileFirmwareFlashProgress.WireTo(id_5b34c4c6aa534a7ab9a3013f562c6df9, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_d490253dfdbc4d738e4097faaaf67448.WireTo(id_ae15b3aadf7a460f93d014f3a57ccc51, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"ProgressBar","DestinationIsReference":false} */
            id_ae15b3aadf7a460f93d014f3a57ccc51.WireTo(EIDFileFirmwareFlashPercentage, "progressValue"); /* {"SourceType":"ProgressBar","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            EIDFileFirmwareUpdater.WireTo(EIDFileFirmwareFlashPercentage, "progressPercentage"); /* {"SourceType":"EidFirmwareUpdater","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            id_eac9b8e35a5c4fe79fa606b84c64b416.WireTo(EIDFileUpdateStatusString, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_5b34c4c6aa534a7ab9a3013f562c6df9.WireTo(id_61aa5f189b634472b5025356317f099f, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            EIDFileFirmwarePathConnector.WireTo(EIDFileHexValidate, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"IntelHexCheckValidity","DestinationIsReference":false} */
            EIDFileFirmwarePathConnector.WireTo(EIDFileValidateFirmware, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"EidValidateFirmware","DestinationIsReference":false} */
            EIDStartRecovery.WireTo(EIDRecoverPopup, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":true} */
            EIDStartRecovery.WireTo(id_841dcdfbf340481aad3dbda0d921427b, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"IfElse","DestinationIsReference":false} */
            id_ed5d83930c4d4fe7b27d0b9016939366.WireTo(id_841dcdfbf340481aad3dbda0d921427b, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"IfElse","DestinationIsReference":false} */
            id_841dcdfbf340481aad3dbda0d921427b.WireTo(id_6da9089012774361a11eb8656c3f1c27, "ifOutput"); /* {"SourceType":"IfElse","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_841dcdfbf340481aad3dbda0d921427b.WireTo(EIDRecoveryNoFilepath, "elseOutput"); /* {"SourceType":"IfElse","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_6da9089012774361a11eb8656c3f1c27.WireTo(EIDRecoverCheckDeviceType, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"AndEvent","DestinationIsReference":false} */
            id_6da9089012774361a11eb8656c3f1c27.WireTo(id_4e5556ad854c4592938af74c56a727d9, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"IfElse","DestinationIsReference":false} */
            EIDRecoverCheckDeviceType.WireTo(EIDRecoveryNoDeviceType, "logicalOuput"); /* {"SourceType":"AndEvent","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            EIDRecoveryNoDeviceType.WireTo(id_7d717e2d9dc94ff28eb2410fffe9fa41, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            EIDRecoveryNoDeviceType.WireTo(id_a4a3998e1d744fa28700de9a125618fb, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_7d717e2d9dc94ff28eb2410fffe9fa41.WireTo(id_c1d2e735c5204b90b768b3b8f45750fa, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_a4a3998e1d744fa28700de9a125618fb.WireTo(id_9022a3ce353b4e59b7dd9c338ccd2c3c, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_9022a3ce353b4e59b7dd9c338ccd2c3c.WireTo(EIDRecoveryNoDeviceType, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_6a0ed33d364e4b8ca83c1e823d5469b6.WireTo(id_4e5556ad854c4592938af74c56a727d9, "isEqual"); /* {"SourceType":"Equals","SourceIsReference":false,"DestinationType":"IfElse","DestinationIsReference":false} */
            id_4e5556ad854c4592938af74c56a727d9.WireTo(id_e49b7b7ddf2a4c718e6c60aee1471868, "ifOutput"); /* {"SourceType":"IfElse","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_4e5556ad854c4592938af74c56a727d9.WireTo(id_ba8385e1110c4dbcba3e40e2e87c3574, "elseOutput"); /* {"SourceType":"IfElse","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            RecoverDeviceType.WireTo(id_6a0ed33d364e4b8ca83c1e823d5469b6, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":true,"DestinationType":"Equals","DestinationIsReference":false} */
            id_e49b7b7ddf2a4c718e6c60aee1471868.WireTo(EIDRecoveryUpdater, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EidFirmwareUpdater","DestinationIsReference":false} */
            id_e49b7b7ddf2a4c718e6c60aee1471868.WireTo(EIDRecoveryFlashProgress, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            ComPortList.WireTo(EIDRecoveryUpdater, "selectionName"); /* {"SourceType":"ComPortOptionBox","SourceIsReference":true,"DestinationType":"EidFirmwareUpdater","DestinationIsReference":false} */
            EIDRecoveryUpdater.WireTo(id_16bae58b2a0d4ffd8aa13b5c879dec91, "statusString"); /* {"SourceType":"EidFirmwareUpdater","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            EIDRecoveryUpdater.WireTo(id_1d99f6f0c0814a9b937d443c50b046b8, "updateSuccess"); /* {"SourceType":"EidFirmwareUpdater","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            EIDRecoveryUpdater.WireTo(id_e40c2e783fff47f19fcde91f7bc96521, "updateFailed"); /* {"SourceType":"EidFirmwareUpdater","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_16bae58b2a0d4ffd8aa13b5c879dec91.WireTo(EIDRecoveryStatusString, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_16bae58b2a0d4ffd8aa13b5c879dec91.WireTo(EIDRecoveryFailedStatusString, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_1d99f6f0c0814a9b937d443c50b046b8.WireTo(EIDRecoveryFlashProgress, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_1d99f6f0c0814a9b937d443c50b046b8.WireTo(EIDRecoveryFlashSuccess, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            EIDRecoveryFlashSuccess.WireTo(id_45719901486a4793942a9bf418a9d4ae, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            EIDRecoveryFlashSuccess.WireTo(id_2d66cfd15d0e4e9b8571a9785c16a14d, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_45719901486a4793942a9bf418a9d4ae.WireTo(id_7173a158f8dd41a4b3d0548b899b7cee, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_2d66cfd15d0e4e9b8571a9785c16a14d.WireTo(id_42a24cb00d7d479fbec70bfb0d9dc139, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_42a24cb00d7d479fbec70bfb0d9dc139.WireTo(EIDRecoveryFlashSuccess, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_e40c2e783fff47f19fcde91f7bc96521.WireTo(EIDRecoveryFlashProgress, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_e40c2e783fff47f19fcde91f7bc96521.WireTo(EIDRecoveryFlashFailed, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            EIDRecoveryFlashFailed.WireTo(id_f04a8a0948b24741955910bcfa0b43e0, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            EIDRecoveryFlashFailed.WireTo(id_e18b3e8bd1aa47288c25885d6e5a3fe3, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            EIDRecoveryFlashFailed.WireTo(id_09a79cabb73e4dfdb329ad8c6f50a213, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_f04a8a0948b24741955910bcfa0b43e0.WireTo(id_87414f2af84e4779a9592b30ac7b71d1, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_e18b3e8bd1aa47288c25885d6e5a3fe3.WireTo(EIDRecoveryFailedStatusString, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_09a79cabb73e4dfdb329ad8c6f50a213.WireTo(id_609ad71d48df4c9989f7f53869f4fb1f, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_609ad71d48df4c9989f7f53869f4fb1f.WireTo(EIDRecoveryFlashFailed, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            EIDRecoveryFlashProgress.WireTo(id_11ba57133396478784ea4c89cb5357cf, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            EIDRecoveryFlashProgress.WireTo(id_e23ce2bded7a4d8e957cb7c077d8af7e, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            EIDRecoveryFlashProgress.WireTo(id_ec2440a28e094437bf77cd753bf6e8e8, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_11ba57133396478784ea4c89cb5357cf.WireTo(id_8155771c5fd34b158166269259680aec, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"ProgressBar","DestinationIsReference":false} */
            id_8155771c5fd34b158166269259680aec.WireTo(EIDRecoveryFlashPercentage, "progressValue"); /* {"SourceType":"ProgressBar","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            EIDRecoveryUpdater.WireTo(EIDRecoveryFlashPercentage, "progressPercentage"); /* {"SourceType":"EidFirmwareUpdater","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            id_e23ce2bded7a4d8e957cb7c077d8af7e.WireTo(EIDRecoveryStatusString, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_ec2440a28e094437bf77cd753bf6e8e8.WireTo(id_677572cfb2954441b3a5c942b4a31f3f, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            EIDRecoverCheckDeviceType.WireTo(id_ba8385e1110c4dbcba3e40e2e87c3574, "logicalInputs"); /* {"SourceType":"AndEvent","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            EIDRecoveryNoFilepath.WireTo(id_965fe5fe6f76430c9c9e2b5fe88c710e, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            EIDRecoveryNoFilepath.WireTo(id_212660b7fc4a4e03bcf9d48649e4c73f, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_965fe5fe6f76430c9c9e2b5fe88c710e.WireTo(id_b01f7670dbd142a489fc36748da3c932, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_212660b7fc4a4e03bcf9d48649e4c73f.WireTo(id_a68043651f7b40e2bbb748c61d86fe5e, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_a68043651f7b40e2bbb748c61d86fe5e.WireTo(EIDRecoveryNoFilepath, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            EIDUpdateInformation.WireTo(EIDUpdateConnector, "output"); /* {"SourceType":"Switch","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            EIDUpdateConnector.WireTo(EIDGetFirmwareHash, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            EIDUpdateConnector.WireTo(EIDNewVersionAdapter, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            EIDUpdateConnector.WireTo(EIDFirmwarePathFormatter, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            EIDUpdateConnector.WireTo(EIDRetrieveURL, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            EIDUpdateConnector.WireTo(EIDVersionCompare, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"CompareFirmwareVersions","DestinationIsReference":false} */
            EIDValidateFirmware.WireTo(EIDSwitchFirmwareCode, "firmwareCodePort"); /* {"SourceType":"EidValidateFirmware","SourceIsReference":false,"DestinationType":"Switch","DestinationIsReference":false} */
            EIDFileValidateFirmware.WireTo(EIDSwitchFirmwareCode, "firmwareCodePort"); /* {"SourceType":"EidValidateFirmware","SourceIsReference":false,"DestinationType":"Switch","DestinationIsReference":false} */
            EIDSwitchFirmwareCode.WireTo(id_59680a5a0b2d494bb31764eade2b405f, "inputs"); /* {"SourceType":"Switch","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            EIDSwitchFirmwareCode.WireTo(id_e32da1bd4dde4026a042ce4290e8d7aa, "inputs"); /* {"SourceType":"Switch","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            UpdateFromFileConnector.WireTo(id_59680a5a0b2d494bb31764eade2b405f, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            UpdateFromFileConnector.WireTo(id_e32da1bd4dde4026a042ce4290e8d7aa, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            CheckForUpdatesConnector.WireTo(id_59680a5a0b2d494bb31764eade2b405f, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            CheckForUpdatesConnector.WireTo(id_e32da1bd4dde4026a042ce4290e8d7aa, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            scpDeviceNameConnector.WireTo(id_70cf715179874505ab5f042e1682d56e, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":true,"DestinationType":"Apply","DestinationIsReference":false} */
            scpDeviceNameConnector.WireTo(id_c92ed5ca8c454e569abff95c19ced160, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":true,"DestinationType":"Apply","DestinationIsReference":false} */
            scpDeviceNameConnector.WireTo(id_a78d88158171491097fa7e9a7ec4c434, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":true,"DestinationType":"Apply","DestinationIsReference":false} */
            scpDeviceNameConnector.WireTo(id_23072e7e52b54341bac02d2f6136f905, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":true,"DestinationType":"Apply","DestinationIsReference":false} */
            scpDeviceNameConnector.WireTo(id_1ccde769729441a882f2e57353493c51, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":true,"DestinationType":"Apply","DestinationIsReference":false} */
            scpDeviceNameConnector.WireTo(id_ebe2e08f45aa4a2b9301b07d7141bcb7, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":true,"DestinationType":"Apply","DestinationIsReference":false} */
            id_70cf715179874505ab5f042e1682d56e.WireTo(id_970d31d5cb904cbd8f87cce03a765832, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_c92ed5ca8c454e569abff95c19ced160.WireTo(EIDUpdate, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_a78d88158171491097fa7e9a7ec4c434.WireTo(EIDUpdateDownloadProgress, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_114514ea2f1b4eb7b4f2587f3ed018cf.WireTo(EIDFirmwareFlashProgress, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_ca510aa8b8364bfc95e7b5805a67061a.WireTo(SRS2FirmwareFlashSuccess, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_7e5736dde59b4d05bfc9f5c32f7b6c7b.WireTo(id_cf0ea7d7df94461198efeeb176676f43, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            scpDeviceNameConnector.WireTo(id_09c1ae0c0c0f43339537ab6df35528a8, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":true,"DestinationType":"Apply","DestinationIsReference":false} */
            id_0e7cf56442ea43598a50e21cb5e7e2d1.WireTo(EIDFirmwareFlashFailed, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            scpDeviceNameConnector.WireTo(id_6c288085eb414204895f754161c31577, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":true,"DestinationType":"Apply","DestinationIsReference":false} */
            id_1778a5a3ac204908b97abacafd6d4938.WireTo(id_b0e1629606704fb19e35864cd6d343ca, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            EIDUpdateInformation.WireTo(SRS2UpdateInformation, "inputs"); /* {"SourceType":"Switch","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            EIDUpdateInformation.WireTo(XRS2UpdateInformation, "inputs"); /* {"SourceType":"Switch","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_23072e7e52b54341bac02d2f6136f905.WireTo(id_114514ea2f1b4eb7b4f2587f3ed018cf, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_114514ea2f1b4eb7b4f2587f3ed018cf.WireTo(EIDFileFirmwareFlashProgress, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_1ccde769729441a882f2e57353493c51.WireTo(id_ca510aa8b8364bfc95e7b5805a67061a, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_ca510aa8b8364bfc95e7b5805a67061a.WireTo(EIDFileFirmwareFlashSuccess, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_09c1ae0c0c0f43339537ab6df35528a8.WireTo(id_0e7cf56442ea43598a50e21cb5e7e2d1, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_0e7cf56442ea43598a50e21cb5e7e2d1.WireTo(EIDFileFirmwareFlashFailed, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_6c288085eb414204895f754161c31577.WireTo(id_1778a5a3ac204908b97abacafd6d4938, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_1778a5a3ac204908b97abacafd6d4938.WireTo(id_1639feb335854759bdc24bc212373f51, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_ebe2e08f45aa4a2b9301b07d7141bcb7.WireTo(id_7e5736dde59b4d05bfc9f5c32f7b6c7b, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_7e5736dde59b4d05bfc9f5c32f7b6c7b.WireTo(id_a2830b2b842a49a7bd39efb9903e4835, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            scpDeviceNameConnector.WireTo(id_4330f2fdc8fb473a8b15452a0aeebd9b, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":true,"DestinationType":"Apply","DestinationIsReference":false} */
            id_4330f2fdc8fb473a8b15452a0aeebd9b.WireTo(EIDInvalidFirmwarePopup, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            DatalinkUpdateDownloader.WireTo(id_2f24b74093424fd1bc5aea1069d41f66, "downloadProgress"); /* {"SourceType":"DownloadFile","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            Scale5000UpdateDownloader.WireTo(id_4c3dceb87c314a8a8b678e5fc2d0c682, "downloadProgress"); /* {"SourceType":"DownloadFile","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            EIDUpdateDownloader.WireTo(id_9048ef7559b4412abed65f84cbac1c0f, "downloadProgress"); /* {"SourceType":"DownloadFile","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            EIDRecoverFirmwareTextBox.WireTo(id_d925cfbf11de4fc1a0d4c1d0ea69c86a, "textOutput"); /* {"SourceType":"TextBox","SourceIsReference":true,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_d925cfbf11de4fc1a0d4c1d0ea69c86a.WireTo(id_ed5d83930c4d4fe7b27d0b9016939366, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            EIDRecoveryUpdater.WireTo(id_d925cfbf11de4fc1a0d4c1d0ea69c86a, "firmwarePathPort"); /* {"SourceType":"EidFirmwareUpdater","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            Scale5000CurrentVersionAdapter.WireTo(Scale5000NoUpdatesCurrentVersion, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            Scale5000CompareVersions.WireTo(Scale5000CurrentVersionAdapter, "currentVersion"); /* {"SourceType":"CompareFirmwareVersions","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            ReinstallUSBDriverConnector.WireTo(StartUSBDriverInstallation, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"RunExecutable","DestinationIsReference":false} */
            StartUSBDriverInstallation.WireTo(USBDriverReinstallFailed, "failedToStart"); /* {"SourceType":"RunExecutable","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            USBDriverReinstallFailed.WireTo(id_4a271f7bf5b04314af7834622530ced5, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            USBDriverReinstallFailed.WireTo(id_47d3fe737d5741ae8692d628049cf688, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_47d3fe737d5741ae8692d628049cf688.WireTo(USBDriverReinstallFailed, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            DatalinkFirmwareCheckHash.WireTo(DatalinkUnzipUpdater, "hashMatch"); /* {"SourceType":"CheckFileMD5Hash","SourceIsReference":false,"DestinationType":"UnzipFile","DestinationIsReference":false} */
            DatalinkUnzipUpdater.WireTo(DatalinkRunUpdater, "fileUnzipped"); /* {"SourceType":"UnzipFile","SourceIsReference":false,"DestinationType":"RunExecutable","DestinationIsReference":false} */
            DatalinkUnzipUpdater.WireTo(DatalinkRunUpdater, "filePath"); /* {"SourceType":"UnzipFile","SourceIsReference":false,"DestinationType":"RunExecutable","DestinationIsReference":false} */
            AutomaticUpdateConnector.WireTo(id_4770c103caf14c028fbc7e1bf7d0591d, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"EventGate","DestinationIsReference":false} */
            AutomaticUpdatesPersist.WireTo(id_4770c103caf14c028fbc7e1bf7d0591d, "output"); /* {"SourceType":"Persist","SourceIsReference":true,"DestinationType":"EventGate","DestinationIsReference":false} */
            id_4770c103caf14c028fbc7e1bf7d0591d.WireTo(CheckForUpdatesConnector, "eventOutput"); /* {"SourceType":"EventGate","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            // END AUTO-GENERATED WIRING FOR FirmwareUpdate
            #endregion

            #region DeleteSessions_XRS2_SRS2 WIRING
            // BEGIN AUTO-GENERATED WIRING FOR DeleteSessions_XRS2_SRS2
            id_48d017247e2747059f60cfb040f4c3bf.WireTo(id_797742532e0e4b0498843a54daf1793f, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_48d017247e2747059f60cfb040f4c3bf.WireTo(id_9b6b728aa2484103a955e1e1aa84c0ee, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_797742532e0e4b0498843a54daf1793f.WireTo(id_1336f6c1f70645bb8a177050ceefd398, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_9b6b728aa2484103a955e1e1aa84c0ee.WireTo(id_4e7c07d78a2b4ca9affeb437e8c74df4, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"RightJustify","DestinationIsReference":false} */
            id_4e7c07d78a2b4ca9affeb437e8c74df4.WireTo(id_c713f89a6cb944679cda8e87e4285753, "children"); /* {"SourceType":"RightJustify","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_4e7c07d78a2b4ca9affeb437e8c74df4.WireTo(id_13173868102b466d84cf780f38376486, "children"); /* {"SourceType":"RightJustify","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_13173868102b466d84cf780f38376486.WireTo(id_48d017247e2747059f60cfb040f4c3bf, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_99513a6d5a09462a9cf09c2814d4582e.WireTo(scpArbitrator, "arbitrator"); /* {"SourceType":"SessionDeleteSCP","SourceIsReference":false,"DestinationType":"Arbitrator","DestinationIsReference":true} */
            id_99513a6d5a09462a9cf09c2814d4582e.WireTo(scpProtocol, "requestResponseDataFlow"); /* {"SourceType":"SessionDeleteSCP","SourceIsReference":false,"DestinationType":"SCPProtocol","DestinationIsReference":true} */
            id_f70b9a4eec2d49b3a7291d4ef5d0a0c3.WireTo(id_4a156b60efdb4abeb774e5a60eca9942, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_4a156b60efdb4abeb774e5a60eca9942.WireTo(id_8b9bd9c431684d319c570258beaf8156, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_f70b9a4eec2d49b3a7291d4ef5d0a0c3.WireTo(id_b5081ffa25064584a05fc3986466d713, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_b5081ffa25064584a05fc3986466d713.WireTo(id_bac2620e78614dde9dfe2bc2f160de15, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_bac2620e78614dde9dfe2bc2f160de15.WireTo(id_f70b9a4eec2d49b3a7291d4ef5d0a0c3, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_1cb57a3c0e6d4d1c92125280481be643.WireTo(id_1d3d376dbec34ba3b48754fe3fda25e0, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_1d3d376dbec34ba3b48754fe3fda25e0.WireTo(id_6b2396c36888442baaac6c495e616bca, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_1cb57a3c0e6d4d1c92125280481be643.WireTo(id_ec1017d8cac4449bb91ee3573a2c7093, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_ec1017d8cac4449bb91ee3573a2c7093.WireTo(id_4f418d359b864eeb884f40bd88adfee7, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_4f418d359b864eeb884f40bd88adfee7.WireTo(id_1cb57a3c0e6d4d1c92125280481be643, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_99513a6d5a09462a9cf09c2814d4582e.WireTo(id_1cb57a3c0e6d4d1c92125280481be643, "error"); /* {"SourceType":"SessionDeleteSCP","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            EIDDeleteSelectedSessions.WireTo(id_679dbd92de28437f86537199e22e801f, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Transact","DestinationIsReference":false} */
            id_679dbd92de28437f86537199e22e801f.WireTo(sessionListGrid, "tableDataFlowSource"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"Grid","DestinationIsReference":true} */
            SessionDeleteSelectedSessions.WireTo(scpProtocol, "requestResponseDataFlow"); /* {"SourceType":"SessionDeleteSCP","SourceIsReference":false,"DestinationType":"SCPProtocol","DestinationIsReference":true} */
            SessionDeleteSelectedSessions.WireTo(scpArbitrator, "arbitrator"); /* {"SourceType":"SessionDeleteSCP","SourceIsReference":false,"DestinationType":"Arbitrator","DestinationIsReference":true} */
            id_03660d1db16d4e1a9bbba3d2d9b7caac.WireTo(id_30c6e35bc85c4a91b6b8fa0a92e434f2, "dataFlowDataTableOutput"); /* {"SourceType":"Filter","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_30c6e35bc85c4a91b6b8fa0a92e434f2.WireTo(SessionDeleteSelectedSessions, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"SessionDeleteSCP","DestinationIsReference":false} */
            id_30c6e35bc85c4a91b6b8fa0a92e434f2.WireTo(id_39c3d1547b2d49c8b826ec82382f88c2, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            id_6a671f14aaa74810ad35f3cb58338dbe.WireTo(id_ce81189a00ff48d2a477b9591cc68c4a, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_ce81189a00ff48d2a477b9591cc68c4a.WireTo(id_868bdb726abd400dad035fdaa4ae6fa5, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_39c3d1547b2d49c8b826ec82382f88c2.WireTo(id_c7c7a1bb53af4646bbd253c377ca4133, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            id_c7c7a1bb53af4646bbd253c377ca4133.WireTo(id_868bdb726abd400dad035fdaa4ae6fa5, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_6a671f14aaa74810ad35f3cb58338dbe.WireTo(id_3ebca753f2b74540a303abf516ccaf6b, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_3ebca753f2b74540a303abf516ccaf6b.WireTo(id_e5c3854d95c1490c952b5dc0afd9ca0a, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"RightJustify","DestinationIsReference":false} */
            id_e5c3854d95c1490c952b5dc0afd9ca0a.WireTo(id_33893ffce90944808e3f8536badf39eb, "children"); /* {"SourceType":"RightJustify","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_e5c3854d95c1490c952b5dc0afd9ca0a.WireTo(id_56bd217ea1e643f8b2d3d774212c7f44, "children"); /* {"SourceType":"RightJustify","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_56bd217ea1e643f8b2d3d774212c7f44.WireTo(id_6a671f14aaa74810ad35f3cb58338dbe, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_33893ffce90944808e3f8536badf39eb.WireTo(id_d136475f9a66438a87c0d8c1977b1dfd, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_d136475f9a66438a87c0d8c1977b1dfd.WireTo(SessionDeleteSelectedSessions, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"SessionDeleteSCP","DestinationIsReference":false} */
            id_d136475f9a66438a87c0d8c1977b1dfd.WireTo(id_6a671f14aaa74810ad35f3cb58338dbe, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_6aca070ee6bf4492a0bd6a646966c528.WireTo(id_39c3d1547b2d49c8b826ec82382f88c2, "inputOne"); /* {"SourceType":"GreaterThan","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            id_679dbd92de28437f86537199e22e801f.WireTo(id_81bbaeec7b5b4bc8a8ea23774035d3e0, "eventCompleteNoErrors"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"IfElse","DestinationIsReference":false} */
            id_6aca070ee6bf4492a0bd6a646966c528.WireTo(id_81bbaeec7b5b4bc8a8ea23774035d3e0, "output"); /* {"SourceType":"GreaterThan","SourceIsReference":false,"DestinationType":"IfElse","DestinationIsReference":false} */
            id_81bbaeec7b5b4bc8a8ea23774035d3e0.WireTo(id_6a671f14aaa74810ad35f3cb58338dbe, "ifOutput"); /* {"SourceType":"IfElse","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_81bbaeec7b5b4bc8a8ea23774035d3e0.WireTo(id_2a5458b36d7b42a0910a5206f380a60f, "elseOutput"); /* {"SourceType":"IfElse","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_2a5458b36d7b42a0910a5206f380a60f.WireTo(id_319bda78453643948b2d3a33d288c58f, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_2a5458b36d7b42a0910a5206f380a60f.WireTo(id_813cd9ceb3fb4a12a532feed01390979, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_319bda78453643948b2d3a33d288c58f.WireTo(id_dcf6089271224392bfec8815468c5bae, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_813cd9ceb3fb4a12a532feed01390979.WireTo(id_4e620f498e474e7890d95aadc1c7edb8, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_4e620f498e474e7890d95aadc1c7edb8.WireTo(id_2a5458b36d7b42a0910a5206f380a60f, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_7169f3ed24b64038882656ad626a8971.WireTo(id_f70b9a4eec2d49b3a7291d4ef5d0a0c3, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            SessionDeleteSelectedSessions.WireTo(id_05a43661c70248fab459d30af3547158, "success"); /* {"SourceType":"SessionDeleteSCP","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            selectedSessionsRemoveTransact.WireTo(SessionDeleteSelectedSessions, "tableDataFlowSource"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"SessionDeleteSCP","DestinationIsReference":false} */
            id_05a43661c70248fab459d30af3547158.WireTo(selectedSessionsRemoveTransact, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Transact","DestinationIsReference":false} */
            SessionDeleteSelectedSessions.WireTo(id_1cb57a3c0e6d4d1c92125280481be643, "error"); /* {"SourceType":"SessionDeleteSCP","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_05a43661c70248fab459d30af3547158.WireTo(id_f70b9a4eec2d49b3a7291d4ef5d0a0c3, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_679dbd92de28437f86537199e22e801f.WireTo(id_2e55d4664efb49118dcdd70afef93f07, "tableDataFlowDestination"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"TableDataFlowConnector","DestinationIsReference":false} */
            id_2e55d4664efb49118dcdd70afef93f07.WireTo(id_03660d1db16d4e1a9bbba3d2d9b7caac, "tableDataFlowList"); /* {"SourceType":"TableDataFlowConnector","SourceIsReference":false,"DestinationType":"Filter","DestinationIsReference":false} */
            id_2e55d4664efb49118dcdd70afef93f07.WireTo(SessionDeleteSelectedSessions, "tableDataFlowList"); /* {"SourceType":"TableDataFlowConnector","SourceIsReference":false,"DestinationType":"SessionDeleteSCP","DestinationIsReference":false} */
            selectedSessionsRemoveTransact.WireTo(sessionListScpTableConnectorDestination, "tableDataFlowDestination"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"TableDataFlowConnector","DestinationIsReference":true} */
            EIDDeleteSelectedSessions.WireTo(id_a63396a48ea04b9a9bdffef9361693bc, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_a63396a48ea04b9a9bdffef9361693bc.WireTo(sessionListGrid, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"Grid","DestinationIsReference":true} */
            EIDDeleteAllSessions.WireTo(id_112e5d0dc095444fbf7f0d68c905f902, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Transact","DestinationIsReference":false} */
            id_112e5d0dc095444fbf7f0d68c905f902.WireTo(sessionListGrid, "tableDataFlowSource"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"Grid","DestinationIsReference":true} */
            id_112e5d0dc095444fbf7f0d68c905f902.WireTo(id_99513a6d5a09462a9cf09c2814d4582e, "tableDataFlowDestination"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"SessionDeleteSCP","DestinationIsReference":false} */
            id_7169f3ed24b64038882656ad626a8971.WireTo(id_355497fa88664d6db83547db4aa3ef70, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Transact","DestinationIsReference":false} */
            id_355497fa88664d6db83547db4aa3ef70.WireTo(id_99513a6d5a09462a9cf09c2814d4582e, "tableDataFlowSource"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"SessionDeleteSCP","DestinationIsReference":false} */
            id_355497fa88664d6db83547db4aa3ef70.WireTo(sessionListScpTableConnectorDestination, "tableDataFlowDestination"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"TableDataFlowConnector","DestinationIsReference":true} */
            EIDDeleteAllSessions.WireTo(id_a63396a48ea04b9a9bdffef9361693bc, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_c713f89a6cb944679cda8e87e4285753.WireTo(id_c56ede8d820d4242b1542cf810196011, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_c56ede8d820d4242b1542cf810196011.WireTo(id_99513a6d5a09462a9cf09c2814d4582e, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"SessionDeleteSCP","DestinationIsReference":false} */
            id_c56ede8d820d4242b1542cf810196011.WireTo(id_48d017247e2747059f60cfb040f4c3bf, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_6e66797b51924dd3a23647be2b2e0ab6.WireTo(id_bc12a1de672147d58ae2d2aea63dc36d, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_bc12a1de672147d58ae2d2aea63dc36d.WireTo(id_26c93d2744d7409cade0e0cf42f110dd, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_6e66797b51924dd3a23647be2b2e0ab6.WireTo(id_e0165e2b44834b50b5c5c4dc3006f4f9, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_e0165e2b44834b50b5c5c4dc3006f4f9.WireTo(id_f4c1df4913d1474e80ee4278caade871, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"RightJustify","DestinationIsReference":false} */
            id_f4c1df4913d1474e80ee4278caade871.WireTo(id_f8c34be2883a4ae9b4441e4e7d46a7a1, "children"); /* {"SourceType":"RightJustify","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_f4c1df4913d1474e80ee4278caade871.WireTo(id_0d409290252f4a3fb52f61b96509809e, "children"); /* {"SourceType":"RightJustify","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_0d409290252f4a3fb52f61b96509809e.WireTo(id_6e66797b51924dd3a23647be2b2e0ab6, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_f8c34be2883a4ae9b4441e4e7d46a7a1.WireTo(id_f920e30d2814465db92841666335f9f1, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_f920e30d2814465db92841666335f9f1.WireTo(id_6e66797b51924dd3a23647be2b2e0ab6, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_f920e30d2814465db92841666335f9f1.WireTo(id_c0607234d86748b8b9ec7acff4b86bf2, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"AlertDeleteSCP","DestinationIsReference":false} */
            id_c0607234d86748b8b9ec7acff4b86bf2.WireTo(scpArbitrator, "arbitrator"); /* {"SourceType":"AlertDeleteSCP","SourceIsReference":false,"DestinationType":"Arbitrator","DestinationIsReference":true} */
            id_c0607234d86748b8b9ec7acff4b86bf2.WireTo(scpProtocol, "requestResponseDataFlow"); /* {"SourceType":"AlertDeleteSCP","SourceIsReference":false,"DestinationType":"SCPProtocol","DestinationIsReference":true} */
            id_13a400afca4b4d3f96c04028d8879293.WireTo(id_3d3805dc630e4f99b55ad351a7a6f6bd, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_3d3805dc630e4f99b55ad351a7a6f6bd.WireTo(id_a5406d0cdeff4ab18b71efe7de537a46, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_13a400afca4b4d3f96c04028d8879293.WireTo(id_e5ff6b01c6754064a7acc3cdd747c5e6, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_e5ff6b01c6754064a7acc3cdd747c5e6.WireTo(id_177d155675b84070b7077ed353b9e0a8, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_177d155675b84070b7077ed353b9e0a8.WireTo(id_13a400afca4b4d3f96c04028d8879293, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_c0607234d86748b8b9ec7acff4b86bf2.WireTo(id_0b304d131bf6460fa2577ed5ceb0268e, "error"); /* {"SourceType":"AlertDeleteSCP","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_0b304d131bf6460fa2577ed5ceb0268e.WireTo(id_64c8aeb3ae3642a48cd2d2dac478b836, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_0b304d131bf6460fa2577ed5ceb0268e.WireTo(id_83e76fec24b248c0999f8eee149b3898, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Horizontal","DestinationIsReference":false} */
            id_64c8aeb3ae3642a48cd2d2dac478b836.WireTo(id_14a068d0ee1247c6bde4fd313cbe664c, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_83e76fec24b248c0999f8eee149b3898.WireTo(id_18b11b35e9944402a8da0979c60b7ac5, "children"); /* {"SourceType":"Horizontal","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_18b11b35e9944402a8da0979c60b7ac5.WireTo(id_0b304d131bf6460fa2577ed5ceb0268e, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            EIDDeleteSessions.WireTo(id_ad34acf12b57488b9793866f20c52269, "children"); /* {"SourceType":"Wizard","SourceIsReference":false,"DestinationType":"WizardItem","DestinationIsReference":false} */
            EIDDeleteSessions.WireTo(id_acc9a2d3912749fd813d3736040d86d6, "children"); /* {"SourceType":"Wizard","SourceIsReference":false,"DestinationType":"WizardItem","DestinationIsReference":false} */
            EIDDeleteSessions.WireTo(id_199db356282348de832495a04d6ca839, "children"); /* {"SourceType":"Wizard","SourceIsReference":false,"DestinationType":"WizardItem","DestinationIsReference":false} */
            id_ad34acf12b57488b9793866f20c52269.WireTo(EIDDeleteSelectedSessions, "eventOutput"); /* {"SourceType":"WizardItem","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_acc9a2d3912749fd813d3736040d86d6.WireTo(EIDDeleteAllSessions, "eventOutput"); /* {"SourceType":"WizardItem","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_199db356282348de832495a04d6ca839.WireTo(XRS2DeleteAlerts, "eventOutput"); /* {"SourceType":"WizardItem","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            XRS2ConnectedConnector.WireTo(id_199db356282348de832495a04d6ca839, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":true,"DestinationType":"WizardItem","DestinationIsReference":false} */
            id_7169f3ed24b64038882656ad626a8971.WireTo(id_8a24f36fe4a340d391a4205207293642, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_8a24f36fe4a340d391a4205207293642.WireTo(animalInfoCount, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":true} */
            id_72a41eed63e8409a9053392441c2792f.WireTo(id_13a400afca4b4d3f96c04028d8879293, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_72a41eed63e8409a9053392441c2792f.WireTo(id_c95839c10b874b05acc09e28bab9eadf, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_c95839c10b874b05acc09e28bab9eadf.WireTo(alertCountConnector, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":true} */
            XRS2DeleteAlerts.WireTo(id_e8e6abe9b4674f25bca071440c0be6a8, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Transact","DestinationIsReference":false} */
            id_e8e6abe9b4674f25bca071440c0be6a8.WireTo(alertDataSCP, "tableDataFlowSource"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"AlertDataSCP","DestinationIsReference":true} */
            id_e8e6abe9b4674f25bca071440c0be6a8.WireTo(id_6e66797b51924dd3a23647be2b2e0ab6, "eventCompleteNoErrors"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_e8e6abe9b4674f25bca071440c0be6a8.WireTo(id_c0607234d86748b8b9ec7acff4b86bf2, "tableDataFlowDestination"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"AlertDeleteSCP","DestinationIsReference":false} */
            id_72a41eed63e8409a9053392441c2792f.WireTo(id_1208c8e407e543d78662f15070f2bd62, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Transact","DestinationIsReference":false} */
            id_1208c8e407e543d78662f15070f2bd62.WireTo(id_c0607234d86748b8b9ec7acff4b86bf2, "tableDataFlowSource"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"AlertDeleteSCP","DestinationIsReference":false} */
            id_1208c8e407e543d78662f15070f2bd62.WireTo(alertDataSCP, "tableDataFlowDestination"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"AlertDataSCP","DestinationIsReference":true} */
            id_112e5d0dc095444fbf7f0d68c905f902.WireTo(id_ba6d5278cb09412ab8aae27671ae94ae, "eventCompleteNoErrors"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"Transact","DestinationIsReference":false} */
            id_ba6d5278cb09412ab8aae27671ae94ae.WireTo(id_48d017247e2747059f60cfb040f4c3bf, "eventCompleteNoErrors"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_99513a6d5a09462a9cf09c2814d4582e.WireTo(id_2895bf95b5b043d695b7c3d287e6d381, "success"); /* {"SourceType":"SessionDeleteSCP","SourceIsReference":false,"DestinationType":"LifeDataDeleteSCP","DestinationIsReference":false} */
            id_2895bf95b5b043d695b7c3d287e6d381.WireTo(id_7169f3ed24b64038882656ad626a8971, "success"); /* {"SourceType":"LifeDataDeleteSCP","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_2895bf95b5b043d695b7c3d287e6d381.WireTo(scpArbitrator, "arbitrator"); /* {"SourceType":"LifeDataDeleteSCP","SourceIsReference":false,"DestinationType":"Arbitrator","DestinationIsReference":true} */
            id_2895bf95b5b043d695b7c3d287e6d381.WireTo(scpProtocol, "requestResponseDataFlow"); /* {"SourceType":"LifeDataDeleteSCP","SourceIsReference":false,"DestinationType":"SCPProtocol","DestinationIsReference":true} */
            id_2895bf95b5b043d695b7c3d287e6d381.WireTo(id_1cb57a3c0e6d4d1c92125280481be643, "error"); /* {"SourceType":"LifeDataDeleteSCP","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_ba6d5278cb09412ab8aae27671ae94ae.WireTo(id_2895bf95b5b043d695b7c3d287e6d381, "tableDataFlowDestination"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"LifeDataDeleteSCP","DestinationIsReference":false} */
            id_ba6d5278cb09412ab8aae27671ae94ae.WireTo(lifeDataSCP, "tableDataFlowSource"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"LifeDataSCP","DestinationIsReference":true} */
            id_7169f3ed24b64038882656ad626a8971.WireTo(id_5f3eab9aeec94326bd95c9d0a42a992e, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Transact","DestinationIsReference":false} */
            id_5f3eab9aeec94326bd95c9d0a42a992e.WireTo(lifeDataSCP, "tableDataFlowDestination"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"LifeDataSCP","DestinationIsReference":true} */
            id_5f3eab9aeec94326bd95c9d0a42a992e.WireTo(id_2895bf95b5b043d695b7c3d287e6d381, "tableDataFlowSource"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"LifeDataDeleteSCP","DestinationIsReference":false} */
            scpDeviceNameConnector.WireTo(id_b3f079ac06c940709312f2fbf7235ace, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":true,"DestinationType":"Switch","DestinationIsReference":false} */
            id_b3f079ac06c940709312f2fbf7235ace.WireTo(id_5e42eed1a5e446e299df0731a1020c71, "inputs"); /* {"SourceType":"Switch","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_b3f079ac06c940709312f2fbf7235ace.WireTo(id_45ceacd53ddd44ceb79ae8d5b8f2c817, "inputs"); /* {"SourceType":"Switch","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_b3f079ac06c940709312f2fbf7235ace.WireTo(id_5e42eed1a5e446e299df0731a1020c71, "defaultInput"); /* {"SourceType":"Switch","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_b3f079ac06c940709312f2fbf7235ace.WireTo(id_acc9a2d3912749fd813d3736040d86d6, "output"); /* {"SourceType":"Switch","SourceIsReference":false,"DestinationType":"WizardItem","DestinationIsReference":false} */
            id_7169f3ed24b64038882656ad626a8971.WireTo(id_7349f43f57ae4f14a0041a3ffc64995e, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"ClearTable","DestinationIsReference":false} */
            id_c0607234d86748b8b9ec7acff4b86bf2.WireTo(id_72a41eed63e8409a9053392441c2792f, "success"); /* {"SourceType":"AlertDeleteSCP","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_7349f43f57ae4f14a0041a3ffc64995e.WireTo(alertDataSCP, "table"); /* {"SourceType":"ClearTable","SourceIsReference":false,"DestinationType":"AlertDataSCP","DestinationIsReference":true} */
            id_7169f3ed24b64038882656ad626a8971.WireTo(id_c95839c10b874b05acc09e28bab9eadf, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            // END AUTO-GENERATED WIRING FOR DeleteSessions_XRS2_SRS2
            #endregion

            #region DeleteSessions_Scale5000 WIRING
            // BEGIN AUTO-GENERATED WIRING FOR DeleteSessions_Scale5000 
            Scale5000DeleteSessions.WireTo(id_452cd410b68249898c9a38d8090d75bb, "children"); /* {"SourceType":"Wizard","SourceIsReference":false,"DestinationType":"WizardItem","DestinationIsReference":false} */
            Scale5000DeleteSessions.WireTo(id_cdabf62c83304b42831f31db68d86cdb, "children"); /* {"SourceType":"Wizard","SourceIsReference":false,"DestinationType":"WizardItem","DestinationIsReference":false} */
            Scale5000DeleteSessions.WireTo(id_f9502c5e92d24728870748a784b8f8d7, "children"); /* {"SourceType":"Wizard","SourceIsReference":false,"DestinationType":"WizardItem","DestinationIsReference":false} */
            id_452cd410b68249898c9a38d8090d75bb.WireTo(Scale5000DeleteSelectedSessions, "eventOutput"); /* {"SourceType":"WizardItem","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_cdabf62c83304b42831f31db68d86cdb.WireTo(Scale5000DeleteAllSessions, "eventOutput"); /* {"SourceType":"WizardItem","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_f9502c5e92d24728870748a784b8f8d7.WireTo(Scale5000DeleteAnimalsInSesion, "eventOutput"); /* {"SourceType":"WizardItem","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            Scale5000DeleteSelectedSessions.WireTo(id_7e593a1e387f48ec9eef520448218fe6, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Transact","DestinationIsReference":false} */
            id_7e593a1e387f48ec9eef520448218fe6.WireTo(sessionListGrid, "tableDataFlowSource"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"Grid","DestinationIsReference":true} */
            id_7e593a1e387f48ec9eef520448218fe6.WireTo(id_245f98c7072442b7aa71b681f5d00062, "tableDataFlowDestination"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"TableDataFlowConnector","DestinationIsReference":false} */
            id_245f98c7072442b7aa71b681f5d00062.WireTo(id_8877f6deea8b404db28ac84f1c82cf84, "tableDataFlowList"); /* {"SourceType":"TableDataFlowConnector","SourceIsReference":false,"DestinationType":"Filter","DestinationIsReference":false} */
            id_8877f6deea8b404db28ac84f1c82cf84.WireTo(id_6ef1a7ff90204dd2a06dcd7eb1f7fceb, "dataFlowDataTableOutput"); /* {"SourceType":"Filter","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_6ef1a7ff90204dd2a06dcd7eb1f7fceb.WireTo(id_59d757678fe8437ca54b51662a37be5a, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            id_7e593a1e387f48ec9eef520448218fe6.WireTo(id_da940a49c596462fa0b172a83d433227, "eventCompleteNoErrors"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"IfElse","DestinationIsReference":false} */
            id_4f0605cede024da783149f93cee20caa.WireTo(id_59d757678fe8437ca54b51662a37be5a, "inputOne"); /* {"SourceType":"GreaterThan","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            id_59d757678fe8437ca54b51662a37be5a.WireTo(id_df92778da4a047b3813805808354533e, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            id_4f0605cede024da783149f93cee20caa.WireTo(id_da940a49c596462fa0b172a83d433227, "output"); /* {"SourceType":"GreaterThan","SourceIsReference":false,"DestinationType":"IfElse","DestinationIsReference":false} */
            id_da940a49c596462fa0b172a83d433227.WireTo(id_63d67d8001274b9c90329e0f7841a8d6, "elseOutput"); /* {"SourceType":"IfElse","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_63d67d8001274b9c90329e0f7841a8d6.WireTo(id_a257e73b8a3c47e9925d6521be00a55d, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_63d67d8001274b9c90329e0f7841a8d6.WireTo(id_9ed1b051b79841909cc93a4b0523260f, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_9ed1b051b79841909cc93a4b0523260f.WireTo(id_63d67d8001274b9c90329e0f7841a8d6, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_da940a49c596462fa0b172a83d433227.WireTo(id_b755a40bf8cf48b8b712391e4f0da6bc, "ifOutput"); /* {"SourceType":"IfElse","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_b755a40bf8cf48b8b712391e4f0da6bc.WireTo(id_45888abd3d0946fca32ebdacc087bccd, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_df92778da4a047b3813805808354533e.WireTo(id_45888abd3d0946fca32ebdacc087bccd, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_b755a40bf8cf48b8b712391e4f0da6bc.WireTo(id_f7971d205f764b0f9cf7afc6d5a231be, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"RightJustify","DestinationIsReference":false} */
            id_f7971d205f764b0f9cf7afc6d5a231be.WireTo(id_887f3b39d6dc4347a8155ec7ff07383e, "children"); /* {"SourceType":"RightJustify","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_f7971d205f764b0f9cf7afc6d5a231be.WireTo(id_ec69ddaab61a46c6b7e63dbbb2cac700, "children"); /* {"SourceType":"RightJustify","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_ec69ddaab61a46c6b7e63dbbb2cac700.WireTo(id_b755a40bf8cf48b8b712391e4f0da6bc, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_6ef1a7ff90204dd2a06dcd7eb1f7fceb.WireTo(id_d2ada736889c41d5a1d7e26e23018a61, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"SessionDeleteQuery","DestinationIsReference":false} */
            id_245f98c7072442b7aa71b681f5d00062.WireTo(id_d2ada736889c41d5a1d7e26e23018a61, "tableDataFlowList"); /* {"SourceType":"TableDataFlowConnector","SourceIsReference":false,"DestinationType":"SessionDeleteQuery","DestinationIsReference":false} */
            id_887f3b39d6dc4347a8155ec7ff07383e.WireTo(id_e49d83414cf64a579de25bd2885778ef, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_e49d83414cf64a579de25bd2885778ef.WireTo(id_d2ada736889c41d5a1d7e26e23018a61, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"SessionDeleteQuery","DestinationIsReference":false} */
            id_e49d83414cf64a579de25bd2885778ef.WireTo(id_b755a40bf8cf48b8b712391e4f0da6bc, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_d2ada736889c41d5a1d7e26e23018a61.WireTo(SqliteDb, "sqlDataFlow"); /* {"SourceType":"SessionDeleteQuery","SourceIsReference":false,"DestinationType":"SqliteDB","DestinationIsReference":true} */
            id_d2ada736889c41d5a1d7e26e23018a61.WireTo(id_9fce0aa410dc42bb8d7f69e5343c539e, "success"); /* {"SourceType":"SessionDeleteQuery","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_9fce0aa410dc42bb8d7f69e5343c539e.WireTo(id_482e9408b3fd4a6e9bc4c228ca2fb1d9, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Transact","DestinationIsReference":false} */
            id_482e9408b3fd4a6e9bc4c228ca2fb1d9.WireTo(id_d2ada736889c41d5a1d7e26e23018a61, "tableDataFlowSource"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"SessionDeleteQuery","DestinationIsReference":false} */
            id_482e9408b3fd4a6e9bc4c228ca2fb1d9.WireTo(sessionListTableConnector, "tableDataFlowDestination"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"TableDataFlowConnector","DestinationIsReference":true} */
            id_9fce0aa410dc42bb8d7f69e5343c539e.WireTo(id_a0babac309174795ae32cdf5beaeee75, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_a0babac309174795ae32cdf5beaeee75.WireTo(id_893461027de245aea1b73581680a8c27, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_a0babac309174795ae32cdf5beaeee75.WireTo(id_dff2dc973a9a4bee8c4bde410f8167c2, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_dff2dc973a9a4bee8c4bde410f8167c2.WireTo(id_a0babac309174795ae32cdf5beaeee75, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_d2ada736889c41d5a1d7e26e23018a61.WireTo(id_7da7a71793e44a63807bd8f2028e0718, "error"); /* {"SourceType":"SessionDeleteQuery","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_7da7a71793e44a63807bd8f2028e0718.WireTo(id_31dc680a9e884d2696bf4b97f953fbd5, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_7da7a71793e44a63807bd8f2028e0718.WireTo(id_5ce7b528b8cc4586924b17606cb9a449, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_5ce7b528b8cc4586924b17606cb9a449.WireTo(id_7da7a71793e44a63807bd8f2028e0718, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            Scale5000DeleteAllSessions.WireTo(id_7bdd96ea8be0487281bae9f7fa04e945, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Transact","DestinationIsReference":false} */
            id_7bdd96ea8be0487281bae9f7fa04e945.WireTo(sessionListGrid, "tableDataFlowSource"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"Grid","DestinationIsReference":true} */
            id_ba3e627f147f4f63bd4fa491ef4013ce.WireTo(id_00b9806a4d1141beaf2b5250bfff2838, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_ba3e627f147f4f63bd4fa491ef4013ce.WireTo(id_e2b7d29c2aa64d578f7544870a2963fc, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"RightJustify","DestinationIsReference":false} */
            id_e2b7d29c2aa64d578f7544870a2963fc.WireTo(id_bcb9738028d944d38a8d786b6c741155, "children"); /* {"SourceType":"RightJustify","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_e2b7d29c2aa64d578f7544870a2963fc.WireTo(id_c87d65ad50ac4ac6bfd72ac7b8a22464, "children"); /* {"SourceType":"RightJustify","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_c87d65ad50ac4ac6bfd72ac7b8a22464.WireTo(id_ba3e627f147f4f63bd4fa491ef4013ce, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_bcb9738028d944d38a8d786b6c741155.WireTo(id_61c7d695c4164f15ae887d50b04771ad, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_61c7d695c4164f15ae887d50b04771ad.WireTo(id_ba3e627f147f4f63bd4fa491ef4013ce, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_61c7d695c4164f15ae887d50b04771ad.WireTo(id_bf2bf99800ae46c2a787ab888db762cf, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"SessionDeleteQuery","DestinationIsReference":false} */
            id_7bdd96ea8be0487281bae9f7fa04e945.WireTo(id_bf2bf99800ae46c2a787ab888db762cf, "tableDataFlowDestination"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"SessionDeleteQuery","DestinationIsReference":false} */
            id_bf2bf99800ae46c2a787ab888db762cf.WireTo(SqliteDb, "sqlDataFlow"); /* {"SourceType":"SessionDeleteQuery","SourceIsReference":false,"DestinationType":"SqliteDB","DestinationIsReference":true} */
            id_bf2bf99800ae46c2a787ab888db762cf.WireTo(id_7da7a71793e44a63807bd8f2028e0718, "error"); /* {"SourceType":"SessionDeleteQuery","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_bf2bf99800ae46c2a787ab888db762cf.WireTo(id_945a98d708ff4442bd44dada16462256, "success"); /* {"SourceType":"SessionDeleteQuery","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_945a98d708ff4442bd44dada16462256.WireTo(id_f833a4234daa456ca221ca14cd9c477f, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Transact","DestinationIsReference":false} */
            id_f833a4234daa456ca221ca14cd9c477f.WireTo(id_bf2bf99800ae46c2a787ab888db762cf, "tableDataFlowSource"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"SessionDeleteQuery","DestinationIsReference":false} */
            id_f833a4234daa456ca221ca14cd9c477f.WireTo(sessionListTableConnector, "tableDataFlowDestination"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"TableDataFlowConnector","DestinationIsReference":true} */
            id_945a98d708ff4442bd44dada16462256.WireTo(id_5f4dceb4a7d549db8a369ae531ac0ede, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"LifeDataDeleteQuery","DestinationIsReference":false} */
            id_5f4dceb4a7d549db8a369ae531ac0ede.WireTo(SqliteDb, "sqlDataFlow"); /* {"SourceType":"LifeDataDeleteQuery","SourceIsReference":false,"DestinationType":"SqliteDB","DestinationIsReference":true} */
            id_5f4dceb4a7d549db8a369ae531ac0ede.WireTo(id_2c3aa0324aeb4bf497b8aebb30416cf7, "success"); /* {"SourceType":"LifeDataDeleteQuery","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_2c3aa0324aeb4bf497b8aebb30416cf7.WireTo(id_8b01316204af48b794ccbae9d69dd91a, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Transact","DestinationIsReference":false} */
            id_8b01316204af48b794ccbae9d69dd91a.WireTo(id_5f4dceb4a7d549db8a369ae531ac0ede, "tableDataFlowSource"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"LifeDataDeleteQuery","DestinationIsReference":false} */
            id_2c3aa0324aeb4bf497b8aebb30416cf7.WireTo(id_a0babac309174795ae32cdf5beaeee75, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_7bdd96ea8be0487281bae9f7fa04e945.WireTo(id_c0dbb58f39c14bc1bb30313e9cfdf42a, "eventCompleteNoErrors"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"Transact","DestinationIsReference":false} */
            id_c0dbb58f39c14bc1bb30313e9cfdf42a.WireTo(id_ba3e627f147f4f63bd4fa491ef4013ce, "eventCompleteNoErrors"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_c0dbb58f39c14bc1bb30313e9cfdf42a.WireTo(id_5f4dceb4a7d549db8a369ae531ac0ede, "tableDataFlowDestination"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"LifeDataDeleteQuery","DestinationIsReference":false} */
            id_c0dbb58f39c14bc1bb30313e9cfdf42a.WireTo(animalInfoQuerySelect, "tableDataFlowSource"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"SelectColumns","DestinationIsReference":true} */
            id_5f4dceb4a7d549db8a369ae531ac0ede.WireTo(id_a0babac309174795ae32cdf5beaeee75, "error"); /* {"SourceType":"LifeDataDeleteQuery","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_8b01316204af48b794ccbae9d69dd91a.WireTo(animalInfoQuerySelect, "tableDataFlowDestination"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"SelectColumns","DestinationIsReference":true} */
            Scale5000DeleteAnimalsInSesion.WireTo(id_07294915e06f40488cd68107d8a1d274, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Transact","DestinationIsReference":false} */
            id_07294915e06f40488cd68107d8a1d274.WireTo(id_9b06445643aa4ca1974a1fb9a442c16c, "tableDataFlowDestination"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"TableDataFlowConnector","DestinationIsReference":false} */
            id_9b06445643aa4ca1974a1fb9a442c16c.WireTo(id_18a02f8deb2345348b49e59b58c9136d, "tableDataFlowList"); /* {"SourceType":"TableDataFlowConnector","SourceIsReference":false,"DestinationType":"Filter","DestinationIsReference":false} */
            id_18a02f8deb2345348b49e59b58c9136d.WireTo(id_c02f762bfa2645e1bb8636f3d8be0e90, "dataFlowDataTableOutput"); /* {"SourceType":"Filter","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":false} */
            id_c02f762bfa2645e1bb8636f3d8be0e90.WireTo(id_7b5646861c0d42cc9e95ed4c23cd7b9e, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            id_8e8408cd330243e78963f04ae4c59663.WireTo(id_7b5646861c0d42cc9e95ed4c23cd7b9e, "inputOne"); /* {"SourceType":"GreaterThan","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            id_07294915e06f40488cd68107d8a1d274.WireTo(sessionListGrid, "tableDataFlowSource"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"Grid","DestinationIsReference":true} */
            id_07294915e06f40488cd68107d8a1d274.WireTo(id_adc7cb23cac64e26882a0fde31532fb0, "eventCompleteNoErrors"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"IfElse","DestinationIsReference":false} */
            id_8e8408cd330243e78963f04ae4c59663.WireTo(id_adc7cb23cac64e26882a0fde31532fb0, "output"); /* {"SourceType":"GreaterThan","SourceIsReference":false,"DestinationType":"IfElse","DestinationIsReference":false} */
            id_adc7cb23cac64e26882a0fde31532fb0.WireTo(id_b32d386e15804d5486513738ddc6d381, "ifOutput"); /* {"SourceType":"IfElse","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_b32d386e15804d5486513738ddc6d381.WireTo(id_80f7974b4de04ff4b668e26b1399b75e, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_b32d386e15804d5486513738ddc6d381.WireTo(id_37e080a10e0f4b208894b41eb507c5f5, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"RightJustify","DestinationIsReference":false} */
            id_37e080a10e0f4b208894b41eb507c5f5.WireTo(id_4674a092953845ab97e43d46909bca70, "children"); /* {"SourceType":"RightJustify","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_37e080a10e0f4b208894b41eb507c5f5.WireTo(id_63c90c7440d448f894d9b46fa6a695bc, "children"); /* {"SourceType":"RightJustify","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_7b5646861c0d42cc9e95ed4c23cd7b9e.WireTo(id_b8b58b54f72942deb892801d072c79cd, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"Apply","DestinationIsReference":false} */
            id_adc7cb23cac64e26882a0fde31532fb0.WireTo(id_1b38d526b9434409b3bcc8e8353aa456, "elseOutput"); /* {"SourceType":"IfElse","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_1b38d526b9434409b3bcc8e8353aa456.WireTo(id_f9f31de8fd5f427796da7edb235551cf, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_1b38d526b9434409b3bcc8e8353aa456.WireTo(id_71a22611bcae461db6cc0e7a0fd6b47c, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_71a22611bcae461db6cc0e7a0fd6b47c.WireTo(id_1b38d526b9434409b3bcc8e8353aa456, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_4674a092953845ab97e43d46909bca70.WireTo(id_a0c10e35eb904703ab5efa42de1ad66d, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_a0c10e35eb904703ab5efa42de1ad66d.WireTo(id_b32d386e15804d5486513738ddc6d381, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_63c90c7440d448f894d9b46fa6a695bc.WireTo(id_b32d386e15804d5486513738ddc6d381, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_b8b58b54f72942deb892801d072c79cd.WireTo(id_80f7974b4de04ff4b668e26b1399b75e, "output"); /* {"SourceType":"Apply","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_a0c10e35eb904703ab5efa42de1ad66d.WireTo(id_4fee89bab42045aa88486ea9f65ee3aa, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"SessionDataDeleteQuery","DestinationIsReference":false} */
            id_c02f762bfa2645e1bb8636f3d8be0e90.WireTo(id_4fee89bab42045aa88486ea9f65ee3aa, "fanoutList"); /* {"SourceType":"DataFlowConnector","SourceIsReference":false,"DestinationType":"SessionDataDeleteQuery","DestinationIsReference":false} */
            id_5ec709fbd7dc48daa896c73d9c4dbab2.WireTo(sessionListTableConnector, "tableDataFlowDestination"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"TableDataFlowConnector","DestinationIsReference":true} */
            id_4fee89bab42045aa88486ea9f65ee3aa.WireTo(id_c74e418370384cefb728c0128c54c885, "success"); /* {"SourceType":"SessionDataDeleteQuery","SourceIsReference":false,"DestinationType":"EventConnector","DestinationIsReference":false} */
            id_c74e418370384cefb728c0128c54c885.WireTo(id_5ec709fbd7dc48daa896c73d9c4dbab2, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Transact","DestinationIsReference":false} */
            id_4fee89bab42045aa88486ea9f65ee3aa.WireTo(SqliteDb, "sqlDataFlow"); /* {"SourceType":"SessionDataDeleteQuery","SourceIsReference":false,"DestinationType":"SqliteDB","DestinationIsReference":true} */
            id_c74e418370384cefb728c0128c54c885.WireTo(id_69a132bec2a540798f5192ab4bec6fb2, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_69a132bec2a540798f5192ab4bec6fb2.WireTo(id_0ec5ba53a8d44676a6f745d8a4af84b5, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_69a132bec2a540798f5192ab4bec6fb2.WireTo(id_f8f451bf51b242378f6eb46dd24d75d9, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_f8f451bf51b242378f6eb46dd24d75d9.WireTo(id_69a132bec2a540798f5192ab4bec6fb2, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_4fee89bab42045aa88486ea9f65ee3aa.WireTo(id_767277e46f72429c808ac2a746a8e328, "error"); /* {"SourceType":"SessionDataDeleteQuery","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_767277e46f72429c808ac2a746a8e328.WireTo(id_5140e98ae93d4a06ba659d476e1d8f63, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Text","DestinationIsReference":false} */
            id_767277e46f72429c808ac2a746a8e328.WireTo(id_7af80d5722b847688de777730b0781c5, "children"); /* {"SourceType":"PopupWindow","SourceIsReference":false,"DestinationType":"Button","DestinationIsReference":false} */
            id_7af80d5722b847688de777730b0781c5.WireTo(id_767277e46f72429c808ac2a746a8e328, "eventButtonClicked"); /* {"SourceType":"Button","SourceIsReference":false,"DestinationType":"PopupWindow","DestinationIsReference":false} */
            id_5ec709fbd7dc48daa896c73d9c4dbab2.WireTo(id_4fee89bab42045aa88486ea9f65ee3aa, "tableDataFlowSource"); /* {"SourceType":"Transact","SourceIsReference":false,"DestinationType":"SessionDataDeleteQuery","DestinationIsReference":false} */
            id_9b06445643aa4ca1974a1fb9a442c16c.WireTo(id_4fee89bab42045aa88486ea9f65ee3aa, "tableDataFlowList"); /* {"SourceType":"TableDataFlowConnector","SourceIsReference":false,"DestinationType":"SessionDataDeleteQuery","DestinationIsReference":false} */
            id_2c3aa0324aeb4bf497b8aebb30416cf7.WireTo(id_e5e6fb2059f549d3a5dd80e2a70f6ef3, "fanoutList"); /* {"SourceType":"EventConnector","SourceIsReference":false,"DestinationType":"Data","DestinationIsReference":false} */
            id_e5e6fb2059f549d3a5dd80e2a70f6ef3.WireTo(animalInfoCount, "dataOutput"); /* {"SourceType":"Data","SourceIsReference":false,"DestinationType":"DataFlowConnector","DestinationIsReference":true} */
            // END AUTO-GENERATED WIRING FOR DeleteSessions_Scale5000
            #endregion

            #endregion

            #region manually-added wiring for parser code (must execute after all other code)

            #endregion
        }














}

















































