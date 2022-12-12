Imports System.Data.OleDb

Module MKoneksi
    Public conn As OleDbConnection
    Public ds As DataSet
    Public da As OleDbDataAdapter
    Public str As String
    Public cmd As OleDbCommand
    Public rd As OleDbDataReader

    Public Sub koneksi()
        str = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=db_antrian.mdb"
        conn = New OleDbConnection(str)
        If conn.State = ConnectionState.Closed Then conn.Open()
    End Sub

    Sub Register()
        Call koneksi()
        Dim iduser As String
        cmd = New OleDbCommand("select * from tb_user order by iduser desc", conn)
        rd = cmd.ExecuteReader
        rd.Read()
        If Not rd.HasRows Then
            iduser = "USER" + "000"
        Else
            iduser = Val(Microsoft.VisualBasic.Mid(rd.Item("iduser").ToString, 3, 2)) + 1
            If Len(iduser) = 1 Then
                iduser = "USER00" & iduser & ""
            ElseIf Len(iduser) = 2 Then
                iduser = "USER0" & iduser & ""
            End If
        End If

        Dim register As String = "insert into tb_user values " & _
            "('" & iduser & "','" & _
            FRegister.tb_fullname.Text & "','" & _
            "Active" & "','" & _
            "Customer Service" & "','" & _
            FRegister.tb_username.Text & "','" & _
            FRegister.tb_password.Text & "') "
        cmd = New OleDbCommand(register, conn)
        cmd.ExecuteNonQuery()

        Form1.Show()
        FRegister.Close()
    End Sub

    Sub Login()
        Call koneksi()
        cmd = New OleDbCommand("select * from tb_user where username ='" & _
        Form1.txt_username.Text & _
        "' and pass ='" & Form1.txt_password.Text & "'", conn)
        rd = cmd.ExecuteReader
        rd.Read()
        If Not rd.HasRows Then
            MsgBox("Username dan password tidak dikenali", MsgBoxStyle.Critical, _
            "Error")
        Else
            cmd = New OleDbCommand("select * from tb_user where username ='" & _
            Form1.txt_username.Text & _
            "' and pass ='" & Form1.txt_password.Text & "' and ket ='" & "Customer Service" & "' ", conn)
            rd = cmd.ExecuteReader
            rd.Read()
            If rd.HasRows = True Then
                MsgBox("Login Berhasil", MsgBoxStyle.Information, "Success")
                FCFS.lb_username.Text = "Welcome, " & Form1.txt_username.Text
                FCFS.ShowDialog()
            Else
                MsgBox("Login Berhasil", MsgBoxStyle.Information, "Success")
                FAdmin.lb_username.Text = "Welcome, " & Form1.txt_username.Text
                FAdmin.ShowDialog()
            End If

        End If
    End Sub

    Sub NomorAntrian()
        Call koneksi()
        Dim idantrian As String

        Dim j As String
        Dim nomor As Integer
        j = " Select count(*) from tb_antrian"
        cmd = New OleDb.OleDbCommand(j, conn)
        Dim n As Integer
        n = cmd.ExecuteScalar
        nomor = n
        If nomor = 0 Then
            idantrian = "A0001"
            FNomor.lb_nomor.Text = "00" & nomor + 1
        ElseIf nomor < 10 Then
            idantrian = "A000" & nomor + 1
            FNomor.lb_nomor.Text = "00" & nomor + 1
        ElseIf nomor < 100 Then
            idantrian = "A00" & nomor + 1
            FNomor.lb_nomor.Text = "0" & nomor + 1
        ElseIf nomor > 100 Then
            idantrian = "A0" & nomor + 1
        ElseIf nomor > 1000 Then
            idantrian = "A" & nomor + 1
        End If

        Dim nomorAntrian As String = "insert into tb_antrian values " & _
            "('" & idantrian & "','" & _
            "" & "','" & _
            "Waiting" & "','" & _
            "28/11/2022" & "','" & _
            "USER000" & "') "
        cmd = New OleDbCommand(nomorAntrian, conn)
        cmd.ExecuteNonQuery()


        MsgBox("Terimakasih, Anda sudah berhasil mengambil nomor antrian", _
               MsgBoxStyle.Information, "Sukses")

    End Sub

    Sub dataAntrian()
        Call koneksi()
        da = New OleDbDataAdapter("select idantrian as [NOMOR ANTRIAN],loket as LOKET, status as STATUS  from tb_antrian where status='" & "Waiting" & "' or status='" & "On Progress" & "'", conn)
        ds = New DataSet
        ds.Clear()
        da.Fill(ds, "tb_antrian")
        FCFS.dgv_antrian.DataSource = (ds.Tables("tb_antrian"))
    End Sub

    Sub dataUser()
        Call koneksi()
        da = New OleDbDataAdapter("select iduser as [ID USER] ,nama as NAMA, status as STATUS, ket as KETERANGAN  from tb_user", conn)
        ds = New DataSet
        ds.Clear()
        da.Fill(ds, "tb_user")
        FAdmin.dgv_user.DataSource = (ds.Tables("tb_user"))
    End Sub



End Module
