<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Basket.aspx.cs" Inherits="WebApplication18.Pages.Basket" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>
    </h2>

    <!--
    <table class="table">
  <thead>
    <tr>
      <th scope="col">#</th>
      <th scope="col">First</th>
      <th scope="col">Last</th>
      <th scope="col">Handle</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <th scope="row">1</th>
      <td>Mark</td>
      <td>Otto</td>
      <td>@mdo</td>
    </tr>
    <tr>
      <th scope="row">2</th>
      <td>Jacob</td>
      <td>Thornton</td>
      <td>@fat</td>
    </tr>
    <tr>
      <th scope="row">3</th>
      <td>Larry</td>
      <td>the Bird</td>
      <td>@twitter</td>
    </tr>
  </tbody>
</table>
        -->
    <div class="container">
        <div class ="row">
            <div class ="col-md-12">
                <h2 style ="margin-bottom: 30px">My Shopping Basket</h2>
            </div>
            <div class ="col-md-12">
                <table class ="table table-bordered text-center">
                    
                    <thead>
                        <!--table row!-->
                        <tr>
                            <!--table col!-->
                            <td style="width:80px">Image</td>
                            <td>Name </td>
                            <td>Price</td>
                            <td style="width:80px">Quantity</td>
                        </tr>

                    </thead>
                    <tbody>
                        <!--table row!-->
                        <tr>
                            <!--table col!-->
                            <td ><img src="../Images/NoImageAvailabe.jpg" height="60" /></td>
                            <td style="vertical-align:middle">DVD </td>
                            <td style="vertical-align:middle">$20</td>
                            <td style="vertical-align:middle">13</td>
                        </tr>

                    </tbody>
                </table>
            </div>
            <div class ="col-md-12">
                <div class="pull-right">
                    <a href="#" class="btn btn-success">Checkout</a>
                </div>
            </div>
            
        </div>
        

    </div>

 
    
</asp:Content>
