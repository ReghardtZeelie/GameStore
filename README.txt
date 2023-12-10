--------------------------------------------------------------------------------------
--------------------------------------GameStore---------------------------------------
--------------------------------------------------------------------------------------
High level assumptions
--------------------------------------------------------------------------------------
It is assumed that the developer has access to SQL server Management studio.
It is assumed that the developer has access to an .SQL server admin account that has the rights to execute .SQL scripts.
--------------------------------------------------------------------------------------
Environment:
--------------------------------------------------------------------------------------
Windows 11 pro
IIS 10
visual studio 2022
SQL server 2017
swagger
docker
--------------------------------------------------------------------------------------
Debug/Deployment instructions
--------------------------------------------------------------------------------------

1. Navigate to the directory where the  repository was cloned.
2. Locate the DB folder that is part of the items that was cloned.
3. Locate the CreateDBScript.sql file.
4. Open the file in SQL server Management studio and execute the .SQL script with a user that has admin rights.
5. Create an 'Admin' user and link it to the GameStoreDB game store DB that was created.
6. Open the GameStore.sln with the appropriate version of visual studio.
7. Run a Nuget restore on the solution.
8. locate the appsettings.json from the solution explorer.
9  locate the section in the file named: GameStoreSQL
10. Amend the following sections so that it matches the developer’s local configuration.
	*REGHARDT-PC // The address of the SQL server
	*P@ssW0rd1 // the password of the Admin user that was instructed to be created in step 5.
11 Save the file.
12. Build the solution.
13. The solution is now ready to be debugged.
---------------------------------------------------------------------------------------	
--------------------------------------API Calls----------------------------------------
---------------------------------------------------------------------------------------
::Add User::(POST)
/User/AddUser
Paramaters:
Name            : Data Type
------------------------------
Name            : String (30)
Password        : string (100)
confirmPassword : string (100)
DOB             : Date

::Cart::(POST)
/GameStore/Cart/AddItemsToCart
Paramaters:
Name            : Data Type
------------------------------
UserName        : String (30)
Token           : string 
ItemCode        : int32
Qty             : int 

/GateStore/Cart/ViewCart(GET)
Paramaters:
Name            : Data Type
------------------------------
UserName        : String (30)
Token           : string 


/GateStore/Cart/RemoveitemsfromCart(DELETE)
Paramaters:
Name            : Data Type
------------------------------
UserName        : String (30)
Token           : string 
ItemCode        : int32
Qty             : int 


::Items::
/GameStore/Item/AddNewitem(post)
Paramaters:
Name            : Data Type
------------------------------
UserName        : String (30)
Token           : string 
ItemName        : string (30)
ItemDescription : string (500)
ItemCost        : decimal
ItemWholeSale   : decimal
ItemRetail      : decimal
File            : Byte array
Make            : string(30)
Model           : string(30)

/GameStore/Item/DeleteItem(delete)
Paramaters:
Name            : Data Type
------------------------------
UserName        : String (30)
Token           : string 
ItemCode        : int


/GameStore/Item/ViewItemImage(Get)
Paramaters:
Name            : Data Type
------------------------------
UserName        : String (30)
Token           : string 
ItemCode        : int


/GameStore/Item/Searchitems(Get)
Paramaters:
Name           : Data Type
------------------------------
UserName       : String (30)
Token          : string 
ItemCode       : int

::Login::
/GameStore/Login(Post)
Paramaters:
Name           : Data Type
------------------------------
Name           : String (30)
Password       : string 
ItemCode       : int

/GameStore/Logout(Post)
UserName        : String (30)
Token           : string 
---------------------------------------------------------------------------------------