$conn = New-Object System.Data.SqlClient.SqlConnection('Server=db43195.public.databaseasp.net;Database=db43195;User Id=db43195;Password=Mo@123456;Encrypt=False')
$conn.Open()
$cmd = $conn.CreateCommand()
$cmd.CommandText = 'SELECT TOP 10 Email, UserTypes FROM [auth].[ApplicationUsers] WHERE (UserTypes & 1) = 1 OR (UserTypes & 2) = 2 OR (UserTypes & 17) = 17 OR (UserTypes & 18) = 18'
$reader = $cmd.ExecuteReader()
while($reader.Read()) {
    Write-Host "Email: $($reader['Email']) - Types: $($reader['UserTypes'])"
}
$conn.Close()
