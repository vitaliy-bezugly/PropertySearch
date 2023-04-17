[container-name] - propertysearch-mssql-1
[database-user] - SA
[database-password] - 'T3stdatabase'

1. Open cmd
2. Run: <code>docker exec -it [container-name]  "bash"</code>
3. Type: <code>/opt/mssql-tools/bin/sqlcmd -S localhost -I -U [database-user] -P [database-password]</code>
4. Then run sql script (line by line)

```
USE PropertySearch
GO

INSERT INTO [AspNetRoles] VALUES ('63258DBE-D709-41C8-90AF-7278CB987C1E', 'admin', 'ADMIN', 'da526518-db01-4f97-88eb-5a18b2e2a9fe'), ('9E4F35DB-192C-47FB-B7D4-A281B7989470', 'landlord', 'LANDLORD', '917e5175-d706-4017-94b8-662db82e62cf'), ('A84FBF0E-1B2A-4EA2-B703-A7E8D9F4A647', 'user', 'USER', 'd2af717f-ad22-4450-ba66-1deebe950707');
GO

INSERT INTO [AspNetUsers] VALUES ('1F9EBB06-0947-4D6F-F99C-08DB34EFB52C', '', 1, '2023-04-04 12:33:55.0476896', 'admin', 'ADMIN', 'adminemail@exapmle.com', 'ADMINEMAIL@EXAMPLE.COM', 1, 'AQAAAAEAACcQAAAAEFAe+gNY6lCyryO4anQUFi6cOEZk7fZingwSWtQqlzKGEX5omFWbMibK2liqa6fUiw==', 'PTPHI5XVGUOUNYIO7F3QT3DNZFUI6GEC', '37f724a4-84ce-4305-a3a0-c73e20b2d7e5', NULL, 0, 0, NULL, 1, 0);
GO

INSERT INTO [AspNetUserRoles] VALUES ('1F9EBB06-0947-4D6F-F99C-08DB34EFB52C', '63258DBE-D709-41C8-90AF-7278CB987C1E'), ('1F9EBB06-0947-4D6F-F99C-08DB34EFB52C', '9E4F35DB-192C-47FB-B7D4-A281B7989470'), ('1F9EBB06-0947-4D6F-F99C-08DB34EFB52C', 'A84FBF0E-1B2A-4EA2-B703-A7E8D9F4A647');
GO

INSERT INTO [Contact] VALUES ('1F9EBB06-0947-4D6F-F99C-08DB34EEB52C', 'Email address', 'adminemail@exapmle.com', '2023-04-04 12:33:55.0476896', '1F9EBB06-0947-4D6F-F99C-08DB34EFB52C'), ('1F9EBB06-0947-4D6F-F99C-08DB24EEB52C', 'Postcode', '47206', '2023-04-04 12:33:55.0476896', '1F9EBB06-0947-4D6F-F99C-08DB34EFB52C'), ('1F9EBB06-0947-4D6F-F99C-08DB34EEB57C', 'Phone number', '095-209-21-40', '2023-04-04 12:33:55.0476896', '1F9EBB06-0947-4D6F-F99C-08DB34EFB52C');
GO

### The last query is very huge so you can divide it into small parts or insert not all items
INSERT INTO Accommodation VALUES ('8FE86923-0BFA-41AD-8B1D-147B00C585CB', 'A luxurious villa', 'Modern studio apartment with all the amenities. Located in a vibrant neighborhood with plenty of restaurants and bars.', 605, 'https://www.realestate.com.au/blog/images/1024x768-fit,progressive/2019/08/21114902/capi_89fabccddb1bb4e76ce88070ffd56381_3926fda65b7c535a9f34af1ed1945f37.jpeg', '1F9EBB06-0947-4D6F-F99C-08DB34EFB52C', '2023-03-27 12:43:36.0871408'), ('2DA307F8-0EB9-433A-B650-2859B999A97D', 'Third accommodation', 'Accommodation, that was created to test creation accommodation feature', 450, 'https://img.freepik.com/free-vector/web-development-programmer-engineering-coding-website-augmented-reality-interface-screens-developer-project-engineer-programming-software-application-design-cartoon-illustration_107791-3863.jpg?w=2000', '1F9EBB06-0947-4D6F-F99C-08DB34EFB52C', '2023-03-28 12:25:38.1057471'), ('24F61215-0095-4C21-B56D-2C122325AC31', 'A modern apartment with sleek furnishings', 'Cozy 1-bedroom cottage nestled among the trees. Perfect for a romantic getaway or a peaceful retreat.', 760, NULL, '1F9EBB06-0947-4D6F-F99C-08DB34EFB52C', '2023-03-27 12:43:36.0761225'), ('23747225-F3B2-4F84-ADB6-35273EE8D886', 'Mine accommodation', 'Some description 2', 1200, 'https://www.dmu.ac.uk/webimages/study/accommodation/halls-of-residence/bede-hall/bede-hall-bedroom-02.jpg', '1F9EBB06-0947-4D6F-F99C-08DB34EFB52C', '2023-04-03 15:58:17.1233847');
GO
```

5. To exit from sqlcmd type: <code>exit</code>