Public Class FrmMMaster
    Private Sub FrmMMaster_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim l As Single = (MDIForm.ClientSize.Width - Me.Width) / 2
        Dim t As Single = ((MDIForm.ClientSize.Height - Me.Height) / 2) - 30
        Me.SetBounds(l, t, Me.Width, Me.Height)
        Me.MdiParent = MDIForm
    End Sub
End Class