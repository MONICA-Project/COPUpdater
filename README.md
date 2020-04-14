# Monica COPUpdater
<!-- Short description of the project. -->

COPUpdater
CopUpdater is responsible for listening to state changes from datatstrems and triggering push updates to the COP clients using the signalR protocol. An overview of the COP architecture can be seen [here](https://github.com/MONICA-Project/COP.API).


The COPUpdater is connected to the following components in MONICA:
* IoT DB [GOST](https://github.com/gost/server) Provides the OGC Sensorthings API database
* [COP.DB](https://github.com/MONICA-Project/COP.DB) - Instance data for the event.
* MQTT Broker - Provides updates on the datastreams.
* EventHub provides push functionality to apps and COP-UI. This component is in fact integrated in the COP.API module but as a separate component [COP.API](https://github.com/MONICA-Project/COP.API).

The COPUpdater is based on the following technologies:
*	MQTT interface for receiving messages for updating COP status
*	SignalR for pushing updates to clients

The COPUpdater provides the following main functionalities:
* Filtering of messages
* Aggregation over time  
* Payload optimizations, i.e. very large payloads are not forwarded through SignalR. Instead it will only send a message indicating that a updated value can be retrieved.



The COPUpdater is implemented in ASP.NET Core 2.1 and lack any api of itself.

<!-- A teaser figure may be added here. It is best to keep the figure small (<500KB) and in the same repo -->

## Getting Started
The COP.API is developed in Visual Studio using Dotnet Core 2.1.

The easiest way to build it is to clone the repository using Visual Studio 2017 or higher and then build the software or to use the DotNet Core 2.1 SDK.

There are a number of ready made Docker Compose Packages demonstration environments that include the COPUpdater and all dependencies and provides an easy way of testing the COPUpdater 
## Docker Compose with complete demonstration environments including the COP.API
* [Demonstration of staff management with LoRa based locators]( https://github.com/MONICA-Project/staff-management-demo)
* [Demonstration of crowd management using smart wristbands](https://github.com/MONICA-Project/DockerGlobalWristbandSimulation)
* [Demonstration of Sound Monitoring an event using Sound Level Meters](https://github.com/MONICA-Project/DockerSoundDemo)
* [Environment Sensors for managing weather related incidents Demo](https://github.com/MONICA-Project/DockerEnvironmentSensorDemo) Also provides an insight in how aggrgate values are managed.

## Deployment
For deployment the COPUpdater relies on [COP.DB](https://github.com/MONICA-Project/COP.DB) for internal use as well as a connection to IoT DB [GOST](https://github.com/gost/server). There also needs to be an MQTT broker available. If one uses one of the demonstration docker compose environments they will be automatically created.

### Docker
#### Environment Variables that need to be set
| Variable | Description | Example | 
| --------------- | --------------- | --------------- |
| CONNECTION_STR | COP.DB connection string | Host=copdb; Database=monica_wt2019; Username=postgres; Password=postgres;Port=5432 | 
| GOST_PREFIX | Should match the MQTT prefix useb by the GOST db | GOST |
| MONICA_THING_TYPE | Which MONICA thing type should the COPUpdater manage  | Soundmeter |
| COP_AUTH_TOKEN | 6ffdcacb-c485-499c-bce9-23f76d06aa36 | The fixed access token |
| COP_API_BASE_PATH_HUB | The path to the SignalR event hub | http://copapi/ | 
| COP_API_BASE_PATH | The path to the COP.API | http://copapi/ | 
 | gostServer | GOST Server address | http://gost:8080/v1.0/ |
  | mqttServer | MQTT Broker address | mosquitto |
| signalR | true if SignalR is to be used | true|
|OGCSearch| False uses legacy model of finding datastreams |true|
| signalR | true if SignalR is to be used | true|
|aggregate| If true the messages will be aggregated before sending SignalR message, false is the normal |false |
| signalR | true if SignalR is to be used | true|
|MqttGostPrefix| Should match the MQTT prefix useb by the GOST db |GOST |




To run the latest version of COP.API:
```bash
docker run  -e CONNECTION_STR=Host=copdb;Database=monica_wt2019;Username=postgres;Password=postgres;Port=5432 -e GOST_PREFIX=GOST -e COP_AUTH_TOKEN=6ffdcacb-c485-499c-bce9-23f76d06aa36 -e MONICA_THING_TYPE=Soundmeter -e COP_API_BASE_PATH_HUB=http://copapi/ -e COP_API_BASE_PATH=http://copapi/ -e gostServer=http://gost:8080/v1.0/  -e mqttServer=http://mosquitto  -e signalR=true  -e aggregate=false -e MqttGostPrefix=GOST -e OGCSearch=true monicaproject/copupdater:0.7
```
NB! The Prerequisites must exist and be started when starting the COPUpdater 

## Development
To start development it is enough to clone the repository and then build it either using Visual Studio or Dotnet Core SDK to build and run the API.
It is recomended to use on of the complete demonstration environments to have some testing data.

The code for the COP.API is based on ASP.NET Core framework and for COP.DB access Microsoft.EntityFrameworkCore is used.


### Prerequisite
* GOST (IoT DB). Installation instructions are available [here](https://www.gostserver.xyz/)
* COP.DB Installation instructions [here](https://github.com/MONICA-Project/COP.DB)
* Dotnet Core SDK 2.1 available [here](https://dotnet.microsoft.com/download/dotnet-core/2.1)
    - Or use Visual Studio 2017 or higher
* MQTT Broker (For instance [Eclipse Mosquitto](https://mosquitto.org/))



### Build

```bash
dotnet build
```

### Endpoints exposed
None
## 
## Contributing
Contributions are welcome. 

Please fork, make your changes, and submit a pull request. For major changes, please open an issue first and discuss it with the other authors.

## Affiliation
![MONICA](https://github.com/MONICA-Project/template/raw/master/monica.png)  
This work is supported by the European Commission through the [MONICA H2020 PROJECT](https://www.monica-project.eu) under grant agreement No 732350.
