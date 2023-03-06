# PropertySearch

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
  <li>Pass your own connection string to <code>appsettings.json</code> file in section <code>ConnectionStrings</code> to a <code>LocalDatabaseConnection</code>
  <li>Use <code>Package Manager Console</code> and run <code>Update-Database</code> command to migrate the last migration to a database model</li>
  <li>Run the app (normally, it will run in <code>https://localhost:7230</code> url) </li>
  </ol>
  
``` cmd
dotnet run
```
