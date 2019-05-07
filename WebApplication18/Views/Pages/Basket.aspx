<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Basket.aspx.cs" Inherits="WebApplication18.Pages.WebForm3" %>
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
            <div id="AllproductsInBasket" class ="col-md-12">
                
            </div>
            <div class ="col-md-12">
                <div class="pull-right">
                    <a href="#" class="btn btn-success">Checkout</a>
                </div>
            </div>
            
        </div>
        

    </div>

      <script type="text/javascript">

        $(document).ready(function () {
            
               var mainDiv = document.getElementById('AllProductsInBasket');
               
               var getUrl = window.location;
               var baseUrl = getUrl.protocol + "//" + getUrl.host
               console.log(baseUrl);
               
                console.log("before jquery");
                jQuery.ajax({
                    type: "GET",
                    url: baseUrl + "/api/user/getShoppingBasket",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success:
                        function (response) {
                            console.log("response");
                        if (response != "") {
                           
                            var str = "<table class =\"table table-bordered text-center\">"
                                + " <thead>"
                                + "<tr>"
                                + "< td style = \"width:80px\" > Image</td>"
                                + "<td>Name </td>"
                                + "<td>Price</td>"
                                + "<td>Rank</td>"
                                + "<td style=\"width:80px\">Quantity</td>"
                                + "</tr>"
                                + "</thead>"
                                + "<tbody>";
                            for (i = 0; i < products.length; i++){
                                conosle.log("in for");
                                    var products = response.split(";");
                                    var productfields = products[i].split(",");
                                    var productName = productfields[0];
                                    var price = productfields[1];
                                    var quantity = productfields[2];
                                    str += "<tr>" +
                                    "<td ><img src=\"../Images/NoImageAvailabe.jpg\"" + "height=\"60\" /></td>" +
                                    + "<td style=\"vertical-align:middle\">DVD </td>" +
                                    + "<td style=\"vertical-align :middle\">" + productName + "</td>" +
                                    + "<td style=\"vertical-align:middle\">" + price + "</td>" +
                                    + "<td style=\"vertical-align:middle\">" + quantity + "</td>" +
                                    + "</tr>";
                            }
                            str += " </tbody>" + "</table>";
                            mainDiv.innerHTML = str;
                            window.location.href = baseUrl+"/";
                        }
                        else {
                            console.log(response);   
                        }
                    },
                    error: function (response) {
                        console.log(response);
                    }
                });
        });

    </script>
</asp:Content>
