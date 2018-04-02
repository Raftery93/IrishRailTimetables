# IrishRailTimetables
This is a UWP which is part of my module "Mobile Applications Development". It uses an API provided by IrishRail. It shows details about certain trains times in Ireland.

## Getting Started

To deploy this UWP, you will need to clone or download this repository. Once downloaded, you will need to run the .sln file.

### Prerequisites

Software you need to run this UWP:

```
Visual Studio 2017
Windows 10
```

### Installing

Breakdown of installing this application:

Clone/Download

```
Click the link to clone app -> download as zip -> unzip folder
```

Running application

```
Open Visual Studio 2017 -> File -> Open -> Folder -> Select IrishRailTimetables
```

## Running the Program

### Launch application

When the app is closed down, and re-opened again, it remembers what pivot page you were on when you closed it.

First Pivot page contains a dropdown box which contains all train stations. click the dropdown box and select a station to view train details.

![alt text](https://i.imgur.com/TBmRgaD.png)


Second Pivot page gets your current location and display train details of the closest station to your location. To do this, click on current location*.

![alt text](https://i.imgur.com/5Ka3vLU.png)


Third Pivot page opens a map of all stations around Ireland. There is also a text box which saves whatever text you type, for when you open the app again. To test this, type into the textbox provided and then close/reopen the app.

![alt text](https://i.imgur.com/f3z8vIQ.png)


Fourth Pivot page displays a list of all the stations and there distance in kilometers from your current location. Click on the Get Distances button*.

![alt text](https://i.imgur.com/1YjfmhA.png)


*These functions use geo location so you must allow the app to access your current location.


## Windows Store

Add additional notes about how to deploy this on a live system


## Authors

* **Conor Raftery** - *Only Contributor* - [Raftery93](https://github.com/Raftery93)
