Imports System.Data.SQLite
Public Class FrmSystemUser

    Dim DB_Path As String = "Data Source=" & Application.StartupPath & "\db.db;"
    Dim table1 As String = "system_user"
    Dim table2 As String = "MYSQLConnection"
    Dim username, password, host, dbname, port As String
    Private Sub FrmSystemUser_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub FrmSystemUser_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Dim SQLiteCon As New SQLiteConnection(DB_Path)
        Try
            SQLiteCon.Open()
        Catch ex As Exception
            SQLiteCon.Dispose()
            SQLiteCon = Nothing
            MsgBox(ex.Message)
            Exit Sub
        End Try

        Dim TableDB As New DataTable

        Try
            LoadDB("select* from " & table2 & " order by id", TableDB, SQLiteCon)
            DataGridView.DataSource = Nothing
            DataGridView.DataSource = TableDB
            DataGridView.Columns("id").Visible = False
            DataGridView.Columns("hosts").HeaderText = "HOST/IP"
            DataGridView.Columns("dbname").HeaderText = "DATABASE NAME"
            DataGridView.Columns("port").HeaderText = "PORT"
            DataGridView.Columns("dbname").Width = 110
            DataGridView.Columns("hosts").Width = 80
            DataGridView.Columns("port").Width = 45
            DataGridView.ReadOnly = True
            DataGridView.ClearSelection()

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        If DataGridView.Rows.Count >= 0 Then
            txtDatabase.Text = DataGridView.Rows(0).Cells(1).Value.ToString
            txtHost.Text = DataGridView.Rows(0).Cells(2).Value.ToString
            txtPort.Text = DataGridView.Rows(0).Cells(3).Value.ToString
        End If
        TableDB.Dispose()
        TableDB = Nothing
        SQLiteCon.Close()
        SQLiteCon.Dispose()
        SQLiteCon = Nothing
    End Sub



    'Sub to read the database
    Private Sub LoadDB(ByVal q As String, ByVal tbl As DataTable, ByVal cn As SQLiteConnection)
        Dim SQLiteDA As New SQLiteDataAdapter(q, cn)
        SQLiteDA.Fill(tbl)
        SQLiteDA.Dispose()
        SQLiteDA = Nothing
    End Sub

    'Sub to write to the database
    Private Sub ExecuteNonQuery(ByVal query As String, ByVal cn As SQLiteConnection)
        Dim SQLiteCM As New SQLiteCommand(query, cn)
        SQLiteCM.ExecuteNonQuery()
        SQLiteCM.Dispose()
        SQLiteCM = Nothing
    End Sub

    Private Function isLogin(username As String, password As String) As Boolean
        Dim isValid As Boolean = False
        Dim userinfo As DataRow = Nothing
        Dim sql As String = "Select * From system_user WHERE username=@username"
        Try
            Dim conn As New SQLiteConnection(DB_Path)
            Dim cmd As New SQLiteCommand(conn)
            cmd.Parameters.AddWithValue("@username", username)
            cmd.CommandText = sql
            conn.Open()
            Dim da As New SQLiteDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                userinfo = dt.Rows(0)
                If userinfo("password").Equals(password) Then
                    isValid = True
                    dt.Dispose()
                    dt = Nothing
                End If
            End If
        Catch ex As Exception

            conn.Close()
            conn.Dispose()
            conn = Nothing
        End Try
        Return isValid
    End Function

    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        If isLogin(txtUsername.Text.Trim, txtPassword.Text.Trim) Then
            MsgBox("Login Successfully")
            GrpConnection.Enabled = True
            DataGridView.Enabled = True
        Else
            MsgBox("Login failed")
            GrpConnection.Enabled = False
            DataGridView.Enabled = False
        End If
    End Sub

    Private Sub DataGridView_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView.CellContentClick

    End Sub

    Private Sub DataGridView_CellMouseDown(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridView.CellMouseDown
        With DataGridView
            If e.RowIndex >= 0 Then


                txtDatabase.Text = .Rows(0).Cells(1).Value.ToString
                txtHost.Text = .Rows(0).Cells(2).Value.ToString
                txtPort.Text = .Rows(0).Cells(3).Value.ToString

            End If
        End With
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        FrmLogin.Show()
        Me.Hide()
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        If txtHost.Text = "" Then
            MessageBox.Show("Host/IP has not been filled, please fill in Host/IP", "Failed")
            Return
        End If

        If txtDatabase.Text = "" Then
            MessageBox.Show("Databse Name  has not been filled, please fill in Databse Name", "Failed")
            Return
        End If
        If txtPort.Text = "" Then
            MessageBox.Show("PORT  has not been filled, please fill in PORT", "Failed")
            Return
        End If
        Dim SQLiteCon As New SQLiteConnection(DB_Path)

        Try
            SQLiteCon.Open()
        Catch ex As Exception
            SQLiteCon.Dispose()
            SQLiteCon = Nothing
            MsgBox(ex.Message)
            Exit Sub
        End Try
        '' UPDATE SETTINGS
        Try
            ExecuteNonQuery("update " & table2 & " set hosts='" & txtHost.Text & "',dbname='" & txtDatabase.Text & "',port='" & txtPort.Text & "' Where id=1", SQLiteCon)
            MsgBox("Update successfully")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        txtDatabase.Text = ""
        txtHost.Text = ""
        txtPort.Text = ""
        Dim TableDB As New DataTable
        Try
            LoadDB("select* from " & table2 & " order by id", TableDB, SQLiteCon)
            DataGridView.DataSource = Nothing
            DataGridView.DataSource = TableDB
            DataGridView.Columns("id").Visible = False
            DataGridView.Columns("hosts").HeaderText = "HOST/IP"
            DataGridView.Columns("dbname").HeaderText = "DATABASE NAME"
            DataGridView.Columns("port").HeaderText = "PORT"
            DataGridView.Columns("dbname").Width = 110
            DataGridView.Columns("hosts").Width = 80
            DataGridView.Columns("port").Width = 45
            DataGridView.ReadOnly = True
            DataGridView.ClearSelection()

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        SQLiteCon.Close()
        SQLiteCon.Dispose()
        SQLiteCon = Nothing
    End Sub
End Class