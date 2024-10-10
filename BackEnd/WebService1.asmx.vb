x`Imports System.Data.SqlClient
Imports System.Web.Services

<System.Web.Services.WebService(Namespace:="http://tempuri.org/")>
Public Class WebService1
    Inherits System.Web.Services.WebService

    Public Class TaskItem
        Public Property Id As Integer
        Public Property Title As String
        Public Property Description As String
        Public Property Deadline As DateTime
        Public Property IsCompleted As Boolean
    End Class

    Private connectionString As String = "Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\USERS\IAS_A\ONEDRIVE\DESKTOP\FACULTATE\ANUL 3\SEMESTRUL 2\II\LABORATOR 5\TEMA 2 II\TEMA_II_1\APP_DATA\DATABASE1.MDF;Integrated Security=True;Connect Timeout=30"

    <WebMethod()>
    Public Function GetTasks() As List(Of TaskItem)
        Dim tasks As New List(Of TaskItem)()
        Using connection As New SqlConnection(connectionString)
            connection.Open()
            Dim command As New SqlCommand("SELECT * FROM TaskDatabase", connection)
            Dim reader As SqlDataReader = command.ExecuteReader()
            While reader.Read()
                tasks.Add(New TaskItem() With {
                    .Id = reader.GetInt32(0),
                    .Title = reader.GetString(1),
                    .Description = reader.GetString(2),
                    .Deadline = reader.GetDateTime(3),
                    .IsCompleted = reader.GetBoolean(4)
                })
            End While
        End Using
        Return tasks
    End Function
    <WebMethod()>
    Public Function AddTask(ByVal title As String, ByVal description As String, ByVal deadline As DateTime) As Integer
        Dim taskId As Integer = -1
        Using connection As New SqlConnection(connectionString)
            connection.Open()
            Dim command As New SqlCommand("INSERT INTO TaskDatabase (Title, Description, Deadline, IsCompleted) VALUES (@Title, @Description, @Deadline, 0); SELECT SCOPE_IDENTITY();", connection)
            command.Parameters.AddWithValue("@Title", title)
            command.Parameters.AddWithValue("@Description", description)
            command.Parameters.AddWithValue("@Deadline", deadline)
            taskId = Convert.ToInt32(command.ExecuteScalar())
        End Using
        Return taskId
    End Function


    <WebMethod()>
    Public Function UpdateTask(ByVal taskId As Integer, ByVal title As String, ByVal description As String, ByVal deadline As DateTime, ByVal isCompleted As Boolean) As Boolean
        Using connection As New SqlConnection(connectionString)
            connection.Open()
            Dim command As New SqlCommand("UPDATE TaskDatabase SET Title = @Title, Description = @Description, Deadline = @Deadline, IsCompleted = @IsCompleted WHERE Id = @Id", connection)
            command.Parameters.AddWithValue("@Title", title)
            command.Parameters.AddWithValue("@Description", description)
            command.Parameters.AddWithValue("@Deadline", deadline)
            command.Parameters.AddWithValue("@IsCompleted", isCompleted)
            command.Parameters.AddWithValue("@Id", taskId)
            Return command.ExecuteNonQuery() > 0
        End Using
    End Function

    <WebMethod()>
    Public Function DeleteTask(ByVal taskId As Integer) As Boolean
        Using connection As New SqlConnection(connectionString)
            connection.Open()

            'Șterge task-ul cu ID-ul specificat
            Dim deleteCommand As New SqlCommand("DELETE FROM TaskDatabase WHERE Id = @Id", connection)
            deleteCommand.Parameters.AddWithValue("@Id", taskId)
            deleteCommand.ExecuteNonQuery()

            'Reinițializează valorile ID
            Dim resetCommand As New SqlCommand("DBCC CHECKIDENT ('TaskDatabase', RESEED, 0)", connection)
            resetCommand.ExecuteNonQuery()

            Return True
        End Using
    End Function

End Class
