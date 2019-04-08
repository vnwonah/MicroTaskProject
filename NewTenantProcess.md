# This Document Outlines the Process for Creating a New Tenant

## Tenant can be added to a shared database with other tenants or created with its own database.

### Adding a Tenant into a Shared Database

Required Parameters:

TenantName
Team Logo Link
Team Country
Team User

Create a Tenant Key for the Tenant

<#
.SYNOPSIS
    Returns an integer tenant key from a tenant name for use in the catalog.
#>
function Get-TenantKey
{
    param
    (
        # Tenant name 
        [parameter(Mandatory=$true)]
        [String]$TenantName
    )

    $normalizedTenantName = $TenantName.Replace(' ', '').ToLower()

    # Produce utf8 encoding of tenant name 
    $utf8 = New-Object System.Text.UTF8Encoding
    $tenantNameBytes = $utf8.GetBytes($normalizedTenantName)

    # Produce the md5 hash which reduces the size
    $md5 = new-object -TypeName System.Security.Cryptography.MD5CryptoServiceProvider
    $tenantHashBytes = $md5.ComputeHash($tenantNameBytes)

    # Convert to integer for use as the key in the catalog 
    $tenantKey = [bitconverter]::ToInt32($tenantHashBytes,0)

    return $tenantKey
}

Check if Key exists in Database, throw error if yes

# if a tenant database is input use that, otherwise use the default database 
    if($TenantDatabase)
    {    
        $serverName = $TenantDatabase.ServerName
        $databaseName = $TenantDatabase.DatabaseName
    }
    else
    {
        $serverName = $config.TenantsServerNameStem + $wtpUser.Name
        $databaseName = $config.TenantsDatabaseName
    }
    
Next, Create the new Tenant in the Database (Initialize Tenant)

Parameters: 

        -ServerName
        -DatabaseName
        -TenantKey
        -TenantName
        -Team Logo Link
        -Team Country
        -Team User
        
        Create new team in teams database
        
        //add tenant to catalog
        
        parameters: -Catalog $catalog `
        -TenantName $TenantName `
        -TenantKey $tenantKey `
        -ServerName $serverName `
        -DatabaseName $DatabaseName
        
         $ServerFullyQualifiedName = $ServerName + ".database.windows.net"
         
          # Register the tenant in the catalog
   function Add-TenantToCatalog
{
    param(
        [parameter(Mandatory=$true)]
        [object]$Catalog,

        [parameter(Mandatory=$true)]
        [string]$TenantName,

        [parameter(Mandatory=$true)]
        [int32]$TenantKey,

        [parameter(Mandatory=$true)]
        [string]$ServerName,

        [parameter(Mandatory=$true)]
        [string]$DatabaseName
    )

    $ServerFullyQualifiedName = $ServerName + ".database.windows.net"

    # Add the tenant-to-database mapping to the catalog (idempotent)
    Add-ListMapping `
        -KeyType $([int]) `
        -ListShardMap $Catalog.ShardMap `
        -SqlServerName $ServerFullyQualifiedName `
        -SqlDatabaseName $DatabaseName `
        -ListPoint $TenantKey

    # Add the tenant name to the catalog as extended meta data (idempotent)
    Add-ExtendedTenantMetaDataToCatalog `
        -Catalog $Catalog `
        -TenantKey $TenantKey `
        -TenantName $TenantName
}

function Add-ListMapping
{
    param 
    (
         # Type of list shard map
        [parameter(Mandatory=$true)]
        [Type]$KeyType,

        [parameter(Mandatory=$true)]
        [System.Object]$ListShardMap,

        [parameter(Mandatory=$true)]
        [object]$ListPoint,

        [parameter(Mandatory=$true)]
        [String]$SqlServerName,

        [parameter(Mandatory=$true)]
        [String]$SqlDatabaseName
    )
      
    # Add new shard location to list shard map
    $ShardLocation = New-Object Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement.ShardLocation($SqlServerName, $SqlDatabaseName, 'tcp', '1433')

    # Check if the list mapping already exists in the shard map manager    
    $InputShard = $listShardMap.GetShard($ShardLocation)
    $InputPoint = $ListPoint -as $KeyType
    
    Write-Verbose "`tChecking if List Point $ListPoint exists for $SqlDatabaseName..."

    $Mappings = $ListShardMap.GetMappings($InputPoint)

    if($Mappings.count -gt 0 -and $Mappings[0].Value -eq $InputPoint)
    {
        Write-Verbose "`tPoint ($InputPoint) already exists for $SqlDatabaseName"
    }
    else
    {
        Write-Verbose "`tPoint ($InputPoint) for $SqlDatabaseName does not exist, adding..."
        $ShardReference = $listShardMap.CreatePointMapping($InputPoint, $InputShard)
        Write-Verbose "`tNew point ($InputPoint) for $SqlDatabaseName added to list shard map"
    }
}

function Add-ExtendedTenantMetaDataToCatalog
{
    param(
        [parameter(Mandatory=$true)]
        [object]$Catalog,

        [parameter(Mandatory=$true)]
        [int32]$TenantKey,

        [parameter(Mandatory=$true)]
        [string]$TenantName
    )

    $config = Get-Configuration

    # Get the raw tenant key value used within the shard map
    $tenantRawKey = Get-TenantRawKey ($TenantKey)
    $rawkeyHexString = $tenantRawKey.RawKeyHexString


    # Add the tenant name into the Tenants table
    $commandText = "
        MERGE INTO Tenants as [target]
        USING (VALUES ($rawkeyHexString, '$TenantName')) AS source
            (TenantId, TenantName)
        ON target.TenantId = source.TenantId
        WHEN MATCHED THEN
            UPDATE SET TenantName = source.TenantName
        WHEN NOT MATCHED THEN
            INSERT (TenantId, TenantName)
            VALUES (source.TenantId, source.TenantName);"

    Invoke-SqlAzureWithRetry `
        -ServerInstance $Catalog.FullyQualifiedServerName `
        -Username $config.TenantAdminuserName `
        -Password $config.TenantAdminPassword `
        -Database $Catalog.Database.DatabaseName `
        -Query $commandText `
        -ConnectionTimeout 30 `
        -QueryTimeout 30 `
}

