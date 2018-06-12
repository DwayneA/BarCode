Imports System
Imports System.IO.Ports

Public Class Barcode

    Dim WithEvents port As SerialPort = New System.IO.Ports.SerialPort("COM1", 9600, Parity.Mark, 8, StopBits.One)
    Public Code
    Public Code2

    Private Sub Barcode_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            CheckForIllegalCrossThreadCalls = False
            If port.IsOpen = False Then port.Open()
        Catch ex As Exception
        End Try

    End Sub

    ''Listen to data on RS-232 and add data to Code variable...

    Public Sub port_DataReceived(ByVal sender As Object, ByVal e As System.IO.Ports.SerialDataReceivedEventArgs) Handles port.DataReceived

        Try
            Code = (port.ReadExisting)
            CopyToClipboard()
        Catch ex As Exception
        End Try

    End Sub

    ''Start BackgroundProcess's in a new thread...

    Public Sub CopyToClipboard()

        Try
            Dim thread As New System.Threading.Thread(AddressOf Me.BackgroundProcess)
            thread.SetApartmentState(Threading.ApartmentState.STA)
            thread.Start()
            thread.Join()
        Catch ex As Exception
        End Try

    End Sub

    Private Sub BackgroundProcess()

        Try
            ''Clear the clipboard...
            Clipboard.Clear()

            If Code <> "" Then
                Clipboard.SetDataObject(Code, False)
            Else
                MessageBox.Show("There is no data to add to the clipboard...")
            End If
            ''Declares an IDataObject to hold the data returned from the clipboard.
            ''Retrieves the data from the clipboard.
            ''Determines whether the data is in a format you can use.
            If Clipboard.GetData(DataFormats.StringFormat) Then
                ''Yes it is, so display it...
                TextBox1.Text = Code
                SendKeys.SendWait("^{V}{ENTER}") ''paste to place that has focus and hit enter
            Else
                MessageBox.Show("No data in the clipboard...")
            End If
        Catch ex As Exception
        End Try

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        Me.Dispose()

    End Sub

End Class
