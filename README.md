# Pokemon Translator API

## Table of Contents
1. [Overview](#overview)
2. [Dependencies](#dependencies)
3. [Getting Started](#getting-started)
4. [Build the application using Docker (Optional)](#build-the-application-using-docker-optional)
5. [Test API using Postman](#test-api-using-postman)
6. [Miscellaneous](#miscellaneous)
7. [License](#license)
8. [Roadmap](#roadmap)

## Overview
The Pokemon Translator API is a C# project designed to fetch Pokemon information from [PokeApi](https://pokeapi.co/) and to apply translations to their descriptions using the [Fun Translations API](https://api.funtranslations.com/). Specifically, it translates descriptions into Shakespearean or Yoda-like speech patterns, depending on the context. A caching mechanism uses an in-memory storage: when a request is done, the API checks the cache for the Pokemon data. If data is not available, it fetches it from PokeAPI and stores it in the cache for future requests. This project is intended as a development tool and no official releases will be delivered.

## Dependencies
1. OS: Developed and tested on Windows11. Tested on Ubuntu 22.04
2. Visual Studio 2022 
3. Frameworks: .NET 8 and .ASP.NET Core.
4. Tools: Git
4. Docker Desktop 4.36.0 (optional)
5. Any tool for API test (eg. Postman, optional)

## Getting Started
### 1. Clone the repository 

Clone the repository with a git command:

```sh
git clone https://github.com/Bert89/pokedex.git
```

### 2. Build and Run the solution

Build/Run the solution using VS2022 or open a terminal and move to the root of the solution, and run the command:
```sh
dotnet build
cd Pokedex
dotnet run
```

### 3. Try to call one of the available API:

#### 1. Endpoint 1 (GET): http://localhost:5000/pokemon/pokemon-name

To fetch Pokemon information from its name. Replace pokemon-name with the name of your Pokemon. If it doesn't exists, an error will be returned

#### 2. Endpoint 2 (GET): http://localhost:5000/pokemon/translated/pokemon-name

To fetch Pokemon information from its name. Replace pokemon-name with the name of your Pokemon. If it doesn't exists, an error will be returned. If Pokemon is a legendary one or its Habitat is "cave", then the description will be translated using **Yoda** translation. Otherwise, the **Shakespeare** translation will be used. In case of  error, the standard description will be reported. 

### 4. Run unit tests (Optional)
The solution provides some unit tests. Execute the following command to run all tests:
```sh
dotnet test
```


## Build the application using Docker (Optional)
Once installed Docker Desktop or Docker package in your Linux system, move to the root of the project and build the docker image using the following command:

```sh
docker build -t image-name .
```
This allows to docker engine to download all the required packages and frameworks. To run the image on port 8080, execute the following command:

```sh
docker run -p 8080:8080 image-name
```
Now is possible to try the API navigating the URL http://localhost:8080/pokemon/pokemon-name
More info about docker commands and parameters, are available to [Docker Doc](https://docs.docker.com/reference/cli/docker/) 

## Test API using Postman

It is possible to test the API using Postman (or similar test API tool):

In case of valid response using Endpoint 1, an object like this will be returned:

```sh
{
    "name": "zapdos",
    "description": "A legendary bird Pokemon that is said to appear from clouds while dropping enormous lightning bolts.",
    "habitat": "rare",
    "isLegendary": true,
    "error": null
}
```

Otherwise, the response will be:

```
{
    "message": "Pokemon not found",
    "details": "Response status code does not indicate success: 404 (Not Found)."
}
```

## Miscellaneous
1. All git history was exported and available in [gitlog.txt](https://github.com/Bert89/pokedex/gitlog.txt)
2. In order to change communication protocol or improve microservices architecture, Protobuf models are already provided in solution. To use them instead the standard C# models, it is necessary to build them using the ***protoc*** executable included in Utils folder. <br> **NOTE: The old models will be replaced with the new ones**. <br> Run the command from 'Pokedex' folder: 
```sh
..\Utils\protoc.exe --csharp_out=./Models --proto_path=./proto ./proto/*.proto
```
3. All cache mechanisms used in this example are in-memory


## License
This project is licensed under the [MIT License](https://github.com/Bert89/pokedex/blob/main/LICENSE.txt). See the LICENSE file for more details.


## Roadmap
Indentify and separate different projects into microservices and use scalable and performing communication protocols like gRPC to improve interoperability. 

For example, the microservices list could be divided as following:
1. Application to wrap PokeApi.Co library
2. Application to wrap translators
3. Application that expose required endpoints
4. Isolate the Caching service
5. Interation with some database and/or authorization machanism
6. Models integration
7. Logging
8. Build a CI/CD Pipeline for building, testing, and deploying. 

