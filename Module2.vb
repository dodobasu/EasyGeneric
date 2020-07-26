Imports MySql.Data.MySqlClient
Module Connection
    Public conn As New MySqlConnection
    Dim Result As Boolean
    Dim strConn As String

    Public Function OpenDB() As Boolean
        Try
            If conn.State = ConnectionState.Closed Then
                strConn = "server=" & hostz & ";userid=" & userz & ";password=" & passwordz & ";Database=" & databasez
                conn.ConnectionString = strConn
                conn.Open()
                Result = True
            End If
        Catch ex As Exception
            Result = False

        End Try
        Return Result
    End Function

    Public Function CloseDB() As Boolean
        Try
            If conn.State = ConnectionState.Open Then
                conn.Close()
                Result = True
            End If
        Catch ex As Exception
            Result = False

        End Try
        Return Result
    End Function

End Module