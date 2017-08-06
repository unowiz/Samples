$tenantId = "<tenantid>"
$resourceUri = "<resourceuri>"
$clientId = "<clientid>"

# デバイス コードの取得
$uri = "https://login.microsoftonline.com/" + $TenantId + "/oauth2/devicecode?" + `
       "resource=" + [System.Uri]::EscapeDataString($resourceUri) + "&" + `
       "client_id=" + $clientId
$headers = @{
    "Accept" = "application/json"
}
$result = Invoke-RestMethod -Method "Get" -Uri $uri -Headers $headers

$userCode = $result.user_code
$deviceCode = $result.device_code

Write-Output $userCode
Start-Process "https://aka.ms/devicelogin"

Read-Host | Out-Null

# トークンの取得
$uri = "https://login.microsoftonline.com/" + $TenantId + "/oauth2/token"
$headers = @{ 
    "Accept" = "application/json"
    "Content-Type" = "application/x-www-form-urlencoded"
}
$body = "resource=" + [System.Uri]::EscapeDataString($resourceUri) + "&" + `
        "client_id=" + $clientId + "&" + `
        "grant_type=device_code&" + `
        "code=" + [System.Uri]::EscapeDataString($deviceCode)

$result = Invoke-RestMethod -Method "Post" -Uri $uri -Headers $headers -Body $body

$accessToken = $result.access_token

# サイトのタイトルを取得
$uri = $resourceUri + "/_api/web/title"
$headers = @{ 
    "Accept" = "application/json"
    "Authorization" = "Bearer " + $accessToken
}
$result = Invoke-RestMethod -Method "Get" -Uri $uri -Headers $headers
Write-Output $result.value

# ドキュメントの一覧を取得
$uri = $resourceUri + "/_api/web/getfolderbyserverrelativeurl('/Shared%20Documents')/files"
$headers = @{ 
    "Accept" = "application/json"
    "Authorization" = "Bearer " + $accessToken
}
$result = Invoke-RestMethod -Method "Get" -Uri $uri -Headers $headers
$result.value | select Name, TimeCreated, TimeLastModified
