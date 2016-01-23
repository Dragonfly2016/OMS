Imports System.Data.OleDb

Public Class FrmPizzaCounter
    'Frequently Used Variables'
    Dim grandtotal As Decimal
    Private cn As New OleDbConnection
    Private cmd As New OleDbCommand
    Private rdr As OleDbDataReader
    Private ds As New DataSet
    Private da As OleDb.OleDbDataAdapter

    'Private Sub ClearData()
    '    cboTableNo.Text = Nothing
    '    ListView1.Items.Clear()
    '    cboQty.Text = 1
    'End Sub

    Private Sub frmpizzacounter_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Database Connection String and Open Database if Closed'
        cn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Application.StartupPath & "\Pizza.accdb"
        If cn.State = ConnectionState.Closed Then cn.Open()
        'Retrieve Item Categories from Database that Match User Selection'
        Try
            cmd.CommandText = "select * from tblItemCategory"
            cmd.Connection = cn
            rdr = cmd.ExecuteReader
            While (rdr.Read())
                cboCategory.Items.Add(rdr("ItemCategory"))
            End While
            cmd.Dispose()
            rdr.Close()
        Catch ex As Exception
            MessageBox.Show(ex.ToString())
        End Try
        'Retrieve Item Category Options for Additional Options from Database'
        cmd.CommandText = "select * from tblItemCatOption"
        cmd.Connection = cn
        rdr = cmd.ExecuteReader
        While (rdr.Read())
            chkladditionalOptions.Items.Add(rdr("ItemCatOption"))
        End While
        cmd.Dispose()
        rdr.Close()
        'Retrieve Last Order Number from Database & Add 1 for Next Order'
        cmd.CommandText = "select * from tblOrders"
        cmd.Connection = cn
        rdr = cmd.ExecuteReader
        While (rdr.Read())
            txtOrderNo.Text = (rdr("ID") + 1)
            'cboOrderNo.Items.Add(rdr("ID") +1)
        End While
        cmd.Dispose()
        rdr.Close()
    End Sub
    'Category Selected Index Changed'
    Private Sub cboCategory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboCategory.SelectedIndexChanged
        'Clear any Previous Item Data'
        cboItem.Items.Clear()
        'cboCategory.Items.Clear()
        'Retrieve Item Categories from Database that Match User Selection'
        Try
            cmd.CommandText = "select * from tblItem where ItemCategory = '" & cboCategory.SelectedItem & "'"
            cmd.Connection = cn
            rdr = cmd.ExecuteReader
            While (rdr.Read())
                cboItem.Items.Add(rdr("ItemRef"))
            End While
            cmd.Dispose()
            rdr.Close()
        Catch ex As Exception
            MessageBox.Show(ex.ToString())
        End Try
    End Sub
    'Item Selected Index Changed'
    Private Sub cboItem_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboItem.SelectedIndexChanged
        'Retrieve Item Prices from Database'
        Try
            cmd.CommandText = "select * from tblItem where ItemRef = '" & cboItem.Text & "'"
            cmd.Connection = cn
            rdr = cmd.ExecuteReader
            If (rdr.Read() = True) Then
                txtPrice.Text = (rdr("ItemPrice"))
                'Ensures 2dp Currency Format'
                txtPrice.Text = CDbl(txtPrice.Text)
            End If
            cmd.Dispose()
            rdr.Close()
        Catch ex As Exception
            MessageBox.Show("Error")
        End Try
    End Sub
    ''Adds Price of Additional Items'
    'Private Sub ExtraOptions()
    '    Dim price As Double = txtPrice.Text
    '    For i = 0 To chkladditionalOptions.Items.Count - 1
    '        If chkladditionalOptions.GetItemChecked(i) = True Then
    '            price = price + 1
    '            txtPrice.Text = price.ToString
    '        End If
    '    Next
    'End Sub
    ''Removes Price of Additional Items'
    'Private Sub RemoveExtraOptions()
    '    Dim price As Double = txtPrice.Text
    '    For i = 0 To chkladditionalOptions.Items.Count - 1
    '        If chkladditionalOptions.GetItemChecked(i) = True Then
    '            price = price - 1
    '            txtPrice.Text = price.ToString
    '            chkladditionalOptions.SetItemCheckState(i, CheckState.Unchecked)
    '        End If
    '    Next
    'End Sub
    'Add Item Button'
    Private Sub btnAddItem_Click(sender As Object, e As EventArgs) Handles btnAddItem.Click
        Dim arrLVItem(0) As ListViewItem
        ' Dim arrcheckedItem(5) As
        'Add Array of Data to ListView1'
        For j = 0 To arrLVItem.Length - 1
            'Initialize ListViewItem Array
            arrLVItem(j) = New ListViewItem
            'Add Text To First ListView Item 
            arrLVItem(j).SubItems(0).Text = cboCategory.Text
            'Add SubItems to ListView Item
            arrLVItem(j).SubItems().Add(cboItem.Text)
            Dim qty As Integer
            Dim price As Double
            Dim total As Double
            qty = (cboQty.Text)
            price = CDbl(txtPrice.Text)
            total = qty * price

            'Add Cost of each Additional Item to Price and Total'
            For i = 0 To chkladditionalOptions.Items.Count - 1
                If chkladditionalOptions.GetItemChecked(i) = True Then
                    'Retrieve Item Category Options for Additional Options from Database'
                    cmd.CommandText = "select * from tblItemCatOption"
                    cmd.Connection = cn
                    rdr = cmd.ExecuteReader
                    While (rdr.Read())

                        'If chkladditionalOptions.Then Then

                        chkladditionalOptions.Items.Add(rdr("ItemCatOption"))
                        'chkladditionalOptions.Items.Add(rdr("ItemCatOptCost"))
                        'chkladditionalOptions.Items.Add(rdr("ItemCatOptCost"))

                    End While
                    cmd.Dispose()
                    rdr.Close()
                    'price = price + 1
                    'txtPrice.Text = price.ToString
                    'total = total + 1
                End If
            Next i
            arrLVItem(j).SubItems().Add(chkladditionalOptions.Text)
            arrLVItem(j).SubItems().Add(cboQty.Text)
            arrLVItem(j).SubItems().Add(txtPrice.Text)
            arrLVItem(j).SubItems().Add(total)
            grandtotal = grandtotal + total
        Next j
        ListView1.Items.AddRange(arrLVItem)
    End Sub
    'Remove Items from ListView1'
    Private Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click
        Dim objListItem As ListViewItem
        For Each objListItem In ListView1.SelectedItems
            objListItem.Remove()
        Next objListItem
    End Sub
    'Clear All Inputs and Output Data'
    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        Dim x As DialogResult
        x = MessageBox.Show("Are you sure you want to delete?", "Click No to cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)
        If x = Windows.Forms.DialogResult.Yes Then
            grandtotal = 0
            cboCategory.Text = ""
            cboQty.Text = ""
            cboItem.Text = ""
            txtPrice.Text = ""
            cboTableNo.Text = ""
            txtOrderNo.Text = ""
            'Clear both ListViews'
            Dim objListItem As ListViewItem
            For Each objListItem In ListView1.Items
                For Each ListViewItem In ListView2.Items
                    objListItem.Remove()
                    ListViewItem.Remove()
                Next ListViewItem
            Next objListItem
            For i = 0 To chkladditionalOptions.Items.Count - 1
                If chkladditionalOptions.GetItemChecked(i) = True Then
                    chkladditionalOptions.SetItemCheckState(i, CheckState.Unchecked)
                End If
            Next
        End If
    End Sub
    'Accept and Save Button'
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        'Saves data in ListView1 and Displays Array of Data in ListView2'
        Try
            cmd.CommandText = "insert into tblOrders (OrderNumber, TableNumber, OrderTotal) values ('" & txtOrderNo.Text & "','" & cboTableNo.Text & "','" & grandtotal & "')"
            cmd.Connection = cn
            cmd.ExecuteNonQuery()
            cn.Close()
        Catch ex As Exception
            MessageBox.Show("Error while inserting record into table..." & ex.Message.ToString, "Insert Records")
        End Try
        'Create an Array to Add to ListView2'
        Dim arrLVItem(0) As ListViewItem
        For j = 0 To arrLVItem.Length - 1
            'Initialize ListViewItem Array
            arrLVItem(j) = New ListViewItem
            'Add Text To First ListView Item 
            arrLVItem(j).SubItems(0).Text = cboTableNo.Text
            'Add SubItems to ListView Item
            arrLVItem(j).SubItems().Add(txtOrderNo.Text)
            arrLVItem(j).SubItems().Add(grandtotal)
        Next j
        ListView2.Items.AddRange(arrLVItem)
    End Sub
    'Table Number Index Changed'
    Private Sub cboTableNo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboTableNo.SelectedIndexChanged
        'Allows User Editing of Table No's'
        cboTableNo.Enabled = True
    End Sub
    'New Top Menu Item'
    Private Sub NewToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NewToolStripMenuItem.Click
        'Get User Confirmation to Start a New Order'
        Dim x As DialogResult
        x = MessageBox.Show("Are you sure you want to delete?", "Click No to cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)
        If x = Windows.Forms.DialogResult.Yes Then
            grandtotal = 0
            cboCategory.Text = ""
            cboQty.Text = ""
            cboItem.Text = ""
            txtPrice.Text = ""
            Dim objListItem As ListViewItem
            For Each objListItem In ListView1.Items
                objListItem.Remove()
            Next objListItem
            For i = 0 To chkladditionalOptions.Items.Count - 1
                If chkladditionalOptions.GetItemChecked(i) = True Then
                    chkladditionalOptions.SetItemCheckState(i, CheckState.Unchecked)
                End If
            Next
        End If
    End Sub
    'Exit Top Menu Item'
    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HelpToolStripMenuItem.Click
        'Get User Confirmation to Exit'
        Dim x As DialogResult
        x = MessageBox.Show("Are you sure you want to Exit?", "Click No to Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)
        If x = Windows.Forms.DialogResult.Yes Then Me.Close()
    End Sub
    'Help Top Menu Item'
    Private Sub HelpToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HelpToolStripMenuItem.Click
        'Show Help Form'
        Help.Show()
    End Sub
End Class