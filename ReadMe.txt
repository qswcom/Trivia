Note: Just tested in Windows enviroment

1. Requirement
    Net5.0
    Mysql or other database
2. Build
    run command "dotnet build TriviaServer.sln"
    run command "dotnet build TriviaClient.sln"
3. Config
    Go to "TriviaServer\AppServer\Main\bin\Debug\net5.0" folder
    Go to "configs" subfolder
    Config database connection string.
    Go back to "TriviaServer\AppServer\Main\bin\Debug\net5.0" folder
    Run "TriviaServer.AppServer.Main.exe -t" to initialize database (this operation can create 10000+ test questions, not used for production enviroment)
4. Run 
    Go to "TriviaServer\AppServer\Main\bin\Debug\net5.0" folder
    Run "TriviaServer.AppServer.Main.exe"

    Go to TriviaClient\ConsoleClient\Main\bin\Debug\net5.0" folder
    Run "TriviaClient.ConsoleClient.Main.exe" (can run multiple instance, please don't use same user id)