## AWS CLI
[Installing or updating to the latest version of the AWS CLI](https://docs.aws.amazon.com/cli/latest/userguide/getting-started-install.html)

***
<br>

## AWS Credentials
- Navigate to IAM: Log in to the AWS Management Console and go to the IAM service.
- Create a new user or select an existing one: If you don't have one, create a new IAM user. Otherwise, select the user you want to configure with the AWS CLI.
- Access Security Credentials: Go to the "Security Credentials" tab for the selected user.
- Create Access Keys: Under "Access keys", click on "Create access key".
- Choose CLI: Select "Command Line Interface (CLI)" as the use case for the access key.
- Record the Keys: Carefully record the generated Access Key ID and Secret Access Key. These keys are essential for authentication. Note: You will not be able to view the Secret Access Key again after this step.
- Configure the AWS CLI: Use the aws configure command in your terminal and enter the Access Key ID and Secret Access Key when prompted. You will also be prompted for a default region and output format.
- Using Profiles (Optional): For managing multiple AWS accounts, you can use profiles. Create profiles in the ~/.aws/config file with different access keys and configurations.

## Install AWS Templates

`dotnet new install Amazon.Lambda.Templates`

***
<br>

## Set up directory structure for new project/solution

```
mkdir AwsLambda2
cd AwsLambda2
touch README.md
git init
```

***
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
dotnet new gitignore
```

***
<br>

## Customize the function

- Add the following nuget to the Host: Amazon.Lambda.AspNetCoreServer.Hosting
- In the Program.cs, after creating the "builder", add the following line...
```builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi)```
- For the test projects, create a simple test.
- Add the following line to the PropertyGroup for the Host.csproj file.  This allows you to deploy via VS 
to AWS (although my instructions are for the CLI and is not required)
    ```
    <PropertyGroup>
    ...
    <AWSProjectType>Lambda</AWSProjectType>
    </PropertyGroup>
    ```

***
<br>

## Create the config for the Lambda function that will be created in AWS

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
  "function-name": "xx13-dev-test1",
  "environment-variables": "ASPNETCORE_ENVIRONMENT=Development;Name=Value",
  "function-url-enable": true,
  "function-role": "xx13-dev-lm-basic"
}
```

### Notes
- the naming of some of this stuff is not important or driven by the Terraform resource creation.
- the environment-variables are just an example.  Actually there is a little bit of an issue due
to Terraform.  Who is responsible for creating env variables, Terraform?  This app?  That goes for 
some of the other settings as well.  Terraform should be responsible for most of this, but I 
have not had time to figure out how to force that issue.

***
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

### Notes
- Again, Terraform would normally be responsible for creating the roles/policies and the lambda function in AWS.  But this was
created by me prior to creating the Terraform code.

***
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

***
<br>

## Deploy

cd <host directory>
dotnet lambda deploy-function AwsLambda2Host

***
<br>

## After deployment, inspect details of the function
dotnet lambda get-function-config AwsLambda2Host
- note the url 

***
<br>

## Test
- Using the url from above...
`curl <url>/weatherforecast`

***
<br>

## Clean it up
`dotnet lambda delete-function AwsLambda2Host`

## Github actions
- At the project root, create a .github subdirectory.
- In that directory, create a workflows subdirectory.
- In this directory you will add .yml files (i.e. github action files)

## Useful Github CLI Commands

- View the workflows for the repository.  The workflows are the "Github Actions".
```
gh workflow view
```

### Add access keys to Github as secrets for deployment to AWS
- Add your access key and secret as Environment variables
```
AWS_ACCESS_KEY_ID=$(aws configure get aws_access_key_id)
AWS_SECRET_ACCESS_KEY=$(aws configure get aws_secret_access_key)
```

- Add the secrets to the repository.   
  i.e. Settings > Security > Secrets and variables > Actions > Repository Secrets
```
gh secret set AWS_ACCESS_KEY_ID --body "$AWS_ACCESS_KEY_ID" --app actions
gh secret set AWS_SECRET_ACCESS_KEY --body "$AWS_SECRET_ACCESS_KEY" --app actions
```

### Remove access keys
```
gh secret delete AWS_ACCESS_KEY_ID --app actions
gh secret delete AWS_SECRET_ACCESS_KEY --app actions
```
