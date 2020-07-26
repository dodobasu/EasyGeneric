
Imports System.Data.SQLite
Imports MySql.Data.MySqlClient


Public Class FrmLogin


    Dim dt As DataTable
    Dim n As Integer = 0


    Private Sub FrmLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LinkLabel1.Text = "website : http://www.easysolution.net.in"
        LinkLabel1.Links.Add(10, 33, "http://www.easysolution.net.in")

        isNetwork()
        isSqlite()

    End Sub


    Public Sub isNetwork()

        If My.Computer.Network.IsAvailable Then
            lblNetwork.BackColor = Color.DarkGreen
            lblNetwork.Text = "GOOD ! LAN Network available"
        Else
            lblNetwork.BackColor = Color.OrangeRed
            lblNetwork.Text = "SORRY ! LAN Network not available"
        End If
    End Sub

    Public Sub isSqlite()
        Dim DB_Path As String = "Data Source=" & Application.StartupPath & "\db.db;"
        Dim sql As String = "Select * From MYSQLConnection WHERE id=1"


        Try
            Dim conn As New SQLiteConnection(DB_Path)
            Dim cmd = New SQLiteCommand(conn)
            cmd.CommandText = sql
            conn.Open()
            Dim da As New SQLiteDataAdapter(cmd)
            Dim dbrs As DataRow
            Dim dt As DataTable = New DataTable()
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                dbrs = dt.Rows(0)

                hostz = dbrs("hosts")
                databasez = dbrs("dbname")
                userz = "root"
                passwordz = "Pp1975$3"
                lblSqlite.BackColor = Color.DarkGreen
                lblSqlite.Text = "GOOD!System DB is working"
            Else
                lblSqlite.BackColor = Color.OrangeRed
                lblSqlite.Text = "Sorry!System DB is not working"
            End If
        Catch ex As Exception
            lblSqlite.BackColor = Color.OrangeRed
            lblSqlite.Text = "Very Bad!System DB is corrupt" & ex.GetBaseException.Message()
        End Try
    End Sub

    Private Sub lblMysql_Click(sender As Object, e As EventArgs) Handles lblMysql.Click

    End Sub

    Private Sub FrmLogin_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        If OpenDB() = True Then
            lblMysql.BackColor = Color.DarkGreen
            lblMysql.Text = "GOOD !Server DB is Working"
        ElseIf OpenDB() = False Then

            lblMysql.BackColor = Color.OrangeRed
            lblMysql.Text = "SORRY !Server DB is not Working"

        End If
        CloseDB()
    End Sub

    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Dim dr As MySqlDataReader
        Dim count As Integer = 0


        Try
            OpenDB()
            Dim sql As String = "SELECT * from user where username='" & txtUsername.Text.Trim & "' AND password='" & txtPassword.Text.Trim & "'"
            Dim cmd = New MySqlCommand(sql, conn)
            dr = cmd.ExecuteReader()

            While dr.Read
                count = count + 1
            End While


            If count = 1 Then
                MessageBox.Show("Username & password are correct")
                namez = dr.GetString("name")
                rolez = dr.GetString("role")
                timez = DateTime.Now
                MDIForm.Show()
                Me.Hide()
            ElseIf count > 1 Then
                MessageBox.Show("Username & password are duplicate")
            Else
                MessageBox.Show("Username & password are incorrect")
                n = n + 1

            End If

            CloseDB()

        Catch ex As Exception

        End Try

        If n > 3 Then
            MessageBox.Show("You have tried 3 times - programme will be closed")
            End
        End If
    End Sub

    Private Sub btnSettings_Click(sender As Object, e As EventArgs) Handles btnSettings.Click
        FrmSystemUser.Show()
        Me.Hide()
    End Sub
End Class
