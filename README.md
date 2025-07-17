# Accounting for Dentists

Web app for dentists to track income and expenses and fulfil their tax obligations.

## Description

The web app allows dentists to record income and expenses and view detailed reports with BAS and income information for tax purposes.

## Getting Started

### Dependencies

* .NET 9.0

### Installing

* Build the application using `dotnet build`
* Configure the following environment variables:  
    `Authentication:Microsoft:ClientSecret` - Client secret for Microsoft OpenID SSO
    `Authentication:Microsoft:ClientId` - Client ID for Microsoft OpenID SSO
    `ConnectionStrings__AccountingForDentists` - Connection string for the AccountingForDentists database.

### Executing program

* Run the program by executing `dotnet accounting-for-dentists.dll`

### Docker
The application can be run in a docker container using the sample `docker-compose.yml` file.

* Modify `docker-compose.yml` with the connection settings for your environment.
* Launch the image by running `docker compose up -d`
* Access the website in a browser at `https://localhost:8080` (assuming default port) 

## Help

Raise an issue for help

## Authors

Sahil Jain

## Version History

* 1.0.0-alpha.0
    * First development version.  Allows entering income and viewing BAS.

## License

This project is licensed under the MIT License - see the LICENSE.md file for details

## Acknowledgments

Developed for my wife to help he with her taxes.