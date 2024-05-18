# npgsql-ef-demo
Demo program for Npgsql with Entity Framework

## Install dotnet

```
# docker run -it --rm yugabytedb/yugabyte bash
#yum install -y git libicu wget
curl -Ls https://dotnet.microsoft.com/download/dotnet/scripts/v1/dotnet-install.sh | bash -x

alias dotnet=~/.dotnet/dotnet
```

## create PgInsertExample Hello World

```
dotnet new console -n PgInsertExample
cd PgInsertExample
dotnet add package Npgsql
dotnet run
```

## Test PgInsertExample

start YugabyteDB on same port / address as PostgreSQL

```
docker run -d -p5432:5433 yugabytedb/yugabyte yugabyted start --background=false --tserver_flags=yb_enable_read_committed_isolation=true
type pg_isready 2>/dev/null && until pg_isready ; do sleep 1 ; done | uniq -c
```

Run

```
cd PgInsertExample
dotnet run
```

