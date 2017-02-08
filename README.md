# Remote Sleep

Send your PC power commands...Remotely!

Supports the following states:

* Sleep
* Shutdown
* Hibernate

Paired with an Android app [here](https://github.com/Sephizor/RemoteSleepAndroid)

## Installation
Both methods require Visual Studio to be installed
#### Method 1 (preferred)
* Open solution with Visual Studio
* Switch build configuration to `Release`
* Open Visual Studio Developer Command Prompt as Administrator at the binary location
* Navigate to the bin\Release
* Run `installutil.exe RemoteSleepService.exe`

#### Method 2
* Download the prebuilt binary from [here](https://github.com/Sephizor/RemoteSleepService/releases/download/1.0/RemoteSleepService.exe)
* Create a Program Files folder called `RemoteSleepService`
* Open a Visual Studio Developer Command Prompt in that folder
* Run `installutil.exe RemoteSleepService.exe`

### Uninstallation
* Open a Visual Studio Developer Command Prompt as Administrator at the binary location
* Run `installutil.exe /u RemoteSleepService.exe`
