<img src="webapp/wwwroot/images/logo.png" width='300px' >

_This application was build as an university project where the main requirement was to create an application created using C# technologies (ASP.NET MVC & Web API)._

# Intro

Meetly is a simple chat applications for users. The goal for this project was to build a straightforward application that uses both server rendering for building a web application, simple API endpoints and database communication

**Main features:**
- Creating public, private rooms
- Real-time communication

**Technologies used:**

- Frontend: ASP-NET MVC, Tailwind
- Backend: .NET CORE WEB API
- Communication: HTTP, MQTT, SignalR
- Db: MongoDB
- Auth: Auth0

## Architecture

The project consists of two main applications:
- webapp (ASP.NET MVC)
- server (.NET CORE WEB API)

They both reference to an internal package called `data-bindings` that shares models between backend and frontend application

There are two ways of communication introduced between two applications:
- HTTP Request-response architecture for simple CRUD actions
- MQTT Event Driven architecture for chat communication

## Before you start
This application uses the following 3rd-party technologies:
- Auth0 for authentication
- HiveMQ for MQTT events

It is recommended to setup the free-tier applications in both 3rd parties to successfully run the project.

Each project contains its own README.md file that describes in details the configuration needed to be provided