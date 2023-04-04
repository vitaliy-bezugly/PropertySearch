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
<h2>🛠️ Local running</h2>
<p>Follow the steps</p>
<ol>
<li>Make sure you have the tools to compile .Net 6 projects</li>
<li>Clone the repository</li>
  </ol>
  
```
git clone https://github.com/VitaliyMinaev/PropertySearch.git
```

<ol start="3">
  <li>Type in cmd <code>docker compose up -d</code>. This action initializes the docker container with all necessary dependencies.</li>
  <li>Use <code>Package Manager Console</code> and run <code>Update-Database</code> command to push the last migration to the database model</li>
  <li>Then run <code>property-search-initial-script.sql</code> on the newly created mssql database.</li>
  <li>Run the app (normally, it will run in <code>https://localhost:7230</code> url) </li>
  </ol>
  
``` cmd
dotnet run
```
