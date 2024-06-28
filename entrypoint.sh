#!/bin/bash

set -e
run_cmd="dotnet Ozon.MerchService.dll --no-build -v d"
export PATH="$PATH:/root/.dotnet/tools"

>&2 echo "---- Run MerchService DB migration"
dotnet Ozon.MerchService.Migrator.dll --no-build -v d -- --dryrun
dotnet Ozon.MerchService.Migrator.dll --no-build -v d
>&2 echo "---- MerchService DB migration completed"

>&2 echo "---- Starting app"
>&2 echo "---- Run MerchService $run_cmd"
exec $run_cmd