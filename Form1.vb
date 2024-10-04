Imports System.Data.SqlClient

Public Class Form1
    ' Declare controls for login
    Private txtUsername As TextBox
    Private txtPassword As TextBox
    Private btnLogin As Button

    ' Declare controls for the quiz
    Private labelQuestion As Label
    Private radioButton1 As RadioButton
    Private radioButton2 As RadioButton
    Private radioButton3 As RadioButton
    Private buttonSubmit As Button
    Private labelResult As Label

    ' Connection string for SQL database
    Private connectionString As String = "Server=your_server;Database=your_database;User Id=your_username;Password=your_password;"

    ' Constructor - Initializes components
    Public Sub New()
        InitializeComponent()
    End Sub

    ' This method is responsible for initializing the form components
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Initialize Login Controls
        InitializeLoginControls()
    End Sub

    ' Method to initialize login controls
    Private Sub InitializeLoginControls()
        txtUsername = New TextBox()
        txtUsername.Location = New System.Drawing.Point(100, 50)
        txtUsername.Size = New System.Drawing.Size(200, 20)
        Me.Controls.Add(txtUsername)

        txtPassword = New TextBox()
        txtPassword.Location = New System.Drawing.Point(100, 90)
        txtPassword.Size = New System.Drawing.Size(200, 20)
        txtPassword.PasswordChar = "*"c
        Me.Controls.Add(txtPassword)

        btnLogin = New Button()
        btnLogin.Text = "Login"
        btnLogin.Location = New System.Drawing.Point(100, 130)
        AddHandler btnLogin.Click, AddressOf BtnLogin_Click
        Me.Controls.Add(btnLogin)
    End Sub

    ' Event handler for login button click
    Private Sub BtnLogin_Click(sender As Object, e As EventArgs)
        Dim username As String = txtUsername.Text
        Dim password As String = txtPassword.Text

        Using connection As New SqlConnection(connectionString)
            Try
                connection.Open()
                Dim query As String = "SELECT COUNT(1) FROM Users WHERE Username=@Username AND Password=@Password"
                Dim cmd As New SqlCommand(query, connection)
                cmd.Parameters.AddWithValue("@Username", username)
                cmd.Parameters.AddWithValue("@Password", password)

                Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())

                If count = 1 Then
                    ' Hide login controls and load the quiz
                    txtUsername.Visible = False
                    txtPassword.Visible = False
                    btnLogin.Visible = False
                    LoadQuiz()
                Else
                    MessageBox.Show("Invalid login credentials.")
                End If
            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
    End Sub

    ' Method to load quiz questions from the database
    Private Sub LoadQuiz()
        Using connection As New SqlConnection(connectionString)
            Try
                connection.Open()
                Dim query As String = "SELECT QuestionText, OptionA, OptionB, OptionC FROM Questions WHERE QuestionID = 1" ' Example to fetch question
                Dim cmd As New SqlCommand(query, connection)
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        labelQuestion = New Label()
                        labelQuestion.Text = reader("QuestionText").ToString()
                        labelQuestion.Location = New System.Drawing.Point(50, 30)
                        labelQuestion.Size = New System.Drawing.Size(300, 40)
                        Me.Controls.Add(labelQuestion)

                        radioButton1 = New RadioButton()
                        radioButton1.Text = "A) " & reader("OptionA").ToString()
                        radioButton1.Location = New System.Drawing.Point(50, 80)
                        radioButton1.Size = New System.Drawing.Size(300, 20)
                        Me.Controls.Add(radioButton1)

                        radioButton2 = New RadioButton()
                        radioButton2.Text = "B) " & reader("OptionB").ToString()
                        radioButton2.Location = New System.Drawing.Point(50, 110)
                        radioButton2.Size = New System.Drawing.Size(300, 20)
                        Me.Controls.Add(radioButton2)

                        radioButton3 = New RadioButton()
                        radioButton3.Text = "C) " & reader("OptionC").ToString()
                        radioButton3.Location = New System.Drawing.Point(50, 140)
                        radioButton3.Size = New System.Drawing.Size(300, 20)
                        Me.Controls.Add(radioButton3)

                        buttonSubmit = New Button()
                        buttonSubmit.Text = "Submit"
                        buttonSubmit.Location = New System.Drawing.Point(50, 170)
                        AddHandler buttonSubmit.Click, AddressOf ButtonSubmit_Click
                        Me.Controls.Add(buttonSubmit)

                        labelResult = New Label()
                        labelResult.Location = New System.Drawing.Point(50, 210)
                        labelResult.Size = New System.Drawing.Size(300, 20)
                        labelResult.Visible = False
                        Me.Controls.Add(labelResult)
                    End If
                End Using
            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
    End Sub

    ' Event handler for the Submit button click
    Private Sub ButtonSubmit_Click(sender As Object, e As EventArgs)
        Dim correctOption As String = "A" ' Adjust according to your logic or fetched from the database
        If (radioButton1.Checked AndAlso correctOption = "A") OrElse
           (radioButton2.Checked AndAlso correctOption = "B") OrElse
           (radioButton3.Checked AndAlso correctOption = "C") Then
            labelResult.Text = "Correct! Your score: 1/1"
        Else
            labelResult.Text = "Incorrect. Your score: 0/1"
        End If
        labelResult.Visible = True
    End Sub
End Class
