Imports System
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.Web.Services

''' <summary>
''' Summary description for InventoryService
''' </summary>
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")>
Public Class WebService2
    Inherits System.Web.Services.WebService

    Public Class Product
        Public Property NrCurent As Integer
        Public Property CodProdus As String
        Public Property NumeProdus As String
        Public Property Cantitate As Integer
        Public Property Pret As Decimal
    End Class

    Private connectionString As String = "Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security=True;Connect Timeout=30"
    <WebMethod>
    Public Function GetProducts() As List(Of Product)
        Dim products As New List(Of Product)()
        Using connection As New SqlConnection(connectionString)
            connection.Open()
            Using command As New SqlCommand("SELECT * FROM Produs", connection)
                Using reader As SqlDataReader = command.ExecuteReader()
                    While reader.Read()
                        products.Add(New Product() With {
                                .NrCurent = reader.GetInt32(0),
                                .CodProdus = reader.GetString(1),
                                .NumeProdus = reader.GetString(2),
                                .Cantitate = reader.GetInt32(3),
                                .Pret = reader.GetDecimal(4)
                            })
                    End While
                End Using
            End Using
        End Using
        Return products
    End Function

    <WebMethod>
    Public Function AddProduct(cod As String, nume As String, cantitate As Integer, pret As Decimal) As Integer
        Dim nrCurent As Integer = -1
        Using connection As New SqlConnection(connectionString)
            connection.Open()
            Dim command As New SqlCommand("INSERT INTO Produs (CodProdus, NumeProdus, Cantitate, Pret) VALUES (@CodProdus, @NumeProdus, @Cantitate, @Pret); SELECT SCOPE_IDENTITY();", connection)
            command.Parameters.AddWithValue("@CodProdus", cod)
            command.Parameters.AddWithValue("@NumeProdus", nume)
            command.Parameters.AddWithValue("@Cantitate", cantitate)
            command.Parameters.AddWithValue("@Pret", pret)
            nrCurent = Convert.ToInt32(command.ExecuteScalar())
        End Using
        Return nrCurent
    End Function

    <WebMethod>
    Public Function UpdateProduct(nrCurent As Integer, cod As String, nume As String, cantitate As Integer, pret As Decimal) As Boolean
        Using connection As New SqlConnection(connectionString)
            connection.Open()
            Using command As New SqlCommand("UPDATE Produs SET CodProdus = @CodProdus, NumeProdus = @NumeProdus, Cantitate = @Cantitate, Pret = @Pret WHERE NrCurent = @NrCurent", connection)
                command.Parameters.AddWithValue("@CodProdus", cod)
                command.Parameters.AddWithValue("@NumeProdus", nume)
                command.Parameters.AddWithValue("@Cantitate", cantitate)
                command.Parameters.AddWithValue("@Pret", pret)
                command.Parameters.AddWithValue("@NrCurent", nrCurent)
                Return command.ExecuteNonQuery() > 0
            End Using
        End Using
    End Function

    <WebMethod>
    Public Function DeleteProduct(nrCurent As Integer) As Boolean
        Using connection As New SqlConnection(connectionString)
            connection.Open()
            Using deleteCommand As New SqlCommand("DELETE FROM Produs WHERE NrCurent = @NrCurent", connection)
                deleteCommand.Parameters.AddWithValue("@NrCurent", nrCurent)
                deleteCommand.ExecuteNonQuery()
            End Using
            Return True
        End Using
    End Function
End Class

