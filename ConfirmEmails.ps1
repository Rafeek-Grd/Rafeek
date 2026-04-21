$conn = New-Object System.Data.SqlClient.SqlConnection('Server=db43195.public.databaseasp.net;Database=db43195;User Id=db43195;Password=Mo@123456;Encrypt=False')
$conn.Open()
$cmd = $conn.CreateCommand()
$cmd.CommandText = 'UPDATE [auth].[ApplicationUsers] SET EmailConfirmed = 1'
$rows = $cmd.ExecuteNonQuery()
Write-Host "Updated $rows users to have confirmed emails."
$conn.Close()
