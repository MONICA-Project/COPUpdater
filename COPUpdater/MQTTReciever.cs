using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Client;
using MQTTnet;
using MQTTnet.Client.Receiving;
using MQTTnet.Client.Options;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Xml;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
namespace COPUpdater
{
    public class MQTTReciever
    {
        static
        MQTTnet.Extensions.ManagedClient.ManagedMqttClient ml = null;
        Dictionary<string, int> ThingsFromDataStreams = new Dictionary<string, int>();
        Dictionary<string, int> PeopleFromDataStreams = new Dictionary<string, int>();
        Dictionary<string, int> ZoneFromDataStreams = new Dictionary<string, int>();
        List<string> datastreams = new List<string>();
        public string thingtype = "";
        bool useDBDirectly = true;
        HubConnection connection;
        string copApiBasePath = "";
        string copApiBasePathHub = "";
        string copAuthToken = "";
        bool usesignalR = false;
        bool useOnlySignalR = false;
        bool mqttDebug = false;
        bool aggregate = false;

        private readonly ILogger _logger;

        public MQTTReciever()
        {
            var logFactory = new LoggerFactory()
           .AddConsole(LogLevel.Warning)
           .AddConsole()
           .AddDebug();

            var logger = logFactory.CreateLogger<MQTTReciever>();
            _logger = logger;
        }


        /// <summary>
        /// Starts the updater
        /// </summary>
        public void init()
        {
            //Read environment settings.
            thingtype = Environment.GetEnvironmentVariable("MONICA_THING_TYPE");
            copApiBasePath = Environment.GetEnvironmentVariable("COP_API_BASE_PATH");
            copApiBasePathHub = Environment.GetEnvironmentVariable("COP_API_BASE_PATH_HUB");
            copAuthToken = Environment.GetEnvironmentVariable("COP_AUTH_TOKEN");
            string signalR = Environment.GetEnvironmentVariable("signalR");
            string OnlySignalR = Environment.GetEnvironmentVariable("useOnlySignalR");
            string smqttDebug = Environment.GetEnvironmentVariable("mqttDebug");
            string aggregateString = Environment.GetEnvironmentVariable("aggregate");

            if (signalR != null && signalR != "")
                usesignalR = bool.Parse(signalR);
            if (OnlySignalR != null && OnlySignalR != "")
                useOnlySignalR = bool.Parse(OnlySignalR);
            if (smqttDebug != null && smqttDebug != "")
                mqttDebug = bool.Parse(smqttDebug);
            if (aggregateString != null && aggregateString != "")
                aggregate = bool.Parse(aggregateString);


            // Start DB Connect and make a simple query to initialize it

            IO.Swagger.DatabaseInterface.DBObservation dbO = new IO.Swagger.DatabaseInterface.DBObservation();
            string error = "";
            dbO.ListObs(2, ref error);


            //Fetch all datastreams that we want to handle
            System.Console.WriteLine("Fetching:" + thingtype);
            string JsonResult = "";
            WebClient wclient = new WebClient();
            try
            {
                wclient.Encoding = System.Text.Encoding.UTF8;
                wclient.Headers["Accept"] = "application/json";
                wclient.Headers["Content-Type"] = "application/json";
                wclient.Headers["Authorization"] = copAuthToken;
                JsonResult = wclient.DownloadString(copApiBasePath + "thingsWithObservation?thingType=" + thingtype.Replace(" ", "%20"));



            }
            catch (WebException exception)
            {
                System.Console.WriteLine("Invokation error" + copApiBasePath + "thingsWithObservation " + exception.Message);
            }

            string InnerText = JsonResult;


            // Build x-ref of thingId, personId, ZoneId mapped to topic.
            InnerText = "{Kalle:" + InnerText + "}";
            XmlDocument xDoc = JsonConvert.DeserializeXmlNode(InnerText, "Root");
            XmlNodeList Obs = xDoc.SelectNodes("//observations");
            int i = 0;
            foreach (XmlNode ob in Obs)
            {
                // If there are limits to the number of devices, be sure to keep inside them.
                if (i >= settings.deviceStartIndex && i < settings.deviceEndIndex)
                {
                    int personId = -1;
                    int zoneId = -1;
                    XmlNode xThingId = ob.SelectSingleNode("./thingId");
                    if (xThingId != null)
                    {
                        int thingId = int.Parse(xThingId.InnerText);
                        XmlNode xPersonid = ob.SelectSingleNode("./personid");
                        if (xPersonid.InnerText != "" && xPersonid.InnerText != "null")
                            personId = int.Parse(xPersonid.InnerText);
                        XmlNode xZoneid = ob.SelectSingleNode("./zoneid");
                        if (xZoneid.InnerText != "")
                            zoneId = int.Parse(xZoneid.InnerText);
                        XmlNode xStreamIdPlusMore = ob.SelectSingleNode("./datastreamId");
                        string streamId = xStreamIdPlusMore.InnerText.Substring(0, xStreamIdPlusMore.InnerText.IndexOf(":"));
                        XmlNode xStreamType = ob.SelectSingleNode("./type");
                        string streamType = xStreamType.InnerText;
                        if (!ThingsFromDataStreams.ContainsKey(streamId) && (streamType=="AGGREGATE" && aggregate))
                        {
                            ThingsFromDataStreams.Add(streamId, thingId);
                            datastreams.Add(streamId);
                        }
                        if (!ThingsFromDataStreams.ContainsKey(streamId) && (streamType != "AGGREGATE" && !aggregate))
                        {
                            ThingsFromDataStreams.Add(streamId, thingId);
                            datastreams.Add(streamId);
                        }
                        if (personId > 0)
                            if (!PeopleFromDataStreams.ContainsKey(streamId))
                            {
                                PeopleFromDataStreams.Add(streamId, personId);
                            }
                        if (zoneId > 0)
                            if (!ZoneFromDataStreams.ContainsKey(streamId))
                            {
                                ZoneFromDataStreams.Add(streamId, zoneId);
                            }
                    }
                }
                i++;
            }


            //ThingsFromDataStreams.Add("CrowdHeatmap", 650);
            //datastreams.Add("CrowdHeatmap");

            _logger.LogError("COPUpdater listening to " + datastreams.Count.ToString());
            //Setup SignalR
            connection = new HubConnectionBuilder()
            .WithUrl(copApiBasePathHub + "signalR/wearableupdate")
            .Build();
            connection.StartAsync().Wait();
            connection.Closed += async (berr) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };
   
