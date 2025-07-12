
## Install AWS Templates

`dotnet new install Amazon.Lambda.Templates>`

<br>

## Set up directory structure for new project/solution

```
mkdir AwsLambda2
cd AwsLambda2
touch README.md
git init
```

<br>

## Create a .Net webapi function

```
mkdir src
cd src
dotnet new sln --name AwsLambda2
dotnet new webapi --name AwsLambda2Host --framework net8.0
dotnet new xunit --name AwsLambda2Host.Tests --framework net8.0
dotnet new xunit --name AwsLambda2Host.IntegrationTests --framework net8.0
dotnet sln add AwsLambda2Host
dotnet sln add AwsLambda2Host.Tests
dotnet sln add AwsLambda2Host.IntegrationTests
cd AwsLambda2Host.Tests
dotnet add reference ../AwsLambda2Host/AwsLambda2Host.csproj
cd ..
cd AwsLambda2Host.IntegrationTests
dotnet add reference ../AwsLambda2Host/AwsLambda2Host.csproj
```

<br>

## Customize the function

- Add the following nuget to the Host: Amazon.Lambda.AspNetCoreServer.Hosting
- In the Program.cs, after creating the "builder", add the following line...
```builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi)```
- Add a reference from the Test project to the Host and create a simple test.
- Add the following line to the PropertyGroup for the Host.cspror file.
- This allows you to deploy via VS to AWS (although my instructions are for the CLI)
    ```
    <PropertyGroup>
    ...
    <AWSProjectType>Lambda</AWSProjectType>
    </PropertyGroup>
    ```

<br>

In the Host project, create a file named 'aws-lambda-tools-defaults.json'.  You
will populate this file with the json (below).  This will be used for defaults
during the deploy.
```
{
  "Information": [
    "This file provides default values for the deployment wizard inside Visual Studio and the AWS Lambda commands added to the .NET Core CLI.",
    "To learn more about the Lambda commands with the .NET Core CLI execute the following command at the command line in the project root directory.",
    "dotnet lambda help",
    "All the command line options for the Lambda command can be specified in this file."
  ],
  "profile": "",
  "region": "us-east-2",
  "configuration": "Release",
  "function-architecture": "x86_64",
  "function-runtime": "dotnet8",
  "function-memory-size": 256,
  "function-timeout": 30,
  "function-handler": "AwsLambda2Host",
  "function-name": "stevedawslambda2",
  "environment-variables": "ASPNETCORE_ENVIRONMENT=Development;Name=Value",
  "function-url-enable": true,
  "function-role": "aws-lambda-basic"
}
```

<br>

## Provide permissions for the function that will be deployed

The Function needs an IAM role in order to the basics.  The following code
will create an IAM Role with a policy (which is then referenced in the json defaults)

```
aws iam create-role \
  --role-name aws-lambda-basic \
  --assume-role-policy-document \
'{
    "Version": "2012-10-17",
    "Statement": [
        { 
            "Effect": "Allow", 
            "Principal": {
                "Service": "lambda.amazonaws.com"
            }, 
            "Action": "sts:AssumeRole"
        }
    ]
}'
```

```
aws iam attach-role-policy --role-name aws-lambda-basic --policy-arn arn:aws:iam::aws:policy/service-role/AWSLambdaBasicExecutionRole
```

<br>

## Build, Test the function from the command line

```
cd <src directory>
dotnet clean
dotnet build
dotnet test --filter .Tests
dotnet test --filter .IntegrationTests

cd <host directory>
dotnet run
# it should start and provide you with a url that you can use to test
# Example: 
curl http://localhost:5158/weatherforecast
```

dotnet run

<br>

## Deploy

cd <host directory>
dotnet lambda deploy-function AwsLambda2Host

<br>

## After deployment, inspect details of the function
dotnet lambda get-function-config AwsLambda2Host
- note the url 

<br>

## Test
- Using the url from above...  
`curl <url>/weatherforecast`

<br>

## Note: Something like this should work, but I don't have the correct syntax
dotnet lambda invoke-function AwsLambda2Host/weatherforecast

<br>

## Clean it up
`dotnet lambda delete-function AwsLambda2Host`
