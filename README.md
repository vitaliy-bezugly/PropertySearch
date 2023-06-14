# PropertySearch
The Home Rental Offer Web Application is a useful tool for anyone looking to create and manage their home rental offers. With its easy-to-use features and intuitive user interface, users can create and manage their home rental offers in a timely and organized manner, as well as find potential rentals and connect with other users.

<h2>🐳 Functionality </h2>
<ul>
  <li>User Registration and Login: Users can create a new account or log in to an existing account using their email address and password. Once logged in, users are redirected to the dashboard where they can view their own offers and manage their account.</li>
  <li>Add New Home Rental Offer: Once logged in, users can create a new home rental offer that includes details such as the location, price, description, and images. The offer will be displayed on the offers page where other users can view it.</li>
  <li>Home Rental Offer Management: Users who have added an offer can edit or update it at any time. This feature helps offer owners keep their offer up to date and relevant to potential renters.</li>
  <li>View Other Users' Offers and Accounts: Users can view the offers and accounts of other users who have created an account on the application. This feature allows users to find potential rentals and to connect with other users who may be interested in renting their home.</li>
  <li>User Experience: The application has a clean and intuitive user interface that makes it easy for users to manage their home rental offers. The application's features are designed to be easy to use, and users can easily navigate through the different sections of the application.</li>
</ul>

## Libraries, tools, technologies and frameworks with which the application was created

<ul>
    <li><code>ASP .NET Core MVC</code> - with razor pages</li>
    <li><code>ASP .NET Core Identity</code> - to add login authorization and authentication to application.</li>
    <li><code>Entity Framework Core</code> - as ORM</li>
    <li><code>MSSQL</code> - as data storage</li>
    <li><code>XUnit</code> - as framework for testing</li>
    <li><code>Docker</code> - to help build the application and port it to other devices</li>
</ul>

## Clone the repository:

```
git clone https://github.com/vitaliy-bezugly/PropertySearch.git
```

<h2>🛠️ Local running</h2>
To run the application on your local machine, you have two options:

<ol>
<li>
  Using .NET Core: Make sure you have the .NET Core SDK installed. Open a terminal or command prompt and navigate to the root folder of the project. Run the following command:
</li>

```
dotnet run --project src/PropertySearch.Api
```

Ensure that you have configured environment variables.

<li>
  Using <code>Docker</code>: Make sure you have Docker installed on your machine.
</li>

Firstly, you need to create an .env file in the root folder where you will describe the configuration of the projects. You must add the following keys:
- <code>MSSQL_PASSWORD</code> - password for the mssql database
- <code>MSSQL_PORT</code> - mssql database port (optionally it should be 1433)
- <code>PS_DOCKER_PORT</code> - port of the "Property Search" application in the docker (optionally 80). If you want to install a different one, make sure you add the exact same port as the property lookup service environment variable in <code>docker-compose.yml</code>
- <code>PS_HOST_PORT</code> - port of the real estate search application on the host computer
- <code>PS_SEND_GRID_KEY</code> - your platform private key <a src="https://sendgrid.com/">Send Grid</a> to send email
- <code>PS_IP_INFO_TOKEN</code> - your personal token <a src="https://ipinfo.io/">IP Info</a>

Then, open a terminal or command prompt and navigate to the root folder of the project. Run the following command:

```
docker-compose up -d
```

This command will build and run the Docker containers for the application, along with all their dependencies. The API will be accessible at http://localhost:8080 on your host computer.

Note: With Docker, the necessary database migrations will be applied automatically.

## Environment variables 

<ul>
  <li>DB_CONN - connection string of <code>MSSQL</code> database</li>
  <li>SendGridKey - your platform private key <a src="https://sendgrid.com/">Send Grid</a> to send email</li>
  <li>IpInfoToken - your personal token <a src="https://ipinfo.io/">IP Info</a></li>
</ul>

## Authors

- [@Vitaliy Bezugly](https://github.com/vitaliy-bezugly)
- [@Brannmarkt](https://github.com/Brannmarkt)
- [@Alzgaymer](https://github.com/Alzgaymer)
- [@DmytroHerasymchuk](https://github.com/DmytroHerasymchuk)

## Images
![image](https://user-images.githubusercontent.com/87979065/234870871-d2d8f643-21d9-4da6-bc68-6c1f88f5c51d.png)
![image](https://user-images.githubusercontent.com/87979065/234871712-5f10daa8-7ecb-46e1-a2e3-b5390910f80d.png)
![image](https://user-images.githubusercontent.com/87979065/234871734-e9218444-b945-4429-8ff4-e36e5a01a848.png)
![image](https://user-images.githubusercontent.com/87979065/234871887-0859ee01-ffc4-45ba-af66-6c82322ba1b7.png)