            ml = (ManagedMqttClient)new MqttFactory().CreateManagedMqttClient();
            // Setup and start a managed MQTT client.
            var options = new ManagedMqttClientOptionsBuilder()
                .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                .WithClientOptions(new MqttClientOptionsBuilder()
                    .WithClientId(System.Guid.NewGuid().ToString())
                    .WithKeepAlivePeriod(TimeSpan.FromSeconds(120))
                    .WithCommunicationTimeout(TimeSpan.FromSeconds(60))
                   //.WithCredentials("mosquitto", "mosquitto")
                   //.WithTcpServer("monappdwp5.monica-cloud.eu")
                    .WithTcpServer("192.168.229.101")
                    .Build())
                .Build();
            ml = (ManagedMqttClient)new MqttFactory().CreateManagedMqttClient();

            if(mqttDebug)
            {
                MQTTnet.Diagnostics.MqttNetGlobalLogger.LogMessagePublished += (s, e) =>
                {
                    var trace = $">> [{e.TraceMessage.Timestamp:O}] [{e.TraceMessage.ThreadId}] [{e.TraceMessage.Source}] [{e.TraceMessage.Level}]: {e.TraceMessage.Message}";
                    if (e.TraceMessage.Exception != null)
                    {
                        trace += Environment.NewLine + e.TraceMessage.Exception.ToString();
                    }

                    CultureInfo ci = Thread.CurrentThread.CurrentCulture;
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("sv-SE");

                    string fileName = "MQTT" + DateTime.Now.ToShortDateString() + ".txt";

                    string logFolder = "." + Path.DirectorySeparatorChar;

                    FileStream w = File.Open(logFolder + fileName, System.IO.FileMode.Append, System.IO.FileAccess.Write, System.IO.FileShare.Write);
                    StreamWriter sw = new StreamWriter(w, System.Text.Encoding.Default);
                    sw.Write(trace + Environment.NewLine);
                    sw.Close();

                    Thread.CurrentThread.CurrentCulture = ci;
                };
            }
            foreach (string topic in datastreams)
            {
                ml.SubscribeAsync(new TopicFilterBuilder().WithTopic(topic).Build());
            }
            ml.ApplicationMessageReceivedHandler =
               new  MqttApplicationMessageReceivedHandlerDelegate(e =>
                {
                    if (ThingsFromDataStreams.ContainsKey(e.ApplicationMessage.Topic))
                    {
                        int thingid = ThingsFromDataStreams[e.ApplicationMessage.Topic];
                        string observation = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);

                        int? zoneid = null;
                        if (ZoneFromDataStreams.ContainsKey(e.ApplicationMessage.Topic))
                            zoneid = ZoneFromDataStreams[e.ApplicationMessage.Topic];

                        int? personid = null;
                        if (PeopleFromDataStreams.ContainsKey(e.ApplicationMessage.Topic))
                            personid = PeopleFromDataStreams[e.ApplicationMessage.Topic];

                        string safeObs = JsonConvert.ToString(observation);


                        if (thingtype == "incidentreporter")
                            ManageIncidentReporter(observation);
                        else if (thingtype == "button_press")
                            ManageButtonPress(observation);
                        else
                            ProcessNewSensorData(e.ApplicationMessage.Topic, thingid, observation, zoneid, personid);


                    }
                });

