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
docker run --name yb -d -p5432:5433 -p 15433:15433 yugabytedb/yugabyte yugabyted start --background=false --tserver_flags=yb_enable_read_committed_isolation=true,ysql_pg_conf_csv="{shared_preload_libraries='auto_explain',log_statement='all'}"

type pg_isready 2>/dev/null && until pg_isready ; do sleep 1 ; done | uniq -c
# tail -F /root/var/data/yb-data/tserver/logs/postgresql*

```

Run

```
cd PgInsertExample
dotnet run
```

### Trace the RPCs to master:

When in the YugabyteDB container you can use `tcpdump` to look at the RPCs to the `yb-master` (I used that to see the impact of `ServerCompatibilityMode=NoTypeLoading`)
```
docker exec -it yb bash

tcpdump -nni any dst port 7100 -A |
# display every 10 seconds the count of lines with ".yb."
 awk -F . '
 / IP [0-9.]+[.][0-9.]+ > [0-9.]+[.]7100: /{t1=$1}
 /[.]yb[.]/{c[$0]=c[$0]+1}
 substr(t1,1,7)>substr(t0,1,7){    # print every 10 seconds
  for (i in c) printf "%10d %-s\n",c[i],i;print t1;delete(c)
 }
 {t0=t1}
'

```