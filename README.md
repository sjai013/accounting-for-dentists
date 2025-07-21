# Accounting for Dentists

Web app for dentists to track income and expenses and fulfil their tax obligations.

## Description

The web app allows dentists to record income and expenses and view detailed reports with BAS and income information for tax purposes.

## Getting Started

### Dependencies

* .NET 9.0

### Building Docker Image
* Build the docker image by running
```
 docker build . -t ghcr.io/sjai013/accounting-for-dentists:{VERSION} --platform linux/amd64
```
### Installing

* Build the application using `dotnet build`
* Configure the following environment variables:  
    `Authentication:Microsoft:ClientSecret` - Client secret for Microsoft OpenID SSO
    `Authentication:Microsoft:ClientId` - Client ID for Microsoft OpenID SSO.

Tenant data (database, uploaded files) are stored in the `tenants` directory, with a separate database for each user using the `Tenand ID (tid)` and `User Object ID (oid)` returned by the login provider.  Each user's database is password-protected using the `sub` claim provided by the login provider.

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

* 0.2.0
    * Form for entering net income (combination of sales and expenses) and viewing BAS.
    * Form for entering expenses
    * View income, sales, expenses
    * Upload attachments for income or expense
    * Highly secure multi-tenant SQLite database - database decryption requires token from login provider


## License

This project is licensed under the MIT License - see the LICENSE.md file for details

## Acknowledgments

Developed for my wife to help her with her taxation and reporting requirements.