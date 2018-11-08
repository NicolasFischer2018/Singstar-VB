<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Singstar_VB
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.SingstarDVD = New System.Windows.Forms.TextBox
        Me.Button2 = New System.Windows.Forms.Button
        Me.Songs_ListBox = New System.Windows.Forms.ListBox
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.Button5 = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.Button3 = New System.Windows.Forms.Button
        Me.TabPage3 = New System.Windows.Forms.TabPage
        Me.Label5 = New System.Windows.Forms.Label
        Me.Button6 = New System.Windows.Forms.Button
        Me.Library_ListBox = New System.Windows.Forms.ListBox
        Me.TabPage2 = New System.Windows.Forms.TabPage
        Me.Button1 = New System.Windows.Forms.Button
        Me.Label4 = New System.Windows.Forms.Label
        Me.Output_path = New System.Windows.Forms.TextBox
        Me.Button4 = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.Library_path = New System.Windows.Forms.TextBox
        Me.Logs = New System.Windows.Forms.ListBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.BMP2TX2 = New System.Diagnostics.Process
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.SuspendLayout()
        '
        'SingstarDVD
        '
        Me.SingstarDVD.Location = New System.Drawing.Point(3, 15)
        Me.SingstarDVD.Name = "SingstarDVD"
        Me.SingstarDVD.Size = New System.Drawing.Size(221, 20)
        Me.SingstarDVD.TabIndex = 1
        Me.SingstarDVD.Text = "Select Singstar DVD Drive"
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(243, 6)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(52, 29)
        Me.Button2.TabIndex = 2
        Me.Button2.Text = "Open"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Songs_ListBox
        '
        Me.Songs_ListBox.FormattingEnabled = True
        Me.Songs_ListBox.Location = New System.Drawing.Point(3, 40)
        Me.Songs_ListBox.Name = "Songs_ListBox"
        Me.Songs_ListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.Songs_ListBox.Size = New System.Drawing.Size(545, 121)
        Me.Songs_ListBox.TabIndex = 4
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Location = New System.Drawing.Point(0, 3)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(562, 237)
        Me.TabControl1.TabIndex = 5
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.Button5)
        Me.TabPage1.Controls.Add(Me.Label2)
        Me.TabPage1.Controls.Add(Me.Button3)
        Me.TabPage1.Controls.Add(Me.Button2)
        Me.TabPage1.Controls.Add(Me.Songs_ListBox)
        Me.TabPage1.Controls.Add(Me.SingstarDVD)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(554, 211)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Browse/Copy Singstar DVD"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'Button5
        '
        Me.Button5.Location = New System.Drawing.Point(124, 164)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(99, 40)
        Me.Button5.TabIndex = 8
        Me.Button5.Text = "Extract CORE Files"
        Me.Button5.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(380, 164)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(168, 20)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Number of Songs: 0"
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(3, 164)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(112, 41)
        Me.Button3.TabIndex = 5
        Me.Button3.Text = "Copy selected items to local library"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.Label5)
        Me.TabPage3.Controls.Add(Me.Button6)
        Me.TabPage3.Controls.Add(Me.Library_ListBox)
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Size = New System.Drawing.Size(554, 211)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Create Singstar DVD"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(376, 170)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(168, 20)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "Number of Songs: 0"
        '
        'Button6
        '
        Me.Button6.Location = New System.Drawing.Point(3, 170)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(131, 38)
        Me.Button6.TabIndex = 1
        Me.Button6.Text = "Create Singstar DVD"
        Me.Button6.UseVisualStyleBackColor = True
        '
        'Library_ListBox
        '
        Me.Library_ListBox.FormattingEnabled = True
        Me.Library_ListBox.Location = New System.Drawing.Point(3, 3)
        Me.Library_ListBox.Name = "Library_ListBox"
        Me.Library_ListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.Library_ListBox.Size = New System.Drawing.Size(541, 160)
        Me.Library_ListBox.TabIndex = 0
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.Button1)
        Me.TabPage2.Controls.Add(Me.Label4)
        Me.TabPage2.Controls.Add(Me.Output_path)
        Me.TabPage2.Controls.Add(Me.Button4)
        Me.TabPage2.Controls.Add(Me.Label1)
        Me.TabPage2.Controls.Add(Me.Library_path)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(554, 211)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Preferences"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(251, 54)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(52, 29)
        Me.Button1.TabIndex = 6
        Me.Button1.Text = "Select"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 50)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(64, 13)
        Me.Label4.TabIndex = 5
        Me.Label4.Text = "Output Path"
        '
        'Output_path
        '
        Me.Output_path.Location = New System.Drawing.Point(5, 63)
        Me.Output_path.Name = "Output_path"
        Me.Output_path.Size = New System.Drawing.Size(240, 20)
        Me.Output_path.TabIndex = 4
        '
        'Button4
        '
        Me.Button4.Location = New System.Drawing.Point(252, 9)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(52, 29)
        Me.Button4.TabIndex = 3
        Me.Button4.Text = "Select"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(7, 5)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(92, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Local Library Path"
        '
        'Library_path
        '
        Me.Library_path.Location = New System.Drawing.Point(6, 18)
        Me.Library_path.Name = "Library_path"
        Me.Library_path.Size = New System.Drawing.Size(240, 20)
        Me.Library_path.TabIndex = 0
        '
        'Logs
        '
        Me.Logs.FormattingEnabled = True
        Me.Logs.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Logs.Location = New System.Drawing.Point(4, 261)
        Me.Logs.Name = "Logs"
        Me.Logs.SelectionMode = System.Windows.Forms.SelectionMode.None
        Me.Logs.Size = New System.Drawing.Size(554, 121)
        Me.Logs.TabIndex = 6
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 244)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(30, 13)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "Logs"
        '
        'BMP2TX2
        '
        Me.BMP2TX2.StartInfo.CreateNoWindow = True
        Me.BMP2TX2.StartInfo.Domain = ""
        Me.BMP2TX2.StartInfo.LoadUserProfile = False
        Me.BMP2TX2.StartInfo.Password = Nothing
        Me.BMP2TX2.StartInfo.StandardErrorEncoding = Nothing
        Me.BMP2TX2.StartInfo.StandardOutputEncoding = Nothing
        Me.BMP2TX2.StartInfo.UserName = ""
        Me.BMP2TX2.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden
        Me.BMP2TX2.SynchronizingObject = Me
        '
        'Singstar_VB
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(567, 385)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Logs)
        Me.Controls.Add(Me.TabControl1)
        Me.Name = "Singstar_VB"
        Me.Text = "Singstar-VB"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.TabPage3.ResumeLayout(False)
        Me.TabPage3.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents SingstarDVD As System.Windows.Forms.TextBox
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Songs_ListBox As System.Windows.Forms.ListBox
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Library_path As System.Windows.Forms.TextBox
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Logs As System.Windows.Forms.ListBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Library_ListBox As System.Windows.Forms.ListBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Output_path As System.Windows.Forms.TextBox
    Friend WithEvents Button6 As System.Windows.Forms.Button
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents BMP2TX2 As System.Diagnostics.Process
    Friend WithEvents Label5 As System.Windows.Forms.Label

End Class
