$ErrorActionPreference='Stop'

$mysqlcontainername = "hotline-mysql"
$hostport = 3308

if($env:mysqlpwd -eq $null)
{
    Write-Error "Create env variable for the mysql pawd before proceeding"
}


Write-Host 'pulling mysql image'
docker pull mysql

Write-Host 'run mysql container if not already running'
$image = docker ps -q -f name=$mysqlcontainername
if($image.Count -ne 0)
{
    Write-Host 'Skipping creation as mysql instance is already running'
}
else
{
    Write-Host 'Starting mysql container named ' $mysqlcontainername
    docker run -p 3308:3306 --name=$mysqlcontainername -v /data/mysql:/var/lib/mysql -e MYSQL_ROOT_PASSWORD=$env:mysqlpwd -d mysql
}

Write-Host 'Setting up database for hello world'
$args = @("-c", "Server=127.0.0.1;Port=3308;Database=Messages;Uid=root;Pwd=$env:mysqlpwd;", "-a", "HelloWorld.dll")
& '.\DatabaseSetup.exe' @args