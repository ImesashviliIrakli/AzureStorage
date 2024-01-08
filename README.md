# Azure Storage and Azure Functions Sample Project

## Overview

This repository contains a .NET project that demonstrates the integration of Azure Storage (Blob) and Azure Functions with various triggers. The purpose of this project is to provide developers with simple examples and a quick start guide for working with Azure Storage and Azure Functions in a .NET environment.

## Features

- **Azure Blob Storage Integration:** Examples showcasing basic operations with Azure Blob Storage, such as uploading, downloading, and deleting blobs.

- **Azure Functions Triggers:**
  - **Blob Trigger:** Demonstrates how to create an Azure Function that is triggered when a new blob is added or modified in a specified container.
  
  - **Timer Trigger:** Illustrates the usage of a timer-triggered Azure Function to perform periodic tasks.

  - **HTTP Trigger:** Shows how to create an HTTP-triggered Azure Function that can be invoked via HTTP requests.

## Examples

### Azure Blob Storage

- **BlobOperations.cs:**
  - Uploading a blob
  - Downloading a blob
  - Deleting a blob

### Azure Functions

- **BlobTriggerFunction.cs:**
  - Azure Function triggered on new blob creation or modification.

- **TimerTriggerFunction.cs:**
  - Azure Function triggered on a specified schedule.

- **HttpTriggerFunction.cs:**
  - Azure Function triggered by HTTP requests.

## Acknowledgments

Special thanks to the Microsoft Azure team for providing robust cloud services and tools.

Happy coding!
