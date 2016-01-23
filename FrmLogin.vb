Public Class FrmLogin
    Private Sub frmLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Set Focus to Username Textbox'
        txtUsername.Focus()
    End Sub

    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Dim cn As New ADODB.Connection
        Dim rs As New ADODB.Recordset, sSQL As String
        cn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Application.StartupPath & "\Pizza.accdb"
        'cn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\user\Desktop\USBDrive\Work\Assignment3\falguni\Project\POMS\Pizza.accdb"
        Try
            If cn.State = ConnectionState.Closed Then cn.Open()
            sSQL = "SELECT * FROM tblUser WHERE Status = 'A' AND Username ='" & txtUsername.Text.ToString & "'"
            rs.Open(sSQL, cn)
            With rs
                If .EOF And .BOF Then
                    MessageBox.Show("No such user")
                Else
                    If .Fields("UserPassword").Value.ToString = txtPassword.Text.ToString Then
                        MessageBox.Show("Welcome " & .Fields("FirstName").Value.ToString)
                        Me.Hide()
                        FrmPizzaCounter.Show()
                    Else
                        MessageBox.Show("Invalid Password")
                    End If
                End If
            End With
            rs.Close()

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        If cn.State = ConnectionState.Open Then cn.Close()
        rs = Nothing
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Dim x As DialogResult
        x = MessageBox.Show("Are you sure?", "Please log in", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)
        If x = Windows.Forms.DialogResult.Yes Then Application.Exit()


    End Sub

End Class