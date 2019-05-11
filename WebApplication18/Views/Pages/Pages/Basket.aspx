<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Basket.aspx.cs" Inherits="WebApplication18.Views.Pages.Basket" %>
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
            <div id="AllProductsInBasket" class ="col-md-12">
                
            </div>
            <div id="checkout" class ="col-md-12"> 
            </div>      
        </div>
    </div>
        

      <script type="text/javascript">

        $(document).ready(function () {
            
               var mainDiv = document.getElementById('AllProductsInBasket');
               var checkoutDiv = document.getElementById('checkout');
               
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
                                console.log(response);
                           
                            var str = "<table class =\"table table-bordered text-center\">"
                                + "<thead>"
                                + "<tr>"
                                + "<td style = \"width:80px\" > Image</td>"
                                + "<td>Name </td>"
                                + "<td>Price</td>"
                                + "<td style=\"width:110px\">ID</td>"
                                + "<td style=\"width:110px\">Quantity</td>"
                                + "<td style=\"width:60px\">Delete</td>"
                                + "</tr>"
                                + "</thead>"
                                + "<tbody>";
                            var products = response.split(";");
                            for (i = 0; i < products.length-1; i++){
                                
                                    
                                var productfields = products[i].split(",");
                                var productName = productfields[0];
                                var price = productfields[1];
                                var id = productfields[2];
                                var quantity = +productfields[3];
                                    str += "<tr>" +
                                    "<td ><img src=\"../Images/NoImageAvailabe.jpg\"" + "height=\"60\" /></td><td style=\"vertical-align :middle\">" + productName + "</td><td style=\"vertical-align:middle\">" + price + "</td><td style=\"vertical-align:middle\">" + id + "</td><td type=\"text\" style=\"vertical-align:middle\">" + quantity + "</td><td style=\"vertical-align :middle\"><form><input type = \"button\" class=\"btn btn-danger\" value = \"Delete\" onclick=\"deleteRow("+id+");\"></form></td></tr>";
                            }
                            str += " </tbody>" + "</table>";
                            mainDiv.innerHTML = str;
                            checkoutDiv.innerHTML = "<div class=\"pull-right\"><input type=\"button\" class=\"btn btn-success\" value=\"Checkout\" onclick=\" checkout()\"></div>";
//<a href=\"#\" class=\"btn btn-success\">Checkout</a>

                        }
                        else {
                              mainDiv.innerHTML = " <h3 style =\"margin-bottom: 30px\">Basket is EMPTY</h3>";
                            //<form><input type=\"image\" src=\"../Images/trash.png\" name=\"Delete\" width=\"25\" height=\"25\" align=\"top\" alt=\"Stop sign\ onclick=\"deleteRow(this);\"></form>
                            //<img src=\"../Images/trash.png\"" + "height=\"27\" />
                            //                                    "<td ><img src=\"../Images/NoImageAvailabe.jpg\"" + "height=\"60\" /></td><td style=\"vertical-align :middle\">" + productName + "</td><td style=\"vertical-align:middle\">" + price + "</td><td style=\"vertical-align:middle\">" + id + "</td><td style=\"vertical-align:middle\">" + quantity + "</td><td style=\"vertical-align :middle\"><form><input type=\"image\" src=\"../Images/trash.png\" name=\"Delete\" width=\"25\" height=\"25\" align=\"top\" alt=\"Stop sign\ onclick=\"deleteRow();\"></form></td></tr>";

                        }
                    },
                    error: function (response) {
                        console.log(response);
                    }
                });
          });

          function deleteRow(id) {
              event.preventDefault();
              console.log(id);
              var getUrl = window.location;
              var baseUrl = getUrl.protocol + "//" + getUrl.host
               jQuery.ajax({
                    type: "GET",
                    url: baseUrl+"/api/user/removeProductFromCart?productId=" + id,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {

                        if (response == "ok") {

                            location.reload();
                        }
                        else {
                            console.log(response);   
                        }
                    },
                    error: function (response) {
                        console.log(response);
                    }
                });
          }

                function checkout() {
                                
                              event.preventDefault();
                              console.log("222");
                              var getUrl = window.location;
                              var baseUrl = getUrl.protocol + "//" + getUrl.host
                              window.location.href = baseUrl+"/Checkout/";
                          }
         /* $(".deleteRow").click(function() {
              console.log("11111");
            });*/
    </script>
</asp:Content>