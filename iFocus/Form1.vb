Imports MySql.Data.MySqlClient
Imports System.Timers
Public Class Form1
    Dim WithEvents Timer As New Timer(5000)
    Dim blockScreen = 0
    Dim PCname = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\Software\iFocus", "Name", Nothing)

    Private Declare Function GetForegroundWindow Lib "user32" Alias "GetForegroundWindow" () As IntPtr
    Private Declare Auto Function GetWindowText Lib "user32" (ByVal hWnd As System.IntPtr, ByVal lpString As System.Text.StringBuilder, ByVal cch As Integer) As Integer
    Private makel As String

    Private Function GetCaption() As String
        Dim Caption As New System.Text.StringBuilder(256)
        Dim hWnd As IntPtr = GetForegroundWindow()
        GetWindowText(hWnd, Caption, Caption.Capacity)
        Return Caption.ToString()
    End Function
    Sub tampilData()
        PCname = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\Software\iFocus", "Name", Nothing)

        Konek()

        da = New MySqlDataAdapter("SELECT * FROM account", Koneksi)
        dt = New DataTable
        da.Fill(dt)
        'DGV.Rows.Clear()
        'For i = 0 To dt.Rows.Count - 1
        '    DGV.Rows.Add(dt.Rows(i).Item(0))
        '    DGV.Rows(i).Cells(1).Value = dt.Rows(i).Item(1)
        '    DGV.Rows(i).Cells(2).Value = dt.Rows(i).Item(2)
        'Next
        'MsgBox(dt.Rows(0).Item(0))
        'Label1.Text = dt.Rows(0).Item(1)
        blockScreen = dt.Rows(0).Item(1)
        System.Console.WriteLine(blockScreen)
        System.Console.WriteLine(PCname)
        System.Console.WriteLine(GetCaption())
        If Me.InvokeRequired Then
            Me.BeginInvoke(New Action(
                           Sub()
                               If blockScreen = 0 Then
                                   Me.FormBorderStyle = Windows.Forms.FormBorderStyle.FixedDialog
                                   Me.WindowState = FormWindowState.Normal
                                   Me.TopMost = False
                               ElseIf blockScreen = 1 Then
                                   Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
                                   Me.WindowState = FormWindowState.Maximized
                                   Me.TopMost = True
                                   MenuStrip1.Visible = False
                               End If

                           End Sub))
        End If
        Diskonek()
    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Timer.Start()
        Me.TransparencyKey = Color.LightBlue
        Me.BackColor = Color.LightBlue

        PCNameToolStripMenuItem.Text = PCname
        If PCname IsNot Nothing Then
            RegisterPCToolStripMenuItem.Text = "Unregister PC"
        End If
    End Sub
    Private Sub Form1_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If (e.CloseReason = CloseReason.UserClosing) Then
            If blockScreen = 1 Then
                e.Cancel = True
            End If
        End If
    End Sub
    Private Sub Form1_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.X Then
            Application.Exit()
            MsgBox("wkwkwk")
        End If
    End Sub
    Private Sub Timer_Elapsed(sender As Object, e As ElapsedEventArgs) Handles Timer.Elapsed
        tampilData()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        My.Computer.Registry.CurrentUser.CreateSubKey("Software\iFocus")
        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\Software\iFocus", "name", "PC01")
    End Sub

    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        If Me.WindowState = FormWindowState.Minimized Then
            NotifyIcon1.Visible = True
            NotifyIcon1.Icon = SystemIcons.Application
            'NotifyIcon1.BalloonTipIcon = ToolTipIcon.Info
            'NotifyIcon1.BalloonTipTitle = "iFocus"
            'NotifyIcon1.BalloonTipText = "iFocus"
            'NotifyIcon1.ShowBalloonTip(50000)
            ShowInTaskbar = False
        End If
    End Sub

    Private Sub NotifyIcon1_DoubleClick(sender As Object, e As EventArgs) Handles NotifyIcon1.DoubleClick
        ShowInTaskbar = True
        Me.WindowState = FormWindowState.Normal
        NotifyIcon1.Visible = False
    End Sub

    Private Sub RegisterPCToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RegisterPCToolStripMenuItem.Click

    End Sub
End Class