            ml.StartAsync(options).Wait();
        }



      


        /// <summary>
        /// Process new sensor data that arrives
        /// </summary>
        /// <param name="topic">The topic of the message</param>
        /// <param name="thingid">Set if a thing is connected to the datastream</param>
        /// <param name="observation">The actual meassage</param>
        /// <param name="zoneid">Set if a zone is connected to the datastream</param>
        /// <param name="personid">Set if a person is connected to the datastream</param>
        public void ProcessNewSensorData(string topic, int? thingid, string observation, int? zoneid, int? personid)
        {
            try
            {

                string message = "";
                string action = "";
                string dataStreamId = createDataStreamId(thingid.ToString(), ref observation, zoneid, personid, ref message, ref action, topic);
                
                string errorMessage = "Testar Loggning";
                long newId = 0;
                if (!useOnlySignalR) //true if the latest observation in the database should not be updated
                {
                    //Update the latest observation in the database.
                    if (thingtype == "PeopleHeatmap")
                        observation = "{\"phenomenonTime\":\""+ DateTime.Now.ToString("O") +"\", \"result\":" + observation + "}";
                    IO.Swagger.DatabaseInterface.DBObservation dbO = new IO.Swagger.DatabaseInterface.DBObservation();
                    if (!dbO.AddUpdateObservation(thingid, topic + dataStreamId, DateTime.Now, observation, ref errorMessage, ref newId))
                    {
                        _logger.LogError("COPUpdater update obs failed {errorMessage}", errorMessage);
                    }
                }

                if (action != "") //Should we emit an update on signalR?
                {
                    if (usesignalR) //True when we use signal directly.
                    {
                        try
                        {
                            connection.InvokeAsync(action, message).Wait();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError("signalR publish failed {ex.Message}", ex.Message);
                        }
                    }
                    else //Post the SignalR update using the COPs API
                    {
                        string payload = @"{
  ""action"": ""###action###"",
  ""message"": ###message###
}";
                        // System.Console.WriteLine("Action:" + action);
                        payload = payload.Replace("###action###", action);
                        payload = payload.Replace("###message###", JsonConvert.ToString(message));
                        HttpWebRequest client = (HttpWebRequest)WebRequest.Create(copApiBasePath + "HUBMessage");
                        try
                        {

                            client.Method = "POST";

                            client.ContentType = "application/json";
                            client.Accept = "application/json";
                            client.Headers["Authorization"] = copAuthToken;
                            client.Timeout = 2000;
                            UTF8Encoding encoding = new UTF8Encoding();
                            byte[] byte1 = encoding.GetBytes(payload);

                            // Set the content length of the string being posted.

                            client.ContentLength = byte1.GetLength(0);
                            Stream newStream = client.GetRequestStream();
                            newStream.Write(byte1, 0, byte1.Length);
                            newStream.Close();

                            HttpWebResponse response = (HttpWebResponse)client.GetResponse();
                            Stream responseStream = response.GetResponseStream();
                            StreamReader reader = new StreamReader(responseStream);
                            string jsonString = reader.ReadToEnd();
                            responseStream.Close();
                            response.Close();
                        }
                        catch (WebException exception)
                        {
                            string errorResponse = "";
                            //read the response.

                            if (exception.Response != null)
                            {
                                Stream errRes = exception.Response.GetResponseStream();
                                if (errRes != null)
                                {

                                    StreamReader sr = new StreamReader(errRes, true);

                                    errorResponse = sr.ReadToEnd();
                                    errRes.Close();

                                }
                            }
                            _logger.LogError("Invocation error {copApiBasePath} {exception.Message} {errorResponse}", copApiBasePath, exception.Message, errorResponse);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("general error when processing {ex.Message}", ex.Message);
            }

        }


        /// <summary>
        /// Manage an incident report from smart glasses
        /// </summary>
        /// <param name="observation">The recieved message</param>
        public void ManageIncidentReporter(string observation)
        {
            dynamic iPayload = JValue.Parse(observation);

            dynamic jmsgC = new JObject();
            /*
                         * {
                              "incidentid": 0,
                              "description": "string",
                              "type": "string",
                              "position": "string",
                              "prio": 0,
                              "status": "string",
                              "probability": 0,
                              "interventionplan": "string",
                              "incidenttime": "2018-12-07T13:20:12.639Z",
                              "wbid": "string",
                              "telephone": "string"
                            }
             */
            jmsgC.incidentid = 0;
            string reportType = iPayload.result.reportType;
            if (reportType == "video" || reportType == "picture" || reportType == "audio")
                jmsgC.description = reportType + " from " + iPayload.result.tagId;
            else
                jmsgC.description = iPayload.result.message;
            jmsgC.type = "FieldReport";
            double lat = iPayload.result.lat;
            double lon = iPayload.result.lon;
            jmsgC.position = "[[" + lat.ToString().Replace(',', '.') + "," + lon.ToString().Replace(',', '.') + "]]";
            jmsgC.prio = 0;
            jmsgC.status = "ONGOING";
            jmsgC.probability = 1.0;
            jmsgC.interventionplan = "";
            jmsgC.incidenttime = iPayload.phenomenonTime;
            string tmpDate = iPayload.phenomenonTime;
            DateTime tmpDate2 = DateTime.Parse(tmpDate);
            jmsgC.wbid = iPayload.result.tagId;
            jmsgC.telephone = "";
            string temp = iPayload.result.message;
            string[] split = temp.Split("/");
            if (reportType == "video" || reportType == "picture" || reportType == "audio")
                jmsgC.additionalMedia = split[split.GetUpperBound(0) - 1] + "/" + split[split.GetUpperBound(0)];
            else
                jmsgC.additionalMedia = "";
            jmsgC.mediaType = iPayload.result.reportType;


            string errorMessage = "";
            long newid = -1;
            //Update database with the new incident
            IO.Swagger.DatabaseInterface.DBIncident dbO = new IO.Swagger.DatabaseInterface.DBIncident();
            if (!dbO.AddIncident((string)jmsgC.description, (string)jmsgC.type, (string)jmsgC.position, 0, "ONGOING", 1.0, "", tmpDate2, (string)jmsgC.wbid, "", (string)jmsgC.additionalMedia, (string)jmsgC.mediaType,"", ref errorMessage, ref newid))
            {

                _logger.LogError("COPUpdater update incident failed {errorMessage}", errorMessage);
            }

            //Create and send signalR message
            dynamic message2 = new JObject();
            message2.type = "newincident";
            message2.incidentid = newid;
            message2.status = "ONGOING";
            message2.incidenttype = jmsgC.type;
            message2.prio = 1.0;
            message2.timestamp = DateTime.Now;
            message2.mediatype = jmsgC.mediaType;
            message2.additionalmedia = jmsgC.additionalMedia;
            message2.wbid = jmsgC.wbid;
            string strMessage = message2.ToString();
            connection.InvokeAsync("Incidents", strMessage).Wait();

        }

        /// <summary>
        /// Mange when a button press is detected on a tracker
        /// </summary>
        /// <param name="observation"></param>
        public void ManageButtonPress(string observation)
        {
            //Should be properly fixed later
            dynamic tmpPayload = JValue.Parse(observation);
            string tmp = tmpPayload.result;
            dynamic iPayload = JValue.Parse(tmp);
            dynamic jmsgC = new JObject();

            jmsgC.incidentid = 0;
            jmsgC.description = "Tracker button pressed";
            jmsgC.type = "ButtonPressed";
            double lat = iPayload.result.last_known_lat;
            double lon = iPayload.result.last_known_lon;
            jmsgC.position = "[[" + lat.ToString().Replace(',', '.') + "," + lon.ToString().Replace(',', '.') + "]]";
            jmsgC.prio = 0;
            jmsgC.status = "ONGOING";
            jmsgC.probability = 1.0;
            jmsgC.interventionplan = "";
            jmsgC.incidenttime = iPayload.phenomenonTime;
            string tmpDate = iPayload.phenomenonTime;
            DateTime tmpDate2 = DateTime.Parse(tmpDate);
            jmsgC.wbid = iPayload.result.tagId;
            jmsgC.telephone = "";
            jmsgC.additionalMedia = "";
            jmsgC.mediaType = "";


            string errorMessage = "";
            long newid = -1;
            //Update database with the new incident
            IO.Swagger.DatabaseInterface.DBIncident dbO = new IO.Swagger.DatabaseInterface.DBIncident();
            if (!dbO.AddIncident((string)jmsgC.description, (string)jmsgC.type, (string)jmsgC.position, 0, "ONGOING", 1.0, "", tmpDate2, (string)jmsgC.wbid, "", (string)jmsgC.additionalMedia, (string)jmsgC.mediaType, "",ref errorMessage, ref newid))
            {

                _logger.LogError("COPUpdater update incident failed {errorMessage}", errorMessage);
            }

            //Create and send signalR message
            dynamic message2 = new JObject();
            message2.type = "newincident";
            message2.incidentid = newid;
            message2.status = "ONGOING";
            message2.incidenttype = jmsgC.type;
            message2.prio = 1.0;
            message2.timestamp = DateTime.Now;
            message2.mediatype = jmsgC.mediaType;
            message2.additionalmedia = jmsgC.additionalMedia;
            message2.wbid = jmsgC.wbid;
            string strMessage = message2.ToString();

            connection.InvokeAsync("Incidents", strMessage).Wait();

        }

        /// <summary>
        /// Creates the complete data stream id for the event
        /// Also maps  an action and an message to be sent on signalR
        /// </summary>
        /// <param name="thingid"></param>
        /// <param name="observation"></param>
        /// <param name="zoneid"></param>
        /// <param name="personid"></param>
        /// <param name="message"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public string createDataStreamId(string thingid, ref string observation, int? zoneid, int? personid, ref string message, ref string action, string topic)
        {
            string retval = "";
            action = "";
            message = "";
            switch (thingtype)
            {
                case "Soundmeter":
                    dynamic json = JValue.Parse(observation);
                    string type = json.result.valueType;
                    if (type == null || type == "")
                        type = json.result.type;
                    if (type == "no_event" || type=="gunshot" || type=="scream")
                    {
                        retval = ":" + type + ":";
                        action = "";
                    }
                    else
                    { 
                        retval = ":" + type + ":";
                        action = "SoundsensorUpdate";

                        dynamic jmsg = new JObject();
                        jmsg.type = "dbupdate";
                        jmsg.thingid = int.Parse(thingid);
                        jmsg.valueType = type;

                        jmsg.timestamp = DateTime.Now;
                        jmsg.result = json.result;
                        string strMessage = jmsg.ToString();
                        message = strMessage;
                    }
                    break;
                case "SoundmeterAggregate":
                    dynamic jsonSA = JValue.Parse(observation);
                    //string typeSA = jsonSA.result.valueType;
                    retval = "::";
                    action = "AggregateUpdate";

                    dynamic jmsgSA = new JObject();
                    jmsgSA.type = "SoundmeterAggregate";
                    jmsgSA.thingid = int.Parse(thingid);
                    jmsgSA.valueType = "n/a";

                    jmsgSA.timestamp = DateTime.Now;
                    jmsgSA.result = jsonSA.result;
                    string strMessageSA = jmsgSA.ToString();
                    message = strMessageSA;
                    break; 
                case "SoundHeatmap":
                    dynamic jsonshm = JValue.Parse(observation);
                    string typeshm = jsonshm.result.unit;
                    string translatedType = "";
                    switch (typeshm)
                    {
                        case "SPL dB Cfull":
                            translatedType = "dbCheatmapfull";
                            break;
                        case "SPL dB Afull":
                            translatedType = "dbAheatmapfull";
                            break;
                        case "SPL dB Zfull":
                            translatedType = "dbZheatmapfull";
                            break;
                        case "SPL dB Z":
                            translatedType = "dbZheatmap";
                            break;
                        case "SPL dB A":
                            translatedType = "dbAheatmap";
                            break;
                        case "SPL dB C":
                            translatedType = "dbCheatmap";
                            break;
                        default:
                            translatedType = typeshm;
                            break;
                    }
                    retval = ":" + translatedType + ":";
                    //Commented away due to overloading of signalr.
                    //action = "SoundheatmapUpdate"; 
                    action = "";
                    dynamic jmsgshm = new JObject();
                    jmsgshm.type = "dbupdate";
                    jmsgshm.thingid = int.Parse(thingid);
                    jmsgshm.valueType = translatedType;

                    jmsgshm.timestamp = DateTime.Now;
                    jmsgshm.result = jsonshm;
                    string strMessageshm = jmsgshm.ToString();
                    message = strMessageshm;
                    break;
                case "PeopleHeatmap":
                    dynamic jsonphm = JValue.Parse(observation);
                    string typephm = "noPeople";
                   
                    retval = "::";
                    //Commented away due to overloading of signalr.
                    //action = "SoundheatmapUpdate"; 
                    action = "";
                    dynamic jmsgphm = new JObject();
                    jmsgphm.type = "dbupdate";
                    jmsgphm.thingid = int.Parse(thingid);
                    jmsgphm.valueType = "";

                    jmsgphm.timestamp = DateTime.Now;
                    jmsgphm.result = jsonphm;
                    string strMessagephm = jmsgphm.ToString();
                    message = strMessagephm;
                    break;
                case "Temperature":
                    if(aggregate)
                    {
                        dynamic jsonTmp = JValue.Parse(observation);
                        string typeTmp = "TemperatureAggregate";
                        retval = ":AGGREGATE:";
                        action = "TemperatureUpdate";

                        dynamic jmsgTmp = new JObject();
                        jmsgTmp.type = "TempratureAggregateUpdate";
                    //    jmsgTmp.thingid = int.Parse(thingid);
                        jmsgTmp.valueType = typeTmp;

                        jmsgTmp.timestamp = DateTime.Now;
                        jmsgTmp.result = BuildAggregates(topic);
                        jmsgTmp.result.latest = jsonTmp.result;
                        string strMessageTmp = jmsgTmp.ToString();
                        message = strMessageTmp;
                        observation = message;
                    }
                    else
                    { 
                        dynamic jsonTmp = JValue.Parse(observation);
                        string typeTmp = "Temperature";
                        retval = ":" + ":";
                        action = "TemperatureUpdate";

                        dynamic jmsgTmp = new JObject();
                        jmsgTmp.type = "TemperatureUpdate";
                        jmsgTmp.thingid = int.Parse(thingid);
                        jmsgTmp.valueType = typeTmp;

                        jmsgTmp.timestamp = DateTime.Now;
                        jmsgTmp.result = jsonTmp.result;
                        string strMessageTmp = jmsgTmp.ToString();
                        message = strMessageTmp;
                    }
                    break;
                case "Humidity":
                    if (aggregate)
                    {
                        dynamic jsonTmpH = JValue.Parse(observation);
                        string typeTmpH = "Humidity";
                        retval = ":AGGREGATE:";
                        action = "HumidityUpdate";

                        dynamic jmsgTmpH = new JObject();
                        jmsgTmpH.type = "HumidityAggregateUpdate";
                      //  jmsgTmpH.thingid = int.Parse(thingid);
                        jmsgTmpH.valueType = typeTmpH;

                        jmsgTmpH.timestamp = DateTime.Now;
                        jmsgTmpH.result = BuildAggregates(topic);
                        jmsgTmpH.result.latest = jsonTmpH.result;
                        string strMessageTmpH = jmsgTmpH.ToString();
                        message = strMessageTmpH;
                        observation = message;
                    }
                    else
                    {
                        dynamic jsonTmpH = JValue.Parse(observation);
                        string typeTmpH = "Humidity";
                        retval = ":" + ":";
                        action = "HumidityUpdate";

                        dynamic jmsgTmpH = new JObject();
                        jmsgTmpH.type = "HumidityUpdate";
                        jmsgTmpH.thingid = int.Parse(thingid);
                        jmsgTmpH.valueType = typeTmpH;

                        jmsgTmpH.timestamp = DateTime.Now;
                        jmsgTmpH.result = jsonTmpH.result;
                        string strMessageTmpH = jmsgTmpH.ToString();
                        message = strMessageTmpH;
                    }
                    break;
                case "Windspeed":
                    if (aggregate)
                    {
                        dynamic jsonWS = JValue.Parse(observation);
                        string typeWS = "Windspeed";
                        retval = ":AGGREGATE:";
                        action = "WindUpdate";

                        dynamic jmsgWS = new JObject();
                        jmsgWS.type = "WindspeedAggregateUpdate";
                       // jmsgWS.thingid = int.Parse(thingid);
                        jmsgWS.valueType = typeWS;

                        jmsgWS.timestamp = DateTime.Now;
                        jmsgWS.result = BuildAggregates(topic);
                        jmsgWS.result.latest = jsonWS.result;
                        string strMessageWS = jmsgWS.ToString();
                        message = strMessageWS;
                        observation = message;

                    }
                    else
                    {
                        dynamic jsonWS = JValue.Parse(observation);
                        string typeWS = "Windspeed";
                        retval = ":" + ":";
                        action = "WindUpdate";

                        dynamic jmsgWS = new JObject();
                        jmsgWS.type = "WindspeedUpdate";
                        jmsgWS.thingid = int.Parse(thingid);
                        jmsgWS.valueType = typeWS;

                        jmsgWS.timestamp = DateTime.Now;
                        jmsgWS.result = jsonWS.result;
                        string strMessageWS = jmsgWS.ToString();
                        message = strMessageWS;
                    }
                    break;
                case "Camera":
                    dynamic jsonC = JValue.Parse(observation);
                    string type_module = jsonC.result.type_module;
                    string[] cameraIds = jsonC.result.camera_ids.ToObject<string[]>();

                    string camerastr = string.Concat(cameraIds);

                    retval = ":" + camerastr + ":";
                    dynamic jmsgC = new JObject();

                    jmsgC.type = "peoplecountupdate";

                    if (zoneid != null)
                    {
                        jmsgC.zoneid = zoneid;
                        action = "ZoneUpdate";
                    }
                    jmsgC.peoplecount = jsonC.result.density_count;
                    if (jsonC.result.density_count == null)
                        jmsgC.peoplecount = jsonC.result.count;
                    jmsgC.timestamp = DateTime.Now;

                    message = jmsgC.ToString();

                    break;
                case "Aggregate":
                    dynamic jsonA = JValue.Parse(observation);

                    //string type_module2 = jsonA.result.type_module;
                    //string[] cameraIds2 = jsonA.result.camera_ids.ToObject<string[]>();

                    //string camerastr2 = string.Concat(cameraIds2);
                    string camerastr2 = jsonA.result.cameraid;

                    retval = ":" + camerastr2 + ":";

                    //o	{“type”:”peoplecountupdate”,”zoneid”:3, "peoplecount": 200, "timestamp": "2018-06-30T10:06:59"}
                    dynamic jmsgA = new JObject();
                    action = "PeopleGateCounting";
                    jmsgA.type = "PeopleGateCounting";
                    jmsgA.thingid = int.Parse(thingid);
                    //jmsgA.peoplecount = jsonA.result.count;
                    //jmsgA.name = jsonA.result.name;
                    jmsgA.result = jsonA.result;
                    jmsgA.timestamp = DateTime.Now;
                    jmsgA.phenomenonTime = DateTime.Now;
                    jmsgA.resultTime = DateTime.Now;
                    message = jmsgA.ToString();

                    break;
                case "wearables UWB":
                case "Wearable Smartphone":
                    dynamic jsonW = JValue.Parse(observation);
                    string tagId = jsonW.result.tagId;
                    retval = ":" + tagId + ":";
                    dynamic jmsgW = new JObject();
                    //o	{“type”:”peoplecountupdate”,”zoneid”:3, "peoplecount": 200, "timestamp": "2018-06-30T10:06:59"}
                    jmsgW.type = "posupdate";

                    if (personid != null)
                    {
                        jmsgW.peopleid = personid;
                        action = "peoplewithwearablesUpdate";
                    }
                    jmsgW.lat = jsonW.result.lat;
                    jmsgW.lon = jsonW.result.lon;
                    jmsgW.timestamp = DateTime.Now;

                    message = jmsgW.ToString();

                    break;
                default:
                    message = "";
                    action = "";
                    break;
            }
            return retval;

        }

        public JObject BuildAggregates(string topic)
        {

            double latest = 0.0;
            int no5 = 0;
            double sum5 = 0.0;
            double max5 = 0.0;
            double min5 = double.MaxValue;
            double avg5 = 0.0;
            int no10 = 0;
            double sum10 = 0.0;
            double max10 = 0.0;
            double min10 = double.MaxValue; 
            double avg10 = 0.0;

            foreach(var item in ThingsFromDataStreams.Keys)
            { 
                dynamic fiveMinutes = LastObservationUntil(item.ToString(), DateTime.UtcNow.AddMinutes(-5).ToString("O"));

                foreach (dynamic val in fiveMinutes.value)
                {
                    double result = val.result;
                    if (min5 > result)
                        min5 = result;
                    if (max5 < result)
                        max5 = result;
                    sum5 += result;
                    no5++;
                               
                }
            }

            foreach (var item in ThingsFromDataStreams.Keys)
            {
                dynamic tenMinutes = LastObservationUntil(item.ToString(), DateTime.UtcNow.AddMinutes(-10).ToString("O"));

                foreach (dynamic val in tenMinutes.value)
                {
                    double result = val.result;
                    if (min10 > result)
                        min10 = result;
                    if (max10 < result)
                        max10 = result;
                    sum10 += result;
                    if (no10 == 0)
                        latest = result;
                    no10++;

                }
            }



            dynamic json = new JObject();
           // json.latest= latest;
            json.max5 = max5;
            json.min5 = min5;
            json.avg5 = sum5/no5;
            json.max10 = max10;
            json.min10 = min10;
            json.avg10 = sum10/no10;
            return json;
        }


        public  dynamic LastObservationUntil(string datastream, string time)
        {
            string  res;
            dynamic retVal = null; 
            string url = settings.gostServer + datastream.Replace(settings.CameraPrefix, "") + "?$filter=phenomenonTime%20gt%20'"+time+"'";
            HttpWebRequest client = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                client.Method = "GET";
                client.ContentType = "application/json";
                client.Accept = "application/json";

                client.Timeout = 2000;

                HttpWebResponse response = (HttpWebResponse)client.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                res = reader.ReadToEnd();
                responseStream.Close();
                response.Close();
                if (res != "")
                {
                    retVal = JValue.Parse(res);
                

                }


            }
            catch (WebException exception)
            {
                string errorResponse = "";
                //read the response.

                if (exception.Response != null)
                {
                    Stream errRes = exception.Response.GetResponseStream();
                    if (errRes != null)
                    {

                        StreamReader sr = new StreamReader(errRes, true);

                        errorResponse = sr.ReadToEnd();
                        errRes.Close();
                       
                    }
                }
                System.Console.WriteLine("Invokation error" + url + "" + exception.Message + " " + errorResponse);

            }
            catch (Exception exception)
            {

                System.Console.WriteLine("Result parse error(Maybe)" + url + "" + exception.Message);
               
            }
            return retVal;
        }


    }
}

